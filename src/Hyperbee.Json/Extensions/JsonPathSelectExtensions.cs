using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Evaluators;

namespace Hyperbee.Json.Extensions;

public static class JsonPathSelectExtensions
{
    // JsonElement

    public static IEnumerable<JsonElement> Select( this JsonElement element, string query )
    {
        return new JsonPath( null ).Select( element, query );
    }

    public static IEnumerable<JsonElement> Select( this JsonElement element, string query, IJsonPathFilterEvaluator<JsonElement> evaluator )
    {
        return new JsonPath( evaluator ).Select( element, query );
    }

    // JsonDocument

    public static IEnumerable<JsonElement> Select( this JsonDocument document, string query )
    {
        return new JsonPath( null ).Select( document.RootElement, query );
    }

    public static IEnumerable<JsonElement> Select( this JsonDocument document, string query, IJsonPathFilterEvaluator<JsonElement> evaluator )
    {
        return new JsonPath( evaluator ).Select( document.RootElement, query );
    }

    // JsonNode

    public static IEnumerable<JsonNode> Select( this JsonNode node, string query )
    {
        return new Nodes.JsonPathNode( null ).Select( node, query );
    }

    public static IEnumerable<JsonNode> Select( this JsonNode node, string query, IJsonPathFilterEvaluator<JsonNode> evaluator )
    {
        return new Nodes.JsonPathNode( evaluator ).Select( node, query );
    }
}

