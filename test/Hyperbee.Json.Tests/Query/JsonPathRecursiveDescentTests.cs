using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Query;

[TestClass]
public class JsonPathRecursiveDescentTests : JsonTestBase
{
    [DataTestMethod]
    [DataRow( "$..", typeof( JsonDocument ) )]
    [DataRow( "$..", typeof( JsonNode ) )]
    [ExpectedException( typeof( NotSupportedException ) )]
    public void RecursiveDescent( string query, Type sourceType )
    {
        // no consensus

        const string json = """
        [
            {"a": {"b": "c"}},
            [0, 1]
        ]
        """;

        var source = GetDocumentFromSource( sourceType, json );

        _ = source.Select( query ).ToList();
    }

    [DataTestMethod]
    [DataRow( "$..*", typeof( JsonDocument ) )]
    [DataRow( "$..*", typeof( JsonNode ) )]
    public void RecursiveDescentOnNestedArrays( string query, Type sourceType )
    {
        const string json = """
        [
            [0],
            [1]
        ]
        """;

        var source = GetDocumentFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();
        var expected = new[] { source.GetPropertyFromPath( "$[0]" ), source.GetPropertyFromPath( "$[1]" ), source.GetPropertyFromPath( "$[0][0]" ), source.GetPropertyFromPath( "$[1][0]" ) };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$.key..", typeof( JsonDocument ) )]
    [DataRow( "$.key..", typeof( JsonNode ) )]
    [ExpectedException( typeof( NotSupportedException ) )]
    public void RecursiveDescentAfterDotNotation( string query, Type sourceType )
    {
        //consensus: NOT_SUPPORTED

        const string json = """
        {
            "some key": "value",
            "key": {
                "complex": "string",
                "primitives": [0, 1]
            }
        }
        """;

        var source = GetDocumentFromSource( sourceType, json );

        _ = source.Select( query ).ToList();
    }
}
