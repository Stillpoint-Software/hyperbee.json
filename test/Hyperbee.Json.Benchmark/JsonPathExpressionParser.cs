using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Evaluators;
using Hyperbee.Json.Evaluators.Parser;

namespace Hyperbee.Json.Benchmark;

public class JsonPathExpressionParser
{
    private ParseExpressionContext<JsonNode> _nodeExpressionContext;
    private ParseExpressionContext<JsonElement> _elementExpressionContext;

    [Params( "(\"world\" == 'world') && (true || false)" )]
    public string Filter;


    [GlobalSetup]
    public void Setup()
    {
        _nodeExpressionContext = new ParseExpressionContext<JsonNode>(
            Expression.Parameter( typeof( JsonNode ) ),
            Expression.Parameter( typeof( JsonNode ) ),
            new JsonPathExpressionNodeEvaluator() );


        _elementExpressionContext = new ParseExpressionContext<JsonElement>(
            Expression.Parameter( typeof( JsonElement ) ),
            Expression.Parameter( typeof( JsonElement ) ),
            new JsonPathExpressionElementEvaluator() );
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonElement()
    {
        JsonPathExpression.Parse( Filter, _elementExpressionContext );
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonNode()
    {
        JsonPathExpression.Parse( Filter, _nodeExpressionContext );
    }
}
