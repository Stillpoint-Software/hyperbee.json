// This file was auto generated.

using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Cts.TestSupport;

namespace Hyperbee.Json.Cts.Tests;

[TestClass]
public class CtsNameSelectorTest
{        
    [DataTestMethod( @"double quotes (1)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes_1( Type documentType )
    {
        const string selector = "$[\"a\"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "A",
                  "b": "B"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"double quotes, absent data (2)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__absent_data_2( Type documentType )
    {
        const string selector = "$[\"c\"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "A",
                  "b": "B"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                []
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"double quotes, array data (3)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__array_data_3( Type documentType )
    {
        const string selector = "$[\"a\"]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "first",
                  "second"
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
        
    [DataTestMethod( @"double quotes, embedded U+0000 (4)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0000_4( Type documentType )
    {
        const string selector = "$[\"\u0000\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0001 (5)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0001_5( Type documentType )
    {
        const string selector = "$[\"\u0001\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0002 (6)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0002_6( Type documentType )
    {
        const string selector = "$[\"\u0002\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0003 (7)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0003_7( Type documentType )
    {
        const string selector = "$[\"\u0003\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0004 (8)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0004_8( Type documentType )
    {
        const string selector = "$[\"\u0004\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0005 (9)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0005_9( Type documentType )
    {
        const string selector = "$[\"\u0005\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0006 (10)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0006_10( Type documentType )
    {
        const string selector = "$[\"\u0006\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0007 (11)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0007_11( Type documentType )
    {
        const string selector = "$[\"\u0007\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0008 (12)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0008_12( Type documentType )
    {
        const string selector = "$[\"\b\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0009 (13)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0009_13( Type documentType )
    {
        const string selector = "$[\"\t\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+000A (14)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_000A_14( Type documentType )
    {
        const string selector = "$[\"\n\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+000B (15)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_000B_15( Type documentType )
    {
        const string selector = "$[\"\u000b\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+000C (16)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_000C_16( Type documentType )
    {
        const string selector = "$[\"\f\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+000D (17)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_000D_17( Type documentType )
    {
        const string selector = "$[\"\r\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+000E (18)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_000E_18( Type documentType )
    {
        const string selector = "$[\"\u000e\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+000F (19)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_000F_19( Type documentType )
    {
        const string selector = "$[\"\u000f\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0010 (20)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0010_20( Type documentType )
    {
        const string selector = "$[\"\u0010\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0011 (21)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0011_21( Type documentType )
    {
        const string selector = "$[\"\u0011\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0012 (22)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0012_22( Type documentType )
    {
        const string selector = "$[\"\u0012\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0013 (23)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0013_23( Type documentType )
    {
        const string selector = "$[\"\u0013\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0014 (24)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0014_24( Type documentType )
    {
        const string selector = "$[\"\u0014\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0015 (25)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0015_25( Type documentType )
    {
        const string selector = "$[\"\u0015\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0016 (26)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0016_26( Type documentType )
    {
        const string selector = "$[\"\u0016\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0017 (27)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0017_27( Type documentType )
    {
        const string selector = "$[\"\u0017\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0018 (28)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0018_28( Type documentType )
    {
        const string selector = "$[\"\u0018\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0019 (29)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0019_29( Type documentType )
    {
        const string selector = "$[\"\u0019\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+001A (30)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_001A_30( Type documentType )
    {
        const string selector = "$[\"\u001a\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+001B (31)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_001B_31( Type documentType )
    {
        const string selector = "$[\"\u001b\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+001C (32)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_001C_32( Type documentType )
    {
        const string selector = "$[\"\u001c\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+001D (33)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_001D_33( Type documentType )
    {
        const string selector = "$[\"\u001d\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+001E (34)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_001E_34( Type documentType )
    {
        const string selector = "$[\"\u001e\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+001F (35)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_001F_35( Type documentType )
    {
        const string selector = "$[\"\u001f\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded U+0020 (36)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_U_0020_36( Type documentType )
    {
        const string selector = "$[\" \"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  " ": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"double quotes, escaped double quote (37)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__escaped_double_quote_37( Type documentType )
    {
        const string selector = "$[\"\\\"\"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "\"": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"double quotes, escaped reverse solidus (38)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__escaped_reverse_solidus_38( Type documentType )
    {
        const string selector = "$[\"\\\\\"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "\\": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"double quotes, escaped solidus (39)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__escaped_solidus_39( Type documentType )
    {
        const string selector = "$[\"\\/\"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "/": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"double quotes, escaped backspace (40)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__escaped_backspace_40( Type documentType )
    {
        const string selector = "$[\"\\b\"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "\b": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"double quotes, escaped form feed (41)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__escaped_form_feed_41( Type documentType )
    {
        const string selector = "$[\"\\f\"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "\f": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"double quotes, escaped line feed (42)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__escaped_line_feed_42( Type documentType )
    {
        const string selector = "$[\"\\n\"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "\n": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"double quotes, escaped carriage return (43)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__escaped_carriage_return_43( Type documentType )
    {
        const string selector = "$[\"\\r\"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "\r": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"double quotes, escaped tab (44)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__escaped_tab_44( Type documentType )
    {
        const string selector = "$[\"\\t\"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "\t": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"double quotes, escaped ☺, upper case hex (45)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__escaped____upper_case_hex_45( Type documentType )
    {
        const string selector = "$[\"\\u263A\"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "☺": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"double quotes, escaped ☺, lower case hex (46)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__escaped____lower_case_hex_46( Type documentType )
    {
        const string selector = "$[\"\\u263a\"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "☺": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"double quotes, surrogate pair 𝄞 (47)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__surrogate_pair____47( Type documentType )
    {
        const string selector = "$[\"\\uD834\\uDD1E\"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "𝄞": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"double quotes, surrogate pair 😀 (48)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__surrogate_pair____48( Type documentType )
    {
        const string selector = "$[\"\\uD83D\\uDE00\"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "😀": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"double quotes, invalid escaped single quote (49)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__invalid_escaped_single_quote_49( Type documentType )
    {
        const string selector = "$[\"\\'\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, embedded double quote (50)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__embedded_double_quote_50( Type documentType )
    {
        const string selector = "$[\"\"\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, incomplete escape (51)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__incomplete_escape_51( Type documentType )
    {
        const string selector = "$[\"\\\"]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes (52)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes_52( Type documentType )
    {
        const string selector = "$['a']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "A",
                  "b": "B"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, absent data (53)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__absent_data_53( Type documentType )
    {
        const string selector = "$['c']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "A",
                  "b": "B"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                []
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, array data (54)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__array_data_54( Type documentType )
    {
        const string selector = "$['a']";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "first",
                  "second"
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
        
    [DataTestMethod( @"single quotes, embedded U+0000 (55)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0000_55( Type documentType )
    {
        const string selector = "$['\u0000']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0001 (56)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0001_56( Type documentType )
    {
        const string selector = "$['\u0001']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0002 (57)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0002_57( Type documentType )
    {
        const string selector = "$['\u0002']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0003 (58)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0003_58( Type documentType )
    {
        const string selector = "$['\u0003']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0004 (59)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0004_59( Type documentType )
    {
        const string selector = "$['\u0004']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0005 (60)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0005_60( Type documentType )
    {
        const string selector = "$['\u0005']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0006 (61)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0006_61( Type documentType )
    {
        const string selector = "$['\u0006']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0007 (62)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0007_62( Type documentType )
    {
        const string selector = "$['\u0007']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0008 (63)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0008_63( Type documentType )
    {
        const string selector = "$['\b']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0009 (64)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0009_64( Type documentType )
    {
        const string selector = "$['\t']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+000A (65)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_000A_65( Type documentType )
    {
        const string selector = "$['\n']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+000B (66)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_000B_66( Type documentType )
    {
        const string selector = "$['\u000b']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+000C (67)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_000C_67( Type documentType )
    {
        const string selector = "$['\f']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+000D (68)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_000D_68( Type documentType )
    {
        const string selector = "$['\r']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+000E (69)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_000E_69( Type documentType )
    {
        const string selector = "$['\u000e']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+000F (70)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_000F_70( Type documentType )
    {
        const string selector = "$['\u000f']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0010 (71)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0010_71( Type documentType )
    {
        const string selector = "$['\u0010']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0011 (72)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0011_72( Type documentType )
    {
        const string selector = "$['\u0011']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0012 (73)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0012_73( Type documentType )
    {
        const string selector = "$['\u0012']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0013 (74)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0013_74( Type documentType )
    {
        const string selector = "$['\u0013']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0014 (75)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0014_75( Type documentType )
    {
        const string selector = "$['\u0014']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0015 (76)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0015_76( Type documentType )
    {
        const string selector = "$['\u0015']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0016 (77)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0016_77( Type documentType )
    {
        const string selector = "$['\u0016']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0017 (78)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0017_78( Type documentType )
    {
        const string selector = "$['\u0017']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0018 (79)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0018_79( Type documentType )
    {
        const string selector = "$['\u0018']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0019 (80)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0019_80( Type documentType )
    {
        const string selector = "$['\u0019']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+001A (81)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_001A_81( Type documentType )
    {
        const string selector = "$['\u001a']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+001B (82)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_001B_82( Type documentType )
    {
        const string selector = "$['\u001b']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+001C (83)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_001C_83( Type documentType )
    {
        const string selector = "$['\u001c']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+001D (84)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_001D_84( Type documentType )
    {
        const string selector = "$['\u001d']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+001E (85)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_001E_85( Type documentType )
    {
        const string selector = "$['\u001e']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+001F (86)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_001F_86( Type documentType )
    {
        const string selector = "$['\u001f']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded U+0020 (87)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_U_0020_87( Type documentType )
    {
        const string selector = "$[' ']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  " ": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, escaped single quote (88)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__escaped_single_quote_88( Type documentType )
    {
        const string selector = "$['\\'']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "'": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, escaped reverse solidus (89)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__escaped_reverse_solidus_89( Type documentType )
    {
        const string selector = "$['\\\\']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "\\": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, escaped solidus (90)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__escaped_solidus_90( Type documentType )
    {
        const string selector = "$['\\/']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "/": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, escaped backspace (91)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__escaped_backspace_91( Type documentType )
    {
        const string selector = "$['\\b']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "\b": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, escaped form feed (92)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__escaped_form_feed_92( Type documentType )
    {
        const string selector = "$['\\f']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "\f": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, escaped line feed (93)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__escaped_line_feed_93( Type documentType )
    {
        const string selector = "$['\\n']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "\n": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, escaped carriage return (94)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__escaped_carriage_return_94( Type documentType )
    {
        const string selector = "$['\\r']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "\r": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, escaped tab (95)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__escaped_tab_95( Type documentType )
    {
        const string selector = "$['\\t']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "\t": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, escaped ☺, upper case hex (96)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__escaped____upper_case_hex_96( Type documentType )
    {
        const string selector = "$['\\u263A']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "☺": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, escaped ☺, lower case hex (97)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__escaped____lower_case_hex_97( Type documentType )
    {
        const string selector = "$['\\u263a']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "☺": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, surrogate pair 𝄞 (98)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__surrogate_pair____98( Type documentType )
    {
        const string selector = "$['\\uD834\\uDD1E']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "𝄞": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, surrogate pair 😀 (99)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__surrogate_pair____99( Type documentType )
    {
        const string selector = "$['\\uD83D\\uDE00']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "😀": "A"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, invalid escaped double quote (100)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__invalid_escaped_double_quote_100( Type documentType )
    {
        const string selector = "$['\\\"']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, embedded single quote (101)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__embedded_single_quote_101( Type documentType )
    {
        const string selector = "$[''']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"single quotes, incomplete escape (102)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__incomplete_escape_102( Type documentType )
    {
        const string selector = "$['\\']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"double quotes, empty (103)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_double_quotes__empty_103( Type documentType )
    {
        const string selector = "$[\"\"]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "A",
                  "b": "B",
                  "": "C"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "C"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"single quotes, empty (104)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_single_quotes__empty_104( Type documentType )
    {
        const string selector = "$['']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "A",
                  "b": "B",
                  "": "C"
                }
            """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "C"
                ]
            """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
}


