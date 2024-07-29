using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Path.Query;

[TestClass]
public class JsonPathMathOperationsTests : JsonTestBase
{
    [DataTestMethod]
    [DataRow( "$[?(@.value + 1 == 43)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.value + 1 == 43)]", typeof( JsonNode ) )]
    [DataRow( "$[?(@.value - 1 == 41)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.value - 1 == 41)]", typeof( JsonNode ) )]
    [DataRow( "$[?(@.value * 2 == 84)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.value * 2 == 84)]", typeof( JsonNode ) )]
    [DataRow( "$[?(@.value / 2 == 21)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.value / 2 == 21)]", typeof( JsonNode ) )]
    [DataRow( "$[?(@.value % 10 == 2)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.value % 10 == 2)]", typeof( JsonNode ) )]
    public void MathOperations( string query, Type sourceType )
    {
        const string json =
            """
            [
                {
                    "value": 42
                }
            ]
            """;
        var source = GetDocumentAdapter( sourceType, json );
        var expected = new[]
        {
            source.FromJsonPathPointer( "$[0]" )
        };

        var matches = source.Select( query ).ToList();
        Assert.AreEqual( 1, matches.Count );
        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[?(@.a + @.b == 3)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.a + @.b == 3)]", typeof( JsonNode ) )]
    [DataRow( "$[?(@.a - @.b == -1)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.a - @.b == -1)]", typeof( JsonNode ) )]
    [DataRow( "$[?(@.a * @.b == 2)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.a * @.b == 2)]", typeof( JsonNode ) )]
    [DataRow( "$[?(@.a / @.b == 0.5)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.a / @.b == 0.5)]", typeof( JsonNode ) )]
    public void MathOperationsWithMultipleKeys( string query, Type sourceType )
    {
        const string json =
            """
            [
                {
                    "a": 1,
                    "b": 2
                }
            ]
            """;
        var source = GetDocumentAdapter( sourceType, json );
        var expected = new[]
        {
            source.FromJsonPathPointer( "$[0]" )
        };

        var matches = source.Select( query ).ToList();
        Assert.AreEqual( 1, matches.Count );
        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[?(@.array[0] + @.array[1] == 3)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.array[0] + @.array[1] == 3)]", typeof( JsonNode ) )]
    [DataRow( "$[?(@.array[2] - @.array[1] == 1)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.array[2] - @.array[1] == 1)]", typeof( JsonNode ) )]
    [DataRow( "$[?(@.array[1] * @.array[2] == 6)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.array[1] * @.array[2] == 6)]", typeof( JsonNode ) )]
    [DataRow( "$[?(@.array[2] / @.array[0] == 3)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.array[2] / @.array[0] == 3)]", typeof( JsonNode ) )]
    public void MathOperationsInArray( string query, Type sourceType )
    {
        const string json =
            """
            [
                {
                    "array": [1, 2, 3]
                }
            ]
            """;
        var source = GetDocumentAdapter( sourceType, json );
        var expected = new[]
        {
            source.FromJsonPathPointer( "$[0]" )
        };

        var matches = source.Select( query ).ToList();
        Assert.AreEqual( 1, matches.Count );
        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }
}
