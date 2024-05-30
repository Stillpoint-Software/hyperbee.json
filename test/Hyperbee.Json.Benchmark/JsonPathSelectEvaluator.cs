using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Evaluators;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Benchmark;

[MemoryDiagnoser]
[ShortRunJob]
public class JsonPathSelectEvaluator
{
    [Params( @"$..book[?(@.price == 8.99 && @.category == ""fiction"")]" )] //, "$.store.book[?(match(@.title, \"Sayings*\" ))]" )]
    public string Filter;

    public JsonNode _node;
    public JsonElement _element;

    [GlobalSetup]
    public void Setup()
    {
        var document = "{\r\n  \"store\": {\r\n    \"book\": [\r\n      {\r\n        \"category\": \"reference\",\r\n        \"author\": \"Nigel Rees\",\r\n        \"title\": \"Sayings of the Century\",\r\n        \"price\": 8.95\r\n      },\r\n      {\r\n        \"category\": \"fiction\",\r\n        \"author\": \"Evelyn Waugh\",\r\n        \"title\": \"Sword of Honour\",\r\n        \"price\": 12.99\r\n      },\r\n      {\r\n        \"category\": \"fiction\",\r\n        \"author\": \"Herman Melville\",\r\n        \"title\": \"Moby Dick\",\r\n        \"isbn\": \"0-553-21311-3\",\r\n        \"price\": 8.99\r\n      },\r\n      {\r\n        \"category\": \"fiction\",\r\n        \"author\": \"J. R. R. Tolkien\",\r\n        \"title\": \"The Lord of the Rings\",\r\n        \"isbn\": \"0-395-19395-8\",\r\n        \"price\": 22.99\r\n      }\r\n    ],\r\n    \"bicycle\": {\r\n      \"color\": \"red\",\r\n      \"price\": 19.95\r\n    }\r\n  }\r\n}";

        _node = JsonNode.Parse( document )!;
        _element = JsonDocument.Parse( document ).RootElement;
    }

    [Benchmark]
    public void JsonPath_CSharpEvaluator_JsonElement()
    {
        var _ = _element.Select( Filter, new JsonPathCSharpElementEvaluator() ).ToArray();
    }

    [Benchmark]
    public void JsonPath_CSharpEvaluator_JsonNode()
    {
        var _ = _node.Select( Filter, new JsonPathCSharpNodeEvaluator() ).ToArray();
    }

    [Benchmark]
    public void JsonPath_ExpressionEvaluator_JsonElement()
    {
        var _ = _element.Select( Filter, new JsonPathExpressionElementEvaluator() ).ToArray();
    }

    [Benchmark]
    public void JsonPath_ExpressionEvaluator_JsonNode()
    {
        var _ = _node.Select( Filter, new JsonPathExpressionNodeEvaluator() ).ToArray();
    }
}
