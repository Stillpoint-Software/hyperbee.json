﻿using System.Collections.Generic;
using System.Text.Json.Nodes;
using Hyperbee.Json.Descriptors.Node;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Parser.Expressions;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Query;

[TestClass]
public class JsonComparerComparandTests : JsonTestBase
{
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
    public void ComparandWithEqualResults( object left, object right, bool areEqual )
    {
        var context = new FilterContext<JsonNode>( new NodeTypeDescriptor() );

        var a = new ComparerExpressionFactory<JsonNode>.Comparand( context, left );
        var b = new ComparerExpressionFactory<JsonNode>.Comparand( context, right );

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

    public void ComparandWithGreaterResults( object left, object right, bool areEqual )
    {
        var context = new FilterContext<JsonNode>( new NodeTypeDescriptor() );

        var a = new ComparerExpressionFactory<JsonNode>.Comparand( context, left );
        var b = new ComparerExpressionFactory<JsonNode>.Comparand( context, right );

        var result = a >= b;

        Assert.AreEqual( areEqual, result );
    }

    [DataTestMethod]
    [DataRow( """{ "value": 1 }""", 99F, false )]
    [DataRow( """{ "value": 99 }""", 99F, true )]
    [DataRow( """{ "value": "hello" }""", "world", false )]
    [DataRow( """{ "value": "hello" }""", "hello", true )]
    [DataRow( """{ "value": { "child": 5 } }""", "hello", false )]
    public void ComparandWithJsonObjectResults( string left, object right, bool areEqual )
    {
        var context = new FilterContext<JsonNode>( new NodeTypeDescriptor() );
        var node = new List<JsonNode> { JsonNode.Parse( left )!["value"] };

        var a = new ComparerExpressionFactory<JsonNode>.Comparand( context, node );
        var b = new ComparerExpressionFactory<JsonNode>.Comparand( context, right );

        var result = a == b;

        Assert.AreEqual( areEqual, result );
    }


    [DataTestMethod]
    [DataRow( """[1,2,3]""", 2F, true )]
    [DataRow( """["hello","hi","world" ]""", "hi", true )]
    [DataRow( """[1,2,3]""", 99F, false )]
    [DataRow( """["hello","world" ]""", "hi", false )]
    public void ComparandWithLeftJsonArray( string left, object right, bool areEqual )
    {
        var context = new FilterContext<JsonNode>( new NodeTypeDescriptor() );

        var a = new ComparerExpressionFactory<JsonNode>.Comparand( context, JsonNode.Parse( left ) );
        var b = new ComparerExpressionFactory<JsonNode>.Comparand( context, right );

        var result = a == b;

        Assert.AreEqual( areEqual, result );
    }

    [DataTestMethod]
    [DataRow( 2F, """[1,2,3]""", true )]
    [DataRow( "hi", """["hello","hi","world" ]""", true )]
    [DataRow( 99F, """[1,2,3]""", false )]
    [DataRow( "hi", """["hello","world" ]""", false )]
    public void ComparandWithRightJsonArray( object left, string right, bool areEqual )
    {
        var context = new FilterContext<JsonNode>( new NodeTypeDescriptor() );

        var a = new ComparerExpressionFactory<JsonNode>.Comparand( context, left );
        var b = new ComparerExpressionFactory<JsonNode>.Comparand( context, JsonNode.Parse( right ) );

        var result = a == b;

        Assert.AreEqual( areEqual, result );
    }

    [TestMethod]
    public void ComparandWithEmpty()
    {
        var context = new FilterContext<JsonNode>( new NodeTypeDescriptor() );

        var a = new ComparerExpressionFactory<JsonNode>.Comparand( context, new List<JsonNode>() );
        var b = new ComparerExpressionFactory<JsonNode>.Comparand( context, 1 );

        Assert.IsFalse( a < b );
        Assert.IsFalse( a <= b );

        Assert.IsFalse( a > b );
        Assert.IsFalse( a >= b );

        Assert.IsFalse( a == b );
        Assert.IsTrue( a != b );
    }
}

