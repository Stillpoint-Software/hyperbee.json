```

BenchmarkDotNet v0.15.2, Windows 11 (10.0.26100.4484/24H2/2024Update/HudsonValley)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.203
  [Host]   : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2
  ShortRun : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2


 | Method                |       Mean |      Error |     StdDev |  Allocated
 | :-------------------- | ---------: | ---------: | ---------: | ---------:
 | `$..[?(@.price < 10)]`
 | Hyperbee.JsonElement  |  10.163 μs |  1.3183 μs |  0.0723 μs |   20.73 KB
 |                       |            |            |            |           
 | `$..['bicycle','price']`
 | Hyperbee.JsonElement  |   4.531 μs |  1.0563 μs |  0.0579 μs |    5.37 KB
 |                       |            |            |            |           
 | `$..*`
 | Hyperbee.JsonElement  |   4.016 μs |  0.6737 μs |  0.0369 μs |    6.51 KB
 |                       |            |            |            |           
 | `$..author`
 | Hyperbee.JsonElement  |   4.023 μs |  1.0348 μs |  0.0567 μs |    5.16 KB
 |                       |            |            |            |           
 | `$..book[?@.isbn]`
 | Hyperbee.JsonElement  |   4.965 μs |  1.2376 μs |  0.0678 μs |     6.8 KB
 |                       |            |            |            |           
 | `$..book[?@.price == (...)tegory == 'fiction'] [52]`
 | Hyperbee.JsonElement  |   6.274 μs |  1.0350 μs |  0.0567 μs |    9.47 KB
 |                       |            |            |            |           
 | `$..book[0,1]`
 | Hyperbee.JsonElement  |   4.062 μs |  0.8640 μs |  0.0474 μs |    5.16 KB
 |                       |            |            |            |           
 | `$.store..price`
 | Hyperbee.JsonElement  |   3.861 μs |  1.0337 μs |  0.0567 μs |     4.8 KB
 |                       |            |            |            |           
 | `$.store.* #First()`
 | Hyperbee.JsonElement  |   2.936 μs |  0.1081 μs |  0.0059 μs |    2.91 KB
 |                       |            |            |            |           
 | `$.store.*`
 | Hyperbee.JsonElement  |   3.065 μs |  3.7327 μs |  0.2046 μs |    2.88 KB
 |                       |            |            |            |           
 | `$.store.bicycle.color`
 | Hyperbee.JsonElement  |   2.579 μs |  1.0927 μs |  0.0599 μs |     2.3 KB
 |                       |            |            |            |           
 | `$.store.book[-1:]`
 | Hyperbee.JsonElement  |   2.803 μs |  0.3956 μs |  0.0217 μs |    2.47 KB
 |                       |            |            |            |           
 | `$.store.book[:2]`
 | Hyperbee.JsonElement  |   3.027 μs |  1.6825 μs |  0.0922 μs |    2.47 KB
 |                       |            |            |            |           
 | `$.store.book[?(!@.isbn)]`
 | Hyperbee.JsonElement  |   3.679 μs |  0.3157 μs |  0.0173 μs |    4.32 KB
 |                       |            |            |            |           
 | `$.store.book[?(@.author && @.title)]`
 | Hyperbee.JsonElement  |   4.155 μs |  2.5445 μs |  0.1395 μs |    5.52 KB
 |                       |            |            |            |           
 | `$.store.book[?(@.category == 'fiction')]`
 | Hyperbee.JsonElement  |   4.072 μs |  1.6135 μs |  0.0884 μs |    5.09 KB
 |                       |            |            |            |           
 | `$.store.book[?(@.pri(...)egory == 'fiction')] [56]`
 | Hyperbee.JsonElement  |   5.138 μs |  0.9838 μs |  0.0539 μs |    6.74 KB
 |                       |            |            |            |           
 | `$.store.book[?(@.price < 10)].title`
 | Hyperbee.JsonElement  |   4.115 μs |  1.1332 μs |  0.0621 μs |     5.1 KB
 |                       |            |            |            |           
 | `$.store.book[?(@.price == 8.99)]`
 | Hyperbee.JsonElement  |   4.055 μs |  2.6745 μs |  0.1466 μs |     4.9 KB
 |                       |            |            |            |           
 | `$.store.book[?(@.price > 10 && @.price < 20)]`
 | Hyperbee.JsonElement  |   4.832 μs |  0.5305 μs |  0.0291 μs |    6.55 KB
 |                       |            |            |            |           
 | `$.store.book[?(@.title =~ /Sword/)]`
 | Hyperbee.JsonElement  |         NA |         NA |         NA |         NA
 |                       |            |            |            |           
 | `$.store.book[?(length(@.title) > 10)]`
 | Hyperbee.JsonElement  |         NA |         NA |         NA |         NA
 |                       |            |            |            |           
 | `$.store.book['category','author']`
 | Hyperbee.JsonElement  |   3.480 μs |  0.2227 μs |  0.0122 μs |    2.67 KB
 |                       |            |            |            |           
 | `$.store.book[*].author`
 | Hyperbee.JsonElement  |   3.362 μs |  1.4354 μs |  0.0787 μs |    3.12 KB
 |                       |            |            |            |           
 | `$.store.book[*]`
 | Hyperbee.JsonElement  |   2.923 μs |  0.4430 μs |  0.0243 μs |    2.71 KB
 |                       |            |            |            |           
 | `$.store.book[0,1]`
 | Hyperbee.JsonElement  |   2.794 μs |  0.0686 μs |  0.0038 μs |    2.47 KB
 |                       |            |            |            |           
 | `$.store.book[0:3:2]`
 | Hyperbee.JsonElement  |   2.805 μs |  1.6556 μs |  0.0907 μs |    2.47 KB
 |                       |            |            |            |           
 | `$.store.book[0].title`
 | Hyperbee.JsonElement  |   2.604 μs |  0.4241 μs |  0.0232 μs |    2.27 KB
 |                       |            |            |            |           
 | `$.store.book[0]`
 | Hyperbee.JsonElement  |   2.569 μs |  1.0771 μs |  0.0590 μs |    2.27 KB
 |                       |            |            |            |           
 | `$`
 | Hyperbee.JsonElement  |   2.554 μs |  0.6726 μs |  0.0369 μs |    2.27 KB

Benchmarks with issues:
  JsonPathBenchmark.Hyperbee.JsonElement: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$.store.book[?(@.title =~ /Sword/)]]
  JsonPathBenchmark.Hyperbee.JsonElement: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$.store.book[?(length(@.title) > 10)]]
```
