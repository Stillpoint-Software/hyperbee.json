﻿---
layout: default
title: Comparison
parent: JsonPath
nav_order: 4
---

## Comparison with Other Libraries

There are other excellent .NET JsonPath libraries, some with excellent communities. Hyperbee is focused on
conformance with the RFC, performance, and on supporting **both** `JsonElement` and `JsonNode`. We are very 
interested in growing our community, and invite participation if you have an idea to share, or an improvement to make.

## Hyperbee JsonPath

- **Pros:**
  - High Performance, low allocating.
  - Supports **both** `JsonElement`, and `JsonNode`.
  - Deferred execution queries with `IEnumerable`.
  - Enhanced JsonPath syntax.
  - Easy to extend.


### [JsonPath.Net](https://docs.json-everything.net/path/basics/) Json-Everything

- **Pros:**
  - Comprehensive feature set.
  - Deferred execution queries with `IEnumerable`.
  - Enhanced JsonPath syntax.
  - Strong community support.
  
- **Cons:**
  - No support for `JsonElement`.
  - More memory intensive.
  - Not quite as fast as other implementations.
   
### [JsonCons.NET](https://danielaparker.github.io/JsonCons.Net/articles/JsonPath/JsonConsJsonPath.html)

- **Pros:**
  - High performance.
  - Enhanced JsonPath syntax.

- **Cons:**
  - No support for `JsonNode`.
  - Does not return an `IEnumerable` result (no defered query execution).
  
### [Json.NET](https://www.newtonsoft.com/json) Newtonsoft

- **Pros:**
  - Comprehensive feature set.
  - Deferred execution queries with `IEnumerable`.
  - Documentation and examples.
  - Strong community support.

- **Cons:**
  - No support for `JsonElement`, or `JsonNode`.

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
