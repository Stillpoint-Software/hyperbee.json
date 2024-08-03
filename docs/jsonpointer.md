---
layout: default
title: JsonPointer
nav_order: 3
---

# Hyperbee JsonPointer

Hyperbee JsonPointer provides a simple and efficient way to navigate JSON documents using pointer syntax, as defined in [RFC 6901](https://www.rfc-editor.org/rfc/rfc6901.html).
It supports both `JsonElement` and `JsonNode`, making it a versatile tool for JSON manipulation in .NET.

## Features

- **High Performance:** Optimized for speed and efficiency.
- **Low Memory Allocations:** Designed to minimize memory usage.
- **Conformance:** Fully adheres to RFC 6901 for JSON Pointer.
- **Supports both `JsonElement` and `JsonNode`:** Works seamlessly with both JSON document types.

## Usage

### Basic Usage

You can use JsonPointer to retrieve a value from a JSON document.

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

var document = JsonDocument.Parse(json);
var value = JsonPathPointer<JsonElement>.FromPointer(document.RootElement, "/store/book/0/category")

Console.WriteLine(value.GetString()); // Output: "fiction"
```

### Using JsonNode

JsonPointer also supports JsonNode for pointer operations.

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

var node = JsonNode.Parse(json);
var value = JsonPointer<JsonNode>.FromPointer(node, "/store/book/1/category")

Console.WriteLine(value.GetValue<string>()); // Output: "science"
```
