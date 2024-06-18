
# Hyperbee.Json

`Hyperbee.Json` is a high-performance JSONPath parser for .NET, that supports both `JsonElement` and `JsonNode`.  
The library is designed to be quick and extensible, allowing support for other JSON document types.

## Features

- **High Performance:** Optimized for performance and efficiency.
- **Supports:** `JsonElement` and `JsonNode`.
- **Extensible:** Easily extended to support additional JSON document types.
- **`IEnumerable` Results:** Deferred execution queries with `IEnumerable`.
- **Comformant:** Adheres to the JSONPath Specification [RFC 9535](https://www.rfc-editor.org/rfc/rfc9535.html). 

## JSONPath Consensus

Hyperbee.Json aims to follow the emerging [JSONPath consensus](https://cburgmer.github.io/json-path-comparison) standard where applicable.   
This standardization effort is critical for ensuring consistent behavior across different implementations of JSONPath.  
However, where the consensus is ambiguous or not aligned with our performance and usability goals, we may deviate. Our  
goal is always to provide a robust and performant library while keeping an eye on standardization progress.

## Installation

Install via NuGet:

```bash
dotnet add package Hyperbee.Json
```

## Usage

### Basic Examples

#### Selecting a Single Element

```csharp
using Hyperbee.JsonPath;
using System.Text.Json;

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
var result = JsonPath.Select(root, "$.store.book[0].category");

Console.WriteLine(result.First()); // Output: "fiction"
```

#### Selecting Multiple Elements

```csharp
using Hyperbee.JsonPath;
using System.Text.Json;

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

### Advanced Examples

#### Filtering

```csharp
using Hyperbee.JsonPath;
using System.Text.Json;

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

#### Working with JsonNode

```csharp
using Hyperbee.JsonPath;
using System.Text.Json.Nodes;

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

## JSONPath Syntax Reference

Here's a quick reference for JSONPath syntax supported by Hyperbee.Json:

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

JSONPath expressions refer to a JSON structure in the same way as XPath expressions 
are used in combination with an XML document. JSONPath assumes the name `$` is assigned 
to the root level object.

JSONPath expressions can use dot-notation:

    $.store.book[0].title

or bracket-notation:

    $['store']['book'][0]['title']

JSONPath allows the wildcard symbol `*` for member names and array indices. It
borrows the descendant operator `..` from [E4X][e4x], and the array slice
syntax proposal `[start:end:step]` from ECMASCRIPT 4.

Expressions can be used as an alternative to explicit names or indices, as in:

    $.store.book[(@.length-1)].title

using the symbol `@` for the current object. Filter expressions are supported via
the syntax `?(<boolean expr>)`, as in:

    $.store.book[?(@.price < 10)].title

For a complete guide, see [JSONPath Syntax](docs/syntax.md).

## Helper Classes

Hyperbee.Json also provides several helpers to facilitate document operations. For more details, 
refer to our [Helper Classes Documentation](docs/helper-classes.md).

## Benchmarks

```
| Method                           | Filter               | Document                | Mean      | Error      | StdDev    | Gen0   | Gen1   | Allocated |
|--------------------------------- |--------------------- |------------------------ |----------:|-----------:|----------:|-------:|-------:|----------:|
| JsonPath_JsonCons_JsonElement    | $..*                 | {\r\n (...)}\r\n} [783] |  5.745 us |  1.0845 us | 0.0594 us | 1.0834 | 0.0076 |   8.89 KB |
| JsonPath_Hyperbee_JsonElement    | $..*                 | {\r\n (...)}\r\n} [783] |  9.228 us |  1.2318 us | 0.0675 us | 1.6937 | 0.0153 |  13.87 KB |
| JsonPath_Newtonsoft_JObject      | $..*                 | {\r\n (...)}\r\n} [783] | 10.153 us |  1.4503 us | 0.0795 us | 1.8158 | 0.0763 |  14.86 KB |
| JsonPath_Hyperbee_JsonNode       | $..*                 | {\r\n (...)}\r\n} [783] | 12.293 us |  3.2456 us | 0.1779 us | 1.6785 | 0.0305 |  13.82 KB |
| JsonPath_JsonEverything_JsonNode | $..*                 | {\r\n (...)}\r\n} [783] | 21.725 us |  5.3280 us | 0.2920 us | 4.4861 | 0.1831 |  36.81 KB |
|                                  |                      |                         |           |            |           |        |        |           |
| JsonPath_Hyperbee_JsonElement    | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] |  4.137 us |  0.4001 us | 0.0219 us | 0.7401 |      - |   6.08 KB |
| JsonPath_JsonCons_JsonElement    | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] |  5.190 us |  1.4934 us | 0.0819 us | 0.6180 |      - |   5.09 KB |
| JsonPath_Hyperbee_JsonNode       | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] |  6.807 us |  0.9382 us | 0.0514 us | 1.0147 | 0.0153 |   8.34 KB |
| JsonPath_Newtonsoft_JObject      | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] | 10.147 us |  5.4939 us | 0.3011 us | 1.9379 | 0.0763 |  15.84 KB |
| JsonPath_JsonEverything_JsonNode | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] | 13.489 us | 20.7403 us | 1.1368 us | 1.9379 | 0.0458 |  15.85 KB |
|                                  |                      |                         |           |            |           |        |        |           |
| JsonPath_JsonCons_JsonElement    | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  3.047 us |  0.4309 us | 0.0236 us | 0.3967 |      - |   3.25 KB |
| JsonPath_Hyperbee_JsonNode       | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  3.297 us |  0.2681 us | 0.0147 us | 0.3815 |      - |   3.12 KB |
| JsonPath_Hyperbee_JsonElement    | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  4.008 us |  6.2011 us | 0.3399 us | 0.3433 |      - |   2.81 KB |
| JsonPath_JsonEverything_JsonNode | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  4.555 us |  0.9731 us | 0.0533 us | 0.7248 |      - |   5.96 KB |
| JsonPath_Newtonsoft_JObject      | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  9.069 us |  2.3418 us | 0.1284 us | 1.7700 | 0.0153 |  14.56 KB |
```

## Comparison with Other Libraries

There are excellent options available for RFC-9535 .NET JsonPath.

### [JsonPath.Net](https://docs.json-everything.net/path/basics/) Json-Everything

- **Pros:**
  - Extensive JSON ecosystem.
  - Comprehensive feature set.
  - Deferred execution queries with `IEnumerable`.
  - Strong community support.

- **Cons:**
  - No support for `JsonElement`.
  - Slower performance and higher memory allocation than other `System.Text.Json` implementations.
   
### [JsonCons.NET](https://danielaparker.github.io/JsonCons.Net/articles/JsonPath/JsonConsJsonPath.html)

- **Pros:**
  - High performance.
  - Enhanced JsonPath syntax.

- **Cons:**
  - No support for `JsonNode`.
  - Does not return an `IEnumerable` result (no defered query execution).
  making it less efficient for certain operations.
  
### [Json.NET](https://www.newtonsoft.com/json) Newtonsoft

- **Pros:**
  - Comprehensive feature set.
  - Documentation and examples.
  - Level 2 .NET Foundation Project.

- **Cons:**
  - No support for `JsonElement`, or `JsonNode`.
  - Slower performance and higher memory allocation than `System.Text.Json`.

### Why Choose [Hyperbee.Json](https://github.com/Stillpoint-Software/Hyperbee.Json) ?

- High Performance.
- Supports both `JsonElement`, and `JsonNode`.
- Deferred execution queries with `IEnumerable`.
- Extendable to support additional JSON document types and functions.

## Credits

Hyperbee.Json is built upon the great work of several open-source projects. Special thanks to:

- Stefan Goessner for the original [JSONPath implementation](https://goessner.net/articles/JsonPath/).
- System.Text.Json team for their work on the `System.Text.Json` library.
- Atif Aziz's C# port of Goessner's JSONPath library [.NET JSONPath](https://github.com/atifaziz/JSONPath).  
- Christoph Burgmer [JSONPath consensus effort](https://cburgmer.github.io/json-path-comparison).

## Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for more details.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
