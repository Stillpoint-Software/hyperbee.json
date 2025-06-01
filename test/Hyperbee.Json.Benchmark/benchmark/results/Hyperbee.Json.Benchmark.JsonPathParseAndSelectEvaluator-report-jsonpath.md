```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4202)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.203
  [Host]   : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2
  ShortRun : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2


 | Method                 |       Mean |      Error |     StdDev |  Allocated
 | :--------------------- | ---------: | ---------: | ---------: | ---------:
 | `$..[?(@.price < 10)]`
 | JsonCraft.JsonElement  |   5.309 μs |  1.6696 μs |  0.0915 μs |    3.59 KB
 | Hyperbee.JsonElement   |  10.112 μs |  2.1872 μs |  0.1199 μs |   20.73 KB
 |                        |            |            |            |           
 | `$..['bicycle','price']`
 | JsonCraft.JsonElement  |   4.551 μs |  2.5508 μs |  0.1398 μs |    4.01 KB
 | Hyperbee.JsonElement   |   4.581 μs |  2.8094 μs |  0.1540 μs |    5.37 KB
 |                        |            |            |            |           
 | `$..*`
 | JsonCraft.JsonElement  |   3.541 μs |  1.5742 μs |  0.0863 μs |    2.88 KB
 | Hyperbee.JsonElement   |   4.132 μs |  0.8317 μs |  0.0456 μs |     5.5 KB
 |                        |            |            |            |           
 | `$..author`
 | Hyperbee.JsonElement   |   3.958 μs |  2.4245 μs |  0.1329 μs |    5.16 KB
 | JsonCraft.JsonElement  |   4.014 μs |  2.7295 μs |  0.1496 μs |    2.88 KB
 |                        |            |            |            |           
 | `$..book[0,1]`
 | Hyperbee.JsonElement   |   3.964 μs |  1.3351 μs |  0.0732 μs |    5.16 KB
 | JsonCraft.JsonElement  |   4.027 μs |  1.6110 μs |  0.0883 μs |    3.09 KB
 |                        |            |            |            |           
 | `$.store..price`
 | Hyperbee.JsonElement   |   3.846 μs |  0.8747 μs |  0.0479 μs |     4.8 KB
 | JsonCraft.JsonElement  |   4.126 μs |  0.6668 μs |  0.0365 μs |    3.13 KB
 |                        |            |            |            |           
 | `$.store.*`
 | JsonCraft.JsonElement  |   2.610 μs |  0.7912 μs |  0.0434 μs |    2.49 KB
 | Hyperbee.JsonElement   |   2.983 μs |  2.4118 μs |  0.1322 μs |    2.88 KB
 |                        |            |            |            |           
 | `$.store.bicycle.color`
 | Hyperbee.JsonElement   |   2.568 μs |  1.8970 μs |  0.1040 μs |     2.3 KB
 | JsonCraft.JsonElement  |   2.635 μs |  0.5189 μs |  0.0284 μs |    2.49 KB
 |                        |            |            |            |           
 | `$.store.book[-1:]`
 | JsonCraft.JsonElement  |   2.675 μs |  0.8429 μs |  0.0462 μs |    2.58 KB
 | Hyperbee.JsonElement   |   2.734 μs |  0.5098 μs |  0.0279 μs |    2.47 KB
 |                        |            |            |            |           
 | `$.store.book[:2]`
 | Hyperbee.JsonElement   |   2.782 μs |  1.4813 μs |  0.0812 μs |    2.47 KB
 | JsonCraft.JsonElement  |   2.794 μs |  2.5981 μs |  0.1424 μs |    2.58 KB
 |                        |            |            |            |           
 | `$.store.book[?(@.author && @.title)]`
 | JsonCraft.JsonElement  |   3.899 μs |  8.2799 μs |  0.4539 μs |     3.3 KB
 | Hyperbee.JsonElement   |   4.404 μs |  2.5578 μs |  0.1402 μs |    5.52 KB
 |                        |            |            |            |           
 | `$.store.book[?(@.category == 'fiction')]`
 | JsonCraft.JsonElement  |   3.522 μs |  2.0617 μs |  0.1130 μs |    3.38 KB
 | Hyperbee.JsonElement   |   4.316 μs |  3.5501 μs |  0.1946 μs |    5.09 KB
 |                        |            |            |            |           
 | `$.store.book[?(@.price < 10)].title`
 | Hyperbee.JsonElement   |   4.266 μs |  3.2809 μs |  0.1798 μs |     5.1 KB
 | JsonCraft.JsonElement  |   6.221 μs |  7.3531 μs |  0.4031 μs |    3.37 KB
 |                        |            |            |            |           
 | `$.store.book[?(@.price > 10 && @.price < 20)]`
 | JsonCraft.JsonElement  |   4.356 μs |  3.6377 μs |  0.1994 μs |    3.82 KB
 | Hyperbee.JsonElement   |   4.918 μs |  0.8512 μs |  0.0467 μs |    6.55 KB
 |                        |            |            |            |           
 | `$.store.book[*].author`
 | JsonCraft.JsonElement  |   2.994 μs |  1.5486 μs |  0.0849 μs |    2.63 KB
 | Hyperbee.JsonElement   |   3.377 μs |  0.6483 μs |  0.0355 μs |    3.12 KB
 |                        |            |            |            |           
 | `$.store.book[*]`
 | JsonCraft.JsonElement  |   2.674 μs |  0.6596 μs |  0.0362 μs |    2.48 KB
 | Hyperbee.JsonElement   |   2.891 μs |  1.1848 μs |  0.0649 μs |    2.71 KB
 |                        |            |            |            |           
 | `$.store.book[0].title`
 | Hyperbee.JsonElement   |   2.680 μs |  2.2509 μs |  0.1234 μs |    2.27 KB
 | JsonCraft.JsonElement  |   2.716 μs |  0.7777 μs |  0.0426 μs |    2.55 KB
```
