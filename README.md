
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
| Method                           | Filter               | Document                | Mean      | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|--------------------------------- |--------------------- |------------------------ |----------:|----------:|----------:|-------:|-------:|----------:|
| JsonPath_JsonCons_JsonElement    | $..*                 | {\r\n (...)}\r\n} [783] |  5.641 us | 1.7028 us | 0.0933 us | 1.0834 | 0.0076 |   8.89 KB |
| JsonPath_Hyperbee_JsonElement    | $..*                 | {\r\n (...)}\r\n} [783] |  9.282 us | 0.3754 us | 0.0206 us | 1.7090 | 0.0153 |  13.97 KB |
| JsonPath_Newtonsoft_JObject      | $..*                 | {\r\n (...)}\r\n} [783] | 10.253 us | 0.3012 us | 0.0165 us | 1.8158 | 0.0763 |  14.86 KB |
| JsonPath_Hyperbee_JsonNode       | $..*                 | {\r\n (...)}\r\n} [783] | 11.766 us | 1.6505 us | 0.0905 us | 1.6937 | 0.0305 |  13.92 KB |
| JsonPath_JsonEverything_JsonNode | $..*                 | {\r\n (...)}\r\n} [783] | 21.374 us | 1.5675 us | 0.0859 us | 4.4861 | 0.1831 |  36.81 KB |
|                                  |                      |                         |           |           |           |        |        |           |
| JsonPath_Hyperbee_JsonElement    | $..price             | {\r\n (...)}\r\n} [783] |  4.643 us | 0.4466 us | 0.0245 us | 0.8011 | 0.0076 |   6.58 KB |
| JsonPath_JsonCons_JsonElement    | $..price             | {\r\n (...)}\r\n} [783] |  4.687 us | 3.8371 us | 0.2103 us | 0.7019 |      - |   5.75 KB |
| JsonPath_Hyperbee_JsonNode       | $..price             | {\r\n (...)}\r\n} [783] |  7.912 us | 2.6792 us | 0.1469 us | 1.1139 | 0.0153 |   9.13 KB |
| JsonPath_Newtonsoft_JObject      | $..price             | {\r\n (...)}\r\n} [783] |  9.949 us | 2.2783 us | 0.1249 us | 1.7548 | 0.0763 |   14.4 KB |
| JsonPath_JsonEverything_JsonNode | $..price             | {\r\n (...)}\r\n} [783] | 16.051 us | 2.1267 us | 0.1166 us | 3.3569 | 0.0916 |  27.63 KB |
|                                  |                      |                         |           |           |           |        |        |           |
| JsonPath_Hyperbee_JsonElement    | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] |  4.213 us | 2.6720 us | 0.1465 us | 0.7401 |      - |   6.08 KB |
| JsonPath_JsonCons_JsonElement    | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] |  5.199 us | 0.3090 us | 0.0169 us | 0.6180 |      - |   5.09 KB |
| JsonPath_Hyperbee_JsonNode       | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] |  7.187 us | 4.2940 us | 0.2354 us | 1.0147 | 0.0153 |   8.34 KB |
| JsonPath_Newtonsoft_JObject      | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] |  9.893 us | 1.8922 us | 0.1037 us | 1.9379 | 0.0763 |  15.84 KB |
| JsonPath_JsonEverything_JsonNode | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] | 11.318 us | 0.6778 us | 0.0372 us | 1.9379 | 0.0458 |  15.85 KB |
|                                  |                      |                         |           |           |           |        |        |           |
| JsonPath_Hyperbee_JsonElement    | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  2.741 us | 0.0894 us | 0.0049 us | 0.3433 |      - |   2.81 KB |
| JsonPath_JsonCons_JsonElement    | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  3.019 us | 0.2138 us | 0.0117 us | 0.3967 |      - |   3.25 KB |
| JsonPath_Hyperbee_JsonNode       | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  3.262 us | 0.3993 us | 0.0219 us | 0.3815 |      - |   3.12 KB |
| JsonPath_JsonEverything_JsonNode | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  4.486 us | 0.4810 us | 0.0264 us | 0.7248 |      - |   5.96 KB |
| JsonPath_Newtonsoft_JObject      | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  8.930 us | 0.3298 us | 0.0181 us | 1.7700 | 0.0153 |  14.56 KB |
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
