---
layout: default
title: Hyperbee Json
nav_order: 1
---

# Welcome to Hyperbee Json

Hyperbee Json is a high-performance JSON library for .NET, providing robust support for JSONPath, JsonPointer, JsonPatch, and JsonDiff. 
This library is optimized for speed and low memory allocations, and it adheres to relevant RFCs to ensure reliable and predictable behavior.

Unlike other libraries that support only `JsonElement` or `JsonNode`, Hyperbee Json supports both types, and can be easily extended to 
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

[Read more about JsonPath]({{ site.url }}{{ site.baseurl }}jsonpath/jsonpath.html)

## JSONPointer

JSONPointer is a syntax for identifying a specific value within a JSON document. It is simple and easy to use, making it an excellent 
choice for pinpointing exact values. Hyperbee.Json's JsonPointer implementation adheres to [RFC 6901](https://www.rfc-editor.org/rfc/rfc6901.html).

[Read more about JsonPointer]({{ site.url }}{{ site.baseurl }}jsonpointer.html)

## JSONPatch

JSONPatch is a format for describing changes to a JSON document. It allows you to apply partial modifications to JSON data efficiently. 
Hyperbee.Json supports JsonPatch as defined in [RFC 6902](https://www.rfc-editor.org/rfc/rfc6902.html), ensuring compatibility and reliability.

[Read more about JsonPatch]({{ site.url }}{{ site.baseurl }}jsonpatch.html)

## JSONDiff

JSONDiff allows you to compute the difference between two JSON documents, which is useful for versioning and synchronization. 
Hyperbee.Json's implementation is optimized for performance and low memory usage, adhering to the standards set in [RFC 6902](https://www.rfc-editor.org/rfc/rfc6902.html).

[Read more about JsonDiff]({{ site.url }}{{ site.baseurl }}jsonpatch.html#jsondiff)

## Getting Started

To get started with Hyperbee.Json, refer to the documentation for detailed instructions and examples. Install the library via NuGet:

Install via NuGet:

```bash
dotnet add package Hyperbee.Json
```

## Credits

Hyperbee.Json is built upon the great work of several open-source projects. Special thanks to:

- System.Text.Json team for their work on the `System.Text.Json` library.
- Stefan Gossner for the original [JSONPath implementation](https://goessner.net/articles/JsonPath/).
- Atif Aziz's C# port of Goessner's JSONPath library [.NET JSONPath](https://github.com/atifaziz/JSONPath).  
- [JSONPath Compliance Test Suite Team](https://github.com/jsonpath-standard/jsonpath-compliance-test-suite).
- Christoph Burgmer [JSONPath consensus effort](https://cburgmer.github.io/json-path-comparison).
- [Just The Docs](https://github.com/just-the-docs/just-the-docs) for the documentation theme.

## Contributing

We welcome contributions! Please see our [Contributing Guide](https://github.com/Stillpoint-Software/.github/blob/main/.github/CONTRIBUTING.md) 
for more details.
