using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Cts.Tests
{
    [TestClass]
    public class CtsFunctionsTest
    {
        
        [TestMethod( "count, count function (1)" )]
        public void Test_count__count_function_1()
        {
            var selector = "$[?count(@..*)>2]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": [
                      1,
                      2,
                      3
                    ]
                  },
                  {
                    "a": [
                      1
                    ],
                    "d": "f"
                  },
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": [
                      1,
                      2,
                      3
                    ]
                  },
                  {
                    "a": [
                      1
                    ],
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "count, single-node arg (2)" )]
        public void Test_count__single_node_arg_2()
        {
            var selector = "$[?count(@.a)>1]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": [
                      1,
                      2,
                      3
                    ]
                  },
                  {
                    "a": [
                      1
                    ],
                    "d": "f"
                  },
                  {
                    "a": 1,
                    "d": "f"
                  }
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
        
        [TestMethod( "count, multiple-selector arg (3)" )]
        public void Test_count__multiple_selector_arg_3()
        {
            var selector = "$[?count(@['a','d'])>1]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": [
                      1,
                      2,
                      3
                    ]
                  },
                  {
                    "a": [
                      1
                    ],
                    "d": "f"
                  },
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": [
                      1
                    ],
                    "d": "f"
                  },
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "count, non-query arg, number (4)" )]
        public void Test_count__non_query_arg__number_4()
        {
            var selector = "$[?count(1)>2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "count, non-query arg, string (5)" )]
        public void Test_count__non_query_arg__string_5()
        {
            var selector = "$[?count('string')>2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "count, non-query arg, true (6)" )]
        public void Test_count__non_query_arg__true_6()
        {
            var selector = "$[?count(true)>2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "count, non-query arg, false (7)" )]
        public void Test_count__non_query_arg__false_7()
        {
            var selector = "$[?count(false)>2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "count, non-query arg, null (8)" )]
        public void Test_count__non_query_arg__null_8()
        {
            var selector = "$[?count(null)>2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "count, result must be compared (9)" )]
        public void Test_count__result_must_be_compared_9()
        {
            var selector = "$[?count(@..*)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "count, no params (10)" )]
        public void Test_count__no_params_10()
        {
            var selector = "$[?count()==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "count, too many params (11)" )]
        public void Test_count__too_many_params_11()
        {
            var selector = "$[?count(@.a,@.b)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "length, string data (12)" )]
        public void Test_length__string_data_12()
        {
            var selector = "$[?length(@.a)>=2]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "ab"
                  },
                  {
                    "a": "d"
                  }
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "ab"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "length, string data, unicode (13)" )]
        public void Test_length__string_data__unicode_13()
        {
            var selector = "$[?length(@)==2]";
            var document = JsonNode.Parse(
                """
                [
                  "☺",
                  "☺☺",
                  "☺☺☺",
                  "ж",
                  "жж",
                  "жжж",
                  "磨",
                  "阿美",
                  "形声字"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "☺☺",
                  "жж",
                  "阿美"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "length, array data (14)" )]
        public void Test_length__array_data_14()
        {
            var selector = "$[?length(@.a)>=2]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": [
                      1,
                      2,
                      3
                    ]
                  },
                  {
                    "a": [
                      1
                    ]
                  }
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": [
                      1,
                      2,
                      3
                    ]
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "length, missing data (15)" )]
        public void Test_length__missing_data_15()
        {
            var selector = "$[?length(@.a)>=2]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "d": "f"
                  }
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
        
        [TestMethod( "length, number arg (16)" )]
        public void Test_length__number_arg_16()
        {
            var selector = "$[?length(1)>=2]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "d": "f"
                  }
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
        
        [TestMethod( "length, true arg (17)" )]
        public void Test_length__true_arg_17()
        {
            var selector = "$[?length(true)>=2]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "d": "f"
                  }
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
        
        [TestMethod( "length, false arg (18)" )]
        public void Test_length__false_arg_18()
        {
            var selector = "$[?length(false)>=2]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "d": "f"
                  }
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
        
        [TestMethod( "length, null arg (19)" )]
        public void Test_length__null_arg_19()
        {
            var selector = "$[?length(null)>=2]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "d": "f"
                  }
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
        
        [TestMethod( "length, result must be compared (20)" )]
        public void Test_length__result_must_be_compared_20()
        {
            var selector = "$[?length(@.a)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "length, no params (21)" )]
        public void Test_length__no_params_21()
        {
            var selector = "$[?length()==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "length, too many params (22)" )]
        public void Test_length__too_many_params_22()
        {
            var selector = "$[?length(@.a,@.b)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "length, non-singular query arg (23)" )]
        public void Test_length__non_singular_query_arg_23()
        {
            var selector = "$[?length(@.*)<3]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "length, arg is a function expression (24)" )]
        public void Test_length__arg_is_a_function_expression_24()
        {
            var selector = "$.values[?length(@.a)==length(value($..c))]";
            var document = JsonNode.Parse(
                """
                {
                  "c": "cd",
                  "values": [
                    {
                      "a": "ab"
                    },
                    {
                      "a": "d"
                    }
                  ]
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "ab"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "length, arg is special nothing (25)" )]
        public void Test_length__arg_is_special_nothing_25()
        {
            var selector = "$[?length(value(@.a))>0]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "ab"
                  },
                  {
                    "c": "d"
                  },
                  {
                    "a": null
                  }
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "ab"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, found match (26)" )]
        public void Test_match__found_match_26()
        {
            var selector = "$[?match(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "ab"
                  }
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "ab"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, double quotes (27)" )]
        public void Test_match__double_quotes_27()
        {
            var selector = "$[?match(@.a, \"a.*\")]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "ab"
                  }
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "ab"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, regex from the document (28)" )]
        public void Test_match__regex_from_the_document_28()
        {
            var selector = "$.values[?match(@, $.regex)]";
            var document = JsonNode.Parse(
                """
                {
                  "regex": "b.?b",
                  "values": [
                    "abc",
                    "bcd",
                    "bab",
                    "bba",
                    "bbab",
                    "b",
                    true,
                    [],
                    {}
                  ]
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "bab"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, don't select match (29)" )]
        public void Test_match__don_t_select_match_29()
        {
            var selector = "$[?!match(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "ab"
                  }
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
        
        [TestMethod( "match, not a match (30)" )]
        public void Test_match__not_a_match_30()
        {
            var selector = "$[?match(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "bc"
                  }
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
        
        [TestMethod( "match, select non-match (31)" )]
        public void Test_match__select_non_match_31()
        {
            var selector = "$[?!match(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "bc"
                  }
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "bc"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, non-string first arg (32)" )]
        public void Test_match__non_string_first_arg_32()
        {
            var selector = "$[?match(1, 'a.*')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "bc"
                  }
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
        
        [TestMethod( "match, non-string second arg (33)" )]
        public void Test_match__non_string_second_arg_33()
        {
            var selector = "$[?match(@.a, 1)]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "bc"
                  }
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
        
        [TestMethod( "match, filter, match function, unicode char class, uppercase (34)" )]
        public void Test_match__filter__match_function__unicode_char_class__uppercase_34()
        {
            var selector = "$[?match(@, '\\\\p{Lu}')]";
            var document = JsonNode.Parse(
                """
                [
                  "ж",
                  "Ж",
                  "1",
                  "жЖ",
                  true,
                  [],
                  {}
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "Ж"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, filter, match function, unicode char class negated, uppercase (35)" )]
        public void Test_match__filter__match_function__unicode_char_class_negated__uppercase_35()
        {
            var selector = "$[?match(@, '\\\\P{Lu}')]";
            var document = JsonNode.Parse(
                """
                [
                  "ж",
                  "Ж",
                  "1",
                  true,
                  [],
                  {}
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "ж",
                  "1"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, filter, match function, unicode, surrogate pair (36)" )]
        public void Test_match__filter__match_function__unicode__surrogate_pair_36()
        {
            var selector = "$[?match(@, 'a.b')]";
            var document = JsonNode.Parse(
                """
                [
                  "a𐄁b",
                  "ab",
                  "1",
                  true,
                  [],
                  {}
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "a𐄁b"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, dot matcher on \u2028 (37)" )]
        public void Test_match__dot_matcher_on__u2028_37()
        {
            var selector = "$[?match(@, '.')]";
            var document = JsonNode.Parse(
                """
                [
                  "\u2028",
                  "\r",
                  "\n",
                  true,
                  [],
                  {}
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "\u2028"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, dot matcher on \u2029 (38)" )]
        public void Test_match__dot_matcher_on__u2029_38()
        {
            var selector = "$[?match(@, '.')]";
            var document = JsonNode.Parse(
                """
                [
                  "\u2029",
                  "\r",
                  "\n",
                  true,
                  [],
                  {}
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "\u2029"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, result cannot be compared (39)" )]
        public void Test_match__result_cannot_be_compared_39()
        {
            var selector = "$[?match(@.a, 'a.*')==true]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "match, too few params (40)" )]
        public void Test_match__too_few_params_40()
        {
            var selector = "$[?match(@.a)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "match, too many params (41)" )]
        public void Test_match__too_many_params_41()
        {
            var selector = "$[?match(@.a,@.b,@.c)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "match, arg is a function expression (42)" )]
        public void Test_match__arg_is_a_function_expression_42()
        {
            var selector = "$.values[?match(@.a, value($..['regex']))]";
            var document = JsonNode.Parse(
                """
                {
                  "regex": "a.*",
                  "values": [
                    {
                      "a": "ab"
                    },
                    {
                      "a": "ba"
                    }
                  ]
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "ab"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, dot in character class (43)" )]
        public void Test_match__dot_in_character_class_43()
        {
            var selector = "$[?match(@, 'a[.b]c')]";
            var document = JsonNode.Parse(
                """
                [
                  "abc",
                  "a.c",
                  "axc"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "abc",
                  "a.c"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, escaped dot (44)" )]
        public void Test_match__escaped_dot_44()
        {
            var selector = "$[?match(@, 'a\\\\.c')]";
            var document = JsonNode.Parse(
                """
                [
                  "abc",
                  "a.c",
                  "axc"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "a.c"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, escaped backslash before dot (45)" )]
        public void Test_match__escaped_backslash_before_dot_45()
        {
            var selector = "$[?match(@, 'a\\\\\\\\.c')]";
            var document = JsonNode.Parse(
                """
                [
                  "abc",
                  "a.c",
                  "axc",
                  "a\\\u2028c"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "a\\\u2028c"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, escaped left square bracket (46)" )]
        public void Test_match__escaped_left_square_bracket_46()
        {
            var selector = "$[?match(@, 'a\\\\[.c')]";
            var document = JsonNode.Parse(
                """
                [
                  "abc",
                  "a.c",
                  "a[\u2028c"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "a[\u2028c"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, escaped right square bracket (47)" )]
        public void Test_match__escaped_right_square_bracket_47()
        {
            var selector = "$[?match(@, 'a[\\\\].]c')]";
            var document = JsonNode.Parse(
                """
                [
                  "abc",
                  "a.c",
                  "a\u2028c",
                  "a]c"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "a.c",
                  "a]c"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, explicit caret (48)" )]
        public void Test_match__explicit_caret_48()
        {
            var selector = "$[?match(@, '^ab.*')]";
            var document = JsonNode.Parse(
                """
                [
                  "abc",
                  "axc",
                  "ab",
                  "xab"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "abc",
                  "ab"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "match, explicit dollar (49)" )]
        public void Test_match__explicit_dollar_49()
        {
            var selector = "$[?match(@, '.*bc$')]";
            var document = JsonNode.Parse(
                """
                [
                  "abc",
                  "axc",
                  "ab",
                  "abcx"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "abc"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, at the end (50)" )]
        public void Test_search__at_the_end_50()
        {
            var selector = "$[?search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "the end is ab"
                  }
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "the end is ab"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, double quotes (51)" )]
        public void Test_search__double_quotes_51()
        {
            var selector = "$[?search(@.a, \"a.*\")]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "the end is ab"
                  }
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "the end is ab"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, at the start (52)" )]
        public void Test_search__at_the_start_52()
        {
            var selector = "$[?search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "ab is at the start"
                  }
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "ab is at the start"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, in the middle (53)" )]
        public void Test_search__in_the_middle_53()
        {
            var selector = "$[?search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "contains two matches"
                  }
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "contains two matches"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, regex from the document (54)" )]
        public void Test_search__regex_from_the_document_54()
        {
            var selector = "$.values[?search(@, $.regex)]";
            var document = JsonNode.Parse(
                """
                {
                  "regex": "b.?b",
                  "values": [
                    "abc",
                    "bcd",
                    "bab",
                    "bba",
                    "bbab",
                    "b",
                    true,
                    [],
                    {}
                  ]
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "bab",
                  "bba",
                  "bbab"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, don't select match (55)" )]
        public void Test_search__don_t_select_match_55()
        {
            var selector = "$[?!search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "contains two matches"
                  }
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
        
        [TestMethod( "search, not a match (56)" )]
        public void Test_search__not_a_match_56()
        {
            var selector = "$[?search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "bc"
                  }
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
        
        [TestMethod( "search, select non-match (57)" )]
        public void Test_search__select_non_match_57()
        {
            var selector = "$[?!search(@.a, 'a.*')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "bc"
                  }
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "bc"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, non-string first arg (58)" )]
        public void Test_search__non_string_first_arg_58()
        {
            var selector = "$[?search(1, 'a.*')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "bc"
                  }
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
        
        [TestMethod( "search, non-string second arg (59)" )]
        public void Test_search__non_string_second_arg_59()
        {
            var selector = "$[?search(@.a, 1)]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "bc"
                  }
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
        
        [TestMethod( "search, filter, search function, unicode char class, uppercase (60)" )]
        public void Test_search__filter__search_function__unicode_char_class__uppercase_60()
        {
            var selector = "$[?search(@, '\\\\p{Lu}')]";
            var document = JsonNode.Parse(
                """
                [
                  "ж",
                  "Ж",
                  "1",
                  "жЖ",
                  true,
                  [],
                  {}
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "Ж",
                  "жЖ"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, filter, search function, unicode char class negated, uppercase (61)" )]
        public void Test_search__filter__search_function__unicode_char_class_negated__uppercase_61()
        {
            var selector = "$[?search(@, '\\\\P{Lu}')]";
            var document = JsonNode.Parse(
                """
                [
                  "ж",
                  "Ж",
                  "1",
                  true,
                  [],
                  {}
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "ж",
                  "1"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, filter, search function, unicode, surrogate pair (62)" )]
        public void Test_search__filter__search_function__unicode__surrogate_pair_62()
        {
            var selector = "$[?search(@, 'a.b')]";
            var document = JsonNode.Parse(
                """
                [
                  "a𐄁bc",
                  "abc",
                  "1",
                  true,
                  [],
                  {}
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "a𐄁bc"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, dot matcher on \u2028 (63)" )]
        public void Test_search__dot_matcher_on__u2028_63()
        {
            var selector = "$[?search(@, '.')]";
            var document = JsonNode.Parse(
                """
                [
                  "\u2028",
                  "\r\u2028\n",
                  "\r",
                  "\n",
                  true,
                  [],
                  {}
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "\u2028",
                  "\r\u2028\n"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, dot matcher on \u2029 (64)" )]
        public void Test_search__dot_matcher_on__u2029_64()
        {
            var selector = "$[?search(@, '.')]";
            var document = JsonNode.Parse(
                """
                [
                  "\u2029",
                  "\r\u2029\n",
                  "\r",
                  "\n",
                  true,
                  [],
                  {}
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "\u2029",
                  "\r\u2029\n"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, result cannot be compared (65)" )]
        public void Test_search__result_cannot_be_compared_65()
        {
            var selector = "$[?search(@.a, 'a.*')==true]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "search, too few params (66)" )]
        public void Test_search__too_few_params_66()
        {
            var selector = "$[?search(@.a)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "search, too many params (67)" )]
        public void Test_search__too_many_params_67()
        {
            var selector = "$[?search(@.a,@.b,@.c)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "search, arg is a function expression (68)" )]
        public void Test_search__arg_is_a_function_expression_68()
        {
            var selector = "$.values[?search(@, value($..['regex']))]";
            var document = JsonNode.Parse(
                """
                {
                  "regex": "b.?b",
                  "values": [
                    "abc",
                    "bcd",
                    "bab",
                    "bba",
                    "bbab",
                    "b",
                    true,
                    [],
                    {}
                  ]
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "bab",
                  "bba",
                  "bbab"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, dot in character class (69)" )]
        public void Test_search__dot_in_character_class_69()
        {
            var selector = "$[?search(@, 'a[.b]c')]";
            var document = JsonNode.Parse(
                """
                [
                  "x abc y",
                  "x a.c y",
                  "x axc y"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "x abc y",
                  "x a.c y"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, escaped dot (70)" )]
        public void Test_search__escaped_dot_70()
        {
            var selector = "$[?search(@, 'a\\\\.c')]";
            var document = JsonNode.Parse(
                """
                [
                  "x abc y",
                  "x a.c y",
                  "x axc y"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "x a.c y"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, escaped backslash before dot (71)" )]
        public void Test_search__escaped_backslash_before_dot_71()
        {
            var selector = "$[?search(@, 'a\\\\\\\\.c')]";
            var document = JsonNode.Parse(
                """
                [
                  "x abc y",
                  "x a.c y",
                  "x axc y",
                  "x a\\\u2028c y"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "x a\\\u2028c y"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, escaped left square bracket (72)" )]
        public void Test_search__escaped_left_square_bracket_72()
        {
            var selector = "$[?search(@, 'a\\\\[.c')]";
            var document = JsonNode.Parse(
                """
                [
                  "x abc y",
                  "x a.c y",
                  "x a[\u2028c y"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "x a[\u2028c y"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "search, escaped right square bracket (73)" )]
        public void Test_search__escaped_right_square_bracket_73()
        {
            var selector = "$[?search(@, 'a[\\\\].]c')]";
            var document = JsonNode.Parse(
                """
                [
                  "x abc y",
                  "x a.c y",
                  "x a\u2028c y",
                  "x a]c y"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "x a.c y",
                  "x a]c y"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "value, single-value nodelist (74)" )]
        public void Test_value__single_value_nodelist_74()
        {
            var selector = "$[?value(@.*)==4]";
            var document = JsonNode.Parse(
                """
                [
                  [
                    4
                  ],
                  {
                    "foo": 4
                  },
                  [
                    5
                  ],
                  {
                    "foo": 5
                  },
                  4
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  [
                    4
                  ],
                  {
                    "foo": 4
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "value, multi-value nodelist (75)" )]
        public void Test_value__multi_value_nodelist_75()
        {
            var selector = "$[?value(@.*)==4]";
            var document = JsonNode.Parse(
                """
                [
                  [
                    4,
                    4
                  ],
                  {
                    "foo": 4,
                    "bar": 4
                  }
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
        
        [TestMethod( "value, too few params (76)" )]
        public void Test_value__too_few_params_76()
        {
            var selector = "$[?value()==4]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "value, too many params (77)" )]
        public void Test_value__too_many_params_77()
        {
            var selector = "$[?value(@.a,@.b)==4]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "value, result must be compared (78)" )]
        public void Test_value__result_must_be_compared_78()
        {
            var selector = "$[?value(@.a)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
    }
}

