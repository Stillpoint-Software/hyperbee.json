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
        public void Test_basic__root_1()
        {
            var selector = "$";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector ).ToArray();
            var expect = JsonNode.Parse(
                """[["first","second"]]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "basic, no leading whitespace (2)" )]
        public void Test_basic__no_leading_whitespace_2()
        {
            var selector = " $";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "basic, no trailing whitespace (3)" )]
        public void Test_basic__no_trailing_whitespace_3()
        {
            var selector = "$ ";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "basic, name shorthand (4)" )]
        public void Test_basic__name_shorthand_4()
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
        public void Test_basic__name_shorthand__extended_unicode___5()
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
        public void Test_basic__name_shorthand__underscore_6()
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
        public void Test_basic__name_shorthand__symbol_7()
        {
            var selector = "$.&";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "basic, name shorthand, number (8)" )]
        public void Test_basic__name_shorthand__number_8()
        {
            var selector = "$.1";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "basic, name shorthand, absent data (9)" )]
        public void Test_basic__name_shorthand__absent_data_9()
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
        public void Test_basic__name_shorthand__array_data_10()
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
        public void Test_basic__wildcard_shorthand__object_data_11()
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
        public void Test_basic__wildcard_shorthand__array_data_12()
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
        public void Test_basic__wildcard_selector__array_data_13()
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
        public void Test_basic__wildcard_shorthand__then_name_shorthand_14()
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
        public void Test_basic__multiple_selectors_15()
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
        public void Test_basic__multiple_selectors__space_instead_of_comma_16()
        {
            var selector = "$[0 2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "basic, multiple selectors, name and index, array data (17)" )]
        public void Test_basic__multiple_selectors__name_and_index__array_data_17()
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
        public void Test_basic__multiple_selectors__name_and_index__object_data_18()
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
        public void Test_basic__multiple_selectors__index_and_slice_19()
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
        public void Test_basic__multiple_selectors__index_and_slice__overlapping_20()
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
        public void Test_basic__multiple_selectors__duplicate_index_21()
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
        public void Test_basic__multiple_selectors__wildcard_and_index_22()
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
        public void Test_basic__multiple_selectors__wildcard_and_name_23()
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
        public void Test_basic__multiple_selectors__wildcard_and_slice_24()
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
        public void Test_basic__multiple_selectors__multiple_wildcards_25()
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
        public void Test_basic__empty_segment_26()
        {
            var selector = "$[]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "basic, descendant segment, index (27)" )]
        public void Test_basic__descendant_segment__index_27()
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
        public void Test_basic__descendant_segment__name_shorthand_28()
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
        public void Test_basic__descendant_segment__wildcard_shorthand__array_data_29()
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
        public void Test_basic__descendant_segment__wildcard_selector__array_data_30()
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
        public void Test_basic__descendant_segment__wildcard_selector__nested_arrays_31()
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
        public void Test_basic__descendant_segment__wildcard_selector__nested_objects_32()
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
        public void Test_basic__descendant_segment__wildcard_shorthand__object_data_33()
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
        public void Test_basic__descendant_segment__wildcard_shorthand__nested_data_34()
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
        public void Test_basic__descendant_segment__multiple_selectors_35()
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
        public void Test_basic__descendant_segment__object_traversal__multiple_selectors_36()
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
        public void Test_basic__bald_descendant_segment_37()
        {
            var selector = "$..";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, existence, without segments (38)" )]
        public void Test_filter__existence__without_segments_38()
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
        public void Test_filter__existence_39()
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
        public void Test_filter__existence__present_with_null_40()
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
        public void Test_filter__equals_string__single_quotes_41()
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
        public void Test_filter__equals_numeric_string__single_quotes_42()
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
        public void Test_filter__equals_string__double_quotes_43()
        {
            var selector = "$[?@.a==\"b\"]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals numeric string, double quotes (44)" )]
        public void Test_filter__equals_numeric_string__double_quotes_44()
        {
            var selector = "$[?@.a==\"1\"]";
            var document = JsonNode.Parse(
                """[{"a":"1","d":"e"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"1","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, equals number (45)" )]
        public void Test_filter__equals_number_45()
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
        public void Test_filter__equals_null_46()
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
        public void Test_filter__equals_null__absent_from_data_47()
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
        public void Test_filter__equals_true_48()
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
        public void Test_filter__equals_false_49()
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
        public void Test_filter__equals_self_50()
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
        public void Test_filter__deep_equality__arrays_51()
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
        public void Test_filter__deep_equality__objects_52()
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
        public void Test_filter__not_equals_string__single_quotes_53()
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
        public void Test_filter__not_equals_numeric_string__single_quotes_54()
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
        public void Test_filter__not_equals_string__single_quotes__different_type_55()
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
        public void Test_filter__not_equals_string__double_quotes_56()
        {
            var selector = "$[?@.a!=\"b\"]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not-equals numeric string, double quotes (57)" )]
        public void Test_filter__not_equals_numeric_string__double_quotes_57()
        {
            var selector = "$[?@.a!=\"1\"]";
            var document = JsonNode.Parse(
                """[{"a":"1","d":"e"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not-equals string, double quotes, different types (58)" )]
        public void Test_filter__not_equals_string__double_quotes__different_types_58()
        {
            var selector = "$[?@.a!=\"b\"]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":1,"d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":1,"d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, not-equals number (59)" )]
        public void Test_filter__not_equals_number_59()
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
        public void Test_filter__not_equals_number__different_types_60()
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
        public void Test_filter__not_equals_null_61()
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
        public void Test_filter__not_equals_null__absent_from_data_62()
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
        public void Test_filter__not_equals_true_63()
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
        public void Test_filter__not_equals_false_64()
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
        public void Test_filter__less_than_string__single_quotes_65()
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
        public void Test_filter__less_than_string__double_quotes_66()
        {
            var selector = "$[?@.a<\"c\"]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, less than number (67)" )]
        public void Test_filter__less_than_number_67()
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
        public void Test_filter__less_than_null_68()
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
        public void Test_filter__less_than_true_69()
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
        public void Test_filter__less_than_false_70()
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
        public void Test_filter__less_than_or_equal_to_string__single_quotes_71()
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
        public void Test_filter__less_than_or_equal_to_string__double_quotes_72()
        {
            var selector = "$[?@.a<=\"c\"]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, less than or equal to number (73)" )]
        public void Test_filter__less_than_or_equal_to_number_73()
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
        public void Test_filter__less_than_or_equal_to_null_74()
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
        public void Test_filter__less_than_or_equal_to_true_75()
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
        public void Test_filter__less_than_or_equal_to_false_76()
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
        public void Test_filter__greater_than_string__single_quotes_77()
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
        public void Test_filter__greater_than_string__double_quotes_78()
        {
            var selector = "$[?@.a>\"c\"]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, greater than number (79)" )]
        public void Test_filter__greater_than_number_79()
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
        public void Test_filter__greater_than_null_80()
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
        public void Test_filter__greater_than_true_81()
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
        public void Test_filter__greater_than_false_82()
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
        public void Test_filter__greater_than_or_equal_to_string__single_quotes_83()
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
        public void Test_filter__greater_than_or_equal_to_string__double_quotes_84()
        {
            var selector = "$[?@.a>=\"c\"]";
            var document = JsonNode.Parse(
                """[{"a":"b","d":"e"},{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"c","d":"f"},{"a":"d","d":"f"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, greater than or equal to number (85)" )]
        public void Test_filter__greater_than_or_equal_to_number_85()
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
        public void Test_filter__greater_than_or_equal_to_null_86()
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
        public void Test_filter__greater_than_or_equal_to_true_87()
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
        public void Test_filter__greater_than_or_equal_to_false_88()
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
        public void Test_filter__exists_and_not_equals_null__absent_from_data_89()
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
        public void Test_filter__exists_and_exists__data_false_90()
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
        public void Test_filter__exists_or_exists__data_false_91()
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
        public void Test_filter__and_92()
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
        public void Test_filter__or_93()
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
        public void Test_filter__not_expression_94()
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
        public void Test_filter__not_exists_95()
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
        public void Test_filter__not_exists__data_null_96()
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
        public void Test_filter__non_singular_existence__wildcard_97()
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
        public void Test_filter__non_singular_existence__multiple_98()
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
        public void Test_filter__non_singular_existence__slice_99()
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
        public void Test_filter__non_singular_existence__negated_100()
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
        public void Test_filter__non_singular_query_in_comparison__slice_101()
        {
            var selector = "$[?@[0:0]==0]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, non-singular query in comparison, all children (102)" )]
        public void Test_filter__non_singular_query_in_comparison__all_children_102()
        {
            var selector = "$[?@[*]==0]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, non-singular query in comparison, descendants (103)" )]
        public void Test_filter__non_singular_query_in_comparison__descendants_103()
        {
            var selector = "$[?@..a==0]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, non-singular query in comparison, combined (104)" )]
        public void Test_filter__non_singular_query_in_comparison__combined_104()
        {
            var selector = "$[?@.a[*].a==0]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, nested (105)" )]
        public void Test_filter__nested_105()
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
        public void Test_filter__name_segment_on_primitive__selects_nothing_106()
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
        public void Test_filter__name_segment_on_array__selects_nothing_107()
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
        public void Test_filter__index_segment_on_object__selects_nothing_108()
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
        public void Test_filter__relative_non_singular_query__index__equal_109()
        {
            var selector = "$[?(@[0, 0]==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, relative non-singular query, index, not equal (110)" )]
        public void Test_filter__relative_non_singular_query__index__not_equal_110()
        {
            var selector = "$[?(@[0, 0]!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, relative non-singular query, index, less-or-equal (111)" )]
        public void Test_filter__relative_non_singular_query__index__less_or_equal_111()
        {
            var selector = "$[?(@[0, 0]<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, relative non-singular query, name, equal (112)" )]
        public void Test_filter__relative_non_singular_query__name__equal_112()
        {
            var selector = "$[?(@['a', 'a']==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, relative non-singular query, name, not equal (113)" )]
        public void Test_filter__relative_non_singular_query__name__not_equal_113()
        {
            var selector = "$[?(@['a', 'a']!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, relative non-singular query, name, less-or-equal (114)" )]
        public void Test_filter__relative_non_singular_query__name__less_or_equal_114()
        {
            var selector = "$[?(@['a', 'a']<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, relative non-singular query, combined, equal (115)" )]
        public void Test_filter__relative_non_singular_query__combined__equal_115()
        {
            var selector = "$[?(@[0, '0']==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, relative non-singular query, combined, not equal (116)" )]
        public void Test_filter__relative_non_singular_query__combined__not_equal_116()
        {
            var selector = "$[?(@[0, '0']!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, relative non-singular query, combined, less-or-equal (117)" )]
        public void Test_filter__relative_non_singular_query__combined__less_or_equal_117()
        {
            var selector = "$[?(@[0, '0']<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, relative non-singular query, wildcard, equal (118)" )]
        public void Test_filter__relative_non_singular_query__wildcard__equal_118()
        {
            var selector = "$[?(@.*==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, relative non-singular query, wildcard, not equal (119)" )]
        public void Test_filter__relative_non_singular_query__wildcard__not_equal_119()
        {
            var selector = "$[?(@.*!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, relative non-singular query, wildcard, less-or-equal (120)" )]
        public void Test_filter__relative_non_singular_query__wildcard__less_or_equal_120()
        {
            var selector = "$[?(@.*<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, relative non-singular query, slice, equal (121)" )]
        public void Test_filter__relative_non_singular_query__slice__equal_121()
        {
            var selector = "$[?(@[0:0]==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, relative non-singular query, slice, not equal (122)" )]
        public void Test_filter__relative_non_singular_query__slice__not_equal_122()
        {
            var selector = "$[?(@[0:0]!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, relative non-singular query, slice, less-or-equal (123)" )]
        public void Test_filter__relative_non_singular_query__slice__less_or_equal_123()
        {
            var selector = "$[?(@[0:0]<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, absolute non-singular query, index, equal (124)" )]
        public void Test_filter__absolute_non_singular_query__index__equal_124()
        {
            var selector = "$[?($[0, 0]==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, absolute non-singular query, index, not equal (125)" )]
        public void Test_filter__absolute_non_singular_query__index__not_equal_125()
        {
            var selector = "$[?($[0, 0]!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, absolute non-singular query, index, less-or-equal (126)" )]
        public void Test_filter__absolute_non_singular_query__index__less_or_equal_126()
        {
            var selector = "$[?($[0, 0]<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, absolute non-singular query, name, equal (127)" )]
        public void Test_filter__absolute_non_singular_query__name__equal_127()
        {
            var selector = "$[?($['a', 'a']==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, absolute non-singular query, name, not equal (128)" )]
        public void Test_filter__absolute_non_singular_query__name__not_equal_128()
        {
            var selector = "$[?($['a', 'a']!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, absolute non-singular query, name, less-or-equal (129)" )]
        public void Test_filter__absolute_non_singular_query__name__less_or_equal_129()
        {
            var selector = "$[?($['a', 'a']<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, absolute non-singular query, combined, equal (130)" )]
        public void Test_filter__absolute_non_singular_query__combined__equal_130()
        {
            var selector = "$[?($[0, '0']==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, absolute non-singular query, combined, not equal (131)" )]
        public void Test_filter__absolute_non_singular_query__combined__not_equal_131()
        {
            var selector = "$[?($[0, '0']!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, absolute non-singular query, combined, less-or-equal (132)" )]
        public void Test_filter__absolute_non_singular_query__combined__less_or_equal_132()
        {
            var selector = "$[?($[0, '0']<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, absolute non-singular query, wildcard, equal (133)" )]
        public void Test_filter__absolute_non_singular_query__wildcard__equal_133()
        {
            var selector = "$[?($.*==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, absolute non-singular query, wildcard, not equal (134)" )]
        public void Test_filter__absolute_non_singular_query__wildcard__not_equal_134()
        {
            var selector = "$[?($.*!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, absolute non-singular query, wildcard, less-or-equal (135)" )]
        public void Test_filter__absolute_non_singular_query__wildcard__less_or_equal_135()
        {
            var selector = "$[?($.*<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, absolute non-singular query, slice, equal (136)" )]
        public void Test_filter__absolute_non_singular_query__slice__equal_136()
        {
            var selector = "$[?($[0:0]==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, absolute non-singular query, slice, not equal (137)" )]
        public void Test_filter__absolute_non_singular_query__slice__not_equal_137()
        {
            var selector = "$[?($[0:0]!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, absolute non-singular query, slice, less-or-equal (138)" )]
        public void Test_filter__absolute_non_singular_query__slice__less_or_equal_138()
        {
            var selector = "$[?($[0:0]<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, multiple selectors (139)" )]
        public void Test_filter__multiple_selectors_139()
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
        public void Test_filter__multiple_selectors__comparison_140()
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
        public void Test_filter__multiple_selectors__overlapping_141()
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
        public void Test_filter__multiple_selectors__filter_and_index_142()
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
        public void Test_filter__multiple_selectors__filter_and_wildcard_143()
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
        public void Test_filter__multiple_selectors__filter_and_slice_144()
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
        public void Test_filter__multiple_selectors__comparison_filter__index_and_slice_145()
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
        public void Test_filter__equals_number__zero_and_negative_zero_146()
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
        public void Test_filter__equals_number__with_and_without_decimal_fraction_147()
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
        public void Test_filter__equals_number__exponent_148()
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
        public void Test_filter__equals_number__positive_exponent_149()
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
        public void Test_filter__equals_number__negative_exponent_150()
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
        public void Test_filter__equals_number__decimal_fraction_151()
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
        public void Test_filter__equals_number__decimal_fraction__no_fractional_digit_152()
        {
            var selector = "$[?@.a==1.]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, equals number, decimal fraction, exponent (153)" )]
        public void Test_filter__equals_number__decimal_fraction__exponent_153()
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
        public void Test_filter__equals_number__decimal_fraction__positive_exponent_154()
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
        public void Test_filter__equals_number__decimal_fraction__negative_exponent_155()
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
        public void Test_filter__equals__special_nothing_156()
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
        public void Test_filter__equals__empty_node_list_and_empty_node_list_157()
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
        public void Test_filter__equals__empty_node_list_and_special_nothing_158()
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
        public void Test_filter__object_data_159()
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
        public void Test_filter__and_binds_more_tightly_than_or_160()
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
        public void Test_filter__left_to_right_evaluation_161()
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
        public void Test_filter__group_terms__left_162()
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
        public void Test_filter__group_terms__right_163()
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
        public void Test_filter__string_literal__single_quote_in_double_quotes_164()
        {
            var selector = "$[?@ == \"quoted' literal\"]";
            var document = JsonNode.Parse(
                """["quoted' literal","a","quoted\\' literal"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["quoted' literal"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, string literal, double quote in single quotes (165)" )]
        public void Test_filter__string_literal__double_quote_in_single_quotes_165()
        {
            var selector = "$[?@ == 'quoted\" literal']";
            var document = JsonNode.Parse(
                """["quoted\" literal","a","quoted\\\" literal","'quoted\" literal'"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["quoted\" literal"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, string literal, escaped single quote in single quotes (166)" )]
        public void Test_filter__string_literal__escaped_single_quote_in_single_quotes_166()
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
        public void Test_filter__string_literal__escaped_double_quote_in_double_quotes_167()
        {
            var selector = "$[?@ == \"quoted\\\" literal\"]";
            var document = JsonNode.Parse(
                """["quoted\" literal","a","quoted\\\" literal","'quoted\" literal'"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["quoted\" literal"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "filter, literal true must be compared (168)" )]
        public void Test_filter__literal_true_must_be_compared_168()
        {
            var selector = "$[?true]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, literal false must be compared (169)" )]
        public void Test_filter__literal_false_must_be_compared_169()
        {
            var selector = "$[?false]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, literal string must be compared (170)" )]
        public void Test_filter__literal_string_must_be_compared_170()
        {
            var selector = "$[?'abc']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, literal int must be compared (171)" )]
        public void Test_filter__literal_int_must_be_compared_171()
        {
            var selector = "$[?2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, literal float must be compared (172)" )]
        public void Test_filter__literal_float_must_be_compared_172()
        {
            var selector = "$[?2.2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, literal null must be compared (173)" )]
        public void Test_filter__literal_null_must_be_compared_173()
        {
            var selector = "$[?null]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, and, literals must be compared (174)" )]
        public void Test_filter__and__literals_must_be_compared_174()
        {
            var selector = "$[?true && false]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, or, literals must be compared (175)" )]
        public void Test_filter__or__literals_must_be_compared_175()
        {
            var selector = "$[?true || false]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, and, right hand literal must be compared (176)" )]
        public void Test_filter__and__right_hand_literal_must_be_compared_176()
        {
            var selector = "$[?true == false && false]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, or, right hand literal must be compared (177)" )]
        public void Test_filter__or__right_hand_literal_must_be_compared_177()
        {
            var selector = "$[?true == false || false]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, and, left hand literal must be compared (178)" )]
        public void Test_filter__and__left_hand_literal_must_be_compared_178()
        {
            var selector = "$[?false && true == false]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "filter, or, left hand literal must be compared (179)" )]
        public void Test_filter__or__left_hand_literal_must_be_compared_179()
        {
            var selector = "$[?false || true == false]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "index selector, first element (180)" )]
        public void Test_index_selector__first_element_180()
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
        public void Test_index_selector__second_element_181()
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
        public void Test_index_selector__out_of_bound_182()
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
        public void Test_index_selector__overflowing_index_183()
        {
            var selector = "$[231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "index selector, not actually an index, overflowing index leads into general text (184)" )]
        public void Test_index_selector__not_actually_an_index__overflowing_index_leads_into_general_text_184()
        {
            var selector = "$[231584178474632390847141970017375815706539969331281128078915168SomeRandomText]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "index selector, negative (185)" )]
        public void Test_index_selector__negative_185()
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
        public void Test_index_selector__more_negative_186()
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
        public void Test_index_selector__negative_out_of_bound_187()
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
        public void Test_index_selector__on_object_188()
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
        public void Test_index_selector__leading_0_189()
        {
            var selector = "$[01]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "index selector, leading -0 (190)" )]
        public void Test_index_selector__leading__0_190()
        {
            var selector = "$[-01]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes (191)" )]
        public void Test_name_selector__double_quotes_191()
        {
            var selector = "$[\"a\"]";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, absent data (192)" )]
        public void Test_name_selector__double_quotes__absent_data_192()
        {
            var selector = "$[\"c\"]";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, array data (193)" )]
        public void Test_name_selector__double_quotes__array_data_193()
        {
            var selector = "$[\"a\"]";
            var document = JsonNode.Parse(
                """["first","second"]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, embedded U+0000 (194)" )]
        public void Test_name_selector__double_quotes__embedded_U_0000_194()
        {
            var selector = "$[\"\u0000\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0001 (195)" )]
        public void Test_name_selector__double_quotes__embedded_U_0001_195()
        {
            var selector = "$[\"\u0001\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0002 (196)" )]
        public void Test_name_selector__double_quotes__embedded_U_0002_196()
        {
            var selector = "$[\"\u0002\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0003 (197)" )]
        public void Test_name_selector__double_quotes__embedded_U_0003_197()
        {
            var selector = "$[\"\u0003\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0004 (198)" )]
        public void Test_name_selector__double_quotes__embedded_U_0004_198()
        {
            var selector = "$[\"\u0004\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0005 (199)" )]
        public void Test_name_selector__double_quotes__embedded_U_0005_199()
        {
            var selector = "$[\"\u0005\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0006 (200)" )]
        public void Test_name_selector__double_quotes__embedded_U_0006_200()
        {
            var selector = "$[\"\u0006\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0007 (201)" )]
        public void Test_name_selector__double_quotes__embedded_U_0007_201()
        {
            var selector = "$[\"\u0007\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0008 (202)" )]
        public void Test_name_selector__double_quotes__embedded_U_0008_202()
        {
            var selector = "$[\"\b\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0009 (203)" )]
        public void Test_name_selector__double_quotes__embedded_U_0009_203()
        {
            var selector = "$[\"\t\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+000A (204)" )]
        public void Test_name_selector__double_quotes__embedded_U_000A_204()
        {
            var selector = "$[\"\n\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+000B (205)" )]
        public void Test_name_selector__double_quotes__embedded_U_000B_205()
        {
            var selector = "$[\"\u000b\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+000C (206)" )]
        public void Test_name_selector__double_quotes__embedded_U_000C_206()
        {
            var selector = "$[\"\f\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+000D (207)" )]
        public void Test_name_selector__double_quotes__embedded_U_000D_207()
        {
            var selector = "$[\"\r\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+000E (208)" )]
        public void Test_name_selector__double_quotes__embedded_U_000E_208()
        {
            var selector = "$[\"\u000e\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+000F (209)" )]
        public void Test_name_selector__double_quotes__embedded_U_000F_209()
        {
            var selector = "$[\"\u000f\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0010 (210)" )]
        public void Test_name_selector__double_quotes__embedded_U_0010_210()
        {
            var selector = "$[\"\u0010\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0011 (211)" )]
        public void Test_name_selector__double_quotes__embedded_U_0011_211()
        {
            var selector = "$[\"\u0011\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0012 (212)" )]
        public void Test_name_selector__double_quotes__embedded_U_0012_212()
        {
            var selector = "$[\"\u0012\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0013 (213)" )]
        public void Test_name_selector__double_quotes__embedded_U_0013_213()
        {
            var selector = "$[\"\u0013\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0014 (214)" )]
        public void Test_name_selector__double_quotes__embedded_U_0014_214()
        {
            var selector = "$[\"\u0014\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0015 (215)" )]
        public void Test_name_selector__double_quotes__embedded_U_0015_215()
        {
            var selector = "$[\"\u0015\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0016 (216)" )]
        public void Test_name_selector__double_quotes__embedded_U_0016_216()
        {
            var selector = "$[\"\u0016\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0017 (217)" )]
        public void Test_name_selector__double_quotes__embedded_U_0017_217()
        {
            var selector = "$[\"\u0017\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0018 (218)" )]
        public void Test_name_selector__double_quotes__embedded_U_0018_218()
        {
            var selector = "$[\"\u0018\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0019 (219)" )]
        public void Test_name_selector__double_quotes__embedded_U_0019_219()
        {
            var selector = "$[\"\u0019\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+001A (220)" )]
        public void Test_name_selector__double_quotes__embedded_U_001A_220()
        {
            var selector = "$[\"\u001a\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+001B (221)" )]
        public void Test_name_selector__double_quotes__embedded_U_001B_221()
        {
            var selector = "$[\"\u001b\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+001C (222)" )]
        public void Test_name_selector__double_quotes__embedded_U_001C_222()
        {
            var selector = "$[\"\u001c\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+001D (223)" )]
        public void Test_name_selector__double_quotes__embedded_U_001D_223()
        {
            var selector = "$[\"\u001d\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+001E (224)" )]
        public void Test_name_selector__double_quotes__embedded_U_001E_224()
        {
            var selector = "$[\"\u001e\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+001F (225)" )]
        public void Test_name_selector__double_quotes__embedded_U_001F_225()
        {
            var selector = "$[\"\u001f\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded U+0020 (226)" )]
        public void Test_name_selector__double_quotes__embedded_U_0020_226()
        {
            var selector = "$[\" \"]";
            var document = JsonNode.Parse(
                """{" ":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped double quote (227)" )]
        public void Test_name_selector__double_quotes__escaped_double_quote_227()
        {
            var selector = "$[\"\\\"\"]";
            var document = JsonNode.Parse(
                """{"\"":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped reverse solidus (228)" )]
        public void Test_name_selector__double_quotes__escaped_reverse_solidus_228()
        {
            var selector = "$[\"\\\\\"]";
            var document = JsonNode.Parse(
                """{"\\":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped solidus (229)" )]
        public void Test_name_selector__double_quotes__escaped_solidus_229()
        {
            var selector = "$[\"\\/\"]";
            var document = JsonNode.Parse(
                """{"/":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped backspace (230)" )]
        public void Test_name_selector__double_quotes__escaped_backspace_230()
        {
            var selector = "$[\"\\b\"]";
            var document = JsonNode.Parse(
                """{"\b":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped form feed (231)" )]
        public void Test_name_selector__double_quotes__escaped_form_feed_231()
        {
            var selector = "$[\"\\f\"]";
            var document = JsonNode.Parse(
                """{"\f":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped line feed (232)" )]
        public void Test_name_selector__double_quotes__escaped_line_feed_232()
        {
            var selector = "$[\"\\n\"]";
            var document = JsonNode.Parse(
                """{"\n":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped carriage return (233)" )]
        public void Test_name_selector__double_quotes__escaped_carriage_return_233()
        {
            var selector = "$[\"\\r\"]";
            var document = JsonNode.Parse(
                """{"\r":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped tab (234)" )]
        public void Test_name_selector__double_quotes__escaped_tab_234()
        {
            var selector = "$[\"\\t\"]";
            var document = JsonNode.Parse(
                """{"\t":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped ☺, upper case hex (235)" )]
        public void Test_name_selector__double_quotes__escaped____upper_case_hex_235()
        {
            var selector = "$[\"\\u263A\"]";
            var document = JsonNode.Parse(
                """{"☺":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, escaped ☺, lower case hex (236)" )]
        public void Test_name_selector__double_quotes__escaped____lower_case_hex_236()
        {
            var selector = "$[\"\\u263a\"]";
            var document = JsonNode.Parse(
                """{"☺":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, surrogate pair 𝄞 (237)" )]
        public void Test_name_selector__double_quotes__surrogate_pair____237()
        {
            var selector = "$[\"\\uD834\\uDD1E\"]";
            var document = JsonNode.Parse(
                """{"𝄞":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, surrogate pair 😀 (238)" )]
        public void Test_name_selector__double_quotes__surrogate_pair____238()
        {
            var selector = "$[\"\\uD83D\\uDE00\"]";
            var document = JsonNode.Parse(
                """{"😀":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, double quotes, invalid escaped single quote (239)" )]
        public void Test_name_selector__double_quotes__invalid_escaped_single_quote_239()
        {
            var selector = "$[\"\\'\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, embedded double quote (240)" )]
        public void Test_name_selector__double_quotes__embedded_double_quote_240()
        {
            var selector = "$[\"\"\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, incomplete escape (241)" )]
        public void Test_name_selector__double_quotes__incomplete_escape_241()
        {
            var selector = "$[\"\\\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes (242)" )]
        public void Test_name_selector__single_quotes_242()
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
        public void Test_name_selector__single_quotes__absent_data_243()
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
        public void Test_name_selector__single_quotes__array_data_244()
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
        public void Test_name_selector__single_quotes__embedded_U_0000_245()
        {
            var selector = "$['\u0000']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0001 (246)" )]
        public void Test_name_selector__single_quotes__embedded_U_0001_246()
        {
            var selector = "$['\u0001']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0002 (247)" )]
        public void Test_name_selector__single_quotes__embedded_U_0002_247()
        {
            var selector = "$['\u0002']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0003 (248)" )]
        public void Test_name_selector__single_quotes__embedded_U_0003_248()
        {
            var selector = "$['\u0003']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0004 (249)" )]
        public void Test_name_selector__single_quotes__embedded_U_0004_249()
        {
            var selector = "$['\u0004']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0005 (250)" )]
        public void Test_name_selector__single_quotes__embedded_U_0005_250()
        {
            var selector = "$['\u0005']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0006 (251)" )]
        public void Test_name_selector__single_quotes__embedded_U_0006_251()
        {
            var selector = "$['\u0006']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0007 (252)" )]
        public void Test_name_selector__single_quotes__embedded_U_0007_252()
        {
            var selector = "$['\u0007']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0008 (253)" )]
        public void Test_name_selector__single_quotes__embedded_U_0008_253()
        {
            var selector = "$['\b']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0009 (254)" )]
        public void Test_name_selector__single_quotes__embedded_U_0009_254()
        {
            var selector = "$['\t']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+000A (255)" )]
        public void Test_name_selector__single_quotes__embedded_U_000A_255()
        {
            var selector = "$['\n']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+000B (256)" )]
        public void Test_name_selector__single_quotes__embedded_U_000B_256()
        {
            var selector = "$['\u000b']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+000C (257)" )]
        public void Test_name_selector__single_quotes__embedded_U_000C_257()
        {
            var selector = "$['\f']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+000D (258)" )]
        public void Test_name_selector__single_quotes__embedded_U_000D_258()
        {
            var selector = "$['\r']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+000E (259)" )]
        public void Test_name_selector__single_quotes__embedded_U_000E_259()
        {
            var selector = "$['\u000e']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+000F (260)" )]
        public void Test_name_selector__single_quotes__embedded_U_000F_260()
        {
            var selector = "$['\u000f']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0010 (261)" )]
        public void Test_name_selector__single_quotes__embedded_U_0010_261()
        {
            var selector = "$['\u0010']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0011 (262)" )]
        public void Test_name_selector__single_quotes__embedded_U_0011_262()
        {
            var selector = "$['\u0011']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0012 (263)" )]
        public void Test_name_selector__single_quotes__embedded_U_0012_263()
        {
            var selector = "$['\u0012']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0013 (264)" )]
        public void Test_name_selector__single_quotes__embedded_U_0013_264()
        {
            var selector = "$['\u0013']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0014 (265)" )]
        public void Test_name_selector__single_quotes__embedded_U_0014_265()
        {
            var selector = "$['\u0014']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0015 (266)" )]
        public void Test_name_selector__single_quotes__embedded_U_0015_266()
        {
            var selector = "$['\u0015']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0016 (267)" )]
        public void Test_name_selector__single_quotes__embedded_U_0016_267()
        {
            var selector = "$['\u0016']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0017 (268)" )]
        public void Test_name_selector__single_quotes__embedded_U_0017_268()
        {
            var selector = "$['\u0017']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0018 (269)" )]
        public void Test_name_selector__single_quotes__embedded_U_0018_269()
        {
            var selector = "$['\u0018']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0019 (270)" )]
        public void Test_name_selector__single_quotes__embedded_U_0019_270()
        {
            var selector = "$['\u0019']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+001A (271)" )]
        public void Test_name_selector__single_quotes__embedded_U_001A_271()
        {
            var selector = "$['\u001a']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+001B (272)" )]
        public void Test_name_selector__single_quotes__embedded_U_001B_272()
        {
            var selector = "$['\u001b']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+001C (273)" )]
        public void Test_name_selector__single_quotes__embedded_U_001C_273()
        {
            var selector = "$['\u001c']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+001D (274)" )]
        public void Test_name_selector__single_quotes__embedded_U_001D_274()
        {
            var selector = "$['\u001d']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+001E (275)" )]
        public void Test_name_selector__single_quotes__embedded_U_001E_275()
        {
            var selector = "$['\u001e']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+001F (276)" )]
        public void Test_name_selector__single_quotes__embedded_U_001F_276()
        {
            var selector = "$['\u001f']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded U+0020 (277)" )]
        public void Test_name_selector__single_quotes__embedded_U_0020_277()
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
        public void Test_name_selector__single_quotes__escaped_single_quote_278()
        {
            var selector = """$['\'']""";
            var document = JsonNode.Parse(
                """{"'":"A"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["A"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, escaped reverse solidus (279)" )]
        public void Test_name_selector__single_quotes__escaped_reverse_solidus_279()
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
        public void Test_name_selector__single_quotes__escaped_solidus_280()
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
        public void Test_name_selector__single_quotes__escaped_backspace_281()
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
        public void Test_name_selector__single_quotes__escaped_form_feed_282()
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
        public void Test_name_selector__single_quotes__escaped_line_feed_283()
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
        public void Test_name_selector__single_quotes__escaped_carriage_return_284()
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
        public void Test_name_selector__single_quotes__escaped_tab_285()
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
        public void Test_name_selector__single_quotes__escaped____upper_case_hex_286()
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
        public void Test_name_selector__single_quotes__escaped____lower_case_hex_287()
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
        public void Test_name_selector__single_quotes__surrogate_pair____288()
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
        public void Test_name_selector__single_quotes__surrogate_pair____289()
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
        public void Test_name_selector__single_quotes__invalid_escaped_double_quote_290()
        {
            var selector = "$['\\\"']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, embedded single quote (291)" )]
        public void Test_name_selector__single_quotes__embedded_single_quote_291()
        {
            var selector = "$[''']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, single quotes, incomplete escape (292)" )]
        public void Test_name_selector__single_quotes__incomplete_escape_292()
        {
            var selector = "$['\\']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "name selector, double quotes, empty (293)" )]
        public void Test_name_selector__double_quotes__empty_293()
        {
            var selector = "$[\"\"]";
            var document = JsonNode.Parse(
                """{"a":"A","b":"B","":"C"}""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """["C"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "name selector, single quotes, empty (294)" )]
        public void Test_name_selector__single_quotes__empty_294()
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
        public void Test_slice_selector__slice_selector_295()
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
        public void Test_slice_selector__slice_selector_with_step_296()
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
        public void Test_slice_selector__slice_selector_with_everything_omitted__short_form_297()
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
        public void Test_slice_selector__slice_selector_with_everything_omitted__long_form_298()
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
        public void Test_slice_selector__slice_selector_with_start_omitted_299()
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
        public void Test_slice_selector__slice_selector_with_start_and_end_omitted_300()
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
        public void Test_slice_selector__negative_step_with_default_start_and_end_301()
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
        public void Test_slice_selector__negative_step_with_default_start_302()
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
        public void Test_slice_selector__negative_step_with_default_end_303()
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
        public void Test_slice_selector__larger_negative_step_304()
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
        public void Test_slice_selector__negative_range_with_default_step_305()
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
        public void Test_slice_selector__negative_range_with_negative_step_306()
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
        public void Test_slice_selector__negative_range_with_larger_negative_step_307()
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
        public void Test_slice_selector__larger_negative_range_with_larger_negative_step_308()
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
        public void Test_slice_selector__negative_from__positive_to_309()
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
        public void Test_slice_selector__negative_from_310()
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
        public void Test_slice_selector__positive_from__negative_to_311()
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
        public void Test_slice_selector__negative_from__positive_to__negative_step_312()
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
        public void Test_slice_selector__positive_from__negative_to__negative_step_313()
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
        public void Test_slice_selector__too_many_colons_314()
        {
            var selector = "$[1:2:3:4]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "slice selector, non-integer array index (315)" )]
        public void Test_slice_selector__non_integer_array_index_315()
        {
            var selector = "$[1:2:a]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "slice selector, zero step (316)" )]
        public void Test_slice_selector__zero_step_316()
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
        public void Test_slice_selector__empty_range_317()
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
        public void Test_slice_selector__slice_selector_with_everything_omitted_with_empty_array_318()
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
        public void Test_slice_selector__negative_step_with_empty_array_319()
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
        public void Test_slice_selector__maximal_range_with_positive_step_320()
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
        public void Test_slice_selector__maximal_range_with_negative_step_321()
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
        public void Test_slice_selector__excessively_large_to_value_322()
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
        public void Test_slice_selector__excessively_small_from_value_323()
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
        public void Test_slice_selector__excessively_large_from_value_with_negative_step_324()
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
        public void Test_slice_selector__excessively_small_to_value_with_negative_step_325()
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
        public void Test_slice_selector__excessively_large_step_326()
        {
            var selector = "$[1:10:113667776004]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector ).ToArray();
            var expect = JsonNode.Parse(
                """[1]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, excessively small step (327)" )]
        public void Test_slice_selector__excessively_small_step_327()
        {
            var selector = "$[-1:-10:-113667776004]";
            var document = JsonNode.Parse(
                """[0,1,2,3,4,5,6,7,8,9]""" );
            var results = document.Select( selector ).ToArray();
            var expect = JsonNode.Parse(
                """[9]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "slice selector, overflowing to value (328)" )]
        public void Test_slice_selector__overflowing_to_value_328()
        {
            var selector = "$[2:231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "slice selector, underflowing from value (329)" )]
        public void Test_slice_selector__underflowing_from_value_329()
        {
            var selector = "$[-231584178474632390847141970017375815706539969331281128078915168015826259279872:1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "slice selector, overflowing from value with negative step (330)" )]
        public void Test_slice_selector__overflowing_from_value_with_negative_step_330()
        {
            var selector = "$[231584178474632390847141970017375815706539969331281128078915168015826259279872:0:-1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "slice selector, underflowing to value with negative step (331)" )]
        public void Test_slice_selector__underflowing_to_value_with_negative_step_331()
        {
            var selector = "$[3:-231584178474632390847141970017375815706539969331281128078915168015826259279872:-1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "slice selector, overflowing step (332)" )]
        public void Test_slice_selector__overflowing_step_332()
        {
            var selector = "$[1:10:231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "slice selector, underflowing step (333)" )]
        public void Test_slice_selector__underflowing_step_333()
        {
            var selector = "$[-1:-10:-231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, count, count function (334)" )]
        public void Test_functions__count__count_function_334()
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
        public void Test_functions__count__single_node_arg_335()
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
        public void Test_functions__count__multiple_selector_arg_336()
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
        public void Test_functions__count__non_query_arg__number_337()
        {
            var selector = "$[?count(1)>2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, count, non-query arg, string (338)" )]
        public void Test_functions__count__non_query_arg__string_338()
        {
            var selector = "$[?count('string')>2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, count, non-query arg, true (339)" )]
        public void Test_functions__count__non_query_arg__true_339()
        {
            var selector = "$[?count(true)>2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, count, non-query arg, false (340)" )]
        public void Test_functions__count__non_query_arg__false_340()
        {
            var selector = "$[?count(false)>2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, count, non-query arg, null (341)" )]
        public void Test_functions__count__non_query_arg__null_341()
        {
            var selector = "$[?count(null)>2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, count, result must be compared (342)" )]
        public void Test_functions__count__result_must_be_compared_342()
        {
            var selector = "$[?count(@..*)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, count, no params (343)" )]
        public void Test_functions__count__no_params_343()
        {
            var selector = "$[?count()==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, count, too many params (344)" )]
        public void Test_functions__count__too_many_params_344()
        {
            var selector = "$[?count(@.a,@.b)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, length, string data (345)" )]
        public void Test_functions__length__string_data_345()
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
        public void Test_functions__length__string_data__unicode_346()
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
        public void Test_functions__length__array_data_347()
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
        public void Test_functions__length__missing_data_348()
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
        public void Test_functions__length__number_arg_349()
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
        public void Test_functions__length__true_arg_350()
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
        public void Test_functions__length__false_arg_351()
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
        public void Test_functions__length__null_arg_352()
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
        public void Test_functions__length__result_must_be_compared_353()
        {
            var selector = "$[?length(@.a)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, length, no params (354)" )]
        public void Test_functions__length__no_params_354()
        {
            var selector = "$[?length()==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, length, too many params (355)" )]
        public void Test_functions__length__too_many_params_355()
        {
            var selector = "$[?length(@.a,@.b)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, length, non-singular query arg (356)" )]
        public void Test_functions__length__non_singular_query_arg_356()
        {
            var selector = "$[?length(@.*)<3]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, length, arg is a function expression (357)" )]
        public void Test_functions__length__arg_is_a_function_expression_357()
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
        public void Test_functions__length__arg_is_special_nothing_358()
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
        public void Test_functions__match__found_match_359()
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
        public void Test_functions__match__double_quotes_360()
        {
            var selector = "$[?match(@.a, \"a.*\")]";
            var document = JsonNode.Parse(
                """[{"a":"ab"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"ab"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, match, regex from the document (361)" )]
        public void Test_functions__match__regex_from_the_document_361()
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
        public void Test_functions__match__don_t_select_match_362()
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
        public void Test_functions__match__not_a_match_363()
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
        public void Test_functions__match__select_non_match_364()
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
        public void Test_functions__match__non_string_first_arg_365()
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
        public void Test_functions__match__non_string_second_arg_366()
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
        public void Test_functions__match__filter__match_function__unicode_char_class__uppercase_367()
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
        public void Test_functions__match__filter__match_function__unicode_char_class_negated__uppercase_368()
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
        public void Test_functions__match__filter__match_function__unicode__surrogate_pair_369()
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
        public void Test_functions__match__dot_matcher_on__u2028_370()
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
        public void Test_functions__match__dot_matcher_on__u2029_371()
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
        public void Test_functions__match__result_cannot_be_compared_372()
        {
            var selector = "$[?match(@.a, 'a.*')==true]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, match, too few params (373)" )]
        public void Test_functions__match__too_few_params_373()
        {
            var selector = "$[?match(@.a)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, match, too many params (374)" )]
        public void Test_functions__match__too_many_params_374()
        {
            var selector = "$[?match(@.a,@.b,@.c)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, match, arg is a function expression (375)" )]
        public void Test_functions__match__arg_is_a_function_expression_375()
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
        public void Test_functions__match__dot_in_character_class_376()
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
        public void Test_functions__match__escaped_dot_377()
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
        public void Test_functions__match__escaped_backslash_before_dot_378()
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
        public void Test_functions__match__escaped_left_square_bracket_379()
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
        public void Test_functions__match__escaped_right_square_bracket_380()
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
        public void Test_functions__match__explicit_caret_381()
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
        public void Test_functions__match__explicit_dollar_382()
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
        public void Test_functions__search__at_the_end_383()
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
        public void Test_functions__search__double_quotes_384()
        {
            var selector = "$[?search(@.a, \"a.*\")]";
            var document = JsonNode.Parse(
                """[{"a":"the end is ab"}]""" );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """[{"a":"the end is ab"}]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "functions, search, at the start (385)" )]
        public void Test_functions__search__at_the_start_385()
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
        public void Test_functions__search__in_the_middle_386()
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
        public void Test_functions__search__regex_from_the_document_387()
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
        public void Test_functions__search__don_t_select_match_388()
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
        public void Test_functions__search__not_a_match_389()
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
        public void Test_functions__search__select_non_match_390()
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
        public void Test_functions__search__non_string_first_arg_391()
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
        public void Test_functions__search__non_string_second_arg_392()
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
        public void Test_functions__search__filter__search_function__unicode_char_class__uppercase_393()
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
        public void Test_functions__search__filter__search_function__unicode_char_class_negated__uppercase_394()
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
        public void Test_functions__search__filter__search_function__unicode__surrogate_pair_395()
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
        public void Test_functions__search__dot_matcher_on__u2028_396()
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
        public void Test_functions__search__dot_matcher_on__u2029_397()
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
        public void Test_functions__search__result_cannot_be_compared_398()
        {
            var selector = "$[?search(@.a, 'a.*')==true]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, search, too few params (399)" )]
        public void Test_functions__search__too_few_params_399()
        {
            var selector = "$[?search(@.a)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, search, too many params (400)" )]
        public void Test_functions__search__too_many_params_400()
        {
            var selector = "$[?search(@.a,@.b,@.c)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, search, arg is a function expression (401)" )]
        public void Test_functions__search__arg_is_a_function_expression_401()
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
        public void Test_functions__search__dot_in_character_class_402()
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
        public void Test_functions__search__escaped_dot_403()
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
        public void Test_functions__search__escaped_backslash_before_dot_404()
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
        public void Test_functions__search__escaped_left_square_bracket_405()
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
        public void Test_functions__search__escaped_right_square_bracket_406()
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
        public void Test_functions__value__single_value_nodelist_407()
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
        public void Test_functions__value__multi_value_nodelist_408()
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
        public void Test_functions__value__too_few_params_409()
        {
            var selector = "$[?value()==4]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, value, too many params (410)" )]
        public void Test_functions__value__too_many_params_410()
        {
            var selector = "$[?value(@.a,@.b)==4]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "functions, value, result must be compared (411)" )]
        public void Test_functions__value__result_must_be_compared_411()
        {
            var selector = "$[?value(@.a)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
            }
            catch ( NotSupportedException ) { return; }
            catch ( ArgumentException ) { return; }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

            Assert.Fail( "Failed to throw exception" );
        }


        [TestMethod( "whitespace, filter, space between question mark and expression (412)" )]
        public void Test_whitespace__filter__space_between_question_mark_and_expression_412()
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
        public void Test_whitespace__filter__newline_between_question_mark_and_expression_413()
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
        public void Test_whitespace__filter__tab_between_question_mark_and_expression_414()
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
        public void Test_whitespace__filter__return_between_question_mark_and_expression_415()
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
        public void Test_whitespace__filter__space_between_question_mark_and_parenthesized_expression_416()
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
        public void Test_whitespace__filter__newline_between_question_mark_and_parenthesized_expression_417()
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
        public void Test_whitespace__filter__tab_between_question_mark_and_parenthesized_expression_418()
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
        public void Test_whitespace__filter__return_between_question_mark_and_parenthesized_expression_419()
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
        public void Test_whitespace__filter__space_between_parenthesized_expression_and_bracket_420()
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
        public void Test_whitespace__filter__newline_between_parenthesized_expression_and_bracket_421()
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
        public void Test_whitespace__filter__tab_between_parenthesized_expression_and_bracket_422()
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
        public void Test_whitespace__filter__return_between_parenthesized_expression_and_bracket_423()
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
        public void Test_whitespace__filter__space_between_bracket_and_question_mark_424()
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
        public void Test_whitespace__filter__newline_between_bracket_and_question_mark_425()
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
        public void Test_whitespace__filter__tab_between_bracket_and_question_mark_426()
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
        public void Test_whitespace__filter__return_between_bracket_and_question_mark_427()
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
        public void Test_whitespace__functions__space_between_function_name_and_parenthesis_428()
        {
            var selector = "$[?count (@.*)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "whitespace, functions, newline between function name and parenthesis (429)" )]
        public void Test_whitespace__functions__newline_between_function_name_and_parenthesis_429()
        {
            var selector = "$[?count\n(@.*)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "whitespace, functions, tab between function name and parenthesis (430)" )]
        public void Test_whitespace__functions__tab_between_function_name_and_parenthesis_430()
        {
            var selector = "$[?count\t(@.*)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "whitespace, functions, return between function name and parenthesis (431)" )]
        public void Test_whitespace__functions__return_between_function_name_and_parenthesis_431()
        {
            var selector = "$[?count\r(@.*)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "whitespace, functions, space between parenthesis and arg (432)" )]
        public void Test_whitespace__functions__space_between_parenthesis_and_arg_432()
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
        public void Test_whitespace__functions__newline_between_parenthesis_and_arg_433()
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
        public void Test_whitespace__functions__tab_between_parenthesis_and_arg_434()
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
        public void Test_whitespace__functions__return_between_parenthesis_and_arg_435()
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
        public void Test_whitespace__functions__space_between_arg_and_comma_436()
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
        public void Test_whitespace__functions__newline_between_arg_and_comma_437()
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
        public void Test_whitespace__functions__tab_between_arg_and_comma_438()
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
        public void Test_whitespace__functions__return_between_arg_and_comma_439()
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
        public void Test_whitespace__functions__space_between_comma_and_arg_440()
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
        public void Test_whitespace__functions__newline_between_comma_and_arg_441()
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
        public void Test_whitespace__functions__tab_between_comma_and_arg_442()
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
        public void Test_whitespace__functions__return_between_comma_and_arg_443()
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
        public void Test_whitespace__functions__space_between_arg_and_parenthesis_444()
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
        public void Test_whitespace__functions__newline_between_arg_and_parenthesis_445()
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
        public void Test_whitespace__functions__tab_between_arg_and_parenthesis_446()
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
        public void Test_whitespace__functions__return_between_arg_and_parenthesis_447()
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
        public void Test_whitespace__functions__spaces_in_a_relative_singular_selector_448()
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
        public void Test_whitespace__functions__newlines_in_a_relative_singular_selector_449()
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
        public void Test_whitespace__functions__tabs_in_a_relative_singular_selector_450()
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
        public void Test_whitespace__functions__returns_in_a_relative_singular_selector_451()
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
        public void Test_whitespace__functions__spaces_in_an_absolute_singular_selector_452()
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
        public void Test_whitespace__functions__newlines_in_an_absolute_singular_selector_453()
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
        public void Test_whitespace__functions__tabs_in_an_absolute_singular_selector_454()
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
        public void Test_whitespace__functions__returns_in_an_absolute_singular_selector_455()
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
        public void Test_whitespace__operators__space_before____456()
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
        public void Test_whitespace__operators__newline_before____457()
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
        public void Test_whitespace__operators__tab_before____458()
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
        public void Test_whitespace__operators__return_before____459()
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
        public void Test_whitespace__operators__space_after____460()
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
        public void Test_whitespace__operators__newline_after____461()
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
        public void Test_whitespace__operators__tab_after____462()
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
        public void Test_whitespace__operators__return_after____463()
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
        public void Test_whitespace__operators__space_before____464()
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
        public void Test_whitespace__operators__newline_before____465()
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
        public void Test_whitespace__operators__tab_before____466()
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
        public void Test_whitespace__operators__return_before____467()
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
        public void Test_whitespace__operators__space_after____468()
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
        public void Test_whitespace__operators__newline_after____469()
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
        public void Test_whitespace__operators__tab_after____470()
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
        public void Test_whitespace__operators__return_after____471()
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
        public void Test_whitespace__operators__space_before____472()
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
        public void Test_whitespace__operators__newline_before____473()
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
        public void Test_whitespace__operators__tab_before____474()
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
        public void Test_whitespace__operators__return_before____475()
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
        public void Test_whitespace__operators__space_after____476()
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
        public void Test_whitespace__operators__newline_after____477()
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
        public void Test_whitespace__operators__tab_after____478()
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
        public void Test_whitespace__operators__return_after____479()
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
        public void Test_whitespace__operators__space_before____480()
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
        public void Test_whitespace__operators__newline_before____481()
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
        public void Test_whitespace__operators__tab_before____482()
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
        public void Test_whitespace__operators__return_before____483()
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
        public void Test_whitespace__operators__space_after____484()
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
        public void Test_whitespace__operators__newline_after____485()
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
        public void Test_whitespace__operators__tab_after____486()
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
        public void Test_whitespace__operators__return_after____487()
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
        public void Test_whitespace__operators__space_before___488()
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
        public void Test_whitespace__operators__newline_before___489()
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
        public void Test_whitespace__operators__tab_before___490()
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
        public void Test_whitespace__operators__return_before___491()
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
        public void Test_whitespace__operators__space_after___492()
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
        public void Test_whitespace__operators__newline_after___493()
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
        public void Test_whitespace__operators__tab_after___494()
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
        public void Test_whitespace__operators__return_after___495()
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
        public void Test_whitespace__operators__space_before___496()
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
        public void Test_whitespace__operators__newline_before___497()
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
        public void Test_whitespace__operators__tab_before___498()
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
        public void Test_whitespace__operators__return_before___499()
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
        public void Test_whitespace__operators__space_after___500()
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
        public void Test_whitespace__operators__newline_after___501()
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
        public void Test_whitespace__operators__tab_after___502()
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
        public void Test_whitespace__operators__return_after___503()
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
        public void Test_whitespace__operators__space_before____504()
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
        public void Test_whitespace__operators__newline_before____505()
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
        public void Test_whitespace__operators__tab_before____506()
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
        public void Test_whitespace__operators__return_before____507()
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
        public void Test_whitespace__operators__space_after____508()
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
        public void Test_whitespace__operators__newline_after____509()
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
        public void Test_whitespace__operators__tab_after____510()
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
        public void Test_whitespace__operators__return_after____511()
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
        public void Test_whitespace__operators__space_before____512()
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
        public void Test_whitespace__operators__newline_before____513()
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
        public void Test_whitespace__operators__tab_before____514()
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
        public void Test_whitespace__operators__return_before____515()
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
        public void Test_whitespace__operators__space_after____516()
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
        public void Test_whitespace__operators__newline_after____517()
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
        public void Test_whitespace__operators__tab_after____518()
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
        public void Test_whitespace__operators__return_after____519()
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
        public void Test_whitespace__operators__space_between_logical_not_and_test_expression_520()
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
        public void Test_whitespace__operators__newline_between_logical_not_and_test_expression_521()
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
        public void Test_whitespace__operators__tab_between_logical_not_and_test_expression_522()
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
        public void Test_whitespace__operators__return_between_logical_not_and_test_expression_523()
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
        public void Test_whitespace__operators__space_between_logical_not_and_parenthesized_expression_524()
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
        public void Test_whitespace__operators__newline_between_logical_not_and_parenthesized_expression_525()
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
        public void Test_whitespace__operators__tab_between_logical_not_and_parenthesized_expression_526()
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
        public void Test_whitespace__operators__return_between_logical_not_and_parenthesized_expression_527()
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
        public void Test_whitespace__selectors__space_between_root_and_bracket_528()
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
        public void Test_whitespace__selectors__newline_between_root_and_bracket_529()
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
        public void Test_whitespace__selectors__tab_between_root_and_bracket_530()
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
        public void Test_whitespace__selectors__return_between_root_and_bracket_531()
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
        public void Test_whitespace__selectors__space_between_bracket_and_bracket_532()
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
        public void Test_whitespace__selectors__newline_between_root_and_bracket_533()
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
        public void Test_whitespace__selectors__tab_between_root_and_bracket_534()
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
        public void Test_whitespace__selectors__return_between_root_and_bracket_535()
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
        public void Test_whitespace__selectors__space_between_root_and_dot_536()
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
        public void Test_whitespace__selectors__newline_between_root_and_dot_537()
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
        public void Test_whitespace__selectors__tab_between_root_and_dot_538()
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
        public void Test_whitespace__selectors__return_between_root_and_dot_539()
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
        public void Test_whitespace__selectors__space_between_dot_and_name_540()
        {
            var selector = "$. a";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "whitespace, selectors, newline between dot and name (541)" )]
        public void Test_whitespace__selectors__newline_between_dot_and_name_541()
        {
            var selector = "$.\na";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "whitespace, selectors, tab between dot and name (542)" )]
        public void Test_whitespace__selectors__tab_between_dot_and_name_542()
        {
            var selector = "$.\ta";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "whitespace, selectors, return between dot and name (543)" )]
        public void Test_whitespace__selectors__return_between_dot_and_name_543()
        {
            var selector = "$.\ra";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "whitespace, selectors, space between recursive descent and name (544)" )]
        public void Test_whitespace__selectors__space_between_recursive_descent_and_name_544()
        {
            var selector = "$.. a";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "whitespace, selectors, newline between recursive descent and name (545)" )]
        public void Test_whitespace__selectors__newline_between_recursive_descent_and_name_545()
        {
            var selector = "$..\na";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "whitespace, selectors, tab between recursive descent and name (546)" )]
        public void Test_whitespace__selectors__tab_between_recursive_descent_and_name_546()
        {
            var selector = "$..\ta";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "whitespace, selectors, return between recursive descent and name (547)" )]
        public void Test_whitespace__selectors__return_between_recursive_descent_and_name_547()
        {
            var selector = "$..\ra";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            try
            {
                document.Select( selector ).ToArray();
                Assert.Fail( "Failed to throw exception" );
            }
            catch ( NotSupportedException ) { }
            catch ( ArgumentException ) { }
            catch ( Exception e )
            {
                Assert.Fail( $"Invalid exception of type {e.GetType().Name}" );
            }

        }


        [TestMethod( "whitespace, selectors, space between bracket and selector (548)" )]
        public void Test_whitespace__selectors__space_between_bracket_and_selector_548()
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
        public void Test_whitespace__selectors__newline_between_bracket_and_selector_549()
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
        public void Test_whitespace__selectors__tab_between_bracket_and_selector_550()
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
        public void Test_whitespace__selectors__return_between_bracket_and_selector_551()
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
        public void Test_whitespace__selectors__space_between_selector_and_bracket_552()
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
        public void Test_whitespace__selectors__newline_between_selector_and_bracket_553()
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
        public void Test_whitespace__selectors__tab_between_selector_and_bracket_554()
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
        public void Test_whitespace__selectors__return_between_selector_and_bracket_555()
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
        public void Test_whitespace__selectors__space_between_selector_and_comma_556()
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
        public void Test_whitespace__selectors__newline_between_selector_and_comma_557()
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
        public void Test_whitespace__selectors__tab_between_selector_and_comma_558()
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
        public void Test_whitespace__selectors__return_between_selector_and_comma_559()
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
        public void Test_whitespace__selectors__space_between_comma_and_selector_560()
        {
            var selector = "$['a', 'b']";
            var document = JsonNode.Parse(
                """{"a":"ab","b":"bc"}""" );
            var results = document.Select( selector ).ToArray();
            var expect = JsonNode.Parse(
                """["ab","bc"]""" );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }


        [TestMethod( "whitespace, selectors, newline between comma and selector (561)" )]
        public void Test_whitespace__selectors__newline_between_comma_and_selector_561()
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
        public void Test_whitespace__selectors__tab_between_comma_and_selector_562()
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
        public void Test_whitespace__selectors__return_between_comma_and_selector_563()
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
        public void Test_whitespace__slice__space_between_start_and_colon_564()
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
        public void Test_whitespace__slice__newline_between_start_and_colon_565()
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
        public void Test_whitespace__slice__tab_between_start_and_colon_566()
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
        public void Test_whitespace__slice__return_between_start_and_colon_567()
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
        public void Test_whitespace__slice__space_between_colon_and_end_568()
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
        public void Test_whitespace__slice__newline_between_colon_and_end_569()
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
        public void Test_whitespace__slice__tab_between_colon_and_end_570()
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
        public void Test_whitespace__slice__return_between_colon_and_end_571()
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
        public void Test_whitespace__slice__space_between_end_and_colon_572()
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
        public void Test_whitespace__slice__newline_between_end_and_colon_573()
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
        public void Test_whitespace__slice__tab_between_end_and_colon_574()
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
        public void Test_whitespace__slice__return_between_end_and_colon_575()
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
        public void Test_whitespace__slice__space_between_colon_and_step_576()
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
        public void Test_whitespace__slice__newline_between_colon_and_step_577()
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
        public void Test_whitespace__slice__tab_between_colon_and_step_578()
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
        public void Test_whitespace__slice__return_between_colon_and_step_579()
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

