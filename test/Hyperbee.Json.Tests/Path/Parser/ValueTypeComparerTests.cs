using System.Collections.Generic;
using System.Text.Json.Nodes;
using Hyperbee.Json.Path.Filters.Parser;
using Hyperbee.Json.Path.Filters.Values;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Path.Parser;

[TestClass]
public class NodeTypeComparerTests : JsonTestBase
{
    [TestMethod]
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
    public void Compare_WithEqualResults( object left, object right, bool areEqual )
    {
        // Arrange
        var comparer = GetComparer();
        var a = GetNodeValue( left );
        var b = GetNodeValue( right );

        // Act
        var result = comparer.Compare( a, b, Operator.Equals ) == 0;

        // Assert
        Assert.AreEqual( areEqual, result );
    }

    [TestMethod]
    [DataRow( true, true, true )]
    [DataRow( false, false, true )]
    [DataRow( false, true, false )]
    [DataRow( true, false, true )]
    [DataRow( "hello", "hello", true )]
    [DataRow( 10F, 10F, true )]
    [DataRow( 14F, 10F, true )]
    [DataRow( 1F, 14F, false )]
    public void Compare_WithGreaterResults( object left, object right, bool areEqual )
    {
        // Arrange
        var comparer = GetComparer();
        var a = GetNodeValue( left );
        var b = GetNodeValue( right );

        // Act
        var result = comparer.Compare( a, b, Operator.GreaterThanOrEqual ) >= 0;

        // Assert
        Assert.AreEqual( areEqual, result );
    }

    [TestMethod]
    [DataRow( """{ "value": 1 }""", 99F, false )]
    [DataRow( """{ "value": 99 }""", 99F, true )]
    [DataRow( """{ "value": "hello" }""", "world", false )]
    [DataRow( """{ "value": "hello" }""", "hello", true )]
    [DataRow( """{ "value": { "child": 5 } }""", "hello", false )]
    public void Compare_WithJsonObjectResults( string left, object right, bool areEqual )
    {
        // Arrange
        var comparer = GetComparer();
        var a = GetNodeValue( new List<JsonNode> { JsonNode.Parse( left )!["value"] } );
        var b = GetNodeValue( right );

        // Act
        var result = comparer.Compare( a, b, Operator.GreaterThanOrEqual ) == 0;

        // Assert
        Assert.AreEqual( areEqual, result );
    }

    [TestMethod]
    [DataRow( """[1,2,3]""", 2F, true )]
    [DataRow( """["hello","hi","world" ]""", "hi", true )]
    [DataRow( """[1,2,3]""", 99F, false )]
    [DataRow( """["hello","world" ]""", "hi", false )]
    public void Compare_WithLeftJsonArray( string left, object right, bool areEqual )
    {
        // Arrange
        var comparer = GetComparer();
        var a = GetNodeValue( JsonNode.Parse( left )!.AsArray() );
        var b = GetNodeValue( right );

        // Act
        var result = comparer.Compare( a, b, Operator.GreaterThanOrEqual ) == 0;

        // Assert
        Assert.AreEqual( areEqual, result );
    }

    [TestMethod]
    [DataRow( 2F, """[1,2,3]""", true )]
    [DataRow( "hi", """["hello","hi","world" ]""", true )]
    [DataRow( 99F, """[1,2,3]""", false )]
    [DataRow( "hi", """["hello","world" ]""", false )]
    public void Compare_WithRightJsonArray( object left, string right, bool areEqual )
    {
        // Arrange
        var comparer = GetComparer();
        var a = GetNodeValue( left );
        var b = GetNodeValue( JsonNode.Parse( right )!.AsArray() );

        // Act
        var result = comparer.Compare( a, b, Operator.GreaterThanOrEqual ) == 0;

        // Assert
        Assert.AreEqual( areEqual, result );
    }

    [TestMethod]
    public void Compare_WithEmpty()
    {
        var comparer = GetComparer();
        var a = new NodeList<JsonNode>( [], true );
        var b = new ScalarValue<float>( 1F );

        Assert.IsFalse( comparer.Compare( a, b, Operator.LessThan ) < 0 );
        Assert.IsFalse( comparer.Compare( a, b, Operator.LessThanOrEqual ) <= 0 );

        Assert.IsFalse( comparer.Compare( a, b, Operator.GreaterThan ) > 0 );
        Assert.IsFalse( comparer.Compare( a, b, Operator.GreaterThanOrEqual ) >= 0 );

        Assert.IsFalse( comparer.Compare( a, b, Operator.Equals ) == 0 );
        Assert.IsTrue( comparer.Compare( a, b, Operator.NotEquals ) != 0 );
    }

    // Helper methods

    private static ValueTypeComparer<JsonNode> GetComparer() => new();

    private static IValueType GetNodeValue( object item )
    {
        return item switch
        {
            string itemString => Scalar.Value( itemString ),
            float itemFloat => Scalar.Value( itemFloat ),
            bool itemBool => Scalar.Value( itemBool ),
            IEnumerable<JsonNode> nodes => new NodeList<JsonNode>( nodes, true ),
            _ => Scalar.Nothing
        };
    }
}
