using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Extensions;
using JsonCons.JsonPath;
using Newtonsoft.Json.Linq;
using JsonEverything = Json.Path;

namespace Hyperbee.Json.Benchmark;

public class JsonPathParseAndSelect
{
    [Params(
        "$.store.book[0]",
        "$.store.book[?(@.price == 8.99)]",
        "$..price",
        "$..* `First()`",
        "$..*"
    )]
    public string Filter;

    public string Document;

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
    }
    
    public (string, bool) GetFilter()
    {
        const string First = " `First()`";
        
        return Filter.EndsWith( First ) ? (Filter[..^First.Length], true) : (Filter, false);
    }

    [Benchmark]
    public void JsonPath_Hyperbee_JsonElement()
    {
        var (filter, first) = GetFilter();

        var element = JsonDocument.Parse( Document ).RootElement;

        if ( first )
            _ = element.Select( filter ).First();
        else
            _ = element.Select( filter ).ToArray();
    }

    [Benchmark]
    public void JsonPath_Hyperbee_JsonNode()
    {
        var (filter, first) = GetFilter();

        var node = JsonNode.Parse( Document )!;

        if ( first )
            _ = node.Select( filter ).First();
        else
            _ = node.Select( filter ).ToArray();
    }

    [Benchmark]
    public void JsonPath_Newtonsoft_JObject()
    {
        var (filter, first) = GetFilter();

        var jObject = JObject.Parse( Document );

        if ( first )
            _ = jObject.SelectTokens( filter ).First();
        else
            _ = jObject.SelectTokens( filter ).ToArray();
    }

    [Benchmark]
    public void JsonPath_JsonEverything_JsonNode()
    {
        var (filter, first) = GetFilter();

        var path = JsonEverything.JsonPath.Parse( filter );
        var node = JsonNode.Parse( Document )!;

        if ( first )
            _ = path.Evaluate( node ).Matches!.First();
        else
            _ = path.Evaluate( node ).Matches!.ToArray();
    }

    [Benchmark]
    public void JsonPath_JsonCons_JsonElement()
    {
        var (filter, first) = GetFilter();

        var path = JsonSelector.Parse( filter )!;
        var element = JsonDocument.Parse( Document ).RootElement;

        if ( first )
            _ = path.Select( element ).First();
        else
            _ = path.Select( element );
    }
}
