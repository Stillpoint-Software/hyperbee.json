using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Core;

public static class JsonNodeFactory
{
    public static JsonNode Create( in JsonElement element )
    {
        // use of Json*.Create results in a 20x performance increase.
        // the internal implementation of create, makes a JsonNode
        // that is backed by JsonElement.

        return element.ValueKind switch
        {
            JsonValueKind.Object => JsonObject.Create( element ),
            JsonValueKind.Array => JsonArray.Create( element ),
            _ => JsonValue.Create( element )
        };
    }
}
