---
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
 | Hyperbee_JsonElement     |   2.874 us |   1.6256 us |  0.0891 us |    3.52 KB
 | Hyperbee_JsonNode        |   3.173 us |   0.7979 us |  0.0437 us |    3.09 KB
 | JsonEverything_JsonNode  |   3.199 us |   2.4697 us |  0.1354 us |    3.53 KB
 | JsonCons_JsonElement     |   5.976 us |   8.4042 us |  0.4607 us |    8.48 KB
 | Newtonsoft_JObject       |   9.219 us |   2.9245 us |  0.1603 us |   14.22 KB
 |                          |            |             |            |           
 | `$..*`
 | JsonCons_JsonElement     |   5.674 us |   3.8650 us |  0.2119 us |    8.45 KB
 | Hyperbee_JsonElement     |   7.934 us |   3.5907 us |  0.1968 us |    9.13 KB
 | Hyperbee_JsonNode        |  10.457 us |   7.7120 us |  0.4227 us |   10.91 KB
 | Newtonsoft_JObject       |  10.722 us |   4.1310 us |  0.2264 us |   14.86 KB
 | JsonEverything_JsonNode  |  23.096 us |  10.8629 us |  0.5954 us |   36.81 KB
 |                          |            |             |            |           
 | `$..price`
 | Hyperbee_JsonElement     |   4.428 us |   4.6731 us |  0.2561 us |     4.2 KB
 | JsonCons_JsonElement     |   5.355 us |   1.1624 us |  0.0637 us |    5.65 KB
 | Hyperbee_JsonNode        |   7.931 us |   0.6970 us |  0.0382 us |    7.48 KB
 | Newtonsoft_JObject       |  10.334 us |   8.2331 us |  0.4513 us |    14.4 KB
 | JsonEverything_JsonNode  |  17.000 us |  14.9812 us |  0.8212 us |   27.63 KB
 |                          |            |             |            |           
 | `$.store.book[?(@.price == 8.99)]`
 | Hyperbee_JsonElement     |   4.153 us |   3.6089 us |  0.1978 us |    5.24 KB
 | JsonCons_JsonElement     |   4.873 us |   1.0395 us |  0.0570 us |    5.05 KB
 | Hyperbee_JsonNode        |   6.980 us |   5.1007 us |  0.2796 us |       8 KB
 | Newtonsoft_JObject       |  10.629 us |   3.9096 us |  0.2143 us |   15.84 KB
 | JsonEverything_JsonNode  |  11.133 us |   7.2544 us |  0.3976 us |   15.85 KB
 |                          |            |             |            |           
 | `$.store.book[0]`
 | Hyperbee_JsonElement     |   2.677 us |   2.2733 us |  0.1246 us |    2.27 KB
 | Hyperbee_JsonNode        |   3.126 us |   3.5345 us |  0.1937 us |    2.77 KB
 | JsonCons_JsonElement     |   3.229 us |   0.0681 us |  0.0037 us |    3.21 KB
 | JsonEverything_JsonNode  |   4.612 us |   2.0037 us |  0.1098 us |    5.96 KB
 | Newtonsoft_JObject       |   9.627 us |   1.1498 us |  0.0630 us |   14.56 KB
