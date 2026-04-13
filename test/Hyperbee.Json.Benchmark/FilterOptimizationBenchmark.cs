using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Path.Filters;
using Hyperbee.Json.Path.Filters.Parser;
using Hyperbee.Json.Query;

namespace Hyperbee.Json.Benchmark;

// Isolates the two axes that the optimization pass targets:
//   Compile  - parse + Expression.Compile() for a single filter
//   Evaluate - per-evaluation cost of an already-compiled filter
public class FilterOptimizationBenchmark
{
    [Params(
        "@.price < 10",
        "@.price > 10 && @.price < 20",
        "@.category == 'fiction'",
        "@.author && @.title",
        "!@.isbn",
        "length(@.title) > 10"
    )]
    public string Filter;

    private string _document;
    private JsonElement _root;
    private JsonElement _book;

    private Func<FilterRuntimeContext<JsonElement>, bool> _compiled;
    private FilterRuntimeContext<JsonElement> _ctx;

    [GlobalSetup]
    public void Setup()
    {
        _document =
            """
            {
              "store": {
                "book": [
                  { "category":"reference","author":"Nigel Rees","title":"Sayings of the Century","price":8.95 },
                  { "category":"fiction","author":"Evelyn Waugh","title":"Sword of Honour","price":12.99 },
                  { "category":"fiction","author":"Herman Melville","title":"Moby Dick","isbn":"0-553-21311-3","price":8.99 },
                  { "category":"fiction","author":"J. R. R. Tolkien","title":"The Lord of the Rings","isbn":"0-395-19395-8","price":22.99 }
                ],
                "bicycle": { "color":"red","price":19.95 }
              }
            }
            """;

        _root = JsonDocument.Parse( _document ).RootElement;
        _book = _root.GetProperty( "store" ).GetProperty( "book" )[0];

        _compiled = FilterParser<JsonElement>.Compile( Filter );
        _ctx = new FilterRuntimeContext<JsonElement>( _book, _root );
    }

    // Compile: measures parse + expression tree construction + Expression.Compile().
    // Clears JsonQueryParser sub-query cache each iteration so nested selectors
    // pay realistic cold-parse cost.
    [Benchmark]
    public Func<FilterRuntimeContext<JsonElement>, bool> Compile()
    {
        JsonQueryParser.Clear();
        return FilterParser<JsonElement>.Compile( Filter );
    }

    // Evaluate: hot-path per-invocation cost. Dominated by anything the
    // compiled delegate does at runtime (e.g. JsonQueryParser.Parse lookups
    // inside the Select helper).
    [Benchmark( OperationsPerInvoke = 1000 )]
    public bool Evaluate()
    {
        var result = false;
        for ( var i = 0; i < 1000; i++ )
            result = _compiled( _ctx );
        return result;
    }
}
