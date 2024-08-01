---
layout: default
title: JsonPatch
nav_order: 2
---

# Hyperbee JsonPatch

Hyperbee JsonPatch is a high-performance library for applying JSON patches to JSON documents, as defined in [RFC 6902](https://www.rfc-editor.org/rfc/rfc6902.html). It supports both `JsonElement` and `JsonNode`, allowing for efficient and flexible modifications of JSON data.

## Features

- **High Performance:** Optimized for speed and low memory allocations.
- **Supports:** `JsonElement` and `JsonNode`.
- **RFC Conformance:** Fully adheres to RFC 6902 for reliable behavior.

## Usage

### Applying a Patch

You can use JsonPatch to apply a series of operations to a JSON document.

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

var patch = """
[
  { "op": "add", "path": "/store/book/0/title", "value": "New Book" },
  { "op": "remove", "path": "/store/book/1" }
]
""";

var root = JsonDocument.Parse(json);
var patchDoc = JsonDocument.Parse(patch);
JsonPatch.Apply(root, patchDoc);

Console.WriteLine(root.RootElement.GetProperty("store").GetProperty("book")[0].GetProperty("title")); // Output: "New Book"
```

### Using JsonNode

JsonPatch also supports JsonNode for patch operations.

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

var patch = """
[
  { "op": "replace", "path": "/store/book/0/category", "value": "non-fiction" },
  { "op": "remove", "path": "/store/book/1" }
]
""";

var root = JsonNode.Parse(json);
var patchDoc = JsonNode.Parse(patch);
JsonPatch.Apply(root, patchDoc);

Console.WriteLine(root["store"]["book"][0]["category"]); // Output: "non-fiction"
```

## Why Choose Hyperbee.JsonPatch?

- **Fast and Efficient:** Designed for high performance and low memory usage.
- **Versatile:** Works seamlessly with both `JsonElement` and `JsonNode`.
- **Standards Compliant:** Adheres strictly to RFC 6902 for JSON Patch.

## Additional Documentation

Additional documentation can be found in the project's `/docs` folder.
