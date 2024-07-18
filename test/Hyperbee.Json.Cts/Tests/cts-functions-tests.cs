// This file was auto generated.

using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Cts.TestSupport;

namespace Hyperbee.Json.Cts.Tests;

[TestClass]
public class CtsFunctionsTest
{
    [DataTestMethod( @"count, count function (1)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_count__count_function_1( Type documentType )
    {
        const string selector = "$[?count(@..*)>2]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
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
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"count, single-node arg (2)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_count__single_node_arg_2( Type documentType )
    {
        const string selector = "$[?count(@.a)>1]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                []
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"count, multiple-selector arg (3)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_count__multiple_selector_arg_3( Type documentType )
    {
        const string selector = "$[?count(@['a','d'])>1]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
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
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"count, non-query arg, number (4)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_count__non_query_arg__number_4( Type documentType )
    {
        const string selector = "$[?count(1)>2]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"count, non-query arg, string (5)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_count__non_query_arg__string_5( Type documentType )
    {
        const string selector = "$[?count('string')>2]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"count, non-query arg, true (6)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_count__non_query_arg__true_6( Type documentType )
    {
        const string selector = "$[?count(true)>2]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"count, non-query arg, false (7)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_count__non_query_arg__false_7( Type documentType )
    {
        const string selector = "$[?count(false)>2]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"count, non-query arg, null (8)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_count__non_query_arg__null_8( Type documentType )
    {
        const string selector = "$[?count(null)>2]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"count, result must be compared (9)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_count__result_must_be_compared_9( Type documentType )
    {
        const string selector = "$[?count(@..*)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"count, no params (10)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_count__no_params_10( Type documentType )
    {
        const string selector = "$[?count()==1]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"count, too many params (11)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_count__too_many_params_11( Type documentType )
    {
        const string selector = "$[?count(@.a,@.b)==1]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"length, string data (12)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_length__string_data_12( Type documentType )
    {
        const string selector = "$[?length(@.a)>=2]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "ab"
                  },
                  {
                    "a": "d"
                  }
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "ab"
                  }
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"length, string data, unicode (13)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_length__string_data__unicode_13( Type documentType )
    {
        const string selector = "$[?length(@)==2]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "☺☺",
                  "жж",
                  "阿美"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"length, array data (14)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_length__array_data_14( Type documentType )
    {
        const string selector = "$[?length(@.a)>=2]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
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
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"length, missing data (15)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_length__missing_data_15( Type documentType )
    {
        const string selector = "$[?length(@.a)>=2]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "d": "f"
                  }
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

    [DataTestMethod( @"length, number arg (16)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_length__number_arg_16( Type documentType )
    {
        const string selector = "$[?length(1)>=2]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "d": "f"
                  }
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

    [DataTestMethod( @"length, true arg (17)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_length__true_arg_17( Type documentType )
    {
        const string selector = "$[?length(true)>=2]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "d": "f"
                  }
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

    [DataTestMethod( @"length, false arg (18)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_length__false_arg_18( Type documentType )
    {
        const string selector = "$[?length(false)>=2]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "d": "f"
                  }
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

    [DataTestMethod( @"length, null arg (19)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_length__null_arg_19( Type documentType )
    {
        const string selector = "$[?length(null)>=2]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "d": "f"
                  }
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

    [DataTestMethod( @"length, result must be compared (20)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_length__result_must_be_compared_20( Type documentType )
    {
        const string selector = "$[?length(@.a)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"length, no params (21)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_length__no_params_21( Type documentType )
    {
        const string selector = "$[?length()==1]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"length, too many params (22)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_length__too_many_params_22( Type documentType )
    {
        const string selector = "$[?length(@.a,@.b)==1]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"length, non-singular query arg (23)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_length__non_singular_query_arg_23( Type documentType )
    {
        const string selector = "$[?length(@.*)<3]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"length, arg is a function expression (24)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_length__arg_is_a_function_expression_24( Type documentType )
    {
        const string selector = "$.values[?length(@.a)==length(value($..c))]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "ab"
                  }
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"length, arg is special nothing (25)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_length__arg_is_special_nothing_25( Type documentType )
    {
        const string selector = "$[?length(value(@.a))>0]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "ab"
                  }
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, found match (26)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__found_match_26( Type documentType )
    {
        const string selector = "$[?match(@.a, 'a.*')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "ab"
                  }
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "ab"
                  }
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, double quotes (27)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__double_quotes_27( Type documentType )
    {
        const string selector = "$[?match(@.a, \"a.*\")]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "ab"
                  }
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "ab"
                  }
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, regex from the document (28)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__regex_from_the_document_28( Type documentType )
    {
        const string selector = "$.values[?match(@, $.regex)]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "bab"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, don't select match (29)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__don_t_select_match_29( Type documentType )
    {
        const string selector = "$[?!match(@.a, 'a.*')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "ab"
                  }
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

    [DataTestMethod( @"match, not a match (30)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__not_a_match_30( Type documentType )
    {
        const string selector = "$[?match(@.a, 'a.*')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "bc"
                  }
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

    [DataTestMethod( @"match, select non-match (31)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__select_non_match_31( Type documentType )
    {
        const string selector = "$[?!match(@.a, 'a.*')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "bc"
                  }
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "bc"
                  }
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, non-string first arg (32)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__non_string_first_arg_32( Type documentType )
    {
        const string selector = "$[?match(1, 'a.*')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "bc"
                  }
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

    [DataTestMethod( @"match, non-string second arg (33)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__non_string_second_arg_33( Type documentType )
    {
        const string selector = "$[?match(@.a, 1)]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "bc"
                  }
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

    [DataTestMethod( @"match, filter, match function, unicode char class, uppercase (34)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__filter__match_function__unicode_char_class__uppercase_34( Type documentType )
    {
        const string selector = "$[?match(@, '\\\\p{Lu}')]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "Ж"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, filter, match function, unicode char class negated, uppercase (35)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__filter__match_function__unicode_char_class_negated__uppercase_35( Type documentType )
    {
        const string selector = "$[?match(@, '\\\\P{Lu}')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "ж",
                  "Ж",
                  "1",
                  true,
                  [],
                  {}
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ж",
                  "1"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, filter, match function, unicode, surrogate pair (36)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__filter__match_function__unicode__surrogate_pair_36( Type documentType )
    {
        const string selector = "$[?match(@, 'a.b')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "a𐄁b",
                  "ab",
                  "1",
                  true,
                  [],
                  {}
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "a𐄁b"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, dot matcher on \u2028 (37)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__dot_matcher_on__u2028_37( Type documentType )
    {
        const string selector = "$[?match(@, '.')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "\u2028",
                  "\r",
                  "\n",
                  true,
                  [],
                  {}
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "\u2028"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, dot matcher on \u2029 (38)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__dot_matcher_on__u2029_38( Type documentType )
    {
        const string selector = "$[?match(@, '.')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "\u2029",
                  "\r",
                  "\n",
                  true,
                  [],
                  {}
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "\u2029"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, result cannot be compared (39)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__result_cannot_be_compared_39( Type documentType )
    {
        const string selector = "$[?match(@.a, 'a.*')==true]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"match, too few params (40)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__too_few_params_40( Type documentType )
    {
        const string selector = "$[?match(@.a)==1]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"match, too many params (41)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__too_many_params_41( Type documentType )
    {
        const string selector = "$[?match(@.a,@.b,@.c)==1]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"match, arg is a function expression (42)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__arg_is_a_function_expression_42( Type documentType )
    {
        const string selector = "$.values[?match(@.a, value($..['regex']))]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "ab"
                  }
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, dot in character class (43)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__dot_in_character_class_43( Type documentType )
    {
        const string selector = "$[?match(@, 'a[.b]c')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "abc",
                  "a.c",
                  "axc"
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "abc",
                  "a.c"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, escaped dot (44)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__escaped_dot_44( Type documentType )
    {
        const string selector = "$[?match(@, 'a\\\\.c')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "abc",
                  "a.c",
                  "axc"
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "a.c"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, escaped backslash before dot (45)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__escaped_backslash_before_dot_45( Type documentType )
    {
        const string selector = "$[?match(@, 'a\\\\\\\\.c')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "abc",
                  "a.c",
                  "axc",
                  "a\\\u2028c"
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "a\\\u2028c"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, escaped left square bracket (46)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__escaped_left_square_bracket_46( Type documentType )
    {
        const string selector = "$[?match(@, 'a\\\\[.c')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "abc",
                  "a.c",
                  "a[\u2028c"
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "a[\u2028c"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, escaped right square bracket (47)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__escaped_right_square_bracket_47( Type documentType )
    {
        const string selector = "$[?match(@, 'a[\\\\].]c')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "abc",
                  "a.c",
                  "a\u2028c",
                  "a]c"
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "a.c",
                  "a]c"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, explicit caret (48)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__explicit_caret_48( Type documentType )
    {
        const string selector = "$[?match(@, '^ab.*')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "abc",
                  "axc",
                  "ab",
                  "xab"
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "abc",
                  "ab"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"match, explicit dollar (49)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_match__explicit_dollar_49( Type documentType )
    {
        const string selector = "$[?match(@, '.*bc$')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "abc",
                  "axc",
                  "ab",
                  "abcx"
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "abc"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, at the end (50)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__at_the_end_50( Type documentType )
    {
        const string selector = "$[?search(@.a, 'a.*')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "the end is ab"
                  }
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "the end is ab"
                  }
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, double quotes (51)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__double_quotes_51( Type documentType )
    {
        const string selector = "$[?search(@.a, \"a.*\")]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "the end is ab"
                  }
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "the end is ab"
                  }
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, at the start (52)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__at_the_start_52( Type documentType )
    {
        const string selector = "$[?search(@.a, 'a.*')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "ab is at the start"
                  }
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "ab is at the start"
                  }
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, in the middle (53)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__in_the_middle_53( Type documentType )
    {
        const string selector = "$[?search(@.a, 'a.*')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "contains two matches"
                  }
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "contains two matches"
                  }
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, regex from the document (54)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__regex_from_the_document_54( Type documentType )
    {
        const string selector = "$.values[?search(@, $.regex)]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "bab",
                  "bba",
                  "bbab"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, don't select match (55)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__don_t_select_match_55( Type documentType )
    {
        const string selector = "$[?!search(@.a, 'a.*')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "contains two matches"
                  }
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

    [DataTestMethod( @"search, not a match (56)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__not_a_match_56( Type documentType )
    {
        const string selector = "$[?search(@.a, 'a.*')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "bc"
                  }
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

    [DataTestMethod( @"search, select non-match (57)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__select_non_match_57( Type documentType )
    {
        const string selector = "$[?!search(@.a, 'a.*')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "bc"
                  }
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "bc"
                  }
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, non-string first arg (58)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__non_string_first_arg_58( Type documentType )
    {
        const string selector = "$[?search(1, 'a.*')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "bc"
                  }
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

    [DataTestMethod( @"search, non-string second arg (59)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__non_string_second_arg_59( Type documentType )
    {
        const string selector = "$[?search(@.a, 1)]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "bc"
                  }
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

    [DataTestMethod( @"search, filter, search function, unicode char class, uppercase (60)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__filter__search_function__unicode_char_class__uppercase_60( Type documentType )
    {
        const string selector = "$[?search(@, '\\\\p{Lu}')]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "Ж",
                  "жЖ"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, filter, search function, unicode char class negated, uppercase (61)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__filter__search_function__unicode_char_class_negated__uppercase_61( Type documentType )
    {
        const string selector = "$[?search(@, '\\\\P{Lu}')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "ж",
                  "Ж",
                  "1",
                  true,
                  [],
                  {}
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "ж",
                  "1"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, filter, search function, unicode, surrogate pair (62)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__filter__search_function__unicode__surrogate_pair_62( Type documentType )
    {
        const string selector = "$[?search(@, 'a.b')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "a𐄁bc",
                  "abc",
                  "1",
                  true,
                  [],
                  {}
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "a𐄁bc"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, dot matcher on \u2028 (63)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__dot_matcher_on__u2028_63( Type documentType )
    {
        const string selector = "$[?search(@, '.')]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "\u2028",
                  "\r\u2028\n"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, dot matcher on \u2029 (64)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__dot_matcher_on__u2029_64( Type documentType )
    {
        const string selector = "$[?search(@, '.')]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "\u2029",
                  "\r\u2029\n"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, result cannot be compared (65)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__result_cannot_be_compared_65( Type documentType )
    {
        const string selector = "$[?search(@.a, 'a.*')==true]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"search, too few params (66)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__too_few_params_66( Type documentType )
    {
        const string selector = "$[?search(@.a)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"search, too many params (67)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__too_many_params_67( Type documentType )
    {
        const string selector = "$[?search(@.a,@.b,@.c)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"search, arg is a function expression (68)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__arg_is_a_function_expression_68( Type documentType )
    {
        const string selector = "$.values[?search(@, value($..['regex']))]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "bab",
                  "bba",
                  "bbab"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, dot in character class (69)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__dot_in_character_class_69( Type documentType )
    {
        const string selector = "$[?search(@, 'a[.b]c')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "x abc y",
                  "x a.c y",
                  "x axc y"
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "x abc y",
                  "x a.c y"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, escaped dot (70)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__escaped_dot_70( Type documentType )
    {
        const string selector = "$[?search(@, 'a\\\\.c')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "x abc y",
                  "x a.c y",
                  "x axc y"
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "x a.c y"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, escaped backslash before dot (71)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__escaped_backslash_before_dot_71( Type documentType )
    {
        const string selector = "$[?search(@, 'a\\\\\\\\.c')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "x abc y",
                  "x a.c y",
                  "x axc y",
                  "x a\\\u2028c y"
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "x a\\\u2028c y"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, escaped left square bracket (72)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__escaped_left_square_bracket_72( Type documentType )
    {
        const string selector = "$[?search(@, 'a\\\\[.c')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "x abc y",
                  "x a.c y",
                  "x a[\u2028c y"
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "x a[\u2028c y"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"search, escaped right square bracket (73)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_search__escaped_right_square_bracket_73( Type documentType )
    {
        const string selector = "$[?search(@, 'a[\\\\].]c')]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "x abc y",
                  "x a.c y",
                  "x a\u2028c y",
                  "x a]c y"
                ]
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "x a.c y",
                  "x a]c y"
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"value, single-value nodelist (74)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_value__single_value_nodelist_74( Type documentType )
    {
        const string selector = "$[?value(@.*)==4]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  [
                    4
                  ],
                  {
                    "foo": 4
                  }
                ]
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"value, multi-value nodelist (75)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_value__multi_value_nodelist_75( Type documentType )
    {
        const string selector = "$[?value(@.*)==4]";
        var document = TestHelper.Parse( documentType,
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
                """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                []
                """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"value, too few params (76)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_value__too_few_params_76( Type documentType )
    {
        const string selector = "$[?value()==4]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"value, too many params (77)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_value__too_many_params_77( Type documentType )
    {
        const string selector = "$[?value(@.a,@.b)==4]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"value, result must be compared (78)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_value__result_must_be_compared_78( Type documentType )
    {
        const string selector = "$[?value(@.a)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
}


