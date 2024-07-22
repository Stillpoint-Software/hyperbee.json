// This file was auto generated.

using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Cts.TestSupport;

namespace Hyperbee.Json.Cts.Tests;

[TestClass]
public class CtsBasicTest
{
    [DataTestMethod( @"root (1)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_root_1( Type documentType )
    {
        const string selector = "$";
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
                  [
                    "first",
                    "second"
                  ]
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"no leading whitespace (2)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_no_leading_whitespace_2( Type documentType )
    {
        const string selector = " $";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"no trailing whitespace (3)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_no_trailing_whitespace_3( Type documentType )
    {
        const string selector = "$ ";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"name shorthand (4)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_name_shorthand_4( Type documentType )
    {
        const string selector = "$.a";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "A",
                  "b": "B"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"name shorthand, extended unicode ☺ (5)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_name_shorthand__extended_unicode___5( Type documentType )
    {
        const string selector = "$.☺";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "☺": "A",
                  "b": "B"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"name shorthand, underscore (6)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_name_shorthand__underscore_6( Type documentType )
    {
        const string selector = "$._";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "_": "A",
                  "_foo": "B"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "A"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"name shorthand, symbol (7)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_name_shorthand__symbol_7( Type documentType )
    {
        const string selector = "$.&";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"name shorthand, number (8)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_name_shorthand__number_8( Type documentType )
    {
        const string selector = "$.1";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"name shorthand, absent data (9)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_name_shorthand__absent_data_9( Type documentType )
    {
        const string selector = "$.c";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "A",
                  "b": "B"
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

    [DataTestMethod( @"name shorthand, array data (10)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_name_shorthand__array_data_10( Type documentType )
    {
        const string selector = "$.a";
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

    [DataTestMethod( @"wildcard shorthand, object data (11)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_wildcard_shorthand__object_data_11( Type documentType )
    {
        const string selector = "$.*";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "A",
                  "b": "B"
                }
            """ );
        var results = document.Select( selector );
        var expectOneOf = TestHelper.Parse( documentType,
            """
                [
                  [
                    "A",
                    "B"
                  ],
                  [
                    "B",
                    "A"
                  ]
                ]
            """ ).Root;

        var match = TestHelper.MatchAny( documentType, results, expectOneOf );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"wildcard shorthand, array data (12)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_wildcard_shorthand__array_data_12( Type documentType )
    {
        const string selector = "$.*";
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
                  "first",
                  "second"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"wildcard selector, array data (13)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_wildcard_selector__array_data_13( Type documentType )
    {
        const string selector = "$[*]";
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
                  "first",
                  "second"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"wildcard shorthand, then name shorthand (14)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_wildcard_shorthand__then_name_shorthand_14( Type documentType )
    {
        const string selector = "$.*.a";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "x": {
                    "a": "Ax",
                    "b": "Bx"
                  },
                  "y": {
                    "a": "Ay",
                    "b": "By"
                  }
                }
            """ );
        var results = document.Select( selector );
        var expectOneOf = TestHelper.Parse( documentType,
            """
                [
                  [
                    "Ax",
                    "Ay"
                  ],
                  [
                    "Ay",
                    "Ax"
                  ]
                ]
            """ ).Root;

        var match = TestHelper.MatchAny( documentType, results, expectOneOf );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"multiple selectors (15)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_multiple_selectors_15( Type documentType )
    {
        const string selector = "$[0,2]";
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
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  0,
                  2
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"multiple selectors, space instead of comma (16)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_multiple_selectors__space_instead_of_comma_16( Type documentType )
    {
        const string selector = "$[0 2]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"multiple selectors, name and index, array data (17)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_multiple_selectors__name_and_index__array_data_17( Type documentType )
    {
        const string selector = "$['a',1]";
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
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  1
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"multiple selectors, name and index, object data (18)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_multiple_selectors__name_and_index__object_data_18( Type documentType )
    {
        const string selector = "$['a',1]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": 1,
                  "b": 2
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  1
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"multiple selectors, index and slice (19)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_multiple_selectors__index_and_slice_19( Type documentType )
    {
        const string selector = "$[1,5:7]";
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
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  1,
                  5,
                  6
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"multiple selectors, index and slice, overlapping (20)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_multiple_selectors__index_and_slice__overlapping_20( Type documentType )
    {
        const string selector = "$[1,0:3]";
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
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  1,
                  0,
                  1,
                  2
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"multiple selectors, duplicate index (21)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_multiple_selectors__duplicate_index_21( Type documentType )
    {
        const string selector = "$[1,1]";
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
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  1,
                  1
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"multiple selectors, wildcard and index (22)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_multiple_selectors__wildcard_and_index_22( Type documentType )
    {
        const string selector = "$[*,1]";
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
            """ );
        var results = document.Select( selector );
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
                  9,
                  1
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"multiple selectors, wildcard and name (23)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_multiple_selectors__wildcard_and_name_23( Type documentType )
    {
        const string selector = "$[*,'a']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "A",
                  "b": "B"
                }
            """ );
        var results = document.Select( selector );
        var expectOneOf = TestHelper.Parse( documentType,
            """
                [
                  [
                    "A",
                    "B",
                    "A"
                  ],
                  [
                    "B",
                    "A",
                    "A"
                  ]
                ]
            """ ).Root;

        var match = TestHelper.MatchAny( documentType, results, expectOneOf );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"multiple selectors, wildcard and slice (24)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_multiple_selectors__wildcard_and_slice_24( Type documentType )
    {
        const string selector = "$[*,0:2]";
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
            """ );
        var results = document.Select( selector );
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
                  9,
                  0,
                  1
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"multiple selectors, multiple wildcards (25)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_multiple_selectors__multiple_wildcards_25( Type documentType )
    {
        const string selector = "$[*,*]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  0,
                  1,
                  2
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  0,
                  1,
                  2,
                  0,
                  1,
                  2
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"empty segment (26)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_empty_segment_26( Type documentType )
    {
        const string selector = "$[]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }

    [DataTestMethod( @"descendant segment, index (27)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_descendant_segment__index_27( Type documentType )
    {
        const string selector = "$..[1]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "o": [
                    0,
                    1,
                    [
                      2,
                      3
                    ]
                  ]
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  1,
                  3
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"descendant segment, name shorthand (28)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_descendant_segment__name_shorthand_28( Type documentType )
    {
        const string selector = "$..a";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "o": [
                    {
                      "a": "b"
                    },
                    {
                      "a": "c"
                    }
                  ]
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "b",
                  "c"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"descendant segment, wildcard shorthand, array data (29)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_descendant_segment__wildcard_shorthand__array_data_29( Type documentType )
    {
        const string selector = "$..*";
        var document = TestHelper.Parse( documentType,
            """
                [
                  0,
                  1
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  0,
                  1
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"descendant segment, wildcard selector, array data (30)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_descendant_segment__wildcard_selector__array_data_30( Type documentType )
    {
        const string selector = "$..[*]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  0,
                  1
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  0,
                  1
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"descendant segment, wildcard selector, nested arrays (31)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_descendant_segment__wildcard_selector__nested_arrays_31( Type documentType )
    {
        const string selector = "$..[*]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  [
                    [
                      1
                    ]
                  ],
                  [
                    2
                  ]
                ]
            """ );
        var results = document.Select( selector );
        var expectOneOf = TestHelper.Parse( documentType,
            """
                [
                  [
                    [
                      [
                        1
                      ]
                    ],
                    [
                      2
                    ],
                    [
                      1
                    ],
                    1,
                    2
                  ],
                  [
                    [
                      [
                        1
                      ]
                    ],
                    [
                      2
                    ],
                    [
                      1
                    ],
                    2,
                    1
                  ]
                ]
            """ ).Root;

        var match = TestHelper.MatchAny( documentType, results, expectOneOf );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"descendant segment, wildcard selector, nested objects (32)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_descendant_segment__wildcard_selector__nested_objects_32( Type documentType )
    {
        const string selector = "$..[*]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": {
                    "c": {
                      "e": 1
                    }
                  },
                  "b": {
                    "d": 2
                  }
                }
            """ );
        var results = document.Select( selector );
        var expectOneOf = TestHelper.Parse( documentType,
            """
                [
                  [
                    {
                      "c": {
                        "e": 1
                      }
                    },
                    {
                      "d": 2
                    },
                    {
                      "e": 1
                    },
                    1,
                    2
                  ],
                  [
                    {
                      "c": {
                        "e": 1
                      }
                    },
                    {
                      "d": 2
                    },
                    {
                      "e": 1
                    },
                    2,
                    1
                  ],
                  [
                    {
                      "c": {
                        "e": 1
                      }
                    },
                    {
                      "d": 2
                    },
                    2,
                    {
                      "e": 1
                    },
                    1
                  ],
                  [
                    {
                      "d": 2
                    },
                    {
                      "c": {
                        "e": 1
                      }
                    },
                    {
                      "e": 1
                    },
                    1,
                    2
                  ],
                  [
                    {
                      "d": 2
                    },
                    {
                      "c": {
                        "e": 1
                      }
                    },
                    {
                      "e": 1
                    },
                    2,
                    1
                  ],
                  [
                    {
                      "d": 2
                    },
                    {
                      "c": {
                        "e": 1
                      }
                    },
                    2,
                    {
                      "e": 1
                    },
                    1
                  ]
                ]
            """ ).Root;

        var match = TestHelper.MatchAny( documentType, results, expectOneOf );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"descendant segment, wildcard shorthand, object data (33)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_descendant_segment__wildcard_shorthand__object_data_33( Type documentType )
    {
        const string selector = "$..*";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": "b"
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "b"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"descendant segment, wildcard shorthand, nested data (34)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_descendant_segment__wildcard_shorthand__nested_data_34( Type documentType )
    {
        const string selector = "$..*";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "o": [
                    {
                      "a": "b"
                    }
                  ]
                }
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  [
                    {
                      "a": "b"
                    }
                  ],
                  {
                    "a": "b"
                  },
                  "b"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"descendant segment, multiple selectors (35)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_descendant_segment__multiple_selectors_35( Type documentType )
    {
        const string selector = "$..['a','d']";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
            """ );
        var results = document.Select( selector );
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "b",
                  "e",
                  "c",
                  "f"
                ]
            """ ).Root;

        var match = TestHelper.MatchOne( documentType, results, expect );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"descendant segment, object traversal, multiple selectors (36)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_descendant_segment__object_traversal__multiple_selectors_36( Type documentType )
    {
        const string selector = "$..['a','d']";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "x": {
                    "a": "b",
                    "d": "e"
                  },
                  "y": {
                    "a": "c",
                    "d": "f"
                  }
                }
            """ );
        var results = document.Select( selector );
        var expectOneOf = TestHelper.Parse( documentType,
            """
                [
                  [
                    "b",
                    "e",
                    "c",
                    "f"
                  ],
                  [
                    "c",
                    "f",
                    "b",
                    "e"
                  ]
                ]
            """ ).Root;

        var match = TestHelper.MatchAny( documentType, results, expectOneOf );
        Assert.IsTrue( match );
    }

    [DataTestMethod( @"bald descendant segment (37)" )]
    [DataRow( typeof( JsonNode ) )]
    [DataRow( typeof( JsonElement ) )]
    public void Test_bald_descendant_segment_37( Type documentType )
    {
        const string selector = "$..";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
}


