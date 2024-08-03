---
layout: default
title: Overview
parent: JsonPath
nav_order: 1
---

# Hyperbee JsonPath

Hyperbee JsonPath is a high-performance JSONPath parser for `System.Text.Json`, that supports both `JsonElement` and `JsonNode`.
The library is designed to be fast and extensible, allowing support for other JSON document types and functions.

## Features

Hyperbee is fast, lightweight, fully RFC-9535 conforming, and supports **both** `JsonElement` and `JsonNode`.

- High Performance, low allocating.
- Supports **both** `JsonElement`, and `JsonNode`.
- Deferred execution queries with `IEnumerable`.
- Enhanced JsonPath syntax.
- Extendable to support additional JSON document types.
- RFC conforming JSONPath implementation.

## JSONPath RFC

Hyperbee.Json conforms to [RFC 9535](https://www.rfc-editor.org/rfc/rfc9535.html), and aims to support the [JSONPath consensus](https://cburgmer.github.io/json-path-comparison) 
when the RFC is unopinionated. When the RFC is unopinionated, and where the consensus is ambiguous or not aligned with our 
performance and usability goals, we may deviate. Our goal is to provide a robust and performant library while strengthening our alignment with the RFC and the community.

## Usage

### Selecting Elements

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
var result = JsonPath.Select(root, "$.store.book[*].category");

foreach (var item in result)
{
    Console.WriteLine(item); // Output: "fiction" and "science"
}
```

### Selecting Nodes

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
var result = JsonPath.Select(root, "$.store.book[0].category");

Console.WriteLine(result.First()); // Output: "fiction"
```

### Selecting Elements with Path

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
var (element, path) = JsonPath.SelectPath(root, "$.store.book[*].category").First();

Console.WriteLine(element); // Output: "fiction" 
Console.WriteLine(path);    // Output: "$.store.book[0].category"

```

