using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Core;
using Hyperbee.Json.Dynamic;
using Hyperbee.Json.Patch;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json.Serialization;
using AspNetCore = Microsoft.AspNetCore.JsonPatch;
using JsonEverything = Json.Patch;

namespace Hyperbee.Json.Benchmark;

public class JsonPatchBenchmark
{
    [Params( """{"name":"John","age":30,"city":"New York"}""" )]
    public string Source;

    [Params(
        """[{ "op":"add", "path":"/country", "value":"USA" }]"""
    )]
    //"""[{ "op":"remove", "path":"/age" }]"""
    public string Operations;

    private JsonNode _nodeSource;
    private JsonNode _nodeEverythingSource;
    private JsonElement _elementSource;
    private dynamic _dynamicSource;

    private JsonNode _nodeElementSource;

    private JsonPatch _patchNode;
    private JsonPatch _patchElement;
    private JsonEverything.JsonPatch _everythingPath;
    private JsonPatchDocument _aspPatch;

    [GlobalSetup]
    public void Setup()
    {
        _nodeSource = JsonNode.Parse( Source );
        _nodeEverythingSource = JsonNode.Parse( Source );
        _elementSource = JsonDocument.Parse( Source ).RootElement;
        _dynamicSource = JsonDynamicHelper.ConvertToDynamic( JsonNode.Parse( Source ) );

        _nodeElementSource = JsonNodeFactory.Create( _elementSource );

        _patchNode = JsonSerializer.Deserialize<JsonPatch>( Operations );
        _patchElement = JsonSerializer.Deserialize<JsonPatch>( Operations );

        _everythingPath = JsonSerializer.Deserialize<JsonEverything.JsonPatch>( Operations );
        _aspPatch = new JsonPatchDocument(
            JsonSerializer.Deserialize<List<AspNetCore.Operations.Operation>>( Operations ),
            new DefaultContractResolver()
        );
    }

    [Benchmark]
    public void Hyperbee_JsonNode()
    {
        _patchNode.Apply( _nodeSource );
    }

    [Benchmark]
    public void Hyperbee_JsonElement()
    {
        _patchElement.Apply( _nodeElementSource ); // Test a JsonNode backed by a JsonElement
    }

    [Benchmark]
    public void AspNetCore_JsonNode()
    {
        _aspPatch.ApplyTo( _dynamicSource );
    }

    [Benchmark]
    public void JsonEverything_JsonNode()
    {
        _everythingPath.Apply( _nodeEverythingSource );
    }

}
