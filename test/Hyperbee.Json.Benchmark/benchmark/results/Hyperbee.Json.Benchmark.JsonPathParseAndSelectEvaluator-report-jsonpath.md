```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3880/23H2/2023Update/SunValley3)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.302
  [Host]   : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2 DEBUG
  ShortRun : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2


 | Method                   |       Mean |      Error |     StdDev |  Allocated
 | :----------------------- | ---------: | ---------: | ---------: | ---------:
 | `$..* `First()``
 | Hyperbee_JsonElement     |   3.226 μs |  0.6858 μs |  0.0376 μs |    3.52 KB
 | Hyperbee_JsonNode        |   3.403 μs |  1.3666 μs |  0.0749 μs |    3.09 KB
 | JsonEverything_JsonNode  |   3.408 μs |  0.7708 μs |  0.0423 μs |    3.53 KB
 | JsonCons_JsonElement     |   6.340 μs |  2.8437 μs |  0.1559 μs |    8.48 KB
 | Newtonsoft_JObject       |   9.130 μs |  5.0118 μs |  0.2747 μs |   14.22 KB
 |                          |            |            |            |           
 | `$..*`
 | JsonCons_JsonElement     |   6.288 μs |  1.6933 μs |  0.0928 μs |    8.45 KB
 | Hyperbee_JsonElement     |   8.484 μs |  8.2180 μs |  0.4505 μs |    9.13 KB
 | Newtonsoft_JObject       |  10.413 μs |  7.0744 μs |  0.3878 μs |   14.86 KB
 | Hyperbee_JsonNode        |  10.737 μs |  0.8507 μs |  0.0466 μs |   10.91 KB
 | JsonEverything_JsonNode  |  24.210 μs |  7.8836 μs |  0.4321 μs |   36.81 KB
 |                          |            |            |            |           
 | `$..price`
 | Hyperbee_JsonElement     |   4.667 μs |  1.9273 μs |  0.1056 μs |     4.2 KB
 | JsonCons_JsonElement     |   5.352 μs |  6.3938 μs |  0.3505 μs |    5.65 KB
 | Hyperbee_JsonNode        |   7.734 μs |  5.8949 μs |  0.3231 μs |    7.48 KB
 | Newtonsoft_JObject       |  10.164 μs |  2.3366 μs |  0.1281 μs |    14.4 KB
 | JsonEverything_JsonNode  |  17.801 μs |  8.6851 μs |  0.4761 μs |   27.63 KB
 |                          |            |            |            |           
 | `$.store.book[?(@.price == 8.99)]`
 | Hyperbee_JsonElement     |   4.351 μs |  3.7892 μs |  0.2077 μs |    5.24 KB
 | JsonCons_JsonElement     |   5.239 μs |  4.2647 μs |  0.2338 μs |    5.05 KB
 | Hyperbee_JsonNode        |   7.101 μs |  2.4381 μs |  0.1336 μs |       8 KB
 | Newtonsoft_JObject       |  10.409 μs |  9.4057 μs |  0.5156 μs |   15.84 KB
 | JsonEverything_JsonNode  |  11.744 μs |  9.9334 μs |  0.5445 μs |   15.85 KB
 |                          |            |            |            |           
 | `$.store.book[0]`
 | JsonCons_JsonElement     |   3.232 μs |  1.2559 μs |  0.0688 μs |    3.21 KB
 | Hyperbee_JsonElement     |   3.269 μs |  5.8324 μs |  0.3197 μs |    2.27 KB
 | Hyperbee_JsonNode        |   3.313 μs |  2.3646 μs |  0.1296 μs |    2.77 KB
 | JsonEverything_JsonNode  |   5.090 μs |  5.4770 μs |  0.3002 μs |    5.96 KB
 | Newtonsoft_JObject       |   9.542 μs |  1.5667 μs |  0.0859 μs |   14.56 KB
```
