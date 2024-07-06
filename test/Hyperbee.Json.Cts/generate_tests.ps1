function Invoke-WebRequestWithRetry {
    param (
        [Parameter(Mandatory=$true)]
        [string]$Url,
        [int]$MaxRetries = 5,
        [int]$RetryDelay = 3 # seconds
    )

    Write-Host "Downloading $Url"

    $attempt = 0
    while ($attempt -lt $MaxRetries) {
        try {
            $response = Invoke-WebRequest -Uri $Url
            return $response
        }
        catch {
            $attempt++
            Write-Host "Attempt $attempt failed: $_"
            if ($attempt -ge $MaxRetries) {
                throw "Failed to retrieve the content after $MaxRetries attempts. Error: $_"
            }
            Start-Sleep -Seconds $RetryDelay
        }
    }

    Write-Host "Download complete."
}

function Get-JsonContent {
    param (
        [Parameter(Mandatory=$true)]
        [string]$Url,
        [string]$SavePath
    )

    # Fetch the JSON content as a string
    $response = Invoke-WebRequestWithRetry -Url $Url
    $jsonContent = $response.Content

    # Save the JSON content to a file in a pretty formatted way if SavePath is provided
    if ($PSBoundParameters.ContainsKey('SavePath')) {
        $prettyJson = $jsonContent | ConvertFrom-Json -AsHashtable | ConvertTo-Json -Depth 10
        Set-Content -Path $SavePath -Value $prettyJson
        Write-Output "JSON content saved to '$SavePath'."
    }

    # Convert the raw JSON string to a PowerShell hashtable to access properties
    $jsonObject = $jsonContent | ConvertFrom-Json -AsHashtable

    # Use regex to extract all selector properties
    $pattern = '"selector"\s*:\s*"(.*?[^\\])"'
    $matches = [regex]::Matches($jsonContent, $pattern)

    # Iterate through all tests and collect the properties
    $output = @()
    for ($i = 0; $i -lt $jsonObject.tests.Count; $i++) {
        $test = $jsonObject.tests[$i]

        $name = $test['name']

        # convert json to strings BEFORE adding to ps object to prevent unwanted conversions
        $document = ConvertTo-Json -InputObject $test['document'] -Depth 10 -Compress
        $result = if ($test.ContainsKey('result')) { ConvertTo-Json -InputObject $test['result'] -Depth 10 -Compress } else { $null }
        $results = if ($test.ContainsKey('results')) { ConvertTo-Json -InputObject $test['results'] -Depth 10 -Compress } else { $null }
        $invalid_selector = if ($test.ContainsKey('invalid_selector')) { $test['invalid_selector'] } else { $null }

        $rawJsonSelector = $matches[$i].Groups[1].Value

        $item = [PSCustomObject]@{
            name             = $name
            document         = $document
            result           = $result
            results          = $results
            selector         = $rawJsonSelector
            invalid_selector = $invalid_selector
        }

        $output += $item
    }

    return $output
}

# Helper function to convert test names to C# method names
function Convert-ToCSharpMethodName {
    param (
        [string]$name
    )
    return $name -replace '[^a-zA-Z0-9]', '_'
}

function Get-UnitTestContent {
    param (
        [Parameter(Mandatory=$true)]
        [array]$JsonTests
    )

    # Prepare the content for the C# unit test file
    $unitTestContent = @"
using System;
using System.Text.Json.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Cts
{
    [TestClass]
    public class CtsJsonTest
    {
        `r`n
"@

    $testNumber = 0

    # Loop through each test case in the JSON and generate a TestMethod
    foreach ($test in $JsonTests) {
        $name = $test.name
        $methodName = Convert-ToCSharpMethodName $name  # Convert $test.name to C# method name

        if($null -eq $name -or $name -eq "") {
            continue
        }

        $testNumber++
        $selector = $test.selector

        if ($selector.EndsWith('\')) {
            $selector += '\'
        }

        $invalidSelector = if ($test.invalid_selector) { $true } else { $false }

        $document = $test.document
        $result = $test.result
        $results = $test.results

        # Replace placeholders in the template with actual test case data
        $unitTestContent += @"
        `r`n
        [TestMethod( `"$name` ($testNumber)" )]
        public void Test`_$methodName`_$testNumber()
        {
            var selector = `"$selector`";`r`n
"@
        
        if ($invalidSelector) {
            $unitTestContent += @"
            var document = JsonNode.Parse( `"[0]`" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( `"Failed to throw exception`" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch (Exception e)
            {
                Assert.Fail( $`"Invalid exception of type {e.GetType().Name}`" );
            }

        }`r`n
"@
        } else {
            $unitTestContent += @"
            var document = JsonNode.Parse(
                `"`"`"$document`"`"`");
            var results = document.Select(selector);`r`n
"@

            if ($null -ne $result) {
                $unitTestContent += @"
            var expect = JsonNode.Parse(
                `"`"`"$result`"`"`");

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }`r`n
"@
            } elseif ($null -ne $results) {
                $unitTestContent += @"
            var expectOneOf = JsonNode.Parse(
                `"`"`"$results`"`"`");

            var match = TestHelper.MatchAny(results, expectOneOf!);
            Assert.IsTrue(match);
        }`r`n
"@
            } else {
                $unitTestContent += @"
            Assert.Fail(`"missing results`");
        }`r`n
"@
            }
        }
    }

    # Close the class and namespace
    $unitTestContent += @"
    }
}`r`n
"@

    return $unitTestContent
}

# Generate unit-tests
$jsonUrl = "https://raw.githubusercontent.com/Stillpoint-Software/jsonpath-compliance-test-suite/main/cts.json"
$savePath = Join-Path -Path $PSScriptRoot -ChildPath "CtsJsonTest.json"
$jsonContent = Get-JsonContent -Url $jsonUrl -SavePath $savePath

$unitTestContent = Get-UnitTestContent -JsonTests $jsonContent

# Save the generated unit-test file
$unitTestPath = Join-Path -Path $PSScriptRoot -ChildPath "CtsJsonTest.cs"
Set-Content -Path $unitTestPath -Value $unitTestContent

Write-Output "C# unit test file 'CtsJsonTest.cs' generated successfully at '$unitTestPath'."