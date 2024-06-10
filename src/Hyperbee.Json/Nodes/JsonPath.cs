using System.Text.Json.Nodes;
using Hyperbee.Json.Evaluators;

namespace Hyperbee.Json.Nodes;

public sealed class JsonPathNode
{
    public static IJsonPathFilterEvaluator<JsonNode> FilterEvaluator { get; set; } = new JsonPathExpressionEvaluator<JsonNode>();

    private readonly JsonPathVisitorBase<JsonNode> _visitor = new JsonPathNodeVisitor();

    public IEnumerable<JsonNode> Select( in JsonNode value, string query )
    {
        return _visitor.ExpressionVisitor( value, value, query, FilterEvaluator );
    }

    internal IEnumerable<JsonNode> Select( in JsonNode value, JsonNode root, string query )
    {
        return _visitor.ExpressionVisitor( value, root, query, FilterEvaluator );
    }
}
