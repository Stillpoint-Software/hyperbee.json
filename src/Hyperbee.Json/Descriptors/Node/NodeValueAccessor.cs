using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Descriptors.Node;

internal class NodeValueAccessor : IValueAccessor<JsonNode>
{

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public NodeKind GetNodeKind( in JsonNode value )
    {
        return value switch
        {
            JsonArray => NodeKind.Array,
            JsonObject => NodeKind.Object,
            _ => NodeKind.Value
        };
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public IEnumerable<(JsonNode, string)> EnumerateObject( JsonNode value )
    {
        return value is JsonObject objectValue
            ? objectValue.Select( x => (x.Value, x.Key) )
            : [];
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public IEnumerable<JsonNode> EnumerateArray( JsonNode value )
    {
        return value as JsonArray ?? [];
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public int GetArrayLength( in JsonNode value )
    {
        if ( value is JsonArray jsonArray )
            return jsonArray.Count;

        return 0;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool TryGetIndexAt( in JsonNode value, int index, out JsonNode element )
    {
        var array = (JsonArray) value;
        element = null;

        if ( index < 0 ) // flip negative index to positive
            index = array.Count + index;

        if ( index < 0 || index >= array.Count ) // out of bounds
            return false;

        element = value[index];
        return true;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool TryGetProperty( in JsonNode value, string childSelector, out JsonNode childValue )
    {
        if ( value is JsonObject valueObject && valueObject.TryGetPropertyValue( childSelector, out childValue ) )
            return true;

        childValue = default;
        return false;
    }

    public bool TryGetValue( JsonNode node, out IConvertible value )
    {
        switch ( node?.GetValueKind() )
        {
            case JsonValueKind.String:
                value = node.GetValue<string>();
                break;

            case JsonValueKind.Number:
                if ( node is JsonValue jsonValue )
                {
                    if ( jsonValue.TryGetValue( out int intValue ) )
                    {
                        value = intValue;
                        break;
                    }

                    if ( jsonValue.TryGetValue( out float floatValue ) )
                    {
                        value = floatValue;
                        break;
                    }
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
            case null:
                value = null;
                break;
            default:
                value = false;
                return false;
        }

        return true;
    }
}
