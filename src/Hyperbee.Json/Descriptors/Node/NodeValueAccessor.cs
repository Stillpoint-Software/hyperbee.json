using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Descriptors.Node;

internal sealed class NodeValueAccessor : IValueAccessor<JsonNode>
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
    public IEnumerable<(JsonNode, string)> EnumerateObject( in JsonNode value, bool excludeValues = false )
    {
        if ( value is not JsonObject objectValue )
            return [];

        if ( !excludeValues )
            return objectValue.Select( x => (x.Value, x.Key) );

        return objectValue
            .Where( x =>
            {
                var valueKind = x.Value!.GetValueKind();
                return valueKind == JsonValueKind.Object || valueKind == JsonValueKind.Array;
            } )
            .Select( x => (x.Value, x.Key) );
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public IEnumerable<JsonNode> EnumerateArray( in JsonNode value, bool excludeValues = false )
    {
        if ( value is not JsonArray arrayValue )
            return [];

        if ( !excludeValues )
            return arrayValue;

        return arrayValue
            .Where( x =>
            {
                var valueKind = x.GetValueKind();
                return valueKind == JsonValueKind.Object || valueKind == JsonValueKind.Array;
            } );
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public int GetArrayLength( in JsonNode value )
    {
        if ( value is JsonArray jsonArray )
            return jsonArray.Count;

        return 0;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public JsonNode IndexAt( in JsonNode value, int index )
    {
        var array = (JsonArray) value;

        if ( index < 0 ) // flip negative index to positive
            index = array.Count + index;

        return value[index];
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool TryGetIndexAt( in JsonNode value, int index, out JsonNode item )
    {
        var array = (JsonArray) value;

        if ( index < 0 ) // flip negative index to positive
            index = array.Count + index;

        if ( index < array.Count )
        {
            item = value[index];
            return true;
        }

        item = null; // out of bounds
        return false;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool TryGetProperty( in JsonNode value, string propertyName, out JsonNode propertyValue )
    {
        if ( value is JsonObject valueObject && valueObject.TryGetPropertyValue( propertyName, out propertyValue ) )
            return true;

        propertyValue = default;
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
