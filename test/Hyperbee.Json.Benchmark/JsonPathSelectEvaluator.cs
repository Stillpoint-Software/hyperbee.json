using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Extensions;
using Newtonsoft.Json.Linq;

namespace Hyperbee.Json.Benchmark;

public class JsonPathSelectEvaluator
{
    [Params(
        "$",
        "$.store.book[0]",
        "$.store..price",
        "$..book[0]",
        "$.store.*",
        "$.store.book[*].author",
        "$.store.book[-1:]",
        "$.store.book[0,1]",
        "$.store.book['category','author']",
        "$..book[?@.isbn]",
        "$.store.book[?@.price == 8.99]",
        "$..*",
        "$..book[?@.price == 8.99 && @.category == 'fiction']"
    )]
    public string Filter;

    public JsonNode _node;
    public JsonElement _element;

    private JObject _jObject;

    [GlobalSetup]
    public void Setup()
    {
        const string document =
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
        """;

        _jObject = JObject.Parse( document );

        _node = JsonNode.Parse( document )!;
        _element = JsonDocument.Parse( document ).RootElement;
    }

    [Benchmark]
    public void Hyperbee_JsonElement()
    {
        var _ = _element.Select( Filter ).ToArray();
    }

    [Benchmark]
    public void Hyperbee_JsonNode()
    {
        var _ = _node.Select( Filter ).ToArray();
    }

    [Benchmark]
    public void Newtonsoft_JObject()
    {
        var _ = _jObject.SelectTokens( Filter ).ToArray();
    }
}
