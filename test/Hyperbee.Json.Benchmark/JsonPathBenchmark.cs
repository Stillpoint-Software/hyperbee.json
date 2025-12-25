using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Hyperbee.Json.Extensions;
using JsonCons.JsonPath;
using Newtonsoft.Json.Linq;
using JsonEverything = Json.Path;

namespace Hyperbee.Json.Benchmark;


public class JsonPathBenchmark
{
    /*
    [Params(
|      | `$..* First()`
       | `$..*`
       | `$..price`
       | `$.store.book[?(@.price == 8.99)]`
       | `$.store.book[0]`
    )]
    */

    [Params(
        // Root and Wildcard
        "$",
        "$.store.*",
        "$.store.* #First()",   // Test Enumerable.First()

        // Property and Index Access
        "$.store.book[0]",
        "$.store.book[0].title",
        "$.store.book[*]",
        "$.store.book[*].author",
        "$.store.book['category','author']",

        // Recursive Descent
        "$.store..price",
        "$..author",
        "$..*",
        "$..['bicycle','price']",
        "$..book[0,1]",
        "$..book[?@.isbn]",

        // Filters
        "$.store.book[?(@.price < 10)].title",
        "$.store.book[?(@.price > 10 && @.price < 20)]",
        "$.store.book[?(@.category == 'fiction')]",
        "$.store.book[?(@.author && @.title)]",
        "$.store.book[?(@.price == 8.99)]",
        "$..[?(@.price < 10)]",
        "$..book[?@.price == 8.99 && @.category == 'fiction']",
        "$.store.book[?(@.price < 10 || @.category == 'fiction')]",
        "$.store.book[?(!@.isbn)]",
        "$.store.book[?(length(@.title) > 10)]",


        // Array Slices and Unions
        "$.store.book[-1:]",
        "$.store.book[0,1]",
        "$.store.book[:2]",
        "$.store.book[0:3:2]",

        // Property Access (Direct)
        "$.store.bicycle.color"
    )]
    public string Filter;

    public string Document;
    public JsonNode _node;
    public JsonElement _element;
    private JObject _jObject;


    public Consumer _consumer = new();

    [GlobalSetup]
    public void Setup()
    {
        Document =
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


        _jObject = JObject.Parse( Document );
        _node = JsonNode.Parse( Document )!;
        _element = JsonDocument.Parse( Document ).RootElement;
    }

    public (string, bool) GetFilter()
    {
        const string First = " #First()";

        return Filter.EndsWith( First )
            ? (Filter[..^First.Length], true)
            : (Filter, false);
    }

    private void Consume<T>( IEnumerable<T> select, bool takeFirst )
    {
        if ( takeFirst )
            _ = select.First();
        else
            select.Consume( _consumer );
    }

    [Benchmark( Description = "Hyperbee.JsonElement" )]
    public void Hyperbee_JsonElement()
    {
        var (filter, first) = GetFilter();
        var select = _element.Select( filter );

        Consume( select, first );
    }

    //[Benchmark( Description = "Hyperbee.JsonNode" )]
    public void Hyperbee_JsonNode()
    {
        var (filter, first) = GetFilter();

        var node = JsonNode.Parse( Document )!;
        var select = node.Select( filter );

        Consume( select, first );
    }

    //[Benchmark( Description = "Newtonsoft.JObject" )]
    public void Newtonsoft_JObject()
    {
        var (filter, first) = GetFilter();

        var jObject = JObject.Parse( Document );
        var select = jObject.SelectTokens( filter );

        Consume( select, first );
    }

    //[Benchmark( Description = "JsonEverything.JsonNode" )]
    public void JsonEverything_JsonNode()
    {
        var (filter, first) = GetFilter();

        var path = JsonEverything.JsonPath.Parse( filter );
        var node = JsonNode.Parse( Document )!;
        var select = path.Evaluate( node ).Matches!;

        Consume( select, first );
    }

    //[Benchmark( Description = "JsonCons.JsonElement" )]
    public void JsonCons_JsonElement()
    {
        var (filter, first) = GetFilter();

        var path = JsonSelector.Parse( filter )!;
        var element = JsonDocument.Parse( Document ).RootElement;
        var select = path.Select( element );

        Consume( select, first );
    }
}
