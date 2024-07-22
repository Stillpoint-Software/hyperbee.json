using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Descriptors.Node;

internal class NodeValueAccessor : IValueAccessor<JsonNode>
{
    public IEnumerable<(JsonNode, string, SelectorKind)> EnumerateChildren( JsonNode value, bool includeValues = true )
    {
        // allocating is faster than using yield return and less memory intensive
        // because we avoid calling reverse on the enumerable (which anyway allocates a new array)

        switch ( value )
        {
            case JsonArray arrayValue:
                {
                    var length = arrayValue.Count;
                    var results = new (JsonNode, string, SelectorKind)[length];

                    var reverseIndex = length - 1;
                    for ( var index = 0; index < length; index++, reverseIndex-- )
                    {
                        var child = arrayValue[index];

                        if ( includeValues || child is JsonObject or JsonArray )
                        {
                            results[reverseIndex] = (child, index.ToString(), SelectorKind.Index);
                        }
                    }

                    return results;
                }
            case JsonObject objectValue:
                {
                    var results = new Stack<(JsonNode, string, SelectorKind)>(); // stack will reverse the list
                    foreach ( var child in objectValue )
                    {
                        if ( includeValues || child.Value is JsonObject or JsonArray )
                            results.Push( (child.Value, child.Key, SelectorKind.Name) );
                    }

                    return results;
                }
        }

        return [];
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool TryGetElementAt( in JsonNode value, int index, out JsonNode element )
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

    public bool TryGetChild( in JsonNode value, string childSelector, SelectorKind selectorKind, out JsonNode childValue )
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
                    if ( selectorKind == SelectorKind.Name )
                        break;

                    if ( int.TryParse( childSelector, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index ) )
                    {
                        if ( index < 0 ) // flip negative index to positive
                            index = valueArray.Count + index;

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

    public bool TryGetFromPointer( in JsonNode node, JsonPathSegment segment, out JsonNode childValue )
    {
        return node.TryGetFromJsonPathPointer( segment, out childValue );
    }

    // Filter methods

    public bool DeepEquals( JsonNode left, JsonNode right )
    {
        return JsonNode.DeepEquals( left, right );
    }

    public bool TryParseNode( ref Utf8JsonReader reader, out JsonNode node )
    {
        try
        {
            node = JsonNode.Parse( ref reader );
            return true;
        }
        catch
        {
            node = null;
            return false;
        }
    }

    public bool TryGetValueFromNode( JsonNode node, out IConvertible value )
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
