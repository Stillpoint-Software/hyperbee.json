using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Hyperbee.Json.Descriptors.Element;

internal class ElementValueAccessor : IValueAccessor<JsonElement>
{
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

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public IEnumerable<(JsonElement, string)> EnumerateObject( JsonElement value )
    {
        return value.ValueKind == JsonValueKind.Object
            ? value.EnumerateObject().Select( x => (x.Value, x.Name) )
            : [];
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public IEnumerable<JsonElement> EnumerateArray( JsonElement value )
    {
        return value.ValueKind == JsonValueKind.Array
            ? value.EnumerateArray()
            : [];
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public int GetArrayLength( in JsonElement value )
    {
        return value.ValueKind == JsonValueKind.Array
            ? value.GetArrayLength()
            : 0;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool TryGetIndexAt( in JsonElement value, int index, out JsonElement element )
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
    public bool TryGetProperty( in JsonElement value, string childSelector, out JsonElement childValue )
    {
        if ( value.ValueKind == JsonValueKind.Object && value.TryGetProperty( childSelector, out childValue ) )
            return true;

        childValue = default;
        return false;
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
}
