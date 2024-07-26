using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Descriptors.Node;

internal class NodeParserAccessor : IParserAccessor<JsonNode>
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
}
