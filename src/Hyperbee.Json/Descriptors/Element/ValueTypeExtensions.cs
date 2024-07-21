using System.Text.Json;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Element;

public static class ValueTypeExtensions
{
    public static bool TryGetValue<T>( this IValueType input, out T value ) where T : IConvertible
    {
        switch ( input )
        {
            case ScalarValue<T> valueType:
                value = valueType.Value;
                return true;

            case NodeList<JsonElement> nodesType:
                var element = nodesType.FirstOrDefault();
                if ( element.TryConvertTo( out value ) )
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

    private static bool TryConvertTo<T>( this JsonElement element, out T value ) where T : IConvertible
    {
        value = default;

        try
        {
            var type = typeof( T );

            switch ( element.ValueKind )
            {
                case JsonValueKind.String when type == typeof( string ):
                    value = (T) (IConvertible) element.GetString();
                    return true;

                case JsonValueKind.Number when type == typeof( int ) && element.TryGetInt32( out var intValue ):
                    value = (T) (IConvertible) intValue;
                    return true;

                case JsonValueKind.Number when type == typeof( float ) && element.TryGetSingle( out var floatValue ):
                    value = (T) (IConvertible) floatValue;
                    return true;

                case JsonValueKind.True when type == typeof( bool ):
                    value = (T) (IConvertible) true;
                    return true;

                case JsonValueKind.False when type == typeof( bool ):
                    value = (T) (IConvertible) false;
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
