using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Query;

[TestClass]
public class JsonPathRootOnScalarTests : JsonTestBase
{
    [DataTestMethod]
    [DataRow( "$", typeof(JsonDocument) )]
    [DataRow( "$", typeof(JsonNode) )]
    public void RootOnScalar( string query, Type sourceType )
    {
        const string json = "42";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();
        var expected = new[]
        {
            source.GetPropertyFromKey( "$" )
        };

        // no consensus

        Assert.IsTrue( expected.SequenceEqual( matches ) );
        Assert.IsTrue( JsonValueHelper.GetInt32( matches.First() ) == 42 );
    }

    [DataTestMethod]
    [DataRow( "$", typeof(JsonDocument) )]
    [DataRow( "$", typeof(JsonNode) )]
    public void RootOnScalarFalse( string query, Type sourceType )
    {
        const string json = "false";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();
        var expected = new[]
        {
            source.GetPropertyFromKey( "$" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
        Assert.IsTrue( JsonValueHelper.GetBoolean( matches.First() ) == false );
    }

    [DataTestMethod]
    [DataRow( "$", typeof(JsonDocument) )]
    [DataRow( "$", typeof(JsonNode) )]
    public void RootOnScalarTrue( string query, Type sourceType )
    {
        const string json = "true";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();
        var expected = new[]
        {
            source.GetPropertyFromKey( "$" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
        Assert.IsTrue( JsonValueHelper.GetBoolean( matches.First() ) );
    }
}