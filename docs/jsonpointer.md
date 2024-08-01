---
layout: default
title: JsonPointer
nav_order: 3
---

# Hyperbee JsonPointer

Hyperbee JsonPointer provides a simple and efficient way to navigate JSON documents using pointer syntax, as defined in [RFC 6901](https://www.rfc-editor.org/rfc/rfc6901.html). It supports both `JsonElement` and `JsonNode`, making it a versatile tool for JSON manipulation in .NET.

## Features

- **High Performance:** Optimized for speed and efficiency.
- **Supports:** `JsonElement` and `JsonNode`.
- **RFC Conformance:** Fully adheres to RFC 6901 for reliable behavior.

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

## Why Choose Hyperbee JsonPointer?

- **Fast and Efficient:** Designed for high performance and low memory usage.
- **Versatile:** Works seamlessly with both `JsonElement` and `JsonNode`.
- **Standards Compliant:** Adheres strictly to RFC 6901 for JSON Pointer.

