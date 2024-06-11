using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Evaluators.Parser;
using Hyperbee.Json.Evaluators.Parser.Element;
using Hyperbee.Json.Evaluators.Parser.Node;

namespace Hyperbee.Json.Benchmark;

public class JsonPathExpressionParser
{
    private ParseExpressionContext _nodeExpressionContext;
    private ParseExpressionContext _elementExpressionContext;

    [Params( "(\"world\" == 'world') && (true || false)" )]
    public string Filter;


    [GlobalSetup]
    public void Setup()
    {
        _nodeExpressionContext = new ParseExpressionContext(
            Expression.Parameter( typeof( JsonNode ) ),
            Expression.Parameter( typeof( JsonNode ) ),
            new JsonNodeTypeDescriptor() );

        _elementExpressionContext = new ParseExpressionContext(
            Expression.Parameter( typeof( JsonElement ) ),
            Expression.Parameter( typeof( JsonElement ) ),
            new JsonElementTypeDescriptor() );
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
