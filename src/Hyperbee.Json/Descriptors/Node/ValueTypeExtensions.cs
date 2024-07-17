using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Node;

public static class ValueTypeExtensions
{
    public static bool TryGetValue<T>( this IValueType input, out T value ) where T : IConvertible
    {
        switch ( input )
        {
            case ScalarValue<T> valueType:
                value = valueType.Value;
                return true;

            case NodeList<JsonNode> nodesType:
                var node = nodesType.FirstOrDefault();
                if ( node.TryConvertTo( out value ) )
                    return true;
                break;
        }

        value = default;
        return false;
    }

    public static bool TryGetNode<T>( this IValueType input, out T value )
    {
        if ( input is NodeList<T> nodes )
        {
            value = nodes.OneOrDefault();
            return true;
        }

        value = default;
        return false;
    }

    private static bool TryConvertTo<T>( this JsonNode node, out T value ) where T : IConvertible
    {
        value = default;
        try
        {
            if ( typeof( T ) == typeof( string ) && node is JsonValue jsonValue && jsonValue.TryGetValue( out string stringValue ) )
            {
                value = (T) (IConvertible) stringValue;
                return true;
            }

            if ( typeof( T ) == typeof( int ) && node is JsonValue jsonInt && jsonInt.TryGetValue( out int intValue ) )
            {
                value = (T) (IConvertible) intValue;
                return true;
            }

            if ( typeof( T ) == typeof( float ) && node is JsonValue jsonFloat && jsonFloat.TryGetValue( out float floatValue ) )
            {
                value = (T) (IConvertible) floatValue;
                return true;
            }

            if ( typeof( T ) == typeof( float ) && node is JsonArray jsonArray )
            {
                value = (T) (IConvertible) jsonArray.Count;
                return true;
            }

            if ( typeof( T ) == typeof( float ) && node is JsonObject jsonObject )
            {
                value = (T) (IConvertible) jsonObject.Count;
                return true;
            }

            if ( typeof( T ) == typeof( bool ) && node is JsonValue jsonBool && jsonBool.TryGetValue( out bool boolValue ) )
            {
                value = (T) (IConvertible) boolValue;
                return true;
            }
        }
        catch
        {
            // Ignore conversion errors
        }

        return false;
    }
}
