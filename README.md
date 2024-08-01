# Welcome to Hyperbee.Json

Hyperbee.Json is a high-performance JSON library for .NET, providing robust support for JSONPath, JsonPointer, JsonPatch, and JsonDiff. 
This library is optimized for speed and low memory allocations, and it adheres to relevant RFCs to ensure reliable and predictable behavior.

Unlike other libraries that support only `JsonElement` or `JsonNode`, Hyperbee.Json supports **both** types, and can be easily extended to 
support additional document types and functions.

## Features

- **High Performance:** Optimized for performance and efficiency.
- **Low Memory Allocations:** Designed to minimize memory usage.
- **Comprehensive JSON Support:** Supports JSONPath, JsonPointer, JsonPatch, and JsonDiff.
- **Conformance:** Adheres to the JSONPath Specification [RFC 9535](https://www.rfc-editor.org/rfc/rfc9535.html), JSONPointer [RFC 6901](https://www.rfc-editor.org/rfc/rfc6901.html), and JsonPatch [RFC 6902](https://www.rfc-editor.org/rfc/rfc6902.html).
- **Supports both `JsonElement` and `JsonNode`:** Works seamlessly with both JSON document types.

## JSONPath

JSONPath is a query language for JSON, allowing you to navigate and extract data from JSON documents using a set of path expressions. 
Hyperbee.Json's JSONPath implementation is designed for optimal performance, ensuring low memory allocations and fast query execution. 
It fully conforms to [RFC 9535](https://www.rfc-editor.org/rfc/rfc9535.html).

- [Read more about JsonPath](https://stillpoint-software.github.io/hyperbee.json/jsonpath/jsonpath.html)  
- [Read more about JsonPath syntax](https://stillpoint-software.github.io/hyperbee.json/jsonpath/syntax.html)

## JSONPointer

JSONPointer is a syntax for identifying a specific value within a JSON document. It is simple and easy to use, making it an excellent 
choice for pinpointing exact values. Hyperbee.Json's JsonPointer implementation adheres to [RFC 6901](https://www.rfc-editor.org/rfc/rfc6901.html).

- [Read more about JsonPointer](https://stillpoint-software.github.io/hyperbee.json/jsonpointer.html)

## JSONPatch

JSONPatch is a format for describing changes to a JSON document. It allows you to apply partial modifications to JSON data efficiently. 
Hyperbee.Json supports JsonPatch as defined in [RFC 6902](https://www.rfc-editor.org/rfc/rfc6902.html), ensuring compatibility and reliability.

- [Read more about JsonPatch](https://stillpoint-software.github.io/hyperbee.json/jsonpatch.html)

## JSONDiff

JSONDiff allows you to compute the difference between two JSON documents, which is useful for versioning and synchronization. 
Hyperbee.Json's implementation is optimized for performance and low memory usage, adhering to the standards set in [RFC 6902](https://www.rfc-editor.org/rfc/rfc6902.html).

- [Read more about JsonDiff](https://stillpoint-software.github.io/hyperbee.json/jsonpatch.html#jsondiff)

## Getting Started

To get started with Hyperbee.Json, refer to the documentation for detailed instructions and examples. Install the library via NuGet:

Install via NuGet:

```bash
dotnet add package Hyperbee.Json
```

## Documentation

Documentation can be found in the project's `/docs` folder.

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

 | Method                   |       Mean |       Error |     StdDev |  Allocated
 | :----------------------- | ---------: | ----------: | ---------: | ---------:
 | `$..* First()`
 | Hyperbee_JsonElement     |   2.874 μs |   1.6256 μs |  0.0891 μs |    3.52 KB
 | Hyperbee_JsonNode        |   3.173 μs |   0.7979 μs |  0.0437 μs |    3.09 KB
 | JsonEverything_JsonNode  |   3.199 μs |   2.4697 μs |  0.1354 μs |    3.53 KB
 | JsonCons_JsonElement     |   5.976 μs |   8.4042 μs |  0.4607 μs |    8.48 KB
 | Newtonsoft_JObject       |   9.219 μs |   2.9245 μs |  0.1603 μs |   14.22 KB
 |                          |            |             |            |           
 | `$..*`
 | JsonCons_JsonElement     |   5.674 μs |   3.8650 μs |  0.2119 μs |    8.45 KB
 | Hyperbee_JsonElement     |   7.934 μs |   3.5907 μs |  0.1968 μs |    9.13 KB
 | Hyperbee_JsonNode        |  10.457 μs |   7.7120 μs |  0.4227 μs |   10.91 KB
 | Newtonsoft_JObject       |  10.722 μs |   4.1310 μs |  0.2264 μs |   14.86 KB
 | JsonEverything_JsonNode  |  23.096 μs |  10.8629 μs |  0.5954 μs |   36.81 KB
 |                          |            |             |            |           
 | `$..price`
 | Hyperbee_JsonElement     |   4.428 μs |   4.6731 μs |  0.2561 μs |     4.2 KB
 | JsonCons_JsonElement     |   5.355 μs |   1.1624 μs |  0.0637 μs |    5.65 KB
 | Hyperbee_JsonNode        |   7.931 μs |   0.6970 μs |  0.0382 μs |    7.48 KB
 | Newtonsoft_JObject       |  10.334 μs |   8.2331 μs |  0.4513 μs |    14.4 KB
 | JsonEverything_JsonNode  |  17.000 μs |  14.9812 μs |  0.8212 μs |   27.63 KB
 |                          |            |             |            |           
 | `$.store.book[?(@.price == 8.99)]`
 | Hyperbee_JsonElement     |   4.153 μs |   3.6089 μs |  0.1978 μs |    5.24 KB
 | JsonCons_JsonElement     |   4.873 μs |   1.0395 μs |  0.0570 μs |    5.05 KB
 | Hyperbee_JsonNode        |   6.980 μs |   5.1007 μs |  0.2796 μs |       8 KB
 | Newtonsoft_JObject       |  10.629 μs |   3.9096 μs |  0.2143 μs |   15.84 KB
 | JsonEverything_JsonNode  |  11.133 μs |   7.2544 μs |  0.3976 μs |   15.85 KB
 |                          |            |             |            |           
 | `$.store.book[0]`
 | Hyperbee_JsonElement     |   2.677 μs |   2.2733 μs |  0.1246 μs |    2.27 KB
 | Hyperbee_JsonNode        |   3.126 μs |   3.5345 μs |  0.1937 μs |    2.77 KB
 | JsonCons_JsonElement     |   3.229 μs |   0.0681 μs |  0.0037 μs |    3.21 KB
 | JsonEverything_JsonNode  |   4.612 μs |   2.0037 μs |  0.1098 μs |    5.96 KB
 | Newtonsoft_JObject       |   9.627 μs |   1.1498 μs |  0.0630 μs |   14.56 KB

## Credits

Hyperbee.Json is built upon the great work of several open-source projects. Special thanks to:

- System.Text.Json team for their work on the `System.Text.Json` library.
- Stefan Goessner for the original [JSONPath implementation](https://goessner.net/articles/JsonPath/).
- Atif Aziz's C# port of Goessner's JSONPath library [.NET JSONPath](https://github.com/atifaziz/JSONPath).  
- Christoph Burgmer [JSONPath consensus effort](https://cburgmer.github.io/json-path-comparison).
- [JSONPath Compliance Test Suite Team](https://github.com/jsonpath-standard/jsonpath-compliance-test-suite).

## Contributing

We welcome contributions! Please see our [Contributing Guide](https://github.com/Stillpoint-Software/.github/blob/main/.github/CONTRIBUTING.md) for more details.
