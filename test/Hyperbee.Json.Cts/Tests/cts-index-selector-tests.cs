// This file was auto generated.

using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Cts.TestSupport;

namespace Hyperbee.Json.Cts.Tests;

[TestClass]
public class CtsIndexSelectorTest
{
    [DataTestMethod( @"first element (1)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_first_element_1( Type documentType )
    {
        const string selector = "$[0]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "first",
                  "second"
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "first"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"second element (2)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_second_element_2( Type documentType )
    {
        const string selector = "$[1]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "first",
                  "second"
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "second"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"out of bound (3)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_out_of_bound_3( Type documentType )
    {
        const string selector = "$[2]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "first",
                  "second"
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                []
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"overflowing index (4)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_overflowing_index_4( Type documentType )
    {
        const string selector = "$[231584178474632390847141970017375815706539969331281128078915168015826259279872]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"not actually an index, overflowing index leads into general text (5)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_not_actually_an_index__overflowing_index_leads_into_general_text_5( Type documentType )
    {
        const string selector = "$[231584178474632390847141970017375815706539969331281128078915168SomeRandomText]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"negative (6)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_negative_6( Type documentType )
    {
        const string selector = "$[-1]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "first",
                  "second"
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "second"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"more negative (7)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_more_negative_7( Type documentType )
    {
        const string selector = "$[-2]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "first",
                  "second"
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "first"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"negative out of bound (8)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_negative_out_of_bound_8( Type documentType )
    {
        const string selector = "$[-3]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "first",
                  "second"
                ]
            """ );
        var results = document.Select( selector ).ToArray();
        var expect = TestHelper.Parse( documentType,
            """
                []
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"on object (9)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_on_object_9( Type documentType )
    {
        const string selector = "$[0]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "foo": 1
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                []
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"leading 0 (10)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_leading_0_10( Type documentType )
    {
        const string selector = "$[01]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"leading -0 (11)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_leading__0_11( Type documentType )
    {
        const string selector = "$[-01]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
}


