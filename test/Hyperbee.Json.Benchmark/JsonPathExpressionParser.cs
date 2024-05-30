using System.Linq.Expressions;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Evaluators;
using Hyperbee.Json.Evaluators.Parser;

namespace Hyperbee.Json.Benchmark;

[MemoryDiagnoser]
[ShortRunJob]
public class JsonPathExpressionParser
{
    private JsonPathExpressionNodeEvaluator _nodeEvaluator;
    private ParameterExpression _jsonNodeParam;

    private JsonPathExpressionElementEvaluator _elementEvaluator;
    private ParameterExpression _jsonElementParam;

    [Params( "(\"world\" == 'world') && (true || false)" )]
    public string Filter;

    [GlobalSetup]
    public void Setup()
    {
        _nodeEvaluator = new JsonPathExpressionNodeEvaluator();
        _jsonNodeParam = Expression.Parameter( typeof( JsonNode ) );

        _elementEvaluator = new JsonPathExpressionElementEvaluator();
        _jsonElementParam = Expression.Parameter( typeof( JsonNode ) );
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonElement()
    {
        JsonPathExpression.Parse( Filter, _jsonNodeParam, _jsonNodeParam, _nodeEvaluator );
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonNode()
    {
        JsonPathExpression.Parse( Filter, _jsonElementParam, _jsonElementParam, _elementEvaluator );
    }
}
