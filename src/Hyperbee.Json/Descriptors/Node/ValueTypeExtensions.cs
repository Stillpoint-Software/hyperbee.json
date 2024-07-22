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
            var type = typeof( T );

            switch ( node )
            {
                case JsonValue jsonValue when type == typeof( string ) && jsonValue.TryGetValue( out string stringValue ):
                    value = (T) (IConvertible) stringValue;
                    return true;

                case JsonValue jsonInt when type == typeof( int ) && jsonInt.TryGetValue( out int intValue ):
                    value = (T) (IConvertible) intValue;
                    return true;

                case JsonValue jsonFloat when type == typeof( float ) && jsonFloat.TryGetValue( out float floatValue ):
                    value = (T) (IConvertible) floatValue;
                    return true;

                case JsonArray jsonArray when type == typeof( float ):
                    value = (T) (IConvertible) jsonArray.Count;
                    return true;

                case JsonObject jsonObject when type == typeof( float ):
                    value = (T) (IConvertible) jsonObject.Count;
                    return true;

                case JsonValue jsonBool when type == typeof( bool ) && jsonBool.TryGetValue( out bool boolValue ):
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
