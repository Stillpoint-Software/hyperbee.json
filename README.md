
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
| JsonPath_JsonCons_JsonElement    | $..*                 | {\r\n (...)}\r\n} [783] |  5.563 us | 1.2525 us | 0.0687 us | 1.0300 | 0.0076 |   8.45 KB |
| JsonPath_Hyperbee_JsonElement    | $..*                 | {\r\n (...)}\r\n} [783] |  9.559 us | 1.3493 us | 0.0740 us | 1.7090 | 0.0153 |  13.97 KB |
| JsonPath_Newtonsoft_JObject      | $..*                 | {\r\n (...)}\r\n} [783] | 10.372 us | 0.8962 us | 0.0491 us | 1.8158 | 0.0763 |  14.86 KB |
| JsonPath_Hyperbee_JsonNode       | $..*                 | {\r\n (...)}\r\n} [783] | 11.705 us | 5.7483 us | 0.3151 us | 1.6479 |      - |  13.92 KB |
| JsonPath_JsonEverything_JsonNode | $..*                 | {\r\n (...)}\r\n} [783] | 21.941 us | 3.2732 us | 0.1794 us | 4.4861 | 0.1831 |  36.81 KB |
|                                  |                      |                         |           |           |           |        |        |           |
| JsonPath_Hyperbee_JsonElement    | $..*|First()         | {\r\n (...)}\r\n} [783] |  3.163 us | 3.3098 us | 0.1814 us | 0.4654 | 0.0038 |   3.82 KB |
| JsonPath_JsonEverything_JsonNode | $..*|First()         | {\r\n (...)}\r\n} [783] |  3.220 us | 0.4186 us | 0.0229 us | 0.4311 |      - |   3.53 KB |
| JsonPath_Hyperbee_JsonNode       | $..*|First()         | {\r\n (...)}\r\n} [783] |  4.908 us | 0.5720 us | 0.0314 us | 0.3662 |      - |   3.11 KB |
| JsonPath_JsonCons_JsonElement    | $..*|First()         | {\r\n (...)}\r\n} [783] |  5.609 us | 2.1326 us | 0.1169 us | 1.0376 | 0.0076 |   8.48 KB |
| JsonPath_Newtonsoft_JObject      | $..*|First()         | {\r\n (...)}\r\n} [783] |  8.551 us | 0.9476 us | 0.0519 us | 1.7395 | 0.0458 |  14.22 KB |
|                                  |                      |                         |           |           |           |        |        |           |
| JsonPath_JsonCons_JsonElement    | $..price             | {\r\n (...)}\r\n} [783] |  4.628 us | 0.6357 us | 0.0348 us | 0.6866 |      - |   5.65 KB |
| JsonPath_Hyperbee_JsonElement    | $..price             | {\r\n (...)}\r\n} [783] |  4.692 us | 0.5979 us | 0.0328 us | 0.8011 | 0.0076 |   6.58 KB |
| JsonPath_Hyperbee_JsonNode       | $..price             | {\r\n (...)}\r\n} [783] |  7.585 us | 1.3128 us | 0.0720 us | 1.1139 | 0.0229 |   9.13 KB |
| JsonPath_Newtonsoft_JObject      | $..price             | {\r\n (...)}\r\n} [783] | 10.028 us | 1.0181 us | 0.0558 us | 1.7548 | 0.0763 |   14.4 KB |
| JsonPath_JsonEverything_JsonNode | $..price             | {\r\n (...)}\r\n} [783] | 16.348 us | 1.5375 us | 0.0843 us | 3.3569 | 0.0610 |  27.63 KB |
|                                  |                      |                         |           |           |           |        |        |           |
| JsonPath_Hyperbee_JsonElement    | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] |  4.106 us | 0.5003 us | 0.0274 us | 0.7401 |      - |   6.08 KB |
| JsonPath_JsonCons_JsonElement    | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] |  4.970 us | 0.5980 us | 0.0328 us | 0.6180 |      - |   5.05 KB |
| JsonPath_Hyperbee_JsonNode       | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] |  6.610 us | 1.6158 us | 0.0886 us | 1.0147 | 0.0153 |   8.34 KB |
| JsonPath_Newtonsoft_JObject      | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] |  9.670 us | 3.6023 us | 0.1975 us | 1.9379 | 0.0763 |  15.84 KB |
| JsonPath_JsonEverything_JsonNode | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] | 11.227 us | 2.5612 us | 0.1404 us | 1.9379 | 0.0458 |  15.85 KB |
|                                  |                      |                         |           |           |           |        |        |           |
| JsonPath_Hyperbee_JsonElement    | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  2.771 us | 0.1885 us | 0.0103 us | 0.3433 |      - |   2.81 KB |
| JsonPath_JsonCons_JsonElement    | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  3.026 us | 0.6708 us | 0.0368 us | 0.3929 |      - |   3.21 KB |
| JsonPath_Hyperbee_JsonNode       | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  3.260 us | 0.5807 us | 0.0318 us | 0.3815 |      - |   3.12 KB |
| JsonPath_JsonEverything_JsonNode | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  4.699 us | 0.9516 us | 0.0522 us | 0.7019 |      - |   5.96 KB |
| JsonPath_Newtonsoft_JObject      | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  8.909 us | 0.3828 us | 0.0210 us | 1.7700 | 0.0153 |  14.56 KB |
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
