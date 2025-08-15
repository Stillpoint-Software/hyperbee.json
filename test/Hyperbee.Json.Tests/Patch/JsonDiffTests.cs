using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Patch;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Patch;

[TestClass]
public class JsonDiffTests : JsonTestBase
{
    [TestMethod]
    [DataRow( typeof( JsonDocument ) )]
    [DataRow( typeof( JsonNode ) )]
    public void Add_WhenTargetHasAdditionalProperty( Type sourceType )
    {
        var source =
            """
                {
                    "first": "John"
                }
            """;

        var target =
            """
                {
                    "first": "John",
                    "last": "Doe"
                }
            """;

        var results = Diff( sourceType, source, target );

        Assert.IsTrue( results.Length == 1 );
        Assert.AreEqual( PatchOperationType.Add, results[0].Operation );
        Assert.AreEqual( "/last", results[0].Path );
        Assert.AreEqual( "Doe", Unwrap( results[0].Value ) );
    }

    [TestMethod]
    [DataRow( typeof( JsonDocument ) )]
    [DataRow( typeof( JsonNode ) )]
    public void Add_WhenTargetArrayHasMoreItems( Type sourceType )
    {
        var source =
            """
                {
                    "categories": [ "A" ]
                }
            """;

        var target =
            """
                {
                    "categories": [ "A", "B" ]
                }
            """;

        var results = Diff( sourceType, source, target );

        Assert.IsTrue( results.Length == 1 );
        Assert.AreEqual( PatchOperationType.Add, results[0].Operation );
        Assert.AreEqual( "/categories/1", results[0].Path );
        Assert.AreEqual( "B", Unwrap( results[0].Value ) );
    }

    [TestMethod]
    [DataRow( typeof( JsonDocument ) )]
    [DataRow( typeof( JsonNode ) )]
    public void Remove_WhenTargetIsMissingProperty( Type sourceType )
    {
        var source =
            """
                {
                    "first": "John",
                    "last": "Doe"
                }
            """;

        var target =
            """
                {
                    "first": "John"
                }
            """;

        var results = Diff( sourceType, source, target );

        Assert.IsTrue( results.Length == 1 );
        Assert.AreEqual( PatchOperationType.Remove, results[0].Operation );
        Assert.AreEqual( "/last", results[0].Path );
        Assert.IsNull( results[0].Value );
    }

    [TestMethod]
    [DataRow( typeof( JsonDocument ) )]
    [DataRow( typeof( JsonNode ) )]
    public void Remove_WhenTargetArrayHasLessItems( Type sourceType )
    {
        var source =
            """
                {
                    "categories": [ "A", "B" ]
                }
            """;

        var target =
            """
                {
                    "categories": [ "A" ]
                }
            """;

        var results = Diff( sourceType, source, target );

        Assert.IsTrue( results.Length == 1 );
        Assert.AreEqual( PatchOperationType.Remove, results[0].Operation );
        Assert.AreEqual( "/categories/1", results[0].Path );
        Assert.IsNull( results[0].Value );
    }

    [TestMethod]
    [DataRow( typeof( JsonDocument ) )]
    [DataRow( typeof( JsonNode ) )]
    public void Replace_WhenTargetPropertyUpdated( Type sourceType )
    {
        var source =
            """
                {
                    "first": "John"
                }
            """;

        var target =
            """
                {
                    "first": "Mark"
                }
            """;

        var results = Diff( sourceType, source, target );

        Assert.IsTrue( results.Length == 1 );
        Assert.AreEqual( PatchOperationType.Replace, results[0].Operation );
        Assert.AreEqual( "/first", results[0].Path );
        Assert.AreEqual( "Mark", Unwrap( results[0].Value ) );
    }

    [TestMethod]
    [DataRow( typeof( JsonDocument ) )]
    [DataRow( typeof( JsonNode ) )]
    public void Replace_WhenTargetArrayItemsAreDifferent( Type sourceType )
    {
        var source =
            """
                {
                    "categories": [ "A", "B" ]
                }
            """;

        var target =
            """
                {
                    "categories": [ "A", "C" ]
                }
            """;

        var results = Diff( sourceType, source, target );

        Assert.IsTrue( results.Length == 1 );
        Assert.AreEqual( PatchOperationType.Replace, results[0].Operation );
        Assert.AreEqual( "/categories/1", results[0].Path );
        Assert.AreEqual( "C", Unwrap( results[0].Value ) );
    }

    [TestMethod]
    [DataRow( typeof( JsonDocument ) )]
    [DataRow( typeof( JsonNode ) )]
    public void Replace_WhenComplexTargetArrayHasDifferentValues( Type sourceType )
    {
        var source =
            """
                {
                    "categories": [ 
                        "A",
                        {
                            "name": "B",
                            "value": 1
                        } 
                    ]
                }
            """;

        var target =
            """
                {
                    "categories": [ 
                        "A",
                        {
                            "name": "B",
                            "value": 2
                        },
                        {
                            "name": "C",
                            "value": 3
                        } 
                    ]
                }
            """;

        var results = Diff( sourceType, source, target );

        Assert.IsTrue( results.Length == 2 );

        Assert.AreEqual( PatchOperationType.Add, results[0].Operation );
        Assert.AreEqual( "/categories/2", results[0].Path );

        Assert.AreEqual( PatchOperationType.Replace, results[1].Operation );
        Assert.AreEqual( "/categories/1/value", results[1].Path );
        Assert.AreEqual( 2, Unwrap( results[1].Value ) );
    }

