---
layout: default
title: JsonPatch
nav_order: 2
---

# Hyperbee JsonPatch

Hyperbee JsonPatch is a high-performance library for applying JSON patches to JSON documents, as defined in [RFC 6902](https://www.rfc-editor.org/rfc/rfc6902.html).
It supports both `JsonElement` and `JsonNode`, allowing for efficient and flexible modifications of JSON data.

## Features

- **High Performance:** Optimized for speed and efficiency.
- **Low Memory Allocations:** Designed to minimize memory usage.
- **Conformance:** Fully adheres to RFC 6902 for JSON Patch.
- **Supports both `JsonElement` and `JsonNode`:** Works seamlessly with both JSON document types.

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

var document = JsonDocument.Parse( json );
var jsonPatch = JsonSerializer.Deserialize<JsonPatch>( patch );

jsonPatch.Apply( document.RootElement, out var node );  // Apply updates a JsonNode (since elements cannot be modified)

var value = JsonPathPointer<JsonNode>.FromPointer( node, "/store/book/0/title" );
Console.WriteLine( value ); // Output: "New Book"
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
    { "op": "add", "path": "/store/book/0/title", "value": "New Book" },
    { "op": "remove", "path": "/store/book/1" }
]
""";

var node = JsonNode.Parse( json );
var jsonPatch = JsonSerializer.Deserialize<JsonPatch>( patch );

jsonPatch.Apply( node ); // Apply modifies the JsonNode in place (does rollback changes if an error occurs)

var value = JsonPathPointer<JsonNode>.FromPointer( node, "/store/book/0/title" );
Console.WriteLine( value ); // Output: "New Book"
```

### JsonPatch Operations

JsonPatch can also be created manually using `PatchOperation` objects.

```csharp
var patch = new JsonPatch(
    new PatchOperation( PatchOperationType.Add, "/store/book/0/title", From: null, "New Book" ),
    new PatchOperation( PatchOperationType.Remove, "/store/book/1", From: null, Value: null ),
);
```

### JsonDiff

JsonPatch `PatchOperation` can also be generated from the difference between two JSON documents.

```csharp
var source = JsonNode.Parse(
"""
    {
        "first": "John"
    }
""" );

var target = JsonNode.Parse(
"""
    {
        "first": "John",
        "last": "Doe"
    }
""" );

var patchOperations = JsonDiff<JsonNode>.Diff( source, target );

var patch = JsonSerializer.Serialize( patchOperations );
Console.WriteLine( patch ); // Output: [{"op":"add","path":"/last","value":"Doe"}]
```

