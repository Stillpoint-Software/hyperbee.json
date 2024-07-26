using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

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

    public bool TryGetFromPointer( in JsonNode node, JsonPathSegment segment, out JsonNode childValue ) =>
        node.TryGetFromJsonPathPointer( segment, out childValue );

    public bool DeepEquals( JsonNode left, JsonNode right ) =>
        JsonNode.DeepEquals( left, right );
}
