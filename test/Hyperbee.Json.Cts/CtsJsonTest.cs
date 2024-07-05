using System;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Cts
{
    [TestClass]
    public class CtsJsonTest
    {



        [TestMethod( "basic, root (1)" )]
        public void Test_1_basic__root()
        {
            var selector = "$";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[["first","second"]]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, no leading whitespace (2)" )]
        public void Test_2_basic__no_leading_whitespace()
        {
            var selector = " $";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "basic, no trailing whitespace (3)" )]
        public void Test_3_basic__no_trailing_whitespace()
        {
            var selector = "$ ";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "basic, name shorthand (4)" )]
        public void Test_4_basic__name_shorthand()
        {
            var selector = "$.a";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, name shorthand, extended unicode ☺ (5)" )]
        public void Test_5_basic__name_shorthand__extended_unicode__()
        {
            var selector = "$.☺";
            var document = JsonNode.Parse(
                """{"☺":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, name shorthand, underscore (6)" )]
        public void Test_6_basic__name_shorthand__underscore()
        {
            var selector = "$._";
            var document = JsonNode.Parse(
                """{"_":"A","_foo":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, name shorthand, symbol (7)" )]
        public void Test_7_basic__name_shorthand__symbol()
        {
            var selector = "$.&";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "basic, name shorthand, number (8)" )]
        public void Test_8_basic__name_shorthand__number()
        {
            var selector = "$.1";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "basic, name shorthand, absent data (9)" )]
        public void Test_9_basic__name_shorthand__absent_data()
        {
            var selector = "$.c";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, name shorthand, array data (10)" )]
        public void Test_10_basic__name_shorthand__array_data()
        {
            var selector = "$.a";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, wildcard shorthand, object data (11)" )]
        public void Test_11_basic__wildcard_shorthand__object_data()
        {
            var selector = "$.*";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[["A","B"],["B","A"]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, wildcard shorthand, array data (12)" )]
        public void Test_12_basic__wildcard_shorthand__array_data()
        {
            var selector = "$.*";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["first","second"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, wildcard selector, array data (13)" )]
        public void Test_13_basic__wildcard_selector__array_data()
        {
            var selector = "$[*]";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["first","second"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, wildcard shorthand, then name shorthand (14)" )]
        public void Test_14_basic__wildcard_shorthand__then_name_shorthand()
        {
            var selector = "$.*.a";
            var document = JsonNode.Parse(
                """{"x":{"a":"Ax","b":"Bx"},"y":{"a":"Ay","b":"By"}}""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[["Ax","Ay"],["Ay","Ax"]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, multiple selectors (15)" )]
        public void Test_15_basic__multiple_selectors()
        {
            var selector = "$[0,2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,2]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, multiple selectors, space instead of comma (16)" )]
        public void Test_16_basic__multiple_selectors__space_instead_of_comma()
        {
            var selector = "$[0 2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "basic, multiple selectors, name and index, array data (17)" )]
        public void Test_17_basic__multiple_selectors__name_and_index__array_data()
        {
            var selector = "$['a',1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, multiple selectors, name and index, object data (18)" )]
        public void Test_18_basic__multiple_selectors__name_and_index__object_data()
        {
            var selector = "$['a',1]";
            var document = JsonNode.Parse(
                """{"a":1,"b":2}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, multiple selectors, index and slice (19)" )]
        public void Test_19_basic__multiple_selectors__index_and_slice()
        {
            var selector = "$[1,5:7]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,5,6]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, multiple selectors, index and slice, overlapping (20)" )]
        public void Test_20_basic__multiple_selectors__index_and_slice__overlapping()
        {
            var selector = "$[1,0:3]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,0,1,2]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, multiple selectors, duplicate index (21)" )]
        public void Test_21_basic__multiple_selectors__duplicate_index()
        {
            var selector = "$[1,1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,1]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, multiple selectors, wildcard and index (22)" )]
        public void Test_22_basic__multiple_selectors__wildcard_and_index()
        {
            var selector = "$[*,1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9,1]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, multiple selectors, wildcard and name (23)" )]
        public void Test_23_basic__multiple_selectors__wildcard_and_name()
        {
            var selector = "$[*,'a']";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[["A","B","A"],["B","A","A"]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, multiple selectors, wildcard and slice (24)" )]
        public void Test_24_basic__multiple_selectors__wildcard_and_slice()
        {
            var selector = "$[*,0:2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9,0,1]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, multiple selectors, multiple wildcards (25)" )]
        public void Test_25_basic__multiple_selectors__multiple_wildcards()
        {
            var selector = "$[*,*]";
            var document = JsonNode.Parse(
                """[0,1,2]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1,2,0,1,2]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, empty segment (26)" )]
        public void Test_26_basic__empty_segment()
        {
            var selector = "$[]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "basic, descendant segment, index (27)" )]
        public void Test_27_basic__descendant_segment__index()
        {
            var selector = "$..[1]";
            var document = JsonNode.Parse(
                """{"o":[0,1,[2,3]]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,3]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, descendant segment, name shorthand (28)" )]
        public void Test_28_basic__descendant_segment__name_shorthand()
        {
            var selector = "$..a";
            var document = JsonNode.Parse(
                """{"o":[{"a":"b"},{"a":"c"}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["b","c"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, descendant segment, wildcard shorthand, array data (29)" )]
        public void Test_29_basic__descendant_segment__wildcard_shorthand__array_data()
        {
            var selector = "$..*";
            var document = JsonNode.Parse(
                """[0,1]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, descendant segment, wildcard selector, array data (30)" )]
        public void Test_30_basic__descendant_segment__wildcard_selector__array_data()
        {
            var selector = "$..[*]";
            var document = JsonNode.Parse(
                """[0,1]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, descendant segment, wildcard selector, nested arrays (31)" )]
        public void Test_31_basic__descendant_segment__wildcard_selector__nested_arrays()
        {
            var selector = "$..[*]";
            var document = JsonNode.Parse(
                """[[[1]],[2]]""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[[[[1]],[2],[1],1,2],[[[1]],[2],[1],2,1]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, descendant segment, wildcard selector, nested objects (32)" )]
        public void Test_32_basic__descendant_segment__wildcard_selector__nested_objects()
        {
            var selector = "$..[*]";
            var document = JsonNode.Parse(
                """{"a":{"c":{"e":1}},"b":{"d":2}}""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[[{"c":{"e":1}},{"d":2},{"e":1},1,2],[{"c":{"e":1}},{"d":2},{"e":1},2,1],[{"c":{"e":1}},{"d":2},2,{"e":1},1],[{"d":2},{"c":{"e":1}},{"e":1},1,2],[{"d":2},{"c":{"e":1}},{"e":1},2,1],[{"d":2},{"c":{"e":1}},2,{"e":1},1]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, descendant segment, wildcard shorthand, object data (33)" )]
        public void Test_33_basic__descendant_segment__wildcard_shorthand__object_data()
        {
            var selector = "$..*";
            var document = JsonNode.Parse(
                """{"a":"b"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["b"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, descendant segment, wildcard shorthand, nested data (34)" )]
        public void Test_34_basic__descendant_segment__wildcard_shorthand__nested_data()
        {
            var selector = "$..*";
            var document = JsonNode.Parse(
                """{"o":[{"a":"b"}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[[{"a":"b"}],{"a":"b"},"b"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, descendant segment, multiple selectors (35)" )]
        public void Test_35_basic__descendant_segment__multiple_selectors()
        {
            var selector = "$..['a','d']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["b","e","c","f"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, descendant segment, object traversal, multiple selectors (36)" )]
        public void Test_36_basic__descendant_segment__object_traversal__multiple_selectors()
        {
            var selector = "$..['a','d']";
            var document = JsonNode.Parse(
                """{"x":{"a":"b","d":"e"},"y":{"a":"c","d":"f"}}""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[["b","e","c","f"],["c","f","b","e"]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, bald descendant segment (37)" )]
        public void Test_37_basic__bald_descendant_segment()
        {
            var selector = "$..";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, existence, without segments (38)" )]
        public void Test_38_filter__existence__without_segments()
        {
            var selector = "$[?@]";
            var document = JsonNode.Parse(
                """{"a":1,"b":null}""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[[1,null],[null,1]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, existence (39)" )]
        public void Test_39_filter__existence()
        {
            var selector = "$[?@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, existence, present with null (40)" )]
        public void Test_40_filter__existence__present_with_null()
        {
            var selector = "$[?@.a]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":null,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals string, single quotes (41)" )]
        public void Test_41_filter__equals_string__single_quotes()
        {
            var selector = "$[?@.a=='b']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals numeric string, single quotes (42)" )]
        public void Test_42_filter__equals_numeric_string__single_quotes()
        {
            var selector = "$[?@.a=='1']";
            var document = JsonNode.Parse(
                """[{"a":"1","d":"e"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"1","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals string, double quotes (43)" )]
        public void Test_43_filter__equals_string__double_quotes()
        {
            var selector = "$[?@.a==\\";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals numeric string, double quotes (44)" )]
        public void Test_44_filter__equals_numeric_string__double_quotes()
        {
            var selector = "$[?@.a==\\";
            var document = JsonNode.Parse(
                """[{"a":"1","d":"e"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"1","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals number (45)" )]
        public void Test_45_filter__equals_number()
        {
            var selector = "$[?@.a==1]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":"c","d":"f"},{"a":2,"d":"f"},{"a":"1","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals null (46)" )]
        public void Test_46_filter__equals_null()
        {
            var selector = "$[?@.a==null]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":null,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals null, absent from data (47)" )]
        public void Test_47_filter__equals_null__absent_from_data()
        {
            var selector = "$[?@.a==null]";
            var document = JsonNode.Parse(
                """[{"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals true (48)" )]
        public void Test_48_filter__equals_true()
        {
            var selector = "$[?@.a==true]";
            var document = JsonNode.Parse(
                """[{"a":true,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":true,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals false (49)" )]
        public void Test_49_filter__equals_false()
        {
            var selector = "$[?@.a==false]";
            var document = JsonNode.Parse(
                """[{"a":false,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":false,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals self (50)" )]
        public void Test_50_filter__equals_self()
        {
            var selector = "$[?@==@]";
            var document = JsonNode.Parse(
                """[1,null,true,{"a":"b"},[false]]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,null,true,{"a":"b"},[false]]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, deep equality, arrays (51)" )]
        public void Test_51_filter__deep_equality__arrays()
        {
            var selector = "$[?@.a==@.b]";
            var document = JsonNode.Parse(
                """[{"a":false,"b":[1,2]},{"a":[[1,[2]]],"b":[[1,[2]]]},{"a":[[1,[2]]],"b":[[[2],1]]},{"a":[[1,[2]]],"b":[[1,2]]}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":[[1,[2]]],"b":[[1,[2]]]}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, deep equality, objects (52)" )]
        public void Test_52_filter__deep_equality__objects()
        {
            var selector = "$[?@.a==@.b]";
            var document = JsonNode.Parse(
                """[{"a":false,"b":{"x":1,"y":{"z":1}}},{"a":{"x":1,"y":{"z":1}},"b":{"x":1,"y":{"z":1}}},{"a":{"x":1,"y":{"z":1}},"b":{"y":{"z":1},"x":1}},{"a":{"x":1,"y":{"z":1}},"b":{"x":1}},{"a":{"x":1,"y":{"z":1}},"b":{"x":1,"y":{"z":2}}}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":{"x":1,"y":{"z":1}},"b":{"x":1,"y":{"z":1}}},{"a":{"x":1,"y":{"z":1}},"b":{"y":{"z":1},"x":1}}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not-equals string, single quotes (53)" )]
        public void Test_53_filter__not_equals_string__single_quotes()
        {
            var selector = "$[?@.a!='b']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not-equals numeric string, single quotes (54)" )]
        public void Test_54_filter__not_equals_numeric_string__single_quotes()
        {
            var selector = "$[?@.a!='1']";
            var document = JsonNode.Parse(
                """[{"a":"1","d":"e"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not-equals string, single quotes, different type (55)" )]
        public void Test_55_filter__not_equals_string__single_quotes__different_type()
        {
            var selector = "$[?@.a!='b']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not-equals string, double quotes (56)" )]
        public void Test_56_filter__not_equals_string__double_quotes()
        {
            var selector = "$[?@.a!=\\";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not-equals numeric string, double quotes (57)" )]
        public void Test_57_filter__not_equals_numeric_string__double_quotes()
        {
            var selector = "$[?@.a!=\\";
            var document = JsonNode.Parse(
                """[{"a":"1","d":"e"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not-equals string, double quotes, different types (58)" )]
        public void Test_58_filter__not_equals_string__double_quotes__different_types()
        {
            var selector = "$[?@.a!=\\";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not-equals number (59)" )]
        public void Test_59_filter__not_equals_number()
        {
            var selector = "$[?@.a!=1]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":2,"d":"f"},{"a":"1","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":2,"d":"f"},{"a":"1","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not-equals number, different types (60)" )]
        public void Test_60_filter__not_equals_number__different_types()
        {
            var selector = "$[?@.a!=1]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not-equals null (61)" )]
        public void Test_61_filter__not_equals_null()
        {
            var selector = "$[?@.a!=null]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not-equals null, absent from data (62)" )]
        public void Test_62_filter__not_equals_null__absent_from_data()
        {
            var selector = "$[?@.a!=null]";
            var document = JsonNode.Parse(
                """[{"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"d":"e"},{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not-equals true (63)" )]
        public void Test_63_filter__not_equals_true()
        {
            var selector = "$[?@.a!=true]";
            var document = JsonNode.Parse(
                """[{"a":true,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not-equals false (64)" )]
        public void Test_64_filter__not_equals_false()
        {
            var selector = "$[?@.a!=false]";
            var document = JsonNode.Parse(
                """[{"a":false,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, less than string, single quotes (65)" )]
        public void Test_65_filter__less_than_string__single_quotes()
        {
            var selector = "$[?@.a<'c']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, less than string, double quotes (66)" )]
        public void Test_66_filter__less_than_string__double_quotes()
        {
            var selector = "$[?@.a<\\";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, less than number (67)" )]
        public void Test_67_filter__less_than_number()
        {
            var selector = "$[?@.a<10]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":10,"d":"e"},{"a":"c","d":"f"},{"a":20,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, less than null (68)" )]
        public void Test_68_filter__less_than_null()
        {
            var selector = "$[?@.a<null]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, less than true (69)" )]
        public void Test_69_filter__less_than_true()
        {
            var selector = "$[?@.a<true]";
            var document = JsonNode.Parse(
                """[{"a":true,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, less than false (70)" )]
        public void Test_70_filter__less_than_false()
        {
            var selector = "$[?@.a<false]";
            var document = JsonNode.Parse(
                """[{"a":false,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, less than or equal to string, single quotes (71)" )]
        public void Test_71_filter__less_than_or_equal_to_string__single_quotes()
        {
            var selector = "$[?@.a<='c']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, less than or equal to string, double quotes (72)" )]
        public void Test_72_filter__less_than_or_equal_to_string__double_quotes()
        {
            var selector = "$[?@.a<=\\";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, less than or equal to number (73)" )]
        public void Test_73_filter__less_than_or_equal_to_number()
        {
            var selector = "$[?@.a<=10]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":10,"d":"e"},{"a":"c","d":"f"},{"a":20,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":10,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, less than or equal to null (74)" )]
        public void Test_74_filter__less_than_or_equal_to_null()
        {
            var selector = "$[?@.a<=null]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":null,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, less than or equal to true (75)" )]
        public void Test_75_filter__less_than_or_equal_to_true()
        {
            var selector = "$[?@.a<=true]";
            var document = JsonNode.Parse(
                """[{"a":true,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":true,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, less than or equal to false (76)" )]
        public void Test_76_filter__less_than_or_equal_to_false()
        {
            var selector = "$[?@.a<=false]";
            var document = JsonNode.Parse(
                """[{"a":false,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":false,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, greater than string, single quotes (77)" )]
        public void Test_77_filter__greater_than_string__single_quotes()
        {
            var selector = "$[?@.a>'c']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, greater than string, double quotes (78)" )]
        public void Test_78_filter__greater_than_string__double_quotes()
        {
            var selector = "$[?@.a>\\";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, greater than number (79)" )]
        public void Test_79_filter__greater_than_number()
        {
            var selector = "$[?@.a>10]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":10,"d":"e"},{"a":"c","d":"f"},{"a":20,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":20,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, greater than null (80)" )]
        public void Test_80_filter__greater_than_null()
        {
            var selector = "$[?@.a>null]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, greater than true (81)" )]
        public void Test_81_filter__greater_than_true()
        {
            var selector = "$[?@.a>true]";
            var document = JsonNode.Parse(
                """[{"a":true,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, greater than false (82)" )]
        public void Test_82_filter__greater_than_false()
        {
            var selector = "$[?@.a>false]";
            var document = JsonNode.Parse(
                """[{"a":false,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, greater than or equal to string, single quotes (83)" )]
        public void Test_83_filter__greater_than_or_equal_to_string__single_quotes()
        {
            var selector = "$[?@.a>='c']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, greater than or equal to string, double quotes (84)" )]
        public void Test_84_filter__greater_than_or_equal_to_string__double_quotes()
        {
            var selector = "$[?@.a>=\\";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, greater than or equal to number (85)" )]
        public void Test_85_filter__greater_than_or_equal_to_number()
        {
            var selector = "$[?@.a>=10]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":10,"d":"e"},{"a":"c","d":"f"},{"a":20,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":10,"d":"e"},{"a":20,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, greater than or equal to null (86)" )]
        public void Test_86_filter__greater_than_or_equal_to_null()
        {
            var selector = "$[?@.a>=null]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":null,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, greater than or equal to true (87)" )]
        public void Test_87_filter__greater_than_or_equal_to_true()
        {
            var selector = "$[?@.a>=true]";
            var document = JsonNode.Parse(
                """[{"a":true,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":true,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, greater than or equal to false (88)" )]
        public void Test_88_filter__greater_than_or_equal_to_false()
        {
            var selector = "$[?@.a>=false]";
            var document = JsonNode.Parse(
                """[{"a":false,"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":false,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, exists and not-equals null, absent from data (89)" )]
        public void Test_89_filter__exists_and_not_equals_null__absent_from_data()
        {
            var selector = "$[?@.a&&@.a!=null]";
            var document = JsonNode.Parse(
                """[{"d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, exists and exists, data false (90)" )]
        public void Test_90_filter__exists_and_exists__data_false()
        {
            var selector = "$[?@.a&&@.b]";
            var document = JsonNode.Parse(
                """[{"a":false,"b":false},{"b":false},{"c":false}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":false,"b":false}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, exists or exists, data false (91)" )]
        public void Test_91_filter__exists_or_exists__data_false()
        {
            var selector = "$[?@.a||@.b]";
            var document = JsonNode.Parse(
                """[{"a":false,"b":false},{"b":false},{"c":false}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":false,"b":false},{"b":false}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, and (92)" )]
        public void Test_92_filter__and()
        {
            var selector = "$[?@.a>0&&@.a<10]";
            var document = JsonNode.Parse(
                """[{"a":-10,"d":"e"},{"a":5,"d":"f"},{"a":20,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":5,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, or (93)" )]
        public void Test_93_filter__or()
        {
            var selector = "$[?@.a=='b'||@.a=='d']";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"f"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not expression (94)" )]
        public void Test_94_filter__not_expression()
        {
            var selector = "$[?!(@.a=='b')]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not exists (95)" )]
        public void Test_95_filter__not_exists()
        {
            var selector = "$[?!@.a]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not exists, data null (96)" )]
        public void Test_96_filter__not_exists__data_null()
        {
            var selector = "$[?!@.a]";
            var document = JsonNode.Parse(
                """[{"a":null,"d":"e"},{"d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, non-singular existence, wildcard (97)" )]
        public void Test_97_filter__non_singular_existence__wildcard()
        {
            var selector = "$[?@.*]";
            var document = JsonNode.Parse(
                """[1,[],[2],{},{"a":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[[2],{"a":3}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, non-singular existence, multiple (98)" )]
        public void Test_98_filter__non_singular_existence__multiple()
        {
            var selector = "$[?@[0, 0, 'a']]";
            var document = JsonNode.Parse(
                """[1,[],[2],[2,3],{"a":3},{"b":4},{"a":3,"b":4}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[[2],[2,3],{"a":3},{"a":3,"b":4}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, non-singular existence, slice (99)" )]
        public void Test_99_filter__non_singular_existence__slice()
        {
            var selector = "$[?@[0:2]]";
            var document = JsonNode.Parse(
                """[1,[],[2],[2,3,4],{},{"a":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[[2],[2,3,4]]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, non-singular existence, negated (100)" )]
        public void Test_100_filter__non_singular_existence__negated()
        {
            var selector = "$[?!@.*]";
            var document = JsonNode.Parse(
                """[1,[],[2],{},{"a":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,[],{}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, non-singular query in comparison, slice (101)" )]
        public void Test_101_filter__non_singular_query_in_comparison__slice()
        {
            var selector = "$[?@[0:0]==0]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, non-singular query in comparison, all children (102)" )]
        public void Test_102_filter__non_singular_query_in_comparison__all_children()
        {
            var selector = "$[?@[*]==0]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, non-singular query in comparison, descendants (103)" )]
        public void Test_103_filter__non_singular_query_in_comparison__descendants()
        {
            var selector = "$[?@..a==0]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, non-singular query in comparison, combined (104)" )]
        public void Test_104_filter__non_singular_query_in_comparison__combined()
        {
            var selector = "$[?@.a[*].a==0]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, nested (105)" )]
        public void Test_105_filter__nested()
        {
            var selector = "$[?@[?@>1]]";
            var document = JsonNode.Parse(
                """[[0],[0,1],[0,1,2],[42]]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[[0,1,2],[42]]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, name segment on primitive, selects nothing (106)" )]
        public void Test_106_filter__name_segment_on_primitive__selects_nothing()
        {
            var selector = "$[?@.a == 1]";
            var document = JsonNode.Parse(
                """{"a":1}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, name segment on array, selects nothing (107)" )]
        public void Test_107_filter__name_segment_on_array__selects_nothing()
        {
            var selector = "$[?@['0'] == 5]";
            var document = JsonNode.Parse(
                """[[5,6]]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, index segment on object, selects nothing (108)" )]
        public void Test_108_filter__index_segment_on_object__selects_nothing()
        {
            var selector = "$[?@[0] == 5]";
            var document = JsonNode.Parse(
                """[{"0":5}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, relative non-singular query, index, equal (109)" )]
        public void Test_109_filter__relative_non_singular_query__index__equal()
        {
            var selector = "$[?(@[0, 0]==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, relative non-singular query, index, not equal (110)" )]
        public void Test_110_filter__relative_non_singular_query__index__not_equal()
        {
            var selector = "$[?(@[0, 0]!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, relative non-singular query, index, less-or-equal (111)" )]
        public void Test_111_filter__relative_non_singular_query__index__less_or_equal()
        {
            var selector = "$[?(@[0, 0]<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, relative non-singular query, name, equal (112)" )]
        public void Test_112_filter__relative_non_singular_query__name__equal()
        {
            var selector = "$[?(@['a', 'a']==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, relative non-singular query, name, not equal (113)" )]
        public void Test_113_filter__relative_non_singular_query__name__not_equal()
        {
            var selector = "$[?(@['a', 'a']!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, relative non-singular query, name, less-or-equal (114)" )]
        public void Test_114_filter__relative_non_singular_query__name__less_or_equal()
        {
            var selector = "$[?(@['a', 'a']<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, relative non-singular query, combined, equal (115)" )]
        public void Test_115_filter__relative_non_singular_query__combined__equal()
        {
            var selector = "$[?(@[0, '0']==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, relative non-singular query, combined, not equal (116)" )]
        public void Test_116_filter__relative_non_singular_query__combined__not_equal()
        {
            var selector = "$[?(@[0, '0']!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, relative non-singular query, combined, less-or-equal (117)" )]
        public void Test_117_filter__relative_non_singular_query__combined__less_or_equal()
        {
            var selector = "$[?(@[0, '0']<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, relative non-singular query, wildcard, equal (118)" )]
        public void Test_118_filter__relative_non_singular_query__wildcard__equal()
        {
            var selector = "$[?(@.*==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, relative non-singular query, wildcard, not equal (119)" )]
        public void Test_119_filter__relative_non_singular_query__wildcard__not_equal()
        {
            var selector = "$[?(@.*!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, relative non-singular query, wildcard, less-or-equal (120)" )]
        public void Test_120_filter__relative_non_singular_query__wildcard__less_or_equal()
        {
            var selector = "$[?(@.*<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, relative non-singular query, slice, equal (121)" )]
        public void Test_121_filter__relative_non_singular_query__slice__equal()
        {
            var selector = "$[?(@[0:0]==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, relative non-singular query, slice, not equal (122)" )]
        public void Test_122_filter__relative_non_singular_query__slice__not_equal()
        {
            var selector = "$[?(@[0:0]!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, relative non-singular query, slice, less-or-equal (123)" )]
        public void Test_123_filter__relative_non_singular_query__slice__less_or_equal()
        {
            var selector = "$[?(@[0:0]<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, absolute non-singular query, index, equal (124)" )]
        public void Test_124_filter__absolute_non_singular_query__index__equal()
        {
            var selector = "$[?($[0, 0]==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, absolute non-singular query, index, not equal (125)" )]
        public void Test_125_filter__absolute_non_singular_query__index__not_equal()
        {
            var selector = "$[?($[0, 0]!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, absolute non-singular query, index, less-or-equal (126)" )]
        public void Test_126_filter__absolute_non_singular_query__index__less_or_equal()
        {
            var selector = "$[?($[0, 0]<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, absolute non-singular query, name, equal (127)" )]
        public void Test_127_filter__absolute_non_singular_query__name__equal()
        {
            var selector = "$[?($['a', 'a']==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, absolute non-singular query, name, not equal (128)" )]
        public void Test_128_filter__absolute_non_singular_query__name__not_equal()
        {
            var selector = "$[?($['a', 'a']!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, absolute non-singular query, name, less-or-equal (129)" )]
        public void Test_129_filter__absolute_non_singular_query__name__less_or_equal()
        {
            var selector = "$[?($['a', 'a']<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, absolute non-singular query, combined, equal (130)" )]
        public void Test_130_filter__absolute_non_singular_query__combined__equal()
        {
            var selector = "$[?($[0, '0']==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, absolute non-singular query, combined, not equal (131)" )]
        public void Test_131_filter__absolute_non_singular_query__combined__not_equal()
        {
            var selector = "$[?($[0, '0']!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, absolute non-singular query, combined, less-or-equal (132)" )]
        public void Test_132_filter__absolute_non_singular_query__combined__less_or_equal()
        {
            var selector = "$[?($[0, '0']<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, absolute non-singular query, wildcard, equal (133)" )]
        public void Test_133_filter__absolute_non_singular_query__wildcard__equal()
        {
            var selector = "$[?($.*==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, absolute non-singular query, wildcard, not equal (134)" )]
        public void Test_134_filter__absolute_non_singular_query__wildcard__not_equal()
        {
            var selector = "$[?($.*!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, absolute non-singular query, wildcard, less-or-equal (135)" )]
        public void Test_135_filter__absolute_non_singular_query__wildcard__less_or_equal()
        {
            var selector = "$[?($.*<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, absolute non-singular query, slice, equal (136)" )]
        public void Test_136_filter__absolute_non_singular_query__slice__equal()
        {
            var selector = "$[?($[0:0]==42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, absolute non-singular query, slice, not equal (137)" )]
        public void Test_137_filter__absolute_non_singular_query__slice__not_equal()
        {
            var selector = "$[?($[0:0]!=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, absolute non-singular query, slice, less-or-equal (138)" )]
        public void Test_138_filter__absolute_non_singular_query__slice__less_or_equal()
        {
            var selector = "$[?($[0:0]<=42)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, multiple selectors (139)" )]
        public void Test_139_filter__multiple_selectors()
        {
            var selector = "$[?@.a,?@.b]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, multiple selectors, comparison (140)" )]
        public void Test_140_filter__multiple_selectors__comparison()
        {
            var selector = "$[?@.a=='b',?@.b=='x']";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, multiple selectors, overlapping (141)" )]
        public void Test_141_filter__multiple_selectors__overlapping()
        {
            var selector = "$[?@.a,?@.d]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, multiple selectors, filter and index (142)" )]
        public void Test_142_filter__multiple_selectors__filter_and_index()
        {
            var selector = "$[?@.a,1]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, multiple selectors, filter and wildcard (143)" )]
        public void Test_143_filter__multiple_selectors__filter_and_wildcard()
        {
            var selector = "$[?@.a,*]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, multiple selectors, filter and slice (144)" )]
        public void Test_144_filter__multiple_selectors__filter_and_slice()
        {
            var selector = "$[?@.a,1:]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"},{"g":"h"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"},{"g":"h"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, multiple selectors, comparison filter, index and slice (145)" )]
        public void Test_145_filter__multiple_selectors__comparison_filter__index_and_slice()
        {
            var selector = "$[1, ?@.a=='b', 1:]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"b":"c","d":"f"},{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals number, zero and negative zero (146)" )]
        public void Test_146_filter__equals_number__zero_and_negative_zero()
        {
            var selector = "$[?@.a==-0]";
            var document = JsonNode.Parse(
                """[{"a":0,"d":"e"},{"a":0.1,"d":"f"},{"a":"0","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":0,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals number, with and without decimal fraction (147)" )]
        public void Test_147_filter__equals_number__with_and_without_decimal_fraction()
        {
            var selector = "$[?@.a==1.0]";
            var document = JsonNode.Parse(
                """[{"a":1,"d":"e"},{"a":2,"d":"f"},{"a":"1","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals number, exponent (148)" )]
        public void Test_148_filter__equals_number__exponent()
        {
            var selector = "$[?@.a==1e2]";
            var document = JsonNode.Parse(
                """[{"a":100,"d":"e"},{"a":100.1,"d":"f"},{"a":"100","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":100,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals number, positive exponent (149)" )]
        public void Test_149_filter__equals_number__positive_exponent()
        {
            var selector = "$[?@.a==1e+2]";
            var document = JsonNode.Parse(
                """[{"a":100,"d":"e"},{"a":100.1,"d":"f"},{"a":"100","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":100,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals number, negative exponent (150)" )]
        public void Test_150_filter__equals_number__negative_exponent()
        {
            var selector = "$[?@.a==1e-2]";
            var document = JsonNode.Parse(
                """[{"a":0.01,"d":"e"},{"a":0.02,"d":"f"},{"a":"0.01","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":0.01,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals number, decimal fraction (151)" )]
        public void Test_151_filter__equals_number__decimal_fraction()
        {
            var selector = "$[?@.a==1.1]";
            var document = JsonNode.Parse(
                """[{"a":1.1,"d":"e"},{"a":1,"d":"f"},{"a":"1.1","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1.1,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals number, decimal fraction, no fractional digit (152)" )]
        public void Test_152_filter__equals_number__decimal_fraction__no_fractional_digit()
        {
            var selector = "$[?@.a==1.]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, equals number, decimal fraction, exponent (153)" )]
        public void Test_153_filter__equals_number__decimal_fraction__exponent()
        {
            var selector = "$[?@.a==1.1e2]";
            var document = JsonNode.Parse(
                """[{"a":110,"d":"e"},{"a":110.1,"d":"f"},{"a":"110","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":110,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals number, decimal fraction, positive exponent (154)" )]
        public void Test_154_filter__equals_number__decimal_fraction__positive_exponent()
        {
            var selector = "$[?@.a==1.1e+2]";
            var document = JsonNode.Parse(
                """[{"a":110,"d":"e"},{"a":110.1,"d":"f"},{"a":"110","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":110,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals number, decimal fraction, negative exponent (155)" )]
        public void Test_155_filter__equals_number__decimal_fraction__negative_exponent()
        {
            var selector = "$[?@.a==1.1e-2]";
            var document = JsonNode.Parse(
                """[{"a":0.011,"d":"e"},{"a":0.012,"d":"f"},{"a":"0.011","d":"g"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":0.011,"d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals, special nothing (156)" )]
        public void Test_156_filter__equals__special_nothing()
        {
            var selector = "$.values[?length(@.a) == value($..c)]";
            var document = JsonNode.Parse(
                """{"c":"cd","values":[{"a":"ab"},{"c":"d"},{"a":null}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"c":"d"},{"a":null}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals, empty node list and empty node list (157)" )]
        public void Test_157_filter__equals__empty_node_list_and_empty_node_list()
        {
            var selector = "$[?@.a == @.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"c":3}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals, empty node list and special nothing (158)" )]
        public void Test_158_filter__equals__empty_node_list_and_special_nothing()
        {
            var selector = "$[?@.a == length(@.b)]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"b":2},{"c":3}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, object data (159)" )]
        public void Test_159_filter__object_data()
        {
            var selector = "$[?@<3]";
            var document = JsonNode.Parse(
                """{"a":1,"b":2,"c":3}""" );
            var results = document.Select( selector );
            var expectOneOf = JsonNode.Parse(
                """[[1,2],[2,1]]""" );

            var match = TestHelper.MatchAny( results, expectOneOf! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, and binds more tightly than or (160)" )]
        public void Test_160_filter__and_binds_more_tightly_than_or()
        {
            var selector = "$[?@.a || @.b && @.c]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2,"c":3},{"c":3},{"b":2},{"a":1,"b":2,"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2,"c":3},{"a":1,"b":2,"c":3}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, left to right evaluation (161)" )]
        public void Test_161_filter__left_to_right_evaluation()
        {
            var selector = "$[?@.a && @.b || @.c]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2},{"a":1,"c":3},{"b":1,"c":3},{"c":3},{"a":1,"b":2,"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2},{"a":1,"c":3},{"b":1,"c":3},{"c":3},{"a":1,"b":2,"c":3}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, group terms, left (162)" )]
        public void Test_162_filter__group_terms__left()
        {
            var selector = "$[?(@.a || @.b) && @.c]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":2},{"a":1,"c":3},{"b":2,"c":3},{"a":1},{"b":2},{"c":3},{"a":1,"b":2,"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"c":3},{"b":2,"c":3},{"a":1,"b":2,"c":3}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, group terms, right (163)" )]
        public void Test_163_filter__group_terms__right()
        {
            var selector = "$[?@.a && (@.b || @.c)]";
            var document = JsonNode.Parse(
                """[{"a":1},{"a":1,"b":2},{"a":1,"c":2},{"b":2},{"c":2},{"a":1,"b":2,"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2},{"a":1,"c":2},{"a":1,"b":2,"c":3}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, string literal, single quote in double quotes (164)" )]
        public void Test_164_filter__string_literal__single_quote_in_double_quotes()
        {
            var selector = "$[?@ == \\";
            var document = JsonNode.Parse(
                """["quoted' literal","a","quoted\\' literal"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["quoted' literal"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, string literal, double quote in single quotes (165)" )]
        public void Test_165_filter__string_literal__double_quote_in_single_quotes()
        {
            var selector = "$[?@ == 'quoted\\";
            var document = JsonNode.Parse(
                """["quoted\" literal","a","quoted\\\" literal","'quoted\" literal'"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["quoted\" literal"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, string literal, escaped single quote in single quotes (166)" )]
        public void Test_166_filter__string_literal__escaped_single_quote_in_single_quotes()
        {
            var selector = "$[?@ == 'quoted\\' literal']";
            var document = JsonNode.Parse(
                """["quoted' literal","a","quoted\\' literal","'quoted\" literal'"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["quoted' literal"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, string literal, escaped double quote in double quotes (167)" )]
        public void Test_167_filter__string_literal__escaped_double_quote_in_double_quotes()
        {
            var selector = "$[?@ == \\";
            var document = JsonNode.Parse(
                """["quoted\" literal","a","quoted\\\" literal","'quoted\" literal'"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["quoted\" literal"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, literal true must be compared (168)" )]
        public void Test_168_filter__literal_true_must_be_compared()
        {
            var selector = "$[?true]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, literal false must be compared (169)" )]
        public void Test_169_filter__literal_false_must_be_compared()
        {
            var selector = "$[?false]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, literal string must be compared (170)" )]
        public void Test_170_filter__literal_string_must_be_compared()
        {
            var selector = "$[?'abc']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, literal int must be compared (171)" )]
        public void Test_171_filter__literal_int_must_be_compared()
        {
            var selector = "$[?2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, literal float must be compared (172)" )]
        public void Test_172_filter__literal_float_must_be_compared()
        {
            var selector = "$[?2.2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, literal null must be compared (173)" )]
        public void Test_173_filter__literal_null_must_be_compared()
        {
            var selector = "$[?null]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, and, literals must be compared (174)" )]
        public void Test_174_filter__and__literals_must_be_compared()
        {
            var selector = "$[?true && false]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, or, literals must be compared (175)" )]
        public void Test_175_filter__or__literals_must_be_compared()
        {
            var selector = "$[?true || false]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, and, right hand literal must be compared (176)" )]
        public void Test_176_filter__and__right_hand_literal_must_be_compared()
        {
            var selector = "$[?true == false && false]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, or, right hand literal must be compared (177)" )]
        public void Test_177_filter__or__right_hand_literal_must_be_compared()
        {
            var selector = "$[?true == false || false]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, and, left hand literal must be compared (178)" )]
        public void Test_178_filter__and__left_hand_literal_must_be_compared()
        {
            var selector = "$[?false && true == false]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "filter, or, left hand literal must be compared (179)" )]
        public void Test_179_filter__or__left_hand_literal_must_be_compared()
        {
            var selector = "$[?false || true == false]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "index selector, first element (180)" )]
        public void Test_180_index_selector__first_element()
        {
            var selector = "$[0]";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["first"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "index selector, second element (181)" )]
        public void Test_181_index_selector__second_element()
        {
            var selector = "$[1]";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["second"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "index selector, out of bound (182)" )]
        public void Test_182_index_selector__out_of_bound()
        {
            var selector = "$[2]";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "index selector, overflowing index (183)" )]
        public void Test_183_index_selector__overflowing_index()
        {
            var selector = "$[231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "index selector, not actually an index, overflowing index leads into general text (184)" )]
        public void Test_184_index_selector__not_actually_an_index__overflowing_index_leads_into_general_text()
        {
            var selector = "$[231584178474632390847141970017375815706539969331281128078915168SomeRandomText]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "index selector, negative (185)" )]
        public void Test_185_index_selector__negative()
        {
            var selector = "$[-1]";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["second"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "index selector, more negative (186)" )]
        public void Test_186_index_selector__more_negative()
        {
            var selector = "$[-2]";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["first"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "index selector, negative out of bound (187)" )]
        public void Test_187_index_selector__negative_out_of_bound()
        {
            var selector = "$[-3]";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "index selector, on object (188)" )]
        public void Test_188_index_selector__on_object()
        {
            var selector = "$[0]";
            var document = JsonNode.Parse(
                """{"foo":1}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "index selector, leading 0 (189)" )]
        public void Test_189_index_selector__leading_0()
        {
            var selector = "$[01]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "index selector, leading -0 (190)" )]
        public void Test_190_index_selector__leading__0()
        {
            var selector = "$[-01]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes (191)" )]
        public void Test_191_name_selector__double_quotes()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, absent data (192)" )]
        public void Test_192_name_selector__double_quotes__absent_data()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, array data (193)" )]
        public void Test_193_name_selector__double_quotes__array_data()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, embedded U+0000 (194)" )]
        public void Test_194_name_selector__double_quotes__embedded_U_0000()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0001 (195)" )]
        public void Test_195_name_selector__double_quotes__embedded_U_0001()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0002 (196)" )]
        public void Test_196_name_selector__double_quotes__embedded_U_0002()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0003 (197)" )]
        public void Test_197_name_selector__double_quotes__embedded_U_0003()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0004 (198)" )]
        public void Test_198_name_selector__double_quotes__embedded_U_0004()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0005 (199)" )]
        public void Test_199_name_selector__double_quotes__embedded_U_0005()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0006 (200)" )]
        public void Test_200_name_selector__double_quotes__embedded_U_0006()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0007 (201)" )]
        public void Test_201_name_selector__double_quotes__embedded_U_0007()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0008 (202)" )]
        public void Test_202_name_selector__double_quotes__embedded_U_0008()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0009 (203)" )]
        public void Test_203_name_selector__double_quotes__embedded_U_0009()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+000A (204)" )]
        public void Test_204_name_selector__double_quotes__embedded_U_000A()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+000B (205)" )]
        public void Test_205_name_selector__double_quotes__embedded_U_000B()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+000C (206)" )]
        public void Test_206_name_selector__double_quotes__embedded_U_000C()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+000D (207)" )]
        public void Test_207_name_selector__double_quotes__embedded_U_000D()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+000E (208)" )]
        public void Test_208_name_selector__double_quotes__embedded_U_000E()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+000F (209)" )]
        public void Test_209_name_selector__double_quotes__embedded_U_000F()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0010 (210)" )]
        public void Test_210_name_selector__double_quotes__embedded_U_0010()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0011 (211)" )]
        public void Test_211_name_selector__double_quotes__embedded_U_0011()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0012 (212)" )]
        public void Test_212_name_selector__double_quotes__embedded_U_0012()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0013 (213)" )]
        public void Test_213_name_selector__double_quotes__embedded_U_0013()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0014 (214)" )]
        public void Test_214_name_selector__double_quotes__embedded_U_0014()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0015 (215)" )]
        public void Test_215_name_selector__double_quotes__embedded_U_0015()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0016 (216)" )]
        public void Test_216_name_selector__double_quotes__embedded_U_0016()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0017 (217)" )]
        public void Test_217_name_selector__double_quotes__embedded_U_0017()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0018 (218)" )]
        public void Test_218_name_selector__double_quotes__embedded_U_0018()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0019 (219)" )]
        public void Test_219_name_selector__double_quotes__embedded_U_0019()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+001A (220)" )]
        public void Test_220_name_selector__double_quotes__embedded_U_001A()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+001B (221)" )]
        public void Test_221_name_selector__double_quotes__embedded_U_001B()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+001C (222)" )]
        public void Test_222_name_selector__double_quotes__embedded_U_001C()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+001D (223)" )]
        public void Test_223_name_selector__double_quotes__embedded_U_001D()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+001E (224)" )]
        public void Test_224_name_selector__double_quotes__embedded_U_001E()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+001F (225)" )]
        public void Test_225_name_selector__double_quotes__embedded_U_001F()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded U+0020 (226)" )]
        public void Test_226_name_selector__double_quotes__embedded_U_0020()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{" ":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped double quote (227)" )]
        public void Test_227_name_selector__double_quotes__escaped_double_quote()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{"\"":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped reverse solidus (228)" )]
        public void Test_228_name_selector__double_quotes__escaped_reverse_solidus()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{"\\":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped solidus (229)" )]
        public void Test_229_name_selector__double_quotes__escaped_solidus()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{"/":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped backspace (230)" )]
        public void Test_230_name_selector__double_quotes__escaped_backspace()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{"\b":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped form feed (231)" )]
        public void Test_231_name_selector__double_quotes__escaped_form_feed()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{"\f":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped line feed (232)" )]
        public void Test_232_name_selector__double_quotes__escaped_line_feed()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{"\n":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped carriage return (233)" )]
        public void Test_233_name_selector__double_quotes__escaped_carriage_return()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{"\r":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped tab (234)" )]
        public void Test_234_name_selector__double_quotes__escaped_tab()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{"\t":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped ☺, upper case hex (235)" )]
        public void Test_235_name_selector__double_quotes__escaped____upper_case_hex()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{"☺":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped ☺, lower case hex (236)" )]
        public void Test_236_name_selector__double_quotes__escaped____lower_case_hex()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{"☺":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, surrogate pair 𝄞 (237)" )]
        public void Test_237_name_selector__double_quotes__surrogate_pair___()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{"𝄞":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, surrogate pair 😀 (238)" )]
        public void Test_238_name_selector__double_quotes__surrogate_pair___()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{"😀":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, invalid escaped single quote (239)" )]
        public void Test_239_name_selector__double_quotes__invalid_escaped_single_quote()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, embedded double quote (240)" )]
        public void Test_240_name_selector__double_quotes__embedded_double_quote()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, incomplete escape (241)" )]
        public void Test_241_name_selector__double_quotes__incomplete_escape()
        {
            var selector = "$[\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes (242)" )]
        public void Test_242_name_selector__single_quotes()
        {
            var selector = "$['a']";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, absent data (243)" )]
        public void Test_243_name_selector__single_quotes__absent_data()
        {
            var selector = "$['c']";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, array data (244)" )]
        public void Test_244_name_selector__single_quotes__array_data()
        {
            var selector = "$['a']";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, embedded U+0000 (245)" )]
        public void Test_245_name_selector__single_quotes__embedded_U_0000()
        {
            var selector = "$['\u0000']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0001 (246)" )]
        public void Test_246_name_selector__single_quotes__embedded_U_0001()
        {
            var selector = "$['\u0001']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0002 (247)" )]
        public void Test_247_name_selector__single_quotes__embedded_U_0002()
        {
            var selector = "$['\u0002']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0003 (248)" )]
        public void Test_248_name_selector__single_quotes__embedded_U_0003()
        {
            var selector = "$['\u0003']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0004 (249)" )]
        public void Test_249_name_selector__single_quotes__embedded_U_0004()
        {
            var selector = "$['\u0004']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0005 (250)" )]
        public void Test_250_name_selector__single_quotes__embedded_U_0005()
        {
            var selector = "$['\u0005']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0006 (251)" )]
        public void Test_251_name_selector__single_quotes__embedded_U_0006()
        {
            var selector = "$['\u0006']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0007 (252)" )]
        public void Test_252_name_selector__single_quotes__embedded_U_0007()
        {
            var selector = "$['\u0007']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0008 (253)" )]
        public void Test_253_name_selector__single_quotes__embedded_U_0008()
        {
            var selector = "$['\b']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0009 (254)" )]
        public void Test_254_name_selector__single_quotes__embedded_U_0009()
        {
            var selector = "$['\t']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+000A (255)" )]
        public void Test_255_name_selector__single_quotes__embedded_U_000A()
        {
            var selector = "$['\n']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+000B (256)" )]
        public void Test_256_name_selector__single_quotes__embedded_U_000B()
        {
            var selector = "$['\u000b']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+000C (257)" )]
        public void Test_257_name_selector__single_quotes__embedded_U_000C()
        {
            var selector = "$['\f']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+000D (258)" )]
        public void Test_258_name_selector__single_quotes__embedded_U_000D()
        {
            var selector = "$['\r']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+000E (259)" )]
        public void Test_259_name_selector__single_quotes__embedded_U_000E()
        {
            var selector = "$['\u000e']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+000F (260)" )]
        public void Test_260_name_selector__single_quotes__embedded_U_000F()
        {
            var selector = "$['\u000f']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0010 (261)" )]
        public void Test_261_name_selector__single_quotes__embedded_U_0010()
        {
            var selector = "$['\u0010']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0011 (262)" )]
        public void Test_262_name_selector__single_quotes__embedded_U_0011()
        {
            var selector = "$['\u0011']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0012 (263)" )]
        public void Test_263_name_selector__single_quotes__embedded_U_0012()
        {
            var selector = "$['\u0012']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0013 (264)" )]
        public void Test_264_name_selector__single_quotes__embedded_U_0013()
        {
            var selector = "$['\u0013']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0014 (265)" )]
        public void Test_265_name_selector__single_quotes__embedded_U_0014()
        {
            var selector = "$['\u0014']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0015 (266)" )]
        public void Test_266_name_selector__single_quotes__embedded_U_0015()
        {
            var selector = "$['\u0015']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0016 (267)" )]
        public void Test_267_name_selector__single_quotes__embedded_U_0016()
        {
            var selector = "$['\u0016']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0017 (268)" )]
        public void Test_268_name_selector__single_quotes__embedded_U_0017()
        {
            var selector = "$['\u0017']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0018 (269)" )]
        public void Test_269_name_selector__single_quotes__embedded_U_0018()
        {
            var selector = "$['\u0018']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0019 (270)" )]
        public void Test_270_name_selector__single_quotes__embedded_U_0019()
        {
            var selector = "$['\u0019']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+001A (271)" )]
        public void Test_271_name_selector__single_quotes__embedded_U_001A()
        {
            var selector = "$['\u001a']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+001B (272)" )]
        public void Test_272_name_selector__single_quotes__embedded_U_001B()
        {
            var selector = "$['\u001b']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+001C (273)" )]
        public void Test_273_name_selector__single_quotes__embedded_U_001C()
        {
            var selector = "$['\u001c']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+001D (274)" )]
        public void Test_274_name_selector__single_quotes__embedded_U_001D()
        {
            var selector = "$['\u001d']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+001E (275)" )]
        public void Test_275_name_selector__single_quotes__embedded_U_001E()
        {
            var selector = "$['\u001e']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+001F (276)" )]
        public void Test_276_name_selector__single_quotes__embedded_U_001F()
        {
            var selector = "$['\u001f']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded U+0020 (277)" )]
        public void Test_277_name_selector__single_quotes__embedded_U_0020()
        {
            var selector = "$[' ']";
            var document = JsonNode.Parse(
                """{" ":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, escaped single quote (278)" )]
        public void Test_278_name_selector__single_quotes__escaped_single_quote()
        {
            var selector = "$['\\'']";
            var document = JsonNode.Parse(
                """{"'":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, escaped reverse solidus (279)" )]
        public void Test_279_name_selector__single_quotes__escaped_reverse_solidus()
        {
            var selector = "$['\\\\']";
            var document = JsonNode.Parse(
                """{"\\":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, escaped solidus (280)" )]
        public void Test_280_name_selector__single_quotes__escaped_solidus()
        {
            var selector = "$['\\/']";
            var document = JsonNode.Parse(
                """{"/":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, escaped backspace (281)" )]
        public void Test_281_name_selector__single_quotes__escaped_backspace()
        {
            var selector = "$['\\b']";
            var document = JsonNode.Parse(
                """{"\b":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, escaped form feed (282)" )]
        public void Test_282_name_selector__single_quotes__escaped_form_feed()
        {
            var selector = "$['\\f']";
            var document = JsonNode.Parse(
                """{"\f":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, escaped line feed (283)" )]
        public void Test_283_name_selector__single_quotes__escaped_line_feed()
        {
            var selector = "$['\\n']";
            var document = JsonNode.Parse(
                """{"\n":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, escaped carriage return (284)" )]
        public void Test_284_name_selector__single_quotes__escaped_carriage_return()
        {
            var selector = "$['\\r']";
            var document = JsonNode.Parse(
                """{"\r":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, escaped tab (285)" )]
        public void Test_285_name_selector__single_quotes__escaped_tab()
        {
            var selector = "$['\\t']";
            var document = JsonNode.Parse(
                """{"\t":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, escaped ☺, upper case hex (286)" )]
        public void Test_286_name_selector__single_quotes__escaped____upper_case_hex()
        {
            var selector = "$['\\u263A']";
            var document = JsonNode.Parse(
                """{"☺":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, escaped ☺, lower case hex (287)" )]
        public void Test_287_name_selector__single_quotes__escaped____lower_case_hex()
        {
            var selector = "$['\\u263a']";
            var document = JsonNode.Parse(
                """{"☺":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, surrogate pair 𝄞 (288)" )]
        public void Test_288_name_selector__single_quotes__surrogate_pair___()
        {
            var selector = "$['\\uD834\\uDD1E']";
            var document = JsonNode.Parse(
                """{"𝄞":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, surrogate pair 😀 (289)" )]
        public void Test_289_name_selector__single_quotes__surrogate_pair___()
        {
            var selector = "$['\\uD83D\\uDE00']";
            var document = JsonNode.Parse(
                """{"😀":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, invalid escaped double quote (290)" )]
        public void Test_290_name_selector__single_quotes__invalid_escaped_double_quote()
        {
            var selector = "$['\\\\";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, embedded single quote (291)" )]
        public void Test_291_name_selector__single_quotes__embedded_single_quote()
        {
            var selector = "$[''']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, single quotes, incomplete escape (292)" )]
        public void Test_292_name_selector__single_quotes__incomplete_escape()
        {
            var selector = "$['\\']";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "name selector, double quotes, empty (293)" )]
        public void Test_293_name_selector__double_quotes__empty()
        {
            var selector = "$[\\";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B","":"C"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["C"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, empty (294)" )]
        public void Test_294_name_selector__single_quotes__empty()
        {
            var selector = "$['']";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B","":"C"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["C"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, slice selector (295)" )]
        public void Test_295_slice_selector__slice_selector()
        {
            var selector = "$[1:3]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,2]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, slice selector with step (296)" )]
        public void Test_296_slice_selector__slice_selector_with_step()
        {
            var selector = "$[1:6:2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,3,5]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, slice selector with everything omitted, short form (297)" )]
        public void Test_297_slice_selector__slice_selector_with_everything_omitted__short_form()
        {
            var selector = "$[:]";
            var document = JsonNode.Parse(
                """[0,1,2,3]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1,2,3]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, slice selector with everything omitted, long form (298)" )]
        public void Test_298_slice_selector__slice_selector_with_everything_omitted__long_form()
        {
            var selector = "$[::]";
            var document = JsonNode.Parse(
                """[0,1,2,3]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1,2,3]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, slice selector with start omitted (299)" )]
        public void Test_299_slice_selector__slice_selector_with_start_omitted()
        {
            var selector = "$[:2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, slice selector with start and end omitted (300)" )]
        public void Test_300_slice_selector__slice_selector_with_start_and_end_omitted()
        {
            var selector = "$[::2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,2,4,6,8]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, negative step with default start and end (301)" )]
        public void Test_301_slice_selector__negative_step_with_default_start_and_end()
        {
            var selector = "$[::-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[3,2,1,0]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, negative step with default start (302)" )]
        public void Test_302_slice_selector__negative_step_with_default_start()
        {
            var selector = "$[:0:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[3,2,1]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, negative step with default end (303)" )]
        public void Test_303_slice_selector__negative_step_with_default_end()
        {
            var selector = "$[2::-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,1,0]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, larger negative step (304)" )]
        public void Test_304_slice_selector__larger_negative_step()
        {
            var selector = "$[::-2]";
            var document = JsonNode.Parse(
                """[0,1,2,3]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[3,1]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, negative range with default step (305)" )]
        public void Test_305_slice_selector__negative_range_with_default_step()
        {
            var selector = "$[-1:-3]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, negative range with negative step (306)" )]
        public void Test_306_slice_selector__negative_range_with_negative_step()
        {
            var selector = "$[-1:-3:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[9,8]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, negative range with larger negative step (307)" )]
        public void Test_307_slice_selector__negative_range_with_larger_negative_step()
        {
            var selector = "$[-1:-6:-2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[9,7,5]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, larger negative range with larger negative step (308)" )]
        public void Test_308_slice_selector__larger_negative_range_with_larger_negative_step()
        {
            var selector = "$[-1:-7:-2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[9,7,5]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, negative from, positive to (309)" )]
        public void Test_309_slice_selector__negative_from__positive_to()
        {
            var selector = "$[-5:7]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[5,6]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, negative from (310)" )]
        public void Test_310_slice_selector__negative_from()
        {
            var selector = "$[-2:]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[8,9]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, positive from, negative to (311)" )]
        public void Test_311_slice_selector__positive_from__negative_to()
        {
            var selector = "$[1:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1,2,3,4,5,6,7,8]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, negative from, positive to, negative step (312)" )]
        public void Test_312_slice_selector__negative_from__positive_to__negative_step()
        {
            var selector = "$[-1:1:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[9,8,7,6,5,4,3,2]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, positive from, negative to, negative step (313)" )]
        public void Test_313_slice_selector__positive_from__negative_to__negative_step()
        {
            var selector = "$[7:-5:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[7,6]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, too many colons (314)" )]
        public void Test_314_slice_selector__too_many_colons()
        {
            var selector = "$[1:2:3:4]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "slice selector, non-integer array index (315)" )]
        public void Test_315_slice_selector__non_integer_array_index()
        {
            var selector = "$[1:2:a]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "slice selector, zero step (316)" )]
        public void Test_316_slice_selector__zero_step()
        {
            var selector = "$[1:2:0]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, empty range (317)" )]
        public void Test_317_slice_selector__empty_range()
        {
            var selector = "$[2:2]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, slice selector with everything omitted with empty array (318)" )]
        public void Test_318_slice_selector__slice_selector_with_everything_omitted_with_empty_array()
        {
            var selector = "$[:]";
            var document = JsonNode.Parse(
                """[]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, negative step with empty array (319)" )]
        public void Test_319_slice_selector__negative_step_with_empty_array()
        {
            var selector = "$[::-1]";
            var document = JsonNode.Parse(
                """[]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, maximal range with positive step (320)" )]
        public void Test_320_slice_selector__maximal_range_with_positive_step()
        {
            var selector = "$[0:10]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, maximal range with negative step (321)" )]
        public void Test_321_slice_selector__maximal_range_with_negative_step()
        {
            var selector = "$[9:0:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[9,8,7,6,5,4,3,2,1]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, excessively large to value (322)" )]
        public void Test_322_slice_selector__excessively_large_to_value()
        {
            var selector = "$[2:113667776004]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,3,4,5,6,7,8,9]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, excessively small from value (323)" )]
        public void Test_323_slice_selector__excessively_small_from_value()
        {
            var selector = "$[-113667776004:1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[0]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, excessively large from value with negative step (324)" )]
        public void Test_324_slice_selector__excessively_large_from_value_with_negative_step()
        {
            var selector = "$[113667776004:0:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[9,8,7,6,5,4,3,2,1]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, excessively small to value with negative step (325)" )]
        public void Test_325_slice_selector__excessively_small_to_value_with_negative_step()
        {
            var selector = "$[3:-113667776004:-1]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[3,2,1,0]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, excessively large step (326)" )]
        public void Test_326_slice_selector__excessively_large_step()
        {
            var selector = "$[1:10:113667776004]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[1]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, excessively small step (327)" )]
        public void Test_327_slice_selector__excessively_small_step()
        {
            var selector = "$[-1:-10:-113667776004]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[9]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, overflowing to value (328)" )]
        public void Test_328_slice_selector__overflowing_to_value()
        {
            var selector = "$[2:231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "slice selector, underflowing from value (329)" )]
        public void Test_329_slice_selector__underflowing_from_value()
        {
            var selector = "$[-231584178474632390847141970017375815706539969331281128078915168015826259279872:1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "slice selector, overflowing from value with negative step (330)" )]
        public void Test_330_slice_selector__overflowing_from_value_with_negative_step()
        {
            var selector = "$[231584178474632390847141970017375815706539969331281128078915168015826259279872:0:-1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "slice selector, underflowing to value with negative step (331)" )]
        public void Test_331_slice_selector__underflowing_to_value_with_negative_step()
        {
            var selector = "$[3:-231584178474632390847141970017375815706539969331281128078915168015826259279872:-1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "slice selector, overflowing step (332)" )]
        public void Test_332_slice_selector__overflowing_step()
        {
            var selector = "$[1:10:231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "slice selector, underflowing step (333)" )]
        public void Test_333_slice_selector__underflowing_step()
        {
            var selector = "$[-1:-10:-231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, count, count function (334)" )]
        public void Test_334_functions__count__count_function()
        {
            var selector = "$[?count(@..*)>2]";
            var document = JsonNode.Parse(
                """[{"a":[1,2,3]},{"a":[1],"d":"f"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":[1,2,3]},{"a":[1],"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, count, single-node arg (335)" )]
        public void Test_335_functions__count__single_node_arg()
        {
            var selector = "$[?count(@.a)>1]";
            var document = JsonNode.Parse(
                """[{"a":[1,2,3]},{"a":[1],"d":"f"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, count, multiple-selector arg (336)" )]
        public void Test_336_functions__count__multiple_selector_arg()
        {
            var selector = "$[?count(@['a','d'])>1]";
            var document = JsonNode.Parse(
                """[{"a":[1,2,3]},{"a":[1],"d":"f"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":[1],"d":"f"},{"a":1,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, count, non-query arg, number (337)" )]
        public void Test_337_functions__count__non_query_arg__number()
        {
            var selector = "$[?count(1)>2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, count, non-query arg, string (338)" )]
        public void Test_338_functions__count__non_query_arg__string()
        {
            var selector = "$[?count('string')>2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, count, non-query arg, true (339)" )]
        public void Test_339_functions__count__non_query_arg__true()
        {
            var selector = "$[?count(true)>2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, count, non-query arg, false (340)" )]
        public void Test_340_functions__count__non_query_arg__false()
        {
            var selector = "$[?count(false)>2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, count, non-query arg, null (341)" )]
        public void Test_341_functions__count__non_query_arg__null()
        {
            var selector = "$[?count(null)>2]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, count, result must be compared (342)" )]
        public void Test_342_functions__count__result_must_be_compared()
        {
            var selector = "$[?count(@..*)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, count, no params (343)" )]
        public void Test_343_functions__count__no_params()
        {
            var selector = "$[?count()==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, count, too many params (344)" )]
        public void Test_344_functions__count__too_many_params()
        {
            var selector = "$[?count(@.a,@.b)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, length, string data (345)" )]
        public void Test_345_functions__length__string_data()
        {
            var selector = "$[?length(@.a)>=2]";
            var document = JsonNode.Parse(
                """[{"a":"ab"},{"a":"d"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"ab"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, length, string data, unicode (346)" )]
        public void Test_346_functions__length__string_data__unicode()
        {
            var selector = "$[?length(@)==2]";
            var document = JsonNode.Parse(
                """["☺","☺☺","☺☺☺","ж","жж","жжж","磨","阿美","形声字"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["☺☺","жж","阿美"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, length, array data (347)" )]
        public void Test_347_functions__length__array_data()
        {
            var selector = "$[?length(@.a)>=2]";
            var document = JsonNode.Parse(
                """[{"a":[1,2,3]},{"a":[1]}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":[1,2,3]}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, length, missing data (348)" )]
        public void Test_348_functions__length__missing_data()
        {
            var selector = "$[?length(@.a)>=2]";
            var document = JsonNode.Parse(
                """[{"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, length, number arg (349)" )]
        public void Test_349_functions__length__number_arg()
        {
            var selector = "$[?length(1)>=2]";
            var document = JsonNode.Parse(
                """[{"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, length, true arg (350)" )]
        public void Test_350_functions__length__true_arg()
        {
            var selector = "$[?length(true)>=2]";
            var document = JsonNode.Parse(
                """[{"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, length, false arg (351)" )]
        public void Test_351_functions__length__false_arg()
        {
            var selector = "$[?length(false)>=2]";
            var document = JsonNode.Parse(
                """[{"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, length, null arg (352)" )]
        public void Test_352_functions__length__null_arg()
        {
            var selector = "$[?length(null)>=2]";
            var document = JsonNode.Parse(
                """[{"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, length, result must be compared (353)" )]
        public void Test_353_functions__length__result_must_be_compared()
        {
            var selector = "$[?length(@.a)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, length, no params (354)" )]
        public void Test_354_functions__length__no_params()
        {
            var selector = "$[?length()==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, length, too many params (355)" )]
        public void Test_355_functions__length__too_many_params()
        {
            var selector = "$[?length(@.a,@.b)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, length, non-singular query arg (356)" )]
        public void Test_356_functions__length__non_singular_query_arg()
        {
            var selector = "$[?length(@.*)<3]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, length, arg is a function expression (357)" )]
        public void Test_357_functions__length__arg_is_a_function_expression()
        {
            var selector = "$.values[?length(@.a)==length(value($..c))]";
            var document = JsonNode.Parse(
                """{"c":"cd","values":[{"a":"ab"},{"a":"d"}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"ab"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, length, arg is special nothing (358)" )]
        public void Test_358_functions__length__arg_is_special_nothing()
        {
            var selector = "$[?length(value(@.a))>0]";
            var document = JsonNode.Parse(
                """[{"a":"ab"},{"c":"d"},{"a":null}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"ab"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, found match (359)" )]
        public void Test_359_functions__match__found_match()
        {
            var selector = "$[?match(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"ab"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"ab"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, double quotes (360)" )]
        public void Test_360_functions__match__double_quotes()
        {
            var selector = "$[?match(@.a, \\";
            var document = JsonNode.Parse(
                """[{"a":"ab"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"ab"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, regex from the document (361)" )]
        public void Test_361_functions__match__regex_from_the_document()
        {
            var selector = "$.values[?match(@, $.regex)]";
            var document = JsonNode.Parse(
                """{"regex":"b.?b","values":["abc","bcd","bab","bba","bbab","b",true,[],{}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["bab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, don't select match (362)" )]
        public void Test_362_functions__match__don_t_select_match()
        {
            var selector = "$[?!match(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"ab"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, not a match (363)" )]
        public void Test_363_functions__match__not_a_match()
        {
            var selector = "$[?match(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, select non-match (364)" )]
        public void Test_364_functions__match__select_non_match()
        {
            var selector = "$[?!match(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"bc"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, non-string first arg (365)" )]
        public void Test_365_functions__match__non_string_first_arg()
        {
            var selector = "$[?match(1, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, non-string second arg (366)" )]
        public void Test_366_functions__match__non_string_second_arg()
        {
            var selector = "$[?match(@.a, 1)]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, filter, match function, unicode char class, uppercase (367)" )]
        public void Test_367_functions__match__filter__match_function__unicode_char_class__uppercase()
        {
            var selector = "$[?match(@, '\\\\p{Lu}')]";
            var document = JsonNode.Parse(
                """["ж","Ж","1","жЖ",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["Ж"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, filter, match function, unicode char class negated, uppercase (368)" )]
        public void Test_368_functions__match__filter__match_function__unicode_char_class_negated__uppercase()
        {
            var selector = "$[?match(@, '\\\\P{Lu}')]";
            var document = JsonNode.Parse(
                """["ж","Ж","1",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ж","1"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, filter, match function, unicode, surrogate pair (369)" )]
        public void Test_369_functions__match__filter__match_function__unicode__surrogate_pair()
        {
            var selector = "$[?match(@, 'a.b')]";
            var document = JsonNode.Parse(
                """["a𐄁b","ab","1",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["a𐄁b"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, dot matcher on \u2028 (370)" )]
        public void Test_370_functions__match__dot_matcher_on__u2028()
        {
            var selector = "$[?match(@, '.')]";
            var document = JsonNode.Parse(
                """["\u2028","\r","\n",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["\u2028"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, dot matcher on \u2029 (371)" )]
        public void Test_371_functions__match__dot_matcher_on__u2029()
        {
            var selector = "$[?match(@, '.')]";
            var document = JsonNode.Parse(
                """["\u2029","\r","\n",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["\u2029"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, result cannot be compared (372)" )]
        public void Test_372_functions__match__result_cannot_be_compared()
        {
            var selector = "$[?match(@.a, 'a.*')==true]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, match, too few params (373)" )]
        public void Test_373_functions__match__too_few_params()
        {
            var selector = "$[?match(@.a)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, match, too many params (374)" )]
        public void Test_374_functions__match__too_many_params()
        {
            var selector = "$[?match(@.a,@.b,@.c)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, match, arg is a function expression (375)" )]
        public void Test_375_functions__match__arg_is_a_function_expression()
        {
            var selector = "$.values[?match(@.a, value($..['regex']))]";
            var document = JsonNode.Parse(
                """{"regex":"a.*","values":[{"a":"ab"},{"a":"ba"}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"ab"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, dot in character class (376)" )]
        public void Test_376_functions__match__dot_in_character_class()
        {
            var selector = "$[?match(@, 'a[.b]c')]";
            var document = JsonNode.Parse(
                """["abc","a.c","axc"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["abc","a.c"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, escaped dot (377)" )]
        public void Test_377_functions__match__escaped_dot()
        {
            var selector = "$[?match(@, 'a\\\\.c')]";
            var document = JsonNode.Parse(
                """["abc","a.c","axc"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["a.c"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, escaped backslash before dot (378)" )]
        public void Test_378_functions__match__escaped_backslash_before_dot()
        {
            var selector = "$[?match(@, 'a\\\\\\\\.c')]";
            var document = JsonNode.Parse(
                """["abc","a.c","axc","a\\\u2028c"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["a\\\u2028c"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, escaped left square bracket (379)" )]
        public void Test_379_functions__match__escaped_left_square_bracket()
        {
            var selector = "$[?match(@, 'a\\\\[.c')]";
            var document = JsonNode.Parse(
                """["abc","a.c","a[\u2028c"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["a[\u2028c"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, escaped right square bracket (380)" )]
        public void Test_380_functions__match__escaped_right_square_bracket()
        {
            var selector = "$[?match(@, 'a[\\\\].]c')]";
            var document = JsonNode.Parse(
                """["abc","a.c","a\u2028c","a]c"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["a.c","a]c"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, explicit caret (381)" )]
        public void Test_381_functions__match__explicit_caret()
        {
            var selector = "$[?match(@, '^ab.*')]";
            var document = JsonNode.Parse(
                """["abc","axc","ab","xab"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["abc","ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, explicit dollar (382)" )]
        public void Test_382_functions__match__explicit_dollar()
        {
            var selector = "$[?match(@, '.*bc$')]";
            var document = JsonNode.Parse(
                """["abc","axc","ab","abcx"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["abc"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, at the end (383)" )]
        public void Test_383_functions__search__at_the_end()
        {
            var selector = "$[?search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"the end is ab"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"the end is ab"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, double quotes (384)" )]
        public void Test_384_functions__search__double_quotes()
        {
            var selector = "$[?search(@.a, \\";
            var document = JsonNode.Parse(
                """[{"a":"the end is ab"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"the end is ab"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, at the start (385)" )]
        public void Test_385_functions__search__at_the_start()
        {
            var selector = "$[?search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"ab is at the start"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"ab is at the start"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, in the middle (386)" )]
        public void Test_386_functions__search__in_the_middle()
        {
            var selector = "$[?search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"contains two matches"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"contains two matches"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, regex from the document (387)" )]
        public void Test_387_functions__search__regex_from_the_document()
        {
            var selector = "$.values[?search(@, $.regex)]";
            var document = JsonNode.Parse(
                """{"regex":"b.?b","values":["abc","bcd","bab","bba","bbab","b",true,[],{}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["bab","bba","bbab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, don't select match (388)" )]
        public void Test_388_functions__search__don_t_select_match()
        {
            var selector = "$[?!search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"contains two matches"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, not a match (389)" )]
        public void Test_389_functions__search__not_a_match()
        {
            var selector = "$[?search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, select non-match (390)" )]
        public void Test_390_functions__search__select_non_match()
        {
            var selector = "$[?!search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"bc"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, non-string first arg (391)" )]
        public void Test_391_functions__search__non_string_first_arg()
        {
            var selector = "$[?search(1, 'a.*')]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, non-string second arg (392)" )]
        public void Test_392_functions__search__non_string_second_arg()
        {
            var selector = "$[?search(@.a, 1)]";
            var document = JsonNode.Parse(
                """[{"a":"bc"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, filter, search function, unicode char class, uppercase (393)" )]
        public void Test_393_functions__search__filter__search_function__unicode_char_class__uppercase()
        {
            var selector = "$[?search(@, '\\\\p{Lu}')]";
            var document = JsonNode.Parse(
                """["ж","Ж","1","жЖ",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["Ж","жЖ"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, filter, search function, unicode char class negated, uppercase (394)" )]
        public void Test_394_functions__search__filter__search_function__unicode_char_class_negated__uppercase()
        {
            var selector = "$[?search(@, '\\\\P{Lu}')]";
            var document = JsonNode.Parse(
                """["ж","Ж","1",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ж","1"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, filter, search function, unicode, surrogate pair (395)" )]
        public void Test_395_functions__search__filter__search_function__unicode__surrogate_pair()
        {
            var selector = "$[?search(@, 'a.b')]";
            var document = JsonNode.Parse(
                """["a𐄁bc","abc","1",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["a𐄁bc"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, dot matcher on \u2028 (396)" )]
        public void Test_396_functions__search__dot_matcher_on__u2028()
        {
            var selector = "$[?search(@, '.')]";
            var document = JsonNode.Parse(
                """["\u2028","\r\u2028\n","\r","\n",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["\u2028","\r\u2028\n"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, dot matcher on \u2029 (397)" )]
        public void Test_397_functions__search__dot_matcher_on__u2029()
        {
            var selector = "$[?search(@, '.')]";
            var document = JsonNode.Parse(
                """["\u2029","\r\u2029\n","\r","\n",true,[],{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["\u2029","\r\u2029\n"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, result cannot be compared (398)" )]
        public void Test_398_functions__search__result_cannot_be_compared()
        {
            var selector = "$[?search(@.a, 'a.*')==true]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, search, too few params (399)" )]
        public void Test_399_functions__search__too_few_params()
        {
            var selector = "$[?search(@.a)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, search, too many params (400)" )]
        public void Test_400_functions__search__too_many_params()
        {
            var selector = "$[?search(@.a,@.b,@.c)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, search, arg is a function expression (401)" )]
        public void Test_401_functions__search__arg_is_a_function_expression()
        {
            var selector = "$.values[?search(@, value($..['regex']))]";
            var document = JsonNode.Parse(
                """{"regex":"b.?b","values":["abc","bcd","bab","bba","bbab","b",true,[],{}]}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["bab","bba","bbab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, dot in character class (402)" )]
        public void Test_402_functions__search__dot_in_character_class()
        {
            var selector = "$[?search(@, 'a[.b]c')]";
            var document = JsonNode.Parse(
                """["x abc y","x a.c y","x axc y"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["x abc y","x a.c y"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, escaped dot (403)" )]
        public void Test_403_functions__search__escaped_dot()
        {
            var selector = "$[?search(@, 'a\\\\.c')]";
            var document = JsonNode.Parse(
                """["x abc y","x a.c y","x axc y"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["x a.c y"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, escaped backslash before dot (404)" )]
        public void Test_404_functions__search__escaped_backslash_before_dot()
        {
            var selector = "$[?search(@, 'a\\\\\\\\.c')]";
            var document = JsonNode.Parse(
                """["x abc y","x a.c y","x axc y","x a\\\u2028c y"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["x a\\\u2028c y"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, escaped left square bracket (405)" )]
        public void Test_405_functions__search__escaped_left_square_bracket()
        {
            var selector = "$[?search(@, 'a\\\\[.c')]";
            var document = JsonNode.Parse(
                """["x abc y","x a.c y","x a[\u2028c y"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["x a[\u2028c y"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, escaped right square bracket (406)" )]
        public void Test_406_functions__search__escaped_right_square_bracket()
        {
            var selector = "$[?search(@, 'a[\\\\].]c')]";
            var document = JsonNode.Parse(
                """["x abc y","x a.c y","x a\u2028c y","x a]c y"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["x a.c y","x a]c y"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, value, single-value nodelist (407)" )]
        public void Test_407_functions__value__single_value_nodelist()
        {
            var selector = "$[?value(@.*)==4]";
            var document = JsonNode.Parse(
                """[[4],{"foo":4},[5],{"foo":5},4]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[[4],{"foo":4}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, value, multi-value nodelist (408)" )]
        public void Test_408_functions__value__multi_value_nodelist()
        {
            var selector = "$[?value(@.*)==4]";
            var document = JsonNode.Parse(
                """[[4,4],{"foo":4,"bar":4}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, value, too few params (409)" )]
        public void Test_409_functions__value__too_few_params()
        {
            var selector = "$[?value()==4]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, value, too many params (410)" )]
        public void Test_410_functions__value__too_many_params()
        {
            var selector = "$[?value(@.a,@.b)==4]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "functions, value, result must be compared (411)" )]
        public void Test_411_functions__value__result_must_be_compared()
        {
            var selector = "$[?value(@.a)]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "whitespace, filter, space between question mark and expression (412)" )]
        public void Test_412_whitespace__filter__space_between_question_mark_and_expression()
        {
            var selector = "$[? @.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, filter, newline between question mark and expression (413)" )]
        public void Test_413_whitespace__filter__newline_between_question_mark_and_expression()
        {
            var selector = "$[?\n@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, filter, tab between question mark and expression (414)" )]
        public void Test_414_whitespace__filter__tab_between_question_mark_and_expression()
        {
            var selector = "$[?\t@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, filter, return between question mark and expression (415)" )]
        public void Test_415_whitespace__filter__return_between_question_mark_and_expression()
        {
            var selector = "$[?\r@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, filter, space between question mark and parenthesized expression (416)" )]
        public void Test_416_whitespace__filter__space_between_question_mark_and_parenthesized_expression()
        {
            var selector = "$[? (@.a)]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, filter, newline between question mark and parenthesized expression (417)" )]
        public void Test_417_whitespace__filter__newline_between_question_mark_and_parenthesized_expression()
        {
            var selector = "$[?\n(@.a)]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, filter, tab between question mark and parenthesized expression (418)" )]
        public void Test_418_whitespace__filter__tab_between_question_mark_and_parenthesized_expression()
        {
            var selector = "$[?\t(@.a)]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, filter, return between question mark and parenthesized expression (419)" )]
        public void Test_419_whitespace__filter__return_between_question_mark_and_parenthesized_expression()
        {
            var selector = "$[?\r(@.a)]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, filter, space between parenthesized expression and bracket (420)" )]
        public void Test_420_whitespace__filter__space_between_parenthesized_expression_and_bracket()
        {
            var selector = "$[?(@.a) ]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, filter, newline between parenthesized expression and bracket (421)" )]
        public void Test_421_whitespace__filter__newline_between_parenthesized_expression_and_bracket()
        {
            var selector = "$[?(@.a)\n]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, filter, tab between parenthesized expression and bracket (422)" )]
        public void Test_422_whitespace__filter__tab_between_parenthesized_expression_and_bracket()
        {
            var selector = "$[?(@.a)\t]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, filter, return between parenthesized expression and bracket (423)" )]
        public void Test_423_whitespace__filter__return_between_parenthesized_expression_and_bracket()
        {
            var selector = "$[?(@.a)\r]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, filter, space between bracket and question mark (424)" )]
        public void Test_424_whitespace__filter__space_between_bracket_and_question_mark()
        {
            var selector = "$[ ?@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, filter, newline between bracket and question mark (425)" )]
        public void Test_425_whitespace__filter__newline_between_bracket_and_question_mark()
        {
            var selector = "$[\n?@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, filter, tab between bracket and question mark (426)" )]
        public void Test_426_whitespace__filter__tab_between_bracket_and_question_mark()
        {
            var selector = "$[\t?@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, filter, return between bracket and question mark (427)" )]
        public void Test_427_whitespace__filter__return_between_bracket_and_question_mark()
        {
            var selector = "$[\r?@.a]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"b":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, space between function name and parenthesis (428)" )]
        public void Test_428_whitespace__functions__space_between_function_name_and_parenthesis()
        {
            var selector = "$[?count (@.*)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "whitespace, functions, newline between function name and parenthesis (429)" )]
        public void Test_429_whitespace__functions__newline_between_function_name_and_parenthesis()
        {
            var selector = "$[?count\n(@.*)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "whitespace, functions, tab between function name and parenthesis (430)" )]
        public void Test_430_whitespace__functions__tab_between_function_name_and_parenthesis()
        {
            var selector = "$[?count\t(@.*)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "whitespace, functions, return between function name and parenthesis (431)" )]
        public void Test_431_whitespace__functions__return_between_function_name_and_parenthesis()
        {
            var selector = "$[?count\r(@.*)==1]";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "whitespace, functions, space between parenthesis and arg (432)" )]
        public void Test_432_whitespace__functions__space_between_parenthesis_and_arg()
        {
            var selector = "$[?count( @.*)==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, newline between parenthesis and arg (433)" )]
        public void Test_433_whitespace__functions__newline_between_parenthesis_and_arg()
        {
            var selector = "$[?count(\n@.*)==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, tab between parenthesis and arg (434)" )]
        public void Test_434_whitespace__functions__tab_between_parenthesis_and_arg()
        {
            var selector = "$[?count(\t@.*)==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, return between parenthesis and arg (435)" )]
        public void Test_435_whitespace__functions__return_between_parenthesis_and_arg()
        {
            var selector = "$[?count(\r@.*)==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, space between arg and comma (436)" )]
        public void Test_436_whitespace__functions__space_between_arg_and_comma()
        {
            var selector = "$[?search(@ ,'[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, newline between arg and comma (437)" )]
        public void Test_437_whitespace__functions__newline_between_arg_and_comma()
        {
            var selector = "$[?search(@\n,'[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, tab between arg and comma (438)" )]
        public void Test_438_whitespace__functions__tab_between_arg_and_comma()
        {
            var selector = "$[?search(@\t,'[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, return between arg and comma (439)" )]
        public void Test_439_whitespace__functions__return_between_arg_and_comma()
        {
            var selector = "$[?search(@\r,'[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, space between comma and arg (440)" )]
        public void Test_440_whitespace__functions__space_between_comma_and_arg()
        {
            var selector = "$[?search(@, '[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, newline between comma and arg (441)" )]
        public void Test_441_whitespace__functions__newline_between_comma_and_arg()
        {
            var selector = "$[?search(@,\n'[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, tab between comma and arg (442)" )]
        public void Test_442_whitespace__functions__tab_between_comma_and_arg()
        {
            var selector = "$[?search(@,\t'[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, return between comma and arg (443)" )]
        public void Test_443_whitespace__functions__return_between_comma_and_arg()
        {
            var selector = "$[?search(@,\r'[a-z]+')]";
            var document = JsonNode.Parse(
                """["foo","123"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, space between arg and parenthesis (444)" )]
        public void Test_444_whitespace__functions__space_between_arg_and_parenthesis()
        {
            var selector = "$[?count(@.* )==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, newline between arg and parenthesis (445)" )]
        public void Test_445_whitespace__functions__newline_between_arg_and_parenthesis()
        {
            var selector = "$[?count(@.*\n)==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, tab between arg and parenthesis (446)" )]
        public void Test_446_whitespace__functions__tab_between_arg_and_parenthesis()
        {
            var selector = "$[?count(@.*\t)==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, return between arg and parenthesis (447)" )]
        public void Test_447_whitespace__functions__return_between_arg_and_parenthesis()
        {
            var selector = "$[?count(@.*\r)==1]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, spaces in a relative singular selector (448)" )]
        public void Test_448_whitespace__functions__spaces_in_a_relative_singular_selector()
        {
            var selector = "$[?length(@ .a .b) == 3]";
            var document = JsonNode.Parse(
                """[{"a":{"b":"foo"}},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":{"b":"foo"}}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, newlines in a relative singular selector (449)" )]
        public void Test_449_whitespace__functions__newlines_in_a_relative_singular_selector()
        {
            var selector = "$[?length(@\n.a\n.b) == 3]";
            var document = JsonNode.Parse(
                """[{"a":{"b":"foo"}},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":{"b":"foo"}}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, tabs in a relative singular selector (450)" )]
        public void Test_450_whitespace__functions__tabs_in_a_relative_singular_selector()
        {
            var selector = "$[?length(@\t.a\t.b) == 3]";
            var document = JsonNode.Parse(
                """[{"a":{"b":"foo"}},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":{"b":"foo"}}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, returns in a relative singular selector (451)" )]
        public void Test_451_whitespace__functions__returns_in_a_relative_singular_selector()
        {
            var selector = "$[?length(@\r.a\r.b) == 3]";
            var document = JsonNode.Parse(
                """[{"a":{"b":"foo"}},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":{"b":"foo"}}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, spaces in an absolute singular selector (452)" )]
        public void Test_452_whitespace__functions__spaces_in_an_absolute_singular_selector()
        {
            var selector = "$..[?length(@)==length($ [0] .a)]";
            var document = JsonNode.Parse(
                """[{"a":"foo"},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, newlines in an absolute singular selector (453)" )]
        public void Test_453_whitespace__functions__newlines_in_an_absolute_singular_selector()
        {
            var selector = "$..[?length(@)==length($\n[0]\n.a)]";
            var document = JsonNode.Parse(
                """[{"a":"foo"},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, tabs in an absolute singular selector (454)" )]
        public void Test_454_whitespace__functions__tabs_in_an_absolute_singular_selector()
        {
            var selector = "$..[?length(@)==length($\t[0]\t.a)]";
            var document = JsonNode.Parse(
                """[{"a":"foo"},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, functions, returns in an absolute singular selector (455)" )]
        public void Test_455_whitespace__functions__returns_in_an_absolute_singular_selector()
        {
            var selector = "$..[?length(@)==length($\r[0]\r.a)]";
            var document = JsonNode.Parse(
                """[{"a":"foo"},{}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["foo"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space before || (456)" )]
        public void Test_456_whitespace__operators__space_before___()
        {
            var selector = "$[?@.a ||@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline before || (457)" )]
        public void Test_457_whitespace__operators__newline_before___()
        {
            var selector = "$[?@.a\n||@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab before || (458)" )]
        public void Test_458_whitespace__operators__tab_before___()
        {
            var selector = "$[?@.a\t||@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return before || (459)" )]
        public void Test_459_whitespace__operators__return_before___()
        {
            var selector = "$[?@.a\r||@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space after || (460)" )]
        public void Test_460_whitespace__operators__space_after___()
        {
            var selector = "$[?@.a|| @.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline after || (461)" )]
        public void Test_461_whitespace__operators__newline_after___()
        {
            var selector = "$[?@.a||\n@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab after || (462)" )]
        public void Test_462_whitespace__operators__tab_after___()
        {
            var selector = "$[?@.a||\t@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return after || (463)" )]
        public void Test_463_whitespace__operators__return_after___()
        {
            var selector = "$[?@.a||\r@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"c":3}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1},{"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space before && (464)" )]
        public void Test_464_whitespace__operators__space_before___()
        {
            var selector = "$[?@.a &&@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline before && (465)" )]
        public void Test_465_whitespace__operators__newline_before___()
        {
            var selector = "$[?@.a\n&&@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab before && (466)" )]
        public void Test_466_whitespace__operators__tab_before___()
        {
            var selector = "$[?@.a\t&&@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return before && (467)" )]
        public void Test_467_whitespace__operators__return_before___()
        {
            var selector = "$[?@.a\r&&@.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space after && (468)" )]
        public void Test_468_whitespace__operators__space_after___()
        {
            var selector = "$[?@.a&& @.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline after && (469)" )]
        public void Test_469_whitespace__operators__newline_after___()
        {
            var selector = "$[?@.a&& @.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab after && (470)" )]
        public void Test_470_whitespace__operators__tab_after___()
        {
            var selector = "$[?@.a&& @.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return after && (471)" )]
        public void Test_471_whitespace__operators__return_after___()
        {
            var selector = "$[?@.a&& @.b]";
            var document = JsonNode.Parse(
                """[{"a":1},{"b":2},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space before == (472)" )]
        public void Test_472_whitespace__operators__space_before___()
        {
            var selector = "$[?@.a ==@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline before == (473)" )]
        public void Test_473_whitespace__operators__newline_before___()
        {
            var selector = "$[?@.a\n==@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab before == (474)" )]
        public void Test_474_whitespace__operators__tab_before___()
        {
            var selector = "$[?@.a\t==@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return before == (475)" )]
        public void Test_475_whitespace__operators__return_before___()
        {
            var selector = "$[?@.a\r==@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space after == (476)" )]
        public void Test_476_whitespace__operators__space_after___()
        {
            var selector = "$[?@.a== @.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline after == (477)" )]
        public void Test_477_whitespace__operators__newline_after___()
        {
            var selector = "$[?@.a==\n@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab after == (478)" )]
        public void Test_478_whitespace__operators__tab_after___()
        {
            var selector = "$[?@.a==\t@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return after == (479)" )]
        public void Test_479_whitespace__operators__return_after___()
        {
            var selector = "$[?@.a==\r@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space before != (480)" )]
        public void Test_480_whitespace__operators__space_before___()
        {
            var selector = "$[?@.a !=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline before != (481)" )]
        public void Test_481_whitespace__operators__newline_before___()
        {
            var selector = "$[?@.a\n!=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab before != (482)" )]
        public void Test_482_whitespace__operators__tab_before___()
        {
            var selector = "$[?@.a\t!=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return before != (483)" )]
        public void Test_483_whitespace__operators__return_before___()
        {
            var selector = "$[?@.a\r!=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space after != (484)" )]
        public void Test_484_whitespace__operators__space_after___()
        {
            var selector = "$[?@.a!= @.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline after != (485)" )]
        public void Test_485_whitespace__operators__newline_after___()
        {
            var selector = "$[?@.a!=\n@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab after != (486)" )]
        public void Test_486_whitespace__operators__tab_after___()
        {
            var selector = "$[?@.a!=\t@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return after != (487)" )]
        public void Test_487_whitespace__operators__return_after___()
        {
            var selector = "$[?@.a!=\r@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space before < (488)" )]
        public void Test_488_whitespace__operators__space_before__()
        {
            var selector = "$[?@.a <@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline before < (489)" )]
        public void Test_489_whitespace__operators__newline_before__()
        {
            var selector = "$[?@.a\n<@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab before < (490)" )]
        public void Test_490_whitespace__operators__tab_before__()
        {
            var selector = "$[?@.a\t<@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return before < (491)" )]
        public void Test_491_whitespace__operators__return_before__()
        {
            var selector = "$[?@.a\r<@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space after < (492)" )]
        public void Test_492_whitespace__operators__space_after__()
        {
            var selector = "$[?@.a< @.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline after < (493)" )]
        public void Test_493_whitespace__operators__newline_after__()
        {
            var selector = "$[?@.a<\n@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab after < (494)" )]
        public void Test_494_whitespace__operators__tab_after__()
        {
            var selector = "$[?@.a<\t@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return after < (495)" )]
        public void Test_495_whitespace__operators__return_after__()
        {
            var selector = "$[?@.a<\r@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space before > (496)" )]
        public void Test_496_whitespace__operators__space_before__()
        {
            var selector = "$[?@.b >@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline before > (497)" )]
        public void Test_497_whitespace__operators__newline_before__()
        {
            var selector = "$[?@.b\n>@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab before > (498)" )]
        public void Test_498_whitespace__operators__tab_before__()
        {
            var selector = "$[?@.b\t>@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return before > (499)" )]
        public void Test_499_whitespace__operators__return_before__()
        {
            var selector = "$[?@.b\r>@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space after > (500)" )]
        public void Test_500_whitespace__operators__space_after__()
        {
            var selector = "$[?@.b> @.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline after > (501)" )]
        public void Test_501_whitespace__operators__newline_after__()
        {
            var selector = "$[?@.b>\n@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab after > (502)" )]
        public void Test_502_whitespace__operators__tab_after__()
        {
            var selector = "$[?@.b>\t@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return after > (503)" )]
        public void Test_503_whitespace__operators__return_after__()
        {
            var selector = "$[?@.b>\r@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space before <= (504)" )]
        public void Test_504_whitespace__operators__space_before___()
        {
            var selector = "$[?@.a <=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline before <= (505)" )]
        public void Test_505_whitespace__operators__newline_before___()
        {
            var selector = "$[?@.a\n<=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab before <= (506)" )]
        public void Test_506_whitespace__operators__tab_before___()
        {
            var selector = "$[?@.a\t<=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return before <= (507)" )]
        public void Test_507_whitespace__operators__return_before___()
        {
            var selector = "$[?@.a\r<=@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space after <= (508)" )]
        public void Test_508_whitespace__operators__space_after___()
        {
            var selector = "$[?@.a<= @.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline after <= (509)" )]
        public void Test_509_whitespace__operators__newline_after___()
        {
            var selector = "$[?@.a<=\n@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab after <= (510)" )]
        public void Test_510_whitespace__operators__tab_after___()
        {
            var selector = "$[?@.a<=\t@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return after <= (511)" )]
        public void Test_511_whitespace__operators__return_after___()
        {
            var selector = "$[?@.a<=\r@.b]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space before >= (512)" )]
        public void Test_512_whitespace__operators__space_before___()
        {
            var selector = "$[?@.b >=@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline before >= (513)" )]
        public void Test_513_whitespace__operators__newline_before___()
        {
            var selector = "$[?@.b\n>=@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab before >= (514)" )]
        public void Test_514_whitespace__operators__tab_before___()
        {
            var selector = "$[?@.b\t>=@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return before >= (515)" )]
        public void Test_515_whitespace__operators__return_before___()
        {
            var selector = "$[?@.b\r>=@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space after >= (516)" )]
        public void Test_516_whitespace__operators__space_after___()
        {
            var selector = "$[?@.b>= @.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline after >= (517)" )]
        public void Test_517_whitespace__operators__newline_after___()
        {
            var selector = "$[?@.b>=\n@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab after >= (518)" )]
        public void Test_518_whitespace__operators__tab_after___()
        {
            var selector = "$[?@.b>=\t@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return after >= (519)" )]
        public void Test_519_whitespace__operators__return_after___()
        {
            var selector = "$[?@.b>=\r@.a]";
            var document = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2},{"a":2,"b":1}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"b":1},{"a":1,"b":2}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space between logical not and test expression (520)" )]
        public void Test_520_whitespace__operators__space_between_logical_not_and_test_expression()
        {
            var selector = "$[?! @.a]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline between logical not and test expression (521)" )]
        public void Test_521_whitespace__operators__newline_between_logical_not_and_test_expression()
        {
            var selector = "$[?!\n@.a]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab between logical not and test expression (522)" )]
        public void Test_522_whitespace__operators__tab_between_logical_not_and_test_expression()
        {
            var selector = "$[?!\t@.a]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return between logical not and test expression (523)" )]
        public void Test_523_whitespace__operators__return_between_logical_not_and_test_expression()
        {
            var selector = "$[?!\r@.a]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, space between logical not and parenthesized expression (524)" )]
        public void Test_524_whitespace__operators__space_between_logical_not_and_parenthesized_expression()
        {
            var selector = "$[?! (@.a=='b')]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, newline between logical not and parenthesized expression (525)" )]
        public void Test_525_whitespace__operators__newline_between_logical_not_and_parenthesized_expression()
        {
            var selector = "$[?!\n(@.a=='b')]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, tab between logical not and parenthesized expression (526)" )]
        public void Test_526_whitespace__operators__tab_between_logical_not_and_parenthesized_expression()
        {
            var selector = "$[?!\t(@.a=='b')]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, operators, return between logical not and parenthesized expression (527)" )]
        public void Test_527_whitespace__operators__return_between_logical_not_and_parenthesized_expression()
        {
            var selector = "$[?!\r(@.a=='b')]";
            var document = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"b","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"a","d":"e"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, space between root and bracket (528)" )]
        public void Test_528_whitespace__selectors__space_between_root_and_bracket()
        {
            var selector = "$ ['a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, newline between root and bracket (529)" )]
        public void Test_529_whitespace__selectors__newline_between_root_and_bracket()
        {
            var selector = "$\n['a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, tab between root and bracket (530)" )]
        public void Test_530_whitespace__selectors__tab_between_root_and_bracket()
        {
            var selector = "$\t['a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, return between root and bracket (531)" )]
        public void Test_531_whitespace__selectors__return_between_root_and_bracket()
        {
            var selector = "$\r['a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, space between bracket and bracket (532)" )]
        public void Test_532_whitespace__selectors__space_between_bracket_and_bracket()
        {
            var selector = "$['a'] ['b']";
            var document = JsonNode.Parse(
                """{"a":{"b":"ab"}}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, newline between root and bracket (533)" )]
        public void Test_533_whitespace__selectors__newline_between_root_and_bracket()
        {
            var selector = "$['a'] \n['b']";
            var document = JsonNode.Parse(
                """{"a":{"b":"ab"}}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, tab between root and bracket (534)" )]
        public void Test_534_whitespace__selectors__tab_between_root_and_bracket()
        {
            var selector = "$['a'] \t['b']";
            var document = JsonNode.Parse(
                """{"a":{"b":"ab"}}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, return between root and bracket (535)" )]
        public void Test_535_whitespace__selectors__return_between_root_and_bracket()
        {
            var selector = "$['a'] \r['b']";
            var document = JsonNode.Parse(
                """{"a":{"b":"ab"}}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, space between root and dot (536)" )]
        public void Test_536_whitespace__selectors__space_between_root_and_dot()
        {
            var selector = "$ .a";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, newline between root and dot (537)" )]
        public void Test_537_whitespace__selectors__newline_between_root_and_dot()
        {
            var selector = "$\n.a";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, tab between root and dot (538)" )]
        public void Test_538_whitespace__selectors__tab_between_root_and_dot()
        {
            var selector = "$\t.a";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, return between root and dot (539)" )]
        public void Test_539_whitespace__selectors__return_between_root_and_dot()
        {
            var selector = "$\r.a";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, space between dot and name (540)" )]
        public void Test_540_whitespace__selectors__space_between_dot_and_name()
        {
            var selector = "$. a";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "whitespace, selectors, newline between dot and name (541)" )]
        public void Test_541_whitespace__selectors__newline_between_dot_and_name()
        {
            var selector = "$.\na";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "whitespace, selectors, tab between dot and name (542)" )]
        public void Test_542_whitespace__selectors__tab_between_dot_and_name()
        {
            var selector = "$.\ta";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "whitespace, selectors, return between dot and name (543)" )]
        public void Test_543_whitespace__selectors__return_between_dot_and_name()
        {
            var selector = "$.\ra";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "whitespace, selectors, space between recursive descent and name (544)" )]
        public void Test_544_whitespace__selectors__space_between_recursive_descent_and_name()
        {
            var selector = "$.. a";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "whitespace, selectors, newline between recursive descent and name (545)" )]
        public void Test_545_whitespace__selectors__newline_between_recursive_descent_and_name()
        {
            var selector = "$..\na";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "whitespace, selectors, tab between recursive descent and name (546)" )]
        public void Test_546_whitespace__selectors__tab_between_recursive_descent_and_name()
        {
            var selector = "$..\ta";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "whitespace, selectors, return between recursive descent and name (547)" )]
        public void Test_547_whitespace__selectors__return_between_recursive_descent_and_name()
        {
            var selector = "$..\ra";
            var document = new JsonObject(); // Empty node
            Assert.ThrowsException<NotSupportedException>( () => document.Select( selector ).ToArray() );
        }


        [TestMethod( "whitespace, selectors, space between bracket and selector (548)" )]
        public void Test_548_whitespace__selectors__space_between_bracket_and_selector()
        {
            var selector = "$[ 'a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, newline between bracket and selector (549)" )]
        public void Test_549_whitespace__selectors__newline_between_bracket_and_selector()
        {
            var selector = "$[\n'a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, tab between bracket and selector (550)" )]
        public void Test_550_whitespace__selectors__tab_between_bracket_and_selector()
        {
            var selector = "$[\t'a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, return between bracket and selector (551)" )]
        public void Test_551_whitespace__selectors__return_between_bracket_and_selector()
        {
            var selector = "$[\r'a']";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, space between selector and bracket (552)" )]
        public void Test_552_whitespace__selectors__space_between_selector_and_bracket()
        {
            var selector = "$['a' ]";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, newline between selector and bracket (553)" )]
        public void Test_553_whitespace__selectors__newline_between_selector_and_bracket()
        {
            var selector = "$['a'\n]";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, tab between selector and bracket (554)" )]
        public void Test_554_whitespace__selectors__tab_between_selector_and_bracket()
        {
            var selector = "$['a'\t]";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, return between selector and bracket (555)" )]
        public void Test_555_whitespace__selectors__return_between_selector_and_bracket()
        {
            var selector = "$['a'\r]";
            var document = JsonNode.Parse(
                """{"a":"ab"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, space between selector and comma (556)" )]
        public void Test_556_whitespace__selectors__space_between_selector_and_comma()
        {
            var selector = "$['a' ,'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, newline between selector and comma (557)" )]
        public void Test_557_whitespace__selectors__newline_between_selector_and_comma()
        {
            var selector = "$['a'\n,'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, tab between selector and comma (558)" )]
        public void Test_558_whitespace__selectors__tab_between_selector_and_comma()
        {
            var selector = "$['a'\t,'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, return between selector and comma (559)" )]
        public void Test_559_whitespace__selectors__return_between_selector_and_comma()
        {
            var selector = "$['a'\r,'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, space between comma and selector (560)" )]
        public void Test_560_whitespace__selectors__space_between_comma_and_selector()
        {
            var selector = "$['a', 'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, newline between comma and selector (561)" )]
        public void Test_561_whitespace__selectors__newline_between_comma_and_selector()
        {
            var selector = "$['a',\n'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, tab between comma and selector (562)" )]
        public void Test_562_whitespace__selectors__tab_between_comma_and_selector()
        {
            var selector = "$['a',\t'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, return between comma and selector (563)" )]
        public void Test_563_whitespace__selectors__return_between_comma_and_selector()
        {
            var selector = "$['a',\r'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, space between start and colon (564)" )]
        public void Test_564_whitespace__slice__space_between_start_and_colon()
        {
            var selector = "$[1 :5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, newline between start and colon (565)" )]
        public void Test_565_whitespace__slice__newline_between_start_and_colon()
        {
            var selector = "$[1\n:5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, tab between start and colon (566)" )]
        public void Test_566_whitespace__slice__tab_between_start_and_colon()
        {
            var selector = "$[1\t:5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, return between start and colon (567)" )]
        public void Test_567_whitespace__slice__return_between_start_and_colon()
        {
            var selector = "$[1\r:5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, space between colon and end (568)" )]
        public void Test_568_whitespace__slice__space_between_colon_and_end()
        {
            var selector = "$[1: 5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, newline between colon and end (569)" )]
        public void Test_569_whitespace__slice__newline_between_colon_and_end()
        {
            var selector = "$[1:\n5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, tab between colon and end (570)" )]
        public void Test_570_whitespace__slice__tab_between_colon_and_end()
        {
            var selector = "$[1:\t5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, return between colon and end (571)" )]
        public void Test_571_whitespace__slice__return_between_colon_and_end()
        {
            var selector = "$[1:\r5:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, space between end and colon (572)" )]
        public void Test_572_whitespace__slice__space_between_end_and_colon()
        {
            var selector = "$[1:5 :2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, newline between end and colon (573)" )]
        public void Test_573_whitespace__slice__newline_between_end_and_colon()
        {
            var selector = "$[1:5\n:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, tab between end and colon (574)" )]
        public void Test_574_whitespace__slice__tab_between_end_and_colon()
        {
            var selector = "$[1:5\t:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, return between end and colon (575)" )]
        public void Test_575_whitespace__slice__return_between_end_and_colon()
        {
            var selector = "$[1:5\r:2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, space between colon and step (576)" )]
        public void Test_576_whitespace__slice__space_between_colon_and_step()
        {
            var selector = "$[1:5: 2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, newline between colon and step (577)" )]
        public void Test_577_whitespace__slice__newline_between_colon_and_step()
        {
            var selector = "$[1:5:\n2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, tab between colon and step (578)" )]
        public void Test_578_whitespace__slice__tab_between_colon_and_step()
        {
            var selector = "$[1:5:\t2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, slice, return between colon and step (579)" )]
        public void Test_579_whitespace__slice__return_between_colon_and_step()
        {
            var selector = "$[1:5:\r2]";
            var document = JsonNode.Parse(
                """[1,2,3,4,5,6]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[2,4]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }
    }
}

