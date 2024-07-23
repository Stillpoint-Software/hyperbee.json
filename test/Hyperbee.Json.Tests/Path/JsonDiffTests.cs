﻿using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Patch;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Path;

[TestClass]
public class JsonDiffTests : JsonTestBase
{
    [DataTestMethod]
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
        Assert.AreEqual( JsonPatchOperationType.Add, results[0].Operation );
        Assert.AreEqual( "/last", results[0].Path );
        Assert.AreEqual( "Doe", Unwrap( results[0].Value ) );
    }

    [DataTestMethod]
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
        Assert.AreEqual( JsonPatchOperationType.Add, results[0].Operation );
        Assert.AreEqual( "/categories/1", results[0].Path );
        Assert.AreEqual( "B", Unwrap( results[0].Value ) );
    }

    [DataTestMethod]
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
        Assert.AreEqual( JsonPatchOperationType.Remove, results[0].Operation );
        Assert.AreEqual( "/last", results[0].Path );
        Assert.IsNull( results[0].Value );
    }

    [DataTestMethod]
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
        Assert.AreEqual( JsonPatchOperationType.Remove, results[0].Operation );
        Assert.AreEqual( "/categories/1", results[0].Path );
        Assert.IsNull( results[0].Value );
    }

    [DataTestMethod]
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
        Assert.AreEqual( JsonPatchOperationType.Replace, results[0].Operation );
        Assert.AreEqual( "/first", results[0].Path );
        Assert.AreEqual( "Mark", Unwrap( results[0].Value ) );
    }


    [DataTestMethod]
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
        Assert.AreEqual( JsonPatchOperationType.Replace, results[0].Operation );
        Assert.AreEqual( "/categories/1", results[0].Path );
        Assert.AreEqual( "C", Unwrap( results[0].Value ) );
    }

    [DataTestMethod]
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

    private static object Unwrap( object value )
    {
        return value switch
        {
            JsonElement element => element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.GetInt32(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => value
            },
            JsonNode node => node.GetValueKind() switch
            {
                JsonValueKind.String => node.GetValue<string>(),
                JsonValueKind.Number => node.GetValue<int>(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => value
            },
            _ => value
        };
    }

    private static JsonPatchOperation[] Diff( Type sourceType, string source, string target )
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
