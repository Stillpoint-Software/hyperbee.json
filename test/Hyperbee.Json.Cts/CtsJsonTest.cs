using System;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Cts
{
    [TestClass]
    public class CtsJsonTest
    {



        // unit-test-ref: "basic, root"
        [TestMethod]
        public void Test_basic__root_Number1()
        {
            var selector = @"$";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[["first","second"]]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, no leading whitespace"
        [TestMethod]
        public void Test_basic__no_leading_whitespace_Number2()
        {
            var selector = @" $";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "basic, no trailing whitespace"
        [TestMethod]
        public void Test_basic__no_trailing_whitespace_Number3()
        {
            var selector = @"$ ";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "basic, name shorthand"
        [TestMethod]
        public void Test_basic__name_shorthand_Number4()
        {
            var selector = @"$.a";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, name shorthand, extended unicode ☺"
        [TestMethod]
        public void Test_basic__name_shorthand__extended_unicode___Number5()
        {
            var selector = @"$.☺";
            var document = JsonNode.Parse(
                """{"☺":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, name shorthand, underscore"
        [TestMethod]
        public void Test_basic__name_shorthand__underscore_Number6()
        {
            var selector = @"$._";
            var document = JsonNode.Parse(
                """{"_":"A","_foo":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, name shorthand, symbol"
        [TestMethod]
        public void Test_basic__name_shorthand__symbol_Number7()
        {
            var selector = @"$.&";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "basic, name shorthand, number"
        [TestMethod]
        public void Test_basic__name_shorthand__number_Number8()
        {
            var selector = @"$.1";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "basic, name shorthand, absent data"
        [TestMethod]
        public void Test_basic__name_shorthand__absent_data_Number9()
        {
            var selector = @"$.c";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, name shorthand, array data"
        [TestMethod]
        public void Test_basic__name_shorthand__array_data_Number10()
        {
            var selector = @"$.a";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, wildcard shorthand, object data"
        [TestMethod]
        public void Test_basic__wildcard_shorthand__object_data_Number11()
        {
            var selector = @"$.*";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[["A","B"],["B","A"]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, wildcard shorthand, array data"
        [TestMethod]
        public void Test_basic__wildcard_shorthand__array_data_Number12()
        {
            var selector = @"$.*";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["first","second"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, wildcard selector, array data"
        [TestMethod]
        public void Test_basic__wildcard_selector__array_data_Number13()
        {
            var selector = @"$[*]";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["first","second"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, wildcard shorthand, then name shorthand"
        [TestMethod]
        public void Test_basic__wildcard_shorthand__then_name_shorthand_Number14()
        {
            var selector = @"$.*.a";
            var document = JsonNode.Parse(
                """{"x":{"a":"Ax","b":"Bx"},"y":{"a":"Ay","b":"By"}}""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[["Ax","Ay"],["Ay","Ax"]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, multiple selectors"
        [TestMethod]
        public void Test_basic__multiple_selectors_Number15()
        {
            var selector = @"$[0,2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,2]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, multiple selectors, space instead of comma"
        [TestMethod]
        public void Test_basic__multiple_selectors__space_instead_of_comma_Number16()
        {
            var selector = @"$[0 2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "basic, multiple selectors, name and index, array data"
        [TestMethod]
        public void Test_basic__multiple_selectors__name_and_index__array_data_Number17()
        {
            var selector = @"$['a',1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, multiple selectors, name and index, object data"
        [TestMethod]
        public void Test_basic__multiple_selectors__name_and_index__object_data_Number18()
        {
            var selector = @"$['a',1]";
            var document = JsonNode.Parse(
                """{"a":1,"b":2}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, multiple selectors, index and slice"
        [TestMethod]
        public void Test_basic__multiple_selectors__index_and_slice_Number19()
        {
            var selector = @"$[1,5:7]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,5,6]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, multiple selectors, index and slice, overlapping"
        [TestMethod]
        public void Test_basic__multiple_selectors__index_and_slice__overlapping_Number20()
        {
            var selector = @"$[1,0:3]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,0,1,2]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, multiple selectors, duplicate index"
        [TestMethod]
        public void Test_basic__multiple_selectors__duplicate_index_Number21()
        {
            var selector = @"$[1,1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,1]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, multiple selectors, wildcard and index"
        [TestMethod]
        public void Test_basic__multiple_selectors__wildcard_and_index_Number22()
        {
            var selector = @"$[*,1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9,1]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, multiple selectors, wildcard and name"
        [TestMethod]
        public void Test_basic__multiple_selectors__wildcard_and_name_Number23()
        {
            var selector = @"$[*,'a']";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[["A","B","A"],["B","A","A"]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, multiple selectors, wildcard and slice"
        [TestMethod]
        public void Test_basic__multiple_selectors__wildcard_and_slice_Number24()
        {
            var selector = @"$[*,0:2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9,0,1]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, multiple selectors, multiple wildcards"
        [TestMethod]
        public void Test_basic__multiple_selectors__multiple_wildcards_Number25()
        {
            var selector = @"$[*,*]";
            var document = JsonNode.Parse(
                """[0,1,2]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1,2,0,1,2]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, empty segment"
        [TestMethod]
        public void Test_basic__empty_segment_Number26()
        {
            var selector = @"$[]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "basic, descendant segment, index"
        [TestMethod]
        public void Test_basic__descendant_segment__index_Number27()
        {
            var selector = @"$..[1]";
            var document = JsonNode.Parse(
                """{"o":[0,1,[2,3]]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,3]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, descendant segment, name shorthand"
        [TestMethod]
        public void Test_basic__descendant_segment__name_shorthand_Number28()
        {
            var selector = @"$..a";
            var document = JsonNode.Parse(
                """{"o":[{"a":"b"},{"a":"c"}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["b","c"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, descendant segment, wildcard shorthand, array data"
        [TestMethod]
        public void Test_basic__descendant_segment__wildcard_shorthand__array_data_Number29()
        {
            var selector = @"$..*";
            var document = JsonNode.Parse(
                """[0,1]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, descendant segment, wildcard selector, array data"
        [TestMethod]
        public void Test_basic__descendant_segment__wildcard_selector__array_data_Number30()
        {
            var selector = @"$..[*]";
            var document = JsonNode.Parse(
                """[0,1]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, descendant segment, wildcard selector, nested arrays"
        [TestMethod]
        public void Test_basic__descendant_segment__wildcard_selector__nested_arrays_Number31()
        {
            var selector = @"$..[*]";
            var document = JsonNode.Parse(
                """[[[1]],[2]]""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[[[[1]],[2],[1],1,2],[[[1]],[2],[1],2,1]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, descendant segment, wildcard selector, nested objects"
        [TestMethod]
        public void Test_basic__descendant_segment__wildcard_selector__nested_objects_Number32()
        {
            var selector = @"$..[*]";
            var document = JsonNode.Parse(
                """{"a":{"c":{"e":1}},"b":{"d":2}}""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[[{"c":{"e":1}},{"d":2},{"e":1},1,2],[{"c":{"e":1}},{"d":2},{"e":1},2,1],[{"c":{"e":1}},{"d":2},2,{"e":1},1],[{"d":2},{"c":{"e":1}},{"e":1},1,2],[{"d":2},{"c":{"e":1}},{"e":1},2,1],[{"d":2},{"c":{"e":1}},2,{"e":1},1]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, descendant segment, wildcard shorthand, object data"
        [TestMethod]
        public void Test_basic__descendant_segment__wildcard_shorthand__object_data_Number33()
        {
            var selector = @"$..*";
            var document = JsonNode.Parse(
                """{"a":"b"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["b"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, descendant segment, wildcard shorthand, nested data"
        [TestMethod]
        public void Test_basic__descendant_segment__wildcard_shorthand__nested_data_Number34()
        {
            var selector = @"$..*";
            var document = JsonNode.Parse(
                """{"o":[{"a":"b"}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[[{"a":"b"}],{"a":"b"},"b"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, descendant segment, multiple selectors"
        [TestMethod]
        public void Test_basic__descendant_segment__multiple_selectors_Number35()
        {
            var selector = @"$..['a','d']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["b","e","c","f"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, descendant segment, object traversal, multiple selectors"
        [TestMethod]
        public void Test_basic__descendant_segment__object_traversal__multiple_selectors_Number36()
        {
            var selector = @"$..['a','d']";
            var document = JsonNode.Parse(
                """{"x":{"a":"b","d":"e"},"y":{"a":"c","d":"f"}}""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[["b","e","c","f"],["c","f","b","e"]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "basic, bald descendant segment"
        [TestMethod]
        public void Test_basic__bald_descendant_segment_Number37()
        {
            var selector = @"$..";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, existence, without segments"
        [TestMethod]
        public void Test_filter__existence__without_segments_Number38()
        {
            var selector = @"$[?@]";
            var document = JsonNode.Parse(
                """{"a":1,"b":null}""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[[1,null],[null,1]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, existence"
        [TestMethod]
        public void Test_filter__existence_Number39()
        {
            var selector = @"$[?@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, existence, present with null"
        [TestMethod]
        public void Test_filter__existence__present_with_null_Number40()
        {
            var selector = @"$[?@.a]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":null,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals string, single quotes"
        [TestMethod]
        public void Test_filter__equals_string__single_quotes_Number41()
        {
            var selector = @"$[?@.a=='b']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals numeric string, single quotes"
        [TestMethod]
        public void Test_filter__equals_numeric_string__single_quotes_Number42()
        {
            var selector = @"$[?@.a=='1']";
            var document = JsonNode.Parse(
                """[{"a":"1","d":"e"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"1","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals string, double quotes"
        [TestMethod]
        public void Test_filter__equals_string__double_quotes_Number43()
        {
            var selector = @"$[?@.a==\";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals numeric string, double quotes"
        [TestMethod]
        public void Test_filter__equals_numeric_string__double_quotes_Number44()
        {
            var selector = @"$[?@.a==\";
            var document = JsonNode.Parse(
                """[{"a":"1","d":"e"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"1","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals number"
        [TestMethod]
        public void Test_filter__equals_number_Number45()
        {
            var selector = @"$[?@.a==1]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":"c","d":"f"},{"a":2,"d":"f"},{"a":"1","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals null"
        [TestMethod]
        public void Test_filter__equals_null_Number46()
        {
            var selector = @"$[?@.a==null]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":null,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals null, absent from data"
        [TestMethod]
        public void Test_filter__equals_null__absent_from_data_Number47()
        {
            var selector = @"$[?@.a==null]";
            var document = JsonNode.Parse(
                """[{"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals true"
        [TestMethod]
        public void Test_filter__equals_true_Number48()
        {
            var selector = @"$[?@.a==true]";
            var document = JsonNode.Parse(
                """[{"a":true,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":true,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals false"
        [TestMethod]
        public void Test_filter__equals_false_Number49()
        {
            var selector = @"$[?@.a==false]";
            var document = JsonNode.Parse(
                """[{"a":false,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":false,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals self"
        [TestMethod]
        public void Test_filter__equals_self_Number50()
        {
            var selector = @"$[?@==@]";
            var document = JsonNode.Parse(
                """[1,null,true,{"a":"b"},[false]]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,null,true,{"a":"b"},[false]]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, deep equality, arrays"
        [TestMethod]
        public void Test_filter__deep_equality__arrays_Number51()
        {
            var selector = @"$[?@.a==@.b]";
            var document = JsonNode.Parse(
                """[{"a":false,"b":[1,2]},{"a":[[1,[2]]],"b":[[1,[2]]]},{"a":[[1,[2]]],"b":[[[2],1]]},{"a":[[1,[2]]],"b":[[1,2]]}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":[[1,[2]]],"b":[[1,[2]]]}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, deep equality, objects"
        [TestMethod]
        public void Test_filter__deep_equality__objects_Number52()
        {
            var selector = @"$[?@.a==@.b]";
            var document = JsonNode.Parse(
                """[{"a":false,"b":{"x":1,"y":{"z":1}}},{"a":{"x":1,"y":{"z":1}},"b":{"x":1,"y":{"z":1}}},{"a":{"x":1,"y":{"z":1}},"b":{"y":{"z":1},"x":1}},{"a":{"x":1,"y":{"z":1}},"b":{"x":1}},{"a":{"x":1,"y":{"z":1}},"b":{"x":1,"y":{"z":2}}}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":{"x":1,"y":{"z":1}},"b":{"x":1,"y":{"z":1}}},{"a":{"x":1,"y":{"z":1}},"b":{"y":{"z":1},"x":1}}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, not-equals string, single quotes"
        [TestMethod]
        public void Test_filter__not_equals_string__single_quotes_Number53()
        {
            var selector = @"$[?@.a!='b']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, not-equals numeric string, single quotes"
        [TestMethod]
        public void Test_filter__not_equals_numeric_string__single_quotes_Number54()
        {
            var selector = @"$[?@.a!='1']";
            var document = JsonNode.Parse(
                """[{"a":"1","d":"e"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, not-equals string, single quotes, different type"
        [TestMethod]
        public void Test_filter__not_equals_string__single_quotes__different_type_Number55()
        {
            var selector = @"$[?@.a!='b']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, not-equals string, double quotes"
        [TestMethod]
        public void Test_filter__not_equals_string__double_quotes_Number56()
        {
            var selector = @"$[?@.a!=\";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, not-equals numeric string, double quotes"
        [TestMethod]
        public void Test_filter__not_equals_numeric_string__double_quotes_Number57()
        {
            var selector = @"$[?@.a!=\";
            var document = JsonNode.Parse(
                """[{"a":"1","d":"e"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, not-equals string, double quotes, different types"
        [TestMethod]
        public void Test_filter__not_equals_string__double_quotes__different_types_Number58()
        {
            var selector = @"$[?@.a!=\";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, not-equals number"
        [TestMethod]
        public void Test_filter__not_equals_number_Number59()
        {
            var selector = @"$[?@.a!=1]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":2,"d":"f"},{"a":"1","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":2,"d":"f"},{"a":"1","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, not-equals number, different types"
        [TestMethod]
        public void Test_filter__not_equals_number__different_types_Number60()
        {
            var selector = @"$[?@.a!=1]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, not-equals null"
        [TestMethod]
        public void Test_filter__not_equals_null_Number61()
        {
            var selector = @"$[?@.a!=null]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, not-equals null, absent from data"
        [TestMethod]
        public void Test_filter__not_equals_null__absent_from_data_Number62()
        {
            var selector = @"$[?@.a!=null]";
            var document = JsonNode.Parse(
                """[{"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"d":"e"},{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, not-equals true"
        [TestMethod]
        public void Test_filter__not_equals_true_Number63()
        {
            var selector = @"$[?@.a!=true]";
            var document = JsonNode.Parse(
                """[{"a":true,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, not-equals false"
        [TestMethod]
        public void Test_filter__not_equals_false_Number64()
        {
            var selector = @"$[?@.a!=false]";
            var document = JsonNode.Parse(
                """[{"a":false,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, less than string, single quotes"
        [TestMethod]
        public void Test_filter__less_than_string__single_quotes_Number65()
        {
            var selector = @"$[?@.a<'c']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, less than string, double quotes"
        [TestMethod]
        public void Test_filter__less_than_string__double_quotes_Number66()
        {
            var selector = @"$[?@.a<\";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, less than number"
        [TestMethod]
        public void Test_filter__less_than_number_Number67()
        {
            var selector = @"$[?@.a<10]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":10,"d":"e"},{"a":"c","d":"f"},{"a":20,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, less than null"
        [TestMethod]
        public void Test_filter__less_than_null_Number68()
        {
            var selector = @"$[?@.a<null]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, less than true"
        [TestMethod]
        public void Test_filter__less_than_true_Number69()
        {
            var selector = @"$[?@.a<true]";
            var document = JsonNode.Parse(
                """[{"a":true,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, less than false"
        [TestMethod]
        public void Test_filter__less_than_false_Number70()
        {
            var selector = @"$[?@.a<false]";
            var document = JsonNode.Parse(
                """[{"a":false,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, less than or equal to string, single quotes"
        [TestMethod]
        public void Test_filter__less_than_or_equal_to_string__single_quotes_Number71()
        {
            var selector = @"$[?@.a<='c']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, less than or equal to string, double quotes"
        [TestMethod]
        public void Test_filter__less_than_or_equal_to_string__double_quotes_Number72()
        {
            var selector = @"$[?@.a<=\";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, less than or equal to number"
        [TestMethod]
        public void Test_filter__less_than_or_equal_to_number_Number73()
        {
            var selector = @"$[?@.a<=10]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":10,"d":"e"},{"a":"c","d":"f"},{"a":20,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":10,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, less than or equal to null"
        [TestMethod]
        public void Test_filter__less_than_or_equal_to_null_Number74()
        {
            var selector = @"$[?@.a<=null]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":null,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, less than or equal to true"
        [TestMethod]
        public void Test_filter__less_than_or_equal_to_true_Number75()
        {
            var selector = @"$[?@.a<=true]";
            var document = JsonNode.Parse(
                """[{"a":true,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":true,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, less than or equal to false"
        [TestMethod]
        public void Test_filter__less_than_or_equal_to_false_Number76()
        {
            var selector = @"$[?@.a<=false]";
            var document = JsonNode.Parse(
                """[{"a":false,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":false,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, greater than string, single quotes"
        [TestMethod]
        public void Test_filter__greater_than_string__single_quotes_Number77()
        {
            var selector = @"$[?@.a>'c']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, greater than string, double quotes"
        [TestMethod]
        public void Test_filter__greater_than_string__double_quotes_Number78()
        {
            var selector = @"$[?@.a>\";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, greater than number"
        [TestMethod]
        public void Test_filter__greater_than_number_Number79()
        {
            var selector = @"$[?@.a>10]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":10,"d":"e"},{"a":"c","d":"f"},{"a":20,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":20,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, greater than null"
        [TestMethod]
        public void Test_filter__greater_than_null_Number80()
        {
            var selector = @"$[?@.a>null]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, greater than true"
        [TestMethod]
        public void Test_filter__greater_than_true_Number81()
        {
            var selector = @"$[?@.a>true]";
            var document = JsonNode.Parse(
                """[{"a":true,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, greater than false"
        [TestMethod]
        public void Test_filter__greater_than_false_Number82()
        {
            var selector = @"$[?@.a>false]";
            var document = JsonNode.Parse(
                """[{"a":false,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, greater than or equal to string, single quotes"
        [TestMethod]
        public void Test_filter__greater_than_or_equal_to_string__single_quotes_Number83()
        {
            var selector = @"$[?@.a>='c']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, greater than or equal to string, double quotes"
        [TestMethod]
        public void Test_filter__greater_than_or_equal_to_string__double_quotes_Number84()
        {
            var selector = @"$[?@.a>=\";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, greater than or equal to number"
        [TestMethod]
        public void Test_filter__greater_than_or_equal_to_number_Number85()
        {
            var selector = @"$[?@.a>=10]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":10,"d":"e"},{"a":"c","d":"f"},{"a":20,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":10,"d":"e"},{"a":20,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, greater than or equal to null"
        [TestMethod]
        public void Test_filter__greater_than_or_equal_to_null_Number86()
        {
            var selector = @"$[?@.a>=null]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":null,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, greater than or equal to true"
        [TestMethod]
        public void Test_filter__greater_than_or_equal_to_true_Number87()
        {
            var selector = @"$[?@.a>=true]";
            var document = JsonNode.Parse(
                """[{"a":true,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":true,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, greater than or equal to false"
        [TestMethod]
        public void Test_filter__greater_than_or_equal_to_false_Number88()
        {
            var selector = @"$[?@.a>=false]";
            var document = JsonNode.Parse(
                """[{"a":false,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":false,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, exists and not-equals null, absent from data"
        [TestMethod]
        public void Test_filter__exists_and_not_equals_null__absent_from_data_Number89()
        {
            var selector = @"$[?@.a&&@.a!=null]";
            var document = JsonNode.Parse(
                """[{"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, exists and exists, data false"
        [TestMethod]
        public void Test_filter__exists_and_exists__data_false_Number90()
        {
            var selector = @"$[?@.a&&@.b]";
            var document = JsonNode.Parse(
                """[{"a":false,"b":false},{"b":false},{"c":false}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":false,"b":false}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, exists or exists, data false"
        [TestMethod]
        public void Test_filter__exists_or_exists__data_false_Number91()
        {
            var selector = @"$[?@.a||@.b]";
            var document = JsonNode.Parse(
                """[{"a":false,"b":false},{"b":false},{"c":false}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":false,"b":false},{"b":false}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, and"
        [TestMethod]
        public void Test_filter__and_Number92()
        {
            var selector = @"$[?@.a>0&&@.a<10]";
            var document = JsonNode.Parse(
                """[{"a":-10,"d":"e"},{"a":5,"d":"f"},{"a":20,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":5,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, or"
        [TestMethod]
        public void Test_filter__or_Number93()
        {
            var selector = @"$[?@.a=='b'||@.a=='d']";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"f"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, not expression"
        [TestMethod]
        public void Test_filter__not_expression_Number94()
        {
            var selector = @"$[?!(@.a=='b')]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, not exists"
        [TestMethod]
        public void Test_filter__not_exists_Number95()
        {
            var selector = @"$[?!@.a]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, not exists, data null"
        [TestMethod]
        public void Test_filter__not_exists__data_null_Number96()
        {
            var selector = @"$[?!@.a]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, non-singular existence, wildcard"
        [TestMethod]
        public void Test_filter__non_singular_existence__wildcard_Number97()
        {
            var selector = @"$[?@.*]";
            var document = JsonNode.Parse(
                """[1,[],[2],{},{"a":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[[2],{"a":3}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, non-singular existence, multiple"
        [TestMethod]
        public void Test_filter__non_singular_existence__multiple_Number98()
        {
            var selector = @"$[?@[0, 0, 'a']]";
            var document = JsonNode.Parse(
                """[1,[],[2],[2,3],{"a":3},{"b":4},{"a":3,"b":4}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[[2],[2,3],{"a":3},{"a":3,"b":4}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, non-singular existence, slice"
        [TestMethod]
        public void Test_filter__non_singular_existence__slice_Number99()
        {
            var selector = @"$[?@[0:2]]";
            var document = JsonNode.Parse(
                """[1,[],[2],[2,3,4],{},{"a":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[[2],[2,3,4]]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, non-singular existence, negated"
        [TestMethod]
        public void Test_filter__non_singular_existence__negated_Number100()
        {
            var selector = @"$[?!@.*]";
            var document = JsonNode.Parse(
                """[1,[],[2],{},{"a":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,[],{}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, non-singular query in comparison, slice"
        [TestMethod]
        public void Test_filter__non_singular_query_in_comparison__slice_Number101()
        {
            var selector = @"$[?@[0:0]==0]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, non-singular query in comparison, all children"
        [TestMethod]
        public void Test_filter__non_singular_query_in_comparison__all_children_Number102()
        {
            var selector = @"$[?@[*]==0]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, non-singular query in comparison, descendants"
        [TestMethod]
        public void Test_filter__non_singular_query_in_comparison__descendants_Number103()
        {
            var selector = @"$[?@..a==0]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, non-singular query in comparison, combined"
        [TestMethod]
        public void Test_filter__non_singular_query_in_comparison__combined_Number104()
        {
            var selector = @"$[?@.a[*].a==0]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, nested"
        [TestMethod]
        public void Test_filter__nested_Number105()
        {
            var selector = @"$[?@[?@>1]]";
            var document = JsonNode.Parse(
                """[[0],[0,1],[0,1,2],[42]]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[[0,1,2],[42]]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, name segment on primitive, selects nothing"
        [TestMethod]
        public void Test_filter__name_segment_on_primitive__selects_nothing_Number106()
        {
            var selector = @"$[?@.a == 1]";
            var document = JsonNode.Parse(
                """{"a":1}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, name segment on array, selects nothing"
        [TestMethod]
        public void Test_filter__name_segment_on_array__selects_nothing_Number107()
        {
            var selector = @"$[?@['0'] == 5]";
            var document = JsonNode.Parse(
                """[[5,6]]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, index segment on object, selects nothing"
        [TestMethod]
        public void Test_filter__index_segment_on_object__selects_nothing_Number108()
        {
            var selector = @"$[?@[0] == 5]";
            var document = JsonNode.Parse(
                """[{"0":5}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, relative non-singular query, index, equal"
        [TestMethod]
        public void Test_filter__relative_non_singular_query__index__equal_Number109()
        {
            var selector = @"$[?(@[0, 0]==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, relative non-singular query, index, not equal"
        [TestMethod]
        public void Test_filter__relative_non_singular_query__index__not_equal_Number110()
        {
            var selector = @"$[?(@[0, 0]!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, relative non-singular query, index, less-or-equal"
        [TestMethod]
        public void Test_filter__relative_non_singular_query__index__less_or_equal_Number111()
        {
            var selector = @"$[?(@[0, 0]<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, relative non-singular query, name, equal"
        [TestMethod]
        public void Test_filter__relative_non_singular_query__name__equal_Number112()
        {
            var selector = @"$[?(@['a', 'a']==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, relative non-singular query, name, not equal"
        [TestMethod]
        public void Test_filter__relative_non_singular_query__name__not_equal_Number113()
        {
            var selector = @"$[?(@['a', 'a']!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, relative non-singular query, name, less-or-equal"
        [TestMethod]
        public void Test_filter__relative_non_singular_query__name__less_or_equal_Number114()
        {
            var selector = @"$[?(@['a', 'a']<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, relative non-singular query, combined, equal"
        [TestMethod]
        public void Test_filter__relative_non_singular_query__combined__equal_Number115()
        {
            var selector = @"$[?(@[0, '0']==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, relative non-singular query, combined, not equal"
        [TestMethod]
        public void Test_filter__relative_non_singular_query__combined__not_equal_Number116()
        {
            var selector = @"$[?(@[0, '0']!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, relative non-singular query, combined, less-or-equal"
        [TestMethod]
        public void Test_filter__relative_non_singular_query__combined__less_or_equal_Number117()
        {
            var selector = @"$[?(@[0, '0']<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, relative non-singular query, wildcard, equal"
        [TestMethod]
        public void Test_filter__relative_non_singular_query__wildcard__equal_Number118()
        {
            var selector = @"$[?(@.*==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, relative non-singular query, wildcard, not equal"
        [TestMethod]
        public void Test_filter__relative_non_singular_query__wildcard__not_equal_Number119()
        {
            var selector = @"$[?(@.*!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, relative non-singular query, wildcard, less-or-equal"
        [TestMethod]
        public void Test_filter__relative_non_singular_query__wildcard__less_or_equal_Number120()
        {
            var selector = @"$[?(@.*<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, relative non-singular query, slice, equal"
        [TestMethod]
        public void Test_filter__relative_non_singular_query__slice__equal_Number121()
        {
            var selector = @"$[?(@[0:0]==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, relative non-singular query, slice, not equal"
        [TestMethod]
        public void Test_filter__relative_non_singular_query__slice__not_equal_Number122()
        {
            var selector = @"$[?(@[0:0]!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, relative non-singular query, slice, less-or-equal"
        [TestMethod]
        public void Test_filter__relative_non_singular_query__slice__less_or_equal_Number123()
        {
            var selector = @"$[?(@[0:0]<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, absolute non-singular query, index, equal"
        [TestMethod]
        public void Test_filter__absolute_non_singular_query__index__equal_Number124()
        {
            var selector = @"$[?($[0, 0]==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, absolute non-singular query, index, not equal"
        [TestMethod]
        public void Test_filter__absolute_non_singular_query__index__not_equal_Number125()
        {
            var selector = @"$[?($[0, 0]!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, absolute non-singular query, index, less-or-equal"
        [TestMethod]
        public void Test_filter__absolute_non_singular_query__index__less_or_equal_Number126()
        {
            var selector = @"$[?($[0, 0]<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, absolute non-singular query, name, equal"
        [TestMethod]
        public void Test_filter__absolute_non_singular_query__name__equal_Number127()
        {
            var selector = @"$[?($['a', 'a']==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, absolute non-singular query, name, not equal"
        [TestMethod]
        public void Test_filter__absolute_non_singular_query__name__not_equal_Number128()
        {
            var selector = @"$[?($['a', 'a']!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, absolute non-singular query, name, less-or-equal"
        [TestMethod]
        public void Test_filter__absolute_non_singular_query__name__less_or_equal_Number129()
        {
            var selector = @"$[?($['a', 'a']<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, absolute non-singular query, combined, equal"
        [TestMethod]
        public void Test_filter__absolute_non_singular_query__combined__equal_Number130()
        {
            var selector = @"$[?($[0, '0']==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, absolute non-singular query, combined, not equal"
        [TestMethod]
        public void Test_filter__absolute_non_singular_query__combined__not_equal_Number131()
        {
            var selector = @"$[?($[0, '0']!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, absolute non-singular query, combined, less-or-equal"
        [TestMethod]
        public void Test_filter__absolute_non_singular_query__combined__less_or_equal_Number132()
        {
            var selector = @"$[?($[0, '0']<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, absolute non-singular query, wildcard, equal"
        [TestMethod]
        public void Test_filter__absolute_non_singular_query__wildcard__equal_Number133()
        {
            var selector = @"$[?($.*==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, absolute non-singular query, wildcard, not equal"
        [TestMethod]
        public void Test_filter__absolute_non_singular_query__wildcard__not_equal_Number134()
        {
            var selector = @"$[?($.*!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, absolute non-singular query, wildcard, less-or-equal"
        [TestMethod]
        public void Test_filter__absolute_non_singular_query__wildcard__less_or_equal_Number135()
        {
            var selector = @"$[?($.*<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, absolute non-singular query, slice, equal"
        [TestMethod]
        public void Test_filter__absolute_non_singular_query__slice__equal_Number136()
        {
            var selector = @"$[?($[0:0]==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, absolute non-singular query, slice, not equal"
        [TestMethod]
        public void Test_filter__absolute_non_singular_query__slice__not_equal_Number137()
        {
            var selector = @"$[?($[0:0]!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, absolute non-singular query, slice, less-or-equal"
        [TestMethod]
        public void Test_filter__absolute_non_singular_query__slice__less_or_equal_Number138()
        {
            var selector = @"$[?($[0:0]<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, multiple selectors"
        [TestMethod]
        public void Test_filter__multiple_selectors_Number139()
        {
            var selector = @"$[?@.a,?@.b]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, multiple selectors, comparison"
        [TestMethod]
        public void Test_filter__multiple_selectors__comparison_Number140()
        {
            var selector = @"$[?@.a=='b',?@.b=='x']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, multiple selectors, overlapping"
        [TestMethod]
        public void Test_filter__multiple_selectors__overlapping_Number141()
        {
            var selector = @"$[?@.a,?@.d]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, multiple selectors, filter and index"
        [TestMethod]
        public void Test_filter__multiple_selectors__filter_and_index_Number142()
        {
            var selector = @"$[?@.a,1]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, multiple selectors, filter and wildcard"
        [TestMethod]
        public void Test_filter__multiple_selectors__filter_and_wildcard_Number143()
        {
            var selector = @"$[?@.a,*]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, multiple selectors, filter and slice"
        [TestMethod]
        public void Test_filter__multiple_selectors__filter_and_slice_Number144()
        {
            var selector = @"$[?@.a,1:]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"},{"g":"h"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"},{"g":"h"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, multiple selectors, comparison filter, index and slice"
        [TestMethod]
        public void Test_filter__multiple_selectors__comparison_filter__index_and_slice_Number145()
        {
            var selector = @"$[1, ?@.a=='b', 1:]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"b":"c","d":"f"},{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals number, zero and negative zero"
        [TestMethod]
        public void Test_filter__equals_number__zero_and_negative_zero_Number146()
        {
            var selector = @"$[?@.a==-0]";
            var document = JsonNode.Parse(
                """[{"a":0,"d":"e"},{"a":0.1,"d":"f"},{"a":"0","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":0,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals number, with and without decimal fraction"
        [TestMethod]
        public void Test_filter__equals_number__with_and_without_decimal_fraction_Number147()
        {
            var selector = @"$[?@.a==1.0]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":2,"d":"f"},{"a":"1","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals number, exponent"
        [TestMethod]
        public void Test_filter__equals_number__exponent_Number148()
        {
            var selector = @"$[?@.a==1e2]";
            var document = JsonNode.Parse(
                """[{"a":100,"d":"e"},{"a":100.1,"d":"f"},{"a":"100","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":100,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals number, positive exponent"
        [TestMethod]
        public void Test_filter__equals_number__positive_exponent_Number149()
        {
            var selector = @"$[?@.a==1e+2]";
            var document = JsonNode.Parse(
                """[{"a":100,"d":"e"},{"a":100.1,"d":"f"},{"a":"100","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":100,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals number, negative exponent"
        [TestMethod]
        public void Test_filter__equals_number__negative_exponent_Number150()
        {
            var selector = @"$[?@.a==1e-2]";
            var document = JsonNode.Parse(
                """[{"a":0.01,"d":"e"},{"a":0.02,"d":"f"},{"a":"0.01","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":0.01,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals number, decimal fraction"
        [TestMethod]
        public void Test_filter__equals_number__decimal_fraction_Number151()
        {
            var selector = @"$[?@.a==1.1]";
            var document = JsonNode.Parse(
                """[{"a":1.1,"d":"e"},{"a":1,"d":"f"},{"a":"1.1","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1.1,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals number, decimal fraction, no fractional digit"
        [TestMethod]
        public void Test_filter__equals_number__decimal_fraction__no_fractional_digit_Number152()
        {
            var selector = @"$[?@.a==1.]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, equals number, decimal fraction, exponent"
        [TestMethod]
        public void Test_filter__equals_number__decimal_fraction__exponent_Number153()
        {
            var selector = @"$[?@.a==1.1e2]";
            var document = JsonNode.Parse(
                """[{"a":110,"d":"e"},{"a":110.1,"d":"f"},{"a":"110","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":110,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals number, decimal fraction, positive exponent"
        [TestMethod]
        public void Test_filter__equals_number__decimal_fraction__positive_exponent_Number154()
        {
            var selector = @"$[?@.a==1.1e+2]";
            var document = JsonNode.Parse(
                """[{"a":110,"d":"e"},{"a":110.1,"d":"f"},{"a":"110","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":110,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals number, decimal fraction, negative exponent"
        [TestMethod]
        public void Test_filter__equals_number__decimal_fraction__negative_exponent_Number155()
        {
            var selector = @"$[?@.a==1.1e-2]";
            var document = JsonNode.Parse(
                """[{"a":0.011,"d":"e"},{"a":0.012,"d":"f"},{"a":"0.011","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":0.011,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals, special nothing"
        [TestMethod]
        public void Test_filter__equals__special_nothing_Number156()
        {
            var selector = @"$.values[?length(@.a) == value($..c)]";
            var document = JsonNode.Parse(
                """{"c":"cd","values":[{"a":"ab"},{"c":"d"},{"a":null}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"c":"d"},{"a":null}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals, empty node list and empty node list"
        [TestMethod]
        public void Test_filter__equals__empty_node_list_and_empty_node_list_Number157()
        {
            var selector = @"$[?@.a == @.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"c":3}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, equals, empty node list and special nothing"
        [TestMethod]
        public void Test_filter__equals__empty_node_list_and_special_nothing_Number158()
        {
            var selector = @"$[?@.a == length(@.b)]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"b":2},{"c":3}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, object data"
        [TestMethod]
        public void Test_filter__object_data_Number159()
        {
            var selector = @"$[?@<3]";
            var document = JsonNode.Parse(
                """{"a":1,"b":2,"c":3}""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[[1,2],[2,1]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, and binds more tightly than or"
        [TestMethod]
        public void Test_filter__and_binds_more_tightly_than_or_Number160()
        {
            var selector = @"$[?@.a || @.b && @.c]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2,"c":3},{"c":3},{"b":2},{"a":1,"b":2,"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2,"c":3},{"a":1,"b":2,"c":3}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, left to right evaluation"
        [TestMethod]
        public void Test_filter__left_to_right_evaluation_Number161()
        {
            var selector = @"$[?@.a && @.b || @.c]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2},{"a":1,"c":3},{"b":1,"c":3},{"c":3},{"a":1,"b":2,"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2},{"a":1,"c":3},{"b":1,"c":3},{"c":3},{"a":1,"b":2,"c":3}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, group terms, left"
        [TestMethod]
        public void Test_filter__group_terms__left_Number162()
        {
            var selector = @"$[?(@.a || @.b) && @.c]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":2},{"a":1,"c":3},{"b":2,"c":3},{"a":1},{"b":2},{"c":3},{"a":1,"b":2,"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"c":3},{"b":2,"c":3},{"a":1,"b":2,"c":3}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, group terms, right"
        [TestMethod]
        public void Test_filter__group_terms__right_Number163()
        {
            var selector = @"$[?@.a && (@.b || @.c)]";
            var document = JsonNode.Parse(
                """[{"a":1},{"a":1,"b":2},{"a":1,"c":2},{"b":2},{"c":2},{"a":1,"b":2,"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2},{"a":1,"c":2},{"a":1,"b":2,"c":3}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, string literal, single quote in double quotes"
        [TestMethod]
        public void Test_filter__string_literal__single_quote_in_double_quotes_Number164()
        {
            var selector = @"$[?@ == \";
            var document = JsonNode.Parse(
                """["quoted' literal","a","quoted\\' literal"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["quoted' literal"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, string literal, double quote in single quotes"
        [TestMethod]
        public void Test_filter__string_literal__double_quote_in_single_quotes_Number165()
        {
            var selector = @"$[?@ == 'quoted\";
            var document = JsonNode.Parse(
                """["quoted\" literal","a","quoted\\\" literal","'quoted\" literal'"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["quoted\" literal"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, string literal, escaped single quote in single quotes"
        [TestMethod]
        public void Test_filter__string_literal__escaped_single_quote_in_single_quotes_Number166()
        {
            var selector = @"$[?@ == 'quoted\\' literal']";
            var document = JsonNode.Parse(
                """["quoted' literal","a","quoted\\' literal","'quoted\" literal'"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["quoted' literal"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, string literal, escaped double quote in double quotes"
        [TestMethod]
        public void Test_filter__string_literal__escaped_double_quote_in_double_quotes_Number167()
        {
            var selector = @"$[?@ == \";
            var document = JsonNode.Parse(
                """["quoted\" literal","a","quoted\\\" literal","'quoted\" literal'"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["quoted\" literal"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "filter, literal true must be compared"
        [TestMethod]
        public void Test_filter__literal_true_must_be_compared_Number168()
        {
            var selector = @"$[?true]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, literal false must be compared"
        [TestMethod]
        public void Test_filter__literal_false_must_be_compared_Number169()
        {
            var selector = @"$[?false]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, literal string must be compared"
        [TestMethod]
        public void Test_filter__literal_string_must_be_compared_Number170()
        {
            var selector = @"$[?'abc']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, literal int must be compared"
        [TestMethod]
        public void Test_filter__literal_int_must_be_compared_Number171()
        {
            var selector = @"$[?2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, literal float must be compared"
        [TestMethod]
        public void Test_filter__literal_float_must_be_compared_Number172()
        {
            var selector = @"$[?2.2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, literal null must be compared"
        [TestMethod]
        public void Test_filter__literal_null_must_be_compared_Number173()
        {
            var selector = @"$[?null]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, and, literals must be compared"
        [TestMethod]
        public void Test_filter__and__literals_must_be_compared_Number174()
        {
            var selector = @"$[?true && false]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, or, literals must be compared"
        [TestMethod]
        public void Test_filter__or__literals_must_be_compared_Number175()
        {
            var selector = @"$[?true || false]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, and, right hand literal must be compared"
        [TestMethod]
        public void Test_filter__and__right_hand_literal_must_be_compared_Number176()
        {
            var selector = @"$[?true == false && false]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, or, right hand literal must be compared"
        [TestMethod]
        public void Test_filter__or__right_hand_literal_must_be_compared_Number177()
        {
            var selector = @"$[?true == false || false]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, and, left hand literal must be compared"
        [TestMethod]
        public void Test_filter__and__left_hand_literal_must_be_compared_Number178()
        {
            var selector = @"$[?false && true == false]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "filter, or, left hand literal must be compared"
        [TestMethod]
        public void Test_filter__or__left_hand_literal_must_be_compared_Number179()
        {
            var selector = @"$[?false || true == false]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "index selector, first element"
        [TestMethod]
        public void Test_index_selector__first_element_Number180()
        {
            var selector = @"$[0]";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["first"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "index selector, second element"
        [TestMethod]
        public void Test_index_selector__second_element_Number181()
        {
            var selector = @"$[1]";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["second"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "index selector, out of bound"
        [TestMethod]
        public void Test_index_selector__out_of_bound_Number182()
        {
            var selector = @"$[2]";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "index selector, overflowing index"
        [TestMethod]
        public void Test_index_selector__overflowing_index_Number183()
        {
            var selector = @"$[231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "index selector, not actually an index, overflowing index leads into general text"
        [TestMethod]
        public void Test_index_selector__not_actually_an_index__overflowing_index_leads_into_general_text_Number184()
        {
            var selector = @"$[231584178474632390847141970017375815706539969331281128078915168SomeRandomText]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "index selector, negative"
        [TestMethod]
        public void Test_index_selector__negative_Number185()
        {
            var selector = @"$[-1]";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["second"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "index selector, more negative"
        [TestMethod]
        public void Test_index_selector__more_negative_Number186()
        {
            var selector = @"$[-2]";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["first"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "index selector, negative out of bound"
        [TestMethod]
        public void Test_index_selector__negative_out_of_bound_Number187()
        {
            var selector = @"$[-3]";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "index selector, on object"
        [TestMethod]
        public void Test_index_selector__on_object_Number188()
        {
            var selector = @"$[0]";
            var document = JsonNode.Parse(
                """{"foo":1}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "index selector, leading 0"
        [TestMethod]
        public void Test_index_selector__leading_0_Number189()
        {
            var selector = @"$[01]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "index selector, leading -0"
        [TestMethod]
        public void Test_index_selector__leading__0_Number190()
        {
            var selector = @"$[-01]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes"
        [TestMethod]
        public void Test_name_selector__double_quotes_Number191()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, absent data"
        [TestMethod]
        public void Test_name_selector__double_quotes__absent_data_Number192()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, array data"
        [TestMethod]
        public void Test_name_selector__double_quotes__array_data_Number193()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0000"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0000_Number194()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0001"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0001_Number195()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0002"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0002_Number196()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0003"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0003_Number197()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0004"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0004_Number198()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0005"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0005_Number199()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0006"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0006_Number200()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0007"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0007_Number201()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0008"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0008_Number202()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0009"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0009_Number203()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+000A"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_000A_Number204()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+000B"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_000B_Number205()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+000C"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_000C_Number206()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+000D"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_000D_Number207()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+000E"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_000E_Number208()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+000F"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_000F_Number209()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0010"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0010_Number210()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0011"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0011_Number211()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0012"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0012_Number212()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0013"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0013_Number213()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0014"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0014_Number214()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0015"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0015_Number215()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0016"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0016_Number216()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0017"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0017_Number217()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0018"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0018_Number218()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0019"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0019_Number219()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+001A"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_001A_Number220()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+001B"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_001B_Number221()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+001C"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_001C_Number222()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+001D"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_001D_Number223()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+001E"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_001E_Number224()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+001F"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_001F_Number225()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded U+0020"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_U_0020_Number226()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{" ":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, escaped double quote"
        [TestMethod]
        public void Test_name_selector__double_quotes__escaped_double_quote_Number227()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{"\"":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, escaped reverse solidus"
        [TestMethod]
        public void Test_name_selector__double_quotes__escaped_reverse_solidus_Number228()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{"\\":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, escaped solidus"
        [TestMethod]
        public void Test_name_selector__double_quotes__escaped_solidus_Number229()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{"/":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, escaped backspace"
        [TestMethod]
        public void Test_name_selector__double_quotes__escaped_backspace_Number230()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{"\b":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, escaped form feed"
        [TestMethod]
        public void Test_name_selector__double_quotes__escaped_form_feed_Number231()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{"\f":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, escaped line feed"
        [TestMethod]
        public void Test_name_selector__double_quotes__escaped_line_feed_Number232()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{"\n":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, escaped carriage return"
        [TestMethod]
        public void Test_name_selector__double_quotes__escaped_carriage_return_Number233()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{"\r":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, escaped tab"
        [TestMethod]
        public void Test_name_selector__double_quotes__escaped_tab_Number234()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{"\t":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, escaped ☺, upper case hex"
        [TestMethod]
        public void Test_name_selector__double_quotes__escaped____upper_case_hex_Number235()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{"☺":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, escaped ☺, lower case hex"
        [TestMethod]
        public void Test_name_selector__double_quotes__escaped____lower_case_hex_Number236()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{"☺":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, surrogate pair 𝄞"
        [TestMethod]
        public void Test_name_selector__double_quotes__surrogate_pair____Number237()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{"𝄞":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, surrogate pair 😀"
        [TestMethod]
        public void Test_name_selector__double_quotes__surrogate_pair____Number238()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{"😀":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, double quotes, invalid escaped single quote"
        [TestMethod]
        public void Test_name_selector__double_quotes__invalid_escaped_single_quote_Number239()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, embedded double quote"
        [TestMethod]
        public void Test_name_selector__double_quotes__embedded_double_quote_Number240()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, incomplete escape"
        [TestMethod]
        public void Test_name_selector__double_quotes__incomplete_escape_Number241()
        {
            var selector = @"$[\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes"
        [TestMethod]
        public void Test_name_selector__single_quotes_Number242()
        {
            var selector = @"$['a']";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, absent data"
        [TestMethod]
        public void Test_name_selector__single_quotes__absent_data_Number243()
        {
            var selector = @"$['c']";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, array data"
        [TestMethod]
        public void Test_name_selector__single_quotes__array_data_Number244()
        {
            var selector = @"$['a']";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0000"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0000_Number245()
        {
            var selector = @"$['\u0000']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0001"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0001_Number246()
        {
            var selector = @"$['\u0001']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0002"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0002_Number247()
        {
            var selector = @"$['\u0002']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0003"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0003_Number248()
        {
            var selector = @"$['\u0003']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0004"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0004_Number249()
        {
            var selector = @"$['\u0004']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0005"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0005_Number250()
        {
            var selector = @"$['\u0005']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0006"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0006_Number251()
        {
            var selector = @"$['\u0006']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0007"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0007_Number252()
        {
            var selector = @"$['\u0007']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0008"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0008_Number253()
        {
            var selector = @"$['\b']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0009"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0009_Number254()
        {
            var selector = @"$['\t']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+000A"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_000A_Number255()
        {
            var selector = @"$['\n']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+000B"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_000B_Number256()
        {
            var selector = @"$['\u000b']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+000C"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_000C_Number257()
        {
            var selector = @"$['\f']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+000D"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_000D_Number258()
        {
            var selector = @"$['\r']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+000E"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_000E_Number259()
        {
            var selector = @"$['\u000e']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+000F"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_000F_Number260()
        {
            var selector = @"$['\u000f']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0010"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0010_Number261()
        {
            var selector = @"$['\u0010']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0011"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0011_Number262()
        {
            var selector = @"$['\u0011']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0012"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0012_Number263()
        {
            var selector = @"$['\u0012']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0013"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0013_Number264()
        {
            var selector = @"$['\u0013']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0014"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0014_Number265()
        {
            var selector = @"$['\u0014']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0015"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0015_Number266()
        {
            var selector = @"$['\u0015']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0016"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0016_Number267()
        {
            var selector = @"$['\u0016']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0017"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0017_Number268()
        {
            var selector = @"$['\u0017']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0018"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0018_Number269()
        {
            var selector = @"$['\u0018']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0019"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0019_Number270()
        {
            var selector = @"$['\u0019']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+001A"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_001A_Number271()
        {
            var selector = @"$['\u001a']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+001B"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_001B_Number272()
        {
            var selector = @"$['\u001b']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+001C"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_001C_Number273()
        {
            var selector = @"$['\u001c']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+001D"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_001D_Number274()
        {
            var selector = @"$['\u001d']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+001E"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_001E_Number275()
        {
            var selector = @"$['\u001e']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+001F"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_001F_Number276()
        {
            var selector = @"$['\u001f']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded U+0020"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_U_0020_Number277()
        {
            var selector = @"$[' ']";
            var document = JsonNode.Parse(
                """{" ":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, escaped single quote"
        [TestMethod]
        public void Test_name_selector__single_quotes__escaped_single_quote_Number278()
        {
            var selector = @"$['\\'']";
            var document = JsonNode.Parse(
                """{"'":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, escaped reverse solidus"
        [TestMethod]
        public void Test_name_selector__single_quotes__escaped_reverse_solidus_Number279()
        {
            var selector = @"$['\\\\']";
            var document = JsonNode.Parse(
                """{"\\":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, escaped solidus"
        [TestMethod]
        public void Test_name_selector__single_quotes__escaped_solidus_Number280()
        {
            var selector = @"$['\\/']";
            var document = JsonNode.Parse(
                """{"/":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, escaped backspace"
        [TestMethod]
        public void Test_name_selector__single_quotes__escaped_backspace_Number281()
        {
            var selector = @"$['\\b']";
            var document = JsonNode.Parse(
                """{"\b":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, escaped form feed"
        [TestMethod]
        public void Test_name_selector__single_quotes__escaped_form_feed_Number282()
        {
            var selector = @"$['\\f']";
            var document = JsonNode.Parse(
                """{"\f":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, escaped line feed"
        [TestMethod]
        public void Test_name_selector__single_quotes__escaped_line_feed_Number283()
        {
            var selector = @"$['\\n']";
            var document = JsonNode.Parse(
                """{"\n":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, escaped carriage return"
        [TestMethod]
        public void Test_name_selector__single_quotes__escaped_carriage_return_Number284()
        {
            var selector = @"$['\\r']";
            var document = JsonNode.Parse(
                """{"\r":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, escaped tab"
        [TestMethod]
        public void Test_name_selector__single_quotes__escaped_tab_Number285()
        {
            var selector = @"$['\\t']";
            var document = JsonNode.Parse(
                """{"\t":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, escaped ☺, upper case hex"
        [TestMethod]
        public void Test_name_selector__single_quotes__escaped____upper_case_hex_Number286()
        {
            var selector = @"$['\\u263A']";
            var document = JsonNode.Parse(
                """{"☺":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, escaped ☺, lower case hex"
        [TestMethod]
        public void Test_name_selector__single_quotes__escaped____lower_case_hex_Number287()
        {
            var selector = @"$['\\u263a']";
            var document = JsonNode.Parse(
                """{"☺":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, surrogate pair 𝄞"
        [TestMethod]
        public void Test_name_selector__single_quotes__surrogate_pair____Number288()
        {
            var selector = @"$['\\uD834\\uDD1E']";
            var document = JsonNode.Parse(
                """{"𝄞":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, surrogate pair 😀"
        [TestMethod]
        public void Test_name_selector__single_quotes__surrogate_pair____Number289()
        {
            var selector = @"$['\\uD83D\\uDE00']";
            var document = JsonNode.Parse(
                """{"😀":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, invalid escaped double quote"
        [TestMethod]
        public void Test_name_selector__single_quotes__invalid_escaped_double_quote_Number290()
        {
            var selector = @"$['\\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, embedded single quote"
        [TestMethod]
        public void Test_name_selector__single_quotes__embedded_single_quote_Number291()
        {
            var selector = @"$[''']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, single quotes, incomplete escape"
        [TestMethod]
        public void Test_name_selector__single_quotes__incomplete_escape_Number292()
        {
            var selector = @"$['\\']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "name selector, double quotes, empty"
        [TestMethod]
        public void Test_name_selector__double_quotes__empty_Number293()
        {
            var selector = @"$[\";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B","":"C"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["C"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "name selector, single quotes, empty"
        [TestMethod]
        public void Test_name_selector__single_quotes__empty_Number294()
        {
            var selector = @"$['']";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B","":"C"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["C"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, slice selector"
        [TestMethod]
        public void Test_slice_selector__slice_selector_Number295()
        {
            var selector = @"$[1:3]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,2]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, slice selector with step"
        [TestMethod]
        public void Test_slice_selector__slice_selector_with_step_Number296()
        {
            var selector = @"$[1:6:2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,3,5]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, slice selector with everything omitted, short form"
        [TestMethod]
        public void Test_slice_selector__slice_selector_with_everything_omitted__short_form_Number297()
        {
            var selector = @"$[:]";
            var document = JsonNode.Parse(
                """[0,1,2,3]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1,2,3]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, slice selector with everything omitted, long form"
        [TestMethod]
        public void Test_slice_selector__slice_selector_with_everything_omitted__long_form_Number298()
        {
            var selector = @"$[::]";
            var document = JsonNode.Parse(
                """[0,1,2,3]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1,2,3]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, slice selector with start omitted"
        [TestMethod]
        public void Test_slice_selector__slice_selector_with_start_omitted_Number299()
        {
            var selector = @"$[:2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, slice selector with start and end omitted"
        [TestMethod]
        public void Test_slice_selector__slice_selector_with_start_and_end_omitted_Number300()
        {
            var selector = @"$[::2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,2,4,6,8]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, negative step with default start and end"
        [TestMethod]
        public void Test_slice_selector__negative_step_with_default_start_and_end_Number301()
        {
            var selector = @"$[::-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[3,2,1,0]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, negative step with default start"
        [TestMethod]
        public void Test_slice_selector__negative_step_with_default_start_Number302()
        {
            var selector = @"$[:0:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[3,2,1]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, negative step with default end"
        [TestMethod]
        public void Test_slice_selector__negative_step_with_default_end_Number303()
        {
            var selector = @"$[2::-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,1,0]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, larger negative step"
        [TestMethod]
        public void Test_slice_selector__larger_negative_step_Number304()
        {
            var selector = @"$[::-2]";
            var document = JsonNode.Parse(
                """[0,1,2,3]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[3,1]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, negative range with default step"
        [TestMethod]
        public void Test_slice_selector__negative_range_with_default_step_Number305()
        {
            var selector = @"$[-1:-3]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, negative range with negative step"
        [TestMethod]
        public void Test_slice_selector__negative_range_with_negative_step_Number306()
        {
            var selector = @"$[-1:-3:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[9,8]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, negative range with larger negative step"
        [TestMethod]
        public void Test_slice_selector__negative_range_with_larger_negative_step_Number307()
        {
            var selector = @"$[-1:-6:-2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[9,7,5]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, larger negative range with larger negative step"
        [TestMethod]
        public void Test_slice_selector__larger_negative_range_with_larger_negative_step_Number308()
        {
            var selector = @"$[-1:-7:-2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[9,7,5]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, negative from, positive to"
        [TestMethod]
        public void Test_slice_selector__negative_from__positive_to_Number309()
        {
            var selector = @"$[-5:7]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[5,6]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, negative from"
        [TestMethod]
        public void Test_slice_selector__negative_from_Number310()
        {
            var selector = @"$[-2:]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[8,9]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, positive from, negative to"
        [TestMethod]
        public void Test_slice_selector__positive_from__negative_to_Number311()
        {
            var selector = @"$[1:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,2,3,4,5,6,7,8]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, negative from, positive to, negative step"
        [TestMethod]
        public void Test_slice_selector__negative_from__positive_to__negative_step_Number312()
        {
            var selector = @"$[-1:1:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[9,8,7,6,5,4,3,2]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, positive from, negative to, negative step"
        [TestMethod]
        public void Test_slice_selector__positive_from__negative_to__negative_step_Number313()
        {
            var selector = @"$[7:-5:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[7,6]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, too many colons"
        [TestMethod]
        public void Test_slice_selector__too_many_colons_Number314()
        {
            var selector = @"$[1:2:3:4]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "slice selector, non-integer array index"
        [TestMethod]
        public void Test_slice_selector__non_integer_array_index_Number315()
        {
            var selector = @"$[1:2:a]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "slice selector, zero step"
        [TestMethod]
        public void Test_slice_selector__zero_step_Number316()
        {
            var selector = @"$[1:2:0]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, empty range"
        [TestMethod]
        public void Test_slice_selector__empty_range_Number317()
        {
            var selector = @"$[2:2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, slice selector with everything omitted with empty array"
        [TestMethod]
        public void Test_slice_selector__slice_selector_with_everything_omitted_with_empty_array_Number318()
        {
            var selector = @"$[:]";
            var document = JsonNode.Parse(
                """[]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, negative step with empty array"
        [TestMethod]
        public void Test_slice_selector__negative_step_with_empty_array_Number319()
        {
            var selector = @"$[::-1]";
            var document = JsonNode.Parse(
                """[]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, maximal range with positive step"
        [TestMethod]
        public void Test_slice_selector__maximal_range_with_positive_step_Number320()
        {
            var selector = @"$[0:10]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, maximal range with negative step"
        [TestMethod]
        public void Test_slice_selector__maximal_range_with_negative_step_Number321()
        {
            var selector = @"$[9:0:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[9,8,7,6,5,4,3,2,1]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, excessively large to value"
        [TestMethod]
        public void Test_slice_selector__excessively_large_to_value_Number322()
        {
            var selector = @"$[2:113667776004]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,3,4,5,6,7,8,9]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, excessively small from value"
        [TestMethod]
        public void Test_slice_selector__excessively_small_from_value_Number323()
        {
            var selector = @"$[-113667776004:1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, excessively large from value with negative step"
        [TestMethod]
        public void Test_slice_selector__excessively_large_from_value_with_negative_step_Number324()
        {
            var selector = @"$[113667776004:0:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[9,8,7,6,5,4,3,2,1]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, excessively small to value with negative step"
        [TestMethod]
        public void Test_slice_selector__excessively_small_to_value_with_negative_step_Number325()
        {
            var selector = @"$[3:-113667776004:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[3,2,1,0]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, excessively large step"
        [TestMethod]
        public void Test_slice_selector__excessively_large_step_Number326()
        {
            var selector = @"$[1:10:113667776004]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, excessively small step"
        [TestMethod]
        public void Test_slice_selector__excessively_small_step_Number327()
        {
            var selector = @"$[-1:-10:-113667776004]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[9]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "slice selector, overflowing to value"
        [TestMethod]
        public void Test_slice_selector__overflowing_to_value_Number328()
        {
            var selector = @"$[2:231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "slice selector, underflowing from value"
        [TestMethod]
        public void Test_slice_selector__underflowing_from_value_Number329()
        {
            var selector = @"$[-231584178474632390847141970017375815706539969331281128078915168015826259279872:1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "slice selector, overflowing from value with negative step"
        [TestMethod]
        public void Test_slice_selector__overflowing_from_value_with_negative_step_Number330()
        {
            var selector = @"$[231584178474632390847141970017375815706539969331281128078915168015826259279872:0:-1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "slice selector, underflowing to value with negative step"
        [TestMethod]
        public void Test_slice_selector__underflowing_to_value_with_negative_step_Number331()
        {
            var selector = @"$[3:-231584178474632390847141970017375815706539969331281128078915168015826259279872:-1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "slice selector, overflowing step"
        [TestMethod]
        public void Test_slice_selector__overflowing_step_Number332()
        {
            var selector = @"$[1:10:231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "slice selector, underflowing step"
        [TestMethod]
        public void Test_slice_selector__underflowing_step_Number333()
        {
            var selector = @"$[-1:-10:-231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, count, count function"
        [TestMethod]
        public void Test_functions__count__count_function_Number334()
        {
            var selector = @"$[?count(@..*)>2]";
            var document = JsonNode.Parse(
                """[{"a":[1,2,3]},{"a":[1],"d":"f"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":[1,2,3]},{"a":[1],"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, count, single-node arg"
        [TestMethod]
        public void Test_functions__count__single_node_arg_Number335()
        {
            var selector = @"$[?count(@.a)>1]";
            var document = JsonNode.Parse(
                """[{"a":[1,2,3]},{"a":[1],"d":"f"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, count, multiple-selector arg"
        [TestMethod]
        public void Test_functions__count__multiple_selector_arg_Number336()
        {
            var selector = @"$[?count(@['a','d'])>1]";
            var document = JsonNode.Parse(
                """[{"a":[1,2,3]},{"a":[1],"d":"f"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":[1],"d":"f"},{"a":1,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, count, non-query arg, number"
        [TestMethod]
        public void Test_functions__count__non_query_arg__number_Number337()
        {
            var selector = @"$[?count(1)>2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, count, non-query arg, string"
        [TestMethod]
        public void Test_functions__count__non_query_arg__string_Number338()
        {
            var selector = @"$[?count('string')>2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, count, non-query arg, true"
        [TestMethod]
        public void Test_functions__count__non_query_arg__true_Number339()
        {
            var selector = @"$[?count(true)>2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, count, non-query arg, false"
        [TestMethod]
        public void Test_functions__count__non_query_arg__false_Number340()
        {
            var selector = @"$[?count(false)>2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, count, non-query arg, null"
        [TestMethod]
        public void Test_functions__count__non_query_arg__null_Number341()
        {
            var selector = @"$[?count(null)>2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, count, result must be compared"
        [TestMethod]
        public void Test_functions__count__result_must_be_compared_Number342()
        {
            var selector = @"$[?count(@..*)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, count, no params"
        [TestMethod]
        public void Test_functions__count__no_params_Number343()
        {
            var selector = @"$[?count()==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, count, too many params"
        [TestMethod]
        public void Test_functions__count__too_many_params_Number344()
        {
            var selector = @"$[?count(@.a,@.b)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, length, string data"
        [TestMethod]
        public void Test_functions__length__string_data_Number345()
        {
            var selector = @"$[?length(@.a)>=2]";
            var document = JsonNode.Parse(
                """[{"a":"ab"},{"a":"d"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"ab"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, length, string data, unicode"
        [TestMethod]
        public void Test_functions__length__string_data__unicode_Number346()
        {
            var selector = @"$[?length(@)==2]";
            var document = JsonNode.Parse(
                """["☺","☺☺","☺☺☺","ж","жж","жжж","磨","阿美","形声字"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["☺☺","жж","阿美"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, length, array data"
        [TestMethod]
        public void Test_functions__length__array_data_Number347()
        {
            var selector = @"$[?length(@.a)>=2]";
            var document = JsonNode.Parse(
                """[{"a":[1,2,3]},{"a":[1]}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":[1,2,3]}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, length, missing data"
        [TestMethod]
        public void Test_functions__length__missing_data_Number348()
        {
            var selector = @"$[?length(@.a)>=2]";
            var document = JsonNode.Parse(
                """[{"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, length, number arg"
        [TestMethod]
        public void Test_functions__length__number_arg_Number349()
        {
            var selector = @"$[?length(1)>=2]";
            var document = JsonNode.Parse(
                """[{"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, length, true arg"
        [TestMethod]
        public void Test_functions__length__true_arg_Number350()
        {
            var selector = @"$[?length(true)>=2]";
            var document = JsonNode.Parse(
                """[{"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, length, false arg"
        [TestMethod]
        public void Test_functions__length__false_arg_Number351()
        {
            var selector = @"$[?length(false)>=2]";
            var document = JsonNode.Parse(
                """[{"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, length, null arg"
        [TestMethod]
        public void Test_functions__length__null_arg_Number352()
        {
            var selector = @"$[?length(null)>=2]";
            var document = JsonNode.Parse(
                """[{"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, length, result must be compared"
        [TestMethod]
        public void Test_functions__length__result_must_be_compared_Number353()
        {
            var selector = @"$[?length(@.a)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, length, no params"
        [TestMethod]
        public void Test_functions__length__no_params_Number354()
        {
            var selector = @"$[?length()==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, length, too many params"
        [TestMethod]
        public void Test_functions__length__too_many_params_Number355()
        {
            var selector = @"$[?length(@.a,@.b)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, length, non-singular query arg"
        [TestMethod]
        public void Test_functions__length__non_singular_query_arg_Number356()
        {
            var selector = @"$[?length(@.*)<3]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, length, arg is a function expression"
        [TestMethod]
        public void Test_functions__length__arg_is_a_function_expression_Number357()
        {
            var selector = @"$.values[?length(@.a)==length(value($..c))]";
            var document = JsonNode.Parse(
                """{"c":"cd","values":[{"a":"ab"},{"a":"d"}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"ab"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, length, arg is special nothing"
        [TestMethod]
        public void Test_functions__length__arg_is_special_nothing_Number358()
        {
            var selector = @"$[?length(value(@.a))>0]";
            var document = JsonNode.Parse(
                """[{"a":"ab"},{"c":"d"},{"a":null}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"ab"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, found match"
        [TestMethod]
        public void Test_functions__match__found_match_Number359()
        {
            var selector = @"$[?match(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"ab"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"ab"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, double quotes"
        [TestMethod]
        public void Test_functions__match__double_quotes_Number360()
        {
            var selector = @"$[?match(@.a, \";
            var document = JsonNode.Parse(
                """[{"a":"ab"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"ab"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, regex from the document"
        [TestMethod]
        public void Test_functions__match__regex_from_the_document_Number361()
        {
            var selector = @"$.values[?match(@, $.regex)]";
            var document = JsonNode.Parse(
                """{"regex":"b.?b","values":["abc","bcd","bab","bba","bbab","b",true,[],{}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["bab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, don't select match"
        [TestMethod]
        public void Test_functions__match__don_t_select_match_Number362()
        {
            var selector = @"$[?!match(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"ab"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, not a match"
        [TestMethod]
        public void Test_functions__match__not_a_match_Number363()
        {
            var selector = @"$[?match(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, select non-match"
        [TestMethod]
        public void Test_functions__match__select_non_match_Number364()
        {
            var selector = @"$[?!match(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"bc"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, non-string first arg"
        [TestMethod]
        public void Test_functions__match__non_string_first_arg_Number365()
        {
            var selector = @"$[?match(1, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, non-string second arg"
        [TestMethod]
        public void Test_functions__match__non_string_second_arg_Number366()
        {
            var selector = @"$[?match(@.a, 1)]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, filter, match function, unicode char class, uppercase"
        [TestMethod]
        public void Test_functions__match__filter__match_function__unicode_char_class__uppercase_Number367()
        {
            var selector = @"$[?match(@, '\\\\p{Lu}')]";
            var document = JsonNode.Parse(
                """["ж","Ж","1","жЖ",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["Ж"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, filter, match function, unicode char class negated, uppercase"
        [TestMethod]
        public void Test_functions__match__filter__match_function__unicode_char_class_negated__uppercase_Number368()
        {
            var selector = @"$[?match(@, '\\\\P{Lu}')]";
            var document = JsonNode.Parse(
                """["ж","Ж","1",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ж","1"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, filter, match function, unicode, surrogate pair"
        [TestMethod]
        public void Test_functions__match__filter__match_function__unicode__surrogate_pair_Number369()
        {
            var selector = @"$[?match(@, 'a.b')]";
            var document = JsonNode.Parse(
                """["a𐄁b","ab","1",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["a𐄁b"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, dot matcher on \u2028"
        [TestMethod]
        public void Test_functions__match__dot_matcher_on__u2028_Number370()
        {
            var selector = @"$[?match(@, '.')]";
            var document = JsonNode.Parse(
                """["\u2028","\r","\n",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["\u2028"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, dot matcher on \u2029"
        [TestMethod]
        public void Test_functions__match__dot_matcher_on__u2029_Number371()
        {
            var selector = @"$[?match(@, '.')]";
            var document = JsonNode.Parse(
                """["\u2029","\r","\n",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["\u2029"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, result cannot be compared"
        [TestMethod]
        public void Test_functions__match__result_cannot_be_compared_Number372()
        {
            var selector = @"$[?match(@.a, 'a.*')==true]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, match, too few params"
        [TestMethod]
        public void Test_functions__match__too_few_params_Number373()
        {
            var selector = @"$[?match(@.a)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, match, too many params"
        [TestMethod]
        public void Test_functions__match__too_many_params_Number374()
        {
            var selector = @"$[?match(@.a,@.b,@.c)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, match, arg is a function expression"
        [TestMethod]
        public void Test_functions__match__arg_is_a_function_expression_Number375()
        {
            var selector = @"$.values[?match(@.a, value($..['regex']))]";
            var document = JsonNode.Parse(
                """{"regex":"a.*","values":[{"a":"ab"},{"a":"ba"}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"ab"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, dot in character class"
        [TestMethod]
        public void Test_functions__match__dot_in_character_class_Number376()
        {
            var selector = @"$[?match(@, 'a[.b]c')]";
            var document = JsonNode.Parse(
                """["abc","a.c","axc"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["abc","a.c"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, escaped dot"
        [TestMethod]
        public void Test_functions__match__escaped_dot_Number377()
        {
            var selector = @"$[?match(@, 'a\\\\.c')]";
            var document = JsonNode.Parse(
                """["abc","a.c","axc"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["a.c"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, escaped backslash before dot"
        [TestMethod]
        public void Test_functions__match__escaped_backslash_before_dot_Number378()
        {
            var selector = @"$[?match(@, 'a\\\\\\\\.c')]";
            var document = JsonNode.Parse(
                """["abc","a.c","axc","a\\\u2028c"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["a\\\u2028c"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, escaped left square bracket"
        [TestMethod]
        public void Test_functions__match__escaped_left_square_bracket_Number379()
        {
            var selector = @"$[?match(@, 'a\\\\[.c')]";
            var document = JsonNode.Parse(
                """["abc","a.c","a[\u2028c"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["a[\u2028c"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, escaped right square bracket"
        [TestMethod]
        public void Test_functions__match__escaped_right_square_bracket_Number380()
        {
            var selector = @"$[?match(@, 'a[\\\\].]c')]";
            var document = JsonNode.Parse(
                """["abc","a.c","a\u2028c","a]c"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["a.c","a]c"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, explicit caret"
        [TestMethod]
        public void Test_functions__match__explicit_caret_Number381()
        {
            var selector = @"$[?match(@, '^ab.*')]";
            var document = JsonNode.Parse(
                """["abc","axc","ab","xab"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["abc","ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, match, explicit dollar"
        [TestMethod]
        public void Test_functions__match__explicit_dollar_Number382()
        {
            var selector = @"$[?match(@, '.*bc$')]";
            var document = JsonNode.Parse(
                """["abc","axc","ab","abcx"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["abc"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, at the end"
        [TestMethod]
        public void Test_functions__search__at_the_end_Number383()
        {
            var selector = @"$[?search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"the end is ab"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"the end is ab"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, double quotes"
        [TestMethod]
        public void Test_functions__search__double_quotes_Number384()
        {
            var selector = @"$[?search(@.a, \";
            var document = JsonNode.Parse(
                """[{"a":"the end is ab"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"the end is ab"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, at the start"
        [TestMethod]
        public void Test_functions__search__at_the_start_Number385()
        {
            var selector = @"$[?search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"ab is at the start"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"ab is at the start"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, in the middle"
        [TestMethod]
        public void Test_functions__search__in_the_middle_Number386()
        {
            var selector = @"$[?search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"contains two matches"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"contains two matches"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, regex from the document"
        [TestMethod]
        public void Test_functions__search__regex_from_the_document_Number387()
        {
            var selector = @"$.values[?search(@, $.regex)]";
            var document = JsonNode.Parse(
                """{"regex":"b.?b","values":["abc","bcd","bab","bba","bbab","b",true,[],{}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["bab","bba","bbab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, don't select match"
        [TestMethod]
        public void Test_functions__search__don_t_select_match_Number388()
        {
            var selector = @"$[?!search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"contains two matches"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, not a match"
        [TestMethod]
        public void Test_functions__search__not_a_match_Number389()
        {
            var selector = @"$[?search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, select non-match"
        [TestMethod]
        public void Test_functions__search__select_non_match_Number390()
        {
            var selector = @"$[?!search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"bc"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, non-string first arg"
        [TestMethod]
        public void Test_functions__search__non_string_first_arg_Number391()
        {
            var selector = @"$[?search(1, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, non-string second arg"
        [TestMethod]
        public void Test_functions__search__non_string_second_arg_Number392()
        {
            var selector = @"$[?search(@.a, 1)]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, filter, search function, unicode char class, uppercase"
        [TestMethod]
        public void Test_functions__search__filter__search_function__unicode_char_class__uppercase_Number393()
        {
            var selector = @"$[?search(@, '\\\\p{Lu}')]";
            var document = JsonNode.Parse(
                """["ж","Ж","1","жЖ",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["Ж","жЖ"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, filter, search function, unicode char class negated, uppercase"
        [TestMethod]
        public void Test_functions__search__filter__search_function__unicode_char_class_negated__uppercase_Number394()
        {
            var selector = @"$[?search(@, '\\\\P{Lu}')]";
            var document = JsonNode.Parse(
                """["ж","Ж","1",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ж","1"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, filter, search function, unicode, surrogate pair"
        [TestMethod]
        public void Test_functions__search__filter__search_function__unicode__surrogate_pair_Number395()
        {
            var selector = @"$[?search(@, 'a.b')]";
            var document = JsonNode.Parse(
                """["a𐄁bc","abc","1",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["a𐄁bc"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, dot matcher on \u2028"
        [TestMethod]
        public void Test_functions__search__dot_matcher_on__u2028_Number396()
        {
            var selector = @"$[?search(@, '.')]";
            var document = JsonNode.Parse(
                """["\u2028","\r\u2028\n","\r","\n",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["\u2028","\r\u2028\n"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, dot matcher on \u2029"
        [TestMethod]
        public void Test_functions__search__dot_matcher_on__u2029_Number397()
        {
            var selector = @"$[?search(@, '.')]";
            var document = JsonNode.Parse(
                """["\u2029","\r\u2029\n","\r","\n",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["\u2029","\r\u2029\n"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, result cannot be compared"
        [TestMethod]
        public void Test_functions__search__result_cannot_be_compared_Number398()
        {
            var selector = @"$[?search(@.a, 'a.*')==true]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, search, too few params"
        [TestMethod]
        public void Test_functions__search__too_few_params_Number399()
        {
            var selector = @"$[?search(@.a)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, search, too many params"
        [TestMethod]
        public void Test_functions__search__too_many_params_Number400()
        {
            var selector = @"$[?search(@.a,@.b,@.c)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, search, arg is a function expression"
        [TestMethod]
        public void Test_functions__search__arg_is_a_function_expression_Number401()
        {
            var selector = @"$.values[?search(@, value($..['regex']))]";
            var document = JsonNode.Parse(
                """{"regex":"b.?b","values":["abc","bcd","bab","bba","bbab","b",true,[],{}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["bab","bba","bbab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, dot in character class"
        [TestMethod]
        public void Test_functions__search__dot_in_character_class_Number402()
        {
            var selector = @"$[?search(@, 'a[.b]c')]";
            var document = JsonNode.Parse(
                """["x abc y","x a.c y","x axc y"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["x abc y","x a.c y"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, escaped dot"
        [TestMethod]
        public void Test_functions__search__escaped_dot_Number403()
        {
            var selector = @"$[?search(@, 'a\\\\.c')]";
            var document = JsonNode.Parse(
                """["x abc y","x a.c y","x axc y"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["x a.c y"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, escaped backslash before dot"
        [TestMethod]
        public void Test_functions__search__escaped_backslash_before_dot_Number404()
        {
            var selector = @"$[?search(@, 'a\\\\\\\\.c')]";
            var document = JsonNode.Parse(
                """["x abc y","x a.c y","x axc y","x a\\\u2028c y"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["x a\\\u2028c y"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, escaped left square bracket"
        [TestMethod]
        public void Test_functions__search__escaped_left_square_bracket_Number405()
        {
            var selector = @"$[?search(@, 'a\\\\[.c')]";
            var document = JsonNode.Parse(
                """["x abc y","x a.c y","x a[\u2028c y"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["x a[\u2028c y"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, search, escaped right square bracket"
        [TestMethod]
        public void Test_functions__search__escaped_right_square_bracket_Number406()
        {
            var selector = @"$[?search(@, 'a[\\\\].]c')]";
            var document = JsonNode.Parse(
                """["x abc y","x a.c y","x a\u2028c y","x a]c y"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["x a.c y","x a]c y"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, value, single-value nodelist"
        [TestMethod]
        public void Test_functions__value__single_value_nodelist_Number407()
        {
            var selector = @"$[?value(@.*)==4]";
            var document = JsonNode.Parse(
                """[[4],{"foo":4},[5],{"foo":5},4]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[[4],{"foo":4}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, value, multi-value nodelist"
        [TestMethod]
        public void Test_functions__value__multi_value_nodelist_Number408()
        {
            var selector = @"$[?value(@.*)==4]";
            var document = JsonNode.Parse(
                """[[4,4],{"foo":4,"bar":4}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "functions, value, too few params"
        [TestMethod]
        public void Test_functions__value__too_few_params_Number409()
        {
            var selector = @"$[?value()==4]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, value, too many params"
        [TestMethod]
        public void Test_functions__value__too_many_params_Number410()
        {
            var selector = @"$[?value(@.a,@.b)==4]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "functions, value, result must be compared"
        [TestMethod]
        public void Test_functions__value__result_must_be_compared_Number411()
        {
            var selector = @"$[?value(@.a)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "whitespace, filter, space between question mark and expression"
        [TestMethod]
        public void Test_whitespace__filter__space_between_question_mark_and_expression_Number412()
        {
            var selector = @"$[? @.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, filter, newline between question mark and expression"
        [TestMethod]
        public void Test_whitespace__filter__newline_between_question_mark_and_expression_Number413()
        {
            var selector = @"$[?\n@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, filter, tab between question mark and expression"
        [TestMethod]
        public void Test_whitespace__filter__tab_between_question_mark_and_expression_Number414()
        {
            var selector = @"$[?\t@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, filter, return between question mark and expression"
        [TestMethod]
        public void Test_whitespace__filter__return_between_question_mark_and_expression_Number415()
        {
            var selector = @"$[?\r@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, filter, space between question mark and parenthesized expression"
        [TestMethod]
        public void Test_whitespace__filter__space_between_question_mark_and_parenthesized_expression_Number416()
        {
            var selector = @"$[? (@.a)]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, filter, newline between question mark and parenthesized expression"
        [TestMethod]
        public void Test_whitespace__filter__newline_between_question_mark_and_parenthesized_expression_Number417()
        {
            var selector = @"$[?\n(@.a)]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, filter, tab between question mark and parenthesized expression"
        [TestMethod]
        public void Test_whitespace__filter__tab_between_question_mark_and_parenthesized_expression_Number418()
        {
            var selector = @"$[?\t(@.a)]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, filter, return between question mark and parenthesized expression"
        [TestMethod]
        public void Test_whitespace__filter__return_between_question_mark_and_parenthesized_expression_Number419()
        {
            var selector = @"$[?\r(@.a)]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, filter, space between parenthesized expression and bracket"
        [TestMethod]
        public void Test_whitespace__filter__space_between_parenthesized_expression_and_bracket_Number420()
        {
            var selector = @"$[?(@.a) ]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, filter, newline between parenthesized expression and bracket"
        [TestMethod]
        public void Test_whitespace__filter__newline_between_parenthesized_expression_and_bracket_Number421()
        {
            var selector = @"$[?(@.a)\n]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, filter, tab between parenthesized expression and bracket"
        [TestMethod]
        public void Test_whitespace__filter__tab_between_parenthesized_expression_and_bracket_Number422()
        {
            var selector = @"$[?(@.a)\t]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, filter, return between parenthesized expression and bracket"
        [TestMethod]
        public void Test_whitespace__filter__return_between_parenthesized_expression_and_bracket_Number423()
        {
            var selector = @"$[?(@.a)\r]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, filter, space between bracket and question mark"
        [TestMethod]
        public void Test_whitespace__filter__space_between_bracket_and_question_mark_Number424()
        {
            var selector = @"$[ ?@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, filter, newline between bracket and question mark"
        [TestMethod]
        public void Test_whitespace__filter__newline_between_bracket_and_question_mark_Number425()
        {
            var selector = @"$[\n?@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, filter, tab between bracket and question mark"
        [TestMethod]
        public void Test_whitespace__filter__tab_between_bracket_and_question_mark_Number426()
        {
            var selector = @"$[\t?@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, filter, return between bracket and question mark"
        [TestMethod]
        public void Test_whitespace__filter__return_between_bracket_and_question_mark_Number427()
        {
            var selector = @"$[\r?@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, space between function name and parenthesis"
        [TestMethod]
        public void Test_whitespace__functions__space_between_function_name_and_parenthesis_Number428()
        {
            var selector = @"$[?count (@.*)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "whitespace, functions, newline between function name and parenthesis"
        [TestMethod]
        public void Test_whitespace__functions__newline_between_function_name_and_parenthesis_Number429()
        {
            var selector = @"$[?count\n(@.*)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "whitespace, functions, tab between function name and parenthesis"
        [TestMethod]
        public void Test_whitespace__functions__tab_between_function_name_and_parenthesis_Number430()
        {
            var selector = @"$[?count\t(@.*)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "whitespace, functions, return between function name and parenthesis"
        [TestMethod]
        public void Test_whitespace__functions__return_between_function_name_and_parenthesis_Number431()
        {
            var selector = @"$[?count\r(@.*)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "whitespace, functions, space between parenthesis and arg"
        [TestMethod]
        public void Test_whitespace__functions__space_between_parenthesis_and_arg_Number432()
        {
            var selector = @"$[?count( @.*)==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, newline between parenthesis and arg"
        [TestMethod]
        public void Test_whitespace__functions__newline_between_parenthesis_and_arg_Number433()
        {
            var selector = @"$[?count(\n@.*)==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, tab between parenthesis and arg"
        [TestMethod]
        public void Test_whitespace__functions__tab_between_parenthesis_and_arg_Number434()
        {
            var selector = @"$[?count(\t@.*)==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, return between parenthesis and arg"
        [TestMethod]
        public void Test_whitespace__functions__return_between_parenthesis_and_arg_Number435()
        {
            var selector = @"$[?count(\r@.*)==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, space between arg and comma"
        [TestMethod]
        public void Test_whitespace__functions__space_between_arg_and_comma_Number436()
        {
            var selector = @"$[?search(@ ,'[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, newline between arg and comma"
        [TestMethod]
        public void Test_whitespace__functions__newline_between_arg_and_comma_Number437()
        {
            var selector = @"$[?search(@\n,'[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, tab between arg and comma"
        [TestMethod]
        public void Test_whitespace__functions__tab_between_arg_and_comma_Number438()
        {
            var selector = @"$[?search(@\t,'[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, return between arg and comma"
        [TestMethod]
        public void Test_whitespace__functions__return_between_arg_and_comma_Number439()
        {
            var selector = @"$[?search(@\r,'[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, space between comma and arg"
        [TestMethod]
        public void Test_whitespace__functions__space_between_comma_and_arg_Number440()
        {
            var selector = @"$[?search(@, '[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, newline between comma and arg"
        [TestMethod]
        public void Test_whitespace__functions__newline_between_comma_and_arg_Number441()
        {
            var selector = @"$[?search(@,\n'[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, tab between comma and arg"
        [TestMethod]
        public void Test_whitespace__functions__tab_between_comma_and_arg_Number442()
        {
            var selector = @"$[?search(@,\t'[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, return between comma and arg"
        [TestMethod]
        public void Test_whitespace__functions__return_between_comma_and_arg_Number443()
        {
            var selector = @"$[?search(@,\r'[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, space between arg and parenthesis"
        [TestMethod]
        public void Test_whitespace__functions__space_between_arg_and_parenthesis_Number444()
        {
            var selector = @"$[?count(@.* )==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, newline between arg and parenthesis"
        [TestMethod]
        public void Test_whitespace__functions__newline_between_arg_and_parenthesis_Number445()
        {
            var selector = @"$[?count(@.*\n)==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, tab between arg and parenthesis"
        [TestMethod]
        public void Test_whitespace__functions__tab_between_arg_and_parenthesis_Number446()
        {
            var selector = @"$[?count(@.*\t)==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, return between arg and parenthesis"
        [TestMethod]
        public void Test_whitespace__functions__return_between_arg_and_parenthesis_Number447()
        {
            var selector = @"$[?count(@.*\r)==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, spaces in a relative singular selector"
        [TestMethod]
        public void Test_whitespace__functions__spaces_in_a_relative_singular_selector_Number448()
        {
            var selector = @"$[?length(@ .a .b) == 3]";
            var document = JsonNode.Parse(
                """[{"a":{"b":"foo"}},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":{"b":"foo"}}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, newlines in a relative singular selector"
        [TestMethod]
        public void Test_whitespace__functions__newlines_in_a_relative_singular_selector_Number449()
        {
            var selector = @"$[?length(@\n.a\n.b) == 3]";
            var document = JsonNode.Parse(
                """[{"a":{"b":"foo"}},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":{"b":"foo"}}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, tabs in a relative singular selector"
        [TestMethod]
        public void Test_whitespace__functions__tabs_in_a_relative_singular_selector_Number450()
        {
            var selector = @"$[?length(@\t.a\t.b) == 3]";
            var document = JsonNode.Parse(
                """[{"a":{"b":"foo"}},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":{"b":"foo"}}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, returns in a relative singular selector"
        [TestMethod]
        public void Test_whitespace__functions__returns_in_a_relative_singular_selector_Number451()
        {
            var selector = @"$[?length(@\r.a\r.b) == 3]";
            var document = JsonNode.Parse(
                """[{"a":{"b":"foo"}},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":{"b":"foo"}}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, spaces in an absolute singular selector"
        [TestMethod]
        public void Test_whitespace__functions__spaces_in_an_absolute_singular_selector_Number452()
        {
            var selector = @"$..[?length(@)==length($ [0] .a)]";
            var document = JsonNode.Parse(
                """[{"a":"foo"},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, newlines in an absolute singular selector"
        [TestMethod]
        public void Test_whitespace__functions__newlines_in_an_absolute_singular_selector_Number453()
        {
            var selector = @"$..[?length(@)==length($\n[0]\n.a)]";
            var document = JsonNode.Parse(
                """[{"a":"foo"},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, tabs in an absolute singular selector"
        [TestMethod]
        public void Test_whitespace__functions__tabs_in_an_absolute_singular_selector_Number454()
        {
            var selector = @"$..[?length(@)==length($\t[0]\t.a)]";
            var document = JsonNode.Parse(
                """[{"a":"foo"},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, functions, returns in an absolute singular selector"
        [TestMethod]
        public void Test_whitespace__functions__returns_in_an_absolute_singular_selector_Number455()
        {
            var selector = @"$..[?length(@)==length($\r[0]\r.a)]";
            var document = JsonNode.Parse(
                """[{"a":"foo"},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space before ||"
        [TestMethod]
        public void Test_whitespace__operators__space_before____Number456()
        {
            var selector = @"$[?@.a ||@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline before ||"
        [TestMethod]
        public void Test_whitespace__operators__newline_before____Number457()
        {
            var selector = @"$[?@.a\n||@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab before ||"
        [TestMethod]
        public void Test_whitespace__operators__tab_before____Number458()
        {
            var selector = @"$[?@.a\t||@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return before ||"
        [TestMethod]
        public void Test_whitespace__operators__return_before____Number459()
        {
            var selector = @"$[?@.a\r||@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space after ||"
        [TestMethod]
        public void Test_whitespace__operators__space_after____Number460()
        {
            var selector = @"$[?@.a|| @.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline after ||"
        [TestMethod]
        public void Test_whitespace__operators__newline_after____Number461()
        {
            var selector = @"$[?@.a||\n@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab after ||"
        [TestMethod]
        public void Test_whitespace__operators__tab_after____Number462()
        {
            var selector = @"$[?@.a||\t@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return after ||"
        [TestMethod]
        public void Test_whitespace__operators__return_after____Number463()
        {
            var selector = @"$[?@.a||\r@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space before &&"
        [TestMethod]
        public void Test_whitespace__operators__space_before____Number464()
        {
            var selector = @"$[?@.a &&@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline before &&"
        [TestMethod]
        public void Test_whitespace__operators__newline_before____Number465()
        {
            var selector = @"$[?@.a\n&&@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab before &&"
        [TestMethod]
        public void Test_whitespace__operators__tab_before____Number466()
        {
            var selector = @"$[?@.a\t&&@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return before &&"
        [TestMethod]
        public void Test_whitespace__operators__return_before____Number467()
        {
            var selector = @"$[?@.a\r&&@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space after &&"
        [TestMethod]
        public void Test_whitespace__operators__space_after____Number468()
        {
            var selector = @"$[?@.a&& @.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline after &&"
        [TestMethod]
        public void Test_whitespace__operators__newline_after____Number469()
        {
            var selector = @"$[?@.a&& @.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab after &&"
        [TestMethod]
        public void Test_whitespace__operators__tab_after____Number470()
        {
            var selector = @"$[?@.a&& @.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return after &&"
        [TestMethod]
        public void Test_whitespace__operators__return_after____Number471()
        {
            var selector = @"$[?@.a&& @.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space before =="
        [TestMethod]
        public void Test_whitespace__operators__space_before____Number472()
        {
            var selector = @"$[?@.a ==@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline before =="
        [TestMethod]
        public void Test_whitespace__operators__newline_before____Number473()
        {
            var selector = @"$[?@.a\n==@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab before =="
        [TestMethod]
        public void Test_whitespace__operators__tab_before____Number474()
        {
            var selector = @"$[?@.a\t==@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return before =="
        [TestMethod]
        public void Test_whitespace__operators__return_before____Number475()
        {
            var selector = @"$[?@.a\r==@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space after =="
        [TestMethod]
        public void Test_whitespace__operators__space_after____Number476()
        {
            var selector = @"$[?@.a== @.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline after =="
        [TestMethod]
        public void Test_whitespace__operators__newline_after____Number477()
        {
            var selector = @"$[?@.a==\n@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab after =="
        [TestMethod]
        public void Test_whitespace__operators__tab_after____Number478()
        {
            var selector = @"$[?@.a==\t@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return after =="
        [TestMethod]
        public void Test_whitespace__operators__return_after____Number479()
        {
            var selector = @"$[?@.a==\r@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space before !="
        [TestMethod]
        public void Test_whitespace__operators__space_before____Number480()
        {
            var selector = @"$[?@.a !=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline before !="
        [TestMethod]
        public void Test_whitespace__operators__newline_before____Number481()
        {
            var selector = @"$[?@.a\n!=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab before !="
        [TestMethod]
        public void Test_whitespace__operators__tab_before____Number482()
        {
            var selector = @"$[?@.a\t!=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return before !="
        [TestMethod]
        public void Test_whitespace__operators__return_before____Number483()
        {
            var selector = @"$[?@.a\r!=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space after !="
        [TestMethod]
        public void Test_whitespace__operators__space_after____Number484()
        {
            var selector = @"$[?@.a!= @.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline after !="
        [TestMethod]
        public void Test_whitespace__operators__newline_after____Number485()
        {
            var selector = @"$[?@.a!=\n@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab after !="
        [TestMethod]
        public void Test_whitespace__operators__tab_after____Number486()
        {
            var selector = @"$[?@.a!=\t@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return after !="
        [TestMethod]
        public void Test_whitespace__operators__return_after____Number487()
        {
            var selector = @"$[?@.a!=\r@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space before <"
        [TestMethod]
        public void Test_whitespace__operators__space_before___Number488()
        {
            var selector = @"$[?@.a <@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline before <"
        [TestMethod]
        public void Test_whitespace__operators__newline_before___Number489()
        {
            var selector = @"$[?@.a\n<@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab before <"
        [TestMethod]
        public void Test_whitespace__operators__tab_before___Number490()
        {
            var selector = @"$[?@.a\t<@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return before <"
        [TestMethod]
        public void Test_whitespace__operators__return_before___Number491()
        {
            var selector = @"$[?@.a\r<@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space after <"
        [TestMethod]
        public void Test_whitespace__operators__space_after___Number492()
        {
            var selector = @"$[?@.a< @.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline after <"
        [TestMethod]
        public void Test_whitespace__operators__newline_after___Number493()
        {
            var selector = @"$[?@.a<\n@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab after <"
        [TestMethod]
        public void Test_whitespace__operators__tab_after___Number494()
        {
            var selector = @"$[?@.a<\t@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return after <"
        [TestMethod]
        public void Test_whitespace__operators__return_after___Number495()
        {
            var selector = @"$[?@.a<\r@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space before >"
        [TestMethod]
        public void Test_whitespace__operators__space_before___Number496()
        {
            var selector = @"$[?@.b >@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline before >"
        [TestMethod]
        public void Test_whitespace__operators__newline_before___Number497()
        {
            var selector = @"$[?@.b\n>@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab before >"
        [TestMethod]
        public void Test_whitespace__operators__tab_before___Number498()
        {
            var selector = @"$[?@.b\t>@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return before >"
        [TestMethod]
        public void Test_whitespace__operators__return_before___Number499()
        {
            var selector = @"$[?@.b\r>@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space after >"
        [TestMethod]
        public void Test_whitespace__operators__space_after___Number500()
        {
            var selector = @"$[?@.b> @.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline after >"
        [TestMethod]
        public void Test_whitespace__operators__newline_after___Number501()
        {
            var selector = @"$[?@.b>\n@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab after >"
        [TestMethod]
        public void Test_whitespace__operators__tab_after___Number502()
        {
            var selector = @"$[?@.b>\t@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return after >"
        [TestMethod]
        public void Test_whitespace__operators__return_after___Number503()
        {
            var selector = @"$[?@.b>\r@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space before <="
        [TestMethod]
        public void Test_whitespace__operators__space_before____Number504()
        {
            var selector = @"$[?@.a <=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline before <="
        [TestMethod]
        public void Test_whitespace__operators__newline_before____Number505()
        {
            var selector = @"$[?@.a\n<=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab before <="
        [TestMethod]
        public void Test_whitespace__operators__tab_before____Number506()
        {
            var selector = @"$[?@.a\t<=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return before <="
        [TestMethod]
        public void Test_whitespace__operators__return_before____Number507()
        {
            var selector = @"$[?@.a\r<=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space after <="
        [TestMethod]
        public void Test_whitespace__operators__space_after____Number508()
        {
            var selector = @"$[?@.a<= @.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline after <="
        [TestMethod]
        public void Test_whitespace__operators__newline_after____Number509()
        {
            var selector = @"$[?@.a<=\n@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab after <="
        [TestMethod]
        public void Test_whitespace__operators__tab_after____Number510()
        {
            var selector = @"$[?@.a<=\t@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return after <="
        [TestMethod]
        public void Test_whitespace__operators__return_after____Number511()
        {
            var selector = @"$[?@.a<=\r@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space before >="
        [TestMethod]
        public void Test_whitespace__operators__space_before____Number512()
        {
            var selector = @"$[?@.b >=@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline before >="
        [TestMethod]
        public void Test_whitespace__operators__newline_before____Number513()
        {
            var selector = @"$[?@.b\n>=@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab before >="
        [TestMethod]
        public void Test_whitespace__operators__tab_before____Number514()
        {
            var selector = @"$[?@.b\t>=@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return before >="
        [TestMethod]
        public void Test_whitespace__operators__return_before____Number515()
        {
            var selector = @"$[?@.b\r>=@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space after >="
        [TestMethod]
        public void Test_whitespace__operators__space_after____Number516()
        {
            var selector = @"$[?@.b>= @.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline after >="
        [TestMethod]
        public void Test_whitespace__operators__newline_after____Number517()
        {
            var selector = @"$[?@.b>=\n@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab after >="
        [TestMethod]
        public void Test_whitespace__operators__tab_after____Number518()
        {
            var selector = @"$[?@.b>=\t@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return after >="
        [TestMethod]
        public void Test_whitespace__operators__return_after____Number519()
        {
            var selector = @"$[?@.b>=\r@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space between logical not and test expression"
        [TestMethod]
        public void Test_whitespace__operators__space_between_logical_not_and_test_expression_Number520()
        {
            var selector = @"$[?! @.a]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline between logical not and test expression"
        [TestMethod]
        public void Test_whitespace__operators__newline_between_logical_not_and_test_expression_Number521()
        {
            var selector = @"$[?!\n@.a]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab between logical not and test expression"
        [TestMethod]
        public void Test_whitespace__operators__tab_between_logical_not_and_test_expression_Number522()
        {
            var selector = @"$[?!\t@.a]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return between logical not and test expression"
        [TestMethod]
        public void Test_whitespace__operators__return_between_logical_not_and_test_expression_Number523()
        {
            var selector = @"$[?!\r@.a]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, space between logical not and parenthesized expression"
        [TestMethod]
        public void Test_whitespace__operators__space_between_logical_not_and_parenthesized_expression_Number524()
        {
            var selector = @"$[?! (@.a=='b')]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, newline between logical not and parenthesized expression"
        [TestMethod]
        public void Test_whitespace__operators__newline_between_logical_not_and_parenthesized_expression_Number525()
        {
            var selector = @"$[?!\n(@.a=='b')]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, tab between logical not and parenthesized expression"
        [TestMethod]
        public void Test_whitespace__operators__tab_between_logical_not_and_parenthesized_expression_Number526()
        {
            var selector = @"$[?!\t(@.a=='b')]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, operators, return between logical not and parenthesized expression"
        [TestMethod]
        public void Test_whitespace__operators__return_between_logical_not_and_parenthesized_expression_Number527()
        {
            var selector = @"$[?!\r(@.a=='b')]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, space between root and bracket"
        [TestMethod]
        public void Test_whitespace__selectors__space_between_root_and_bracket_Number528()
        {
            var selector = @"$ ['a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, newline between root and bracket"
        [TestMethod]
        public void Test_whitespace__selectors__newline_between_root_and_bracket_Number529()
        {
            var selector = @"$\n['a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, tab between root and bracket"
        [TestMethod]
        public void Test_whitespace__selectors__tab_between_root_and_bracket_Number530()
        {
            var selector = @"$\t['a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, return between root and bracket"
        [TestMethod]
        public void Test_whitespace__selectors__return_between_root_and_bracket_Number531()
        {
            var selector = @"$\r['a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, space between bracket and bracket"
        [TestMethod]
        public void Test_whitespace__selectors__space_between_bracket_and_bracket_Number532()
        {
            var selector = @"$['a'] ['b']";
            var document = JsonNode.Parse(
                """{"a":{"b":"ab"}}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, newline between root and bracket"
        [TestMethod]
        public void Test_whitespace__selectors__newline_between_root_and_bracket_Number533()
        {
            var selector = @"$['a'] \n['b']";
            var document = JsonNode.Parse(
                """{"a":{"b":"ab"}}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, tab between root and bracket"
        [TestMethod]
        public void Test_whitespace__selectors__tab_between_root_and_bracket_Number534()
        {
            var selector = @"$['a'] \t['b']";
            var document = JsonNode.Parse(
                """{"a":{"b":"ab"}}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, return between root and bracket"
        [TestMethod]
        public void Test_whitespace__selectors__return_between_root_and_bracket_Number535()
        {
            var selector = @"$['a'] \r['b']";
            var document = JsonNode.Parse(
                """{"a":{"b":"ab"}}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, space between root and dot"
        [TestMethod]
        public void Test_whitespace__selectors__space_between_root_and_dot_Number536()
        {
            var selector = @"$ .a";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, newline between root and dot"
        [TestMethod]
        public void Test_whitespace__selectors__newline_between_root_and_dot_Number537()
        {
            var selector = @"$\n.a";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, tab between root and dot"
        [TestMethod]
        public void Test_whitespace__selectors__tab_between_root_and_dot_Number538()
        {
            var selector = @"$\t.a";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, return between root and dot"
        [TestMethod]
        public void Test_whitespace__selectors__return_between_root_and_dot_Number539()
        {
            var selector = @"$\r.a";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, space between dot and name"
        [TestMethod]
        public void Test_whitespace__selectors__space_between_dot_and_name_Number540()
        {
            var selector = @"$. a";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "whitespace, selectors, newline between dot and name"
        [TestMethod]
        public void Test_whitespace__selectors__newline_between_dot_and_name_Number541()
        {
            var selector = @"$.\na";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "whitespace, selectors, tab between dot and name"
        [TestMethod]
        public void Test_whitespace__selectors__tab_between_dot_and_name_Number542()
        {
            var selector = @"$.\ta";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "whitespace, selectors, return between dot and name"
        [TestMethod]
        public void Test_whitespace__selectors__return_between_dot_and_name_Number543()
        {
            var selector = @"$.\ra";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "whitespace, selectors, space between recursive descent and name"
        [TestMethod]
        public void Test_whitespace__selectors__space_between_recursive_descent_and_name_Number544()
        {
            var selector = @"$.. a";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "whitespace, selectors, newline between recursive descent and name"
        [TestMethod]
        public void Test_whitespace__selectors__newline_between_recursive_descent_and_name_Number545()
        {
            var selector = @"$..\na";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "whitespace, selectors, tab between recursive descent and name"
        [TestMethod]
        public void Test_whitespace__selectors__tab_between_recursive_descent_and_name_Number546()
        {
            var selector = @"$..\ta";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "whitespace, selectors, return between recursive descent and name"
        [TestMethod]
        public void Test_whitespace__selectors__return_between_recursive_descent_and_name_Number547()
        {
            var selector = @"$..\ra";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        // unit-test-ref: "whitespace, selectors, space between bracket and selector"
        [TestMethod]
        public void Test_whitespace__selectors__space_between_bracket_and_selector_Number548()
        {
            var selector = @"$[ 'a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, newline between bracket and selector"
        [TestMethod]
        public void Test_whitespace__selectors__newline_between_bracket_and_selector_Number549()
        {
            var selector = @"$[\n'a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, tab between bracket and selector"
        [TestMethod]
        public void Test_whitespace__selectors__tab_between_bracket_and_selector_Number550()
        {
            var selector = @"$[\t'a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, return between bracket and selector"
        [TestMethod]
        public void Test_whitespace__selectors__return_between_bracket_and_selector_Number551()
        {
            var selector = @"$[\r'a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, space between selector and bracket"
        [TestMethod]
        public void Test_whitespace__selectors__space_between_selector_and_bracket_Number552()
        {
            var selector = @"$['a' ]";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, newline between selector and bracket"
        [TestMethod]
        public void Test_whitespace__selectors__newline_between_selector_and_bracket_Number553()
        {
            var selector = @"$['a'\n]";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, tab between selector and bracket"
        [TestMethod]
        public void Test_whitespace__selectors__tab_between_selector_and_bracket_Number554()
        {
            var selector = @"$['a'\t]";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, return between selector and bracket"
        [TestMethod]
        public void Test_whitespace__selectors__return_between_selector_and_bracket_Number555()
        {
            var selector = @"$['a'\r]";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, space between selector and comma"
        [TestMethod]
        public void Test_whitespace__selectors__space_between_selector_and_comma_Number556()
        {
            var selector = @"$['a' ,'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, newline between selector and comma"
        [TestMethod]
        public void Test_whitespace__selectors__newline_between_selector_and_comma_Number557()
        {
            var selector = @"$['a'\n,'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, tab between selector and comma"
        [TestMethod]
        public void Test_whitespace__selectors__tab_between_selector_and_comma_Number558()
        {
            var selector = @"$['a'\t,'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, return between selector and comma"
        [TestMethod]
        public void Test_whitespace__selectors__return_between_selector_and_comma_Number559()
        {
            var selector = @"$['a'\r,'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, space between comma and selector"
        [TestMethod]
        public void Test_whitespace__selectors__space_between_comma_and_selector_Number560()
        {
            var selector = @"$['a', 'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, newline between comma and selector"
        [TestMethod]
        public void Test_whitespace__selectors__newline_between_comma_and_selector_Number561()
        {
            var selector = @"$['a',\n'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, tab between comma and selector"
        [TestMethod]
        public void Test_whitespace__selectors__tab_between_comma_and_selector_Number562()
        {
            var selector = @"$['a',\t'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, selectors, return between comma and selector"
        [TestMethod]
        public void Test_whitespace__selectors__return_between_comma_and_selector_Number563()
        {
            var selector = @"$['a',\r'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, space between start and colon"
        [TestMethod]
        public void Test_whitespace__slice__space_between_start_and_colon_Number564()
        {
            var selector = @"$[1 :5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, newline between start and colon"
        [TestMethod]
        public void Test_whitespace__slice__newline_between_start_and_colon_Number565()
        {
            var selector = @"$[1\n:5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, tab between start and colon"
        [TestMethod]
        public void Test_whitespace__slice__tab_between_start_and_colon_Number566()
        {
            var selector = @"$[1\t:5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, return between start and colon"
        [TestMethod]
        public void Test_whitespace__slice__return_between_start_and_colon_Number567()
        {
            var selector = @"$[1\r:5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, space between colon and end"
        [TestMethod]
        public void Test_whitespace__slice__space_between_colon_and_end_Number568()
        {
            var selector = @"$[1: 5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, newline between colon and end"
        [TestMethod]
        public void Test_whitespace__slice__newline_between_colon_and_end_Number569()
        {
            var selector = @"$[1:\n5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, tab between colon and end"
        [TestMethod]
        public void Test_whitespace__slice__tab_between_colon_and_end_Number570()
        {
            var selector = @"$[1:\t5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, return between colon and end"
        [TestMethod]
        public void Test_whitespace__slice__return_between_colon_and_end_Number571()
        {
            var selector = @"$[1:\r5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, space between end and colon"
        [TestMethod]
        public void Test_whitespace__slice__space_between_end_and_colon_Number572()
        {
            var selector = @"$[1:5 :2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, newline between end and colon"
        [TestMethod]
        public void Test_whitespace__slice__newline_between_end_and_colon_Number573()
        {
            var selector = @"$[1:5\n:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, tab between end and colon"
        [TestMethod]
        public void Test_whitespace__slice__tab_between_end_and_colon_Number574()
        {
            var selector = @"$[1:5\t:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, return between end and colon"
        [TestMethod]
        public void Test_whitespace__slice__return_between_end_and_colon_Number575()
        {
            var selector = @"$[1:5\r:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, space between colon and step"
        [TestMethod]
        public void Test_whitespace__slice__space_between_colon_and_step_Number576()
        {
            var selector = @"$[1:5: 2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, newline between colon and step"
        [TestMethod]
        public void Test_whitespace__slice__newline_between_colon_and_step_Number577()
        {
            var selector = @"$[1:5:\n2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, tab between colon and step"
        [TestMethod]
        public void Test_whitespace__slice__tab_between_colon_and_step_Number578()
        {
            var selector = @"$[1:5:\t2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }


        // unit-test-ref: "whitespace, slice, return between colon and step"
        [TestMethod]
        public void Test_whitespace__slice__return_between_colon_and_step_Number579()
        {
            var selector = @"$[1:5:\r2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect );
            Assert.IsTrue( match );
        }
    }
}

