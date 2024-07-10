// This file was auto generated.

using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Cts.Tests
{
    [TestClass]
    public class CtsFilterTest
    {
        
        [TestMethod( "existence, without segments (1)" )]
        public void Test_existence__without_segments_1()
        {
            var selector = "$[?@]";
            var document = JsonNode.Parse(
                """
                {
                  "a": 1,
                  "b": null
                }
                """);
            var results = document.Select(selector);
            var expectOneOf = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchAny(results, expectOneOf!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "existence (2)" )]
        public void Test_existence_2()
        {
            var selector = "$[?@.a]";
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
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "existence, present with null (3)" )]
        public void Test_existence__present_with_null_3()
        {
            var selector = "$[?@.a]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": null,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals string, single quotes (4)" )]
        public void Test_equals_string__single_quotes_4()
        {
            var selector = "$[?@.a=='b']";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals numeric string, single quotes (5)" )]
        public void Test_equals_numeric_string__single_quotes_5()
        {
            var selector = "$[?@.a=='1']";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "1",
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals string, double quotes (6)" )]
        public void Test_equals_string__double_quotes_6()
        {
            var selector = "$[?@.a==\"b\"]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals numeric string, double quotes (7)" )]
        public void Test_equals_numeric_string__double_quotes_7()
        {
            var selector = "$[?@.a==\"1\"]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "1",
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals number (8)" )]
        public void Test_equals_number_8()
        {
            var selector = "$[?@.a==1]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals null (9)" )]
        public void Test_equals_null_9()
        {
            var selector = "$[?@.a==null]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": null,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals null, absent from data (10)" )]
        public void Test_equals_null__absent_from_data_10()
        {
            var selector = "$[?@.a==null]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                []
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals true (11)" )]
        public void Test_equals_true_11()
        {
            var selector = "$[?@.a==true]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": true,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals false (12)" )]
        public void Test_equals_false_12()
        {
            var selector = "$[?@.a==false]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": false,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals self (13)" )]
        public void Test_equals_self_13()
        {
            var selector = "$[?@==@]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "deep equality, arrays (14)" )]
        public void Test_deep_equality__arrays_14()
        {
            var selector = "$[?@.a==@.b]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "deep equality, objects (15)" )]
        public void Test_deep_equality__objects_15()
        {
            var selector = "$[?@.a==@.b]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "not-equals string, single quotes (16)" )]
        public void Test_not_equals_string__single_quotes_16()
        {
            var selector = "$[?@.a!='b']";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "not-equals numeric string, single quotes (17)" )]
        public void Test_not_equals_numeric_string__single_quotes_17()
        {
            var selector = "$[?@.a!='1']";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "not-equals string, single quotes, different type (18)" )]
        public void Test_not_equals_string__single_quotes__different_type_18()
        {
            var selector = "$[?@.a!='b']";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "not-equals string, double quotes (19)" )]
        public void Test_not_equals_string__double_quotes_19()
        {
            var selector = "$[?@.a!=\"b\"]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "not-equals numeric string, double quotes (20)" )]
        public void Test_not_equals_numeric_string__double_quotes_20()
        {
            var selector = "$[?@.a!=\"1\"]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "not-equals string, double quotes, different types (21)" )]
        public void Test_not_equals_string__double_quotes__different_types_21()
        {
            var selector = "$[?@.a!=\"b\"]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "not-equals number (22)" )]
        public void Test_not_equals_number_22()
        {
            var selector = "$[?@.a!=1]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "not-equals number, different types (23)" )]
        public void Test_not_equals_number__different_types_23()
        {
            var selector = "$[?@.a!=1]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "not-equals null (24)" )]
        public void Test_not_equals_null_24()
        {
            var selector = "$[?@.a!=null]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "not-equals null, absent from data (25)" )]
        public void Test_not_equals_null__absent_from_data_25()
        {
            var selector = "$[?@.a!=null]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "not-equals true (26)" )]
        public void Test_not_equals_true_26()
        {
            var selector = "$[?@.a!=true]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "not-equals false (27)" )]
        public void Test_not_equals_false_27()
        {
            var selector = "$[?@.a!=false]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "less than string, single quotes (28)" )]
        public void Test_less_than_string__single_quotes_28()
        {
            var selector = "$[?@.a<'c']";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "less than string, double quotes (29)" )]
        public void Test_less_than_string__double_quotes_29()
        {
            var selector = "$[?@.a<\"c\"]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "less than number (30)" )]
        public void Test_less_than_number_30()
        {
            var selector = "$[?@.a<10]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "less than null (31)" )]
        public void Test_less_than_null_31()
        {
            var selector = "$[?@.a<null]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                []
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "less than true (32)" )]
        public void Test_less_than_true_32()
        {
            var selector = "$[?@.a<true]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                []
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "less than false (33)" )]
        public void Test_less_than_false_33()
        {
            var selector = "$[?@.a<false]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                []
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "less than or equal to string, single quotes (34)" )]
        public void Test_less_than_or_equal_to_string__single_quotes_34()
        {
            var selector = "$[?@.a<='c']";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "less than or equal to string, double quotes (35)" )]
        public void Test_less_than_or_equal_to_string__double_quotes_35()
        {
            var selector = "$[?@.a<=\"c\"]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "less than or equal to number (36)" )]
        public void Test_less_than_or_equal_to_number_36()
        {
            var selector = "$[?@.a<=10]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "less than or equal to null (37)" )]
        public void Test_less_than_or_equal_to_null_37()
        {
            var selector = "$[?@.a<=null]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": null,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "less than or equal to true (38)" )]
        public void Test_less_than_or_equal_to_true_38()
        {
            var selector = "$[?@.a<=true]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": true,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "less than or equal to false (39)" )]
        public void Test_less_than_or_equal_to_false_39()
        {
            var selector = "$[?@.a<=false]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": false,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "greater than string, single quotes (40)" )]
        public void Test_greater_than_string__single_quotes_40()
        {
            var selector = "$[?@.a>'c']";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "greater than string, double quotes (41)" )]
        public void Test_greater_than_string__double_quotes_41()
        {
            var selector = "$[?@.a>\"c\"]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "d",
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "greater than number (42)" )]
        public void Test_greater_than_number_42()
        {
            var selector = "$[?@.a>10]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 20,
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "greater than null (43)" )]
        public void Test_greater_than_null_43()
        {
            var selector = "$[?@.a>null]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                []
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "greater than true (44)" )]
        public void Test_greater_than_true_44()
        {
            var selector = "$[?@.a>true]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                []
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "greater than false (45)" )]
        public void Test_greater_than_false_45()
        {
            var selector = "$[?@.a>false]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                []
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "greater than or equal to string, single quotes (46)" )]
        public void Test_greater_than_or_equal_to_string__single_quotes_46()
        {
            var selector = "$[?@.a>='c']";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "greater than or equal to string, double quotes (47)" )]
        public void Test_greater_than_or_equal_to_string__double_quotes_47()
        {
            var selector = "$[?@.a>=\"c\"]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "greater than or equal to number (48)" )]
        public void Test_greater_than_or_equal_to_number_48()
        {
            var selector = "$[?@.a>=10]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "greater than or equal to null (49)" )]
        public void Test_greater_than_or_equal_to_null_49()
        {
            var selector = "$[?@.a>=null]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": null,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "greater than or equal to true (50)" )]
        public void Test_greater_than_or_equal_to_true_50()
        {
            var selector = "$[?@.a>=true]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": true,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "greater than or equal to false (51)" )]
        public void Test_greater_than_or_equal_to_false_51()
        {
            var selector = "$[?@.a>=false]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": false,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "exists and not-equals null, absent from data (52)" )]
        public void Test_exists_and_not_equals_null__absent_from_data_52()
        {
            var selector = "$[?@.a&&@.a!=null]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "c",
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "exists and exists, data false (53)" )]
        public void Test_exists_and_exists__data_false_53()
        {
            var selector = "$[?@.a&&@.b]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": false,
                    "b": false
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "exists or exists, data false (54)" )]
        public void Test_exists_or_exists__data_false_54()
        {
            var selector = "$[?@.a||@.b]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "and (55)" )]
        public void Test_and_55()
        {
            var selector = "$[?@.a>0&&@.a<10]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 5,
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "or (56)" )]
        public void Test_or_56()
        {
            var selector = "$[?@.a=='b'||@.a=='d']";
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "not expression (57)" )]
        public void Test_not_expression_57()
        {
            var selector = "$[?!(@.a=='b')]";
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
                """);
            var results = document.Select(selector);
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "not exists (58)" )]
        public void Test_not_exists_58()
        {
            var selector = "$[?!@.a]";
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
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "not exists, data null (59)" )]
        public void Test_not_exists__data_null_59()
        {
            var selector = "$[?!@.a]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "d": "f"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "non-singular existence, wildcard (60)" )]
        public void Test_non_singular_existence__wildcard_60()
        {
            var selector = "$[?@.*]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  [
                    2
                  ],
                  {
                    "a": 3
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "non-singular existence, multiple (61)" )]
        public void Test_non_singular_existence__multiple_61()
        {
            var selector = "$[?@[0, 0, 'a']]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "non-singular existence, slice (62)" )]
        public void Test_non_singular_existence__slice_62()
        {
            var selector = "$[?@[0:2]]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "non-singular existence, negated (63)" )]
        public void Test_non_singular_existence__negated_63()
        {
            var selector = "$[?!@.*]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  1,
                  [],
                  {}
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "non-singular query in comparison, slice (64)" )]
        public void Test_non_singular_query_in_comparison__slice_64()
        {
            var selector = "$[?@[0:0]==0]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "non-singular query in comparison, all children (65)" )]
        public void Test_non_singular_query_in_comparison__all_children_65()
        {
            var selector = "$[?@[*]==0]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "non-singular query in comparison, descendants (66)" )]
        public void Test_non_singular_query_in_comparison__descendants_66()
        {
            var selector = "$[?@..a==0]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "non-singular query in comparison, combined (67)" )]
        public void Test_non_singular_query_in_comparison__combined_67()
        {
            var selector = "$[?@.a[*].a==0]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "nested (68)" )]
        public void Test_nested_68()
        {
            var selector = "$[?@[?@>1]]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "name segment on primitive, selects nothing (69)" )]
        public void Test_name_segment_on_primitive__selects_nothing_69()
        {
            var selector = "$[?@.a == 1]";
            var document = JsonNode.Parse(
                """
                {
                  "a": 1
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
        
        [TestMethod( "name segment on array, selects nothing (70)" )]
        public void Test_name_segment_on_array__selects_nothing_70()
        {
            var selector = "$[?@['0'] == 5]";
            var document = JsonNode.Parse(
                """
                [
                  [
                    5,
                    6
                  ]
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
        
        [TestMethod( "index segment on object, selects nothing (71)" )]
        public void Test_index_segment_on_object__selects_nothing_71()
        {
            var selector = "$[?@[0] == 5]";
            var document = JsonNode.Parse(
                """
                [
                  {
                    "0": 5
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
        
        [TestMethod( "relative non-singular query, index, equal (72)" )]
        public void Test_relative_non_singular_query__index__equal_72()
        {
            var selector = "$[?(@[0, 0]==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "relative non-singular query, index, not equal (73)" )]
        public void Test_relative_non_singular_query__index__not_equal_73()
        {
            var selector = "$[?(@[0, 0]!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "relative non-singular query, index, less-or-equal (74)" )]
        public void Test_relative_non_singular_query__index__less_or_equal_74()
        {
            var selector = "$[?(@[0, 0]<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "relative non-singular query, name, equal (75)" )]
        public void Test_relative_non_singular_query__name__equal_75()
        {
            var selector = "$[?(@['a', 'a']==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "relative non-singular query, name, not equal (76)" )]
        public void Test_relative_non_singular_query__name__not_equal_76()
        {
            var selector = "$[?(@['a', 'a']!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "relative non-singular query, name, less-or-equal (77)" )]
        public void Test_relative_non_singular_query__name__less_or_equal_77()
        {
            var selector = "$[?(@['a', 'a']<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "relative non-singular query, combined, equal (78)" )]
        public void Test_relative_non_singular_query__combined__equal_78()
        {
            var selector = "$[?(@[0, '0']==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "relative non-singular query, combined, not equal (79)" )]
        public void Test_relative_non_singular_query__combined__not_equal_79()
        {
            var selector = "$[?(@[0, '0']!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "relative non-singular query, combined, less-or-equal (80)" )]
        public void Test_relative_non_singular_query__combined__less_or_equal_80()
        {
            var selector = "$[?(@[0, '0']<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "relative non-singular query, wildcard, equal (81)" )]
        public void Test_relative_non_singular_query__wildcard__equal_81()
        {
            var selector = "$[?(@.*==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "relative non-singular query, wildcard, not equal (82)" )]
        public void Test_relative_non_singular_query__wildcard__not_equal_82()
        {
            var selector = "$[?(@.*!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "relative non-singular query, wildcard, less-or-equal (83)" )]
        public void Test_relative_non_singular_query__wildcard__less_or_equal_83()
        {
            var selector = "$[?(@.*<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "relative non-singular query, slice, equal (84)" )]
        public void Test_relative_non_singular_query__slice__equal_84()
        {
            var selector = "$[?(@[0:0]==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "relative non-singular query, slice, not equal (85)" )]
        public void Test_relative_non_singular_query__slice__not_equal_85()
        {
            var selector = "$[?(@[0:0]!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "relative non-singular query, slice, less-or-equal (86)" )]
        public void Test_relative_non_singular_query__slice__less_or_equal_86()
        {
            var selector = "$[?(@[0:0]<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "absolute non-singular query, index, equal (87)" )]
        public void Test_absolute_non_singular_query__index__equal_87()
        {
            var selector = "$[?($[0, 0]==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "absolute non-singular query, index, not equal (88)" )]
        public void Test_absolute_non_singular_query__index__not_equal_88()
        {
            var selector = "$[?($[0, 0]!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "absolute non-singular query, index, less-or-equal (89)" )]
        public void Test_absolute_non_singular_query__index__less_or_equal_89()
        {
            var selector = "$[?($[0, 0]<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "absolute non-singular query, name, equal (90)" )]
        public void Test_absolute_non_singular_query__name__equal_90()
        {
            var selector = "$[?($['a', 'a']==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "absolute non-singular query, name, not equal (91)" )]
        public void Test_absolute_non_singular_query__name__not_equal_91()
        {
            var selector = "$[?($['a', 'a']!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "absolute non-singular query, name, less-or-equal (92)" )]
        public void Test_absolute_non_singular_query__name__less_or_equal_92()
        {
            var selector = "$[?($['a', 'a']<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "absolute non-singular query, combined, equal (93)" )]
        public void Test_absolute_non_singular_query__combined__equal_93()
        {
            var selector = "$[?($[0, '0']==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "absolute non-singular query, combined, not equal (94)" )]
        public void Test_absolute_non_singular_query__combined__not_equal_94()
        {
            var selector = "$[?($[0, '0']!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "absolute non-singular query, combined, less-or-equal (95)" )]
        public void Test_absolute_non_singular_query__combined__less_or_equal_95()
        {
            var selector = "$[?($[0, '0']<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "absolute non-singular query, wildcard, equal (96)" )]
        public void Test_absolute_non_singular_query__wildcard__equal_96()
        {
            var selector = "$[?($.*==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "absolute non-singular query, wildcard, not equal (97)" )]
        public void Test_absolute_non_singular_query__wildcard__not_equal_97()
        {
            var selector = "$[?($.*!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "absolute non-singular query, wildcard, less-or-equal (98)" )]
        public void Test_absolute_non_singular_query__wildcard__less_or_equal_98()
        {
            var selector = "$[?($.*<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "absolute non-singular query, slice, equal (99)" )]
        public void Test_absolute_non_singular_query__slice__equal_99()
        {
            var selector = "$[?($[0:0]==42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "absolute non-singular query, slice, not equal (100)" )]
        public void Test_absolute_non_singular_query__slice__not_equal_100()
        {
            var selector = "$[?($[0:0]!=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "absolute non-singular query, slice, less-or-equal (101)" )]
        public void Test_absolute_non_singular_query__slice__less_or_equal_101()
        {
            var selector = "$[?($[0:0]<=42)]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "multiple selectors (102)" )]
        public void Test_multiple_selectors_102()
        {
            var selector = "$[?@.a,?@.b]";
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
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
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

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors, comparison (103)" )]
        public void Test_multiple_selectors__comparison_103()
        {
            var selector = "$[?@.a=='b',?@.b=='x']";
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
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": "b",
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors, overlapping (104)" )]
        public void Test_multiple_selectors__overlapping_104()
        {
            var selector = "$[?@.a,?@.d]";
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
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors, filter and index (105)" )]
        public void Test_multiple_selectors__filter_and_index_105()
        {
            var selector = "$[?@.a,1]";
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
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
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

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors, filter and wildcard (106)" )]
        public void Test_multiple_selectors__filter_and_wildcard_106()
        {
            var selector = "$[?@.a,*]";
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
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors, filter and slice (107)" )]
        public void Test_multiple_selectors__filter_and_slice_107()
        {
            var selector = "$[?@.a,1:]";
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
                  },
                  {
                    "g": "h"
                  }
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
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

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors, comparison filter, index and slice (108)" )]
        public void Test_multiple_selectors__comparison_filter__index_and_slice_108()
        {
            var selector = "$[1, ?@.a=='b', 1:]";
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
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals number, zero and negative zero (109)" )]
        public void Test_equals_number__zero_and_negative_zero_109()
        {
            var selector = "$[?@.a==-0]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 0,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals number, with and without decimal fraction (110)" )]
        public void Test_equals_number__with_and_without_decimal_fraction_110()
        {
            var selector = "$[?@.a==1.0]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals number, exponent (111)" )]
        public void Test_equals_number__exponent_111()
        {
            var selector = "$[?@.a==1e2]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 100,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals number, positive exponent (112)" )]
        public void Test_equals_number__positive_exponent_112()
        {
            var selector = "$[?@.a==1e+2]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 100,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals number, negative exponent (113)" )]
        public void Test_equals_number__negative_exponent_113()
        {
            var selector = "$[?@.a==1e-2]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 0.01,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals number, decimal fraction (114)" )]
        public void Test_equals_number__decimal_fraction_114()
        {
            var selector = "$[?@.a==1.1]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 1.1,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals number, decimal fraction, no fractional digit (115)" )]
        public void Test_equals_number__decimal_fraction__no_fractional_digit_115()
        {
            var selector = "$[?@.a==1.]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "equals number, decimal fraction, exponent (116)" )]
        public void Test_equals_number__decimal_fraction__exponent_116()
        {
            var selector = "$[?@.a==1.1e2]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 110,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals number, decimal fraction, positive exponent (117)" )]
        public void Test_equals_number__decimal_fraction__positive_exponent_117()
        {
            var selector = "$[?@.a==1.1e+2]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 110,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals number, decimal fraction, negative exponent (118)" )]
        public void Test_equals_number__decimal_fraction__negative_exponent_118()
        {
            var selector = "$[?@.a==1.1e-2]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "a": 0.011,
                    "d": "e"
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals, special nothing (119)" )]
        public void Test_equals__special_nothing_119()
        {
            var selector = "$.values[?length(@.a) == value($..c)]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "c": "d"
                  },
                  {
                    "a": null
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals, empty node list and empty node list (120)" )]
        public void Test_equals__empty_node_list_and_empty_node_list_120()
        {
            var selector = "$[?@.a == @.b]";
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
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "c": 3
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "equals, empty node list and special nothing (121)" )]
        public void Test_equals__empty_node_list_and_special_nothing_121()
        {
            var selector = "$[?@.a == length(@.b)]";
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
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  {
                    "b": 2
                  },
                  {
                    "c": 3
                  }
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "object data (122)" )]
        public void Test_object_data_122()
        {
            var selector = "$[?@<3]";
            var document = JsonNode.Parse(
                """
                {
                  "a": 1,
                  "b": 2,
                  "c": 3
                }
                """);
            var results = document.Select(selector);
            var expectOneOf = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchAny(results, expectOneOf!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "and binds more tightly than or (123)" )]
        public void Test_and_binds_more_tightly_than_or_123()
        {
            var selector = "$[?@.a || @.b && @.c]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "left to right evaluation (124)" )]
        public void Test_left_to_right_evaluation_124()
        {
            var selector = "$[?@.a && @.b || @.c]";
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "group terms, left (125)" )]
        public void Test_group_terms__left_125()
        {
            var selector = "$[?(@.a || @.b) && @.c]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "group terms, right (126)" )]
        public void Test_group_terms__right_126()
        {
            var selector = "$[?@.a && (@.b || @.c)]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "string literal, single quote in double quotes (127)" )]
        public void Test_string_literal__single_quote_in_double_quotes_127()
        {
            var selector = "$[?@ == \"quoted' literal\"]";
            var document = JsonNode.Parse(
                """
                [
                  "quoted' literal",
                  "a",
                  "quoted\\' literal"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "quoted' literal"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "string literal, double quote in single quotes (128)" )]
        public void Test_string_literal__double_quote_in_single_quotes_128()
        {
            var selector = "$[?@ == 'quoted\" literal']";
            var document = JsonNode.Parse(
                """
                [
                  "quoted\" literal",
                  "a",
                  "quoted\\\" literal",
                  "'quoted\" literal'"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "quoted\" literal"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "string literal, escaped single quote in single quotes (129)" )]
        public void Test_string_literal__escaped_single_quote_in_single_quotes_129()
        {
            var selector = "$[?@ == 'quoted\\' literal']";
            var document = JsonNode.Parse(
                """
                [
                  "quoted' literal",
                  "a",
                  "quoted\\' literal",
                  "'quoted\" literal'"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "quoted' literal"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "string literal, escaped double quote in double quotes (130)" )]
        public void Test_string_literal__escaped_double_quote_in_double_quotes_130()
        {
            var selector = "$[?@ == \"quoted\\\" literal\"]";
            var document = JsonNode.Parse(
                """
                [
                  "quoted\" literal",
                  "a",
                  "quoted\\\" literal",
                  "'quoted\" literal'"
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "quoted\" literal"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "literal true must be compared (131)" )]
        public void Test_literal_true_must_be_compared_131()
        {
            var selector = "$[?true]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "literal false must be compared (132)" )]
        public void Test_literal_false_must_be_compared_132()
        {
            var selector = "$[?false]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "literal string must be compared (133)" )]
        public void Test_literal_string_must_be_compared_133()
        {
            var selector = "$[?'abc']";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "literal int must be compared (134)" )]
        public void Test_literal_int_must_be_compared_134()
        {
            var selector = "$[?2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "literal float must be compared (135)" )]
        public void Test_literal_float_must_be_compared_135()
        {
            var selector = "$[?2.2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "literal null must be compared (136)" )]
        public void Test_literal_null_must_be_compared_136()
        {
            var selector = "$[?null]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "and, literals must be compared (137)" )]
        public void Test_and__literals_must_be_compared_137()
        {
            var selector = "$[?true && false]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "or, literals must be compared (138)" )]
        public void Test_or__literals_must_be_compared_138()
        {
            var selector = "$[?true || false]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "and, right hand literal must be compared (139)" )]
        public void Test_and__right_hand_literal_must_be_compared_139()
        {
            var selector = "$[?true == false && false]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "or, right hand literal must be compared (140)" )]
        public void Test_or__right_hand_literal_must_be_compared_140()
        {
            var selector = "$[?true == false || false]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "and, left hand literal must be compared (141)" )]
        public void Test_and__left_hand_literal_must_be_compared_141()
        {
            var selector = "$[?false && true == false]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "or, left hand literal must be compared (142)" )]
        public void Test_or__left_hand_literal_must_be_compared_142()
        {
            var selector = "$[?false || true == false]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
    }
}

