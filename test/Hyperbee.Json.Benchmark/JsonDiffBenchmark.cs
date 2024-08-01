using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Patch;

namespace Hyperbee.Json.Benchmark;

public class JsonDiffBenchmark
{
    [Params( """{"name":"John","age":30,"city":"New York"}""" )]
    public string Source;

    [Params(
        """{"name":"John","age":35,"city":"New York","country":"USA"}""",
        """{"name":"John","age":35}"""
    )]
    public string Target;

    private JsonNode _nodeSource;
    private JsonNode _nodeTarget;
    private JsonElement _elementSource;
    private JsonElement _elementTarget;

    [GlobalSetup]
    public void Setup()
    {
        _nodeSource = JsonNode.Parse( Source );
        _nodeTarget = JsonNode.Parse( Target );
        _elementSource = JsonDocument.Parse( Source ).RootElement;
        _elementTarget = JsonDocument.Parse( Target ).RootElement;
    }

    [Benchmark]
    public void JsonDiff_JsonElement()
    {
        _ = JsonDiff<JsonElement>.Diff( _elementSource, _elementTarget ).ToArray();
    }

    [Benchmark]
    public void JsonDiff_JsonNode()
    {
        _ = JsonDiff<JsonNode>.Diff( _nodeSource, _nodeTarget ).ToArray();
    }
}
