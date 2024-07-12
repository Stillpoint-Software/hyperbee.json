using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Descriptors.Element;
using Hyperbee.Json.Descriptors.Node;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Benchmark;

public class FilterExpressionParserEvaluator
{
    private FilterParserContext<JsonNode> _nodeExecutionParserContext;
    private FilterParserContext<JsonElement> _elementExecutionParserContext;

    [Params( "(\"world\" == 'world') && (true || false)" )]
    public string Filter;

    [GlobalSetup]
    public void Setup()
    {
        _nodeExecutionParserContext = new FilterParserContext<JsonNode>( new NodeTypeDescriptor() );

        _elementExecutionParserContext = new FilterParserContext<JsonElement>( new ElementTypeDescriptor() );
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonElement()
    {
        FilterParser<JsonElement>.Parse( Filter, _elementExecutionParserContext );
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonNode()
    {
        FilterParser<JsonNode>.Parse( Filter, _nodeExecutionParserContext );
    }
}
