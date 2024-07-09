using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Cts.Tests
{
    [TestClass]
    public class CtsNameSelectorTest
    {
        
        [TestMethod( "double quotes (1)" )]
        public void Test_double_quotes_1()
        {
            var selector = "$[\"a\"]";
            var document = JsonNode.Parse(
                """
                {
                  "a": "A",
                  "b": "B"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, absent data (2)" )]
        public void Test_double_quotes__absent_data_2()
        {
            var selector = "$[\"c\"]";
            var document = JsonNode.Parse(
                """
                {
                  "a": "A",
                  "b": "B"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                []
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, array data (3)" )]
        public void Test_double_quotes__array_data_3()
        {
            var selector = "$[\"a\"]";
            var document = JsonNode.Parse(
                """
                [
                  "first",
                  "second"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                []
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, embedded U+0000 (4)" )]
        public void Test_double_quotes__embedded_U_0000_4()
        {
            var selector = "$[\"\u0000\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0001 (5)" )]
        public void Test_double_quotes__embedded_U_0001_5()
        {
            var selector = "$[\"\u0001\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0002 (6)" )]
        public void Test_double_quotes__embedded_U_0002_6()
        {
            var selector = "$[\"\u0002\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0003 (7)" )]
        public void Test_double_quotes__embedded_U_0003_7()
        {
            var selector = "$[\"\u0003\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0004 (8)" )]
        public void Test_double_quotes__embedded_U_0004_8()
        {
            var selector = "$[\"\u0004\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0005 (9)" )]
        public void Test_double_quotes__embedded_U_0005_9()
        {
            var selector = "$[\"\u0005\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0006 (10)" )]
        public void Test_double_quotes__embedded_U_0006_10()
        {
            var selector = "$[\"\u0006\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0007 (11)" )]
        public void Test_double_quotes__embedded_U_0007_11()
        {
            var selector = "$[\"\u0007\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0008 (12)" )]
        public void Test_double_quotes__embedded_U_0008_12()
        {
            var selector = "$[\"\b\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0009 (13)" )]
        public void Test_double_quotes__embedded_U_0009_13()
        {
            var selector = "$[\"\t\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+000A (14)" )]
        public void Test_double_quotes__embedded_U_000A_14()
        {
            var selector = "$[\"\n\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+000B (15)" )]
        public void Test_double_quotes__embedded_U_000B_15()
        {
            var selector = "$[\"\u000b\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+000C (16)" )]
        public void Test_double_quotes__embedded_U_000C_16()
        {
            var selector = "$[\"\f\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+000D (17)" )]
        public void Test_double_quotes__embedded_U_000D_17()
        {
            var selector = "$[\"\r\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+000E (18)" )]
        public void Test_double_quotes__embedded_U_000E_18()
        {
            var selector = "$[\"\u000e\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+000F (19)" )]
        public void Test_double_quotes__embedded_U_000F_19()
        {
            var selector = "$[\"\u000f\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0010 (20)" )]
        public void Test_double_quotes__embedded_U_0010_20()
        {
            var selector = "$[\"\u0010\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0011 (21)" )]
        public void Test_double_quotes__embedded_U_0011_21()
        {
            var selector = "$[\"\u0011\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0012 (22)" )]
        public void Test_double_quotes__embedded_U_0012_22()
        {
            var selector = "$[\"\u0012\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0013 (23)" )]
        public void Test_double_quotes__embedded_U_0013_23()
        {
            var selector = "$[\"\u0013\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0014 (24)" )]
        public void Test_double_quotes__embedded_U_0014_24()
        {
            var selector = "$[\"\u0014\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0015 (25)" )]
        public void Test_double_quotes__embedded_U_0015_25()
        {
            var selector = "$[\"\u0015\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0016 (26)" )]
        public void Test_double_quotes__embedded_U_0016_26()
        {
            var selector = "$[\"\u0016\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0017 (27)" )]
        public void Test_double_quotes__embedded_U_0017_27()
        {
            var selector = "$[\"\u0017\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0018 (28)" )]
        public void Test_double_quotes__embedded_U_0018_28()
        {
            var selector = "$[\"\u0018\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0019 (29)" )]
        public void Test_double_quotes__embedded_U_0019_29()
        {
            var selector = "$[\"\u0019\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+001A (30)" )]
        public void Test_double_quotes__embedded_U_001A_30()
        {
            var selector = "$[\"\u001a\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+001B (31)" )]
        public void Test_double_quotes__embedded_U_001B_31()
        {
            var selector = "$[\"\u001b\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+001C (32)" )]
        public void Test_double_quotes__embedded_U_001C_32()
        {
            var selector = "$[\"\u001c\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+001D (33)" )]
        public void Test_double_quotes__embedded_U_001D_33()
        {
            var selector = "$[\"\u001d\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+001E (34)" )]
        public void Test_double_quotes__embedded_U_001E_34()
        {
            var selector = "$[\"\u001e\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+001F (35)" )]
        public void Test_double_quotes__embedded_U_001F_35()
        {
            var selector = "$[\"\u001f\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded U+0020 (36)" )]
        public void Test_double_quotes__embedded_U_0020_36()
        {
            var selector = "$[\" \"]";
            var document = JsonNode.Parse(
                """
                {
                  " ": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, escaped double quote (37)" )]
        public void Test_double_quotes__escaped_double_quote_37()
        {
            var selector = "$[\"\\\"\"]";
            var document = JsonNode.Parse(
                """
                {
                  "\"": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, escaped reverse solidus (38)" )]
        public void Test_double_quotes__escaped_reverse_solidus_38()
        {
            var selector = "$[\"\\\\\"]";
            var document = JsonNode.Parse(
                """
                {
                  "\\": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, escaped solidus (39)" )]
        public void Test_double_quotes__escaped_solidus_39()
        {
            var selector = "$[\"\\/\"]";
            var document = JsonNode.Parse(
                """
                {
                  "/": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, escaped backspace (40)" )]
        public void Test_double_quotes__escaped_backspace_40()
        {
            var selector = "$[\"\\b\"]";
            var document = JsonNode.Parse(
                """
                {
                  "\b": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, escaped form feed (41)" )]
        public void Test_double_quotes__escaped_form_feed_41()
        {
            var selector = "$[\"\\f\"]";
            var document = JsonNode.Parse(
                """
                {
                  "\f": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, escaped line feed (42)" )]
        public void Test_double_quotes__escaped_line_feed_42()
        {
            var selector = "$[\"\\n\"]";
            var document = JsonNode.Parse(
                """
                {
                  "\n": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, escaped carriage return (43)" )]
        public void Test_double_quotes__escaped_carriage_return_43()
        {
            var selector = "$[\"\\r\"]";
            var document = JsonNode.Parse(
                """
                {
                  "\r": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, escaped tab (44)" )]
        public void Test_double_quotes__escaped_tab_44()
        {
            var selector = "$[\"\\t\"]";
            var document = JsonNode.Parse(
                """
                {
                  "\t": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, escaped ☺, upper case hex (45)" )]
        public void Test_double_quotes__escaped____upper_case_hex_45()
        {
            var selector = "$[\"\\u263A\"]";
            var document = JsonNode.Parse(
                """
                {
                  "☺": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, escaped ☺, lower case hex (46)" )]
        public void Test_double_quotes__escaped____lower_case_hex_46()
        {
            var selector = "$[\"\\u263a\"]";
            var document = JsonNode.Parse(
                """
                {
                  "☺": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, surrogate pair 𝄞 (47)" )]
        public void Test_double_quotes__surrogate_pair____47()
        {
            var selector = "$[\"\\uD834\\uDD1E\"]";
            var document = JsonNode.Parse(
                """
                {
                  "𝄞": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, surrogate pair 😀 (48)" )]
        public void Test_double_quotes__surrogate_pair____48()
        {
            var selector = "$[\"\\uD83D\\uDE00\"]";
            var document = JsonNode.Parse(
                """
                {
                  "😀": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "double quotes, invalid escaped single quote (49)" )]
        public void Test_double_quotes__invalid_escaped_single_quote_49()
        {
            var selector = "$[\"\\'\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, embedded double quote (50)" )]
        public void Test_double_quotes__embedded_double_quote_50()
        {
            var selector = "$[\"\"\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, incomplete escape (51)" )]
        public void Test_double_quotes__incomplete_escape_51()
        {
            var selector = "$[\"\\\"]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes (52)" )]
        public void Test_single_quotes_52()
        {
            var selector = "$['a']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "A",
                  "b": "B"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, absent data (53)" )]
        public void Test_single_quotes__absent_data_53()
        {
            var selector = "$['c']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "A",
                  "b": "B"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                []
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, array data (54)" )]
        public void Test_single_quotes__array_data_54()
        {
            var selector = "$['a']";
            var document = JsonNode.Parse(
                """
                [
                  "first",
                  "second"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                []
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, embedded U+0000 (55)" )]
        public void Test_single_quotes__embedded_U_0000_55()
        {
            var selector = "$['\u0000']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0001 (56)" )]
        public void Test_single_quotes__embedded_U_0001_56()
        {
            var selector = "$['\u0001']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0002 (57)" )]
        public void Test_single_quotes__embedded_U_0002_57()
        {
            var selector = "$['\u0002']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0003 (58)" )]
        public void Test_single_quotes__embedded_U_0003_58()
        {
            var selector = "$['\u0003']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0004 (59)" )]
        public void Test_single_quotes__embedded_U_0004_59()
        {
            var selector = "$['\u0004']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0005 (60)" )]
        public void Test_single_quotes__embedded_U_0005_60()
        {
            var selector = "$['\u0005']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0006 (61)" )]
        public void Test_single_quotes__embedded_U_0006_61()
        {
            var selector = "$['\u0006']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0007 (62)" )]
        public void Test_single_quotes__embedded_U_0007_62()
        {
            var selector = "$['\u0007']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0008 (63)" )]
        public void Test_single_quotes__embedded_U_0008_63()
        {
            var selector = "$['\b']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0009 (64)" )]
        public void Test_single_quotes__embedded_U_0009_64()
        {
            var selector = "$['\t']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+000A (65)" )]
        public void Test_single_quotes__embedded_U_000A_65()
        {
            var selector = "$['\n']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+000B (66)" )]
        public void Test_single_quotes__embedded_U_000B_66()
        {
            var selector = "$['\u000b']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+000C (67)" )]
        public void Test_single_quotes__embedded_U_000C_67()
        {
            var selector = "$['\f']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+000D (68)" )]
        public void Test_single_quotes__embedded_U_000D_68()
        {
            var selector = "$['\r']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+000E (69)" )]
        public void Test_single_quotes__embedded_U_000E_69()
        {
            var selector = "$['\u000e']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+000F (70)" )]
        public void Test_single_quotes__embedded_U_000F_70()
        {
            var selector = "$['\u000f']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0010 (71)" )]
        public void Test_single_quotes__embedded_U_0010_71()
        {
            var selector = "$['\u0010']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0011 (72)" )]
        public void Test_single_quotes__embedded_U_0011_72()
        {
            var selector = "$['\u0011']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0012 (73)" )]
        public void Test_single_quotes__embedded_U_0012_73()
        {
            var selector = "$['\u0012']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0013 (74)" )]
        public void Test_single_quotes__embedded_U_0013_74()
        {
            var selector = "$['\u0013']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0014 (75)" )]
        public void Test_single_quotes__embedded_U_0014_75()
        {
            var selector = "$['\u0014']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0015 (76)" )]
        public void Test_single_quotes__embedded_U_0015_76()
        {
            var selector = "$['\u0015']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0016 (77)" )]
        public void Test_single_quotes__embedded_U_0016_77()
        {
            var selector = "$['\u0016']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0017 (78)" )]
        public void Test_single_quotes__embedded_U_0017_78()
        {
            var selector = "$['\u0017']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0018 (79)" )]
        public void Test_single_quotes__embedded_U_0018_79()
        {
            var selector = "$['\u0018']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0019 (80)" )]
        public void Test_single_quotes__embedded_U_0019_80()
        {
            var selector = "$['\u0019']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+001A (81)" )]
        public void Test_single_quotes__embedded_U_001A_81()
        {
            var selector = "$['\u001a']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+001B (82)" )]
        public void Test_single_quotes__embedded_U_001B_82()
        {
            var selector = "$['\u001b']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+001C (83)" )]
        public void Test_single_quotes__embedded_U_001C_83()
        {
            var selector = "$['\u001c']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+001D (84)" )]
        public void Test_single_quotes__embedded_U_001D_84()
        {
            var selector = "$['\u001d']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+001E (85)" )]
        public void Test_single_quotes__embedded_U_001E_85()
        {
            var selector = "$['\u001e']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+001F (86)" )]
        public void Test_single_quotes__embedded_U_001F_86()
        {
            var selector = "$['\u001f']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded U+0020 (87)" )]
        public void Test_single_quotes__embedded_U_0020_87()
        {
            var selector = "$[' ']";
            var document = JsonNode.Parse(
                """
                {
                  " ": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, escaped single quote (88)" )]
        public void Test_single_quotes__escaped_single_quote_88()
        {
            var selector = "$['\\'']";
            var document = JsonNode.Parse(
                """
                {
                  "'": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, escaped reverse solidus (89)" )]
        public void Test_single_quotes__escaped_reverse_solidus_89()
        {
            var selector = "$['\\\\']";
            var document = JsonNode.Parse(
                """
                {
                  "\\": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, escaped solidus (90)" )]
        public void Test_single_quotes__escaped_solidus_90()
        {
            var selector = "$['\\/']";
            var document = JsonNode.Parse(
                """
                {
                  "/": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, escaped backspace (91)" )]
        public void Test_single_quotes__escaped_backspace_91()
        {
            var selector = "$['\\b']";
            var document = JsonNode.Parse(
                """
                {
                  "\b": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, escaped form feed (92)" )]
        public void Test_single_quotes__escaped_form_feed_92()
        {
            var selector = "$['\\f']";
            var document = JsonNode.Parse(
                """
                {
                  "\f": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, escaped line feed (93)" )]
        public void Test_single_quotes__escaped_line_feed_93()
        {
            var selector = "$['\\n']";
            var document = JsonNode.Parse(
                """
                {
                  "\n": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, escaped carriage return (94)" )]
        public void Test_single_quotes__escaped_carriage_return_94()
        {
            var selector = "$['\\r']";
            var document = JsonNode.Parse(
                """
                {
                  "\r": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, escaped tab (95)" )]
        public void Test_single_quotes__escaped_tab_95()
        {
            var selector = "$['\\t']";
            var document = JsonNode.Parse(
                """
                {
                  "\t": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, escaped ☺, upper case hex (96)" )]
        public void Test_single_quotes__escaped____upper_case_hex_96()
        {
            var selector = "$['\\u263A']";
            var document = JsonNode.Parse(
                """
                {
                  "☺": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, escaped ☺, lower case hex (97)" )]
        public void Test_single_quotes__escaped____lower_case_hex_97()
        {
            var selector = "$['\\u263a']";
            var document = JsonNode.Parse(
                """
                {
                  "☺": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, surrogate pair 𝄞 (98)" )]
        public void Test_single_quotes__surrogate_pair____98()
        {
            var selector = "$['\\uD834\\uDD1E']";
            var document = JsonNode.Parse(
                """
                {
                  "𝄞": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, surrogate pair 😀 (99)" )]
        public void Test_single_quotes__surrogate_pair____99()
        {
            var selector = "$['\\uD83D\\uDE00']";
            var document = JsonNode.Parse(
                """
                {
                  "😀": "A"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "A"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, invalid escaped double quote (100)" )]
        public void Test_single_quotes__invalid_escaped_double_quote_100()
        {
            var selector = "$['\\\"']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, embedded single quote (101)" )]
        public void Test_single_quotes__embedded_single_quote_101()
        {
            var selector = "$[''']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "single quotes, incomplete escape (102)" )]
        public void Test_single_quotes__incomplete_escape_102()
        {
            var selector = "$['\\']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "double quotes, empty (103)" )]
        public void Test_double_quotes__empty_103()
        {
            var selector = "$[\"\"]";
            var document = JsonNode.Parse(
                """
                {
                  "a": "A",
                  "b": "B",
                  "": "C"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "C"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "single quotes, empty (104)" )]
        public void Test_single_quotes__empty_104()
        {
            var selector = "$['']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "A",
                  "b": "B",
                  "": "C"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "C"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
    }
}

