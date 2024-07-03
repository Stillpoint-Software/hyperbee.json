using System;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Cts;

[TestClass]
public class CtsJsonTest
{

    [TestMethod("basic, root")]
    public void Test_BasicRoot_Number1()
    {
        var selector = "$";
        var document = JsonNode.Parse( 
"""
["first","second"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[["first","second"]]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, no leading whitespace")]
    public void Test_BasicNoLeadingWhitespace_Number2()
    {        
        var selector = " $";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("basic, no trailing whitespace")]
    public void Test_BasicNoTrailingWhitespace_Number3()
    {        
        var selector = "$ ";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("basic, name shorthand")]
    public void Test_BasicNameShorthand_Number4()
    {
        var selector = "$.a";
        var document = JsonNode.Parse( 
"""
{"a":"A","b":"B"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, name shorthand, extended unicode ☺")]
    public void Test_BasicNameShorthandExtendedUnicode_Number5()
    {
        var selector = "$.☺";
        var document = JsonNode.Parse( 
"""
{"☺":"A","b":"B"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, name shorthand, underscore")]
    public void Test_BasicNameShorthandUnderscore_Number6()
    {
        var selector = "$._";
        var document = JsonNode.Parse( 
"""
{"_":"A","_foo":"B"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, name shorthand, symbol")]
    public void Test_BasicNameShorthandSymbol_Number7()
    {        
        var selector = "$.&";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("basic, name shorthand, number")]
    public void Test_BasicNameShorthandNumber_Number8()
    {        
        var selector = "$.1";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("basic, name shorthand, absent data")]
    public void Test_BasicNameShorthandAbsentData_Number9()
    {
        var selector = "$.c";
        var document = JsonNode.Parse( 
"""
{"a":"A","b":"B"}
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("basic, name shorthand, array data")]
    public void Test_BasicNameShorthandArrayData_Number10()
    {
        var selector = "$.a";
        var document = JsonNode.Parse( 
"""
["first","second"]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("basic, wildcard shorthand, object data")]
    public void Test_BasicWildcardShorthandObjectData_Number11()
    {
        var selector = "$.*";
        var document = JsonNode.Parse( 
"""
{"a":"A","b":"B"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[["A","B"],["B","A"]]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![0]![count], result));
            count++;
        }
    }

    [TestMethod("basic, wildcard shorthand, array data")]
    public void Test_BasicWildcardShorthandArrayData_Number12()
    {
        var selector = "$.*";
        var document = JsonNode.Parse( 
"""
["first","second"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["first","second"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, wildcard selector, array data")]
    public void Test_BasicWildcardSelectorArrayData_Number13()
    {
        var selector = "$[*]";
        var document = JsonNode.Parse( 
"""
["first","second"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["first","second"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, wildcard shorthand, then name shorthand")]
    public void Test_BasicWildcardShorthandThenNameShorthand_Number14()
    {
        var selector = "$.*.a";
        var document = JsonNode.Parse( 
"""
{"x":{"a":"Ax","b":"Bx"},"y":{"a":"Ay","b":"By"}}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[["Ax","Ay"],["Ay","Ax"]]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![0]![count], result));
            count++;
        }
    }

    [TestMethod("basic, multiple selectors")]
    public void Test_BasicMultipleSelectors_Number15()
    {
        var selector = "$[0,2]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[0,2]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, multiple selectors, space instead of comma")]
    public void Test_BasicMultipleSelectorsSpaceInsteadOfComma_Number16()
    {        
        var selector = "$[0 2]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("basic, multiple selectors, name and index, array data")]
    public void Test_BasicMultipleSelectorsNameAndIndexArrayData_Number17()
    {
        var selector = "$['a',1]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[1]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, multiple selectors, name and index, object data")]
    public void Test_BasicMultipleSelectorsNameAndIndexObjectData_Number18()
    {
        var selector = "$['a',1]";
        var document = JsonNode.Parse( 
"""
{"a":1,"b":2}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[1]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, multiple selectors, index and slice")]
    public void Test_BasicMultipleSelectorsIndexAndSlice_Number19()
    {
        var selector = "$[1,5:7]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[1,5,6]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, multiple selectors, index and slice, overlapping")]
    public void Test_BasicMultipleSelectorsIndexAndSliceOverlapping_Number20()
    {
        var selector = "$[1,0:3]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[1,0,1,2]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, multiple selectors, duplicate index")]
    public void Test_BasicMultipleSelectorsDuplicateIndex_Number21()
    {
        var selector = "$[1,1]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[1,1]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, multiple selectors, wildcard and index")]
    public void Test_BasicMultipleSelectorsWildcardAndIndex_Number22()
    {
        var selector = "$[*,1]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[0,1,2,3,4,5,6,7,8,9,1]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, multiple selectors, wildcard and name")]
    public void Test_BasicMultipleSelectorsWildcardAndName_Number23()
    {
        var selector = "$[*,'a']";
        var document = JsonNode.Parse( 
"""
{"a":"A","b":"B"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[["A","B","A"],["B","A","A"]]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![0]![count], result));
            count++;
        }
    }

    [TestMethod("basic, multiple selectors, wildcard and slice")]
    public void Test_BasicMultipleSelectorsWildcardAndSlice_Number24()
    {
        var selector = "$[*,0:2]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[0,1,2,3,4,5,6,7,8,9,0,1]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, multiple selectors, multiple wildcards")]
    public void Test_BasicMultipleSelectorsMultipleWildcards_Number25()
    {
        var selector = "$[*,*]";
        var document = JsonNode.Parse( 
"""
[0,1,2]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[0,1,2,0,1,2]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, empty segment")]
    public void Test_BasicEmptySegment_Number26()
    {        
        var selector = "$[]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("basic, descendant segment, index")]
    public void Test_BasicDescendantSegmentIndex_Number27()
    {
        var selector = "$..[1]";
        var document = JsonNode.Parse( 
"""
{"o":[0,1,[2,3]]}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[1,3]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, descendant segment, name shorthand")]
    public void Test_BasicDescendantSegmentNameShorthand_Number28()
    {
        var selector = "$..a";
        var document = JsonNode.Parse( 
"""
{"o":[{"a":"b"},{"a":"c"}]}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["b","c"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, descendant segment, wildcard shorthand, array data")]
    public void Test_BasicDescendantSegmentWildcardShorthandArrayData_Number29()
    {
        var selector = "$..*";
        var document = JsonNode.Parse( 
"""
[0,1]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[0,1]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, descendant segment, wildcard selector, array data")]
    public void Test_BasicDescendantSegmentWildcardSelectorArrayData_Number30()
    {
        var selector = "$..[*]";
        var document = JsonNode.Parse( 
"""
[0,1]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[0,1]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, descendant segment, wildcard selector, nested arrays")]
    public void Test_BasicDescendantSegmentWildcardSelectorNestedArrays_Number31()
    {
        var selector = "$..[*]";
        var document = JsonNode.Parse( 
"""
[[[1]],[2]]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[[[[1]],[2],[1],1,2],[[[1]],[2],[1],2,1]]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![0]![count], result));
            count++;
        }
    }

    [TestMethod("basic, descendant segment, wildcard selector, nested objects")]
    public void Test_BasicDescendantSegmentWildcardSelectorNestedObjects_Number32()
    {
        var selector = "$..[*]";
        var document = JsonNode.Parse( 
"""
{"a":{"c":{"e":1}},"b":{"d":2}}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[[{"c":{"e":1}},{"d":2},{"e":1},1,2],[{"c":{"e":1}},{"d":2},{"e":1},2,1],[{"c":{"e":1}},{"d":2},2,{"e":1},1],[{"d":2},{"c":{"e":1}},{"e":1},1,2],[{"d":2},{"c":{"e":1}},{"e":1},2,1],[{"d":2},{"c":{"e":1}},2,{"e":1},1]]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![0]![count], result));
            count++;
        }
    }

    [TestMethod("basic, descendant segment, wildcard shorthand, object data")]
    public void Test_BasicDescendantSegmentWildcardShorthandObjectData_Number33()
    {
        var selector = "$..*";
        var document = JsonNode.Parse( 
"""
{"a":"b"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["b"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, descendant segment, wildcard shorthand, nested data")]
    public void Test_BasicDescendantSegmentWildcardShorthandNestedData_Number34()
    {
        var selector = "$..*";
        var document = JsonNode.Parse( 
"""
{"o":[{"a":"b"}]}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[[{"a":"b"}],{"a":"b"},"b"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, descendant segment, multiple selectors")]
    public void Test_BasicDescendantSegmentMultipleSelectors_Number35()
    {
        var selector = "$..['a','d']";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["b","e","c","f"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("basic, descendant segment, object traversal, multiple selectors")]
    public void Test_BasicDescendantSegmentObjectTraversalMultipleSelectors_Number36()
    {
        var selector = "$..['a','d']";
        var document = JsonNode.Parse( 
"""
{"x":{"a":"b","d":"e"},"y":{"a":"c","d":"f"}}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[["b","e","c","f"],["c","f","b","e"]]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![0]![count], result));
            count++;
        }
    }

    [TestMethod("basic, bald descendant segment")]
    public void Test_BasicBaldDescendantSegment_Number37()
    {        
        var selector = "$..";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, existence, without segments")]
    public void Test_FilterExistenceWithoutSegments_Number38()
    {
        var selector = "$[?@]";
        var document = JsonNode.Parse( 
"""
{"a":1,"b":null}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[[1,null],[null,1]]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![0]![count], result));
            count++;
        }
    }

    [TestMethod("filter, existence")]
    public void Test_FilterExistence_Number39()
    {
        var selector = "$[?@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, existence, present with null")]
    public void Test_FilterExistencePresentWithNull_Number40()
    {
        var selector = "$[?@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":null,"d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":null,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals string, single quotes")]
    public void Test_FilterEqualsStringSingleQuotes_Number41()
    {
        var selector = "$[?@.a=='b']";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals numeric string, single quotes")]
    public void Test_FilterEqualsNumericStringSingleQuotes_Number42()
    {
        var selector = "$[?@.a=='1']";
        var document = JsonNode.Parse( 
"""
[{"a":"1","d":"e"},{"a":1,"d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"1","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals string, double quotes")]
    public void Test_FilterEqualsStringDoubleQuotes_Number43()
    {
        var selector = "$[?@.a==\"b\"]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals numeric string, double quotes")]
    public void Test_FilterEqualsNumericStringDoubleQuotes_Number44()
    {
        var selector = "$[?@.a==\"1\"]";
        var document = JsonNode.Parse( 
"""
[{"a":"1","d":"e"},{"a":1,"d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"1","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals number")]
    public void Test_FilterEqualsNumber_Number45()
    {
        var selector = "$[?@.a==1]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"d":"e"},{"a":"c","d":"f"},{"a":2,"d":"f"},{"a":"1","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals null")]
    public void Test_FilterEqualsNull_Number46()
    {
        var selector = "$[?@.a==null]";
        var document = JsonNode.Parse( 
"""
[{"a":null,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":null,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals null, absent from data")]
    public void Test_FilterEqualsNullAbsentFromData_Number47()
    {
        var selector = "$[?@.a==null]";
        var document = JsonNode.Parse( 
"""
[{"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("filter, equals true")]
    public void Test_FilterEqualsTrue_Number48()
    {
        var selector = "$[?@.a==true]";
        var document = JsonNode.Parse( 
"""
[{"a":true,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":true,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals false")]
    public void Test_FilterEqualsFalse_Number49()
    {
        var selector = "$[?@.a==false]";
        var document = JsonNode.Parse( 
"""
[{"a":false,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":false,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals self")]
    public void Test_FilterEqualsSelf_Number50()
    {
        var selector = "$[?@==@]";
        var document = JsonNode.Parse( 
"""
[1,null,true,{"a":"b"},[false]]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[1,null,true,{"a":"b"},[false]]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, deep equality, arrays")]
    public void Test_FilterDeepEqualityArrays_Number51()
    {
        var selector = "$[?@.a==@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":false,"b":[1,2]},{"a":[[1,[2]]],"b":[[1,[2]]]},{"a":[[1,[2]]],"b":[[[2],1]]},{"a":[[1,[2]]],"b":[[1,2]]}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":[[1,[2]]],"b":[[1,[2]]]}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, deep equality, objects")]
    public void Test_FilterDeepEqualityObjects_Number52()
    {
        var selector = "$[?@.a==@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":false,"b":{"x":1,"y":{"z":1}}},{"a":{"x":1,"y":{"z":1}},"b":{"x":1,"y":{"z":1}}},{"a":{"x":1,"y":{"z":1}},"b":{"y":{"z":1},"x":1}},{"a":{"x":1,"y":{"z":1}},"b":{"x":1}},{"a":{"x":1,"y":{"z":1}},"b":{"x":1,"y":{"z":2}}}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":{"x":1,"y":{"z":1}},"b":{"x":1,"y":{"z":1}}},{"a":{"x":1,"y":{"z":1}},"b":{"y":{"z":1},"x":1}}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, not-equals string, single quotes")]
    public void Test_FilterNotEqualsStringSingleQuotes_Number53()
    {
        var selector = "$[?@.a!='b']";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"c","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, not-equals numeric string, single quotes")]
    public void Test_FilterNotEqualsNumericStringSingleQuotes_Number54()
    {
        var selector = "$[?@.a!='1']";
        var document = JsonNode.Parse( 
"""
[{"a":"1","d":"e"},{"a":1,"d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, not-equals string, single quotes, different type")]
    public void Test_FilterNotEqualsStringSingleQuotesDifferentType_Number55()
    {
        var selector = "$[?@.a!='b']";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"a":1,"d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, not-equals string, double quotes")]
    public void Test_FilterNotEqualsStringDoubleQuotes_Number56()
    {
        var selector = "$[?@.a!=\"b\"]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"c","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, not-equals numeric string, double quotes")]
    public void Test_FilterNotEqualsNumericStringDoubleQuotes_Number57()
    {
        var selector = "$[?@.a!=\"1\"]";
        var document = JsonNode.Parse( 
"""
[{"a":"1","d":"e"},{"a":1,"d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, not-equals string, double quotes, different types")]
    public void Test_FilterNotEqualsStringDoubleQuotesDifferentTypes_Number58()
    {
        var selector = "$[?@.a!=\"b\"]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"a":1,"d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, not-equals number")]
    public void Test_FilterNotEqualsNumber_Number59()
    {
        var selector = "$[?@.a!=1]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"d":"e"},{"a":2,"d":"f"},{"a":"1","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":2,"d":"f"},{"a":"1","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, not-equals number, different types")]
    public void Test_FilterNotEqualsNumberDifferentTypes_Number60()
    {
        var selector = "$[?@.a!=1]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"c","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, not-equals null")]
    public void Test_FilterNotEqualsNull_Number61()
    {
        var selector = "$[?@.a!=null]";
        var document = JsonNode.Parse( 
"""
[{"a":null,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"c","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, not-equals null, absent from data")]
    public void Test_FilterNotEqualsNullAbsentFromData_Number62()
    {
        var selector = "$[?@.a!=null]";
        var document = JsonNode.Parse( 
"""
[{"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"d":"e"},{"a":"c","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, not-equals true")]
    public void Test_FilterNotEqualsTrue_Number63()
    {
        var selector = "$[?@.a!=true]";
        var document = JsonNode.Parse( 
"""
[{"a":true,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"c","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, not-equals false")]
    public void Test_FilterNotEqualsFalse_Number64()
    {
        var selector = "$[?@.a!=false]";
        var document = JsonNode.Parse( 
"""
[{"a":false,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"c","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, less than string, single quotes")]
    public void Test_FilterLessThanStringSingleQuotes_Number65()
    {
        var selector = "$[?@.a<'c']";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, less than string, double quotes")]
    public void Test_FilterLessThanStringDoubleQuotes_Number66()
    {
        var selector = "$[?@.a<\"c\"]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, less than number")]
    public void Test_FilterLessThanNumber_Number67()
    {
        var selector = "$[?@.a<10]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"d":"e"},{"a":10,"d":"e"},{"a":"c","d":"f"},{"a":20,"d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, less than null")]
    public void Test_FilterLessThanNull_Number68()
    {
        var selector = "$[?@.a<null]";
        var document = JsonNode.Parse( 
"""
[{"a":null,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("filter, less than true")]
    public void Test_FilterLessThanTrue_Number69()
    {
        var selector = "$[?@.a<true]";
        var document = JsonNode.Parse( 
"""
[{"a":true,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("filter, less than false")]
    public void Test_FilterLessThanFalse_Number70()
    {
        var selector = "$[?@.a<false]";
        var document = JsonNode.Parse( 
"""
[{"a":false,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("filter, less than or equal to string, single quotes")]
    public void Test_FilterLessThanOrEqualToStringSingleQuotes_Number71()
    {
        var selector = "$[?@.a<='c']";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"},{"a":"c","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, less than or equal to string, double quotes")]
    public void Test_FilterLessThanOrEqualToStringDoubleQuotes_Number72()
    {
        var selector = "$[?@.a<=\"c\"]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"},{"a":"c","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, less than or equal to number")]
    public void Test_FilterLessThanOrEqualToNumber_Number73()
    {
        var selector = "$[?@.a<=10]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"d":"e"},{"a":10,"d":"e"},{"a":"c","d":"f"},{"a":20,"d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"d":"e"},{"a":10,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, less than or equal to null")]
    public void Test_FilterLessThanOrEqualToNull_Number74()
    {
        var selector = "$[?@.a<=null]";
        var document = JsonNode.Parse( 
"""
[{"a":null,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":null,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, less than or equal to true")]
    public void Test_FilterLessThanOrEqualToTrue_Number75()
    {
        var selector = "$[?@.a<=true]";
        var document = JsonNode.Parse( 
"""
[{"a":true,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":true,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, less than or equal to false")]
    public void Test_FilterLessThanOrEqualToFalse_Number76()
    {
        var selector = "$[?@.a<=false]";
        var document = JsonNode.Parse( 
"""
[{"a":false,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":false,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, greater than string, single quotes")]
    public void Test_FilterGreaterThanStringSingleQuotes_Number77()
    {
        var selector = "$[?@.a>'c']";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"a":"c","d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"d","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, greater than string, double quotes")]
    public void Test_FilterGreaterThanStringDoubleQuotes_Number78()
    {
        var selector = "$[?@.a>\"c\"]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"a":"c","d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"d","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, greater than number")]
    public void Test_FilterGreaterThanNumber_Number79()
    {
        var selector = "$[?@.a>10]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"d":"e"},{"a":10,"d":"e"},{"a":"c","d":"f"},{"a":20,"d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":20,"d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, greater than null")]
    public void Test_FilterGreaterThanNull_Number80()
    {
        var selector = "$[?@.a>null]";
        var document = JsonNode.Parse( 
"""
[{"a":null,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("filter, greater than true")]
    public void Test_FilterGreaterThanTrue_Number81()
    {
        var selector = "$[?@.a>true]";
        var document = JsonNode.Parse( 
"""
[{"a":true,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("filter, greater than false")]
    public void Test_FilterGreaterThanFalse_Number82()
    {
        var selector = "$[?@.a>false]";
        var document = JsonNode.Parse( 
"""
[{"a":false,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("filter, greater than or equal to string, single quotes")]
    public void Test_FilterGreaterThanOrEqualToStringSingleQuotes_Number83()
    {
        var selector = "$[?@.a>='c']";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"a":"c","d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"c","d":"f"},{"a":"d","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, greater than or equal to string, double quotes")]
    public void Test_FilterGreaterThanOrEqualToStringDoubleQuotes_Number84()
    {
        var selector = "$[?@.a>=\"c\"]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"a":"c","d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"c","d":"f"},{"a":"d","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, greater than or equal to number")]
    public void Test_FilterGreaterThanOrEqualToNumber_Number85()
    {
        var selector = "$[?@.a>=10]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"d":"e"},{"a":10,"d":"e"},{"a":"c","d":"f"},{"a":20,"d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":10,"d":"e"},{"a":20,"d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, greater than or equal to null")]
    public void Test_FilterGreaterThanOrEqualToNull_Number86()
    {
        var selector = "$[?@.a>=null]";
        var document = JsonNode.Parse( 
"""
[{"a":null,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":null,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, greater than or equal to true")]
    public void Test_FilterGreaterThanOrEqualToTrue_Number87()
    {
        var selector = "$[?@.a>=true]";
        var document = JsonNode.Parse( 
"""
[{"a":true,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":true,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, greater than or equal to false")]
    public void Test_FilterGreaterThanOrEqualToFalse_Number88()
    {
        var selector = "$[?@.a>=false]";
        var document = JsonNode.Parse( 
"""
[{"a":false,"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":false,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, exists and not-equals null, absent from data")]
    public void Test_FilterExistsAndNotEqualsNullAbsentFromData_Number89()
    {
        var selector = "$[?@.a&&@.a!=null]";
        var document = JsonNode.Parse( 
"""
[{"d":"e"},{"a":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"c","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, exists and exists, data false")]
    public void Test_FilterExistsAndExistsDataFalse_Number90()
    {
        var selector = "$[?@.a&&@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":false,"b":false},{"b":false},{"c":false}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":false,"b":false}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, exists or exists, data false")]
    public void Test_FilterExistsOrExistsDataFalse_Number91()
    {
        var selector = "$[?@.a||@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":false,"b":false},{"b":false},{"c":false}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":false,"b":false},{"b":false}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, and")]
    public void Test_FilterAnd_Number92()
    {
        var selector = "$[?@.a>0&&@.a<10]";
        var document = JsonNode.Parse( 
"""
[{"a":-10,"d":"e"},{"a":5,"d":"f"},{"a":20,"d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":5,"d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, or")]
    public void Test_FilterOr_Number93()
    {
        var selector = "$[?@.a=='b'||@.a=='d']";
        var document = JsonNode.Parse( 
"""
[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"c","d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"f"},{"a":"d","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, not expression")]
    public void Test_FilterNotExpression_Number94()
    {
        var selector = "$[?!(@.a=='b')]";
        var document = JsonNode.Parse( 
"""
[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"a","d":"e"},{"a":"d","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, not exists")]
    public void Test_FilterNotExists_Number95()
    {
        var selector = "$[?!@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":"a","d":"e"},{"d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, not exists, data null")]
    public void Test_FilterNotExistsDataNull_Number96()
    {
        var selector = "$[?!@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":null,"d":"e"},{"d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, non-singular existence, wildcard")]
    public void Test_FilterNonSingularExistenceWildcard_Number97()
    {
        var selector = "$[?@.*]";
        var document = JsonNode.Parse( 
"""
[1,[],[2],{},{"a":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[[2],{"a":3}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, non-singular existence, multiple")]
    public void Test_FilterNonSingularExistenceMultiple_Number98()
    {
        var selector = "$[?@[0, 0, 'a']]";
        var document = JsonNode.Parse( 
"""
[1,[],[2],[2,3],{"a":3},{"b":4},{"a":3,"b":4}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[[2],[2,3],{"a":3},{"a":3,"b":4}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, non-singular existence, slice")]
    public void Test_FilterNonSingularExistenceSlice_Number99()
    {
        var selector = "$[?@[0:2]]";
        var document = JsonNode.Parse( 
"""
[1,[],[2],[2,3,4],{},{"a":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[[2],[2,3,4]]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, non-singular existence, negated")]
    public void Test_FilterNonSingularExistenceNegated_Number100()
    {
        var selector = "$[?!@.*]";
        var document = JsonNode.Parse( 
"""
[1,[],[2],{},{"a":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[1,[],{}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, non-singular query in comparison, slice")]
    public void Test_FilterNonSingularQueryInComparisonSlice_Number101()
    {        
        var selector = "$[?@[0:0]==0]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, non-singular query in comparison, all children")]
    public void Test_FilterNonSingularQueryInComparisonAllChildren_Number102()
    {        
        var selector = "$[?@[*]==0]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, non-singular query in comparison, descendants")]
    public void Test_FilterNonSingularQueryInComparisonDescendants_Number103()
    {        
        var selector = "$[?@..a==0]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, non-singular query in comparison, combined")]
    public void Test_FilterNonSingularQueryInComparisonCombined_Number104()
    {        
        var selector = "$[?@.a[*].a==0]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, nested")]
    public void Test_FilterNested_Number105()
    {
        var selector = "$[?@[?@>1]]";
        var document = JsonNode.Parse( 
"""
[[0],[0,1],[0,1,2],[42]]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[[0,1,2],[42]]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, name segment on primitive, selects nothing")]
    public void Test_FilterNameSegmentOnPrimitiveSelectsNothing_Number106()
    {
        var selector = "$[?@.a == 1]";
        var document = JsonNode.Parse( 
"""
{"a":1}
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("filter, name segment on array, selects nothing")]
    public void Test_FilterNameSegmentOnArraySelectsNothing_Number107()
    {
        var selector = "$[?@['0'] == 5]";
        var document = JsonNode.Parse( 
"""
[[5,6]]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("filter, index segment on object, selects nothing")]
    public void Test_FilterIndexSegmentOnObjectSelectsNothing_Number108()
    {
        var selector = "$[?@[0] == 5]";
        var document = JsonNode.Parse( 
"""
[{"0":5}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("filter, relative non-singular query, index, equal")]
    public void Test_FilterRelativeNonSingularQueryIndexEqual_Number109()
    {        
        var selector = "$[?(@[0, 0]==42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, relative non-singular query, index, not equal")]
    public void Test_FilterRelativeNonSingularQueryIndexNotEqual_Number110()
    {        
        var selector = "$[?(@[0, 0]!=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, relative non-singular query, index, less-or-equal")]
    public void Test_FilterRelativeNonSingularQueryIndexLessOrEqual_Number111()
    {        
        var selector = "$[?(@[0, 0]<=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, relative non-singular query, name, equal")]
    public void Test_FilterRelativeNonSingularQueryNameEqual_Number112()
    {        
        var selector = "$[?(@['a', 'a']==42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, relative non-singular query, name, not equal")]
    public void Test_FilterRelativeNonSingularQueryNameNotEqual_Number113()
    {        
        var selector = "$[?(@['a', 'a']!=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, relative non-singular query, name, less-or-equal")]
    public void Test_FilterRelativeNonSingularQueryNameLessOrEqual_Number114()
    {        
        var selector = "$[?(@['a', 'a']<=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, relative non-singular query, combined, equal")]
    public void Test_FilterRelativeNonSingularQueryCombinedEqual_Number115()
    {        
        var selector = "$[?(@[0, '0']==42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, relative non-singular query, combined, not equal")]
    public void Test_FilterRelativeNonSingularQueryCombinedNotEqual_Number116()
    {        
        var selector = "$[?(@[0, '0']!=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, relative non-singular query, combined, less-or-equal")]
    public void Test_FilterRelativeNonSingularQueryCombinedLessOrEqual_Number117()
    {        
        var selector = "$[?(@[0, '0']<=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, relative non-singular query, wildcard, equal")]
    public void Test_FilterRelativeNonSingularQueryWildcardEqual_Number118()
    {        
        var selector = "$[?(@.*==42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, relative non-singular query, wildcard, not equal")]
    public void Test_FilterRelativeNonSingularQueryWildcardNotEqual_Number119()
    {        
        var selector = "$[?(@.*!=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, relative non-singular query, wildcard, less-or-equal")]
    public void Test_FilterRelativeNonSingularQueryWildcardLessOrEqual_Number120()
    {        
        var selector = "$[?(@.*<=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, relative non-singular query, slice, equal")]
    public void Test_FilterRelativeNonSingularQuerySliceEqual_Number121()
    {        
        var selector = "$[?(@[0:0]==42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, relative non-singular query, slice, not equal")]
    public void Test_FilterRelativeNonSingularQuerySliceNotEqual_Number122()
    {        
        var selector = "$[?(@[0:0]!=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, relative non-singular query, slice, less-or-equal")]
    public void Test_FilterRelativeNonSingularQuerySliceLessOrEqual_Number123()
    {        
        var selector = "$[?(@[0:0]<=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, absolute non-singular query, index, equal")]
    public void Test_FilterAbsoluteNonSingularQueryIndexEqual_Number124()
    {        
        var selector = "$[?($[0, 0]==42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, absolute non-singular query, index, not equal")]
    public void Test_FilterAbsoluteNonSingularQueryIndexNotEqual_Number125()
    {        
        var selector = "$[?($[0, 0]!=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, absolute non-singular query, index, less-or-equal")]
    public void Test_FilterAbsoluteNonSingularQueryIndexLessOrEqual_Number126()
    {        
        var selector = "$[?($[0, 0]<=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, absolute non-singular query, name, equal")]
    public void Test_FilterAbsoluteNonSingularQueryNameEqual_Number127()
    {        
        var selector = "$[?($['a', 'a']==42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, absolute non-singular query, name, not equal")]
    public void Test_FilterAbsoluteNonSingularQueryNameNotEqual_Number128()
    {        
        var selector = "$[?($['a', 'a']!=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, absolute non-singular query, name, less-or-equal")]
    public void Test_FilterAbsoluteNonSingularQueryNameLessOrEqual_Number129()
    {        
        var selector = "$[?($['a', 'a']<=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, absolute non-singular query, combined, equal")]
    public void Test_FilterAbsoluteNonSingularQueryCombinedEqual_Number130()
    {        
        var selector = "$[?($[0, '0']==42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, absolute non-singular query, combined, not equal")]
    public void Test_FilterAbsoluteNonSingularQueryCombinedNotEqual_Number131()
    {        
        var selector = "$[?($[0, '0']!=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, absolute non-singular query, combined, less-or-equal")]
    public void Test_FilterAbsoluteNonSingularQueryCombinedLessOrEqual_Number132()
    {        
        var selector = "$[?($[0, '0']<=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, absolute non-singular query, wildcard, equal")]
    public void Test_FilterAbsoluteNonSingularQueryWildcardEqual_Number133()
    {        
        var selector = "$[?($.*==42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, absolute non-singular query, wildcard, not equal")]
    public void Test_FilterAbsoluteNonSingularQueryWildcardNotEqual_Number134()
    {        
        var selector = "$[?($.*!=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, absolute non-singular query, wildcard, less-or-equal")]
    public void Test_FilterAbsoluteNonSingularQueryWildcardLessOrEqual_Number135()
    {        
        var selector = "$[?($.*<=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, absolute non-singular query, slice, equal")]
    public void Test_FilterAbsoluteNonSingularQuerySliceEqual_Number136()
    {        
        var selector = "$[?($[0:0]==42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, absolute non-singular query, slice, not equal")]
    public void Test_FilterAbsoluteNonSingularQuerySliceNotEqual_Number137()
    {        
        var selector = "$[?($[0:0]!=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, absolute non-singular query, slice, less-or-equal")]
    public void Test_FilterAbsoluteNonSingularQuerySliceLessOrEqual_Number138()
    {        
        var selector = "$[?($[0:0]<=42)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, multiple selectors")]
    public void Test_FilterMultipleSelectors_Number139()
    {
        var selector = "$[?@.a,?@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, multiple selectors, comparison")]
    public void Test_FilterMultipleSelectorsComparison_Number140()
    {
        var selector = "$[?@.a=='b',?@.b=='x']";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, multiple selectors, overlapping")]
    public void Test_FilterMultipleSelectorsOverlapping_Number141()
    {
        var selector = "$[?@.a,?@.d]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"},{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, multiple selectors, filter and index")]
    public void Test_FilterMultipleSelectorsFilterAndIndex_Number142()
    {
        var selector = "$[?@.a,1]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, multiple selectors, filter and wildcard")]
    public void Test_FilterMultipleSelectorsFilterAndWildcard_Number143()
    {
        var selector = "$[?@.a,*]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"},{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, multiple selectors, filter and slice")]
    public void Test_FilterMultipleSelectorsFilterAndSlice_Number144()
    {
        var selector = "$[?@.a,1:]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"},{"g":"h"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"},{"g":"h"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, multiple selectors, comparison filter, index and slice")]
    public void Test_FilterMultipleSelectorsComparisonFilterIndexAndSlice_Number145()
    {
        var selector = "$[1, ?@.a=='b', 1:]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"b":"c","d":"f"},{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals number, zero and negative zero")]
    public void Test_FilterEqualsNumberZeroAndNegativeZero_Number146()
    {
        var selector = "$[?@.a==-0]";
        var document = JsonNode.Parse( 
"""
[{"a":0,"d":"e"},{"a":0.1,"d":"f"},{"a":"0","d":"g"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":0,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals number, with and without decimal fraction")]
    public void Test_FilterEqualsNumberWithAndWithoutDecimalFraction_Number147()
    {
        var selector = "$[?@.a==1.0]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"d":"e"},{"a":2,"d":"f"},{"a":"1","d":"g"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals number, exponent")]
    public void Test_FilterEqualsNumberExponent_Number148()
    {
        var selector = "$[?@.a==1e2]";
        var document = JsonNode.Parse( 
"""
[{"a":100,"d":"e"},{"a":100.1,"d":"f"},{"a":"100","d":"g"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":100,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals number, positive exponent")]
    public void Test_FilterEqualsNumberPositiveExponent_Number149()
    {
        var selector = "$[?@.a==1e+2]";
        var document = JsonNode.Parse( 
"""
[{"a":100,"d":"e"},{"a":100.1,"d":"f"},{"a":"100","d":"g"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":100,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals number, negative exponent")]
    public void Test_FilterEqualsNumberNegativeExponent_Number150()
    {
        var selector = "$[?@.a==1e-2]";
        var document = JsonNode.Parse( 
"""
[{"a":0.01,"d":"e"},{"a":0.02,"d":"f"},{"a":"0.01","d":"g"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":0.01,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals number, decimal fraction")]
    public void Test_FilterEqualsNumberDecimalFraction_Number151()
    {
        var selector = "$[?@.a==1.1]";
        var document = JsonNode.Parse( 
"""
[{"a":1.1,"d":"e"},{"a":1,"d":"f"},{"a":"1.1","d":"g"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1.1,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals number, decimal fraction, no fractional digit")]
    public void Test_FilterEqualsNumberDecimalFractionNoFractionalDigit_Number152()
    {        
        var selector = "$[?@.a==1.]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, equals number, decimal fraction, exponent")]
    public void Test_FilterEqualsNumberDecimalFractionExponent_Number153()
    {
        var selector = "$[?@.a==1.1e2]";
        var document = JsonNode.Parse( 
"""
[{"a":110,"d":"e"},{"a":110.1,"d":"f"},{"a":"110","d":"g"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":110,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals number, decimal fraction, positive exponent")]
    public void Test_FilterEqualsNumberDecimalFractionPositiveExponent_Number154()
    {
        var selector = "$[?@.a==1.1e+2]";
        var document = JsonNode.Parse( 
"""
[{"a":110,"d":"e"},{"a":110.1,"d":"f"},{"a":"110","d":"g"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":110,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals number, decimal fraction, negative exponent")]
    public void Test_FilterEqualsNumberDecimalFractionNegativeExponent_Number155()
    {
        var selector = "$[?@.a==1.1e-2]";
        var document = JsonNode.Parse( 
"""
[{"a":0.011,"d":"e"},{"a":0.012,"d":"f"},{"a":"0.011","d":"g"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":0.011,"d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals, special nothing")]
    public void Test_FilterEqualsSpecialNothing_Number156()
    {
        var selector = "$.values[?length(@.a) == value($..c)]";
        var document = JsonNode.Parse( 
"""
{"c":"cd","values":[{"a":"ab"},{"c":"d"},{"a":null}]}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"c":"d"},{"a":null}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals, empty node list and empty node list")]
    public void Test_FilterEqualsEmptyNodeListAndEmptyNodeList_Number157()
    {
        var selector = "$[?@.a == @.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"c":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"c":3}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, equals, empty node list and special nothing")]
    public void Test_FilterEqualsEmptyNodeListAndSpecialNothing_Number158()
    {
        var selector = "$[?@.a == length(@.b)]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"c":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"b":2},{"c":3}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, object data")]
    public void Test_FilterObjectData_Number159()
    {
        var selector = "$[?@<3]";
        var document = JsonNode.Parse( 
"""
{"a":1,"b":2,"c":3}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[[1,2],[2,1]]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![0]![count], result));
            count++;
        }
    }

    [TestMethod("filter, and binds more tightly than or")]
    public void Test_FilterAndBindsMoreTightlyThanOr_Number160()
    {
        var selector = "$[?@.a || @.b && @.c]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2,"c":3},{"c":3},{"b":2},{"a":1,"b":2,"c":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2,"c":3},{"a":1,"b":2,"c":3}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, left to right evaluation")]
    public void Test_FilterLeftToRightEvaluation_Number161()
    {
        var selector = "$[?@.a && @.b || @.c]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":1,"b":2},{"a":1,"c":3},{"b":1,"c":3},{"c":3},{"a":1,"b":2,"c":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2},{"a":1,"c":3},{"b":1,"c":3},{"c":3},{"a":1,"b":2,"c":3}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, group terms, left")]
    public void Test_FilterGroupTermsLeft_Number162()
    {
        var selector = "$[?(@.a || @.b) && @.c]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":2},{"a":1,"c":3},{"b":2,"c":3},{"a":1},{"b":2},{"c":3},{"a":1,"b":2,"c":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"c":3},{"b":2,"c":3},{"a":1,"b":2,"c":3}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, group terms, right")]
    public void Test_FilterGroupTermsRight_Number163()
    {
        var selector = "$[?@.a && (@.b || @.c)]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"a":1,"b":2},{"a":1,"c":2},{"b":2},{"c":2},{"a":1,"b":2,"c":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2},{"a":1,"c":2},{"a":1,"b":2,"c":3}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, string literal, single quote in double quotes")]
    public void Test_FilterStringLiteralSingleQuoteInDoubleQuotes_Number164()
    {
        var selector = "$[?@ == \"quoted' literal\"]";
        var document = JsonNode.Parse( 
"""
["quoted' literal","a","quoted\\' literal"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["quoted' literal"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, string literal, double quote in single quotes")]
    public void Test_FilterStringLiteralDoubleQuoteInSingleQuotes_Number165()
    {
        var selector = "$[?@ == 'quoted\" literal']";
        var document = JsonNode.Parse( 
"""
["quoted\" literal","a","quoted\\\" literal","'quoted\" literal'"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["quoted\" literal"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, string literal, escaped single quote in single quotes")]
    public void Test_FilterStringLiteralEscapedSingleQuoteInSingleQuotes_Number166()
    {
        var selector = "$[?@ == 'quoted\' literal']";
        var document = JsonNode.Parse( 
"""
["quoted' literal","a","quoted\\' literal","'quoted\" literal'"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["quoted' literal"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, string literal, escaped double quote in double quotes")]
    public void Test_FilterStringLiteralEscapedDoubleQuoteInDoubleQuotes_Number167()
    {
        var selector = "$[?@ == \"quoted\\" literal\"]";
        var document = JsonNode.Parse( 
"""
["quoted\" literal","a","quoted\\\" literal","'quoted\" literal'"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["quoted\" literal"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("filter, literal true must be compared")]
    public void Test_FilterLiteralTrueMustBeCompared_Number168()
    {        
        var selector = "$[?true]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, literal false must be compared")]
    public void Test_FilterLiteralFalseMustBeCompared_Number169()
    {        
        var selector = "$[?false]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, literal string must be compared")]
    public void Test_FilterLiteralStringMustBeCompared_Number170()
    {        
        var selector = "$[?'abc']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, literal int must be compared")]
    public void Test_FilterLiteralIntMustBeCompared_Number171()
    {        
        var selector = "$[?2]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, literal float must be compared")]
    public void Test_FilterLiteralFloatMustBeCompared_Number172()
    {        
        var selector = "$[?2.2]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, literal null must be compared")]
    public void Test_FilterLiteralNullMustBeCompared_Number173()
    {        
        var selector = "$[?null]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, and, literals must be compared")]
    public void Test_FilterAndLiteralsMustBeCompared_Number174()
    {        
        var selector = "$[?true && false]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, or, literals must be compared")]
    public void Test_FilterOrLiteralsMustBeCompared_Number175()
    {        
        var selector = "$[?true || false]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, and, right hand literal must be compared")]
    public void Test_FilterAndRightHandLiteralMustBeCompared_Number176()
    {        
        var selector = "$[?true == false && false]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, or, right hand literal must be compared")]
    public void Test_FilterOrRightHandLiteralMustBeCompared_Number177()
    {        
        var selector = "$[?true == false || false]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, and, left hand literal must be compared")]
    public void Test_FilterAndLeftHandLiteralMustBeCompared_Number178()
    {        
        var selector = "$[?false && true == false]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("filter, or, left hand literal must be compared")]
    public void Test_FilterOrLeftHandLiteralMustBeCompared_Number179()
    {        
        var selector = "$[?false || true == false]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("index selector, first element")]
    public void Test_IndexSelectorFirstElement_Number180()
    {
        var selector = "$[0]";
        var document = JsonNode.Parse( 
"""
["first","second"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["first"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("index selector, second element")]
    public void Test_IndexSelectorSecondElement_Number181()
    {
        var selector = "$[1]";
        var document = JsonNode.Parse( 
"""
["first","second"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["second"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("index selector, out of bound")]
    public void Test_IndexSelectorOutOfBound_Number182()
    {
        var selector = "$[2]";
        var document = JsonNode.Parse( 
"""
["first","second"]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("index selector, overflowing index")]
    public void Test_IndexSelectorOverflowingIndex_Number183()
    {        
        var selector = "$[231584178474632390847141970017375815706539969331281128078915168015826259279872]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("index selector, not actually an index, overflowing index leads into general text")]
    public void Test_IndexSelectorNotActuallyAnIndexOverflowingIndexLeadsIntoGeneralText_Number184()
    {        
        var selector = "$[231584178474632390847141970017375815706539969331281128078915168SomeRandomText]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("index selector, negative")]
    public void Test_IndexSelectorNegative_Number185()
    {
        var selector = "$[-1]";
        var document = JsonNode.Parse( 
"""
["first","second"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["second"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("index selector, more negative")]
    public void Test_IndexSelectorMoreNegative_Number186()
    {
        var selector = "$[-2]";
        var document = JsonNode.Parse( 
"""
["first","second"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["first"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("index selector, negative out of bound")]
    public void Test_IndexSelectorNegativeOutOfBound_Number187()
    {
        var selector = "$[-3]";
        var document = JsonNode.Parse( 
"""
["first","second"]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("index selector, on object")]
    public void Test_IndexSelectorOnObject_Number188()
    {
        var selector = "$[0]";
        var document = JsonNode.Parse( 
"""
{"foo":1}
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("index selector, leading 0")]
    public void Test_IndexSelectorLeading0_Number189()
    {        
        var selector = "$[01]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("index selector, leading -0")]
    public void Test_IndexSelectorLeading0_Number190()
    {        
        var selector = "$[-01]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes")]
    public void Test_NameSelectorDoubleQuotes_Number191()
    {
        var selector = "$[\"a\"]";
        var document = JsonNode.Parse( 
"""
{"a":"A","b":"B"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, double quotes, absent data")]
    public void Test_NameSelectorDoubleQuotesAbsentData_Number192()
    {
        var selector = "$[\"c\"]";
        var document = JsonNode.Parse( 
"""
{"a":"A","b":"B"}
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("name selector, double quotes, array data")]
    public void Test_NameSelectorDoubleQuotesArrayData_Number193()
    {
        var selector = "$[\"a\"]";
        var document = JsonNode.Parse( 
"""
["first","second"]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("name selector, double quotes, embedded U+0000")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0000_Number194()
    {        
        var selector = "$[\" \"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0001")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0001_Number195()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0002")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0002_Number196()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0003")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0003_Number197()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0004")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0004_Number198()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0005")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0005_Number199()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0006")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0006_Number200()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0007")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0007_Number201()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0008")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0008_Number202()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0009")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0009_Number203()
    {        
        var selector = "$[\"\t\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+000A")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU000a_Number204()
    {        
        var selector = "$[\"\n\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+000B")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU000b_Number205()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+000C")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU000c_Number206()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+000D")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU000d_Number207()
    {        
        var selector = "$[\"\r\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+000E")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU000e_Number208()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+000F")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU000f_Number209()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0010")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0010_Number210()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0011")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0011_Number211()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0012")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0012_Number212()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0013")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0013_Number213()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0014")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0014_Number214()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0015")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0015_Number215()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0016")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0016_Number216()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0017")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0017_Number217()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0018")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0018_Number218()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0019")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0019_Number219()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+001A")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU001a_Number220()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+001B")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU001b_Number221()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+001C")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU001c_Number222()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+001D")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU001d_Number223()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+001E")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU001e_Number224()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+001F")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU001f_Number225()
    {        
        var selector = "$[\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded U+0020")]
    public void Test_NameSelectorDoubleQuotesEmbeddedU0020_Number226()
    {
        var selector = "$[\" \"]";
        var document = JsonNode.Parse( 
"""
{" ":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, double quotes, escaped double quote")]
    public void Test_NameSelectorDoubleQuotesEscapedDoubleQuote_Number227()
    {
        var selector = "$[\"\\"\"]";
        var document = JsonNode.Parse( 
"""
{"\"":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, double quotes, escaped reverse solidus")]
    public void Test_NameSelectorDoubleQuotesEscapedReverseSolidus_Number228()
    {
        var selector = "$[\"\\\"]";
        var document = JsonNode.Parse( 
"""
{"\\":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, double quotes, escaped solidus")]
    public void Test_NameSelectorDoubleQuotesEscapedSolidus_Number229()
    {
        var selector = "$[\"\/\"]";
        var document = JsonNode.Parse( 
"""
{"/":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, double quotes, escaped backspace")]
    public void Test_NameSelectorDoubleQuotesEscapedBackspace_Number230()
    {
        var selector = "$[\"\b\"]";
        var document = JsonNode.Parse( 
"""
{"\b":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, double quotes, escaped form feed")]
    public void Test_NameSelectorDoubleQuotesEscapedFormFeed_Number231()
    {
        var selector = "$[\"\f\"]";
        var document = JsonNode.Parse( 
"""
{"\f":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, double quotes, escaped line feed")]
    public void Test_NameSelectorDoubleQuotesEscapedLineFeed_Number232()
    {
        var selector = "$[\"\n\"]";
        var document = JsonNode.Parse( 
"""
{"\n":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, double quotes, escaped carriage return")]
    public void Test_NameSelectorDoubleQuotesEscapedCarriageReturn_Number233()
    {
        var selector = "$[\"\r\"]";
        var document = JsonNode.Parse( 
"""
{"\r":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, double quotes, escaped tab")]
    public void Test_NameSelectorDoubleQuotesEscapedTab_Number234()
    {
        var selector = "$[\"\t\"]";
        var document = JsonNode.Parse( 
"""
{"\t":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, double quotes, escaped ☺, upper case hex")]
    public void Test_NameSelectorDoubleQuotesEscapedUpperCaseHex_Number235()
    {
        var selector = "$[\"\u263A\"]";
        var document = JsonNode.Parse( 
"""
{"☺":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, double quotes, escaped ☺, lower case hex")]
    public void Test_NameSelectorDoubleQuotesEscapedLowerCaseHex_Number236()
    {
        var selector = "$[\"\u263a\"]";
        var document = JsonNode.Parse( 
"""
{"☺":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, double quotes, surrogate pair 𝄞")]
    public void Test_NameSelectorDoubleQuotesSurrogatePair_Number237()
    {
        var selector = "$[\"\uD834\uDD1E\"]";
        var document = JsonNode.Parse( 
"""
{"𝄞":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, double quotes, surrogate pair 😀")]
    public void Test_NameSelectorDoubleQuotesSurrogatePair_Number238()
    {
        var selector = "$[\"\uD83D\uDE00\"]";
        var document = JsonNode.Parse( 
"""
{"😀":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, double quotes, invalid escaped single quote")]
    public void Test_NameSelectorDoubleQuotesInvalidEscapedSingleQuote_Number239()
    {        
        var selector = "$[\"\'\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, embedded double quote")]
    public void Test_NameSelectorDoubleQuotesEmbeddedDoubleQuote_Number240()
    {        
        var selector = "$[\"\"\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, incomplete escape")]
    public void Test_NameSelectorDoubleQuotesIncompleteEscape_Number241()
    {        
        var selector = "$[\"\\"]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes")]
    public void Test_NameSelectorSingleQuotes_Number242()
    {
        var selector = "$['a']";
        var document = JsonNode.Parse( 
"""
{"a":"A","b":"B"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, single quotes, absent data")]
    public void Test_NameSelectorSingleQuotesAbsentData_Number243()
    {
        var selector = "$['c']";
        var document = JsonNode.Parse( 
"""
{"a":"A","b":"B"}
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("name selector, single quotes, array data")]
    public void Test_NameSelectorSingleQuotesArrayData_Number244()
    {
        var selector = "$['a']";
        var document = JsonNode.Parse( 
"""
["first","second"]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("name selector, single quotes, embedded U+0000")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0000_Number245()
    {        
        var selector = "$[' ']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0001")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0001_Number246()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0002")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0002_Number247()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0003")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0003_Number248()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0004")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0004_Number249()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0005")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0005_Number250()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0006")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0006_Number251()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0007")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0007_Number252()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0008")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0008_Number253()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0009")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0009_Number254()
    {        
        var selector = "$['\t']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+000A")]
    public void Test_NameSelectorSingleQuotesEmbeddedU000a_Number255()
    {        
        var selector = "$['\n']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+000B")]
    public void Test_NameSelectorSingleQuotesEmbeddedU000b_Number256()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+000C")]
    public void Test_NameSelectorSingleQuotesEmbeddedU000c_Number257()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+000D")]
    public void Test_NameSelectorSingleQuotesEmbeddedU000d_Number258()
    {        
        var selector = "$['\r']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+000E")]
    public void Test_NameSelectorSingleQuotesEmbeddedU000e_Number259()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+000F")]
    public void Test_NameSelectorSingleQuotesEmbeddedU000f_Number260()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0010")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0010_Number261()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0011")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0011_Number262()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0012")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0012_Number263()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0013")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0013_Number264()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0014")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0014_Number265()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0015")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0015_Number266()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0016")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0016_Number267()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0017")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0017_Number268()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0018")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0018_Number269()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0019")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0019_Number270()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+001A")]
    public void Test_NameSelectorSingleQuotesEmbeddedU001a_Number271()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+001B")]
    public void Test_NameSelectorSingleQuotesEmbeddedU001b_Number272()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+001C")]
    public void Test_NameSelectorSingleQuotesEmbeddedU001c_Number273()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+001D")]
    public void Test_NameSelectorSingleQuotesEmbeddedU001d_Number274()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+001E")]
    public void Test_NameSelectorSingleQuotesEmbeddedU001e_Number275()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+001F")]
    public void Test_NameSelectorSingleQuotesEmbeddedU001f_Number276()
    {        
        var selector = "$['']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded U+0020")]
    public void Test_NameSelectorSingleQuotesEmbeddedU0020_Number277()
    {
        var selector = "$[' ']";
        var document = JsonNode.Parse( 
"""
{" ":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, single quotes, escaped single quote")]
    public void Test_NameSelectorSingleQuotesEscapedSingleQuote_Number278()
    {
        var selector = "$['\'']";
        var document = JsonNode.Parse( 
"""
{"'":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, single quotes, escaped reverse solidus")]
    public void Test_NameSelectorSingleQuotesEscapedReverseSolidus_Number279()
    {
        var selector = "$['\\']";
        var document = JsonNode.Parse( 
"""
{"\\":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, single quotes, escaped solidus")]
    public void Test_NameSelectorSingleQuotesEscapedSolidus_Number280()
    {
        var selector = "$['\/']";
        var document = JsonNode.Parse( 
"""
{"/":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, single quotes, escaped backspace")]
    public void Test_NameSelectorSingleQuotesEscapedBackspace_Number281()
    {
        var selector = "$['\b']";
        var document = JsonNode.Parse( 
"""
{"\b":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, single quotes, escaped form feed")]
    public void Test_NameSelectorSingleQuotesEscapedFormFeed_Number282()
    {
        var selector = "$['\f']";
        var document = JsonNode.Parse( 
"""
{"\f":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, single quotes, escaped line feed")]
    public void Test_NameSelectorSingleQuotesEscapedLineFeed_Number283()
    {
        var selector = "$['\n']";
        var document = JsonNode.Parse( 
"""
{"\n":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, single quotes, escaped carriage return")]
    public void Test_NameSelectorSingleQuotesEscapedCarriageReturn_Number284()
    {
        var selector = "$['\r']";
        var document = JsonNode.Parse( 
"""
{"\r":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, single quotes, escaped tab")]
    public void Test_NameSelectorSingleQuotesEscapedTab_Number285()
    {
        var selector = "$['\t']";
        var document = JsonNode.Parse( 
"""
{"\t":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, single quotes, escaped ☺, upper case hex")]
    public void Test_NameSelectorSingleQuotesEscapedUpperCaseHex_Number286()
    {
        var selector = "$['\u263A']";
        var document = JsonNode.Parse( 
"""
{"☺":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, single quotes, escaped ☺, lower case hex")]
    public void Test_NameSelectorSingleQuotesEscapedLowerCaseHex_Number287()
    {
        var selector = "$['\u263a']";
        var document = JsonNode.Parse( 
"""
{"☺":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, single quotes, surrogate pair 𝄞")]
    public void Test_NameSelectorSingleQuotesSurrogatePair_Number288()
    {
        var selector = "$['\uD834\uDD1E']";
        var document = JsonNode.Parse( 
"""
{"𝄞":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, single quotes, surrogate pair 😀")]
    public void Test_NameSelectorSingleQuotesSurrogatePair_Number289()
    {
        var selector = "$['\uD83D\uDE00']";
        var document = JsonNode.Parse( 
"""
{"😀":"A"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["A"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, single quotes, invalid escaped double quote")]
    public void Test_NameSelectorSingleQuotesInvalidEscapedDoubleQuote_Number290()
    {        
        var selector = "$['\\"']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, embedded single quote")]
    public void Test_NameSelectorSingleQuotesEmbeddedSingleQuote_Number291()
    {        
        var selector = "$[''']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, single quotes, incomplete escape")]
    public void Test_NameSelectorSingleQuotesIncompleteEscape_Number292()
    {        
        var selector = "$['\']";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("name selector, double quotes, empty")]
    public void Test_NameSelectorDoubleQuotesEmpty_Number293()
    {
        var selector = "$[\"\"]";
        var document = JsonNode.Parse( 
"""
{"a":"A","b":"B","":"C"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["C"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("name selector, single quotes, empty")]
    public void Test_NameSelectorSingleQuotesEmpty_Number294()
    {
        var selector = "$['']";
        var document = JsonNode.Parse( 
"""
{"a":"A","b":"B","":"C"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["C"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, slice selector")]
    public void Test_SliceSelectorSliceSelector_Number295()
    {
        var selector = "$[1:3]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[1,2]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, slice selector with step")]
    public void Test_SliceSelectorSliceSelectorWithStep_Number296()
    {
        var selector = "$[1:6:2]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[1,3,5]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, slice selector with everything omitted, short form")]
    public void Test_SliceSelectorSliceSelectorWithEverythingOmittedShortForm_Number297()
    {
        var selector = "$[:]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[0,1,2,3]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, slice selector with everything omitted, long form")]
    public void Test_SliceSelectorSliceSelectorWithEverythingOmittedLongForm_Number298()
    {
        var selector = "$[::]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[0,1,2,3]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, slice selector with start omitted")]
    public void Test_SliceSelectorSliceSelectorWithStartOmitted_Number299()
    {
        var selector = "$[:2]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[0,1]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, slice selector with start and end omitted")]
    public void Test_SliceSelectorSliceSelectorWithStartAndEndOmitted_Number300()
    {
        var selector = "$[::2]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[0,2,4,6,8]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, negative step with default start and end")]
    public void Test_SliceSelectorNegativeStepWithDefaultStartAndEnd_Number301()
    {
        var selector = "$[::-1]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[3,2,1,0]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, negative step with default start")]
    public void Test_SliceSelectorNegativeStepWithDefaultStart_Number302()
    {
        var selector = "$[:0:-1]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[3,2,1]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, negative step with default end")]
    public void Test_SliceSelectorNegativeStepWithDefaultEnd_Number303()
    {
        var selector = "$[2::-1]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,1,0]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, larger negative step")]
    public void Test_SliceSelectorLargerNegativeStep_Number304()
    {
        var selector = "$[::-2]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[3,1]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, negative range with default step")]
    public void Test_SliceSelectorNegativeRangeWithDefaultStep_Number305()
    {
        var selector = "$[-1:-3]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("slice selector, negative range with negative step")]
    public void Test_SliceSelectorNegativeRangeWithNegativeStep_Number306()
    {
        var selector = "$[-1:-3:-1]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[9,8]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, negative range with larger negative step")]
    public void Test_SliceSelectorNegativeRangeWithLargerNegativeStep_Number307()
    {
        var selector = "$[-1:-6:-2]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[9,7,5]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, larger negative range with larger negative step")]
    public void Test_SliceSelectorLargerNegativeRangeWithLargerNegativeStep_Number308()
    {
        var selector = "$[-1:-7:-2]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[9,7,5]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, negative from, positive to")]
    public void Test_SliceSelectorNegativeFromPositiveTo_Number309()
    {
        var selector = "$[-5:7]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[5,6]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, negative from")]
    public void Test_SliceSelectorNegativeFrom_Number310()
    {
        var selector = "$[-2:]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[8,9]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, positive from, negative to")]
    public void Test_SliceSelectorPositiveFromNegativeTo_Number311()
    {
        var selector = "$[1:-1]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[1,2,3,4,5,6,7,8]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, negative from, positive to, negative step")]
    public void Test_SliceSelectorNegativeFromPositiveToNegativeStep_Number312()
    {
        var selector = "$[-1:1:-1]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[9,8,7,6,5,4,3,2]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, positive from, negative to, negative step")]
    public void Test_SliceSelectorPositiveFromNegativeToNegativeStep_Number313()
    {
        var selector = "$[7:-5:-1]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[7,6]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, too many colons")]
    public void Test_SliceSelectorTooManyColons_Number314()
    {        
        var selector = "$[1:2:3:4]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("slice selector, non-integer array index")]
    public void Test_SliceSelectorNonIntegerArrayIndex_Number315()
    {        
        var selector = "$[1:2:a]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("slice selector, zero step")]
    public void Test_SliceSelectorZeroStep_Number316()
    {
        var selector = "$[1:2:0]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("slice selector, empty range")]
    public void Test_SliceSelectorEmptyRange_Number317()
    {
        var selector = "$[2:2]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("slice selector, slice selector with everything omitted with empty array")]
    public void Test_SliceSelectorSliceSelectorWithEverythingOmittedWithEmptyArray_Number318()
    {
        var selector = "$[:]";
        var document = JsonNode.Parse( 
"""
null
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("slice selector, negative step with empty array")]
    public void Test_SliceSelectorNegativeStepWithEmptyArray_Number319()
    {
        var selector = "$[::-1]";
        var document = JsonNode.Parse( 
"""
null
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("slice selector, maximal range with positive step")]
    public void Test_SliceSelectorMaximalRangeWithPositiveStep_Number320()
    {
        var selector = "$[0:10]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[0,1,2,3,4,5,6,7,8,9]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, maximal range with negative step")]
    public void Test_SliceSelectorMaximalRangeWithNegativeStep_Number321()
    {
        var selector = "$[9:0:-1]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[9,8,7,6,5,4,3,2,1]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, excessively large to value")]
    public void Test_SliceSelectorExcessivelyLargeToValue_Number322()
    {
        var selector = "$[2:113667776004]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,3,4,5,6,7,8,9]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, excessively small from value")]
    public void Test_SliceSelectorExcessivelySmallFromValue_Number323()
    {
        var selector = "$[-113667776004:1]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("slice selector, excessively large from value with negative step")]
    public void Test_SliceSelectorExcessivelyLargeFromValueWithNegativeStep_Number324()
    {
        var selector = "$[113667776004:0:-1]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[9,8,7,6,5,4,3,2,1]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, excessively small to value with negative step")]
    public void Test_SliceSelectorExcessivelySmallToValueWithNegativeStep_Number325()
    {
        var selector = "$[3:-113667776004:-1]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[3,2,1,0]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, excessively large step")]
    public void Test_SliceSelectorExcessivelyLargeStep_Number326()
    {
        var selector = "$[1:10:113667776004]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[1]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, excessively small step")]
    public void Test_SliceSelectorExcessivelySmallStep_Number327()
    {
        var selector = "$[-1:-10:-113667776004]";
        var document = JsonNode.Parse( 
"""
[0,1,2,3,4,5,6,7,8,9]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[9]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("slice selector, overflowing to value")]
    public void Test_SliceSelectorOverflowingToValue_Number328()
    {        
        var selector = "$[2:231584178474632390847141970017375815706539969331281128078915168015826259279872]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("slice selector, underflowing from value")]
    public void Test_SliceSelectorUnderflowingFromValue_Number329()
    {        
        var selector = "$[-231584178474632390847141970017375815706539969331281128078915168015826259279872:1]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("slice selector, overflowing from value with negative step")]
    public void Test_SliceSelectorOverflowingFromValueWithNegativeStep_Number330()
    {        
        var selector = "$[231584178474632390847141970017375815706539969331281128078915168015826259279872:0:-1]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("slice selector, underflowing to value with negative step")]
    public void Test_SliceSelectorUnderflowingToValueWithNegativeStep_Number331()
    {        
        var selector = "$[3:-231584178474632390847141970017375815706539969331281128078915168015826259279872:-1]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("slice selector, overflowing step")]
    public void Test_SliceSelectorOverflowingStep_Number332()
    {        
        var selector = "$[1:10:231584178474632390847141970017375815706539969331281128078915168015826259279872]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("slice selector, underflowing step")]
    public void Test_SliceSelectorUnderflowingStep_Number333()
    {        
        var selector = "$[-1:-10:-231584178474632390847141970017375815706539969331281128078915168015826259279872]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, count, count function")]
    public void Test_FunctionsCountCountFunction_Number334()
    {
        var selector = "$[?count(@..*)>2]";
        var document = JsonNode.Parse( 
"""
[{"a":[1,2,3]},{"a":[1],"d":"f"},{"a":1,"d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":[1,2,3]},{"a":[1],"d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, count, single-node arg")]
    public void Test_FunctionsCountSingleNodeArg_Number335()
    {
        var selector = "$[?count(@.a)>1]";
        var document = JsonNode.Parse( 
"""
[{"a":[1,2,3]},{"a":[1],"d":"f"},{"a":1,"d":"f"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("functions, count, multiple-selector arg")]
    public void Test_FunctionsCountMultipleSelectorArg_Number336()
    {
        var selector = "$[?count(@['a','d'])>1]";
        var document = JsonNode.Parse( 
"""
[{"a":[1,2,3]},{"a":[1],"d":"f"},{"a":1,"d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":[1],"d":"f"},{"a":1,"d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, count, non-query arg, number")]
    public void Test_FunctionsCountNonQueryArgNumber_Number337()
    {        
        var selector = "$[?count(1)>2]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, count, non-query arg, string")]
    public void Test_FunctionsCountNonQueryArgString_Number338()
    {        
        var selector = "$[?count('string')>2]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, count, non-query arg, true")]
    public void Test_FunctionsCountNonQueryArgTrue_Number339()
    {        
        var selector = "$[?count(true)>2]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, count, non-query arg, false")]
    public void Test_FunctionsCountNonQueryArgFalse_Number340()
    {        
        var selector = "$[?count(false)>2]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, count, non-query arg, null")]
    public void Test_FunctionsCountNonQueryArgNull_Number341()
    {        
        var selector = "$[?count(null)>2]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, count, result must be compared")]
    public void Test_FunctionsCountResultMustBeCompared_Number342()
    {        
        var selector = "$[?count(@..*)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, count, no params")]
    public void Test_FunctionsCountNoParams_Number343()
    {        
        var selector = "$[?count()==1]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, count, too many params")]
    public void Test_FunctionsCountTooManyParams_Number344()
    {        
        var selector = "$[?count(@.a,@.b)==1]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, length, string data")]
    public void Test_FunctionsLengthStringData_Number345()
    {
        var selector = "$[?length(@.a)>=2]";
        var document = JsonNode.Parse( 
"""
[{"a":"ab"},{"a":"d"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"ab"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, length, string data, unicode")]
    public void Test_FunctionsLengthStringDataUnicode_Number346()
    {
        var selector = "$[?length(@)==2]";
        var document = JsonNode.Parse( 
"""
["☺","☺☺","☺☺☺","ж","жж","жжж","磨","阿美","形声字"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["☺☺","жж","阿美"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, length, array data")]
    public void Test_FunctionsLengthArrayData_Number347()
    {
        var selector = "$[?length(@.a)>=2]";
        var document = JsonNode.Parse( 
"""
[{"a":[1,2,3]},{"a":[1]}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":[1,2,3]}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, length, missing data")]
    public void Test_FunctionsLengthMissingData_Number348()
    {
        var selector = "$[?length(@.a)>=2]";
        var document = JsonNode.Parse( 
"""
[{"d":"f"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("functions, length, number arg")]
    public void Test_FunctionsLengthNumberArg_Number349()
    {
        var selector = "$[?length(1)>=2]";
        var document = JsonNode.Parse( 
"""
[{"d":"f"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("functions, length, true arg")]
    public void Test_FunctionsLengthTrueArg_Number350()
    {
        var selector = "$[?length(true)>=2]";
        var document = JsonNode.Parse( 
"""
[{"d":"f"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("functions, length, false arg")]
    public void Test_FunctionsLengthFalseArg_Number351()
    {
        var selector = "$[?length(false)>=2]";
        var document = JsonNode.Parse( 
"""
[{"d":"f"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("functions, length, null arg")]
    public void Test_FunctionsLengthNullArg_Number352()
    {
        var selector = "$[?length(null)>=2]";
        var document = JsonNode.Parse( 
"""
[{"d":"f"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("functions, length, result must be compared")]
    public void Test_FunctionsLengthResultMustBeCompared_Number353()
    {        
        var selector = "$[?length(@.a)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, length, no params")]
    public void Test_FunctionsLengthNoParams_Number354()
    {        
        var selector = "$[?length()==1]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, length, too many params")]
    public void Test_FunctionsLengthTooManyParams_Number355()
    {        
        var selector = "$[?length(@.a,@.b)==1]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, length, non-singular query arg")]
    public void Test_FunctionsLengthNonSingularQueryArg_Number356()
    {        
        var selector = "$[?length(@.*)<3]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, length, arg is a function expression")]
    public void Test_FunctionsLengthArgIsAFunctionExpression_Number357()
    {
        var selector = "$.values[?length(@.a)==length(value($..c))]";
        var document = JsonNode.Parse( 
"""
{"c":"cd","values":[{"a":"ab"},{"a":"d"}]}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"ab"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, length, arg is special nothing")]
    public void Test_FunctionsLengthArgIsSpecialNothing_Number358()
    {
        var selector = "$[?length(value(@.a))>0]";
        var document = JsonNode.Parse( 
"""
[{"a":"ab"},{"c":"d"},{"a":null}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"ab"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, found match")]
    public void Test_FunctionsMatchFoundMatch_Number359()
    {
        var selector = "$[?match(@.a, 'a.*')]";
        var document = JsonNode.Parse( 
"""
[{"a":"ab"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"ab"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, double quotes")]
    public void Test_FunctionsMatchDoubleQuotes_Number360()
    {
        var selector = "$[?match(@.a, \"a.*\")]";
        var document = JsonNode.Parse( 
"""
[{"a":"ab"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"ab"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, regex from the document")]
    public void Test_FunctionsMatchRegexFromTheDocument_Number361()
    {
        var selector = "$.values[?match(@, $.regex)]";
        var document = JsonNode.Parse( 
"""
{"regex":"b.?b","values":["abc","bcd","bab","bba","bbab","b",true,[],{}]}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["bab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, don't select match")]
    public void Test_FunctionsMatchDonTSelectMatch_Number362()
    {
        var selector = "$[?!match(@.a, 'a.*')]";
        var document = JsonNode.Parse( 
"""
[{"a":"ab"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("functions, match, not a match")]
    public void Test_FunctionsMatchNotAMatch_Number363()
    {
        var selector = "$[?match(@.a, 'a.*')]";
        var document = JsonNode.Parse( 
"""
[{"a":"bc"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("functions, match, select non-match")]
    public void Test_FunctionsMatchSelectNonMatch_Number364()
    {
        var selector = "$[?!match(@.a, 'a.*')]";
        var document = JsonNode.Parse( 
"""
[{"a":"bc"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"bc"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, non-string first arg")]
    public void Test_FunctionsMatchNonStringFirstArg_Number365()
    {
        var selector = "$[?match(1, 'a.*')]";
        var document = JsonNode.Parse( 
"""
[{"a":"bc"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("functions, match, non-string second arg")]
    public void Test_FunctionsMatchNonStringSecondArg_Number366()
    {
        var selector = "$[?match(@.a, 1)]";
        var document = JsonNode.Parse( 
"""
[{"a":"bc"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("functions, match, filter, match function, unicode char class, uppercase")]
    public void Test_FunctionsMatchFilterMatchFunctionUnicodeCharClassUppercase_Number367()
    {
        var selector = "$[?match(@, '\\p{Lu}')]";
        var document = JsonNode.Parse( 
"""
["ж","Ж","1","жЖ",true,[],{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["Ж"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, filter, match function, unicode char class negated, uppercase")]
    public void Test_FunctionsMatchFilterMatchFunctionUnicodeCharClassNegatedUppercase_Number368()
    {
        var selector = "$[?match(@, '\\P{Lu}')]";
        var document = JsonNode.Parse( 
"""
["ж","Ж","1",true,[],{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ж","1"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, filter, match function, unicode, surrogate pair")]
    public void Test_FunctionsMatchFilterMatchFunctionUnicodeSurrogatePair_Number369()
    {
        var selector = "$[?match(@, 'a.b')]";
        var document = JsonNode.Parse( 
"""
["a𐄁b","ab","1",true,[],{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["a𐄁b"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, dot matcher on \u2028")]
    public void Test_FunctionsMatchDotMatcherOnU2028_Number370()
    {
        var selector = "$[?match(@, '.')]";
        var document = JsonNode.Parse( 
"""
["\u2028","\r","\n",true,[],{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["\u2028"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, dot matcher on \u2029")]
    public void Test_FunctionsMatchDotMatcherOnU2029_Number371()
    {
        var selector = "$[?match(@, '.')]";
        var document = JsonNode.Parse( 
"""
["\u2029","\r","\n",true,[],{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["\u2029"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, result cannot be compared")]
    public void Test_FunctionsMatchResultCannotBeCompared_Number372()
    {        
        var selector = "$[?match(@.a, 'a.*')==true]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, match, too few params")]
    public void Test_FunctionsMatchTooFewParams_Number373()
    {        
        var selector = "$[?match(@.a)==1]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, match, too many params")]
    public void Test_FunctionsMatchTooManyParams_Number374()
    {        
        var selector = "$[?match(@.a,@.b,@.c)==1]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, match, arg is a function expression")]
    public void Test_FunctionsMatchArgIsAFunctionExpression_Number375()
    {
        var selector = "$.values[?match(@.a, value($..['regex']))]";
        var document = JsonNode.Parse( 
"""
{"regex":"a.*","values":[{"a":"ab"},{"a":"ba"}]}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"ab"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, dot in character class")]
    public void Test_FunctionsMatchDotInCharacterClass_Number376()
    {
        var selector = "$[?match(@, 'a[.b]c')]";
        var document = JsonNode.Parse( 
"""
["abc","a.c","axc"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["abc","a.c"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, escaped dot")]
    public void Test_FunctionsMatchEscapedDot_Number377()
    {
        var selector = "$[?match(@, 'a\\.c')]";
        var document = JsonNode.Parse( 
"""
["abc","a.c","axc"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["a.c"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, escaped backslash before dot")]
    public void Test_FunctionsMatchEscapedBackslashBeforeDot_Number378()
    {
        var selector = "$[?match(@, 'a\\\\.c')]";
        var document = JsonNode.Parse( 
"""
["abc","a.c","axc","a\\\u2028c"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["a\\\u2028c"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, escaped left square bracket")]
    public void Test_FunctionsMatchEscapedLeftSquareBracket_Number379()
    {
        var selector = "$[?match(@, 'a\\[.c')]";
        var document = JsonNode.Parse( 
"""
["abc","a.c","a[\u2028c"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["a[\u2028c"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, escaped right square bracket")]
    public void Test_FunctionsMatchEscapedRightSquareBracket_Number380()
    {
        var selector = "$[?match(@, 'a[\\].]c')]";
        var document = JsonNode.Parse( 
"""
["abc","a.c","a\u2028c","a]c"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["a.c","a]c"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, explicit caret")]
    public void Test_FunctionsMatchExplicitCaret_Number381()
    {
        var selector = "$[?match(@, '^ab.*')]";
        var document = JsonNode.Parse( 
"""
["abc","axc","ab","xab"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["abc","ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, match, explicit dollar")]
    public void Test_FunctionsMatchExplicitDollar_Number382()
    {
        var selector = "$[?match(@, '.*bc$')]";
        var document = JsonNode.Parse( 
"""
["abc","axc","ab","abcx"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["abc"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, at the end")]
    public void Test_FunctionsSearchAtTheEnd_Number383()
    {
        var selector = "$[?search(@.a, 'a.*')]";
        var document = JsonNode.Parse( 
"""
[{"a":"the end is ab"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"the end is ab"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, double quotes")]
    public void Test_FunctionsSearchDoubleQuotes_Number384()
    {
        var selector = "$[?search(@.a, \"a.*\")]";
        var document = JsonNode.Parse( 
"""
[{"a":"the end is ab"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"the end is ab"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, at the start")]
    public void Test_FunctionsSearchAtTheStart_Number385()
    {
        var selector = "$[?search(@.a, 'a.*')]";
        var document = JsonNode.Parse( 
"""
[{"a":"ab is at the start"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"ab is at the start"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, in the middle")]
    public void Test_FunctionsSearchInTheMiddle_Number386()
    {
        var selector = "$[?search(@.a, 'a.*')]";
        var document = JsonNode.Parse( 
"""
[{"a":"contains two matches"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"contains two matches"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, regex from the document")]
    public void Test_FunctionsSearchRegexFromTheDocument_Number387()
    {
        var selector = "$.values[?search(@, $.regex)]";
        var document = JsonNode.Parse( 
"""
{"regex":"b.?b","values":["abc","bcd","bab","bba","bbab","b",true,[],{}]}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["bab","bba","bbab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, don't select match")]
    public void Test_FunctionsSearchDonTSelectMatch_Number388()
    {
        var selector = "$[?!search(@.a, 'a.*')]";
        var document = JsonNode.Parse( 
"""
[{"a":"contains two matches"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("functions, search, not a match")]
    public void Test_FunctionsSearchNotAMatch_Number389()
    {
        var selector = "$[?search(@.a, 'a.*')]";
        var document = JsonNode.Parse( 
"""
[{"a":"bc"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("functions, search, select non-match")]
    public void Test_FunctionsSearchSelectNonMatch_Number390()
    {
        var selector = "$[?!search(@.a, 'a.*')]";
        var document = JsonNode.Parse( 
"""
[{"a":"bc"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"bc"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, non-string first arg")]
    public void Test_FunctionsSearchNonStringFirstArg_Number391()
    {
        var selector = "$[?search(1, 'a.*')]";
        var document = JsonNode.Parse( 
"""
[{"a":"bc"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("functions, search, non-string second arg")]
    public void Test_FunctionsSearchNonStringSecondArg_Number392()
    {
        var selector = "$[?search(@.a, 1)]";
        var document = JsonNode.Parse( 
"""
[{"a":"bc"}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("functions, search, filter, search function, unicode char class, uppercase")]
    public void Test_FunctionsSearchFilterSearchFunctionUnicodeCharClassUppercase_Number393()
    {
        var selector = "$[?search(@, '\\p{Lu}')]";
        var document = JsonNode.Parse( 
"""
["ж","Ж","1","жЖ",true,[],{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["Ж","жЖ"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, filter, search function, unicode char class negated, uppercase")]
    public void Test_FunctionsSearchFilterSearchFunctionUnicodeCharClassNegatedUppercase_Number394()
    {
        var selector = "$[?search(@, '\\P{Lu}')]";
        var document = JsonNode.Parse( 
"""
["ж","Ж","1",true,[],{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ж","1"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, filter, search function, unicode, surrogate pair")]
    public void Test_FunctionsSearchFilterSearchFunctionUnicodeSurrogatePair_Number395()
    {
        var selector = "$[?search(@, 'a.b')]";
        var document = JsonNode.Parse( 
"""
["a𐄁bc","abc","1",true,[],{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["a𐄁bc"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, dot matcher on \u2028")]
    public void Test_FunctionsSearchDotMatcherOnU2028_Number396()
    {
        var selector = "$[?search(@, '.')]";
        var document = JsonNode.Parse( 
"""
["\u2028","\r\u2028\n","\r","\n",true,[],{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["\u2028","\r\u2028\n"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, dot matcher on \u2029")]
    public void Test_FunctionsSearchDotMatcherOnU2029_Number397()
    {
        var selector = "$[?search(@, '.')]";
        var document = JsonNode.Parse( 
"""
["\u2029","\r\u2029\n","\r","\n",true,[],{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["\u2029","\r\u2029\n"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, result cannot be compared")]
    public void Test_FunctionsSearchResultCannotBeCompared_Number398()
    {        
        var selector = "$[?search(@.a, 'a.*')==true]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, search, too few params")]
    public void Test_FunctionsSearchTooFewParams_Number399()
    {        
        var selector = "$[?search(@.a)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, search, too many params")]
    public void Test_FunctionsSearchTooManyParams_Number400()
    {        
        var selector = "$[?search(@.a,@.b,@.c)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, search, arg is a function expression")]
    public void Test_FunctionsSearchArgIsAFunctionExpression_Number401()
    {
        var selector = "$.values[?search(@, value($..['regex']))]";
        var document = JsonNode.Parse( 
"""
{"regex":"b.?b","values":["abc","bcd","bab","bba","bbab","b",true,[],{}]}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["bab","bba","bbab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, dot in character class")]
    public void Test_FunctionsSearchDotInCharacterClass_Number402()
    {
        var selector = "$[?search(@, 'a[.b]c')]";
        var document = JsonNode.Parse( 
"""
["x abc y","x a.c y","x axc y"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["x abc y","x a.c y"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, escaped dot")]
    public void Test_FunctionsSearchEscapedDot_Number403()
    {
        var selector = "$[?search(@, 'a\\.c')]";
        var document = JsonNode.Parse( 
"""
["x abc y","x a.c y","x axc y"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["x a.c y"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, escaped backslash before dot")]
    public void Test_FunctionsSearchEscapedBackslashBeforeDot_Number404()
    {
        var selector = "$[?search(@, 'a\\\\.c')]";
        var document = JsonNode.Parse( 
"""
["x abc y","x a.c y","x axc y","x a\\\u2028c y"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["x a\\\u2028c y"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, escaped left square bracket")]
    public void Test_FunctionsSearchEscapedLeftSquareBracket_Number405()
    {
        var selector = "$[?search(@, 'a\\[.c')]";
        var document = JsonNode.Parse( 
"""
["x abc y","x a.c y","x a[\u2028c y"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["x a[\u2028c y"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, search, escaped right square bracket")]
    public void Test_FunctionsSearchEscapedRightSquareBracket_Number406()
    {
        var selector = "$[?search(@, 'a[\\].]c')]";
        var document = JsonNode.Parse( 
"""
["x abc y","x a.c y","x a\u2028c y","x a]c y"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["x a.c y","x a]c y"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, value, single-value nodelist")]
    public void Test_FunctionsValueSingleValueNodelist_Number407()
    {
        var selector = "$[?value(@.*)==4]";
        var document = JsonNode.Parse( 
"""
[[4],{"foo":4},[5],{"foo":5},4]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[[4],{"foo":4}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("functions, value, multi-value nodelist")]
    public void Test_FunctionsValueMultiValueNodelist_Number408()
    {
        var selector = "$[?value(@.*)==4]";
        var document = JsonNode.Parse( 
"""
[[4,4],{"foo":4,"bar":4}]
""");
        var results = document.Select(selector);
        Assert.Fail("missing results");
    }

    [TestMethod("functions, value, too few params")]
    public void Test_FunctionsValueTooFewParams_Number409()
    {        
        var selector = "$[?value()==4]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, value, too many params")]
    public void Test_FunctionsValueTooManyParams_Number410()
    {        
        var selector = "$[?value(@.a,@.b)==4]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("functions, value, result must be compared")]
    public void Test_FunctionsValueResultMustBeCompared_Number411()
    {        
        var selector = "$[?value(@.a)]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("whitespace, filter, space between question mark and expression")]
    public void Test_WhitespaceFilterSpaceBetweenQuestionMarkAndExpression_Number412()
    {
        var selector = "$[? @.a]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, filter, newline between question mark and expression")]
    public void Test_WhitespaceFilterNewlineBetweenQuestionMarkAndExpression_Number413()
    {
        var selector = "$[?\n@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, filter, tab between question mark and expression")]
    public void Test_WhitespaceFilterTabBetweenQuestionMarkAndExpression_Number414()
    {
        var selector = "$[?\t@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, filter, return between question mark and expression")]
    public void Test_WhitespaceFilterReturnBetweenQuestionMarkAndExpression_Number415()
    {
        var selector = "$[?\r@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, filter, space between question mark and parenthesized expression")]
    public void Test_WhitespaceFilterSpaceBetweenQuestionMarkAndParenthesizedExpression_Number416()
    {
        var selector = "$[? (@.a)]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, filter, newline between question mark and parenthesized expression")]
    public void Test_WhitespaceFilterNewlineBetweenQuestionMarkAndParenthesizedExpression_Number417()
    {
        var selector = "$[?\n(@.a)]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, filter, tab between question mark and parenthesized expression")]
    public void Test_WhitespaceFilterTabBetweenQuestionMarkAndParenthesizedExpression_Number418()
    {
        var selector = "$[?\t(@.a)]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, filter, return between question mark and parenthesized expression")]
    public void Test_WhitespaceFilterReturnBetweenQuestionMarkAndParenthesizedExpression_Number419()
    {
        var selector = "$[?\r(@.a)]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, filter, space between parenthesized expression and bracket")]
    public void Test_WhitespaceFilterSpaceBetweenParenthesizedExpressionAndBracket_Number420()
    {
        var selector = "$[?(@.a) ]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, filter, newline between parenthesized expression and bracket")]
    public void Test_WhitespaceFilterNewlineBetweenParenthesizedExpressionAndBracket_Number421()
    {
        var selector = "$[?(@.a)\n]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, filter, tab between parenthesized expression and bracket")]
    public void Test_WhitespaceFilterTabBetweenParenthesizedExpressionAndBracket_Number422()
    {
        var selector = "$[?(@.a)\t]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, filter, return between parenthesized expression and bracket")]
    public void Test_WhitespaceFilterReturnBetweenParenthesizedExpressionAndBracket_Number423()
    {
        var selector = "$[?(@.a)\r]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, filter, space between bracket and question mark")]
    public void Test_WhitespaceFilterSpaceBetweenBracketAndQuestionMark_Number424()
    {
        var selector = "$[ ?@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, filter, newline between bracket and question mark")]
    public void Test_WhitespaceFilterNewlineBetweenBracketAndQuestionMark_Number425()
    {
        var selector = "$[\n?@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, filter, tab between bracket and question mark")]
    public void Test_WhitespaceFilterTabBetweenBracketAndQuestionMark_Number426()
    {
        var selector = "$[\t?@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, filter, return between bracket and question mark")]
    public void Test_WhitespaceFilterReturnBetweenBracketAndQuestionMark_Number427()
    {
        var selector = "$[\r?@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":"b","d":"e"},{"b":"c","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"b","d":"e"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, space between function name and parenthesis")]
    public void Test_WhitespaceFunctionsSpaceBetweenFunctionNameAndParenthesis_Number428()
    {        
        var selector = "$[?count (@.*)==1]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("whitespace, functions, newline between function name and parenthesis")]
    public void Test_WhitespaceFunctionsNewlineBetweenFunctionNameAndParenthesis_Number429()
    {        
        var selector = "$[?count\n(@.*)==1]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("whitespace, functions, tab between function name and parenthesis")]
    public void Test_WhitespaceFunctionsTabBetweenFunctionNameAndParenthesis_Number430()
    {        
        var selector = "$[?count\t(@.*)==1]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("whitespace, functions, return between function name and parenthesis")]
    public void Test_WhitespaceFunctionsReturnBetweenFunctionNameAndParenthesis_Number431()
    {        
        var selector = "$[?count\r(@.*)==1]";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("whitespace, functions, space between parenthesis and arg")]
    public void Test_WhitespaceFunctionsSpaceBetweenParenthesisAndArg_Number432()
    {
        var selector = "$[?count( @.*)==1]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, newline between parenthesis and arg")]
    public void Test_WhitespaceFunctionsNewlineBetweenParenthesisAndArg_Number433()
    {
        var selector = "$[?count(\n@.*)==1]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, tab between parenthesis and arg")]
    public void Test_WhitespaceFunctionsTabBetweenParenthesisAndArg_Number434()
    {
        var selector = "$[?count(\t@.*)==1]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, return between parenthesis and arg")]
    public void Test_WhitespaceFunctionsReturnBetweenParenthesisAndArg_Number435()
    {
        var selector = "$[?count(\r@.*)==1]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, space between arg and comma")]
    public void Test_WhitespaceFunctionsSpaceBetweenArgAndComma_Number436()
    {
        var selector = "$[?search(@ ,'[a-z]+')]";
        var document = JsonNode.Parse( 
"""
["foo","123"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["foo"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, newline between arg and comma")]
    public void Test_WhitespaceFunctionsNewlineBetweenArgAndComma_Number437()
    {
        var selector = "$[?search(@\n,'[a-z]+')]";
        var document = JsonNode.Parse( 
"""
["foo","123"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["foo"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, tab between arg and comma")]
    public void Test_WhitespaceFunctionsTabBetweenArgAndComma_Number438()
    {
        var selector = "$[?search(@\t,'[a-z]+')]";
        var document = JsonNode.Parse( 
"""
["foo","123"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["foo"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, return between arg and comma")]
    public void Test_WhitespaceFunctionsReturnBetweenArgAndComma_Number439()
    {
        var selector = "$[?search(@\r,'[a-z]+')]";
        var document = JsonNode.Parse( 
"""
["foo","123"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["foo"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, space between comma and arg")]
    public void Test_WhitespaceFunctionsSpaceBetweenCommaAndArg_Number440()
    {
        var selector = "$[?search(@, '[a-z]+')]";
        var document = JsonNode.Parse( 
"""
["foo","123"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["foo"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, newline between comma and arg")]
    public void Test_WhitespaceFunctionsNewlineBetweenCommaAndArg_Number441()
    {
        var selector = "$[?search(@,\n'[a-z]+')]";
        var document = JsonNode.Parse( 
"""
["foo","123"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["foo"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, tab between comma and arg")]
    public void Test_WhitespaceFunctionsTabBetweenCommaAndArg_Number442()
    {
        var selector = "$[?search(@,\t'[a-z]+')]";
        var document = JsonNode.Parse( 
"""
["foo","123"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["foo"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, return between comma and arg")]
    public void Test_WhitespaceFunctionsReturnBetweenCommaAndArg_Number443()
    {
        var selector = "$[?search(@,\r'[a-z]+')]";
        var document = JsonNode.Parse( 
"""
["foo","123"]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["foo"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, space between arg and parenthesis")]
    public void Test_WhitespaceFunctionsSpaceBetweenArgAndParenthesis_Number444()
    {
        var selector = "$[?count(@.* )==1]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, newline between arg and parenthesis")]
    public void Test_WhitespaceFunctionsNewlineBetweenArgAndParenthesis_Number445()
    {
        var selector = "$[?count(@.*\n)==1]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, tab between arg and parenthesis")]
    public void Test_WhitespaceFunctionsTabBetweenArgAndParenthesis_Number446()
    {
        var selector = "$[?count(@.*\t)==1]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, return between arg and parenthesis")]
    public void Test_WhitespaceFunctionsReturnBetweenArgAndParenthesis_Number447()
    {
        var selector = "$[?count(@.*\r)==1]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, spaces in a relative singular selector")]
    public void Test_WhitespaceFunctionsSpacesInARelativeSingularSelector_Number448()
    {
        var selector = "$[?length(@ .a .b) == 3]";
        var document = JsonNode.Parse( 
"""
[{"a":{"b":"foo"}},{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":{"b":"foo"}}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, newlines in a relative singular selector")]
    public void Test_WhitespaceFunctionsNewlinesInARelativeSingularSelector_Number449()
    {
        var selector = "$[?length(@\n.a\n.b) == 3]";
        var document = JsonNode.Parse( 
"""
[{"a":{"b":"foo"}},{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":{"b":"foo"}}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, tabs in a relative singular selector")]
    public void Test_WhitespaceFunctionsTabsInARelativeSingularSelector_Number450()
    {
        var selector = "$[?length(@\t.a\t.b) == 3]";
        var document = JsonNode.Parse( 
"""
[{"a":{"b":"foo"}},{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":{"b":"foo"}}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, returns in a relative singular selector")]
    public void Test_WhitespaceFunctionsReturnsInARelativeSingularSelector_Number451()
    {
        var selector = "$[?length(@\r.a\r.b) == 3]";
        var document = JsonNode.Parse( 
"""
[{"a":{"b":"foo"}},{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":{"b":"foo"}}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, spaces in an absolute singular selector")]
    public void Test_WhitespaceFunctionsSpacesInAnAbsoluteSingularSelector_Number452()
    {
        var selector = "$..[?length(@)==length($ [0] .a)]";
        var document = JsonNode.Parse( 
"""
[{"a":"foo"},{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["foo"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, newlines in an absolute singular selector")]
    public void Test_WhitespaceFunctionsNewlinesInAnAbsoluteSingularSelector_Number453()
    {
        var selector = "$..[?length(@)==length($\n[0]\n.a)]";
        var document = JsonNode.Parse( 
"""
[{"a":"foo"},{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["foo"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, tabs in an absolute singular selector")]
    public void Test_WhitespaceFunctionsTabsInAnAbsoluteSingularSelector_Number454()
    {
        var selector = "$..[?length(@)==length($\t[0]\t.a)]";
        var document = JsonNode.Parse( 
"""
[{"a":"foo"},{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["foo"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, functions, returns in an absolute singular selector")]
    public void Test_WhitespaceFunctionsReturnsInAnAbsoluteSingularSelector_Number455()
    {
        var selector = "$..[?length(@)==length($\r[0]\r.a)]";
        var document = JsonNode.Parse( 
"""
[{"a":"foo"},{}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["foo"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space before ||")]
    public void Test_WhitespaceOperatorsSpaceBefore_Number456()
    {
        var selector = "$[?@.a ||@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"c":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline before ||")]
    public void Test_WhitespaceOperatorsNewlineBefore_Number457()
    {
        var selector = "$[?@.a\n||@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"c":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab before ||")]
    public void Test_WhitespaceOperatorsTabBefore_Number458()
    {
        var selector = "$[?@.a\t||@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"c":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return before ||")]
    public void Test_WhitespaceOperatorsReturnBefore_Number459()
    {
        var selector = "$[?@.a\r||@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"c":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space after ||")]
    public void Test_WhitespaceOperatorsSpaceAfter_Number460()
    {
        var selector = "$[?@.a|| @.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"c":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline after ||")]
    public void Test_WhitespaceOperatorsNewlineAfter_Number461()
    {
        var selector = "$[?@.a||\n@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"c":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab after ||")]
    public void Test_WhitespaceOperatorsTabAfter_Number462()
    {
        var selector = "$[?@.a||\t@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"c":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return after ||")]
    public void Test_WhitespaceOperatorsReturnAfter_Number463()
    {
        var selector = "$[?@.a||\r@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"c":3}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1},{"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space before &&")]
    public void Test_WhitespaceOperatorsSpaceBefore_Number464()
    {
        var selector = "$[?@.a &&@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline before &&")]
    public void Test_WhitespaceOperatorsNewlineBefore_Number465()
    {
        var selector = "$[?@.a\n&&@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab before &&")]
    public void Test_WhitespaceOperatorsTabBefore_Number466()
    {
        var selector = "$[?@.a\t&&@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return before &&")]
    public void Test_WhitespaceOperatorsReturnBefore_Number467()
    {
        var selector = "$[?@.a\r&&@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space after &&")]
    public void Test_WhitespaceOperatorsSpaceAfter_Number468()
    {
        var selector = "$[?@.a&& @.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline after &&")]
    public void Test_WhitespaceOperatorsNewlineAfter_Number469()
    {
        var selector = "$[?@.a&& @.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab after &&")]
    public void Test_WhitespaceOperatorsTabAfter_Number470()
    {
        var selector = "$[?@.a&& @.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return after &&")]
    public void Test_WhitespaceOperatorsReturnAfter_Number471()
    {
        var selector = "$[?@.a&& @.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1},{"b":2},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space before ==")]
    public void Test_WhitespaceOperatorsSpaceBefore_Number472()
    {
        var selector = "$[?@.a ==@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline before ==")]
    public void Test_WhitespaceOperatorsNewlineBefore_Number473()
    {
        var selector = "$[?@.a\n==@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab before ==")]
    public void Test_WhitespaceOperatorsTabBefore_Number474()
    {
        var selector = "$[?@.a\t==@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return before ==")]
    public void Test_WhitespaceOperatorsReturnBefore_Number475()
    {
        var selector = "$[?@.a\r==@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space after ==")]
    public void Test_WhitespaceOperatorsSpaceAfter_Number476()
    {
        var selector = "$[?@.a== @.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline after ==")]
    public void Test_WhitespaceOperatorsNewlineAfter_Number477()
    {
        var selector = "$[?@.a==\n@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab after ==")]
    public void Test_WhitespaceOperatorsTabAfter_Number478()
    {
        var selector = "$[?@.a==\t@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return after ==")]
    public void Test_WhitespaceOperatorsReturnAfter_Number479()
    {
        var selector = "$[?@.a==\r@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space before !=")]
    public void Test_WhitespaceOperatorsSpaceBefore_Number480()
    {
        var selector = "$[?@.a !=@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline before !=")]
    public void Test_WhitespaceOperatorsNewlineBefore_Number481()
    {
        var selector = "$[?@.a\n!=@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab before !=")]
    public void Test_WhitespaceOperatorsTabBefore_Number482()
    {
        var selector = "$[?@.a\t!=@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return before !=")]
    public void Test_WhitespaceOperatorsReturnBefore_Number483()
    {
        var selector = "$[?@.a\r!=@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space after !=")]
    public void Test_WhitespaceOperatorsSpaceAfter_Number484()
    {
        var selector = "$[?@.a!= @.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline after !=")]
    public void Test_WhitespaceOperatorsNewlineAfter_Number485()
    {
        var selector = "$[?@.a!=\n@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab after !=")]
    public void Test_WhitespaceOperatorsTabAfter_Number486()
    {
        var selector = "$[?@.a!=\t@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return after !=")]
    public void Test_WhitespaceOperatorsReturnAfter_Number487()
    {
        var selector = "$[?@.a!=\r@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space before <")]
    public void Test_WhitespaceOperatorsSpaceBefore_Number488()
    {
        var selector = "$[?@.a <@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline before <")]
    public void Test_WhitespaceOperatorsNewlineBefore_Number489()
    {
        var selector = "$[?@.a\n<@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab before <")]
    public void Test_WhitespaceOperatorsTabBefore_Number490()
    {
        var selector = "$[?@.a\t<@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return before <")]
    public void Test_WhitespaceOperatorsReturnBefore_Number491()
    {
        var selector = "$[?@.a\r<@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space after <")]
    public void Test_WhitespaceOperatorsSpaceAfter_Number492()
    {
        var selector = "$[?@.a< @.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline after <")]
    public void Test_WhitespaceOperatorsNewlineAfter_Number493()
    {
        var selector = "$[?@.a<\n@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab after <")]
    public void Test_WhitespaceOperatorsTabAfter_Number494()
    {
        var selector = "$[?@.a<\t@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return after <")]
    public void Test_WhitespaceOperatorsReturnAfter_Number495()
    {
        var selector = "$[?@.a<\r@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space before >")]
    public void Test_WhitespaceOperatorsSpaceBefore_Number496()
    {
        var selector = "$[?@.b >@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline before >")]
    public void Test_WhitespaceOperatorsNewlineBefore_Number497()
    {
        var selector = "$[?@.b\n>@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab before >")]
    public void Test_WhitespaceOperatorsTabBefore_Number498()
    {
        var selector = "$[?@.b\t>@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return before >")]
    public void Test_WhitespaceOperatorsReturnBefore_Number499()
    {
        var selector = "$[?@.b\r>@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space after >")]
    public void Test_WhitespaceOperatorsSpaceAfter_Number500()
    {
        var selector = "$[?@.b> @.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline after >")]
    public void Test_WhitespaceOperatorsNewlineAfter_Number501()
    {
        var selector = "$[?@.b>\n@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab after >")]
    public void Test_WhitespaceOperatorsTabAfter_Number502()
    {
        var selector = "$[?@.b>\t@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return after >")]
    public void Test_WhitespaceOperatorsReturnAfter_Number503()
    {
        var selector = "$[?@.b>\r@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space before <=")]
    public void Test_WhitespaceOperatorsSpaceBefore_Number504()
    {
        var selector = "$[?@.a <=@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline before <=")]
    public void Test_WhitespaceOperatorsNewlineBefore_Number505()
    {
        var selector = "$[?@.a\n<=@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab before <=")]
    public void Test_WhitespaceOperatorsTabBefore_Number506()
    {
        var selector = "$[?@.a\t<=@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return before <=")]
    public void Test_WhitespaceOperatorsReturnBefore_Number507()
    {
        var selector = "$[?@.a\r<=@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space after <=")]
    public void Test_WhitespaceOperatorsSpaceAfter_Number508()
    {
        var selector = "$[?@.a<= @.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline after <=")]
    public void Test_WhitespaceOperatorsNewlineAfter_Number509()
    {
        var selector = "$[?@.a<=\n@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab after <=")]
    public void Test_WhitespaceOperatorsTabAfter_Number510()
    {
        var selector = "$[?@.a<=\t@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return after <=")]
    public void Test_WhitespaceOperatorsReturnAfter_Number511()
    {
        var selector = "$[?@.a<=\r@.b]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space before >=")]
    public void Test_WhitespaceOperatorsSpaceBefore_Number512()
    {
        var selector = "$[?@.b >=@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline before >=")]
    public void Test_WhitespaceOperatorsNewlineBefore_Number513()
    {
        var selector = "$[?@.b\n>=@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab before >=")]
    public void Test_WhitespaceOperatorsTabBefore_Number514()
    {
        var selector = "$[?@.b\t>=@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return before >=")]
    public void Test_WhitespaceOperatorsReturnBefore_Number515()
    {
        var selector = "$[?@.b\r>=@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space after >=")]
    public void Test_WhitespaceOperatorsSpaceAfter_Number516()
    {
        var selector = "$[?@.b>= @.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline after >=")]
    public void Test_WhitespaceOperatorsNewlineAfter_Number517()
    {
        var selector = "$[?@.b>=\n@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab after >=")]
    public void Test_WhitespaceOperatorsTabAfter_Number518()
    {
        var selector = "$[?@.b>=\t@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return after >=")]
    public void Test_WhitespaceOperatorsReturnAfter_Number519()
    {
        var selector = "$[?@.b>=\r@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":1,"b":1},{"a":1,"b":2}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space between logical not and test expression")]
    public void Test_WhitespaceOperatorsSpaceBetweenLogicalNotAndTestExpression_Number520()
    {
        var selector = "$[?! @.a]";
        var document = JsonNode.Parse( 
"""
[{"a":"a","d":"e"},{"d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline between logical not and test expression")]
    public void Test_WhitespaceOperatorsNewlineBetweenLogicalNotAndTestExpression_Number521()
    {
        var selector = "$[?!\n@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":"a","d":"e"},{"d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab between logical not and test expression")]
    public void Test_WhitespaceOperatorsTabBetweenLogicalNotAndTestExpression_Number522()
    {
        var selector = "$[?!\t@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":"a","d":"e"},{"d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return between logical not and test expression")]
    public void Test_WhitespaceOperatorsReturnBetweenLogicalNotAndTestExpression_Number523()
    {
        var selector = "$[?!\r@.a]";
        var document = JsonNode.Parse( 
"""
[{"a":"a","d":"e"},{"d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, space between logical not and parenthesized expression")]
    public void Test_WhitespaceOperatorsSpaceBetweenLogicalNotAndParenthesizedExpression_Number524()
    {
        var selector = "$[?! (@.a=='b')]";
        var document = JsonNode.Parse( 
"""
[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"a","d":"e"},{"a":"d","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, newline between logical not and parenthesized expression")]
    public void Test_WhitespaceOperatorsNewlineBetweenLogicalNotAndParenthesizedExpression_Number525()
    {
        var selector = "$[?!\n(@.a=='b')]";
        var document = JsonNode.Parse( 
"""
[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"a","d":"e"},{"a":"d","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, tab between logical not and parenthesized expression")]
    public void Test_WhitespaceOperatorsTabBetweenLogicalNotAndParenthesizedExpression_Number526()
    {
        var selector = "$[?!\t(@.a=='b')]";
        var document = JsonNode.Parse( 
"""
[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"a","d":"e"},{"a":"d","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, operators, return between logical not and parenthesized expression")]
    public void Test_WhitespaceOperatorsReturnBetweenLogicalNotAndParenthesizedExpression_Number527()
    {
        var selector = "$[?!\r(@.a=='b')]";
        var document = JsonNode.Parse( 
"""
[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"d","d":"f"}]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[{"a":"a","d":"e"},{"a":"d","d":"f"}]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, space between root and bracket")]
    public void Test_WhitespaceSelectorsSpaceBetweenRootAndBracket_Number528()
    {
        var selector = "$ ['a']";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, newline between root and bracket")]
    public void Test_WhitespaceSelectorsNewlineBetweenRootAndBracket_Number529()
    {
        var selector = "$\n['a']";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, tab between root and bracket")]
    public void Test_WhitespaceSelectorsTabBetweenRootAndBracket_Number530()
    {
        var selector = "$\t['a']";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, return between root and bracket")]
    public void Test_WhitespaceSelectorsReturnBetweenRootAndBracket_Number531()
    {
        var selector = "$\r['a']";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, space between bracket and bracket")]
    public void Test_WhitespaceSelectorsSpaceBetweenBracketAndBracket_Number532()
    {
        var selector = "$['a'] ['b']";
        var document = JsonNode.Parse( 
"""
{"a":{"b":"ab"}}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, newline between root and bracket")]
    public void Test_WhitespaceSelectorsNewlineBetweenRootAndBracket_Number533()
    {
        var selector = "$['a'] \n['b']";
        var document = JsonNode.Parse( 
"""
{"a":{"b":"ab"}}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, tab between root and bracket")]
    public void Test_WhitespaceSelectorsTabBetweenRootAndBracket_Number534()
    {
        var selector = "$['a'] \t['b']";
        var document = JsonNode.Parse( 
"""
{"a":{"b":"ab"}}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, return between root and bracket")]
    public void Test_WhitespaceSelectorsReturnBetweenRootAndBracket_Number535()
    {
        var selector = "$['a'] \r['b']";
        var document = JsonNode.Parse( 
"""
{"a":{"b":"ab"}}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, space between root and dot")]
    public void Test_WhitespaceSelectorsSpaceBetweenRootAndDot_Number536()
    {
        var selector = "$ .a";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, newline between root and dot")]
    public void Test_WhitespaceSelectorsNewlineBetweenRootAndDot_Number537()
    {
        var selector = "$\n.a";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, tab between root and dot")]
    public void Test_WhitespaceSelectorsTabBetweenRootAndDot_Number538()
    {
        var selector = "$\t.a";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, return between root and dot")]
    public void Test_WhitespaceSelectorsReturnBetweenRootAndDot_Number539()
    {
        var selector = "$\r.a";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, space between dot and name")]
    public void Test_WhitespaceSelectorsSpaceBetweenDotAndName_Number540()
    {        
        var selector = "$. a";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("whitespace, selectors, newline between dot and name")]
    public void Test_WhitespaceSelectorsNewlineBetweenDotAndName_Number541()
    {        
        var selector = "$.\na";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("whitespace, selectors, tab between dot and name")]
    public void Test_WhitespaceSelectorsTabBetweenDotAndName_Number542()
    {        
        var selector = "$.\ta";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("whitespace, selectors, return between dot and name")]
    public void Test_WhitespaceSelectorsReturnBetweenDotAndName_Number543()
    {        
        var selector = "$.\ra";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("whitespace, selectors, space between recursive descent and name")]
    public void Test_WhitespaceSelectorsSpaceBetweenRecursiveDescentAndName_Number544()
    {        
        var selector = "$.. a";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("whitespace, selectors, newline between recursive descent and name")]
    public void Test_WhitespaceSelectorsNewlineBetweenRecursiveDescentAndName_Number545()
    {        
        var selector = "$..\na";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("whitespace, selectors, tab between recursive descent and name")]
    public void Test_WhitespaceSelectorsTabBetweenRecursiveDescentAndName_Number546()
    {        
        var selector = "$..\ta";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("whitespace, selectors, return between recursive descent and name")]
    public void Test_WhitespaceSelectorsReturnBetweenRecursiveDescentAndName_Number547()
    {        
        var selector = "$..\ra";
        var document = new JsonObject(); // Empty node
        Assert.ThrowsException<NotSupportedException>(() => document.Select(selector));
    }

    [TestMethod("whitespace, selectors, space between bracket and selector")]
    public void Test_WhitespaceSelectorsSpaceBetweenBracketAndSelector_Number548()
    {
        var selector = "$[ 'a']";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, newline between bracket and selector")]
    public void Test_WhitespaceSelectorsNewlineBetweenBracketAndSelector_Number549()
    {
        var selector = "$[\n'a']";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, tab between bracket and selector")]
    public void Test_WhitespaceSelectorsTabBetweenBracketAndSelector_Number550()
    {
        var selector = "$[\t'a']";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, return between bracket and selector")]
    public void Test_WhitespaceSelectorsReturnBetweenBracketAndSelector_Number551()
    {
        var selector = "$[\r'a']";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, space between selector and bracket")]
    public void Test_WhitespaceSelectorsSpaceBetweenSelectorAndBracket_Number552()
    {
        var selector = "$['a' ]";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, newline between selector and bracket")]
    public void Test_WhitespaceSelectorsNewlineBetweenSelectorAndBracket_Number553()
    {
        var selector = "$['a'\n]";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, tab between selector and bracket")]
    public void Test_WhitespaceSelectorsTabBetweenSelectorAndBracket_Number554()
    {
        var selector = "$['a'\t]";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, return between selector and bracket")]
    public void Test_WhitespaceSelectorsReturnBetweenSelectorAndBracket_Number555()
    {
        var selector = "$['a'\r]";
        var document = JsonNode.Parse( 
"""
{"a":"ab"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, space between selector and comma")]
    public void Test_WhitespaceSelectorsSpaceBetweenSelectorAndComma_Number556()
    {
        var selector = "$['a' ,'b']";
        var document = JsonNode.Parse( 
"""
{"a":"ab","b":"bc"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab","bc"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, newline between selector and comma")]
    public void Test_WhitespaceSelectorsNewlineBetweenSelectorAndComma_Number557()
    {
        var selector = "$['a'\n,'b']";
        var document = JsonNode.Parse( 
"""
{"a":"ab","b":"bc"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab","bc"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, tab between selector and comma")]
    public void Test_WhitespaceSelectorsTabBetweenSelectorAndComma_Number558()
    {
        var selector = "$['a'\t,'b']";
        var document = JsonNode.Parse( 
"""
{"a":"ab","b":"bc"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab","bc"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, return between selector and comma")]
    public void Test_WhitespaceSelectorsReturnBetweenSelectorAndComma_Number559()
    {
        var selector = "$['a'\r,'b']";
        var document = JsonNode.Parse( 
"""
{"a":"ab","b":"bc"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab","bc"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, space between comma and selector")]
    public void Test_WhitespaceSelectorsSpaceBetweenCommaAndSelector_Number560()
    {
        var selector = "$['a', 'b']";
        var document = JsonNode.Parse( 
"""
{"a":"ab","b":"bc"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab","bc"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, newline between comma and selector")]
    public void Test_WhitespaceSelectorsNewlineBetweenCommaAndSelector_Number561()
    {
        var selector = "$['a',\n'b']";
        var document = JsonNode.Parse( 
"""
{"a":"ab","b":"bc"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab","bc"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, tab between comma and selector")]
    public void Test_WhitespaceSelectorsTabBetweenCommaAndSelector_Number562()
    {
        var selector = "$['a',\t'b']";
        var document = JsonNode.Parse( 
"""
{"a":"ab","b":"bc"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab","bc"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, selectors, return between comma and selector")]
    public void Test_WhitespaceSelectorsReturnBetweenCommaAndSelector_Number563()
    {
        var selector = "$['a',\r'b']";
        var document = JsonNode.Parse( 
"""
{"a":"ab","b":"bc"}
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
["ab","bc"]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, space between start and colon")]
    public void Test_WhitespaceSliceSpaceBetweenStartAndColon_Number564()
    {
        var selector = "$[1 :5:2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, newline between start and colon")]
    public void Test_WhitespaceSliceNewlineBetweenStartAndColon_Number565()
    {
        var selector = "$[1\n:5:2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, tab between start and colon")]
    public void Test_WhitespaceSliceTabBetweenStartAndColon_Number566()
    {
        var selector = "$[1\t:5:2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, return between start and colon")]
    public void Test_WhitespaceSliceReturnBetweenStartAndColon_Number567()
    {
        var selector = "$[1\r:5:2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, space between colon and end")]
    public void Test_WhitespaceSliceSpaceBetweenColonAndEnd_Number568()
    {
        var selector = "$[1: 5:2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, newline between colon and end")]
    public void Test_WhitespaceSliceNewlineBetweenColonAndEnd_Number569()
    {
        var selector = "$[1:\n5:2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, tab between colon and end")]
    public void Test_WhitespaceSliceTabBetweenColonAndEnd_Number570()
    {
        var selector = "$[1:\t5:2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, return between colon and end")]
    public void Test_WhitespaceSliceReturnBetweenColonAndEnd_Number571()
    {
        var selector = "$[1:\r5:2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, space between end and colon")]
    public void Test_WhitespaceSliceSpaceBetweenEndAndColon_Number572()
    {
        var selector = "$[1:5 :2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, newline between end and colon")]
    public void Test_WhitespaceSliceNewlineBetweenEndAndColon_Number573()
    {
        var selector = "$[1:5\n:2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, tab between end and colon")]
    public void Test_WhitespaceSliceTabBetweenEndAndColon_Number574()
    {
        var selector = "$[1:5\t:2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, return between end and colon")]
    public void Test_WhitespaceSliceReturnBetweenEndAndColon_Number575()
    {
        var selector = "$[1:5\r:2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, space between colon and step")]
    public void Test_WhitespaceSliceSpaceBetweenColonAndStep_Number576()
    {
        var selector = "$[1:5: 2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, newline between colon and step")]
    public void Test_WhitespaceSliceNewlineBetweenColonAndStep_Number577()
    {
        var selector = "$[1:5:\n2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, tab between colon and step")]
    public void Test_WhitespaceSliceTabBetweenColonAndStep_Number578()
    {
        var selector = "$[1:5:\t2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

    [TestMethod("whitespace, slice, return between colon and step")]
    public void Test_WhitespaceSliceReturnBetweenColonAndStep_Number579()
    {
        var selector = "$[1:5:\r2]";
        var document = JsonNode.Parse( 
"""
[1,2,3,4,5,6]
""");
        var results = document.Select(selector);
        var expected = JsonNode.Parse(
"""
[2,4]
""");

        var count = 0;
        foreach (var result in results)
        {
            Assert.IsTrue(JsonNode.DeepEquals(expected![count], result));
            count++;
        }
    }

}
