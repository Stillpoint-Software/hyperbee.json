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

    private JsonPathCSharpElementEvaluator _csharpElementEvaluator;
    private JsonPathCSharpNodeEvaluator _csharpNodeEvaluator;
    private JsonPathExpressionElementEvaluator _expressionElementEvaluator;
    private JsonPathExpressionNodeEvaluator _expressionNodeEvaluator;

    [GlobalSetup]
    public void Setup()
    {
        const string document = """
                                {
                                  "store": {
                                    "book": [
                                      {
                                        "category": "reference",
                                        "author": "Nigel Rees",
                                        "title": "Sayings of the Century",
                                        "price": 8.95
                                      },
                                      {
                                        "category": "fiction",
                                        "author": "Evelyn Waugh",
                                        "title": "Sword of Honour",
                                        "price": 12.99
                                      },
                                      {
                                        "category": "fiction",
                                        "author": "Herman Melville",
                                        "title": "Moby Dick",
                                        "isbn": "0-553-21311-3",
                                        "price": 8.99
                                      },
                                      {
                                        "category": "fiction",
                                        "author": "J. R. R. Tolkien",
                                        "title": "The Lord of the Rings",
                                        "isbn": "0-395-19395-8",
                                        "price": 22.99
                                      }
                                    ],
                                    "bicycle": {
                                      "color": "red",
                                      "price": 19.95
                                    }
                                  }
                                }
                                """;

        _node = JsonNode.Parse( document )!;
        _csharpNodeEvaluator = new JsonPathCSharpNodeEvaluator();
        _expressionNodeEvaluator = new JsonPathExpressionNodeEvaluator();

        _element = JsonDocument.Parse( document ).RootElement;
        _csharpElementEvaluator = new JsonPathCSharpElementEvaluator();
        _expressionElementEvaluator = new JsonPathExpressionElementEvaluator();
    }

    [Benchmark]
    public void JsonPath_CSharpEvaluator_JsonElement()
    {
        var _ = _element.Select( Filter, _csharpElementEvaluator ).ToArray();
    }

    [Benchmark]
    public void JsonPath_CSharpEvaluator_JsonNode()
    {
        var _ = _node.Select( Filter, _csharpNodeEvaluator ).ToArray();
    }

    [Benchmark]
    public void JsonPath_ExpressionEvaluator_JsonElement()
    {
        var _ = _element.Select( Filter, _expressionElementEvaluator ).ToArray();
    }

    [Benchmark]
    public void JsonPath_ExpressionEvaluator_JsonNode()
    {
        var _ = _node.Select( Filter, _expressionNodeEvaluator ).ToArray();
    }
}
