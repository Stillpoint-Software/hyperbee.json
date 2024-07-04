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
        [string]$Url
    )

    # Fetch the JSON content as a string
    $response = Invoke-WebRequestWithRetry -Url $Url
    $jsonContent = $response.Content

    # Convert the raw JSON string to a PowerShell hashtable to access properties
    $jsonObject = $jsonContent | ConvertFrom-Json -AsHashtable

    # Use regex to extract all selector properties without unescaping content
    $pattern = '"selector"\s*:\s*"(.*?)"'
    $matches = [regex]::Matches($jsonContent, $pattern)

    # Iterate through all tests and collect the properties
    $results = @()
    for ($i = 0; $i -lt $jsonObject.tests.Count; $i++) {
        $test = $jsonObject.tests[$i]

        $name = $test['name']
        $document = $test['document']
        $result = if ($test.ContainsKey('result')) { $test['result'] } else { $null }
        $resultsProperty = if ($test.ContainsKey('results')) { $test['results'] } else { $null }
        $invalid_selector = if ($test.ContainsKey('invalid_selector')) { $test['invalid_selector'] } else { $null }

        $rawJsonSelector = $matches[$i].Groups[1].Value

        $output = [PSCustomObject]@{
            name             = $name
            document         = $document
            result           = $result
            results          = $resultsProperty
            selector         = $rawJsonSelector
            invalid_selector = $invalid_selector
        }

        $results += $output
    }

    return $results
}

# Helper function to convert test names to C# method names
function Convert-ToCSharpMethodName {
    param (
        [string]$name
    )
    return $name -replace '[^a-zA-Z0-9]', '_'
}

function Format-StringInBrackets {
    param (
        [string]$Value
    )

    if (-not [string]::IsNullOrWhiteSpace($Value) -and 
        -not ($Value.TrimStart() -match '^\[' -and $Value.TrimEnd() -match '\]$') -and
        -not ($Value.TrimStart() -match '^\{' -and $Value.TrimEnd() -match '\}$')) {
        
        return "[$Value]"
    } else {
        return $Value
    }
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
    {`r`n
"@

    $testNumber = 0

    # Loop through each test case in the JSON and generate a TestMethod
    foreach ($test in $JsonTests) {
        $testNumber++
        $name = $test.name
        $methodName = Convert-ToCSharpMethodName $name  # Convert $test.name to C# method name

        $selector = $test.selector
        $invalidSelector = if ($test.invalid_selector) { $true } else { $false }

        $document = if ($test.document) { (ConvertTo-Json -InputObject $test.document -Depth 4 -Compress) } else { "null" }
        $result = if ($test.result) { (ConvertTo-Json -InputObject $test.result -Depth 4 -Compress) } else { "null" }
        $results = if ($test.results) { (ConvertTo-Json -InputObject $test.results -Depth 4 -Compress) } else { "null" }

        # if result is not a complex object (it is a simple string), wrap it as an array
        $result = Format-StringInBrackets -Value $result

        # Replace placeholders in the template with actual test case data
        $unitTestContent += @"
        [TestMethod]
        public void Test_$methodName`_Number$testNumber()
        {
            var selector = @`"$selector`";`r`n
"@
        
        if ($invalidSelector) {
            $unitTestContent += @"
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));`r`n
        }`r`n
"@
        } else {
            $unitTestContent += @"
            var document = JsonNode.Parse(
                `"`"`"$document`"`"`");
            var results = document.Select(selector);`r`n
"@

            if ($result -ne "null") {
                $unitTestContent += @"
            var expected = JsonNode.Parse(
                `"`"`"$result`"`"`");

            var count = 0;
            foreach (var result in results)
            {
                Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
                count++;
            }
        }`r`n
"@
            } elseif ($results -ne "null") {
                $unitTestContent += @"
            var expectedResults = JsonNode.Parse(
                `"`"`"$results`"`"`");

            var count = 0;
            foreach (var result in results)
            {
                Assert.IsTrue(JsonNode.DeepEquals(expectedResults![0]![count], result));
                count++;
            }
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

# Example usage:
$jsonUrl = "https://raw.githubusercontent.com/Stillpoint-Software/jsonpath-compliance-test-suite/main/cts.json"
$jsonContent = Get-JsonContent -Url $jsonUrl

$unitTestContent = Get-UnitTestContent -JsonTests $jsonContent

# Save the generated C# unit test file
Set-Content -Path "CtsJsonTest.cs" -Value $unitTestContent

Write-Output "C# unit test file 'CtsJsonTest.cs' generated successfully."
