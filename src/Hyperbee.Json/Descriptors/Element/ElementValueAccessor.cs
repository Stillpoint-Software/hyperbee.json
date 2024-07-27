using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Hyperbee.Json.Descriptors.Element;

internal sealed class ElementValueAccessor : IValueAccessor<JsonElement>
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
    public IEnumerable<(JsonElement, string)> EnumerateObject( in JsonElement value, bool excludeValues = false )
    {
        if ( value.ValueKind != JsonValueKind.Object )
            return [];

        return !excludeValues
            ? value.EnumerateObject().Select( x => (x.Value, x.Name) )
            : value.EnumerateObject()
                .Where( x => x.Value.ValueKind == JsonValueKind.Object || x.Value.ValueKind == JsonValueKind.Array )
                .Select( x => (x.Value, x.Name) );
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public IEnumerable<JsonElement> EnumerateArray( in JsonElement value, bool excludeValues = false )
    {
        if ( value.ValueKind != JsonValueKind.Array )
            return [];
        
        return !excludeValues
            ? value.EnumerateArray()
            : value.EnumerateArray()
                .Where( x => x.ValueKind == JsonValueKind.Object || x.ValueKind == JsonValueKind.Array );
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public int GetArrayLength( in JsonElement value )
    {
        return value.ValueKind == JsonValueKind.Array
            ? value.GetArrayLength()
            : 0;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public JsonElement IndexAt( in JsonElement value, int index )
    {
        if ( index < 0 ) // flip negative index to positive
            index = value.GetArrayLength() + index;

        return value[index];
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool TryGetIndexAt( in JsonElement value, int index, out JsonElement item )
    {
        if ( index < 0 ) // flip negative index to positive
            index = value.GetArrayLength() + index;

        if ( index < value.GetArrayLength() ) 
        {
            item = value[index];
            return true;
        }

        item = default; // out of bounds
        return false;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool TryGetProperty( in JsonElement value, string propertyName, out JsonElement propertyValue )
    {
        if ( value.ValueKind == JsonValueKind.Object && value.TryGetProperty( propertyName, out propertyValue ) )
            return true;

        propertyValue = default;
        return false;
    }

    public bool TryGetValue( JsonElement node, out IConvertible value )
    {
        switch ( node.ValueKind )
        {
            case JsonValueKind.String:
                value = node.GetString();
                break;
            case JsonValueKind.Number:
                if ( node.TryGetInt32( out int intValue ) )
                {
                    value = intValue;
                    break;
                }

                if ( node.TryGetSingle( out float floatValue ) )
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
