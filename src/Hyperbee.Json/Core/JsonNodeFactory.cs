using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Core;

public static class JsonNodeFactory
{
    public static JsonNode Create( in JsonElement element )
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => JsonObject.Create( element ),
            JsonValueKind.Array => JsonArray.Create( element ),
            _ => JsonValue.Create( element )
        };
    }
}
