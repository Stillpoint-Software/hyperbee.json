using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Dynamic;

public static class JsonDynamicHelper
{
    public static dynamic ConvertToDynamic( JsonNode value ) => new DynamicJsonNode( ref value );
    public static dynamic ConvertToDynamic( JsonElement value ) => new DynamicJsonElement( ref value );
    public static dynamic ConvertToDynamic( JsonDocument value ) => ConvertToDynamic( value.RootElement );
}
