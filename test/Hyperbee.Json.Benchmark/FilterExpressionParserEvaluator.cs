using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Benchmark;

public class FilterExpressionParserEvaluator
{
    [Params( "(\"world\" == 'world') && (true || false)" )]
    public string Filter;

    [Benchmark]
    public void JsonPathFilterParser_JsonElement()
    {
        FilterParser<JsonElement>.Parse( Filter );
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonNode()
    {
        FilterParser<JsonNode>.Parse( Filter );
    }
}
