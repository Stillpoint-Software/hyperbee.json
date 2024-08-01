```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3958/23H2/2023Update/SunValley3)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.302
  [Host]   : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2


 | Method                   |       Mean |       Error |     StdDev |  Allocated
 | :----------------------- | ---------: | ----------: | ---------: | ---------:
 | `$..* First()`
 | Hyperbee_JsonElement     |   3.075 μs |   0.0128 μs |  0.0007 μs |     3.5 KB
 | Hyperbee_JsonNode        |   3.183 μs |   2.5776 μs |  0.1413 μs |    3.07 KB
 | JsonEverything_JsonNode  |   3.206 μs |   3.7096 μs |  0.2033 μs |    3.53 KB
 | JsonCons_JsonElement     |   6.136 μs |   0.7476 μs |  0.0410 μs |    8.48 KB
 | Newtonsoft_JObject       |   8.829 μs |  10.1411 μs |  0.5559 μs |   14.22 KB
 |                          |            |             |            |           
 | `$..*`
 | JsonCons_JsonElement     |   5.595 μs |   0.8702 μs |  0.0477 μs |    8.45 KB
 | Hyperbee_JsonElement     |   7.152 μs |   5.2088 μs |  0.2855 μs |    9.09 KB
 | Hyperbee_JsonNode        |   9.769 μs |   9.4033 μs |  0.5154 μs |   10.86 KB
 | Newtonsoft_JObject       |   9.780 μs |   4.8754 μs |  0.2672 μs |   14.86 KB
 | JsonEverything_JsonNode  |  22.743 μs |   2.0588 μs |  0.1129 μs |   36.81 KB
 |                          |            |             |            |           
 | `$..price`
 | Hyperbee_JsonElement     |   4.433 μs |   0.7724 μs |  0.0423 μs |    4.34 KB
 | JsonCons_JsonElement     |   4.894 μs |   2.9680 μs |  0.1627 μs |    5.65 KB
 | Hyperbee_JsonNode        |   7.421 μs |   0.4506 μs |  0.0247 μs |    7.63 KB
 | Newtonsoft_JObject       |  12.818 μs |  37.7544 μs |  2.0694 μs |    14.4 KB
 | JsonEverything_JsonNode  |  16.584 μs |  16.7456 μs |  0.9179 μs |   27.63 KB
 |                          |            |             |            |           
 | `$.store.book[?(@.price == 8.99)]`
 | Hyperbee_JsonElement     |   4.164 μs |   3.7708 μs |  0.2067 μs |     5.4 KB
 | JsonCons_JsonElement     |   4.910 μs |   2.4579 μs |  0.1347 μs |    5.05 KB
 | Hyperbee_JsonNode        |   7.098 μs |   0.4756 μs |  0.0261 μs |    8.24 KB
 | Newtonsoft_JObject       |  10.036 μs |  11.8552 μs |  0.6498 μs |   15.84 KB
 | JsonEverything_JsonNode  |  11.373 μs |   3.3498 μs |  0.1836 μs |   15.85 KB
 |                          |            |             |            |           
 | `$.store.book[0]`
 | Hyperbee_JsonElement     |   2.682 μs |   1.8565 μs |  0.1018 μs |    2.27 KB
 | JsonCons_JsonElement     |   3.043 μs |   2.6136 μs |  0.1433 μs |    3.21 KB
 | Hyperbee_JsonNode        |   3.229 μs |   1.4402 μs |  0.0789 μs |    2.79 KB
 | JsonEverything_JsonNode  |   4.894 μs |   3.0709 μs |  0.1683 μs |    5.96 KB
 | Newtonsoft_JObject       |   9.111 μs |   8.1704 μs |  0.4478 μs |   14.56 KB
```
