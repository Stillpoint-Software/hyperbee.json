using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Descriptors.Node;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Query;

[TestClass]
public class JsonPathFilterTests : JsonTestBase
{
    [DataTestMethod]
    [DataRow( "$[?(@.key)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.key)]", typeof( JsonNode ) )]
    [DataRow( "$[? @.key]", typeof( JsonDocument ) )]
    [DataRow( "$[? @.key]", typeof( JsonNode ) )]
    public void FilterWithArrayTruthyProperty( string query, Type sourceType )
    {
        const string json =
            """
            [
              {"some": "some value"}, 
              {"key": "value"}
            ]
            """;
        var source = GetDocumentFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.FromJsonPathPointer( "$[1]" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }
    [DataTestMethod]
    [DataRow( "$[?(@.key)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.key)]", typeof( JsonNode ) )]
    public void FilterWithTruthyProperty( string query, Type sourceType )
    {
        const string json =
            """
            {
              "key": 42,
              "another": {
                "key": 1
              }
            }
            """;
        var source = GetDocumentFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.FromJsonPathPointer( "$['another']" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[?(@.key<42)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.key<42)]", typeof( JsonNode ) )]
    [DataRow( "$[?@.key < 42]", typeof( JsonDocument ) )]
    [DataRow( "$[?@.key < 42]", typeof( JsonNode ) )]
    public void FilterWithLessThan( string query, Type sourceType )
    {
        const string json =
            """
            [
              {"key": 0}, 
              {"key": 42}, 
              {"key": -1}, 
              {"key": 41}, 
              {"key": 43}, 
              {"key": 42.0001}, 
              {"key": 41.9999}, 
              {"key": 100}, 
              {"some": "value"}
            ]
            """;

        var source = GetDocumentFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.FromJsonPathPointer( "$[0]" ),
            source.FromJsonPathPointer( "$[2]" ),
            source.FromJsonPathPointer( "$[3]" ),
            source.FromJsonPathPointer( "$[6]" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$..*[?(@.id>2)]", typeof( JsonDocument ) )]
    [DataRow( "$..*[?(@.id>2)]", typeof( JsonNode ) )]
    public void FilterAfterDoNotationWithWildcardAfterRecursiveDecent( string query, Type sourceType )
    {
        const string json =
            """
            [
              {
                "complex": {
                  "one": [
                    {
                      "name": "first",
                      "id": 1
                    },
                    {
                      "name": "next",
                      "id": 2
                    },
                    {
                      "name": "another",
                      "id": 3
                    },
                    {
                      "name": "more",
                      "id": 4
                    }
                  ],
                  "more": {
                    "name": "next to last",
                    "id": 5
                  }
                }
              },
              {
                "name": "last",
                "id": 6
              }
            ]
            """;

        var source = GetDocumentFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.FromJsonPathPointer( "$[0]['complex']['more']" ),
            source.FromJsonPathPointer( "$[0]['complex']['one'][2]" ),
            source.FromJsonPathPointer( "$[0]['complex']['one'][3]" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[?(@.a && (@.b || @.c))]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.a && (@.b || @.c))]", typeof( JsonNode ) )]
    public void FilterWithDifferentGroupedOperators( string query, Type sourceType )
    {
        const string json =
            """
            [
              {
                "a": true
              },
              {
                "a": true,
                "b": true
              },
              {
                "a": true,
                "b": true,
                "c": true
              },
              {
                "b": true,
                "c": true
              },
              {
                "a": true,
                "c": true
              },
              {
                "c": true
              },
              {
                "b": true
              }
            ]        
            """;

        var source = GetDocumentFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.FromJsonPathPointer( "$[1]" ),
            source.FromJsonPathPointer( "$[2]" ),
            source.FromJsonPathPointer( "$[4]" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }


    [DataTestMethod]
    [DataRow( "$[?(@.a && @.b || @.c)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.a && @.b || @.c)]", typeof( JsonNode ) )]
    public void FilterWithDifferentUngroupedOperators( string query, Type sourceType )
    {
        const string json =
            """
            [
              {
                "a": true,
                "b": true
              },
              {
                "a": true,
                "b": true,
                "c": true
              },
              {
                "b": true,
                "c": true
              },
              {
                "a": true,
                "c": true
              },
              {
                "a": true
              },
              {
                "b": true
              },
              {
                "c": true
              },
              {
                "d": true
              },
              {}
            ]
            
            """;

        var source = GetDocumentFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.FromJsonPathPointer( "$[0]" ),
            source.FromJsonPathPointer( "$[1]" ),
            source.FromJsonPathPointer( "$[2]" ),
            source.FromJsonPathPointer( "$[3]" ),
            source.FromJsonPathPointer( "$[6]" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    // [DataTestMethod]
    // [DataRow( "$[?(@.d == [\"v1\", \"v2\"])]", typeof(JsonDocument) )]
    // [DataRow( "$[?(@.d == [\"v1\", \"v2\"])]", typeof(JsonNode) )]
    // public void FilterWithEqualsArray( string query, Type sourceType )
    // {
    //     const string json =
    //         """
    //         [
    //           {
    //             "d": [
    //               "v1",
    //               "v2"
    //             ]
    //           },
    //           {
    //             "d": [
    //               "a",
    //               "b"
    //             ]
    //           },
    //           {
    //             "d": "v1"
    //           },
    //           {
    //             "d": "v2"
    //           },
    //           {
    //             "d": {}
    //           },
    //           {
    //             "d": []
    //           },
    //           {
    //             "d": null
    //           },
    //           {
    //             "d": -1
    //           },
    //           {
    //             "d": 0
    //           },
    //           {
    //             "d": 1
    //           },
    //           {
    //             "d": "['v1','v2']"
    //           },
    //           {
    //             "d": "['v1', 'v2']"
    //           },
    //           {
    //             "d": "v1,v2"
    //           },
    //           {
    //             "d": "[\"v1\", \"v2\"]"
    //           },
    //           {
    //             "d": "[\"v1\",\"v2\"]"
    //           }
    //         ]
    //         """;
    //
    //     var source = GetDocumentFromSource( sourceType, json );
    //
    //     var matches = source.Select( query );
    //     var expected = new[]
    //     {
    //         source.GetPropertyFromPath( "$[0]['d']" )
    //     };
    //
    //     Assert.IsTrue( expected.SequenceEqual( matches ) );
    // }

    // [DataTestMethod]
    // [DataRow( "$[?(@[0:1]==[1])]", typeof( JsonDocument ) )]
    // [DataRow( "$[?(@[0:1]==[1])]", typeof( JsonNode ) )]
    // public void FilterWithEqualsArrayForSliceWithRange1( string query, Type sourceType )
    // {
    //     var jsonElement1 = JsonDocument.Parse( "[\"v1\", \"v2\"]" ).RootElement;
    //     var jsonElement2 = JsonDocument.Parse( "[\"v1\", \"v2\"]" ).RootElement;
    //
    //     var r1 = jsonElement1.Equals( jsonElement2 );
    //     var r2 = jsonElement1.DeepEquals( jsonElement2 );
    //     var r3 = jsonElement1.Equals( jsonElement1 );
    //
    //
    //     Assert.Fail();
    // }
    //
    // [DataTestMethod]
    // [DataRow( "$[?(@.*==[1,2])]", typeof( JsonDocument ) )]
    // [DataRow( "$[?(@.*==[1,2])]", typeof( JsonNode ) )]
    // public void FilterWithEqualsArrayForDotNotationWithStart( string query, Type sourceType )
    // {
    //     Assert.Fail();
    // }
    //
    // [DataTestMethod]
    // [DataRow( "$[?(@.d==[\"v1\",\"v2\"] || (@.d == true))]", typeof( JsonDocument ) )]
    // [DataRow( "$[?(@.d==[\"v1\",\"v2\"] || (@.d == true))]", typeof( JsonNode ) )]
    // public void FilterWithEqualsArrayOrEqualsTrue( string query, Type sourceType )
    // {
    //     Assert.Fail();
    // }
    //
    // [DataTestMethod]
    // [DataRow( "$[?(@.d==['v1','v2'])]", typeof( JsonDocument ) )]
    // [DataRow( "$[?(@.d==['v1','v2'])]", typeof( JsonNode ) )]
    // public void FilterWithEqualsArrayWithSingleQuotes( string query, Type sourceType )
    // {
    //     Assert.Fail();
    // }
    //
    // [DataTestMethod]
    // [DataRow( "$[?((@.key<44)==false)]", typeof( JsonDocument ) )]
    // [DataRow( "$[?((@.key<44)==false)]", typeof( JsonNode ) )]
    // public void FilterWithEqualsBooleanExpressionValue( string query, Type sourceType )
    // {
    //     Assert.Fail();
    // }
    //
    // [DataTestMethod]
    // [DataRow( "$[?(@.key==false)]", typeof( JsonDocument ) )]
    // [DataRow( "$[?(@.key==false)]", typeof( JsonNode ) )]
    // public void FilterWithEqualsFalse( string query, Type sourceType )
    // {
    //     Assert.Fail();
    // }

    [DataTestMethod]
    [DataRow( true, true, true )]
    [DataRow( false, false, true )]
    [DataRow( false, true, false )]
    [DataRow( true, false, false )]
    [DataRow( "hello", "hello", true )]
    [DataRow( 10F, 10F, true )]
    [DataRow( "hello", "world", false )]
    [DataRow( 99F, 11F, false )]
    [DataRow( "hello", 11F, false )]
    [DataRow( false, 11F, false )]
    [DataRow( true, 11F, false )]
    public void ValueEqualResults( object left, object right, bool areEqual )
    {
        var accessor = new NodeValueAccessor();

        var a = new FilterParser<JsonNode>.JsonComparerExpressionFactory.JsonComparer( accessor, left );
        var b = new FilterParser<JsonNode>.JsonComparerExpressionFactory.JsonComparer( accessor, right );

        var result = a == b;

        Assert.AreEqual( areEqual, result );
    }

    [DataTestMethod]
    [DataRow( true, true, true )]
    [DataRow( false, false, true )]
    [DataRow( false, true, false )]
    [DataRow( true, false, true )]
    [DataRow( "hello", "hello", true )]
    [DataRow( 10F, 10F, true )]
    [DataRow( 14F, 10F, true )]
    [DataRow( 1F, 14F, false )]

    public void ValueGreaterResults( object left, object right, bool areEqual )
    {
        var accessor = new NodeValueAccessor();

        var a = new FilterParser<JsonNode>.JsonComparerExpressionFactory.JsonComparer( accessor, left );
        var b = new FilterParser<JsonNode>.JsonComparerExpressionFactory.JsonComparer( accessor, right );

        var result = a >= b;

        Assert.AreEqual( areEqual, result );
    }

    [DataTestMethod]
    [DataRow( """{ "value": 1 }""", 99F, false )]
    [DataRow( """{ "value": 99 }""", 99F, true )]
    [DataRow( """{ "value": "hello" }""", "world", false )]
    [DataRow( """{ "value": "hello" }""", "hello", true )]
    //[DataRow( """{ "value": { "child": 5 } }""", "hello", false )]
    public void JsonElementValueResults( string left, object right, bool areEqual )
    {
        var accessor = new NodeValueAccessor();
        var node = new List<JsonNode> { JsonNode.Parse( left )["value"] };

        var a = new FilterParser<JsonNode>.JsonComparerExpressionFactory.JsonComparer( accessor, node );
        var b = new FilterParser<JsonNode>.JsonComparerExpressionFactory.JsonComparer( accessor, right );

        var result = a == b;

        Assert.AreEqual( areEqual, result );
    }
}

