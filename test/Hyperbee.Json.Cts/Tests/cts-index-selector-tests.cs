// This file was auto generated.

using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Cts.Tests
{
    [TestClass]
    public class CtsIndexSelectorTest
    {

        [TestMethod( @"first element (1)" )]
        public void Test_first_element_1()
        {
            var selector = "$[0]";
            var document = JsonNode.Parse(
                """
                [
                  "first",
                  "second"
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "first"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"second element (2)" )]
        public void Test_second_element_2()
        {
            var selector = "$[1]";
            var document = JsonNode.Parse(
                """
                [
                  "first",
                  "second"
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "second"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"out of bound (3)" )]
        public void Test_out_of_bound_3()
        {
            var selector = "$[2]";
            var document = JsonNode.Parse(
                """
                [
                  "first",
                  "second"
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

        [TestMethod( @"overflowing index (4)" )]
        public void Test_overflowing_index_4()
        {
            var selector = "$[231584178474632390847141970017375815706539969331281128078915168015826259279872]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"not actually an index, overflowing index leads into general text (5)" )]
        public void Test_not_actually_an_index__overflowing_index_leads_into_general_text_5()
        {
            var selector = "$[231584178474632390847141970017375815706539969331281128078915168SomeRandomText]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"negative (6)" )]
        public void Test_negative_6()
        {
            var selector = "$[-1]";
            var document = JsonNode.Parse(
                """
                [
                  "first",
                  "second"
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "second"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"more negative (7)" )]
        public void Test_more_negative_7()
        {
            var selector = "$[-2]";
            var document = JsonNode.Parse(
                """
                [
                  "first",
                  "second"
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "first"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"negative out of bound (8)" )]
        public void Test_negative_out_of_bound_8()
        {
            var selector = "$[-3]";
            var document = JsonNode.Parse(
                """
                [
                  "first",
                  "second"
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

        [TestMethod( @"on object (9)" )]
        public void Test_on_object_9()
        {
            var selector = "$[0]";
            var document = JsonNode.Parse(
                """
                {
                  "foo": 1
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                []
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"leading 0 (10)" )]
        public void Test_leading_0_10()
        {
            var selector = "$[01]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"leading -0 (11)" )]
        public void Test_leading__0_11()
        {
            var selector = "$[-01]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
    }
}

