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

    public static IEnumerable<JsonElement> Select( this JsonElement element, string query, JsonPathEvaluator<JsonElement> evaluator )
    {
        return new JsonPath( new JsonPathFuncEvaluator<JsonElement>( evaluator ) ).Select( element, query );
    }

    public static IEnumerable<JsonElement> Select( this JsonDocument document, string query )
    {
        return new JsonPath( null ).Select( document.RootElement, query );
    }

    public static IEnumerable<JsonElement> Select( this JsonDocument document, string query, IJsonPathFilterEvaluator<JsonElement> evaluator )
    {
        return new JsonPath( evaluator ).Select( document.RootElement, query );
    }

    public static IEnumerable<JsonElement> Select( this JsonDocument document, string query, JsonPathEvaluator<JsonElement> evaluator )
    {
        return new JsonPath( new JsonPathFuncEvaluator<JsonElement>( evaluator ) ).Select( document.RootElement, query );
    }

    // JsonPathElement

    public static IEnumerable<JsonPathElement> SelectPath( this JsonElement element, string query )
    {
        return new JsonPath( null ).SelectPath( element, query );
    }

    public static IEnumerable<JsonPathElement> SelectPath( this JsonElement element, string query, IJsonPathFilterEvaluator<JsonElement> evaluator )
    {
        return new JsonPath( evaluator ).SelectPath( element, query );
    }

    public static IEnumerable<JsonPathElement> SelectPath( this JsonElement element, string query, JsonPathEvaluator<JsonElement> evaluator )
    {
        return new JsonPath( new JsonPathFuncEvaluator<JsonElement>( evaluator ) ).SelectPath( element, query );
    }

    public static IEnumerable<JsonPathElement> SelectPath( this JsonDocument document, string query )
    {
        return new JsonPath( null ).SelectPath( document.RootElement, query );
    }

    public static IEnumerable<JsonPathElement> SelectPath( this JsonDocument document, string query, IJsonPathFilterEvaluator<JsonElement> evaluator )
    {
        return new JsonPath( evaluator ).SelectPath( document.RootElement, query );
    }

    public static IEnumerable<JsonPathElement> SelectPath( this JsonDocument document, string query, JsonPathEvaluator<JsonElement> evaluator )
    {
        return new JsonPath( new JsonPathFuncEvaluator<JsonElement>( evaluator ) ).SelectPath( document.RootElement, query );
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

    public static IEnumerable<JsonNode> Select( this JsonNode node, string query, JsonPathEvaluator<JsonNode> evaluator )
    {
        return new Nodes.JsonPathNode( new JsonPathFuncEvaluator<JsonNode>( evaluator ) ).Select( node, query );
    }
}

