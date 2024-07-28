using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Pointer;

namespace Hyperbee.Json.Descriptors.Node;

internal class NodeActions : INodeActions<JsonNode>
{
    public bool TryParse( ref Utf8JsonReader reader, out JsonNode node )
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

    public bool TryGetFromPointer( in JsonNode node, JsonPathSegment segment, out JsonNode value ) =>
        JsonPathPointer<JsonNode>.TryGetFromPointer( node, segment, out _, out value );

    public bool DeepEquals( JsonNode left, JsonNode right ) =>
        JsonNode.DeepEquals( left, right );

    public IEnumerable<(JsonNode Value, string Key)> GetChildren( in JsonNode value, bool complexTypesOnly = false )
    {
        // allocating is faster than using yield return and less memory intensive.
        // using stack results in fewer overall allocations than calling reverse,
        // which internally allocates, and then discards, a new array.

        switch ( value )
        {
            case JsonArray jsonArray:
                {
                    var length = jsonArray.Count;
                    var results = new Stack<(JsonNode, string)>( length ); // stack will reverse items

                    for ( var index = 0; index < length; index++ )
                    {
                        var child = value[index];

                        if ( complexTypesOnly && child is not (JsonArray or JsonObject) )
                            continue;

                        results.Push( (child, IndexHelper.GetIndexString( index )) );
                    }

                    return results;
                }
            case JsonObject jsonObject:
                {
                    var results = new Stack<(JsonNode, string)>(); // stack will reverse items
                    foreach ( var child in jsonObject )
                    {
                        if ( complexTypesOnly && child.Value is not (JsonArray or JsonObject) )
                            continue;

                        results.Push( (child.Value, child.Key) );
                    }

                    return results;
                }
        }

        return [];
    }
}
