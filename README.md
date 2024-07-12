
# Hyperbee.Json

`Hyperbee.Json` is a high-performance JSONPath parser for .NET, that supports both `JsonElement` and `JsonNode`.  
The library is designed to be quick and extensible, allowing support for other JSON document types and functions.

## Features

- **High Performance:** Optimized for performance and efficiency.
- **Supports:** `JsonElement` and `JsonNode`.
- **Extensible:** Easily extended to support additional JSON document types and functions.
- **`IEnumerable` Results:** Deferred execution queries with `IEnumerable`.
- **Conformant:** Adheres to the JSONPath Specification [RFC 9535](https://www.rfc-editor.org/rfc/rfc9535.html). 

## JSONPath RFC

Hyperbee.Json conforms to the RFC, and aims to support the [JSONPath consensus](https://cburgmer.github.io/json-path-comparison) 
when the RFC is unopinionated. When the RFC is unopinionated, and where the consensus is ambiguous or not aligned with our 
performance and usability goals, we may deviate. Our goal is always to provide a robust and performant library while  
strengthening our alignment with the RFC and the community.

## Installation

Install via NuGet:

```bash
dotnet add package Hyperbee.Json
```

## Usage

### Basic Examples

#### Selecting Elements

```csharp

var json = """
{ 
  "store": { 
    "book": [
      { "category": "fiction" }, 
      { "category": "science" } 
    ] 
  } 
}
""";

var root = JsonDocument.Parse(json);
var result = JsonPath.Select(root, "$.store.book[*].category");

foreach (var item in result)
{
    Console.WriteLine(item); // Output: "fiction" and "science"
}
```

#### Filtering

```csharp

var json = """
{ 
  "store": { 
    "book": [
      { 
        "category": "fiction",
        "price": 10  
      }, 
      { 
        "category": "science",
        "price": 15  
      } 
    ] 
  } 
}
""";

var root = JsonDocument.Parse(json);
var result = JsonPath.Select(root, "$.store.book[?(@.price > 10)]");

foreach (var item in result)
{
    Console.WriteLine(item); // Output: { "category": "science", "price": 15 }
}
```

#### Working with (JsonElement, Path) pairs
```csharp

var json = """
{ 
  "store": { 
    "book": [
      { "category": "fiction" }, 
      { "category": "science" } 
    ] 
  } 
}
""";

var root = JsonDocument.Parse(json);
var result = JsonPath.SelectPath(root, "$.store.book[0].category");

var (node, path) = result.First();

Console.WriteLine(node); // Output: "fiction"
Console.WriteLine(path); // Output: "$.store.book[0].category
```

#### Working with JsonNode

```csharp

var json = """
{ 
  "store": { 
    "book": [
      { "category": "fiction" }, 
      { "category": "science" } 
    ] 
  } 
}
""";

var root = JsonNode.Parse(json);
var result = JsonPath.Select(root, "$.store.book[0].category");

Console.WriteLine(result.First()); // Output: "fiction"
```

## JSONPath Syntax Overview

Here's a quick overview of JSONPath syntax:

| JSONPath                                     | Description                                                
|:---------------------------------------------|:-----------------------------------------------------------
| `$`                                          | Root node                                    
| `@`                                          | Current node                                 
| `.<name>`, `.'<name>'`, or `."<name>"`       | Object member dot operator
| `[<name>]`, or `['<name>']`, or `["<name>"]` | Object member subscript operator
| `[<index]`                                   | Array access operator
| `[,]`                                        | Union operator
| `[start:end:step]`                           | Array slice operator
| `*`, or `[*]`                                | Wildcard 
| `..`                                         | Recursive descent  
| `?<expr>`                                    | Filter selector

JSONPath expressions refer to a JSON structure, and JSONPath assumes the name `$` is assigned 
to the root JSON object.

JSONPath expressions can use dot-notation:

    $.store.book[0].title

or bracket-notation:

    $['store']['book'][0]['title']

- JSONPath allows the wildcard symbol `*` for member names and array indices. 
- It borrows the descendant operator `..` from [E4X][e4x]
- It uses the `@` symbol to refer to the current object.
- It uses the `?()` syntax for filtering.
- It uses the array slice syntax proposal `[start:end:step]` from ECMASCRIPT 4.

Expressions can be used as an alternative to explicit names or indices, as in:

    $.store.book[(length(@)-1)].title

Filter expressions are supported via the syntax `?(<boolean expr>)`, as in:

    $.store.book[?(@.price < 10)].title

### JSONPath Functions

JsonPath expressions support basic methods calls.

| Method     | Description                                            | Example                                                
|------------|--------------------------------------------------------|------------------------------------------------
| `length()` | Returns the length of an array or string.              | `$.store.book[?(length(@.title) > 5)]`                
| `count()`  | Returns the count of matching elements.                | `$.store.book[?(count(@.authors.) > 1)]`               
| `match()`  | Returns true if a string matches a regular expression. | `$.store.book[?(match(@.title,'.*Century.*'))]`   
| `search()` | Searches for a string within another string.           | `$.store.book[?(search(@.title,'Sword'))]`             
| `value()`  | Accesses the value of a key in the current object.     | `$.store.book[?(value(@.price) < 10)]`                


### JSONPath Custom Functions

You can also extend the supported function set by registering your own functions.

**Example:** Implement a `JsonNode` Path Function:

**Step 1:** Create a custom function that returns the path of a `JsonNode`.

```csharp
public class PathNodeFunction() : FilterExtensionFunction( PathMethodInfo, FilterExtensionInfo.MustCompare )
{
    public const string Name = "path";
    private static readonly MethodInfo PathMethodInfo = GetMethod<PathNodeFunction>( nameof( Path ) );

    private static INodeType Path( INodeType arg )
    {
        if ( arg is not NodesType<JsonNode> nodes )
            return Constants.Null;

        var node = nodes.FirstOrDefault();
        return new ValueType<string>( node?.GetPath() );
}
```

**Step 2:** Register your custom function.

```csharp
JsonTypeDescriptorRegistry.GetDescriptor<JsonNode>().Functions
    .Register( PathNodeFunction.Name, () => new PathNodeFunction() );
```

**Step 3:** Use your custom function in a JSONPath query.

```csharp
var results = source.Select( "$..[?path(@) == '$.store.book[2].title']" );
```

## Comparison with Other Libraries

There are excellent libraries available for RFC-9535 .NET JsonPath.

### [JsonPath.Net](https://docs.json-everything.net/path/basics/) Json-Everything

- **Pros:**
  - Comprehensive feature set.
  - Deferred execution queries with `IEnumerable`.
  - Strong community support.
  - .NET Foundation Project.
  
- **Cons:**
  - No support for `JsonElement`.
  - Not quite as fast as other `System.Text.Json` implementations.
   
### [JsonCons.NET](https://danielaparker.github.io/JsonCons.Net/articles/JsonPath/JsonConsJsonPath.html)

- **Pros:**
  - High performance.
  - Enhanced JsonPath syntax.

- **Cons:**
  - No support for `JsonNode`.
  - Does not return an `IEnumerable` result (no defered query execution).
  
### [Json.NET](https://www.newtonsoft.com/json) Newtonsoft

- **Pros:**
  - Comprehensive feature set.
  - Documentation and examples.
  - Strong community support.
  - .NET Foundation Project.

- **Cons:**
  - No support for `JsonElement`, or `JsonNode`.

### Why Choose [Hyperbee.Json](https://github.com/Stillpoint-Software/Hyperbee.Json) ?

- High Performance.
- Supports both `JsonElement`, and `JsonNode`.
- Deferred execution queries with `IEnumerable`.
- Extendable to support additional JSON document types and functions.
- RFC conforming JSONPath implementation.

## Benchmarks

Here is a performance comparison of various queries on the standard book store document.

```json
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
```

```
| Method                  | Filter                           | Mean      | Error     | StdDev    | Allocated 
|------------------------ |--------------------------------- |----------:|----------:|----------:|----------:
| Hyperbee_JsonElement    | $..* `First()`                   |  3.386 us | 1.6882 us | 0.0925 us |    4.3 KB 
| JsonEverything_JsonNode | $..* `First()`                   |  3.525 us | 0.2192 us | 0.0120 us |   3.53 KB 
| Hyperbee_JsonNode       | $..* `First()`                   |  3.626 us | 1.9373 us | 0.1062 us |   3.45 KB 
| JsonCons_JsonElement    | $..* `First()`                   |  6.309 us | 2.5595 us | 0.1403 us |   8.48 KB 
| Newtonsoft_JObject      | $..* `First()`                   |  9.319 us | 3.2976 us | 0.1808 us |  14.22 KB 
|                         |                                  |           |           |           |           
| JsonCons_JsonElement    | $..*                             |  6.197 us | 3.5939 us | 0.1970 us |   8.45 KB 
| Hyperbee_JsonElement    | $..*                             |  8.819 us | 4.7836 us | 0.2622 us |   11.1 KB 
| Newtonsoft_JObject      | $..*                             | 10.979 us | 0.9546 us | 0.0523 us |  14.86 KB 
| Hyperbee_JsonNode       | $..*                             | 11.580 us | 7.7321 us | 0.4238 us |  10.91 KB 
| JsonEverything_JsonNode | $..*                             | 23.595 us | 6.7195 us | 0.3683 us |  36.81 KB 
|                         |                                  |           |           |           |           
| JsonCons_JsonElement    | $..price                         |  5.604 us | 0.4885 us | 0.0268 us |   5.65 KB 
| Hyperbee_JsonElement    | $..price                         |  5.629 us | 2.6981 us | 0.1479 us |   6.45 KB 
| Hyperbee_JsonNode       | $..price                         |  8.680 us | 5.7286 us | 0.3140 us |   8.86 KB 
| Newtonsoft_JObject      | $..price                         | 11.075 us | 1.8627 us | 0.1021 us |   14.4 KB 
| JsonEverything_JsonNode | $..price                         | 17.710 us | 2.2284 us | 0.1221 us |  27.63 KB 
|                         |                                  |           |           |           |           
| Hyperbee_JsonElement    | $.store.book[?(@.price == 8.99)] |  4.493 us | 0.4963 us | 0.0272 us |   5.95 KB 
| JsonCons_JsonElement    | $.store.book[?(@.price == 8.99)] |  5.424 us | 3.8810 us | 0.2127 us |   5.05 KB 
| Hyperbee_JsonNode       | $.store.book[?(@.price == 8.99)] |  7.535 us | 0.5587 us | 0.0306 us |   8.59 KB 
| Newtonsoft_JObject      | $.store.book[?(@.price == 8.99)] | 10.839 us | 5.3074 us | 0.2909 us |  15.84 KB 
| JsonEverything_JsonNode | $.store.book[?(@.price == 8.99)] | 12.269 us | 5.7236 us | 0.3137 us |  15.85 KB 
|                         |                                  |           |           |           |           
| Hyperbee_JsonElement    | $.store.book[0]                  |  2.847 us | 1.4849 us | 0.0814 us |   2.27 KB 
| JsonCons_JsonElement    | $.store.book[0]                  |  3.352 us | 0.3684 us | 0.0202 us |   3.21 KB 
| Hyperbee_JsonNode       | $.store.book[0]                  |  3.377 us | 0.9370 us | 0.0514 us |   2.77 KB 
| JsonEverything_JsonNode | $.store.book[0]                  |  4.960 us | 2.8846 us | 0.1581 us |   5.96 KB 
| Newtonsoft_JObject      | $.store.book[0]                  |  9.444 us | 4.5874 us | 0.2514 us |  14.56 KB 
```

## Additioal Documentation

Additional documentation can be found in the project's `/docs` folder.

## Credits

Hyperbee.Json is built upon the great work of several open-source projects. Special thanks to:

- Stefan Goessner for the original [JSONPath implementation](https://goessner.net/articles/JsonPath/).
- System.Text.Json team for their work on the `System.Text.Json` library.
- Atif Aziz's C# port of Goessner's JSONPath library [.NET JSONPath](https://github.com/atifaziz/JSONPath).  
- Christoph Burgmer [JSONPath consensus effort](https://cburgmer.github.io/json-path-comparison).

## Contributing

We welcome contributions! Please see our [Contributing Guide](https://github.com/Stillpoint-Software/.github/blob/main/.github/CONTRIBUTING.md) for more details.
