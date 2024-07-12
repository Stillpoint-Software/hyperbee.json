// This file was auto generated.

using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Cts.Tests
{
    [TestClass]
    public class CtsWhitespaceTest
    {

        [TestMethod( @"filter, space between question mark and expression (1)" )]
        public void Test_filter__space_between_question_mark_and_expression_1()
        {
            var selector = "$[? @.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"filter, newline between question mark and expression (2)" )]
        public void Test_filter__newline_between_question_mark_and_expression_2()
        {
            var selector = "$[?\n@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"filter, tab between question mark and expression (3)" )]
        public void Test_filter__tab_between_question_mark_and_expression_3()
        {
            var selector = "$[?\t@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"filter, return between question mark and expression (4)" )]
        public void Test_filter__return_between_question_mark_and_expression_4()
        {
            var selector = "$[?\r@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"filter, space between question mark and parenthesized expression (5)" )]
        public void Test_filter__space_between_question_mark_and_parenthesized_expression_5()
        {
            var selector = "$[? (@.a)]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"filter, newline between question mark and parenthesized expression (6)" )]
        public void Test_filter__newline_between_question_mark_and_parenthesized_expression_6()
        {
            var selector = "$[?\n(@.a)]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"filter, tab between question mark and parenthesized expression (7)" )]
        public void Test_filter__tab_between_question_mark_and_parenthesized_expression_7()
        {
            var selector = "$[?\t(@.a)]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"filter, return between question mark and parenthesized expression (8)" )]
        public void Test_filter__return_between_question_mark_and_parenthesized_expression_8()
        {
            var selector = "$[?\r(@.a)]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"filter, space between parenthesized expression and bracket (9)" )]
        public void Test_filter__space_between_parenthesized_expression_and_bracket_9()
        {
            var selector = "$[?(@.a) ]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"filter, newline between parenthesized expression and bracket (10)" )]
        public void Test_filter__newline_between_parenthesized_expression_and_bracket_10()
        {
            var selector = "$[?(@.a)\n]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"filter, tab between parenthesized expression and bracket (11)" )]
        public void Test_filter__tab_between_parenthesized_expression_and_bracket_11()
        {
            var selector = "$[?(@.a)\t]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"filter, return between parenthesized expression and bracket (12)" )]
        public void Test_filter__return_between_parenthesized_expression_and_bracket_12()
        {
            var selector = "$[?(@.a)\r]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"filter, space between bracket and question mark (13)" )]
        public void Test_filter__space_between_bracket_and_question_mark_13()
        {
            var selector = "$[ ?@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"filter, newline between bracket and question mark (14)" )]
        public void Test_filter__newline_between_bracket_and_question_mark_14()
        {
            var selector = "$[\n?@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"filter, tab between bracket and question mark (15)" )]
        public void Test_filter__tab_between_bracket_and_question_mark_15()
        {
            var selector = "$[\t?@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"filter, return between bracket and question mark (16)" )]
        public void Test_filter__return_between_bracket_and_question_mark_16()
        {
            var selector = "$[\r?@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, space between function name and parenthesis (17)" )]
        public void Test_functions__space_between_function_name_and_parenthesis_17()
        {
            var selector = "$[?count (@.*)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"functions, newline between function name and parenthesis (18)" )]
        public void Test_functions__newline_between_function_name_and_parenthesis_18()
        {
            var selector = "$[?count\n(@.*)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"functions, tab between function name and parenthesis (19)" )]
        public void Test_functions__tab_between_function_name_and_parenthesis_19()
        {
            var selector = "$[?count\t(@.*)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"functions, return between function name and parenthesis (20)" )]
        public void Test_functions__return_between_function_name_and_parenthesis_20()
        {
            var selector = "$[?count\r(@.*)==1]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"functions, space between parenthesis and arg (21)" )]
        public void Test_functions__space_between_parenthesis_and_arg_21()
        {
            var selector = "$[?count( @.*)==1]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, newline between parenthesis and arg (22)" )]
        public void Test_functions__newline_between_parenthesis_and_arg_22()
        {
            var selector = "$[?count(\n@.*)==1]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, tab between parenthesis and arg (23)" )]
        public void Test_functions__tab_between_parenthesis_and_arg_23()
        {
            var selector = "$[?count(\t@.*)==1]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, return between parenthesis and arg (24)" )]
        public void Test_functions__return_between_parenthesis_and_arg_24()
        {
            var selector = "$[?count(\r@.*)==1]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, space between arg and comma (25)" )]
        public void Test_functions__space_between_arg_and_comma_25()
        {
            var selector = "$[?search(@ ,'[a-z]+')]";
            var document = JsonNode.Parse(
                """
                [
                  "foo",
                  "123"
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "foo"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, newline between arg and comma (26)" )]
        public void Test_functions__newline_between_arg_and_comma_26()
        {
            var selector = "$[?search(@\n,'[a-z]+')]";
            var document = JsonNode.Parse(
                """
                [
                  "foo",
                  "123"
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "foo"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, tab between arg and comma (27)" )]
        public void Test_functions__tab_between_arg_and_comma_27()
        {
            var selector = "$[?search(@\t,'[a-z]+')]";
            var document = JsonNode.Parse(
                """
                [
                  "foo",
                  "123"
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "foo"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, return between arg and comma (28)" )]
        public void Test_functions__return_between_arg_and_comma_28()
        {
            var selector = "$[?search(@\r,'[a-z]+')]";
            var document = JsonNode.Parse(
                """
                [
                  "foo",
                  "123"
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "foo"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, space between comma and arg (29)" )]
        public void Test_functions__space_between_comma_and_arg_29()
        {
            var selector = "$[?search(@, '[a-z]+')]";
            var document = JsonNode.Parse(
                """
                [
                  "foo",
                  "123"
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "foo"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, newline between comma and arg (30)" )]
        public void Test_functions__newline_between_comma_and_arg_30()
        {
            var selector = "$[?search(@,\n'[a-z]+')]";
            var document = JsonNode.Parse(
                """
                [
                  "foo",
                  "123"
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "foo"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, tab between comma and arg (31)" )]
        public void Test_functions__tab_between_comma_and_arg_31()
        {
            var selector = "$[?search(@,\t'[a-z]+')]";
            var document = JsonNode.Parse(
                """
                [
                  "foo",
                  "123"
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "foo"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, return between comma and arg (32)" )]
        public void Test_functions__return_between_comma_and_arg_32()
        {
            var selector = "$[?search(@,\r'[a-z]+')]";
            var document = JsonNode.Parse(
                """
                [
                  "foo",
                  "123"
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "foo"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, space between arg and parenthesis (33)" )]
        public void Test_functions__space_between_arg_and_parenthesis_33()
        {
            var selector = "$[?count(@.* )==1]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, newline between arg and parenthesis (34)" )]
        public void Test_functions__newline_between_arg_and_parenthesis_34()
        {
            var selector = "$[?count(@.*\n)==1]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, tab between arg and parenthesis (35)" )]
        public void Test_functions__tab_between_arg_and_parenthesis_35()
        {
            var selector = "$[?count(@.*\t)==1]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, return between arg and parenthesis (36)" )]
        public void Test_functions__return_between_arg_and_parenthesis_36()
        {
            var selector = "$[?count(@.*\r)==1]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, spaces in a relative singular selector (37)" )]
        public void Test_functions__spaces_in_a_relative_singular_selector_37()
        {
            var selector = "$[?length(@ .a .b) == 3]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": {
                      "b": "foo"
                    }
                  },
                  {}
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": {
                      "b": "foo"
                    }
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, newlines in a relative singular selector (38)" )]
        public void Test_functions__newlines_in_a_relative_singular_selector_38()
        {
            var selector = "$[?length(@\n.a\n.b) == 3]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": {
                      "b": "foo"
                    }
                  },
                  {}
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": {
                      "b": "foo"
                    }
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, tabs in a relative singular selector (39)" )]
        public void Test_functions__tabs_in_a_relative_singular_selector_39()
        {
            var selector = "$[?length(@\t.a\t.b) == 3]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": {
                      "b": "foo"
                    }
                  },
                  {}
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": {
                      "b": "foo"
                    }
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, returns in a relative singular selector (40)" )]
        public void Test_functions__returns_in_a_relative_singular_selector_40()
        {
            var selector = "$[?length(@\r.a\r.b) == 3]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": {
                      "b": "foo"
                    }
                  },
                  {}
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": {
                      "b": "foo"
                    }
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, spaces in an absolute singular selector (41)" )]
        public void Test_functions__spaces_in_an_absolute_singular_selector_41()
        {
            var selector = "$..[?length(@)==length($ [0] .a)]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "foo"
                  },
                  {}
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "foo"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, newlines in an absolute singular selector (42)" )]
        public void Test_functions__newlines_in_an_absolute_singular_selector_42()
        {
            var selector = "$..[?length(@)==length($\n[0]\n.a)]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "foo"
                  },
                  {}
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "foo"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, tabs in an absolute singular selector (43)" )]
        public void Test_functions__tabs_in_an_absolute_singular_selector_43()
        {
            var selector = "$..[?length(@)==length($\t[0]\t.a)]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "foo"
                  },
                  {}
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "foo"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"functions, returns in an absolute singular selector (44)" )]
        public void Test_functions__returns_in_an_absolute_singular_selector_44()
        {
            var selector = "$..[?length(@)==length($\r[0]\r.a)]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "foo"
                  },
                  {}
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "foo"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space before || (45)" )]
        public void Test_operators__space_before____45()
        {
            var selector = "$[?@.a ||@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "c": 3
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline before || (46)" )]
        public void Test_operators__newline_before____46()
        {
            var selector = "$[?@.a\n||@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "c": 3
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab before || (47)" )]
        public void Test_operators__tab_before____47()
        {
            var selector = "$[?@.a\t||@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "c": 3
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return before || (48)" )]
        public void Test_operators__return_before____48()
        {
            var selector = "$[?@.a\r||@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "c": 3
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space after || (49)" )]
        public void Test_operators__space_after____49()
        {
            var selector = "$[?@.a|| @.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "c": 3
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline after || (50)" )]
        public void Test_operators__newline_after____50()
        {
            var selector = "$[?@.a||\n@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "c": 3
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab after || (51)" )]
        public void Test_operators__tab_after____51()
        {
            var selector = "$[?@.a||\t@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "c": 3
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return after || (52)" )]
        public void Test_operators__return_after____52()
        {
            var selector = "$[?@.a||\r@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "c": 3
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space before && (53)" )]
        public void Test_operators__space_before____53()
        {
            var selector = "$[?@.a &&@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline before && (54)" )]
        public void Test_operators__newline_before____54()
        {
            var selector = "$[?@.a\n&&@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab before && (55)" )]
        public void Test_operators__tab_before____55()
        {
            var selector = "$[?@.a\t&&@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return before && (56)" )]
        public void Test_operators__return_before____56()
        {
            var selector = "$[?@.a\r&&@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space after && (57)" )]
        public void Test_operators__space_after____57()
        {
            var selector = "$[?@.a&& @.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline after && (58)" )]
        public void Test_operators__newline_after____58()
        {
            var selector = "$[?@.a&& @.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab after && (59)" )]
        public void Test_operators__tab_after____59()
        {
            var selector = "$[?@.a&& @.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return after && (60)" )]
        public void Test_operators__return_after____60()
        {
            var selector = "$[?@.a&& @.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space before == (61)" )]
        public void Test_operators__space_before____61()
        {
            var selector = "$[?@.a ==@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline before == (62)" )]
        public void Test_operators__newline_before____62()
        {
            var selector = "$[?@.a\n==@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab before == (63)" )]
        public void Test_operators__tab_before____63()
        {
            var selector = "$[?@.a\t==@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return before == (64)" )]
        public void Test_operators__return_before____64()
        {
            var selector = "$[?@.a\r==@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space after == (65)" )]
        public void Test_operators__space_after____65()
        {
            var selector = "$[?@.a== @.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline after == (66)" )]
        public void Test_operators__newline_after____66()
        {
            var selector = "$[?@.a==\n@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab after == (67)" )]
        public void Test_operators__tab_after____67()
        {
            var selector = "$[?@.a==\t@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return after == (68)" )]
        public void Test_operators__return_after____68()
        {
            var selector = "$[?@.a==\r@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space before != (69)" )]
        public void Test_operators__space_before____69()
        {
            var selector = "$[?@.a !=@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline before != (70)" )]
        public void Test_operators__newline_before____70()
        {
            var selector = "$[?@.a\n!=@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab before != (71)" )]
        public void Test_operators__tab_before____71()
        {
            var selector = "$[?@.a\t!=@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return before != (72)" )]
        public void Test_operators__return_before____72()
        {
            var selector = "$[?@.a\r!=@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space after != (73)" )]
        public void Test_operators__space_after____73()
        {
            var selector = "$[?@.a!= @.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline after != (74)" )]
        public void Test_operators__newline_after____74()
        {
            var selector = "$[?@.a!=\n@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab after != (75)" )]
        public void Test_operators__tab_after____75()
        {
            var selector = "$[?@.a!=\t@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return after != (76)" )]
        public void Test_operators__return_after____76()
        {
            var selector = "$[?@.a!=\r@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space before < (77)" )]
        public void Test_operators__space_before___77()
        {
            var selector = "$[?@.a <@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline before < (78)" )]
        public void Test_operators__newline_before___78()
        {
            var selector = "$[?@.a\n<@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab before < (79)" )]
        public void Test_operators__tab_before___79()
        {
            var selector = "$[?@.a\t<@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return before < (80)" )]
        public void Test_operators__return_before___80()
        {
            var selector = "$[?@.a\r<@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space after < (81)" )]
        public void Test_operators__space_after___81()
        {
            var selector = "$[?@.a< @.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline after < (82)" )]
        public void Test_operators__newline_after___82()
        {
            var selector = "$[?@.a<\n@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab after < (83)" )]
        public void Test_operators__tab_after___83()
        {
            var selector = "$[?@.a<\t@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return after < (84)" )]
        public void Test_operators__return_after___84()
        {
            var selector = "$[?@.a<\r@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space before > (85)" )]
        public void Test_operators__space_before___85()
        {
            var selector = "$[?@.b >@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline before > (86)" )]
        public void Test_operators__newline_before___86()
        {
            var selector = "$[?@.b\n>@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab before > (87)" )]
        public void Test_operators__tab_before___87()
        {
            var selector = "$[?@.b\t>@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return before > (88)" )]
        public void Test_operators__return_before___88()
        {
            var selector = "$[?@.b\r>@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space after > (89)" )]
        public void Test_operators__space_after___89()
        {
            var selector = "$[?@.b> @.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline after > (90)" )]
        public void Test_operators__newline_after___90()
        {
            var selector = "$[?@.b>\n@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab after > (91)" )]
        public void Test_operators__tab_after___91()
        {
            var selector = "$[?@.b>\t@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return after > (92)" )]
        public void Test_operators__return_after___92()
        {
            var selector = "$[?@.b>\r@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space before <= (93)" )]
        public void Test_operators__space_before____93()
        {
            var selector = "$[?@.a <=@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline before <= (94)" )]
        public void Test_operators__newline_before____94()
        {
            var selector = "$[?@.a\n<=@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab before <= (95)" )]
        public void Test_operators__tab_before____95()
        {
            var selector = "$[?@.a\t<=@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return before <= (96)" )]
        public void Test_operators__return_before____96()
        {
            var selector = "$[?@.a\r<=@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space after <= (97)" )]
        public void Test_operators__space_after____97()
        {
            var selector = "$[?@.a<= @.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline after <= (98)" )]
        public void Test_operators__newline_after____98()
        {
            var selector = "$[?@.a<=\n@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab after <= (99)" )]
        public void Test_operators__tab_after____99()
        {
            var selector = "$[?@.a<=\t@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return after <= (100)" )]
        public void Test_operators__return_after____100()
        {
            var selector = "$[?@.a<=\r@.b]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space before >= (101)" )]
        public void Test_operators__space_before____101()
        {
            var selector = "$[?@.b >=@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline before >= (102)" )]
        public void Test_operators__newline_before____102()
        {
            var selector = "$[?@.b\n>=@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab before >= (103)" )]
        public void Test_operators__tab_before____103()
        {
            var selector = "$[?@.b\t>=@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return before >= (104)" )]
        public void Test_operators__return_before____104()
        {
            var selector = "$[?@.b\r>=@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space after >= (105)" )]
        public void Test_operators__space_after____105()
        {
            var selector = "$[?@.b>= @.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline after >= (106)" )]
        public void Test_operators__newline_after____106()
        {
            var selector = "$[?@.b>=\n@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab after >= (107)" )]
        public void Test_operators__tab_after____107()
        {
            var selector = "$[?@.b>=\t@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return after >= (108)" )]
        public void Test_operators__return_after____108()
        {
            var selector = "$[?@.b>=\r@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 2,
                    "b": 1
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "b": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space between logical not and test expression (109)" )]
        public void Test_operators__space_between_logical_not_and_test_expression_109()
        {
            var selector = "$[?! @.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "a",
                    "d": "e"
                  },
                  {
                    "d": "f"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "d": "f"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline between logical not and test expression (110)" )]
        public void Test_operators__newline_between_logical_not_and_test_expression_110()
        {
            var selector = "$[?!\n@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "a",
                    "d": "e"
                  },
                  {
                    "d": "f"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "d": "f"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab between logical not and test expression (111)" )]
        public void Test_operators__tab_between_logical_not_and_test_expression_111()
        {
            var selector = "$[?!\t@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "a",
                    "d": "e"
                  },
                  {
                    "d": "f"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "d": "f"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return between logical not and test expression (112)" )]
        public void Test_operators__return_between_logical_not_and_test_expression_112()
        {
            var selector = "$[?!\r@.a]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "a",
                    "d": "e"
                  },
                  {
                    "d": "f"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "d": "f"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, space between logical not and parenthesized expression (113)" )]
        public void Test_operators__space_between_logical_not_and_parenthesized_expression_113()
        {
            var selector = "$[?! (@.a=='b')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "a",
                    "d": "e"
                  },
                  {
                    "a": "b",
                    "d": "f"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "a",
                    "d": "e"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, newline between logical not and parenthesized expression (114)" )]
        public void Test_operators__newline_between_logical_not_and_parenthesized_expression_114()
        {
            var selector = "$[?!\n(@.a=='b')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "a",
                    "d": "e"
                  },
                  {
                    "a": "b",
                    "d": "f"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "a",
                    "d": "e"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, tab between logical not and parenthesized expression (115)" )]
        public void Test_operators__tab_between_logical_not_and_parenthesized_expression_115()
        {
            var selector = "$[?!\t(@.a=='b')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "a",
                    "d": "e"
                  },
                  {
                    "a": "b",
                    "d": "f"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "a",
                    "d": "e"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"operators, return between logical not and parenthesized expression (116)" )]
        public void Test_operators__return_between_logical_not_and_parenthesized_expression_116()
        {
            var selector = "$[?!\r(@.a=='b')]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "a": "a",
                    "d": "e"
                  },
                  {
                    "a": "b",
                    "d": "f"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "a",
                    "d": "e"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, space between root and bracket (117)" )]
        public void Test_selectors__space_between_root_and_bracket_117()
        {
            var selector = "$ ['a']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, newline between root and bracket (118)" )]
        public void Test_selectors__newline_between_root_and_bracket_118()
        {
            var selector = "$\n['a']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, tab between root and bracket (119)" )]
        public void Test_selectors__tab_between_root_and_bracket_119()
        {
            var selector = "$\t['a']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, return between root and bracket (120)" )]
        public void Test_selectors__return_between_root_and_bracket_120()
        {
            var selector = "$\r['a']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, space between bracket and bracket (121)" )]
        public void Test_selectors__space_between_bracket_and_bracket_121()
        {
            var selector = "$['a'] ['b']";
            var document = JsonNode.Parse(
                """
                {
                  "a": {
                    "b": "ab"
                  }
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, newline between root and bracket (122)" )]
        public void Test_selectors__newline_between_root_and_bracket_122()
        {
            var selector = "$['a'] \n['b']";
            var document = JsonNode.Parse(
                """
                {
                  "a": {
                    "b": "ab"
                  }
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, tab between root and bracket (123)" )]
        public void Test_selectors__tab_between_root_and_bracket_123()
        {
            var selector = "$['a'] \t['b']";
            var document = JsonNode.Parse(
                """
                {
                  "a": {
                    "b": "ab"
                  }
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, return between root and bracket (124)" )]
        public void Test_selectors__return_between_root_and_bracket_124()
        {
            var selector = "$['a'] \r['b']";
            var document = JsonNode.Parse(
                """
                {
                  "a": {
                    "b": "ab"
                  }
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, space between root and dot (125)" )]
        public void Test_selectors__space_between_root_and_dot_125()
        {
            var selector = "$ .a";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, newline between root and dot (126)" )]
        public void Test_selectors__newline_between_root_and_dot_126()
        {
            var selector = "$\n.a";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, tab between root and dot (127)" )]
        public void Test_selectors__tab_between_root_and_dot_127()
        {
            var selector = "$\t.a";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, return between root and dot (128)" )]
        public void Test_selectors__return_between_root_and_dot_128()
        {
            var selector = "$\r.a";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, space between dot and name (129)" )]
        public void Test_selectors__space_between_dot_and_name_129()
        {
            var selector = "$. a";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"selectors, newline between dot and name (130)" )]
        public void Test_selectors__newline_between_dot_and_name_130()
        {
            var selector = "$.\na";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"selectors, tab between dot and name (131)" )]
        public void Test_selectors__tab_between_dot_and_name_131()
        {
            var selector = "$.\ta";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"selectors, return between dot and name (132)" )]
        public void Test_selectors__return_between_dot_and_name_132()
        {
            var selector = "$.\ra";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"selectors, space between recursive descent and name (133)" )]
        public void Test_selectors__space_between_recursive_descent_and_name_133()
        {
            var selector = "$.. a";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"selectors, newline between recursive descent and name (134)" )]
        public void Test_selectors__newline_between_recursive_descent_and_name_134()
        {
            var selector = "$..\na";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"selectors, tab between recursive descent and name (135)" )]
        public void Test_selectors__tab_between_recursive_descent_and_name_135()
        {
            var selector = "$..\ta";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"selectors, return between recursive descent and name (136)" )]
        public void Test_selectors__return_between_recursive_descent_and_name_136()
        {
            var selector = "$..\ra";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }

        [TestMethod( @"selectors, space between bracket and selector (137)" )]
        public void Test_selectors__space_between_bracket_and_selector_137()
        {
            var selector = "$[ 'a']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, newline between bracket and selector (138)" )]
        public void Test_selectors__newline_between_bracket_and_selector_138()
        {
            var selector = "$[\n'a']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, tab between bracket and selector (139)" )]
        public void Test_selectors__tab_between_bracket_and_selector_139()
        {
            var selector = "$[\t'a']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, return between bracket and selector (140)" )]
        public void Test_selectors__return_between_bracket_and_selector_140()
        {
            var selector = "$[\r'a']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, space between selector and bracket (141)" )]
        public void Test_selectors__space_between_selector_and_bracket_141()
        {
            var selector = "$['a' ]";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, newline between selector and bracket (142)" )]
        public void Test_selectors__newline_between_selector_and_bracket_142()
        {
            var selector = "$['a'\n]";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, tab between selector and bracket (143)" )]
        public void Test_selectors__tab_between_selector_and_bracket_143()
        {
            var selector = "$['a'\t]";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, return between selector and bracket (144)" )]
        public void Test_selectors__return_between_selector_and_bracket_144()
        {
            var selector = "$['a'\r]";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, space between selector and comma (145)" )]
        public void Test_selectors__space_between_selector_and_comma_145()
        {
            var selector = "$['a' ,'b']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab",
                  "b": "bc"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab",
                  "bc"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, newline between selector and comma (146)" )]
        public void Test_selectors__newline_between_selector_and_comma_146()
        {
            var selector = "$['a'\n,'b']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab",
                  "b": "bc"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab",
                  "bc"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, tab between selector and comma (147)" )]
        public void Test_selectors__tab_between_selector_and_comma_147()
        {
            var selector = "$['a'\t,'b']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab",
                  "b": "bc"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab",
                  "bc"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, return between selector and comma (148)" )]
        public void Test_selectors__return_between_selector_and_comma_148()
        {
            var selector = "$['a'\r,'b']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab",
                  "b": "bc"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab",
                  "bc"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, space between comma and selector (149)" )]
        public void Test_selectors__space_between_comma_and_selector_149()
        {
            var selector = "$['a', 'b']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab",
                  "b": "bc"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab",
                  "bc"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, newline between comma and selector (150)" )]
        public void Test_selectors__newline_between_comma_and_selector_150()
        {
            var selector = "$['a',\n'b']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab",
                  "b": "bc"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab",
                  "bc"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, tab between comma and selector (151)" )]
        public void Test_selectors__tab_between_comma_and_selector_151()
        {
            var selector = "$['a',\t'b']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab",
                  "b": "bc"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab",
                  "bc"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"selectors, return between comma and selector (152)" )]
        public void Test_selectors__return_between_comma_and_selector_152()
        {
            var selector = "$['a',\r'b']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "ab",
                  "b": "bc"
                }
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  "ab",
                  "bc"
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, space between start and colon (153)" )]
        public void Test_slice__space_between_start_and_colon_153()
        {
            var selector = "$[1 :5:2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, newline between start and colon (154)" )]
        public void Test_slice__newline_between_start_and_colon_154()
        {
            var selector = "$[1\n:5:2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, tab between start and colon (155)" )]
        public void Test_slice__tab_between_start_and_colon_155()
        {
            var selector = "$[1\t:5:2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, return between start and colon (156)" )]
        public void Test_slice__return_between_start_and_colon_156()
        {
            var selector = "$[1\r:5:2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, space between colon and end (157)" )]
        public void Test_slice__space_between_colon_and_end_157()
        {
            var selector = "$[1: 5:2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, newline between colon and end (158)" )]
        public void Test_slice__newline_between_colon_and_end_158()
        {
            var selector = "$[1:\n5:2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, tab between colon and end (159)" )]
        public void Test_slice__tab_between_colon_and_end_159()
        {
            var selector = "$[1:\t5:2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, return between colon and end (160)" )]
        public void Test_slice__return_between_colon_and_end_160()
        {
            var selector = "$[1:\r5:2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, space between end and colon (161)" )]
        public void Test_slice__space_between_end_and_colon_161()
        {
            var selector = "$[1:5 :2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, newline between end and colon (162)" )]
        public void Test_slice__newline_between_end_and_colon_162()
        {
            var selector = "$[1:5\n:2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, tab between end and colon (163)" )]
        public void Test_slice__tab_between_end_and_colon_163()
        {
            var selector = "$[1:5\t:2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, return between end and colon (164)" )]
        public void Test_slice__return_between_end_and_colon_164()
        {
            var selector = "$[1:5\r:2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, space between colon and step (165)" )]
        public void Test_slice__space_between_colon_and_step_165()
        {
            var selector = "$[1:5: 2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, newline between colon and step (166)" )]
        public void Test_slice__newline_between_colon_and_step_166()
        {
            var selector = "$[1:5:\n2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, tab between colon and step (167)" )]
        public void Test_slice__tab_between_colon_and_step_167()
        {
            var selector = "$[1:5:\t2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }

        [TestMethod( @"slice, return between colon and step (168)" )]
        public void Test_slice__return_between_colon_and_step_168()
        {
            var selector = "$[1:5:\r2]";
            var document = JsonNode.Parse(
                """
                [
                  1,
                  2,
                  3,
                  4,
                  5,
                  6
                ]
                """ );
            var results = document.Select( selector );
            var expect = JsonNode.Parse(
                """
                [
                  2,
                  4
                ]
                """ );

            var match = TestHelper.MatchOne( results, expect! );
            Assert.IsTrue( match );
        }
    }
}

