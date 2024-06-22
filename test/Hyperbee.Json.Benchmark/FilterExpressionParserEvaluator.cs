using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Descriptors.Element;
using Hyperbee.Json.Descriptors.Node;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Benchmark;

public class FilterExpressionParserEvaluator
{
    private FilterExecutionContext _nodeExecutionContext;
    private FilterExecutionContext _elementExecutionContext;

    [Params( "(\"world\" == 'world') && (true || false)" )]
    public string Filter;

    [GlobalSetup]
    public void Setup()
    {
        _nodeExecutionContext = new FilterExecutionContext(
            Expression.Parameter( typeof( JsonNode ) ),
            Expression.Parameter( typeof( JsonNode ) ),
            new NodeTypeDescriptor() );

        _elementExecutionContext = new FilterExecutionContext(
            Expression.Parameter( typeof( JsonElement ) ),
            Expression.Parameter( typeof( JsonElement ) ),
            new ElementTypeDescriptor() );
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonElement()
    {
        FilterParser.Parse( Filter, _elementExecutionContext );
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonNode()
    {
        FilterParser.Parse( Filter, _nodeExecutionContext );
    }
}
