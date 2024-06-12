using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Extensions;
using Newtonsoft.Json.Linq;
using JsonEverything = Json.Path;
using JsonCons.JsonPath;

namespace Hyperbee.Json.Benchmark;

public class JsonPathParseAndSelect
{
    [Params(
        "$.store.book[0]",
        "$.store.book[?(@.price == 8.99)]",
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

    [Benchmark]
    public void JsonPath_Hyperbee_JsonElement()
    {
        var element = JsonDocument.Parse( Document ).RootElement;
        if ( element.Select( Filter ).ToArray().Length <= 0 )
            throw new InvalidDataException( "Failed Test" );
    }

    [Benchmark]
    public void JsonPath_Hyperbee_JsonNode()
    {
        var node = JsonNode.Parse( Document )!;
        if ( node.Select( Filter ).ToArray().Length <= 0 )
            throw new InvalidDataException( "Failed Test" );
    }

    [Benchmark]
    public void JsonPath_Newtonsoft_JObject()
    {
        var jObject = JObject.Parse( Document );
        if ( jObject.SelectTokens( Filter ).ToArray().Length <= 0 )
            throw new InvalidDataException( "Failed Test" );
    }

    [Benchmark]
    public void JsonPath_JsonEverything_JsonNode()
    {
        var path = JsonEverything.JsonPath.Parse( Filter );
        var node = JsonNode.Parse( Document )!;
        if ( path.Evaluate( node ).Matches!.ToArray().Length <= 0 )
            throw new InvalidDataException( "Failed Test" );
    }

    [Benchmark]
    public void JsonPath_JsonCons_JsonNode()
    {
        var path = JsonSelector.Parse( Filter )!;
        var element = JsonDocument.Parse( Document ).RootElement;
        if ( path.Select( element ).ToArray().Length <= 0 )
            throw new InvalidDataException( "Failed Test" );
    }
}
