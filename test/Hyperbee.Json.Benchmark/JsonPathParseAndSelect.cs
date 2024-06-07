using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Evaluators;
using Hyperbee.Json.Extensions;
using Newtonsoft.Json.Linq;

namespace Hyperbee.Json.Benchmark;

public class JsonPathParseAndSelect
{
    [Params(
        "$.store.book[0]",
        "$..*"
    )]
    public string Filter;

    [Params(
        """
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
        """
    )]
    public string Document;

    private JsonPathExpressionElementEvaluator _expressionElementEvaluator;
    private JsonPathExpressionNodeEvaluator _expressionNodeEvaluator;

    [GlobalSetup]
    public void Setup()
    {
        _expressionNodeEvaluator = new JsonPathExpressionNodeEvaluator();
        _expressionElementEvaluator = new JsonPathExpressionElementEvaluator();
    }

    [Benchmark]
    public void JsonPath_ExpressionEvaluator_JsonElement()
    {
        var element = JsonDocument.Parse( Document ).RootElement;
        var _ = element.Select( Filter, _expressionElementEvaluator ).ToArray();
    }

    [Benchmark]
    public void JsonPath_ExpressionEvaluator_JsonNode()
    {
        var node = JsonNode.Parse( Document )!;
        var _ = node.Select( Filter, _expressionNodeEvaluator ).ToArray();
    }

    [Benchmark]
    public void JsonPath_Newtonsoft_JObject()
    {
        var jObject = JObject.Parse( Document );
        var _ = jObject.SelectTokens( Filter ).ToArray();
    }
}
