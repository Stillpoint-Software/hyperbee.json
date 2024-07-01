using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Descriptors.Node;

internal class NodeValueAccessor : IValueAccessor<JsonNode>
{
    public IEnumerable<(JsonNode, string, SelectorKind)> EnumerateChildren( JsonNode value, bool includeValues = true )
    {
        switch ( value )
        {
            case JsonArray arrayValue:
                for ( var index = arrayValue.Count - 1; index >= 0; index-- )

                {
                    var child = arrayValue[index];

                    if ( includeValues || child is JsonObject or JsonArray )
                        yield return (child, index.ToString(), SelectorKind.Index);
                }

                break;
            case JsonObject objectValue:

                if ( includeValues )
                {
                    foreach ( var child in objectValue.Reverse() )
                        yield return (child.Value, child.Key, SelectorKind.Name);
                }
                else
                {
                    foreach ( var child in objectValue.Where( property => property.Value is JsonObject or JsonArray ).Reverse() )
                        yield return (child.Value, child.Key, SelectorKind.Name);
                }

                break;
        }
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public JsonNode GetElementAt( in JsonNode value, int index )
    {
        return value[index];
    }

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
    public int GetArrayLength( in JsonNode value )
    {
        if ( value is JsonArray jsonArray )
            return jsonArray.Count;

        return 0;
    }

    public bool TryGetChildValue( in JsonNode value, string childSelector, out JsonNode childValue )
    {
        switch ( value )
        {
            case JsonObject valueObject:
                {
                    if ( valueObject.TryGetPropertyValue( childSelector, out childValue ) )
                        return true;

                    break;
                }
            case JsonArray valueArray:
                {
                    if ( int.TryParse( childSelector, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index ) )
                    {
                        if ( index >= 0 && index < valueArray.Count )
                        {
                            childValue = value[index];
                            return true;
                        }
                    }

                    break;
                }
            default:
                {
                    if ( !IsPathOperator( childSelector ) )
                        throw new ArgumentException( $"Invalid child type '{childSelector}'. Expected child to be Object, Array or a path selector.", nameof( value ) );

                    break;
                }
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


    public bool TryGetObjects( ReadOnlySpan<char> item, out IEnumerable<JsonNode> nodes )
    {
        try
        {
            var json = JsonNode.Parse( item.ToString() );
            nodes = [json];
            return true;
        }
        catch
        {
            nodes = [];
            return false;
        }
    }

    public bool DeepEquals( JsonNode left, JsonNode right )
    {
        return JsonNode.DeepEquals( left, right );
    }

    public bool TryGetValueFromNode( JsonNode node, out object value )
    {
        switch ( node?.GetValueKind() )
        {
            case JsonValueKind.String:
                value = node.GetValue<string>();
                break;
            case JsonValueKind.Number:
                value = node.GetValue<float>();
                break;
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
