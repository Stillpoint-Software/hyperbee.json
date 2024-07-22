// This file was auto generated.

using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Cts.TestSupport;

namespace Hyperbee.Json.Cts.Tests;

[TestClass]
public class CtsWhitespaceTest
{
    [DataTestMethod( @"filter, space between question mark and expression (1)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__space_between_question_mark_and_expression_1( Type documentType )
    {
        const string selector = "$[? @.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"filter, newline between question mark and expression (2)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__newline_between_question_mark_and_expression_2( Type documentType )
    {
        const string selector = "$[?\n@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"filter, tab between question mark and expression (3)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__tab_between_question_mark_and_expression_3( Type documentType )
    {
        const string selector = "$[?\t@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"filter, return between question mark and expression (4)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__return_between_question_mark_and_expression_4( Type documentType )
    {
        const string selector = "$[?\r@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"filter, space between question mark and parenthesized expression (5)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__space_between_question_mark_and_parenthesized_expression_5( Type documentType )
    {
        const string selector = "$[? (@.a)]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"filter, newline between question mark and parenthesized expression (6)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__newline_between_question_mark_and_parenthesized_expression_6( Type documentType )
    {
        const string selector = "$[?\n(@.a)]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"filter, tab between question mark and parenthesized expression (7)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__tab_between_question_mark_and_parenthesized_expression_7( Type documentType )
    {
        const string selector = "$[?\t(@.a)]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"filter, return between question mark and parenthesized expression (8)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__return_between_question_mark_and_parenthesized_expression_8( Type documentType )
    {
        const string selector = "$[?\r(@.a)]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"filter, space between parenthesized expression and bracket (9)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__space_between_parenthesized_expression_and_bracket_9( Type documentType )
    {
        const string selector = "$[?(@.a) ]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"filter, newline between parenthesized expression and bracket (10)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__newline_between_parenthesized_expression_and_bracket_10( Type documentType )
    {
        const string selector = "$[?(@.a)\n]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"filter, tab between parenthesized expression and bracket (11)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__tab_between_parenthesized_expression_and_bracket_11( Type documentType )
    {
        const string selector = "$[?(@.a)\t]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"filter, return between parenthesized expression and bracket (12)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__return_between_parenthesized_expression_and_bracket_12( Type documentType )
    {
        const string selector = "$[?(@.a)\r]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"filter, space between bracket and question mark (13)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__space_between_bracket_and_question_mark_13( Type documentType )
    {
        const string selector = "$[ ?@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"filter, newline between bracket and question mark (14)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__newline_between_bracket_and_question_mark_14( Type documentType )
    {
        const string selector = "$[\n?@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"filter, tab between bracket and question mark (15)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__tab_between_bracket_and_question_mark_15( Type documentType )
    {
        const string selector = "$[\t?@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"filter, return between bracket and question mark (16)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_filter__return_between_bracket_and_question_mark_16( Type documentType )
    {
        const string selector = "$[\r?@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, space between function name and parenthesis (17)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__space_between_function_name_and_parenthesis_17( Type documentType )
    {
        const string selector = "$[?count (@.*)==1]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"functions, newline between function name and parenthesis (18)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__newline_between_function_name_and_parenthesis_18( Type documentType )
    {
        const string selector = "$[?count\n(@.*)==1]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"functions, tab between function name and parenthesis (19)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__tab_between_function_name_and_parenthesis_19( Type documentType )
    {
        const string selector = "$[?count\t(@.*)==1]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"functions, return between function name and parenthesis (20)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__return_between_function_name_and_parenthesis_20( Type documentType )
    {
        const string selector = "$[?count\r(@.*)==1]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"functions, space between parenthesis and arg (21)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__space_between_parenthesis_and_arg_21( Type documentType )
    {
        const string selector = "$[?count( @.*)==1]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, newline between parenthesis and arg (22)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__newline_between_parenthesis_and_arg_22( Type documentType )
    {
        const string selector = "$[?count(\n@.*)==1]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, tab between parenthesis and arg (23)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__tab_between_parenthesis_and_arg_23( Type documentType )
    {
        const string selector = "$[?count(\t@.*)==1]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, return between parenthesis and arg (24)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__return_between_parenthesis_and_arg_24( Type documentType )
    {
        const string selector = "$[?count(\r@.*)==1]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, space between arg and comma (25)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__space_between_arg_and_comma_25( Type documentType )
    {
        const string selector = "$[?search(@ ,'[a-z]+')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "foo",
                  "123"
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "foo"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, newline between arg and comma (26)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__newline_between_arg_and_comma_26( Type documentType )
    {
        const string selector = "$[?search(@\n,'[a-z]+')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "foo",
                  "123"
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "foo"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, tab between arg and comma (27)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__tab_between_arg_and_comma_27( Type documentType )
    {
        const string selector = "$[?search(@\t,'[a-z]+')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "foo",
                  "123"
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "foo"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, return between arg and comma (28)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__return_between_arg_and_comma_28( Type documentType )
    {
        const string selector = "$[?search(@\r,'[a-z]+')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "foo",
                  "123"
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "foo"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, space between comma and arg (29)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__space_between_comma_and_arg_29( Type documentType )
    {
        const string selector = "$[?search(@, '[a-z]+')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "foo",
                  "123"
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "foo"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, newline between comma and arg (30)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__newline_between_comma_and_arg_30( Type documentType )
    {
        const string selector = "$[?search(@,\n'[a-z]+')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "foo",
                  "123"
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "foo"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, tab between comma and arg (31)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__tab_between_comma_and_arg_31( Type documentType )
    {
        const string selector = "$[?search(@,\t'[a-z]+')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "foo",
                  "123"
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "foo"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, return between comma and arg (32)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__return_between_comma_and_arg_32( Type documentType )
    {
        const string selector = "$[?search(@,\r'[a-z]+')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "foo",
                  "123"
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "foo"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, space between arg and parenthesis (33)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__space_between_arg_and_parenthesis_33( Type documentType )
    {
        const string selector = "$[?count(@.* )==1]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, newline between arg and parenthesis (34)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__newline_between_arg_and_parenthesis_34( Type documentType )
    {
        const string selector = "$[?count(@.*\n)==1]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, tab between arg and parenthesis (35)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__tab_between_arg_and_parenthesis_35( Type documentType )
    {
        const string selector = "$[?count(@.*\t)==1]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, return between arg and parenthesis (36)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__return_between_arg_and_parenthesis_36( Type documentType )
    {
        const string selector = "$[?count(@.*\r)==1]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, spaces in a relative singular selector (37)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__spaces_in_a_relative_singular_selector_37( Type documentType )
    {
        const string selector = "$[?length(@ .a .b) == 3]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": {
                      "b": "foo"
                    }
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, newlines in a relative singular selector (38)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__newlines_in_a_relative_singular_selector_38( Type documentType )
    {
        const string selector = "$[?length(@\n.a\n.b) == 3]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": {
                      "b": "foo"
                    }
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, tabs in a relative singular selector (39)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__tabs_in_a_relative_singular_selector_39( Type documentType )
    {
        const string selector = "$[?length(@\t.a\t.b) == 3]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": {
                      "b": "foo"
                    }
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, returns in a relative singular selector (40)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__returns_in_a_relative_singular_selector_40( Type documentType )
    {
        const string selector = "$[?length(@\r.a\r.b) == 3]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": {
                      "b": "foo"
                    }
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, spaces in an absolute singular selector (41)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__spaces_in_an_absolute_singular_selector_41( Type documentType )
    {
        const string selector = "$..[?length(@)==length($ [0] .a)]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "foo"
                  },
                  {}
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "foo"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, newlines in an absolute singular selector (42)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__newlines_in_an_absolute_singular_selector_42( Type documentType )
    {
        const string selector = "$..[?length(@)==length($\n[0]\n.a)]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "foo"
                  },
                  {}
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "foo"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, tabs in an absolute singular selector (43)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__tabs_in_an_absolute_singular_selector_43( Type documentType )
    {
        const string selector = "$..[?length(@)==length($\t[0]\t.a)]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "foo"
                  },
                  {}
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "foo"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"functions, returns in an absolute singular selector (44)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_functions__returns_in_an_absolute_singular_selector_44( Type documentType )
    {
        const string selector = "$..[?length(@)==length($\r[0]\r.a)]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "foo"
                  },
                  {}
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "foo"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space before || (45)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_before____45( Type documentType )
    {
        const string selector = "$[?@.a ||@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline before || (46)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_before____46( Type documentType )
    {
        const string selector = "$[?@.a\n||@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab before || (47)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_before____47( Type documentType )
    {
        const string selector = "$[?@.a\t||@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return before || (48)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_before____48( Type documentType )
    {
        const string selector = "$[?@.a\r||@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space after || (49)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_after____49( Type documentType )
    {
        const string selector = "$[?@.a|| @.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline after || (50)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_after____50( Type documentType )
    {
        const string selector = "$[?@.a||\n@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab after || (51)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_after____51( Type documentType )
    {
        const string selector = "$[?@.a||\t@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return after || (52)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_after____52( Type documentType )
    {
        const string selector = "$[?@.a||\r@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space before && (53)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_before____53( Type documentType )
    {
        const string selector = "$[?@.a &&@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline before && (54)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_before____54( Type documentType )
    {
        const string selector = "$[?@.a\n&&@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab before && (55)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_before____55( Type documentType )
    {
        const string selector = "$[?@.a\t&&@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return before && (56)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_before____56( Type documentType )
    {
        const string selector = "$[?@.a\r&&@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space after && (57)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_after____57( Type documentType )
    {
        const string selector = "$[?@.a&& @.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline after && (58)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_after____58( Type documentType )
    {
        const string selector = "$[?@.a&& @.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab after && (59)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_after____59( Type documentType )
    {
        const string selector = "$[?@.a&& @.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return after && (60)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_after____60( Type documentType )
    {
        const string selector = "$[?@.a&& @.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space before == (61)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_before____61( Type documentType )
    {
        const string selector = "$[?@.a ==@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline before == (62)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_before____62( Type documentType )
    {
        const string selector = "$[?@.a\n==@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab before == (63)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_before____63( Type documentType )
    {
        const string selector = "$[?@.a\t==@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return before == (64)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_before____64( Type documentType )
    {
        const string selector = "$[?@.a\r==@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space after == (65)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_after____65( Type documentType )
    {
        const string selector = "$[?@.a== @.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline after == (66)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_after____66( Type documentType )
    {
        const string selector = "$[?@.a==\n@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab after == (67)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_after____67( Type documentType )
    {
        const string selector = "$[?@.a==\t@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return after == (68)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_after____68( Type documentType )
    {
        const string selector = "$[?@.a==\r@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 1
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space before != (69)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_before____69( Type documentType )
    {
        const string selector = "$[?@.a !=@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline before != (70)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_before____70( Type documentType )
    {
        const string selector = "$[?@.a\n!=@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab before != (71)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_before____71( Type documentType )
    {
        const string selector = "$[?@.a\t!=@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return before != (72)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_before____72( Type documentType )
    {
        const string selector = "$[?@.a\r!=@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space after != (73)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_after____73( Type documentType )
    {
        const string selector = "$[?@.a!= @.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline after != (74)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_after____74( Type documentType )
    {
        const string selector = "$[?@.a!=\n@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab after != (75)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_after____75( Type documentType )
    {
        const string selector = "$[?@.a!=\t@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return after != (76)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_after____76( Type documentType )
    {
        const string selector = "$[?@.a!=\r@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space before < (77)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_before___77( Type documentType )
    {
        const string selector = "$[?@.a <@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline before < (78)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_before___78( Type documentType )
    {
        const string selector = "$[?@.a\n<@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab before < (79)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_before___79( Type documentType )
    {
        const string selector = "$[?@.a\t<@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return before < (80)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_before___80( Type documentType )
    {
        const string selector = "$[?@.a\r<@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space after < (81)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_after___81( Type documentType )
    {
        const string selector = "$[?@.a< @.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline after < (82)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_after___82( Type documentType )
    {
        const string selector = "$[?@.a<\n@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab after < (83)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_after___83( Type documentType )
    {
        const string selector = "$[?@.a<\t@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return after < (84)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_after___84( Type documentType )
    {
        const string selector = "$[?@.a<\r@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space before > (85)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_before___85( Type documentType )
    {
        const string selector = "$[?@.b >@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline before > (86)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_before___86( Type documentType )
    {
        const string selector = "$[?@.b\n>@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab before > (87)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_before___87( Type documentType )
    {
        const string selector = "$[?@.b\t>@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return before > (88)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_before___88( Type documentType )
    {
        const string selector = "$[?@.b\r>@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space after > (89)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_after___89( Type documentType )
    {
        const string selector = "$[?@.b> @.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline after > (90)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_after___90( Type documentType )
    {
        const string selector = "$[?@.b>\n@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab after > (91)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_after___91( Type documentType )
    {
        const string selector = "$[?@.b>\t@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return after > (92)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_after___92( Type documentType )
    {
        const string selector = "$[?@.b>\r@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space before <= (93)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_before____93( Type documentType )
    {
        const string selector = "$[?@.a <=@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline before <= (94)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_before____94( Type documentType )
    {
        const string selector = "$[?@.a\n<=@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab before <= (95)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_before____95( Type documentType )
    {
        const string selector = "$[?@.a\t<=@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return before <= (96)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_before____96( Type documentType )
    {
        const string selector = "$[?@.a\r<=@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space after <= (97)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_after____97( Type documentType )
    {
        const string selector = "$[?@.a<= @.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline after <= (98)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_after____98( Type documentType )
    {
        const string selector = "$[?@.a<=\n@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab after <= (99)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_after____99( Type documentType )
    {
        const string selector = "$[?@.a<=\t@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return after <= (100)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_after____100( Type documentType )
    {
        const string selector = "$[?@.a<=\r@.b]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space before >= (101)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_before____101( Type documentType )
    {
        const string selector = "$[?@.b >=@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline before >= (102)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_before____102( Type documentType )
    {
        const string selector = "$[?@.b\n>=@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab before >= (103)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_before____103( Type documentType )
    {
        const string selector = "$[?@.b\t>=@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return before >= (104)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_before____104( Type documentType )
    {
        const string selector = "$[?@.b\r>=@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space after >= (105)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_after____105( Type documentType )
    {
        const string selector = "$[?@.b>= @.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline after >= (106)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_after____106( Type documentType )
    {
        const string selector = "$[?@.b>=\n@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab after >= (107)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_after____107( Type documentType )
    {
        const string selector = "$[?@.b>=\t@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return after >= (108)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_after____108( Type documentType )
    {
        const string selector = "$[?@.b>=\r@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space between logical not and test expression (109)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_between_logical_not_and_test_expression_109( Type documentType )
    {
        const string selector = "$[?! @.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "d": "f"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline between logical not and test expression (110)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_between_logical_not_and_test_expression_110( Type documentType )
    {
        const string selector = "$[?!\n@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "d": "f"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab between logical not and test expression (111)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_between_logical_not_and_test_expression_111( Type documentType )
    {
        const string selector = "$[?!\t@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "d": "f"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return between logical not and test expression (112)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_between_logical_not_and_test_expression_112( Type documentType )
    {
        const string selector = "$[?!\r@.a]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "d": "f"
                  }
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, space between logical not and parenthesized expression (113)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__space_between_logical_not_and_parenthesized_expression_113( Type documentType )
    {
        const string selector = "$[?! (@.a=='b')]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, newline between logical not and parenthesized expression (114)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__newline_between_logical_not_and_parenthesized_expression_114( Type documentType )
    {
        const string selector = "$[?!\n(@.a=='b')]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, tab between logical not and parenthesized expression (115)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__tab_between_logical_not_and_parenthesized_expression_115( Type documentType )
    {
        const string selector = "$[?!\t(@.a=='b')]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"operators, return between logical not and parenthesized expression (116)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_operators__return_between_logical_not_and_parenthesized_expression_116( Type documentType )
    {
        const string selector = "$[?!\r(@.a=='b')]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
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
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, space between root and bracket (117)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__space_between_root_and_bracket_117( Type documentType )
    {
        const string selector = "$ ['a']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, newline between root and bracket (118)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__newline_between_root_and_bracket_118( Type documentType )
    {
        const string selector = "$\n['a']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, tab between root and bracket (119)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__tab_between_root_and_bracket_119( Type documentType )
    {
        const string selector = "$\t['a']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, return between root and bracket (120)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__return_between_root_and_bracket_120( Type documentType )
    {
        const string selector = "$\r['a']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, space between bracket and bracket (121)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__space_between_bracket_and_bracket_121( Type documentType )
    {
        const string selector = "$['a'] ['b']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": {
                    "b": "ab"
                  }
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, newline between root and bracket (122)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__newline_between_root_and_bracket_122( Type documentType )
    {
        const string selector = "$['a'] \n['b']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": {
                    "b": "ab"
                  }
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, tab between root and bracket (123)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__tab_between_root_and_bracket_123( Type documentType )
    {
        const string selector = "$['a'] \t['b']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": {
                    "b": "ab"
                  }
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, return between root and bracket (124)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__return_between_root_and_bracket_124( Type documentType )
    {
        const string selector = "$['a'] \r['b']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": {
                    "b": "ab"
                  }
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, space between root and dot (125)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__space_between_root_and_dot_125( Type documentType )
    {
        const string selector = "$ .a";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, newline between root and dot (126)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__newline_between_root_and_dot_126( Type documentType )
    {
        const string selector = "$\n.a";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, tab between root and dot (127)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__tab_between_root_and_dot_127( Type documentType )
    {
        const string selector = "$\t.a";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, return between root and dot (128)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__return_between_root_and_dot_128( Type documentType )
    {
        const string selector = "$\r.a";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, space between dot and name (129)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__space_between_dot_and_name_129( Type documentType )
    {
        const string selector = "$. a";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"selectors, newline between dot and name (130)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__newline_between_dot_and_name_130( Type documentType )
    {
        const string selector = "$.\na";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"selectors, tab between dot and name (131)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__tab_between_dot_and_name_131( Type documentType )
    {
        const string selector = "$.\ta";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"selectors, return between dot and name (132)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__return_between_dot_and_name_132( Type documentType )
    {
        const string selector = "$.\ra";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"selectors, space between recursive descent and name (133)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__space_between_recursive_descent_and_name_133( Type documentType )
    {
        const string selector = "$.. a";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"selectors, newline between recursive descent and name (134)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__newline_between_recursive_descent_and_name_134( Type documentType )
    {
        const string selector = "$..\na";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"selectors, tab between recursive descent and name (135)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__tab_between_recursive_descent_and_name_135( Type documentType )
    {
        const string selector = "$..\ta";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"selectors, return between recursive descent and name (136)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__return_between_recursive_descent_and_name_136( Type documentType )
    {
        const string selector = "$..\ra";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"selectors, space between bracket and selector (137)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__space_between_bracket_and_selector_137( Type documentType )
    {
        const string selector = "$[ 'a']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, newline between bracket and selector (138)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__newline_between_bracket_and_selector_138( Type documentType )
    {
        const string selector = "$[\n'a']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, tab between bracket and selector (139)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__tab_between_bracket_and_selector_139( Type documentType )
    {
        const string selector = "$[\t'a']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, return between bracket and selector (140)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__return_between_bracket_and_selector_140( Type documentType )
    {
        const string selector = "$[\r'a']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, space between selector and bracket (141)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__space_between_selector_and_bracket_141( Type documentType )
    {
        const string selector = "$['a' ]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, newline between selector and bracket (142)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__newline_between_selector_and_bracket_142( Type documentType )
    {
        const string selector = "$['a'\n]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, tab between selector and bracket (143)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__tab_between_selector_and_bracket_143( Type documentType )
    {
        const string selector = "$['a'\t]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, return between selector and bracket (144)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__return_between_selector_and_bracket_144( Type documentType )
    {
        const string selector = "$['a'\r]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, space between selector and comma (145)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__space_between_selector_and_comma_145( Type documentType )
    {
        const string selector = "$['a' ,'b']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab",
                  "b": "bc"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab",
                  "bc"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, newline between selector and comma (146)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__newline_between_selector_and_comma_146( Type documentType )
    {
        const string selector = "$['a'\n,'b']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab",
                  "b": "bc"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab",
                  "bc"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, tab between selector and comma (147)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__tab_between_selector_and_comma_147( Type documentType )
    {
        const string selector = "$['a'\t,'b']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab",
                  "b": "bc"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab",
                  "bc"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, return between selector and comma (148)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__return_between_selector_and_comma_148( Type documentType )
    {
        const string selector = "$['a'\r,'b']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab",
                  "b": "bc"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab",
                  "bc"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, space between comma and selector (149)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__space_between_comma_and_selector_149( Type documentType )
    {
        const string selector = "$['a', 'b']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab",
                  "b": "bc"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab",
                  "bc"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, newline between comma and selector (150)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__newline_between_comma_and_selector_150( Type documentType )
    {
        const string selector = "$['a',\n'b']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab",
                  "b": "bc"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab",
                  "bc"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, tab between comma and selector (151)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__tab_between_comma_and_selector_151( Type documentType )
    {
        const string selector = "$['a',\t'b']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab",
                  "b": "bc"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab",
                  "bc"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"selectors, return between comma and selector (152)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_selectors__return_between_comma_and_selector_152( Type documentType )
    {
        const string selector = "$['a',\r'b']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "ab",
                  "b": "bc"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ab",
                  "bc"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, space between start and colon (153)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__space_between_start_and_colon_153( Type documentType )
    {
        const string selector = "$[1 :5:2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, newline between start and colon (154)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__newline_between_start_and_colon_154( Type documentType )
    {
        const string selector = "$[1\n:5:2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, tab between start and colon (155)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__tab_between_start_and_colon_155( Type documentType )
    {
        const string selector = "$[1\t:5:2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, return between start and colon (156)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__return_between_start_and_colon_156( Type documentType )
    {
        const string selector = "$[1\r:5:2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, space between colon and end (157)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__space_between_colon_and_end_157( Type documentType )
    {
        const string selector = "$[1: 5:2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, newline between colon and end (158)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__newline_between_colon_and_end_158( Type documentType )
    {
        const string selector = "$[1:\n5:2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, tab between colon and end (159)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__tab_between_colon_and_end_159( Type documentType )
    {
        const string selector = "$[1:\t5:2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, return between colon and end (160)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__return_between_colon_and_end_160( Type documentType )
    {
        const string selector = "$[1:\r5:2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, space between end and colon (161)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__space_between_end_and_colon_161( Type documentType )
    {
        const string selector = "$[1:5 :2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, newline between end and colon (162)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__newline_between_end_and_colon_162( Type documentType )
    {
        const string selector = "$[1:5\n:2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, tab between end and colon (163)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__tab_between_end_and_colon_163( Type documentType )
    {
        const string selector = "$[1:5\t:2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, return between end and colon (164)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__return_between_end_and_colon_164( Type documentType )
    {
        const string selector = "$[1:5\r:2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, space between colon and step (165)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__space_between_colon_and_step_165( Type documentType )
    {
        const string selector = "$[1:5: 2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, newline between colon and step (166)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__newline_between_colon_and_step_166( Type documentType )
    {
        const string selector = "$[1:5:\n2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, tab between colon and step (167)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__tab_between_colon_and_step_167( Type documentType )
    {
        const string selector = "$[1:5:\t2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"slice, return between colon and step (168)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_slice__return_between_colon_and_step_168( Type documentType )
    {
        const string selector = "$[1:5:\r2]";
        var document = TestHelper.Parse( documentType,
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
        var expect = TestHelper.Parse( documentType,
            """
                [
                  2,
                  4
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }
}


