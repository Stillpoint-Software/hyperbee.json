```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3037)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 9.0.103
  [Host]   : .NET 9.0.2 (9.0.225.6610), X64 RyuJIT AVX2 [AttachedDebugger]
  ShortRun : .NET 9.0.2 (9.0.225.6610), X64 RyuJIT AVX2


 | Method                   |       Mean |      Error |     StdDev |  Allocated
 | :----------------------- | ---------: | ---------: | ---------: | ---------:
 | `$..* First()`
 | Hyperbee_JsonElement     |   1.867 μs |  0.6072 μs |  0.0333 μs |     3.5 KB
 | Hyperbee_JsonNode        |   1.916 μs |  0.2202 μs |  0.0121 μs |    3.11 KB
 | JsonEverything_JsonNode  |   2.097 μs |  0.7484 μs |  0.0410 μs |    3.49 KB
 | JsonCons_JsonElement     |   3.498 μs |  0.8161 μs |  0.0447 μs |    8.48 KB
 | Newtonsoft_JObject       |   5.734 μs |  1.1777 μs |  0.0646 μs |   14.22 KB
 |                          |            |            |            |           
 | `$..*`
 | JsonCons_JsonElement     |   3.525 μs |  1.0092 μs |  0.0553 μs |    8.45 KB
 | Hyperbee_JsonElement     |   4.288 μs |  0.4672 μs |  0.0256 μs |    8.38 KB
 | Hyperbee_JsonNode        |   5.744 μs |  0.8133 μs |  0.0446 μs |   11.22 KB
 | Newtonsoft_JObject       |   7.111 μs |  1.1213 μs |  0.0615 μs |   14.43 KB
 | JsonEverything_JsonNode  |  18.571 μs |  5.4711 μs |  0.2999 μs |    34.2 KB
 |                          |            |            |            |           
 | `$..price`
 | Hyperbee_JsonElement     |   2.599 μs |  0.2500 μs |  0.0137 μs |    4.11 KB
 | JsonCons_JsonElement     |   2.962 μs |  0.1946 μs |  0.0107 μs |    5.65 KB
 | Hyperbee_JsonNode        |   4.107 μs |  0.8757 μs |  0.0480 μs |    8.22 KB
 | Newtonsoft_JObject       |   6.653 μs |  1.2770 μs |  0.0700 μs |   14.26 KB
 | JsonEverything_JsonNode  |  13.430 μs |  4.4075 μs |  0.2416 μs |   26.46 KB
 |                          |            |            |            |           
 | `$.store.book[?(@.price == 8.99)]`
 | Hyperbee_JsonElement     |   2.778 μs |  0.3989 μs |  0.0219 μs |    5.41 KB
 | JsonCons_JsonElement     |   3.282 μs |  0.5416 μs |  0.0297 μs |    5.05 KB
 | Hyperbee_JsonNode        |   4.279 μs |  0.4826 μs |  0.0265 μs |    8.95 KB
 | Newtonsoft_JObject       |   5.986 μs |  0.7627 μs |  0.0418 μs |   15.78 KB
 | JsonEverything_JsonNode  |   7.135 μs |  1.1539 μs |  0.0632 μs |    15.5 KB
 |                          |            |            |            |           
 | `$.store.book[0]`
 | Hyperbee_JsonElement     |   1.623 μs |  0.3592 μs |  0.0197 μs |    2.27 KB
 | Hyperbee_JsonNode        |   1.925 μs |  0.1748 μs |  0.0096 μs |    2.86 KB
 | JsonCons_JsonElement     |   1.937 μs |  0.1595 μs |  0.0087 μs |    3.21 KB
 | JsonEverything_JsonNode  |   3.095 μs |  0.4032 μs |  0.0221 μs |    5.71 KB
 | Newtonsoft_JObject       |   5.690 μs |  1.3949 μs |  0.0765 μs |   14.51 KB
```
