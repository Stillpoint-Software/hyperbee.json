using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Descriptors.Element;
using Hyperbee.Json.Descriptors.Node;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Benchmark;

public class FilterExpressionParserEvaluator
{
    private FilterContext<JsonNode> _nodeExecutionContext;
    private FilterContext<JsonElement> _elementExecutionContext;

    [Params( "(\"world\" == 'world') && (true || false)" )]
    public string Filter;

    [GlobalSetup]
    public void Setup()
    {
        _nodeExecutionContext = new FilterContext<JsonNode>( new NodeTypeDescriptor() );

        _elementExecutionContext = new FilterContext<JsonElement>( new ElementTypeDescriptor() );
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonElement()
    {
        FilterParser<JsonElement>.Parse( Filter, _elementExecutionContext );
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonNode()
    {
        FilterParser<JsonNode>.Parse( Filter, _nodeExecutionContext );
    }
}
