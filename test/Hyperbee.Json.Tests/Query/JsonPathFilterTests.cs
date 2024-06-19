using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
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
    public void FilterWithTruthyProperty( string query, Type sourceType )
    {
        const string json =
            """
            [
              {"some": "some value"}, 
              {"key": "value"}
            ]
            """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$[1]" )
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

        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$[0]" ),
            source.GetPropertyFromKey( "$[2]" ),
            source.GetPropertyFromKey( "$[3]" ),
            source.GetPropertyFromKey( "$[6]" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }
}
