using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Descriptors.Element;

internal class ElementValueAccessor : IValueAccessor<JsonElement>
{
    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool TryGetElementAt( in JsonElement value, int index, out JsonElement element )
    {
        element = default;

        if ( index < 0 ) // flip negative index to positive
            index = value.GetArrayLength() + index;

        if ( index < 0 || index >= value.GetArrayLength() ) // out of bounds
            return false;

        element = value[index];
        return true;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public NodeKind GetNodeKind( in JsonElement value )
    {
        return value.ValueKind switch
        {
            JsonValueKind.Object => NodeKind.Object,
            JsonValueKind.Array => NodeKind.Array,
            _ => NodeKind.Value
        };
    }

    public IEnumerable<(JsonElement, string)> EnumerateObject( JsonElement value )
    {
        return value.ValueKind == JsonValueKind.Object
            ? value.EnumerateObject().Select( x => (x.Value, x.Name) )
            : [];
    }

    public IEnumerable<(JsonElement, int)> EnumerateArray( JsonElement value )
    {
        return value.ValueKind == JsonValueKind.Array
            ? value.EnumerateArray().Select( ( x, i ) => (x, i) )
            : [];
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public int GetArrayLength( in JsonElement value )
    {
        return value.ValueKind == JsonValueKind.Array
            ? value.GetArrayLength()
            : 0;
    }

    public bool TryGetChild( in JsonElement value, string childSelector, out JsonElement childValue )
    {
        switch ( value.ValueKind )
        {
            case JsonValueKind.Object:
                if ( value.TryGetProperty( childSelector, out childValue ) )
                    return true;
                break;

            case JsonValueKind.Array:
                if ( int.TryParse( childSelector, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index ) )
                {
                    var arrayLength = value.GetArrayLength();

                    if ( index < 0 ) // flip negative index to positive
                        index = arrayLength + index;

                    if ( index >= 0 && index < arrayLength )
                    {
                        childValue = value[index];
                        return true;
                    }
                }

                break;

            default:
                if ( !IsPathOperator( childSelector ) )
                    throw new ArgumentException( $"Invalid child type '{childSelector}'. Expected child to be Object, Array or a path selector.", nameof( value ) );
                break;
        }

        childValue = default;
        return false;

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static bool IsPathOperator( ReadOnlySpan<char> x )
        {
            return x.Length switch
            {
                1 => x[0] == '*',
                2 => x[0] == '.' && x[1] == '.',
                3 => x[0] == '$',
                _ => false
            };
        }
    }

    public bool TryGetValue( JsonElement element, out IConvertible value )
    {
        switch ( element.ValueKind )
        {
            case JsonValueKind.String:
                value = element.GetString();
                break;
            case JsonValueKind.Number:
                if ( element.TryGetInt32( out int intValue ) )
                {
                    value = intValue;
                    break;
                }

                if ( element.TryGetSingle( out float floatValue ) )
                {
                    value = floatValue;
                    break;
                }

                value = false;
                return false;

            case JsonValueKind.True:
                value = true;
                break;
            case JsonValueKind.False:
                value = false;
                break;
            case JsonValueKind.Null:
                value = null;
                break;
            default:
                value = false;
                return false;
        }

        return true;
    }

    public bool TryGetFromPointer( in JsonElement element, JsonPathSegment segment, out JsonElement childValue )
    {
        return element.TryGetFromJsonPathPointer( segment, out childValue );
    }

    // Filter Methods

    public bool DeepEquals( JsonElement left, JsonElement right )
    {
        return left.DeepEquals( right );
    }

}
