// This file was auto generated.

using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Cts.Tests
{
    [TestClass]
    public class CtsSliceSelectorTest
    {

        [TestMethod( @"slice selector (1)" )]
        public void Test_slice_selector_1()
        {
            var selector = "$[1:3]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  1,
                  2
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice selector with step (2)" )]
        public void Test_slice_selector_with_step_2()
        {
            var selector = "$[1:6:2]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  1,
                  3,
                  5
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice selector with everything omitted, short form (3)" )]
        public void Test_slice_selector_with_everything_omitted__short_form_3()
        {
            var selector = "$[:]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice selector with everything omitted, long form (4)" )]
        public void Test_slice_selector_with_everything_omitted__long_form_4()
        {
            var selector = "$[::]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice selector with start omitted (5)" )]
        public void Test_slice_selector_with_start_omitted_5()
        {
            var selector = "$[:2]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  0,
                  1
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice selector with start and end omitted (6)" )]
        public void Test_slice_selector_with_start_and_end_omitted_6()
        {
            var selector = "$[::2]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  0,
                  2,
                  4,
                  6,
                  8
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"negative step with default start and end (7)" )]
        public void Test_negative_step_with_default_start_and_end_7()
        {
            var selector = "$[::-1]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  3,
                  2,
                  1,
                  0
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"negative step with default start (8)" )]
        public void Test_negative_step_with_default_start_8()
        {
            var selector = "$[:0:-1]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  3,
                  2,
                  1
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"negative step with default end (9)" )]
        public void Test_negative_step_with_default_end_9()
        {
            var selector = "$[2::-1]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  1,
                  0
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"larger negative step (10)" )]
        public void Test_larger_negative_step_10()
        {
            var selector = "$[::-2]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  3,
                  1
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"negative range with default step (11)" )]
        public void Test_negative_range_with_default_step_11()
        {
            var selector = "$[-1:-3]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                []
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"negative range with negative step (12)" )]
        public void Test_negative_range_with_negative_step_12()
        {
            var selector = "$[-1:-3:-1]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  9,
                  8
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"negative range with larger negative step (13)" )]
        public void Test_negative_range_with_larger_negative_step_13()
        {
            var selector = "$[-1:-6:-2]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  9,
                  7,
                  5
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"larger negative range with larger negative step (14)" )]
        public void Test_larger_negative_range_with_larger_negative_step_14()
        {
            var selector = "$[-1:-7:-2]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  9,
                  7,
                  5
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"negative from, positive to (15)" )]
        public void Test_negative_from__positive_to_15()
        {
            var selector = "$[-5:7]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  5,
                  6
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"negative from (16)" )]
        public void Test_negative_from_16()
        {
            var selector = "$[-2:]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  8,
                  9
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"positive from, negative to (17)" )]
        public void Test_positive_from__negative_to_17()
        {
            var selector = "$[1:-1]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"negative from, positive to, negative step (18)" )]
        public void Test_negative_from__positive_to__negative_step_18()
        {
            var selector = "$[-1:1:-1]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  9,
                  8,
                  7,
                  6,
                  5,
                  4,
                  3,
                  2
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"positive from, negative to, negative step (19)" )]
        public void Test_positive_from__negative_to__negative_step_19()
        {
            var selector = "$[7:-5:-1]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  7,
                  6
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"too many colons (20)" )]
        public void Test_too_many_colons_20()
        {
            var selector = "$[1:2:3:4]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"non-integer array index (21)" )]
        public void Test_non_integer_array_index_21()
        {
            var selector = "$[1:2:a]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"zero step (22)" )]
        public void Test_zero_step_22()
        {
            var selector = "$[1:2:0]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                []
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"empty range (23)" )]
        public void Test_empty_range_23()
        {
            var selector = "$[2:2]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                []
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice selector with everything omitted with empty array (24)" )]
        public void Test_slice_selector_with_everything_omitted_with_empty_array_24()
        {
            var selector = "$[:]";
            var document = JsonNode.Parse(
                """
                []
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                []
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"negative step with empty array (25)" )]
        public void Test_negative_step_with_empty_array_25()
        {
            var selector = "$[::-1]";
            var document = JsonNode.Parse(
                """
                []
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                []
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"maximal range with positive step (26)" )]
        public void Test_maximal_range_with_positive_step_26()
        {
            var selector = "$[0:10]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"maximal range with negative step (27)" )]
        public void Test_maximal_range_with_negative_step_27()
        {
            var selector = "$[9:0:-1]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  9,
                  8,
                  7,
                  6,
                  5,
                  4,
                  3,
                  2,
                  1
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"excessively large to value (28)" )]
        public void Test_excessively_large_to_value_28()
        {
            var selector = "$[2:113667776004]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"excessively small from value (29)" )]
        public void Test_excessively_small_from_value_29()
        {
            var selector = "$[-113667776004:1]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  0
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"excessively large from value with negative step (30)" )]
        public void Test_excessively_large_from_value_with_negative_step_30()
        {
            var selector = "$[113667776004:0:-1]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  9,
                  8,
                  7,
                  6,
                  5,
                  4,
                  3,
                  2,
                  1
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"excessively small to value with negative step (31)" )]
        public void Test_excessively_small_to_value_with_negative_step_31()
        {
            var selector = "$[3:-113667776004:-1]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  3,
                  2,
                  1,
                  0
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"excessively large step (32)" )]
        public void Test_excessively_large_step_32()
        {
            var selector = "$[1:10:113667776004]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  1
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"excessively small step (33)" )]
        public void Test_excessively_small_step_33()
        {
            var selector = "$[-1:-10:-113667776004]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5,
                  6,
                  7,
                  8,
                  9
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  9
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"overflowing to value (34)" )]
        public void Test_overflowing_to_value_34()
        {
            var selector = "$[2:231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"underflowing from value (35)" )]
        public void Test_underflowing_from_value_35()
        {
            var selector = "$[-231584178474632390847141970017375815706539969331281128078915168015826259279872:1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"overflowing from value with negative step (36)" )]
        public void Test_overflowing_from_value_with_negative_step_36()
        {
            var selector = "$[231584178474632390847141970017375815706539969331281128078915168015826259279872:0:-1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"underflowing to value with negative step (37)" )]
        public void Test_underflowing_to_value_with_negative_step_37()
        {
            var selector = "$[3:-231584178474632390847141970017375815706539969331281128078915168015826259279872:-1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"overflowing step (38)" )]
        public void Test_overflowing_step_38()
        {
            var selector = "$[1:10:231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"underflowing step (39)" )]
        public void Test_underflowing_step_39()
        {
            var selector = "$[-1:-10:-231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
    }
}

