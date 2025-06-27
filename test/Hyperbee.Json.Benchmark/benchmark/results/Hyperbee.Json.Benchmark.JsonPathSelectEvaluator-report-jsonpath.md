```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4351)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 9.0.301
  [Host]   : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX2 [AttachedDebugger]
  ShortRun : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX2


 | Method                |         Mean |         Error |     StdDev |  Allocated
 | :-------------------- | -----------: | ------------: | ---------: | ---------:
 | `$..*`
 | Newtonsoft.JObject    |  1,129.64 ns |    200.111 ns |  10.969 ns |      320 B
 | Hyperbee.JsonElement  |  1,435.16 ns |    270.030 ns |  14.801 ns |     4496 B
 | Hyperbee.JsonNode     |  1,635.74 ns |    212.133 ns |  11.628 ns |     4120 B
 |                       |              |               |            |           
 | `$..book[?@.isbn]`
 | Hyperbee.JsonNode     |  1,751.53 ns |     67.009 ns |   3.673 ns |     4440 B
 | Hyperbee.JsonElement  |  1,939.70 ns |  1,281.085 ns |  70.221 ns |     4792 B
 | Newtonsoft.JObject    |           NA |            NA |         NA |         NA
 |                       |              |               |            |           
 | `$..book[?@.price == (...)tegory == 'fiction'] [52]`
 | Hyperbee.JsonElement  |  3,053.63 ns |    138.091 ns |   7.569 ns |     7528 B
 | Hyperbee.JsonNode     |  3,202.07 ns |    626.290 ns |  34.329 ns |     7304 B
 | Newtonsoft.JObject    |           NA |            NA |         NA |         NA
 |                       |              |               |            |           
 | `$..book[0]`
 | Newtonsoft.JObject    |    952.67 ns |     80.154 ns |   4.394 ns |      464 B
 | Hyperbee.JsonNode     |  1,066.23 ns |    199.175 ns |  10.917 ns |     2808 B
 | Hyperbee.JsonElement  |  1,126.19 ns |     60.415 ns |   3.312 ns |     3120 B
 |                       |              |               |            |           
 | `$.store..price`
 | Newtonsoft.JObject    |  1,002.20 ns |     80.903 ns |   4.435 ns |      472 B
 | Hyperbee.JsonNode     |  1,016.07 ns |     17.480 ns |   0.958 ns |     2480 B
 | Hyperbee.JsonElement  |  1,019.12 ns |    269.498 ns |  14.772 ns |     2744 B
 |                       |              |               |            |           
 | `$.store.*`
 | Newtonsoft.JObject    |    197.98 ns |     45.002 ns |   2.467 ns |      568 B
 | Hyperbee.JsonNode     |    284.19 ns |     87.546 ns |   4.799 ns |      632 B
 | Hyperbee.JsonElement  |    320.20 ns |     82.718 ns |   4.534 ns |      776 B
 |                       |              |               |            |           
 | `$.store.book[-1:]`
 | Hyperbee.JsonNode     |    206.03 ns |     60.986 ns |   3.343 ns |      304 B
 | Hyperbee.JsonElement  |    229.31 ns |     20.912 ns |   1.146 ns |      360 B
 | Newtonsoft.JObject    |    237.86 ns |     48.031 ns |   2.633 ns |      656 B
 |                       |              |               |            |           
 | `$.store.book[?@.price == 8.99]`
 | Hyperbee.JsonElement  |  1,238.46 ns |    117.376 ns |   6.434 ns |     2848 B
 | Hyperbee.JsonNode     |  1,321.87 ns |    540.536 ns |  29.629 ns |     2720 B
 | Newtonsoft.JObject    |           NA |            NA |         NA |         NA
 |                       |              |               |            |           
 | `$.store.book['category','author']`
 | Newtonsoft.JObject    |    293.04 ns |    156.371 ns |   8.571 ns |     1000 B
 | Hyperbee.JsonNode     |    553.55 ns |     28.365 ns |   1.555 ns |      512 B
 | Hyperbee.JsonElement  |    680.40 ns |     61.553 ns |   3.374 ns |      568 B
 |                       |              |               |            |           
 | `$.store.book[*].author`
 | Newtonsoft.JObject    |    379.88 ns |     30.004 ns |   1.645 ns |      784 B
 | Hyperbee.JsonNode     |    541.65 ns |     59.901 ns |   3.283 ns |      928 B
 | Hyperbee.JsonElement  |    652.23 ns |    124.684 ns |   6.834 ns |     1024 B
 |                       |              |               |            |           
 | `$.store.book[0,1]`
 | Hyperbee.JsonNode     |    215.48 ns |     13.220 ns |   0.725 ns |      304 B
 | Hyperbee.JsonElement  |    245.08 ns |     17.875 ns |   0.980 ns |      360 B
 | Newtonsoft.JObject    |    279.02 ns |    168.868 ns |   9.256 ns |      736 B
 |                       |              |               |            |           
 | `$.store.book[0]`
 | Hyperbee.JsonElement  |    101.05 ns |      4.460 ns |   0.244 ns |      160 B
 | Hyperbee.JsonNode     |    101.29 ns |     11.959 ns |   0.655 ns |      192 B
 | Newtonsoft.JObject    |    236.41 ns |     17.471 ns |   0.958 ns |      616 B
 |                       |              |               |            |           
 | `$`
 | Newtonsoft.JObject    |     31.69 ns |      9.529 ns |   0.522 ns |      136 B
 | Hyperbee.JsonNode     |     36.08 ns |      9.450 ns |   0.518 ns |      144 B
 | Hyperbee.JsonElement  |     39.98 ns |     11.650 ns |   0.639 ns |      160 B

Benchmarks with issues:
  JsonPathSelectEvaluator.Newtonsoft.JObject: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.isbn]]
  JsonPathSelectEvaluator.Newtonsoft.JObject: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.price == (...)tegory == 'fiction'] [52]]
  JsonPathSelectEvaluator.Newtonsoft.JObject: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$.store.book[?@.price == 8.99]]
```
