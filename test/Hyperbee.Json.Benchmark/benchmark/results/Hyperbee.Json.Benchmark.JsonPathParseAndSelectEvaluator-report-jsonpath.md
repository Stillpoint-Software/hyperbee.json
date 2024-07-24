```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3880/23H2/2023Update/SunValley3)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.302
  [Host]   : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2


 | Method                   |       Mean |       Error |     StdDev |  Allocated
 | :----------------------- | ---------: | ----------: | ---------: | ---------:
 | `$..* First()`
 | Hyperbee_JsonElement     |   3.068 μs |   2.9479 μs |  0.1616 μs |    3.52 KB
 | JsonEverything_JsonNode  |   3.282 μs |   2.9167 μs |  0.1599 μs |    3.53 KB
 | Hyperbee_JsonNode        |   3.345 μs |   0.8993 μs |  0.0493 μs |    3.09 KB
 | JsonCons_JsonElement     |   6.060 μs |   3.5116 μs |  0.1925 μs |    8.48 KB
 | Newtonsoft_JObject       |   8.957 μs |   9.6253 μs |  0.5276 μs |   14.22 KB
 |                          |            |             |            |           
 | `$..*`
 | JsonCons_JsonElement     |   6.103 μs |   3.4570 μs |  0.1895 μs |    8.45 KB
 | Hyperbee_JsonElement     |   8.121 μs |   7.3217 μs |  0.4013 μs |    9.13 KB
 | Hyperbee_JsonNode        |  10.001 μs |   7.1708 μs |  0.3931 μs |   10.91 KB
 | Newtonsoft_JObject       |  10.987 μs |   0.1412 μs |  0.0077 μs |   14.86 KB
 | JsonEverything_JsonNode  |  23.377 μs |   3.3427 μs |  0.1832 μs |   36.81 KB
 |                          |            |             |            |           
 | `$..price`
 | Hyperbee_JsonElement     |   4.815 μs |   0.9764 μs |  0.0535 μs |     4.2 KB
 | JsonCons_JsonElement     |   4.889 μs |   3.6228 μs |  0.1986 μs |    5.65 KB
 | Hyperbee_JsonNode        |   7.870 μs |   1.1390 μs |  0.0624 μs |    7.48 KB
 | Newtonsoft_JObject       |  10.549 μs |   4.7006 μs |  0.2577 μs |    14.4 KB
 | JsonEverything_JsonNode  |  17.070 μs |  11.7831 μs |  0.6459 μs |   27.63 KB
 |                          |            |             |            |           
 | `$.store.book[?(@.price == 8.99)]`
 | Hyperbee_JsonElement     |   4.389 μs |   3.2924 μs |  0.1805 μs |    5.24 KB
 | JsonCons_JsonElement     |   5.390 μs |   0.3045 μs |  0.0167 μs |    5.05 KB
 | Hyperbee_JsonNode        |   7.378 μs |   0.1344 μs |  0.0074 μs |       8 KB
 | Newtonsoft_JObject       |  10.300 μs |   5.4682 μs |  0.2997 μs |   15.84 KB
 | JsonEverything_JsonNode  |  12.394 μs |   4.1752 μs |  0.2289 μs |   15.85 KB
 |                          |            |             |            |           
 | `$.store.book[0]`
 | Hyperbee_JsonElement     |   2.881 μs |   0.5628 μs |  0.0308 μs |    2.27 KB
 | JsonCons_JsonElement     |   3.303 μs |   0.1674 μs |  0.0092 μs |    3.21 KB
 | Hyperbee_JsonNode        |   3.344 μs |   0.4628 μs |  0.0254 μs |    2.77 KB
 | JsonEverything_JsonNode  |   4.889 μs |   2.8235 μs |  0.1548 μs |    5.96 KB
 | Newtonsoft_JObject       |   9.691 μs |   1.3881 μs |  0.0761 μs |   14.56 KB
```
