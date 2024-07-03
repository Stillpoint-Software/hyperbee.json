function ConvertToCSharpMethodName {
    param(
        [string]$name
    )

    # Split by non-word characters, capitalize each part, and join them
    $nameParts = $name -split '\W+'
    $pascalCaseName = ($nameParts | ForEach-Object { 
        if ($_.Length -gt 0) {
            $_.Substring(0,1).ToUpper() + $_.Substring(1).ToLower()
        }
    }) -join ''

    return $pascalCaseName
}

function EscapePowershell($text) {
    return $text  -replace "`"", '\"' -replace "`t", '\t' -replace "`r", '\r' -replace "`n", '\n'
}

# Define the URL of the JSON file
$jsonUrl = "https://github.com/Stillpoint-Software/jsonpath-compliance-test-suite/raw/main/cts.json"

# Download the JSON file
$jsonContent = Invoke-WebRequest -Uri $jsonUrl | Select-Object -ExpandProperty Content | ConvertFrom-Json -AsHashTable




# Use regex to extract all selector properties
# $pattern = '"selector"\s*:\s*"(.*?)"'
# $matches = [regex]::Matches($jsonContent, $pattern)

# Iterate through all tests and output the properties
# for ($i = 0; $i -lt $jsonObject.tests.Count; $i++) {
#     $test = $jsonObject.tests[$i]
# 
#     $name = $test.name
#     $document = $test.document
#     $result = $test.result -replace 'results', 'result'
#     $invalid_selector = if ($test.PSObject.Properties.Match('invalid_selector').Count -gt 0) { $test.invalid_selector } else { $null }
# 
#     $rawJsonSelector = $matches[$i].Groups[1].Value
# 
#     $output = @{
#         name             = $name
#         document         = $document
#         result           = $result
#         selector         = $rawJsonSelector
#         invalid_selector = $invalid_selector
#     }

# }




# Prepare the content for the C# unit test file
$unitTestContent = @"
using System;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Cts;

[TestClass]
public class CtsJsonTest
{

"@

$testNumber = 0

# Loop through each test case in the JSON and generate a TestMethod
foreach ($test in $jsonContent.tests) {

    $testNumber++
    $name = $test.name
    $methodName = ConvertToCSharpMethodName $name  # Convert $test.name to C# method name

    $selector = $test.selector
    $escapedSelector = EscapePowershell $selector

    $invalidSelector = if ($test.invalid_selector) { $true } else { $false }

    $document = if ($test.document) { ConvertTo-Json -InputObject $test.document -Depth 4 -Compress } else { "null" }

    # Check if the result or results property exists and is not empty
    $result = $null
    if ([bool]($test.Keys -match "result")) {
        Write-Output "result"
        $result = if ($test.result) { ConvertTo-Json -InputObject $test.result -Depth 4 -Compress } else { $null }
    }

    $results = $null
    if ([bool]($test.Keys -match "results")) {
        Write-Output "results"
        $results = if ($test.results) { ConvertTo-Json -InputObject $test.results -Depth 4 -Compress } else { $null }
    }

    # Replace placeholders in the template with actual test case data
    $unitTestContent += @"

    [TestMethod("$name")]
    public void Test_$methodName`_Number$testNumber()
    {
"@

    if ($invalidSelector) {
        $unitTestContent += @"
        
        var selector = `"$escapedSelector`";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

"@
    } else {
        $unitTestContent += @"

        var selector = `"$escapedSelector`";
        var document = JsonNode.Parse( 
"""
$document
""" );
        var results = document.Select(selector);
"@

        if ($result -ne $null) {
            $unitTestContent += @"

        var expected = JsonNode.Parse(
"""
$result
""" );

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

"@
        } elseif ($results -ne  $null) {
            $unitTestContent += @"

        var expectedResults = JsonNode.Parse(
"""
$results
""" );

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expectedResults![0]![count], result));
            count++;
        }
    }

"@
        } else {
            $unitTestContent += @"

        Assert.Fail("missing results");
    }

"@
        }
    }
}


# Close the class and namespace
$unitTestContent += @"

}
"@


#Write-Output $unitTestContent

# Save the generated C# unit test file
Set-Content -Path "CtsJsonTest.cs" -Value $unitTestContent -Encoding UTF8

Write-Output "C# unit test file 'CtsJsonTest.cs' generated successfully."