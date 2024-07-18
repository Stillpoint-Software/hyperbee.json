// This file was auto generated.

using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Cts.TestSupport;

namespace Hyperbee.Json.Cts.Tests;

[TestClass]
public class CtsSliceSelectorTest
{        
    [DataTestMethod( @"slice selector (1)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_slice_selector_1( Type documentType )
    {
        const string selector = "$[1:3]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  1,
                  2
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"slice selector with step (2)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_slice_selector_with_step_2( Type documentType )
    {
        const string selector = "$[1:6:2]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  1,
                  3,
                  5
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"slice selector with everything omitted, short form (3)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_slice_selector_with_everything_omitted__short_form_3( Type documentType )
    {
        const string selector = "$[:]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  0,
                  1,
                  2,
                  3
                ]
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  0,
                  1,
                  2,
                  3
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"slice selector with everything omitted, long form (4)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_slice_selector_with_everything_omitted__long_form_4( Type documentType )
    {
        const string selector = "$[::]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  0,
                  1,
                  2,
                  3
                ]
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  0,
                  1,
                  2,
                  3
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"slice selector with start omitted (5)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_slice_selector_with_start_omitted_5( Type documentType )
    {
        const string selector = "$[:2]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  0,
                  1
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"slice selector with start and end omitted (6)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_slice_selector_with_start_and_end_omitted_6( Type documentType )
    {
        const string selector = "$[::2]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  0,
                  2,
                  4,
                  6,
                  8
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"negative step with default start and end (7)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_negative_step_with_default_start_and_end_7( Type documentType )
    {
        const string selector = "$[::-1]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  0,
                  1,
                  2,
                  3
                ]
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  3,
                  2,
                  1,
                  0
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"negative step with default start (8)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_negative_step_with_default_start_8( Type documentType )
    {
        const string selector = "$[:0:-1]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  0,
                  1,
                  2,
                  3
                ]
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  3,
                  2,
                  1
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"negative step with default end (9)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_negative_step_with_default_end_9( Type documentType )
    {
        const string selector = "$[2::-1]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  0,
                  1,
                  2,
                  3
                ]
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  1,
                  0
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"larger negative step (10)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_larger_negative_step_10( Type documentType )
    {
        const string selector = "$[::-2]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  0,
                  1,
                  2,
                  3
                ]
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  3,
                  1
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"negative range with default step (11)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_negative_range_with_default_step_11( Type documentType )
    {
        const string selector = "$[-1:-3]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                []
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"negative range with negative step (12)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_negative_range_with_negative_step_12( Type documentType )
    {
        const string selector = "$[-1:-3:-1]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  9,
                  8
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"negative range with larger negative step (13)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_negative_range_with_larger_negative_step_13( Type documentType )
    {
        const string selector = "$[-1:-6:-2]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  9,
                  7,
                  5
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"larger negative range with larger negative step (14)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_larger_negative_range_with_larger_negative_step_14( Type documentType )
    {
        const string selector = "$[-1:-7:-2]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  9,
                  7,
                  5
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"negative from, positive to (15)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_negative_from__positive_to_15( Type documentType )
    {
        const string selector = "$[-5:7]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  5,
                  6
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"negative from (16)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_negative_from_16( Type documentType )
    {
        const string selector = "$[-2:]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  8,
                  9
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"positive from, negative to (17)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_positive_from__negative_to_17( Type documentType )
    {
        const string selector = "$[1:-1]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
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
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"negative from, positive to, negative step (18)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_negative_from__positive_to__negative_step_18( Type documentType )
    {
        const string selector = "$[-1:1:-1]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
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
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"positive from, negative to, negative step (19)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_positive_from__negative_to__negative_step_19( Type documentType )
    {
        const string selector = "$[7:-5:-1]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  7,
                  6
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"too many colons (20)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_too_many_colons_20( Type documentType )
    {
        const string selector = "$[1:2:3:4]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"non-integer array index (21)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_non_integer_array_index_21( Type documentType )
    {
        const string selector = "$[1:2:a]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"zero step (22)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_zero_step_22( Type documentType )
    {
        const string selector = "$[1:2:0]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                []
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"empty range (23)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_empty_range_23( Type documentType )
    {
        const string selector = "$[2:2]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                []
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"slice selector with everything omitted with empty array (24)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_slice_selector_with_everything_omitted_with_empty_array_24( Type documentType )
    {
        const string selector = "$[:]";
        var document = TestHelper.Parse( documentType,
            """
                []
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                []
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"negative step with empty array (25)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_negative_step_with_empty_array_25( Type documentType )
    {
        const string selector = "$[::-1]";
        var document = TestHelper.Parse( documentType,
            """
                []
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                []
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"maximal range with positive step (26)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_maximal_range_with_positive_step_26( Type documentType )
    {
        const string selector = "$[0:10]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
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
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"maximal range with negative step (27)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_maximal_range_with_negative_step_27( Type documentType )
    {
        const string selector = "$[9:0:-1]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
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
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"excessively large to value (28)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_excessively_large_to_value_28( Type documentType )
    {
        const string selector = "$[2:113667776004]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
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
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"excessively small from value (29)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_excessively_small_from_value_29( Type documentType )
    {
        const string selector = "$[-113667776004:1]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  0
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"excessively large from value with negative step (30)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_excessively_large_from_value_with_negative_step_30( Type documentType )
    {
        const string selector = "$[113667776004:0:-1]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
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
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"excessively small to value with negative step (31)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_excessively_small_to_value_with_negative_step_31( Type documentType )
    {
        const string selector = "$[3:-113667776004:-1]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  3,
                  2,
                  1,
                  0
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"excessively large step (32)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_excessively_large_step_32( Type documentType )
    {
        const string selector = "$[1:10:113667776004]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  1
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"excessively small step (33)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_excessively_small_step_33( Type documentType )
    {
        const string selector = "$[-1:-10:-113667776004]";
        var document = TestHelper.Parse( documentType,
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
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  9
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"overflowing to value (34)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_overflowing_to_value_34( Type documentType )
    {
        const string selector = "$[2:231584178474632390847141970017375815706539969331281128078915168015826259279872]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"underflowing from value (35)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_underflowing_from_value_35( Type documentType )
    {
        const string selector = "$[-231584178474632390847141970017375815706539969331281128078915168015826259279872:1]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"overflowing from value with negative step (36)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_overflowing_from_value_with_negative_step_36( Type documentType )
    {
        const string selector = "$[231584178474632390847141970017375815706539969331281128078915168015826259279872:0:-1]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"underflowing to value with negative step (37)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_underflowing_to_value_with_negative_step_37( Type documentType )
    {
        const string selector = "$[3:-231584178474632390847141970017375815706539969331281128078915168015826259279872:-1]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"overflowing step (38)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_overflowing_step_38( Type documentType )
    {
        const string selector = "$[1:10:231584178474632390847141970017375815706539969331281128078915168015826259279872]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"underflowing step (39)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_underflowing_step_39( Type documentType )
    {
        const string selector = "$[-1:-10:-231584178474632390847141970017375815706539969331281128078915168015826259279872]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
}


