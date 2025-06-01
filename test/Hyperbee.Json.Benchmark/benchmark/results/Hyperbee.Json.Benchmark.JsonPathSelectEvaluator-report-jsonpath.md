```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.203
  [Host]   : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2
  ShortRun : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2


 | Method                   |         Mean |         Error |      StdDev |  Allocated
 | :----------------------- | -----------: | ------------: | ----------: | ---------:
 | `$..*`
 | 'Json.Net (Newtonsoft)'  |  1,663.17 ns |    306.230 ns |   16.785 ns |      568 B
 | Hyperbee.JsonNode        |  4,235.65 ns |  1,165.972 ns |   63.911 ns |     5056 B
 | Hyperbee.JsonElement     |  4,858.12 ns |    898.871 ns |   49.270 ns |     6408 B
 |                          |              |               |             |           
 | `$..book[?@.isbn]`
 | Hyperbee.JsonNode        |  2,539.14 ns |  1,668.271 ns |   91.444 ns |     3832 B
 | Hyperbee.JsonElement     |  2,852.45 ns |    530.695 ns |   29.089 ns |     3904 B
 | 'Json.Net (Newtonsoft)'  |           NA |            NA |          NA |         NA
 |                          |              |               |             |           
 | `$..book[?@.price == (...)tegory == 'fiction'] [52]`
 | Hyperbee.JsonNode        |  4,596.58 ns |  1,453.515 ns |   79.672 ns |     6944 B
 | Hyperbee.JsonElement     |  4,614.84 ns |  3,365.769 ns |  184.489 ns |     6880 B
 | 'Json.Net (Newtonsoft)'  |           NA |            NA |          NA |         NA
 |                          |              |               |             |           
 | `$..book[0]`
 | 'Json.Net (Newtonsoft)'  |  1,356.94 ns |    353.834 ns |   19.395 ns |      496 B
 | Hyperbee.JsonNode        |  1,514.78 ns |    562.260 ns |   30.819 ns |     1952 B
 | Hyperbee.JsonElement     |  1,763.83 ns |    606.140 ns |   33.225 ns |     1976 B
 |                          |              |               |             |           
 | `$.store..price`
 | 'Json.Net (Newtonsoft)'  |  1,386.04 ns |    116.630 ns |    6.393 ns |      536 B
 | Hyperbee.JsonNode        |  1,432.28 ns |    131.367 ns |    7.201 ns |     1736 B
 | Hyperbee.JsonElement     |  1,522.55 ns |    325.344 ns |   17.833 ns |     1776 B
 |                          |              |               |             |           
 | `$.store.*`
 | 'Json.Net (Newtonsoft)'  |    336.70 ns |     70.197 ns |    3.848 ns |      608 B
 | Hyperbee.JsonNode        |    460.77 ns |    284.563 ns |   15.598 ns |      960 B
 | Hyperbee.JsonElement     |    505.11 ns |    111.571 ns |    6.116 ns |     1216 B
 |                          |              |               |             |           
 | `$.store.book[-1:]`
 | Hyperbee.JsonNode        |    396.47 ns |    167.716 ns |    9.193 ns |      704 B
 | 'Json.Net (Newtonsoft)'  |    402.63 ns |    169.272 ns |    9.278 ns |      688 B
 | Hyperbee.JsonElement     |    411.42 ns |    132.880 ns |    7.284 ns |      896 B
 |                          |              |               |             |           
 | `$.store.book[?@.price == 8.99]`
 | Hyperbee.JsonElement     |  1,867.53 ns |    706.136 ns |   38.706 ns |     3624 B
 | Hyperbee.JsonNode        |  1,963.96 ns |    159.196 ns |    8.726 ns |     3360 B
 | 'Json.Net (Newtonsoft)'  |           NA |            NA |          NA |         NA
 |                          |              |               |             |           
 | `$.store.book['category','author']`
 | 'Json.Net (Newtonsoft)'  |    441.16 ns |     67.371 ns |    3.693 ns |     1000 B
 | Hyperbee.JsonNode        |    965.14 ns |    201.450 ns |   11.042 ns |     1048 B
 | Hyperbee.JsonElement     |  1,365.77 ns |    264.766 ns |   14.513 ns |     1296 B
 |                          |              |               |             |           
 | `$.store.book[*].author`
 | 'Json.Net (Newtonsoft)'  |    621.68 ns |     68.455 ns |    3.752 ns |      840 B
 | Hyperbee.JsonElement     |  1,169.77 ns |    514.153 ns |   28.182 ns |     1752 B
 | Hyperbee.JsonNode        |  1,283.55 ns |  2,651.830 ns |  145.356 ns |     1496 B
 |                          |              |               |             |           
 | `$.store.book[0,1]`
 | Hyperbee.JsonElement     |    434.92 ns |    196.984 ns |   10.797 ns |      912 B
 | Hyperbee.JsonNode        |    446.30 ns |    723.205 ns |   39.641 ns |      712 B
 | 'Json.Net (Newtonsoft)'  |    488.25 ns |    637.440 ns |   34.940 ns |      776 B
 |                          |              |               |             |           
 | `$.store.book[0]`
 | Hyperbee.JsonNode        |    197.91 ns |     28.470 ns |    1.561 ns |      320 B
 | Hyperbee.JsonElement     |    206.25 ns |    117.243 ns |    6.427 ns |      288 B
 | 'Json.Net (Newtonsoft)'  |    397.65 ns |     39.882 ns |    2.186 ns |      648 B
 |                          |              |               |             |           
 | `$`
 | Hyperbee.JsonElement     |     51.24 ns |      6.684 ns |    0.366 ns |      160 B
 | 'Json.Net (Newtonsoft)'  |     59.21 ns |     30.779 ns |    1.687 ns |      136 B
 | Hyperbee.JsonNode        |     66.31 ns |     27.840 ns |    1.526 ns |      144 B

Benchmarks with issues:
  JsonPathSelectEvaluator.'Json.Net (Newtonsoft)': ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.isbn]]
  JsonPathSelectEvaluator.'Json.Net (Newtonsoft)': ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.price == (...)tegory == 'fiction'] [52]]
  JsonPathSelectEvaluator.'Json.Net (Newtonsoft)': ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$.store.book[?@.price == 8.99]]
```
