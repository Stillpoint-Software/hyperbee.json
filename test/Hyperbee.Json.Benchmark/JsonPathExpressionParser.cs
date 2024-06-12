using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Descriptors.Element;
using Hyperbee.Json.Descriptors.Node;
using Hyperbee.Json.Filters.Parser;

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
            new NodeTypeDescriptor() );

        _elementExpressionContext = new ParseExpressionContext(
            Expression.Parameter( typeof( JsonElement ) ),
            Expression.Parameter( typeof( JsonElement ) ),
            new ElementTypeDescriptor() );
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
