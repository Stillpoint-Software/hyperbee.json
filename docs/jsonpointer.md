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

var root = JsonDocument.Parse(json);
var pointer = JsonPointer.Parse("/store/book/0/category");
var value = pointer.Evaluate(root);

Console.WriteLine(value); // Output: "fiction"
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

var root = JsonNode.Parse(json);
var pointer = JsonPointer.Parse("/store/book/1/category");
var value = pointer.Evaluate(root);

Console.WriteLine(value); // Output: "science"
```

## Why Choose Hyperbee.JsonPointer?

- **Fast and Efficient:** Designed for high performance and low memory usage.
- **Versatile:** Works seamlessly with both `JsonElement` and `JsonNode`.
- **Standards Compliant:** Adheres strictly to RFC 6901 for JSON Pointer.

## Additional Documentation

Additional documentation can be found in the project's `/docs` folder.
