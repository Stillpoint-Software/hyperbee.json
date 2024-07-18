// This file was auto generated.

using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Cts.TestSupport;

namespace Hyperbee.Json.Cts.Tests;

[TestClass]
public class CtsFilterTest
{        
    [DataTestMethod( @"existence, without segments (1)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_existence__without_segments_1( Type documentType )
    {
        const string selector = "$[?@]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": 1,
                  "b": null
                }
                """);
        var results = document.Select(selector);
        var expectOneOf = TestHelper.Parse( documentType,
            """
                [
                  [
                    1,
                    null
                  ],
                  [
                    null,
                    1
                  ]
                ]
                """).Root;

        var match = TestHelper.MatchAny(documentType, results, expectOneOf);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"existence (2)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_existence_2( Type documentType )
    {
        const string selector = "$[?@.a]";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"existence, present with null (3)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_existence__present_with_null_3( Type documentType )
    {
        const string selector = "$[?@.a]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": null,
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": null,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals string, single quotes (4)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_string__single_quotes_4( Type documentType )
    {
        const string selector = "$[?@.a=='b']";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals numeric string, single quotes (5)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_numeric_string__single_quotes_5( Type documentType )
    {
        const string selector = "$[?@.a=='1']";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "1",
                    "d": "e"
                  },
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "1",
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals string, double quotes (6)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_string__double_quotes_6( Type documentType )
    {
        const string selector = "$[?@.a==\"b\"]";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals numeric string, double quotes (7)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_numeric_string__double_quotes_7( Type documentType )
    {
        const string selector = "$[?@.a==\"1\"]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "1",
                    "d": "e"
                  },
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "1",
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals number (8)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_number_8( Type documentType )
    {
        const string selector = "$[?@.a==1]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  },
                  {
                    "a": 2,
                    "d": "f"
                  },
                  {
                    "a": "1",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals null (9)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_null_9( Type documentType )
    {
        const string selector = "$[?@.a==null]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": null,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": null,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals null, absent from data (10)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_null__absent_from_data_10( Type documentType )
    {
        const string selector = "$[?@.a==null]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
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
        
    [DataTestMethod( @"equals true (11)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_true_11( Type documentType )
    {
        const string selector = "$[?@.a==true]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": true,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": true,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals false (12)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_false_12( Type documentType )
    {
        const string selector = "$[?@.a==false]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": false,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": false,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals self (13)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_self_13( Type documentType )
    {
        const string selector = "$[?@==@]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  1,
                  null,
                  true,
                  {
                    "a": "b"
                  },
                  [
                    false
                  ]
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  1,
                  null,
                  true,
                  {
                    "a": "b"
                  },
                  [
                    false
                  ]
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"deep equality, arrays (14)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_deep_equality__arrays_14( Type documentType )
    {
        const string selector = "$[?@.a==@.b]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": false,
                    "b": [
                      1,
                      2
                    ]
                  },
                  {
                    "a": [
                      [
                        1,
                        [
                          2
                        ]
                      ]
                    ],
                    "b": [
                      [
                        1,
                        [
                          2
                        ]
                      ]
                    ]
                  },
                  {
                    "a": [
                      [
                        1,
                        [
                          2
                        ]
                      ]
                    ],
                    "b": [
                      [
                        [
                          2
                        ],
                        1
                      ]
                    ]
                  },
                  {
                    "a": [
                      [
                        1,
                        [
                          2
                        ]
                      ]
                    ],
                    "b": [
                      [
                        1,
                        2
                      ]
                    ]
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": [
                      [
                        1,
                        [
                          2
                        ]
                      ]
                    ],
                    "b": [
                      [
                        1,
                        [
                          2
                        ]
                      ]
                    ]
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"deep equality, objects (15)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_deep_equality__objects_15( Type documentType )
    {
        const string selector = "$[?@.a==@.b]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": false,
                    "b": {
                      "x": 1,
                      "y": {
                        "z": 1
                      }
                    }
                  },
                  {
                    "a": {
                      "x": 1,
                      "y": {
                        "z": 1
                      }
                    },
                    "b": {
                      "x": 1,
                      "y": {
                        "z": 1
                      }
                    }
                  },
                  {
                    "a": {
                      "x": 1,
                      "y": {
                        "z": 1
                      }
                    },
                    "b": {
                      "y": {
                        "z": 1
                      },
                      "x": 1
                    }
                  },
                  {
                    "a": {
                      "x": 1,
                      "y": {
                        "z": 1
                      }
                    },
                    "b": {
                      "x": 1
                    }
                  },
                  {
                    "a": {
                      "x": 1,
                      "y": {
                        "z": 1
                      }
                    },
                    "b": {
                      "x": 1,
                      "y": {
                        "z": 2
                      }
                    }
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": {
                      "x": 1,
                      "y": {
                        "z": 1
                      }
                    },
                    "b": {
                      "x": 1,
                      "y": {
                        "z": 1
                      }
                    }
                  },
                  {
                    "a": {
                      "x": 1,
                      "y": {
                        "z": 1
                      }
                    },
                    "b": {
                      "y": {
                        "z": 1
                      },
                      "x": 1
                    }
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"not-equals string, single quotes (16)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_not_equals_string__single_quotes_16( Type documentType )
    {
        const string selector = "$[?@.a!='b']";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"not-equals numeric string, single quotes (17)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_not_equals_numeric_string__single_quotes_17( Type documentType )
    {
        const string selector = "$[?@.a!='1']";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "1",
                    "d": "e"
                  },
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"not-equals string, single quotes, different type (18)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_not_equals_string__single_quotes__different_type_18( Type documentType )
    {
        const string selector = "$[?@.a!='b']";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"not-equals string, double quotes (19)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_not_equals_string__double_quotes_19( Type documentType )
    {
        const string selector = "$[?@.a!=\"b\"]";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"not-equals numeric string, double quotes (20)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_not_equals_numeric_string__double_quotes_20( Type documentType )
    {
        const string selector = "$[?@.a!=\"1\"]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "1",
                    "d": "e"
                  },
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"not-equals string, double quotes, different types (21)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_not_equals_string__double_quotes__different_types_21( Type documentType )
    {
        const string selector = "$[?@.a!=\"b\"]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"not-equals number (22)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_not_equals_number_22( Type documentType )
    {
        const string selector = "$[?@.a!=1]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "e"
                  },
                  {
                    "a": 2,
                    "d": "f"
                  },
                  {
                    "a": "1",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 2,
                    "d": "f"
                  },
                  {
                    "a": "1",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"not-equals number, different types (23)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_not_equals_number__different_types_23( Type documentType )
    {
        const string selector = "$[?@.a!=1]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"not-equals null (24)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_not_equals_null_24( Type documentType )
    {
        const string selector = "$[?@.a!=null]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": null,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"not-equals null, absent from data (25)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_not_equals_null__absent_from_data_25( Type documentType )
    {
        const string selector = "$[?@.a!=null]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"not-equals true (26)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_not_equals_true_26( Type documentType )
    {
        const string selector = "$[?@.a!=true]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": true,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"not-equals false (27)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_not_equals_false_27( Type documentType )
    {
        const string selector = "$[?@.a!=false]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": false,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"less than string, single quotes (28)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_less_than_string__single_quotes_28( Type documentType )
    {
        const string selector = "$[?@.a<'c']";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"less than string, double quotes (29)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_less_than_string__double_quotes_29( Type documentType )
    {
        const string selector = "$[?@.a<\"c\"]";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"less than number (30)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_less_than_number_30( Type documentType )
    {
        const string selector = "$[?@.a<10]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "e"
                  },
                  {
                    "a": 10,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  },
                  {
                    "a": 20,
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"less than null (31)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_less_than_null_31( Type documentType )
    {
        const string selector = "$[?@.a<null]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": null,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
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
        
    [DataTestMethod( @"less than true (32)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_less_than_true_32( Type documentType )
    {
        const string selector = "$[?@.a<true]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": true,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
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
        
    [DataTestMethod( @"less than false (33)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_less_than_false_33( Type documentType )
    {
        const string selector = "$[?@.a<false]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": false,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
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
        
    [DataTestMethod( @"less than or equal to string, single quotes (34)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_less_than_or_equal_to_string__single_quotes_34( Type documentType )
    {
        const string selector = "$[?@.a<='c']";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
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
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"less than or equal to string, double quotes (35)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_less_than_or_equal_to_string__double_quotes_35( Type documentType )
    {
        const string selector = "$[?@.a<=\"c\"]";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
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
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"less than or equal to number (36)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_less_than_or_equal_to_number_36( Type documentType )
    {
        const string selector = "$[?@.a<=10]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "e"
                  },
                  {
                    "a": 10,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  },
                  {
                    "a": 20,
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "e"
                  },
                  {
                    "a": 10,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"less than or equal to null (37)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_less_than_or_equal_to_null_37( Type documentType )
    {
        const string selector = "$[?@.a<=null]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": null,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": null,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"less than or equal to true (38)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_less_than_or_equal_to_true_38( Type documentType )
    {
        const string selector = "$[?@.a<=true]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": true,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": true,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"less than or equal to false (39)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_less_than_or_equal_to_false_39( Type documentType )
    {
        const string selector = "$[?@.a<=false]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": false,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": false,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"greater than string, single quotes (40)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_greater_than_string__single_quotes_40( Type documentType )
    {
        const string selector = "$[?@.a>'c']";
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
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"greater than string, double quotes (41)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_greater_than_string__double_quotes_41( Type documentType )
    {
        const string selector = "$[?@.a>\"c\"]";
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
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"greater than number (42)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_greater_than_number_42( Type documentType )
    {
        const string selector = "$[?@.a>10]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "e"
                  },
                  {
                    "a": 10,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  },
                  {
                    "a": 20,
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 20,
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"greater than null (43)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_greater_than_null_43( Type documentType )
    {
        const string selector = "$[?@.a>null]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": null,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
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
        
    [DataTestMethod( @"greater than true (44)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_greater_than_true_44( Type documentType )
    {
        const string selector = "$[?@.a>true]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": true,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
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
        
    [DataTestMethod( @"greater than false (45)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_greater_than_false_45( Type documentType )
    {
        const string selector = "$[?@.a>false]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": false,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
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
        
    [DataTestMethod( @"greater than or equal to string, single quotes (46)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_greater_than_or_equal_to_string__single_quotes_46( Type documentType )
    {
        const string selector = "$[?@.a>='c']";
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
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "c",
                    "d": "f"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"greater than or equal to string, double quotes (47)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_greater_than_or_equal_to_string__double_quotes_47( Type documentType )
    {
        const string selector = "$[?@.a>=\"c\"]";
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
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "c",
                    "d": "f"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"greater than or equal to number (48)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_greater_than_or_equal_to_number_48( Type documentType )
    {
        const string selector = "$[?@.a>=10]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "e"
                  },
                  {
                    "a": 10,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  },
                  {
                    "a": 20,
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 10,
                    "d": "e"
                  },
                  {
                    "a": 20,
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"greater than or equal to null (49)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_greater_than_or_equal_to_null_49( Type documentType )
    {
        const string selector = "$[?@.a>=null]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": null,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": null,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"greater than or equal to true (50)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_greater_than_or_equal_to_true_50( Type documentType )
    {
        const string selector = "$[?@.a>=true]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": true,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": true,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"greater than or equal to false (51)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_greater_than_or_equal_to_false_51( Type documentType )
    {
        const string selector = "$[?@.a>=false]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": false,
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": false,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"exists and not-equals null, absent from data (52)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_exists_and_not_equals_null__absent_from_data_52( Type documentType )
    {
        const string selector = "$[?@.a&&@.a!=null]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "d": "e"
                  },
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"exists and exists, data false (53)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_exists_and_exists__data_false_53( Type documentType )
    {
        const string selector = "$[?@.a&&@.b]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": false,
                    "b": false
                  },
                  {
                    "b": false
                  },
                  {
                    "c": false
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": false,
                    "b": false
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"exists or exists, data false (54)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_exists_or_exists__data_false_54( Type documentType )
    {
        const string selector = "$[?@.a||@.b]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": false,
                    "b": false
                  },
                  {
                    "b": false
                  },
                  {
                    "c": false
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": false,
                    "b": false
                  },
                  {
                    "b": false
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"and (55)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_and_55( Type documentType )
    {
        const string selector = "$[?@.a>0&&@.a<10]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": -10,
                    "d": "e"
                  },
                  {
                    "a": 5,
                    "d": "f"
                  },
                  {
                    "a": 20,
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 5,
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"or (56)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_or_56( Type documentType )
    {
        const string selector = "$[?@.a=='b'||@.a=='d']";
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
                    "a": "c",
                    "d": "f"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "f"
                  },
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"not expression (57)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_not_expression_57( Type documentType )
    {
        const string selector = "$[?!(@.a=='b')]";
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
                """);
        var results = document.Select(selector);
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
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"not exists (58)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_not_exists_58( Type documentType )
    {
        const string selector = "$[?!@.a]";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"not exists, data null (59)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_not_exists__data_null_59( Type documentType )
    {
        const string selector = "$[?!@.a]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": null,
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"non-singular existence, wildcard (60)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_non_singular_existence__wildcard_60( Type documentType )
    {
        const string selector = "$[?@.*]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  1,
                  [],
                  [
                    2
                  ],
                  {},
                  {
                    "a": 3
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  [
                    2
                  ],
                  {
                    "a": 3
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"non-singular existence, multiple (61)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_non_singular_existence__multiple_61( Type documentType )
    {
        const string selector = "$[?@[0, 0, 'a']]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  1,
                  [],
                  [
                    2
                  ],
                  [
                    2,
                    3
                  ],
                  {
                    "a": 3
                  },
                  {
                    "b": 4
                  },
                  {
                    "a": 3,
                    "b": 4
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  [
                    2
                  ],
                  [
                    2,
                    3
                  ],
                  {
                    "a": 3
                  },
                  {
                    "a": 3,
                    "b": 4
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"non-singular existence, slice (62)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_non_singular_existence__slice_62( Type documentType )
    {
        const string selector = "$[?@[0:2]]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  1,
                  [],
                  [
                    2
                  ],
                  [
                    2,
                    3,
                    4
                  ],
                  {},
                  {
                    "a": 3
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  [
                    2
                  ],
                  [
                    2,
                    3,
                    4
                  ]
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"non-singular existence, negated (63)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_non_singular_existence__negated_63( Type documentType )
    {
        const string selector = "$[?!@.*]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  1,
                  [],
                  [
                    2
                  ],
                  {},
                  {
                    "a": 3
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  1,
                  [],
                  {}
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"non-singular query in comparison, slice (64)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_non_singular_query_in_comparison__slice_64( Type documentType )
    {
        const string selector = "$[?@[0:0]==0]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"non-singular query in comparison, all children (65)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_non_singular_query_in_comparison__all_children_65( Type documentType )
    {
        const string selector = "$[?@[*]==0]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"non-singular query in comparison, descendants (66)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_non_singular_query_in_comparison__descendants_66( Type documentType )
    {
        const string selector = "$[?@..a==0]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"non-singular query in comparison, combined (67)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_non_singular_query_in_comparison__combined_67( Type documentType )
    {
        const string selector = "$[?@.a[*].a==0]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"nested (68)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_nested_68( Type documentType )
    {
        const string selector = "$[?@[?@>1]]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  [
                    0
                  ],
                  [
                    0,
                    1
                  ],
                  [
                    0,
                    1,
                    2
                  ],
                  [
                    42
                  ]
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  [
                    0,
                    1,
                    2
                  ],
                  [
                    42
                  ]
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"name segment on primitive, selects nothing (69)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_name_segment_on_primitive__selects_nothing_69( Type documentType )
    {
        const string selector = "$[?@.a == 1]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": 1
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
        
    [DataTestMethod( @"name segment on array, selects nothing (70)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_name_segment_on_array__selects_nothing_70( Type documentType )
    {
        const string selector = "$[?@['0'] == 5]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  [
                    5,
                    6
                  ]
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
        
    [DataTestMethod( @"index segment on object, selects nothing (71)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_index_segment_on_object__selects_nothing_71( Type documentType )
    {
        const string selector = "$[?@[0] == 5]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "0": 5
                  }
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
        
    [DataTestMethod( @"relative non-singular query, index, equal (72)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_relative_non_singular_query__index__equal_72( Type documentType )
    {
        const string selector = "$[?(@[0, 0]==42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"relative non-singular query, index, not equal (73)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_relative_non_singular_query__index__not_equal_73( Type documentType )
    {
        const string selector = "$[?(@[0, 0]!=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"relative non-singular query, index, less-or-equal (74)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_relative_non_singular_query__index__less_or_equal_74( Type documentType )
    {
        const string selector = "$[?(@[0, 0]<=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"relative non-singular query, name, equal (75)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_relative_non_singular_query__name__equal_75( Type documentType )
    {
        const string selector = "$[?(@['a', 'a']==42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"relative non-singular query, name, not equal (76)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_relative_non_singular_query__name__not_equal_76( Type documentType )
    {
        const string selector = "$[?(@['a', 'a']!=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"relative non-singular query, name, less-or-equal (77)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_relative_non_singular_query__name__less_or_equal_77( Type documentType )
    {
        const string selector = "$[?(@['a', 'a']<=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"relative non-singular query, combined, equal (78)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_relative_non_singular_query__combined__equal_78( Type documentType )
    {
        const string selector = "$[?(@[0, '0']==42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"relative non-singular query, combined, not equal (79)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_relative_non_singular_query__combined__not_equal_79( Type documentType )
    {
        const string selector = "$[?(@[0, '0']!=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"relative non-singular query, combined, less-or-equal (80)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_relative_non_singular_query__combined__less_or_equal_80( Type documentType )
    {
        const string selector = "$[?(@[0, '0']<=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"relative non-singular query, wildcard, equal (81)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_relative_non_singular_query__wildcard__equal_81( Type documentType )
    {
        const string selector = "$[?(@.*==42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"relative non-singular query, wildcard, not equal (82)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_relative_non_singular_query__wildcard__not_equal_82( Type documentType )
    {
        const string selector = "$[?(@.*!=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"relative non-singular query, wildcard, less-or-equal (83)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_relative_non_singular_query__wildcard__less_or_equal_83( Type documentType )
    {
        const string selector = "$[?(@.*<=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"relative non-singular query, slice, equal (84)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_relative_non_singular_query__slice__equal_84( Type documentType )
    {
        const string selector = "$[?(@[0:0]==42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"relative non-singular query, slice, not equal (85)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_relative_non_singular_query__slice__not_equal_85( Type documentType )
    {
        const string selector = "$[?(@[0:0]!=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"relative non-singular query, slice, less-or-equal (86)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_relative_non_singular_query__slice__less_or_equal_86( Type documentType )
    {
        const string selector = "$[?(@[0:0]<=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"absolute non-singular query, index, equal (87)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_absolute_non_singular_query__index__equal_87( Type documentType )
    {
        const string selector = "$[?($[0, 0]==42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"absolute non-singular query, index, not equal (88)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_absolute_non_singular_query__index__not_equal_88( Type documentType )
    {
        const string selector = "$[?($[0, 0]!=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"absolute non-singular query, index, less-or-equal (89)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_absolute_non_singular_query__index__less_or_equal_89( Type documentType )
    {
        const string selector = "$[?($[0, 0]<=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"absolute non-singular query, name, equal (90)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_absolute_non_singular_query__name__equal_90( Type documentType )
    {
        const string selector = "$[?($['a', 'a']==42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"absolute non-singular query, name, not equal (91)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_absolute_non_singular_query__name__not_equal_91( Type documentType )
    {
        const string selector = "$[?($['a', 'a']!=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"absolute non-singular query, name, less-or-equal (92)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_absolute_non_singular_query__name__less_or_equal_92( Type documentType )
    {
        const string selector = "$[?($['a', 'a']<=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"absolute non-singular query, combined, equal (93)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_absolute_non_singular_query__combined__equal_93( Type documentType )
    {
        const string selector = "$[?($[0, '0']==42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"absolute non-singular query, combined, not equal (94)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_absolute_non_singular_query__combined__not_equal_94( Type documentType )
    {
        const string selector = "$[?($[0, '0']!=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"absolute non-singular query, combined, less-or-equal (95)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_absolute_non_singular_query__combined__less_or_equal_95( Type documentType )
    {
        const string selector = "$[?($[0, '0']<=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"absolute non-singular query, wildcard, equal (96)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_absolute_non_singular_query__wildcard__equal_96( Type documentType )
    {
        const string selector = "$[?($.*==42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"absolute non-singular query, wildcard, not equal (97)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_absolute_non_singular_query__wildcard__not_equal_97( Type documentType )
    {
        const string selector = "$[?($.*!=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"absolute non-singular query, wildcard, less-or-equal (98)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_absolute_non_singular_query__wildcard__less_or_equal_98( Type documentType )
    {
        const string selector = "$[?($.*<=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"absolute non-singular query, slice, equal (99)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_absolute_non_singular_query__slice__equal_99( Type documentType )
    {
        const string selector = "$[?($[0:0]==42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"absolute non-singular query, slice, not equal (100)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_absolute_non_singular_query__slice__not_equal_100( Type documentType )
    {
        const string selector = "$[?($[0:0]!=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"absolute non-singular query, slice, less-or-equal (101)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_absolute_non_singular_query__slice__less_or_equal_101( Type documentType )
    {
        const string selector = "$[?($[0:0]<=42)]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"multiple selectors (102)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_multiple_selectors_102( Type documentType )
    {
        const string selector = "$[?@.a,?@.b]";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
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
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"multiple selectors, comparison (103)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_multiple_selectors__comparison_103( Type documentType )
    {
        const string selector = "$[?@.a=='b',?@.b=='x']";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"multiple selectors, overlapping (104)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_multiple_selectors__overlapping_104( Type documentType )
    {
        const string selector = "$[?@.a,?@.d]";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"multiple selectors, filter and index (105)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_multiple_selectors__filter_and_index_105( Type documentType )
    {
        const string selector = "$[?@.a,1]";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
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
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"multiple selectors, filter and wildcard (106)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_multiple_selectors__filter_and_wildcard_106( Type documentType )
    {
        const string selector = "$[?@.a,*]";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"multiple selectors, filter and slice (107)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_multiple_selectors__filter_and_slice_107( Type documentType )
    {
        const string selector = "$[?@.a,1:]";
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
                  },
                  {
                    "g": "h"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  },
                  {
                    "g": "h"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"multiple selectors, comparison filter, index and slice (108)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_multiple_selectors__comparison_filter__index_and_slice_108( Type documentType )
    {
        const string selector = "$[1, ?@.a=='b', 1:]";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "b": "c",
                    "d": "f"
                  },
                  {
                    "a": "b",
                    "d": "e"
                  },
                  {
                    "b": "c",
                    "d": "f"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals number, zero and negative zero (109)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_number__zero_and_negative_zero_109( Type documentType )
    {
        const string selector = "$[?@.a==-0]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 0,
                    "d": "e"
                  },
                  {
                    "a": 0.1,
                    "d": "f"
                  },
                  {
                    "a": "0",
                    "d": "g"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 0,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals number, with and without decimal fraction (110)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_number__with_and_without_decimal_fraction_110( Type documentType )
    {
        const string selector = "$[?@.a==1.0]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "e"
                  },
                  {
                    "a": 2,
                    "d": "f"
                  },
                  {
                    "a": "1",
                    "d": "g"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals number, exponent (111)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_number__exponent_111( Type documentType )
    {
        const string selector = "$[?@.a==1e2]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 100,
                    "d": "e"
                  },
                  {
                    "a": 100.1,
                    "d": "f"
                  },
                  {
                    "a": "100",
                    "d": "g"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 100,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals number, positive exponent (112)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_number__positive_exponent_112( Type documentType )
    {
        const string selector = "$[?@.a==1e+2]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 100,
                    "d": "e"
                  },
                  {
                    "a": 100.1,
                    "d": "f"
                  },
                  {
                    "a": "100",
                    "d": "g"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 100,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals number, negative exponent (113)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_number__negative_exponent_113( Type documentType )
    {
        const string selector = "$[?@.a==1e-2]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 0.01,
                    "d": "e"
                  },
                  {
                    "a": 0.02,
                    "d": "f"
                  },
                  {
                    "a": "0.01",
                    "d": "g"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 0.01,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals number, decimal fraction (114)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_number__decimal_fraction_114( Type documentType )
    {
        const string selector = "$[?@.a==1.1]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1.1,
                    "d": "e"
                  },
                  {
                    "a": 1,
                    "d": "f"
                  },
                  {
                    "a": "1.1",
                    "d": "g"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1.1,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals number, decimal fraction, no fractional digit (115)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_number__decimal_fraction__no_fractional_digit_115( Type documentType )
    {
        const string selector = "$[?@.a==1.]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"equals number, decimal fraction, exponent (116)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_number__decimal_fraction__exponent_116( Type documentType )
    {
        const string selector = "$[?@.a==1.1e2]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 110,
                    "d": "e"
                  },
                  {
                    "a": 110.1,
                    "d": "f"
                  },
                  {
                    "a": "110",
                    "d": "g"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 110,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals number, decimal fraction, positive exponent (117)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_number__decimal_fraction__positive_exponent_117( Type documentType )
    {
        const string selector = "$[?@.a==1.1e+2]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 110,
                    "d": "e"
                  },
                  {
                    "a": 110.1,
                    "d": "f"
                  },
                  {
                    "a": "110",
                    "d": "g"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 110,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals number, decimal fraction, negative exponent (118)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals_number__decimal_fraction__negative_exponent_118( Type documentType )
    {
        const string selector = "$[?@.a==1.1e-2]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 0.011,
                    "d": "e"
                  },
                  {
                    "a": 0.012,
                    "d": "f"
                  },
                  {
                    "a": "0.011",
                    "d": "g"
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 0.011,
                    "d": "e"
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals, special nothing (119)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals__special_nothing_119( Type documentType )
    {
        const string selector = "$.values[?length(@.a) == value($..c)]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "c": "cd",
                  "values": [
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
                }
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "c": "d"
                  },
                  {
                    "a": null
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals, empty node list and empty node list (120)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals__empty_node_list_and_empty_node_list_120( Type documentType )
    {
        const string selector = "$[?@.a == @.b]";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "c": 3
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"equals, empty node list and special nothing (121)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_equals__empty_node_list_and_special_nothing_121( Type documentType )
    {
        const string selector = "$[?@.a == length(@.b)]";
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
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "b": 2
                  },
                  {
                    "c": 3
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"object data (122)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_object_data_122( Type documentType )
    {
        const string selector = "$[?@<3]";
        var document = TestHelper.Parse( documentType,
            """
                {
                  "a": 1,
                  "b": 2,
                  "c": 3
                }
                """);
        var results = document.Select(selector);
        var expectOneOf = TestHelper.Parse( documentType,
            """
                [
                  [
                    1,
                    2
                  ],
                  [
                    2,
                    1
                  ]
                ]
                """).Root;

        var match = TestHelper.MatchAny(documentType, results, expectOneOf);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"and binds more tightly than or (123)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_and_binds_more_tightly_than_or_123( Type documentType )
    {
        const string selector = "$[?@.a || @.b && @.c]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2,
                    "c": 3
                  },
                  {
                    "c": 3
                  },
                  {
                    "b": 2
                  },
                  {
                    "a": 1,
                    "b": 2,
                    "c": 3
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "b": 2,
                    "c": 3
                  },
                  {
                    "a": 1,
                    "b": 2,
                    "c": 3
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"left to right evaluation (124)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_left_to_right_evaluation_124( Type documentType )
    {
        const string selector = "$[?@.a && @.b || @.c]";
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
                  },
                  {
                    "a": 1,
                    "c": 3
                  },
                  {
                    "b": 1,
                    "c": 3
                  },
                  {
                    "c": 3
                  },
                  {
                    "a": 1,
                    "b": 2,
                    "c": 3
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 1,
                    "c": 3
                  },
                  {
                    "b": 1,
                    "c": 3
                  },
                  {
                    "c": 3
                  },
                  {
                    "a": 1,
                    "b": 2,
                    "c": 3
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"group terms, left (125)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_group_terms__left_125( Type documentType )
    {
        const string selector = "$[?(@.a || @.b) && @.c]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 1,
                    "c": 3
                  },
                  {
                    "b": 2,
                    "c": 3
                  },
                  {
                    "a": 1
                  },
                  {
                    "b": 2
                  },
                  {
                    "c": 3
                  },
                  {
                    "a": 1,
                    "b": 2,
                    "c": 3
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "c": 3
                  },
                  {
                    "b": 2,
                    "c": 3
                  },
                  {
                    "a": 1,
                    "b": 2,
                    "c": 3
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"group terms, right (126)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_group_terms__right_126( Type documentType )
    {
        const string selector = "$[?@.a && (@.b || @.c)]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1
                  },
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 1,
                    "c": 2
                  },
                  {
                    "b": 2
                  },
                  {
                    "c": 2
                  },
                  {
                    "a": 1,
                    "b": 2,
                    "c": 3
                  }
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  {
                    "a": 1,
                    "b": 2
                  },
                  {
                    "a": 1,
                    "c": 2
                  },
                  {
                    "a": 1,
                    "b": 2,
                    "c": 3
                  }
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"string literal, single quote in double quotes (127)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_string_literal__single_quote_in_double_quotes_127( Type documentType )
    {
        const string selector = "$[?@ == \"quoted' literal\"]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "quoted' literal",
                  "a",
                  "quoted\\' literal"
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "quoted' literal"
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"string literal, double quote in single quotes (128)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_string_literal__double_quote_in_single_quotes_128( Type documentType )
    {
        const string selector = "$[?@ == 'quoted\" literal']";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "quoted\" literal",
                  "a",
                  "quoted\\\" literal",
                  "'quoted\" literal'"
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "quoted\" literal"
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"string literal, escaped single quote in single quotes (129)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_string_literal__escaped_single_quote_in_single_quotes_129( Type documentType )
    {
        const string selector = "$[?@ == 'quoted\\' literal']";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "quoted' literal",
                  "a",
                  "quoted\\' literal",
                  "'quoted\" literal'"
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "quoted' literal"
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"string literal, escaped double quote in double quotes (130)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_string_literal__escaped_double_quote_in_double_quotes_130( Type documentType )
    {
        const string selector = "$[?@ == \"quoted\\\" literal\"]";
        var document = TestHelper.Parse( documentType,
            """
                [
                  "quoted\" literal",
                  "a",
                  "quoted\\\" literal",
                  "'quoted\" literal'"
                ]
                """);
        var results = document.Select(selector);
        var expect = TestHelper.Parse( documentType,
            """
                [
                  "quoted\" literal"
                ]
                """).Root;

        var match = TestHelper.MatchOne(documentType, results, expect);
        Assert.IsTrue(match);
    }
        
    [DataTestMethod( @"literal true must be compared (131)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_literal_true_must_be_compared_131( Type documentType )
    {
        const string selector = "$[?true]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"literal false must be compared (132)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_literal_false_must_be_compared_132( Type documentType )
    {
        const string selector = "$[?false]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"literal string must be compared (133)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_literal_string_must_be_compared_133( Type documentType )
    {
        const string selector = "$[?'abc']";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"literal int must be compared (134)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_literal_int_must_be_compared_134( Type documentType )
    {
        const string selector = "$[?2]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"literal float must be compared (135)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_literal_float_must_be_compared_135( Type documentType )
    {
        const string selector = "$[?2.2]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"literal null must be compared (136)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_literal_null_must_be_compared_136( Type documentType )
    {
        const string selector = "$[?null]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"and, literals must be compared (137)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_and__literals_must_be_compared_137( Type documentType )
    {
        const string selector = "$[?true && false]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"or, literals must be compared (138)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_or__literals_must_be_compared_138( Type documentType )
    {
        const string selector = "$[?true || false]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"and, right hand literal must be compared (139)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_and__right_hand_literal_must_be_compared_139( Type documentType )
    {
        const string selector = "$[?true == false && false]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"or, right hand literal must be compared (140)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_or__right_hand_literal_must_be_compared_140( Type documentType )
    {
        const string selector = "$[?true == false || false]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"and, left hand literal must be compared (141)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_and__left_hand_literal_must_be_compared_141( Type documentType )
    {
        const string selector = "$[?false && true == false]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
        
    [DataTestMethod( @"or, left hand literal must be compared (142)" )]
    [DataRow( typeof(JsonNode) )]
    [DataRow(typeof(JsonElement))]
    public void Test_or__left_hand_literal_must_be_compared_142( Type documentType )
    {
        const string selector = "$[?false || true == false]";
        var document = TestHelper.Parse( documentType, "[0]" ); // Empty node

        AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
    }
}


