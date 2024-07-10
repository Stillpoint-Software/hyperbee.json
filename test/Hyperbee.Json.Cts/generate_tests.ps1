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
        Write-Host "JSON content saved to '$SavePath'."
    }

    # Convert the raw JSON string to a PowerShell hashtable to access properties
    $jsonObject = $jsonContent | ConvertFrom-Json -AsHashtable

    # Use regex to extract all selector properties
    $pattern = '"selector"\s*:\s*"(.*?[^\\])"'
    $match = [regex]::Matches($jsonContent, $pattern)

    # Iterate through all tests and collect the properties
    $output = @()
    for ($i = 0; $i -lt $jsonObject.tests.Count; $i++) {
        $test = $jsonObject.tests[$i]

        # Split the name into category (group) and name parts
        $fullName = $test['name']
        $splitName = $fullName -split ',', 2
        $group = $splitName[0].Trim()
        $name = if ($splitName.Length -gt 1) { $splitName[1].Trim() } else { $null }

        # Ignore empty groups
        if ([string]::IsNullOrWhiteSpace($group)) {
            continue
        }

        # Convert JSON to strings BEFORE adding to PSObject to prevent unwanted conversions
        $document = ConvertTo-Json -InputObject $test['document'] -Depth 10 
        $result = if ($test.ContainsKey('result')) { ConvertTo-Json -InputObject $test['result'] -Depth 10  } else { $null }
        $results = if ($test.ContainsKey('results')) { ConvertTo-Json -InputObject $test['results'] -Depth 10  } else { $null }
        $invalid_selector = if ($test.ContainsKey('invalid_selector')) { $test['invalid_selector'] } else { $null }

        $rawJsonSelector = $match[$i].Groups[1].Value

        $item = [PSCustomObject]@{
            name             = $name
            group            = $group
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

function FormatJson {
    param (
        [string]$json,
        [int]$indent
    )

    # Ignore empty groups
    if ([string]::IsNullOrWhiteSpace($json)) {
        return $null
    }

    # Detect the line break format
    $lineBreak = if ($json -contains "`r`n") { "`r`n" } else { "`n" }

    # Split the JSON string into lines
    $lines = $json -split $lineBreak

    # Create the indentation string
    $indentation = " " * $indent

    # Add indentation to each line except the first
    $formattedLines = $lines | ForEach-Object { $indentation + $_ }

    # Join the lines back into a single string with the detected line break format
    $formattedJson = $lineBreak + ($formattedLines -join $lineBreak) + $lineBreak + $indentation

    return $formattedJson
}

function Convert-ToPascalCase {
    param (
        [string]$value
    )

    if ([string]::IsNullOrWhiteSpace($value)) {
        return $value
    }

    $words = $value -split '\s+'  # Split the string by whitespace
    $pascalCaseWords = $words | ForEach-Object {
        if ([string]::IsNullOrEmpty($_)) {
            continue
        }

        $firstLetter = $_[0].ToString().ToUpper()
        $restOfString = $_.Substring(1).ToLower()
        $firstLetter + $restOfString
    }

    return $pascalCaseWords -join ''
}

function Get-UnitTestContent {
    param (
        [Parameter(Mandatory=$true)]
        [array]$JsonTests,
        [Parameter(Mandatory=$true)]
        [string]$group
    )

    # Give the class a unique name
    $uniquePart = Convert-ToPascalCase -value $group
    $className = "Cts$($uniquePart)Test"

    # Prepare the content for the C# unit test file
    $unitTestContent = @"
// This file was auto generated.

using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Cts.Tests
{
    [TestClass]
    public class $className
    {`r`n
"@

    $testNumber = 0

    # Loop through each test case in the JSON and generate a TestMethod
    foreach ($test in $JsonTests) {
        $name = $test.name
        $methodName = Convert-ToCSharpMethodName $name  # Convert $test.name to C# method name

        if ($null -eq $name -or $name -eq "") {
            continue
        }

        $testNumber++
        $selector = $test.selector

        if ($selector.EndsWith('\')) {
            $selector += '\'
        }

        $invalidSelector = if ($test.invalid_selector) { $true } else { $false }

        $document = FormatJson -json $test.document -indent 16
        $result = FormatJson -json $test.result -indent 16
        $results = FormatJson -json $test.results -indent 16

        # Replace placeholders in the template with actual test case data
        $unitTestContent += @"
        
        [TestMethod( `"$name` ($testNumber)" )]
        public void Test`_$methodName`_$testNumber()
        {
            var selector = `"$selector`";`r`n
"@
        
        if ($invalidSelector) {
            $unitTestContent += @"
            var document = JsonNode.Parse( `"[0]`" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
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

# Generate unit-tests by category
$ctsPath = Join-Path -Path $PSScriptRoot -ChildPath "cts.json"

if (-not (Test-Path -Path $ctsPath)) {
    $jsonUrl = "https://raw.githubusercontent.com/Stillpoint-Software/jsonpath-compliance-test-suite/main/cts.json"
    $jsonContent = Get-JsonContent -Url $jsonUrl -SavePath $ctsPath
}

# Group tests by category
$groupedTests = $jsonContent | Group-Object -Property { $_.group }

# Ensure the Tests subfolder exists
$testsFolderPath = Join-Path -Path $PSScriptRoot -ChildPath "Tests"
if (-not (Test-Path -Path $testsFolderPath)) {
    New-Item -Path $testsFolderPath -ItemType Directory | Out-Null
}

foreach ($group in $groupedTests) {
    $category = $group.Name
    $categoryTests = $group.Group

    $unitTestContent = Get-UnitTestContent -JsonTests $categoryTests -group $category

    # Replace spaces with hyphens in the category for the filename
    $sanitizedCategory = $category -replace ' ', '-'
    $unitTestPath = Join-Path -Path $testsFolderPath -ChildPath ("cts-" + $sanitizedCategory + "-tests.cs")
    Set-Content -Path $unitTestPath -Value $unitTestContent

    Write-Host "C# unit test file 'cts-$sanitizedCategory-tests.cs' generated successfully at '$unitTestPath'."
}