    [TestMethod]
    [DataRow( typeof( JsonDocument ) )]
    [DataRow( typeof( JsonNode ) )]
    public void MultipleOperations_WhenTargetHasMultipleUpdates( Type sourceType )
    {
        var source =
            """
                {
                    "first": "John",
                    "age": 30,
                    "address": {
                        "city": "New York",
                        "state": "NY"
                    },
                    "categories": [ "A", "B" ]
                }
            """;

        var target =
            """
                {
                    "first": "John",
                    "last": "Doe",
                    "age": 45,
                    "address": {
                        "city": "New York",
                        "zip": "12345"
                    },
                    "categories": [ "B", "C", "D" ],
                    "job": {
                        "title": "Developer",
                        "company": "Microsoft"
                    }
                }
            """;

        var results = Diff( sourceType, source, target ).ToArray();

        Assert.IsTrue( results.Length == 8 );
    }

    [TestMethod]
    [DataRow( typeof( JsonDocument ) )]
    [DataRow( typeof( JsonNode ) )]
    public void EscapePath_WhenJsonHasPropertyNames( Type sourceType )
    {
        var source =
            """
            {
               "foo": ["bar", "baz"],
               "": 0,
               "a/b": 1,
               "c%d": 2,
               "e^f": 3,
               "g|h": 4,
               "i\\j": 5,
               "k\"l": 6,
               " ": 7,
               "m~n": 8
            }
            """;

        var target =
            """
                {
                }
            """;

        var results = Diff( sourceType, source, target ).ToArray();

        Assert.IsTrue( results.Length == 10 );

        Assert.AreEqual( "/foo", results[0].Path );
        Assert.AreEqual( "/", results[1].Path );
        Assert.AreEqual( "/a~1b", results[2].Path );
        Assert.AreEqual( "/c%d", results[3].Path );
        Assert.AreEqual( "/e^f", results[4].Path );
        Assert.AreEqual( "/g|h", results[5].Path );
        Assert.AreEqual( "/i\\j", results[6].Path );
        Assert.AreEqual( "/k\"l", results[7].Path );
        Assert.AreEqual( "/ ", results[8].Path );
        Assert.AreEqual( "/m~0n", results[9].Path );
    }

    [TestMethod]
    public void MultipleOperations_WhenSourceAndTargetAreObjects()
    {
        var source = new
        {
            first = "John",
            age = 30,
            address = new { city = "New York", state = "NY" },
            categories = new[] { "A", "B" }
        };

        var target = new
        {
            first = "John",
            last = "Doe",
            age = 45,
            address = new { city = "New York", zip = "12345" },
            categories = new[] { "B", "C", "D" },
            job = new { title = "Developer", company = "Microsoft" }
        };

        var results = JsonDiff<JsonElement>.Diff( source, target ).ToArray();

        Assert.IsTrue( results.Length == 8 );
    }

    [TestMethod]
    public void ApplyPatch_ToExistingSource()
    {
        const string sourceJson =
            """
            {
            "categories": ["a", "b", "c"]
            }
            """;
        const string targetJson =
            """
            {
            "categories": ["a", "c", "d"]
            }
            """;

        var source = JsonNode.Parse( sourceJson );
        var target = JsonNode.Parse( targetJson );

        var diff = JsonDiff<JsonNode>.Diff( source, target ).ToArray();

        // NOTE: this is testing that the patch applies a copy of the source
        // else JsonNode will fail with a parent already exists exception
        var patch = new JsonPatch( diff );
        patch.Apply( source );
    }

    private static object Unwrap( object value )
    {
        switch ( value )
        {
            case JsonElement element:
                switch ( element.ValueKind )
                {
                    case JsonValueKind.String:
                        return element.GetString();
                    case JsonValueKind.Number:
                        return element.GetInt32();
                    case JsonValueKind.True:
                        return true;
                    case JsonValueKind.False:
                        return false;
                    case JsonValueKind.Null:
                        return null;
                    default:
                        return element;
                }
            case JsonNode node:
                switch ( node.GetValueKind() )
                {
                    case JsonValueKind.String:
                        return node.GetValue<string>();
                    case JsonValueKind.Number:
                        return node.GetValue<int>();
                    case JsonValueKind.True:
                        return true;
                    case JsonValueKind.False:
                        return false;
                    case JsonValueKind.Null:
                        return null;
                    default:
                        return node;
                }
            default:
                return value;
        }
    }

    private static PatchOperation[] Diff( Type sourceType, string source, string target )
    {
        return sourceType switch
        {
            _ when sourceType == typeof( JsonNode ) => JsonDiff<JsonNode>.Diff(
                JsonNode.Parse( source ),
                JsonNode.Parse( target ) ).ToArray(),

            _ when sourceType == typeof( JsonDocument ) => JsonDiff<JsonElement>.Diff(
                JsonDocument.Parse( source ).RootElement,
                JsonDocument.Parse( target ).RootElement ).ToArray(),

            _ => throw new ArgumentOutOfRangeException( nameof( sourceType ), sourceType, null )
        };
    }
}

