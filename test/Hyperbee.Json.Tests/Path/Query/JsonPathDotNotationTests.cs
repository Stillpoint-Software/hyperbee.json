using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Path.Query;

[TestClass]
public class JsonPathDotNotationTests : JsonTestBase
{
    [TestMethod]
    [DataRow( "$.[key]", typeof( JsonDocument ) )]
    [DataRow( "$.[key]", typeof( JsonNode ) )]
    public void DotBracketNotationWithoutQuotes( string query, Type sourceType )
    {
        const string json = """
        {
            "key": "value",
            "other": {
                "key": [
                    {
                        "key": 42
                    }
                ]
            }
        }
        """;
        var source = GetDocumentAdapter( sourceType, json );

        Assert.ThrowsExactly<NotSupportedException>( () =>
        {
            _ = source.Select( query ).ToList();
        } );
    }

    [TestMethod]
    [DataRow( "$.", typeof( JsonDocument ) )]
    [DataRow( "$.", typeof( JsonNode ) )]
    public void DotBracketNotationWithEmptyPath( string query, Type sourceType )
    {
        const string json = """
        {
            "key": 42,
            "": 9001,
            "''": "nice"
        }
        """;
        var source = GetDocumentAdapter( sourceType, json );

        Assert.ThrowsExactly<NotSupportedException>( () =>
        {
            _ = source.Select( query ).ToList();
        } );
    }

    [TestMethod]
    [DataRow( "$.屬性", typeof( JsonDocument ) )]
    [DataRow( "$.屬性", typeof( JsonNode ) )]
    public void DotNotationWithNonAsciiKey( string query, Type sourceType )
    {
        // consensus: none

        const string json = """
        {
            "\u5c6c\u6027": "value"
        }
        """;
        var source = GetDocumentAdapter( sourceType, json );

        var matches = source.Select( query ).ToList();
        var expected = new[]
        {
            source.FromJsonPathPointer("$['屬性']")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
        Assert.IsTrue( TestHelper.GetString( matches[0] ) == "value" );
    }

    [TestMethod]
    [DataRow( "$a", typeof( JsonDocument ) )]
    [DataRow( "$a", typeof( JsonNode ) )]
    public void DotNotationWithoutDot( string query, Type sourceType )
    {
        const string json = """
        {
            "a": 1,
            "$a": 2
        }
        """;
        var source = GetDocumentAdapter( sourceType, json );

        Assert.ThrowsExactly<NotSupportedException>( () =>
        {
            _ = source.Select( query ).ToList();
        } );
    }
}
