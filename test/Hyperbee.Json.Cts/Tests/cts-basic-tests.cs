// This file was auto generated.

using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Cts.Tests
{
    [TestClass]
    public class CtsBasicTest
    {
        
        [TestMethod( "root (1)" )]
        public void Test_root_1()
        {
            var selector = "$";
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
                [
                  [
                    "first",
                    "second"
                  ]
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "no leading whitespace (2)" )]
        public void Test_no_leading_whitespace_2()
        {
            var selector = " $";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "no trailing whitespace (3)" )]
        public void Test_no_trailing_whitespace_3()
        {
            var selector = "$ ";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "name shorthand (4)" )]
        public void Test_name_shorthand_4()
        {
            var selector = "$.a";
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
        
        [TestMethod( "name shorthand, extended unicode ☺ (5)" )]
        public void Test_name_shorthand__extended_unicode___5()
        {
            var selector = "$.☺";
            var document = JsonNode.Parse(
                """
                {
                  "☺": "A",
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
        
        [TestMethod( "name shorthand, underscore (6)" )]
        public void Test_name_shorthand__underscore_6()
        {
            var selector = "$._";
            var document = JsonNode.Parse(
                """
                {
                  "_": "A",
                  "_foo": "B"
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
        
        [TestMethod( "name shorthand, symbol (7)" )]
        public void Test_name_shorthand__symbol_7()
        {
            var selector = "$.&";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "name shorthand, number (8)" )]
        public void Test_name_shorthand__number_8()
        {
            var selector = "$.1";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "name shorthand, absent data (9)" )]
        public void Test_name_shorthand__absent_data_9()
        {
            var selector = "$.c";
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
        
        [TestMethod( "name shorthand, array data (10)" )]
        public void Test_name_shorthand__array_data_10()
        {
            var selector = "$.a";
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
        
        [TestMethod( "wildcard shorthand, object data (11)" )]
        public void Test_wildcard_shorthand__object_data_11()
        {
            var selector = "$.*";
            var document = JsonNode.Parse(
                """
                {
                  "a": "A",
                  "b": "B"
                }
                """);
            var results = document.Select(selector);
            var expectOneOf = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchAny(results, expectOneOf!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "wildcard shorthand, array data (12)" )]
        public void Test_wildcard_shorthand__array_data_12()
        {
            var selector = "$.*";
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
                [
                  "first",
                  "second"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "wildcard selector, array data (13)" )]
        public void Test_wildcard_selector__array_data_13()
        {
            var selector = "$[*]";
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
                [
                  "first",
                  "second"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "wildcard shorthand, then name shorthand (14)" )]
        public void Test_wildcard_shorthand__then_name_shorthand_14()
        {
            var selector = "$.*.a";
            var document = JsonNode.Parse(
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
                """);
            var results = document.Select(selector);
            var expectOneOf = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchAny(results, expectOneOf!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors (15)" )]
        public void Test_multiple_selectors_15()
        {
            var selector = "$[0,2]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  0,
                  2
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors, space instead of comma (16)" )]
        public void Test_multiple_selectors__space_instead_of_comma_16()
        {
            var selector = "$[0 2]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "multiple selectors, name and index, array data (17)" )]
        public void Test_multiple_selectors__name_and_index__array_data_17()
        {
            var selector = "$['a',1]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  1
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors, name and index, object data (18)" )]
        public void Test_multiple_selectors__name_and_index__object_data_18()
        {
            var selector = "$['a',1]";
            var document = JsonNode.Parse(
                """
                {
                  "a": 1,
                  "b": 2
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  1
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors, index and slice (19)" )]
        public void Test_multiple_selectors__index_and_slice_19()
        {
            var selector = "$[1,5:7]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  1,
                  5,
                  6
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors, index and slice, overlapping (20)" )]
        public void Test_multiple_selectors__index_and_slice__overlapping_20()
        {
            var selector = "$[1,0:3]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  1,
                  0,
                  1,
                  2
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors, duplicate index (21)" )]
        public void Test_multiple_selectors__duplicate_index_21()
        {
            var selector = "$[1,1]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
                """
                [
                  1,
                  1
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors, wildcard and index (22)" )]
        public void Test_multiple_selectors__wildcard_and_index_22()
        {
            var selector = "$[*,1]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors, wildcard and name (23)" )]
        public void Test_multiple_selectors__wildcard_and_name_23()
        {
            var selector = "$[*,'a']";
            var document = JsonNode.Parse(
                """
                {
                  "a": "A",
                  "b": "B"
                }
                """);
            var results = document.Select(selector);
            var expectOneOf = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchAny(results, expectOneOf!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors, wildcard and slice (24)" )]
        public void Test_multiple_selectors__wildcard_and_slice_24()
        {
            var selector = "$[*,0:2]";
            var document = JsonNode.Parse(
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
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "multiple selectors, multiple wildcards (25)" )]
        public void Test_multiple_selectors__multiple_wildcards_25()
        {
            var selector = "$[*,*]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  0,
                  1,
                  2,
                  0,
                  1,
                  2
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "empty segment (26)" )]
        public void Test_empty_segment_26()
        {
            var selector = "$[]";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
        
        [TestMethod( "descendant segment, index (27)" )]
        public void Test_descendant_segment__index_27()
        {
            var selector = "$..[1]";
            var document = JsonNode.Parse(
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
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  1,
                  3
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "descendant segment, name shorthand (28)" )]
        public void Test_descendant_segment__name_shorthand_28()
        {
            var selector = "$..a";
            var document = JsonNode.Parse(
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
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "b",
                  "c"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "descendant segment, wildcard shorthand, array data (29)" )]
        public void Test_descendant_segment__wildcard_shorthand__array_data_29()
        {
            var selector = "$..*";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  0,
                  1
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "descendant segment, wildcard selector, array data (30)" )]
        public void Test_descendant_segment__wildcard_selector__array_data_30()
        {
            var selector = "$..[*]";
            var document = JsonNode.Parse(
                """
                [
                  0,
                  1
                ]
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  0,
                  1
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "descendant segment, wildcard selector, nested arrays (31)" )]
        public void Test_descendant_segment__wildcard_selector__nested_arrays_31()
        {
            var selector = "$..[*]";
            var document = JsonNode.Parse(
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
                """);
            var results = document.Select(selector);
            var expectOneOf = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchAny(results, expectOneOf!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "descendant segment, wildcard selector, nested objects (32)" )]
        public void Test_descendant_segment__wildcard_selector__nested_objects_32()
        {
            var selector = "$..[*]";
            var document = JsonNode.Parse(
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
                """);
            var results = document.Select(selector);
            var expectOneOf = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchAny(results, expectOneOf!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "descendant segment, wildcard shorthand, object data (33)" )]
        public void Test_descendant_segment__wildcard_shorthand__object_data_33()
        {
            var selector = "$..*";
            var document = JsonNode.Parse(
                """
                {
                  "a": "b"
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
                """
                [
                  "b"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "descendant segment, wildcard shorthand, nested data (34)" )]
        public void Test_descendant_segment__wildcard_shorthand__nested_data_34()
        {
            var selector = "$..*";
            var document = JsonNode.Parse(
                """
                {
                  "o": [
                    {
                      "a": "b"
                    }
                  ]
                }
                """);
            var results = document.Select(selector);
            var expect = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "descendant segment, multiple selectors (35)" )]
        public void Test_descendant_segment__multiple_selectors_35()
        {
            var selector = "$..['a','d']";
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
                  "b",
                  "e",
                  "c",
                  "f"
                ]
                """);

            var match = TestHelper.MatchOne(results, expect!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "descendant segment, object traversal, multiple selectors (36)" )]
        public void Test_descendant_segment__object_traversal__multiple_selectors_36()
        {
            var selector = "$..['a','d']";
            var document = JsonNode.Parse(
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
                """);
            var results = document.Select(selector);
            var expectOneOf = JsonNode.Parse(
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
                """);

            var match = TestHelper.MatchAny(results, expectOneOf!);
            Assert.IsTrue(match);
        }
        
        [TestMethod( "bald descendant segment (37)" )]
        public void Test_bald_descendant_segment_37()
        {
            var selector = "$..";
            var document = JsonNode.Parse( "[0]" ); // Empty node

            AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => { _ = document.Select( selector ).ToArray(); } );
        }
    }
}

