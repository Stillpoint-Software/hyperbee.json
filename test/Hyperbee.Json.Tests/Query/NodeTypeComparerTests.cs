﻿using System.Collections.Generic;
using System.Text.Json.Nodes;
using Hyperbee.Json.Descriptors.Node;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Query;

[TestClass]
public class NodeTypeComparerTests : JsonTestBase
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
    public void NodeTypeComparer_ShouldCompare_WithEqualResults( object left, object right, bool areEqual )
    {
        // Arrange
        var comparer = GetComparer();
        var a = GetNodeType( left );
        var b = GetNodeType( right );

        // Act
        var result = comparer.Compare( a, b, Operator.Equals ) == 0;

        // Assert
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
    public void NodeTypeComparer_ShouldCompare_WithGreaterResults( object left, object right, bool areEqual )
    {
        // Arrange
        var comparer = GetComparer();
        var a = GetNodeType( left );
        var b = GetNodeType( right );

        // Act
        var result = comparer.Compare( a, b, Operator.GreaterThanOrEqual ) >= 0;

        // Assert
        Assert.AreEqual( areEqual, result );
    }

    [DataTestMethod]
    [DataRow( """{ "value": 1 }""", 99F, false )]
    [DataRow( """{ "value": 99 }""", 99F, true )]
    [DataRow( """{ "value": "hello" }""", "world", false )]
    [DataRow( """{ "value": "hello" }""", "hello", true )]
    [DataRow( """{ "value": { "child": 5 } }""", "hello", false )]
    public void NodeTypeComparer_ShouldCompare_WithJsonObjectResults( string left, object right, bool areEqual )
    {
        // Arrange
        var comparer = GetComparer();
        var a = GetNodeType( new List<JsonNode> { JsonNode.Parse( left )!["value"] } );
        var b = GetNodeType( right );

        // Act
        var result = comparer.Compare( a, b, Operator.GreaterThanOrEqual ) == 0;

        // Assert
        Assert.AreEqual( areEqual, result );
    }

    [DataTestMethod]
    [DataRow( """[1,2,3]""", 2F, true )]
    [DataRow( """["hello","hi","world" ]""", "hi", true )]
    [DataRow( """[1,2,3]""", 99F, false )]
    [DataRow( """["hello","world" ]""", "hi", false )]
    public void NodeTypeComparer_ShouldCompare_WithLeftJsonArray( string left, object right, bool areEqual )
    {
        // Arrange
        var comparer = GetComparer();
        var a = GetNodeType( JsonNode.Parse( left )!.AsArray() );
        var b = GetNodeType( right );

        // Act
        var result = comparer.Compare( a, b, Operator.GreaterThanOrEqual ) == 0;

        // Assert
        Assert.AreEqual( areEqual, result );
    }

    [DataTestMethod]
    [DataRow( 2F, """[1,2,3]""", true )]
    [DataRow( "hi", """["hello","hi","world" ]""", true )]
    [DataRow( 99F, """[1,2,3]""", false )]
    [DataRow( "hi", """["hello","world" ]""", false )]
    public void NodeTypeComparer_ShouldCompare_WithRightJsonArray( object left, string right, bool areEqual )
    {
        // Arrange
        var comparer = GetComparer();
        var a = GetNodeType( left );
        var b = GetNodeType( JsonNode.Parse( right )!.AsArray() );

        // Act
        var result = comparer.Compare( a, b, Operator.GreaterThanOrEqual ) == 0;

        // Assert
        Assert.AreEqual( areEqual, result );
    }

    [TestMethod]
    public void NodeTypeComparer_ShouldCompare_WithEmpty()
    {
        var comparer = GetComparer();
        var a = new NodesType<JsonNode>( [], true );
        var b = new ValueType<float>( 1F );

        Assert.IsFalse( comparer.Compare( a, b, Operator.LessThan ) < 0 );
        Assert.IsFalse( comparer.Compare( a, b, Operator.LessThanOrEqual ) <= 0 );

        Assert.IsFalse( comparer.Compare( a, b, Operator.GreaterThan ) > 0 );
        Assert.IsFalse( comparer.Compare( a, b, Operator.GreaterThanOrEqual ) >= 0 );

        Assert.IsFalse( comparer.Compare( a, b, Operator.Equals ) == 0 );
        Assert.IsTrue( comparer.Compare( a, b, Operator.NotEquals ) != 0 );
    }

    private static NodeTypeComparer<JsonNode> GetComparer() => new( new NodeValueAccessor() );

    private static INodeType GetNodeType( object item ) =>
        item switch
        {
            string itemString => new ValueType<string>( itemString ),
            float itemFloat => new ValueType<float>( itemFloat ),
            bool itemBool => new ValueType<bool>( itemBool ),
            IEnumerable<JsonNode> nodes => new NodesType<JsonNode>( nodes, true ),
            _ => Constants.Nothing
        };
}
