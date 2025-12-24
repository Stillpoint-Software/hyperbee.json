using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Path.Query;

[TestClass]
public class JsonPathInOperationsTests : JsonTestBase
{
    [TestMethod]
    [DataRow( "$[?(@.value in [1, 42, 100])]", "$[0]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.value in [1, 42, 100])]", "$[0]", typeof( JsonNode ) )]
    [DataRow( "$[?(@.value in ['a', 'b', 'c'])]", "$[1]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.value in ['a', 'b', 'c'])]", "$[1]", typeof( JsonNode ) )]
    public void InOperation_SingleValue( string query, string expect, Type sourceType )
    {
        const string json =
            """
            [
                {
                    "value": 42
                },
                {
                    "value": "b"
                }
            ]
            """;
        var source = GetDocumentAdapter( sourceType, json );
        var expected = new[]
        {
            source.FromJsonPathPointer(expect)
        };

        var matches = source.Select( query ).ToList();
        Assert.HasCount( expected.Length, matches );
        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [TestMethod]
    [DataRow( "$[?(@.values in [1, 2, 3])]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.values in [1, 2, 3])]", typeof( JsonNode ) )]
    [DataRow( "$[?(@.values in ['x', 'y', 'z'])]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.values in ['x', 'y', 'z'])]", typeof( JsonNode ) )]
    public void InOperation_ArrayValueIsNotAnArrayElement( string query, Type sourceType )
    {
        const string json =
            """
            [
                {
                    "values": [1, 2, 3]
                },
                {
                    "values": ["x", "y", "z"]
                }
            ]
            """;
        var source = GetDocumentAdapter( sourceType, json );

        var matches = source.Select( query ).ToList();
        Assert.IsEmpty( matches );
    }

    [TestMethod]
    [DataRow( "$[?(@.values in [1, [1,2,3], 3])]", "$[0]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.values in [1, [1,2,3], 3])]", "$[0]", typeof( JsonNode ) )]
    [DataRow( "$[?(@.values in ['x', ['x','y','z'], 'z'])]", "$[1]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.values in ['x', ['x','y','z'], 'z'])]", "$[1]", typeof( JsonNode ) )]
    public void InOperation_ArrayValueIsAnArrayElement( string query, string expect, Type sourceType )
    {
        const string json =
            """
            [
                {
                    "values": [1, 2, 3]
                },
                {
                    "values": ["x", "y", "z"]
                }
            ]
            """;
        var source = GetDocumentAdapter( sourceType, json );
        var expected = new[] { source.FromJsonPathPointer( expect ) };

        var matches = source.Select( query ).ToList();
        Assert.HasCount( expected.Length, matches );
        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [TestMethod]
    [DataRow( "$[?(@.a in [1, 2, 3] && @.b in [4, 5, 6])]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.a in [1, 2, 3] && @.b in [4, 5, 6])]", typeof( JsonNode ) )]
    public void InOperation_MultipleConditions( string query, Type sourceType )
    {
        const string json =
            """
            [
                {
                    "a": 1,
                    "b": 5
                },
                {
                    "a": 3,
                    "b": 6
                }
            ]
            """;
        var source = GetDocumentAdapter( sourceType, json );
        var expected = new[]
        {
            source.FromJsonPathPointer("$[0]"),
            source.FromJsonPathPointer("$[1]")
        };

        var matches = source.Select( query ).ToList();
        Assert.HasCount( expected.Length, matches );
        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [TestMethod]
    [DataRow( "$[?(@.array in [1, 2, 3])]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.array in [1, 2, 3])]", typeof( JsonNode ) )]
    public void InOperation_ArrayNotAnArrayElement( string query, Type sourceType )
    {
        const string json =
            """
            [
                {
                    "array": [1, 2]
                },
                {
                    "array": [3, 4]
                }
            ]
            """;
        var source = GetDocumentAdapter( sourceType, json );

        var matches = source.Select( query ).ToList();
        Assert.IsEmpty( matches );
    }

    [TestMethod]
    [DataRow( "$[?(@.array in [1, [1,2], 3])]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.array in [1, [1,2], 3])]", typeof( JsonNode ) )]
    public void InOperation_ArrayIsArrayElement( string query, Type sourceType )
    {
        const string json =
            """
            [
                {
                    "array": [1, 2]
                },
                {
                    "array": [3, 4]
                }
            ]
            """;
        var source = GetDocumentAdapter( sourceType, json );
        var expected = new[] { source.FromJsonPathPointer( "$[0]" ) };

        var matches = source.Select( query ).ToList();
        Assert.HasCount( expected.Length, matches );
        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }
}
