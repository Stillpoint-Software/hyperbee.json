
# Hyperbee.Json

`Hyperbee.Json` is a high-performance JSONPath parser for .NET, supporting both `JsonElement` and `JsonNode`. 
The library is designed to be quick and extensible, allowing support for other JSON document types.

## Features

- **High Performance:** Optimized for performance and efficiency.
- **Supports:** `JsonElement` and `JsonNode`.
- **Extensible:** Easily extended to support additional JSON document types.
- **`IEnumerable` Results:** Deferred execution queries with `IEnumerable`.
- **Compliant:** Adheres to the JSONPath Specification [RFC 9535](https://www.rfc-editor.org/rfc/rfc9535.html). 

## JSONPath Consensus

Hyperbee.Json aims to follow the emerging JSONPath consensus standard where applicable. This standardization 
effort is critical for ensuring consistent behavior across different implementations of JSONPath. However, 
where the consensus is ambiguous or not aligned with our performance and usability goals, we may deviate. 
Our goal is always to provide a robust and performant library while keeping an eye on standardization progress.

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

var root = JsonDocument.Parse(json).RootElement;
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

var root = JsonDocument.Parse(json).RootElement;
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

var root = JsonDocument.Parse(json).RootElement;
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
| Method                           | Filter               | Document                | Mean     | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|--------------------------------- |--------------------- |------------------------ |---------:|----------:|----------:|-------:|-------:|----------:|
| JsonPath_Hyperbee_JsonElement    | $..*                 | {\r\n (...)}\r\n} [783] | 3.044 us | 0.7532 us | 0.0413 us | 0.4501 | 0.0038 |   3.69 KB |
| JsonPath_Hyperbee_JsonNode       | $..*                 | {\r\n (...)}\r\n} [783] | 3.185 us | 0.9419 us | 0.0516 us | 0.3738 |      - |   3.08 KB |
| JsonPath_JsonEverything_JsonNode | $..*                 | {\r\n (...)}\r\n} [783] | 4.336 us | 8.1303 us | 0.4456 us | 0.4272 |      - |    3.5 KB |
| JsonPath_JsonCons_JsonElement    | $..*                 | {\r\n (...)}\r\n} [783] | 5.808 us | 1.1534 us | 0.0632 us | 1.0300 | 0.0076 |   8.45 KB |
| JsonPath_Newtonsoft_JObject      | $..*                 | {\r\n (...)}\r\n} [783] | 8.585 us | 2.3822 us | 0.1306 us | 1.7242 | 0.0610 |  14.19 KB |
|                                  |                      |                         |          |           |           |        |        |           |
| JsonPath_JsonCons_JsonElement    | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] | 5.070 us | 1.4743 us | 0.0808 us | 0.6180 |      - |   5.05 KB |
| JsonPath_Hyperbee_JsonElement    | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] | 5.071 us | 0.5728 us | 0.0314 us | 0.6104 |      - |   5.05 KB |
| JsonPath_Hyperbee_JsonNode       | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] | 7.758 us | 0.2118 us | 0.0116 us | 0.9613 |      - |   7.95 KB |
| JsonPath_JsonEverything_JsonNode | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] | 9.449 us | 1.3955 us | 0.0765 us | 1.6327 | 0.0305 |  13.41 KB |
| JsonPath_Newtonsoft_JObject      | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] | 9.643 us | 0.8254 us | 0.0452 us | 1.8921 | 0.0610 |  15.54 KB |
|                                  |                      |                         |          |           |           |        |        |           |
| JsonPath_Hyperbee_JsonElement    | $.store.book[0]      | {\r\n (...)}\r\n} [783] | 2.735 us | 1.2518 us | 0.0686 us | 0.3128 |      - |   2.56 KB |
| JsonPath_JsonCons_JsonElement    | $.store.book[0]      | {\r\n (...)}\r\n} [783] | 3.028 us | 0.9835 us | 0.0539 us | 0.3929 |      - |   3.21 KB |
| JsonPath_Hyperbee_JsonNode       | $.store.book[0]      | {\r\n (...)}\r\n} [783] | 3.150 us | 0.5729 us | 0.0314 us | 0.3662 |      - |   3.02 KB |
| JsonPath_JsonEverything_JsonNode | $.store.book[0]      | {\r\n (...)}\r\n} [783] | 4.323 us | 1.0550 us | 0.0578 us | 0.7172 | 0.0076 |   5.88 KB |
| JsonPath_Newtonsoft_JObject      | $.store.book[0]      | {\r\n (...)}\r\n} [783] | 8.405 us | 0.8728 us | 0.0478 us | 1.7700 | 0.0610 |  14.48 KB |
```

## Comparison with Other Libraries

There are excellent options available for RFC-9535 .NET JsonPath.

### JsonEverything

- **Pros:**
  - Extensive JSON ecosystem.
  - Comprehensive feature set.
  - Deferred execution queries with `IEnumerable`.

- **Cons:**
  - Not as fast as other implementations.
  - No support for `JsonElement`.

### JsonCons.NET

- **Pros:**
  - High performance.
  - Enhanced JsonPath syntax.

- **Cons:**
  - Uses Net Standard 2.1
  - Does not return an `IEnumerable` result (no defered query execution).
  making it less efficient for certain operations.

### Json.Net

- **Pros:**
  - Comprehensive feature set.
  - Documentation and examples.

- **Cons:**
  - Does not support `System.Text.Json`.

### Why Choose Hyperbee.Json?

- High Performance.
- Supports `JsonElement`, and `JsonNode`.
- Extendable to support additional JSON document types.
- Deferred execution queries with `IEnumerable`.
- Consensus Focused 

## Credits

Hyperbee.Json is built upon the great work of several open-source projects. Special thanks to:

- Stefan Goessner for the original [JSONPath implementation](https://goessner.net/articles/JsonPath/).
- System.Text.Json team for their work on the `System.Text.Json` library.
- Atif Aziz [.NET JSONPath](https://github.com/atifaziz/JSONPath).  
- Christoph Burgmer [JSONPath consensus effort](https://cburgmer.github.io/json-path-comparison).

## Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for more details.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
