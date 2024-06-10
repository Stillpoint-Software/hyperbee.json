using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Extensions;

public static class JsonPathSelectExtensions
{
    public static IEnumerable<JsonElement> Select( this JsonElement element, string query )
    {
        return new JsonPath().Select( element, query );
    }

    public static IEnumerable<JsonElement> Select( this JsonDocument document, string query )
    {
        return new JsonPath().Select( document.RootElement, query );
    }

    public static IEnumerable<JsonNode> Select( this JsonNode node, string query )
    {
        return new Nodes.JsonPathNode().Select( node, query );
    }
}

