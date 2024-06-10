using System.Text.Json;
using Hyperbee.Json.Evaluators;

namespace Hyperbee.Json;

public sealed class JsonPath
{
    public static IJsonPathFilterEvaluator<JsonElement> FilterEvaluator { get; set; } = new JsonPathExpressionEvaluator<JsonElement>();
    
    private readonly JsonPathVisitorBase<JsonElement> _visitor = new JsonPathElementVisitor();

    public IEnumerable<JsonElement> Select( in JsonElement value, string query )
    {
        return _visitor.ExpressionVisitor( value, value, query, FilterEvaluator );
    }

    internal IEnumerable<JsonElement> Select( in JsonElement value, JsonElement root, string query )
    {
        return _visitor.ExpressionVisitor( value, root, query, FilterEvaluator );
    }
}
