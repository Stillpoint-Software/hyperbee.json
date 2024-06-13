using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Extensions;

public static class JsonPathSelectExtensions
{
    public static IEnumerable<JsonElement> Select( this JsonElement element, string query )
    {
        return JsonPath<JsonElement>.Select( element, query );
    }

    public static IEnumerable<JsonElement> Select( this JsonDocument document, string query )
    {
        return JsonPath<JsonElement>.Select( document.RootElement, query );
    }

    public static IEnumerable<JsonNode> Select( this JsonNode node, string query )
    {
        return JsonPath<JsonNode>.Select( node, query );
    }
}
