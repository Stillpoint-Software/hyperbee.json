
# Hyperbee.Json

`Hyperbee.Json` is a high-performance JSONPath parser for .NET, supporting both `JsonElement` and `JsonNode`. 
The library is designed to be extensible, allowing support for other JSON document types.

## Features

- **Supports `JsonElement` and `JsonNode`:** Flexibility to work with different JSON representations.
- **Extensible:** Easily extended to support additional JSON document types.
- **High Performance:** Optimized for performance and efficiency.
- **Enumerable Results:** Returns an `IEnumerable` for convenient and flexible result handling.
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

string json = "{ \"store\": { \"book\": [ { \"category\": \"fiction\" }, { \"category\": \"science\" } ] } }";
JsonElement root = JsonDocument.Parse(json).RootElement;
var result = JsonPath.Select(root, "$.store.book[0].category");

Console.WriteLine(result.First()); // Output: "fiction"
```

#### Selecting Multiple Elements

```csharp
string json = "{ \"store\": { \"book\": [ { \"category\": \"fiction\" }, { \"category\": \"science\" } ] } }";
JsonElement root = JsonDocument.Parse(json).RootElement;
var result = JsonPath.Select(root, "$.store.book[*].category");

foreach (var item in result)
{
    Console.WriteLine(item); // Output: "fiction" and "science"
}
```

### Advanced Examples

#### Filtering

```csharp
string json = "{ \"store\": { \"book\": [ { \"category\": \"fiction\", \"price\": 10 }, { \"category\": \"science\", \"price\": 15 } ] } }";
JsonElement root = JsonDocument.Parse(json).RootElement;
var result = JsonPath.Select(root, "$.store.book[?(@.price > 10)]");

foreach (var item in result)
{
    Console.WriteLine(item); // Output: { "category": "science", "price": 15 }
}
```

#### Working with JsonNode

```csharp
using System.Text.Json.Nodes;

string json = "{ \"store\": { \"book\": [ { \"category\": \"fiction\" }, { \"category\": \"science\" } ] } }";
JsonNode root = JsonNode.Parse(json);
var result = JsonPath.Select(root, "$.store.book[0].category");

Console.WriteLine(result.First()); // Output: "fiction"
```

## JSONPath Syntax Reference

Here's a quick reference for JSONPath syntax supported by Hyperbee.Json:

| JSONPath           | Description                                                
|:-------------------|:-----------------------------------------------------------
| `$`                | Root object                                    
| `@`                | Current node                                 
| `.`                | Child operator                                             
| `..`               | Recursive descent  
| `*`                | Wildcard 
| `[]`               | Subscript operator
| `[,]`              | Union operator
| `[start:end:step]` | Array slice operator
| `?()`              | Filter selector
| `()`               | Filter expression

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

## Other Helper Methods

Hyperbee.Json also provides several helper methods to facilitate JSONPath operations. For more details, 
refer to our [Helper Methods Documentation](docs/helper-methods).

## Benchmarks

```
| Method                           | Filter               | Document                | Mean      | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|--------------------------------- |--------------------- |------------------------ |----------:|----------:|----------:|-------:|-------:|----------:|
| JsonPath_JsonCons_JsonElement    | $..*                 | {\r\n (...)}\r\n} [783] |  6.220 us | 1.4311 us | 0.0784 us | 1.0300 | 0.0076 |   8.45 KB |
| JsonPath_Hyperbee_JsonElement    | $..*                 | {\r\n (...)}\r\n} [783] | 10.145 us | 3.3266 us | 0.1823 us | 1.9684 | 0.0153 |   16.2 KB |
| JsonPath_Newtonsoft_JObject      | $..*                 | {\r\n (...)}\r\n} [783] | 10.646 us | 2.7453 us | 0.1505 us | 1.8158 | 0.0763 |  14.86 KB |
| JsonPath_Hyperbee_JsonNode       | $..*                 | {\r\n (...)}\r\n} [783] | 13.016 us | 0.6155 us | 0.0337 us | 2.0142 | 0.0305 |  16.53 KB |
| JsonPath_JsonEverything_JsonNode | $..*                 | {\r\n (...)}\r\n} [783] | 23.684 us | 3.8277 us | 0.2098 us | 4.4861 | 0.1831 |  36.81 KB |
|                                  |                      |                         |           |           |           |        |        |           |
| JsonPath_JsonCons_JsonElement    | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] |  5.520 us | 1.6890 us | 0.0926 us | 0.6180 |      - |   5.05 KB |
| JsonPath_Hyperbee_JsonElement    | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] |  5.646 us | 1.4311 us | 0.0784 us | 0.6256 |      - |   5.17 KB |
| JsonPath_Hyperbee_JsonNode       | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] |  8.547 us | 1.0070 us | 0.0552 us | 0.9918 | 0.0153 |   8.22 KB |
| JsonPath_Newtonsoft_JObject      | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] | 10.542 us | 3.8423 us | 0.2106 us | 1.9379 | 0.0763 |  15.84 KB |
| JsonPath_JsonEverything_JsonNode | $.sto(...).99)] [32] | {\r\n (...)}\r\n} [783] | 12.409 us | 4.7083 us | 0.2581 us | 1.9379 | 0.0458 |  15.85 KB |
|                                  |                      |                         |           |           |           |        |        |           |
| JsonPath_Hyperbee_JsonElement    | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  2.959 us | 0.4034 us | 0.0221 us | 0.3281 |      - |   2.69 KB |
| JsonPath_JsonCons_JsonElement    | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  3.291 us | 0.7091 us | 0.0389 us | 0.3929 |      - |   3.21 KB |
| JsonPath_Hyperbee_JsonNode       | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  3.616 us | 0.3865 us | 0.0212 us | 0.3853 |      - |   3.16 KB |
| JsonPath_JsonEverything_JsonNode | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  5.079 us | 1.0213 us | 0.0560 us | 0.7248 |      - |   5.96 KB |
| JsonPath_Newtonsoft_JObject      | $.store.book[0]      | {\r\n (...)}\r\n} [783] |  9.576 us | 3.1535 us | 0.1729 us | 1.7700 | 0.0153 |  14.56 KB |
```

## Comparison with Other Libraries

### JsonEverything

- **Pros:**
  - Comprehensive feature set.
  - Extensive JSON ecosystem.
  - Documentation and examples.

- **Cons:**
  - Limited support for different JSON document types.
  - Not as performant as other implementations.

### JsonCons.NET

- **Pros:**
  - High performance.

- **Cons:**
  - Does not return an enumerable result, making it less flexible, or performant, for certain operations.

### Why Choose Hyperbee.Json?

- **Multiple JSON Types:** Supports both `JsonElement` and `JsonNode`, offering greater flexibility.
- **Extensibility:** Easily extendable to support new JSON document types as needed.
- **User-Friendly:** Intuitive API with clear documentation, tests, and examples.
- **High Performance:** Optimized for speed and efficiency.
- **Enumerable Results:** Provides an `IEnumerable` for easy and flexible result handling.
- **Consensus Focused** 

## Credits

Hyperbee.Json is built upon the great work of several open-source projects. Special thanks to:

- Stefan Goessner for the [original JSONPath implementation](https://goessner.net/articles/JsonPath/).
- System.Text.Json team for their work on the `System.Text.Json` library.
- Atif Aziz [.NET JSONPath](https://github.com/atifaziz/JSONPath)  
- Christoph Burgmer [JSONPath consensus effort](https://cburgmer.github.io/json-path-comparison)

## Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for more details.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

