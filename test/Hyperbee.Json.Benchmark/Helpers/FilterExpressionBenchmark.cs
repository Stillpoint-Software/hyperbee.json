using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Path.Filters.Parser;

namespace Hyperbee.Json.Benchmark.Helpers;

public class FilterExpressionBenchmark
{
    [Params( "(\"world\" == 'world') && (true || false)" )]
    public string Filter;

    [Benchmark]
    public void FilterParser_JsonElement()
    {
        FilterParser<JsonElement>.Parse( Filter );
    }

    [Benchmark]
    public void FilterParser_JsonNode()
    {
        FilterParser<JsonNode>.Parse( Filter );
    }
}
