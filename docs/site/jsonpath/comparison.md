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

.NET 10, BenchmarkDotNet `ShortRun` on an Intel i9-9980HK. `NA` indicates the library does not support that RFC 9535 feature.

```
BenchmarkDotNet v0.15.8, Windows 11
Intel Core i9-9980HK CPU 2.40GHz
.NET 10.0.4, X64 RyuJIT x86-64-v3
```

 | Method                   |          Mean |         Error |        StdDev | Allocated
 | :----------------------- | ------------: | ------------: | ------------: | --------:
 | `$..[?(@.price < 10)]`
 | Hyperbee_JsonElement     |   6,589.72 ns |  13,724.37 ns |    752.279 ns |  13,408 B
 | Hyperbee_JsonNode        |   9,511.05 ns |  14,918.98 ns |    817.759 ns |  18,112 B
 | JsonCons_JsonElement     |   9,617.04 ns |   8,150.52 ns |    446.757 ns |  13,032 B
 | Newtonsoft_JObject       |  13,136.51 ns |  10,565.73 ns |    579.143 ns |  26,480 B
 | JsonEverything_JsonNode  |  24,738.29 ns |  26,520.10 ns |  1,453.656 ns |  49,304 B
 |                          |               |               |               |
 | `$..['bicycle','price']`
 | Hyperbee_JsonElement     |   2,304.00 ns |   4,789.71 ns |    262.540 ns |   3,072 B
 | JsonCons_JsonElement     |   5,411.72 ns |   2,044.44 ns |    112.063 ns |   7,304 B
 | Hyperbee_JsonNode        |   5,959.43 ns |   6,388.91 ns |    350.197 ns |   9,056 B
 | Newtonsoft_JObject       |   8,732.70 ns |   7,717.27 ns |    423.010 ns |  14,904 B
 | JsonEverything_JsonNode  |  16,081.80 ns |  32,632.20 ns |  1,788.681 ns |  29,184 B
 |                          |               |               |               |
 | `$..*`
 | Hyperbee_JsonElement     |   1,530.38 ns |     904.54 ns |     49.581 ns |   4,432 B
 | JsonCons_JsonElement     |   5,529.29 ns |   5,095.25 ns |    279.288 ns |   8,648 B
 | Hyperbee_JsonNode        |   6,375.70 ns |  13,107.14 ns |    718.447 ns |   9,768 B
 | Newtonsoft_JObject       |   8,316.71 ns |  13,631.95 ns |    747.213 ns |  14,528 B
 | JsonEverything_JsonNode  |  17,290.19 ns |  11,706.56 ns |    641.676 ns |  34,784 B
 |                          |               |               |               |
 | `$..author`
 | Hyperbee_JsonElement     |   1,624.67 ns |     304.86 ns |     16.711 ns |   3,056 B
 | JsonCons_JsonElement     |   4,369.83 ns |   3,368.40 ns |    184.633 ns |   5,640 B
 | Hyperbee_JsonNode        |   5,546.52 ns |   2,128.96 ns |    116.695 ns |   8,848 B
 | Newtonsoft_JObject       |   8,346.02 ns |  12,029.11 ns |    659.356 ns |  14,544 B
 | JsonEverything_JsonNode  |  13,113.01 ns |   6,545.11 ns |    358.760 ns |  26,728 B
 |                          |               |               |               |
 | `$..book[?@.isbn]`
 | Hyperbee_JsonElement     |   2,535.29 ns |   5,230.41 ns |    286.697 ns |   4,024 B
 | JsonCons_JsonElement     |   5,479.99 ns |   5,162.81 ns |    282.991 ns |   7,336 B
 | Hyperbee_JsonNode        |   6,270.33 ns |     952.73 ns |     52.222 ns |   9,776 B
 | JsonEverything_JsonNode  |  15,563.95 ns |  10,045.42 ns |    550.624 ns |  30,696 B
 | Newtonsoft_JObject       |            NA |            NA |            NA |        NA
 |                          |               |               |               |
 | `$..book[?@.price == 8.99 && @.category == 'fiction']`
 | Hyperbee_JsonElement     |   3,454.45 ns |   2,551.12 ns |    139.835 ns |   6,120 B
 | Hyperbee_JsonNode        |   7,541.74 ns |  10,405.88 ns |    570.381 ns |  12,000 B
 | JsonCons_JsonElement     |   8,161.76 ns |   6,143.02 ns |    336.720 ns |   8,640 B
 | JsonEverything_JsonNode  |  22,294.77 ns |  27,986.15 ns |  1,534.016 ns |  40,472 B
 | Newtonsoft_JObject       |            NA |            NA |            NA |        NA
 |                          |               |               |               |
 | `$..book[0,1]`
 | Hyperbee_JsonElement     |   1,907.37 ns |   4,405.90 ns |    241.502 ns |   3,056 B
 | JsonCons_JsonElement     |   5,556.87 ns |  13,635.13 ns |    747.387 ns |   6,248 B
 | Hyperbee_JsonNode        |   6,020.31 ns |   4,143.39 ns |    227.113 ns |   8,848 B
 | Newtonsoft_JObject       |   8,536.98 ns |   6,526.24 ns |    357.725 ns |  14,792 B
 | JsonEverything_JsonNode  |  13,979.45 ns |  19,281.48 ns |  1,056.883 ns |  27,048 B
 |                          |               |               |               |
 | `$.store..price`
 | Hyperbee_JsonElement     |   1,369.37 ns |   1,313.98 ns |     72.024 ns |   2,680 B
 | JsonCons_JsonElement     |   4,589.72 ns |   4,689.36 ns |    257.040 ns |   5,704 B
 | Hyperbee_JsonNode        |   5,263.10 ns |   4,128.49 ns |    226.296 ns |   8,576 B
 | Newtonsoft_JObject       |   8,093.64 ns |   2,103.71 ns |    115.311 ns |  14,680 B
 | JsonEverything_JsonNode  |  12,969.09 ns |  11,670.40 ns |    639.694 ns |  27,272 B
 |                          |               |               |               |
 | `$.store.* #First()`
 | Hyperbee_JsonElement     |     423.70 ns |     346.12 ns |     18.972 ns |     752 B
 | JsonCons_JsonElement     |   2,575.12 ns |     900.27 ns |     49.347 ns |   3,384 B
 | Hyperbee_JsonNode        |   2,999.84 ns |   3,209.10 ns |    175.902 ns |   2,944 B
 | JsonEverything_JsonNode  |   3,612.82 ns |   4,544.38 ns |    249.093 ns |   4,648 B
 | Newtonsoft_JObject       |   9,036.39 ns |  25,870.81 ns |  1,418.066 ns |  14,816 B
 |                          |               |               |               |
 | `$.store.*`
 | Hyperbee_JsonElement     |     437.72 ns |     309.52 ns |     16.966 ns |     712 B
 | JsonCons_JsonElement     |   2,932.44 ns |   3,752.08 ns |    205.664 ns |   3,344 B
 | Hyperbee_JsonNode        |   3,011.14 ns |   3,030.26 ns |    166.099 ns |   2,968 B
 | JsonEverything_JsonNode  |   3,630.15 ns |   1,683.62 ns |     92.285 ns |   4,912 B
 | Newtonsoft_JObject       |   7,320.97 ns |   4,230.57 ns |    231.892 ns |  14,776 B
 |                          |               |               |               |
 | `$.store.bicycle.color`
 | Hyperbee_JsonElement     |     167.21 ns |     121.79 ns |      6.676 ns |      80 B
 | JsonCons_JsonElement     |   2,815.20 ns |   1,361.83 ns |     74.646 ns |   3,304 B
 | Hyperbee_JsonNode        |   2,820.77 ns |   2,055.87 ns |    112.689 ns |   2,952 B
 | JsonEverything_JsonNode  |   4,157.10 ns |   4,348.01 ns |    238.329 ns |   5,880 B
 | Newtonsoft_JObject       |   7,383.76 ns |  12,169.49 ns |    667.051 ns |  14,840 B
 |                          |               |               |               |
 | `$.store.book[?(@.price == 8.99)]`
 | Hyperbee_JsonElement     |   1,299.44 ns |     507.79 ns |     27.834 ns |   1,984 B
 | JsonCons_JsonElement     |   4,567.67 ns |     592.57 ns |     32.481 ns |   5,176 B
 | Hyperbee_JsonNode        |   5,711.16 ns |   6,198.26 ns |    339.748 ns |   7,920 B
 | Newtonsoft_JObject       |   8,145.15 ns |   4,697.48 ns |    257.485 ns |  16,128 B
 | JsonEverything_JsonNode  |  11,231.53 ns |  31,597.52 ns |  1,731.967 ns |  15,840 B
 |                          |               |               |               |
 | `$.store.book[?(@.price > 10 && @.price < 20)]`
 | Hyperbee_JsonElement     |   2,045.34 ns |   1,340.62 ns |     73.484 ns |   3,136 B
 | JsonCons_JsonElement     |   6,306.07 ns |   3,978.97 ns |    218.101 ns |   6,384 B
 | Hyperbee_JsonNode        |   6,413.33 ns |   2,512.74 ns |    137.732 ns |   9,104 B
 | Newtonsoft_JObject       |   8,905.79 ns |  15,385.03 ns |    843.305 ns |  17,088 B
 | JsonEverything_JsonNode  |  13,237.72 ns |  19,293.59 ns |  1,057.547 ns |  22,800 B
 |                          |               |               |               |
 | `$.store.book[0]`
 | Hyperbee_JsonElement     |     171.25 ns |     332.05 ns |     18.201 ns |      80 B
 | JsonCons_JsonElement     |   2,867.70 ns |   3,098.16 ns |    169.821 ns |   3,288 B
 | Hyperbee_JsonNode        |   3,051.39 ns |   4,436.76 ns |    243.194 ns |   2,928 B
 | JsonEverything_JsonNode  |   3,961.99 ns |   1,245.96 ns |     68.295 ns |   5,816 B
 | Newtonsoft_JObject       |   7,577.69 ns |   8,275.01 ns |    453.581 ns |  14,824 B
 |                          |               |               |               |
 | `$`
 | Hyperbee_JsonElement     |      29.76 ns |      10.00 ns |      0.548 ns |      56 B
 | JsonEverything_JsonNode  |   2,361.50 ns |   2,021.81 ns |    110.822 ns |   1,928 B
 | Hyperbee_JsonNode        |   2,428.05 ns |   1,414.84 ns |     77.552 ns |   1,792 B
 | JsonCons_JsonElement     |   2,747.57 ns |   4,158.88 ns |    227.962 ns |   3,008 B
 | Newtonsoft_JObject       |   7,584.68 ns |  12,872.74 ns |    705.598 ns |  14,312 B
