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
    [DataRow( "$", typeof( JsonDocument ) )]
    [DataRow( "$", typeof( JsonNode ) )]
    public void RootOnScalar( string query, Type sourceType )
    {
        // consensus: none

        const string json = "42";
        var source = GetDocumentAdapter( sourceType, json );

        var matches = source.Select( query ).ToList();
        var expected = new[]
        {
            source.FromJsonPathPointer( "$" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
        Assert.IsTrue( TestHelper.GetInt32( matches.First() ) == 42 );
    }

    [DataTestMethod]
    [DataRow( "$", typeof( JsonDocument ) )]
    [DataRow( "$", typeof( JsonNode ) )]
    public void RootOnScalarFalse( string query, Type sourceType )
    {
        const string json = "false";
        var source = GetDocumentAdapter( sourceType, json );

        var matches = source.Select( query ).ToList();
        var expected = new[]
        {
            source.FromJsonPathPointer( "$" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
        Assert.IsTrue( TestHelper.GetBoolean( matches.First() ) == false );
    }

    [DataTestMethod]
    [DataRow( "$", typeof( JsonDocument ) )]
    [DataRow( "$", typeof( JsonNode ) )]
    public void RootOnScalarTrue( string query, Type sourceType )
    {
        const string json = "true";
        var source = GetDocumentAdapter( sourceType, json );

        var matches = source.Select( query ).ToList();
        var expected = new[]
        {
            source.FromJsonPathPointer( "$" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
        Assert.IsTrue( TestHelper.GetBoolean( matches.First() ) );
    }
}
