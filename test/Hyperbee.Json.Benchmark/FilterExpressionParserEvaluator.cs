using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Descriptors.Element;
using Hyperbee.Json.Descriptors.Node;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Benchmark;

public class FilterExpressionParserEvaluator
{
    private ITypeDescriptor<JsonNode> _nodeTypeDescriptor;
    private ITypeDescriptor<JsonElement> _elementTypeDescriptor;

    [Params( "(\"world\" == 'world') && (true || false)" )]
    public string Filter;

    [GlobalSetup]
    public void Setup()
    {
        _nodeTypeDescriptor = new NodeTypeDescriptor();
        _elementTypeDescriptor = new ElementTypeDescriptor();
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonElement()
    {
        FilterParser<JsonElement>.Parse( Filter, _elementTypeDescriptor );
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonNode()
    {
        FilterParser<JsonNode>.Parse( Filter, _nodeTypeDescriptor );
    }
}
