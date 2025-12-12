```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i9-12900HK 2.50GHz, 1 CPU, 20 logical and 14 physical cores
.NET SDK 10.0.101
  [Host]   : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3 DEBUG
  ShortRun : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3


 | Method                   |       Mean |      Error |     StdDev |  Allocated
 | :----------------------- | ---------: | ---------: | ---------: | ---------:
 | `$..[?(@.price < 10)]`
 | JsonCraft.JsonElement    |   3.049 μs |  0.7626 μs |  0.0418 μs |    3.59 KB
 | Hyperbee.JsonElement     |   5.400 μs |  0.5197 μs |  0.0285 μs |   15.84 KB
 | Hyperbee.JsonNode        |   5.824 μs |  1.8214 μs |  0.0998 μs |   18.38 KB
 | JsonCons.JsonElement     |   5.955 μs |  2.4616 μs |  0.1349 μs |   12.73 KB
 | Newtonsoft.JObject       |   8.838 μs |  1.3040 μs |  0.0715 μs |   25.86 KB
 | JsonEverything.JsonNode  |  19.543 μs |  9.1216 μs |  0.5000 μs |   48.15 KB
 |                          |            |            |            |           
 | `$..['bicycle','price']`
 | JsonCraft.JsonElement    |   2.451 μs |  0.3691 μs |  0.0202 μs |    4.01 KB
 | Hyperbee.JsonElement     |   2.689 μs |  0.6920 μs |  0.0379 μs |    5.12 KB
 | JsonCons.JsonElement     |   3.217 μs |  0.4797 μs |  0.0263 μs |    7.13 KB
 | Hyperbee.JsonNode        |   3.435 μs |  0.8745 μs |  0.0479 μs |    8.84 KB
 | Newtonsoft.JObject       |   5.548 μs |  1.7475 μs |  0.0958 μs |   14.55 KB
 | JsonEverything.JsonNode  |  12.999 μs |  0.5857 μs |  0.0321 μs |    28.5 KB
 |                          |            |            |            |           
 | `$..*`
 | JsonCraft.JsonElement    |   2.110 μs |  0.2534 μs |  0.0139 μs |    2.88 KB
 | Hyperbee.JsonElement     |   2.478 μs |  0.3201 μs |  0.0175 μs |    6.45 KB
 | Hyperbee.JsonNode        |   3.434 μs |  0.6438 μs |  0.0353 μs |    9.54 KB
 | JsonCons.JsonElement     |   3.450 μs |  0.5528 μs |  0.0303 μs |    8.45 KB
 | Newtonsoft.JObject       |   5.217 μs |  0.4099 μs |  0.0225 μs |   14.19 KB
 | JsonEverything.JsonNode  |  17.330 μs |  1.5400 μs |  0.0844 μs |   33.97 KB
 |                          |            |            |            |           
 | `$..author`
 | JsonCraft.JsonElement    |   2.352 μs |  0.4147 μs |  0.0227 μs |    2.88 KB
 | Hyperbee.JsonElement     |   2.359 μs |  0.2998 μs |  0.0164 μs |     5.1 KB
 | JsonCons.JsonElement     |   2.826 μs |  1.7249 μs |  0.0945 μs |    5.47 KB
 | Hyperbee.JsonNode        |   3.290 μs |  2.1900 μs |  0.1200 μs |    8.64 KB
 | Newtonsoft.JObject       |   5.246 μs |  0.8047 μs |  0.0441 μs |    14.2 KB
 | JsonEverything.JsonNode  |  12.429 μs |  6.1835 μs |  0.3389 μs |    26.1 KB
 |                          |            |            |            |           
 | `$..book[?@.isbn]`
 | Hyperbee.JsonElement     |   2.883 μs |  1.0678 μs |  0.0585 μs |    6.14 KB
 | JsonCons.JsonElement     |   3.362 μs |  0.8878 μs |  0.0487 μs |    7.16 KB
 | Hyperbee.JsonNode        |   3.965 μs |  3.7942 μs |  0.2080 μs |    9.64 KB
 | JsonEverything.JsonNode  |  13.766 μs |  1.2953 μs |  0.0710 μs |   29.98 KB
 | JsonCraft.JsonElement    |         NA |         NA |         NA |         NA
 | Newtonsoft.JObject       |         NA |         NA |         NA |         NA
 |                          |            |            |            |           
 | `$..book[?@.price == 8.99 && @.category == 'fiction']`
 | Hyperbee.JsonElement     |   3.708 μs |  1.8637 μs |  0.1022 μs |    8.28 KB
 | Hyperbee.JsonNode        |   4.746 μs |  0.9875 μs |  0.0541 μs |   11.91 KB
 | JsonCons.JsonElement     |   4.845 μs |  1.3375 μs |  0.0733 μs |    8.48 KB
 | JsonEverything.JsonNode  |  17.256 μs |  1.0914 μs |  0.0598 μs |   39.27 KB
 | JsonCraft.JsonElement    |         NA |         NA |         NA |         NA
 | Newtonsoft.JObject       |         NA |         NA |         NA |         NA
 |                          |            |            |            |           
 | `$..book[0,1]`
 | JsonCraft.JsonElement    |   2.345 μs |  0.1935 μs |  0.0106 μs |    3.09 KB
 | Hyperbee.JsonElement     |   2.417 μs |  0.0175 μs |  0.0010 μs |     5.1 KB
 | JsonCons.JsonElement     |   3.005 μs |  0.9922 μs |  0.0544 μs |     6.1 KB
 | Hyperbee.JsonNode        |   3.163 μs |  1.0620 μs |  0.0582 μs |    8.64 KB
 | Newtonsoft.JObject       |   5.089 μs |  0.8914 μs |  0.0489 μs |   14.45 KB
 | JsonEverything.JsonNode  |  12.339 μs |  0.9570 μs |  0.0525 μs |   26.41 KB
 |                          |            |            |            |           
 | `$..book[0]`
 | JsonCraft.JsonElement    |   2.241 μs |  0.1554 μs |  0.0085 μs |       3 KB
 | Hyperbee.JsonElement     |   2.271 μs |  1.0724 μs |  0.0588 μs |     5.1 KB
 | JsonCons.JsonElement     |   2.708 μs |  0.8340 μs |  0.0457 μs |    5.55 KB
 | Hyperbee.JsonNode        |   3.528 μs |  0.2841 μs |  0.0156 μs |    8.64 KB
 | Newtonsoft.JObject       |   5.005 μs |  0.3486 μs |  0.0191 μs |   14.33 KB
 | JsonEverything.JsonNode  |  11.781 μs |  1.7173 μs |  0.0941 μs |   26.02 KB
 |                          |            |            |            |           
 | `$.store..price`
 | Hyperbee.JsonElement     |   2.293 μs |  0.4706 μs |  0.0258 μs |    4.73 KB
 | JsonCraft.JsonElement    |   2.363 μs |  1.0577 μs |  0.0580 μs |    3.13 KB
 | JsonCons.JsonElement     |   2.720 μs |  2.0514 μs |  0.1124 μs |    5.57 KB
 | Hyperbee.JsonNode        |   3.181 μs |  0.5370 μs |  0.0294 μs |    8.38 KB
 | Newtonsoft.JObject       |   5.083 μs |  0.4737 μs |  0.0260 μs |   14.34 KB
 | JsonEverything.JsonNode  |  13.102 μs |  2.1212 μs |  0.1163 μs |   26.63 KB
 |                          |            |            |            |           
 | `$.store.*`
 | JsonCraft.JsonElement    |   1.561 μs |  0.1662 μs |  0.0091 μs |    2.49 KB
 | Hyperbee.JsonElement     |   1.602 μs |  0.6889 μs |  0.0378 μs |    2.81 KB
 | JsonCons.JsonElement     |   1.693 μs |  0.7460 μs |  0.0409 μs |    3.27 KB
 | Hyperbee.JsonNode        |   1.784 μs |  0.6002 μs |  0.0329 μs |     2.9 KB
 | JsonEverything.JsonNode  |   2.395 μs |  0.3007 μs |  0.0165 μs |     4.8 KB
 | Newtonsoft.JObject       |   4.664 μs |  0.6593 μs |  0.0361 μs |   14.43 KB
 |                          |            |            |            |           
 | `$.store.bicycle.color`
 | Hyperbee.JsonElement     |   1.454 μs |  0.6877 μs |  0.0377 μs |    2.17 KB
 | JsonCraft.JsonElement    |   1.585 μs |  0.7476 μs |  0.0410 μs |    2.45 KB
 | Hyperbee.JsonNode        |   1.695 μs |  0.7429 μs |  0.0407 μs |    2.88 KB
 | JsonCons.JsonElement     |   1.816 μs |  1.7814 μs |  0.0976 μs |    3.23 KB
 | JsonEverything.JsonNode  |   2.892 μs |  0.3155 μs |  0.0173 μs |    5.74 KB
 | Newtonsoft.JObject       |   4.582 μs |  0.7337 μs |  0.0402 μs |   14.49 KB
 |                          |            |            |            |           
 | `$.store.book[-1:]`
 | Hyperbee.JsonElement     |   1.579 μs |  0.5378 μs |  0.0295 μs |    2.41 KB
 | JsonCraft.JsonElement    |   1.622 μs |  0.1762 μs |  0.0097 μs |    2.58 KB
 | Hyperbee.JsonNode        |   1.809 μs |  0.3378 μs |  0.0185 μs |    2.97 KB
 | JsonCons.JsonElement     |   1.882 μs |  0.5619 μs |  0.0308 μs |    3.52 KB
 | JsonEverything.JsonNode  |   2.806 μs |  0.3959 μs |  0.0217 μs |    5.72 KB
 | Newtonsoft.JObject       |   4.663 μs |  0.5645 μs |  0.0309 μs |   14.52 KB
 |                          |            |            |            |           
 | `$.store.book[:2]`
 | Hyperbee.JsonElement     |   1.604 μs |  0.5879 μs |  0.0322 μs |    2.41 KB
 | JsonCraft.JsonElement    |   1.637 μs |  0.0647 μs |  0.0035 μs |    2.58 KB
 | Hyperbee.JsonNode        |   1.826 μs |  0.1527 μs |  0.0084 μs |    2.97 KB
 | JsonCons.JsonElement     |   1.871 μs |  0.6744 μs |  0.0370 μs |    3.54 KB
 | JsonEverything.JsonNode  |   3.068 μs |  0.0441 μs |  0.0024 μs |    6.02 KB
 | Newtonsoft.JObject       |   5.069 μs |  5.6079 μs |  0.3074 μs |   14.51 KB
 |                          |            |            |            |           
 | `$.store.book[?(@.author && @.title)]`
 | JsonCraft.JsonElement    |   2.007 μs |  0.5824 μs |  0.0319 μs |     3.3 KB
 | Hyperbee.JsonElement     |   2.387 μs |  0.5434 μs |  0.0298 μs |    4.18 KB
 | JsonCons.JsonElement     |   2.970 μs |  0.5634 μs |  0.0309 μs |    5.58 KB
 | Hyperbee.JsonNode        |   3.636 μs |  1.5759 μs |  0.0864 μs |    8.08 KB
 | Newtonsoft.JObject       |   5.449 μs |  1.4272 μs |  0.0782 μs |   16.18 KB
 | JsonEverything.JsonNode  |   7.147 μs |  3.7089 μs |  0.2033 μs |   18.32 KB
 |                          |            |            |            |           
 | `$.store.book[?(@.category == 'fiction')]`
 | JsonCraft.JsonElement    |   2.243 μs |  1.6372 μs |  0.0897 μs |    3.38 KB
 | JsonCons.JsonElement     |   2.800 μs |  0.8079 μs |  0.0443 μs |    5.01 KB
 | Hyperbee.JsonElement     |   2.825 μs |  2.4544 μs |  0.1345 μs |    4.34 KB
 | Hyperbee.JsonNode        |   3.391 μs |  3.0949 μs |  0.1696 μs |     8.2 KB
 | Newtonsoft.JObject       |   5.009 μs |  0.9350 μs |  0.0513 μs |   15.74 KB
 | JsonEverything.JsonNode  |   7.002 μs |  0.9434 μs |  0.0517 μs |   16.49 KB
 |                          |            |            |            |           
 | `$.store.book[?(@.price < 10)].title`
 | JsonCraft.JsonElement    |   2.357 μs |  1.1758 μs |  0.0644 μs |    3.37 KB
 | Hyperbee.JsonElement     |   2.496 μs |  1.2422 μs |  0.0681 μs |    4.35 KB
 | JsonCons.JsonElement     |   2.954 μs |  1.3447 μs |  0.0737 μs |    5.18 KB
 | Hyperbee.JsonNode        |   3.466 μs |  0.7381 μs |  0.0405 μs |    8.09 KB
 | Newtonsoft.JObject       |   5.209 μs |  1.2339 μs |  0.0676 μs |   15.89 KB
 | JsonEverything.JsonNode  |   7.547 μs |  1.3411 μs |  0.0735 μs |   17.38 KB
 |                          |            |            |            |           
 | `$.store.book[?(@.price > 10 && @.price < 20)]`
 | JsonCraft.JsonElement    |   2.798 μs |  1.2832 μs |  0.0703 μs |    3.82 KB
 | Hyperbee.JsonElement     |   3.067 μs |  1.9850 μs |  0.1088 μs |    5.37 KB
 | Hyperbee.JsonNode        |   3.827 μs |  2.2293 μs |  0.1222 μs |    9.14 KB
 | JsonCons.JsonElement     |   4.398 μs |  3.5937 μs |  0.1970 μs |    6.23 KB
 | Newtonsoft.JObject       |   5.358 μs |  2.0125 μs |  0.1103 μs |   16.69 KB
 | JsonEverything.JsonNode  |   9.003 μs |  2.3392 μs |  0.1282 μs |   22.27 KB
 |                          |            |            |            |           
 | `$.store.book[?@.price == 8.99]`
 | Hyperbee.JsonElement     |   2.291 μs |  1.2891 μs |  0.0707 μs |    4.15 KB
 | JsonCons.JsonElement     |   2.826 μs |  1.0191 μs |  0.0559 μs |    4.97 KB
 | Hyperbee.JsonNode        |   3.630 μs |  3.3549 μs |  0.1839 μs |    7.89 KB
 | JsonEverything.JsonNode  |   6.450 μs |  1.0062 μs |  0.0552 μs |   15.47 KB
 | JsonCraft.JsonElement    |         NA |         NA |         NA |         NA
 | Newtonsoft.JObject       |         NA |         NA |         NA |         NA
 |                          |            |            |            |           
 | `$.store.book['category','author']`
 | JsonCraft.JsonElement    |   1.650 μs |  0.2189 μs |  0.0120 μs |    2.95 KB
 | JsonCons.JsonElement     |   1.862 μs |  0.1098 μs |  0.0060 μs |    3.61 KB
 | Hyperbee.JsonElement     |   1.988 μs |  0.8489 μs |  0.0465 μs |    2.61 KB
 | JsonEverything.JsonNode  |   2.622 μs |  0.8004 μs |  0.0439 μs |    5.41 KB
 | Hyperbee.JsonNode        |   3.215 μs |  1.1826 μs |  0.0648 μs |    6.42 KB
 | Newtonsoft.JObject       |   4.681 μs |  0.5122 μs |  0.0281 μs |   14.85 KB
 |                          |            |            |            |           
 | `$.store.book[*].author`
 | JsonCraft.JsonElement    |   1.729 μs |  0.7445 μs |  0.0408 μs |    2.63 KB
 | JsonCons.JsonElement     |   1.909 μs |  0.4640 μs |  0.0254 μs |    3.55 KB
 | Hyperbee.JsonElement     |   1.993 μs |  0.3660 μs |  0.0201 μs |    3.05 KB
 | Hyperbee.JsonNode        |   2.885 μs |  2.4990 μs |  0.1370 μs |    6.83 KB
 | Newtonsoft.JObject       |   4.894 μs |  6.5976 μs |  0.3616 μs |   14.64 KB
 | JsonEverything.JsonNode  |   5.893 μs |  1.3873 μs |  0.0760 μs |   12.45 KB
 |                          |            |            |            |           
 | `$.store.book[*]`
 | JsonCraft.JsonElement    |   1.578 μs |  0.8516 μs |  0.0467 μs |    2.45 KB
 | JsonCons.JsonElement     |   1.692 μs |  0.8482 μs |  0.0465 μs |    3.35 KB
 | Hyperbee.JsonElement     |   1.738 μs |  0.3719 μs |  0.0204 μs |    2.65 KB
 | Hyperbee.JsonNode        |   1.923 μs |  0.7320 μs |  0.0401 μs |    3.17 KB
 | JsonEverything.JsonNode  |   3.526 μs |  0.0963 μs |  0.0053 μs |    6.61 KB
 | Newtonsoft.JObject       |   4.752 μs |  0.2905 μs |  0.0159 μs |   14.49 KB
 |                          |            |            |            |           
 | `$.store.book[0,1]`
 | Hyperbee.JsonElement     |   1.560 μs |  0.6522 μs |  0.0358 μs |    2.41 KB
 | JsonCraft.JsonElement    |   1.675 μs |  0.3241 μs |  0.0178 μs |    2.64 KB
 | JsonCons.JsonElement     |   1.910 μs |  0.3014 μs |  0.0165 μs |    3.73 KB
 | Hyperbee.JsonNode        |   1.976 μs |  1.1111 μs |  0.0609 μs |    2.97 KB
 | JsonEverything.JsonNode  |   3.071 μs |  0.1558 μs |  0.0085 μs |    6.07 KB
 | Newtonsoft.JObject       |   4.870 μs |  0.8053 μs |  0.0441 μs |   14.59 KB
 |                          |            |            |            |           
 | `$.store.book[0].title`
 | Hyperbee.JsonElement     |   1.515 μs |  0.1457 μs |  0.0080 μs |    2.17 KB
 | JsonCraft.JsonElement    |   1.713 μs |  1.1282 μs |  0.0618 μs |    2.51 KB
 | JsonCons.JsonElement     |   1.869 μs |  0.0938 μs |  0.0051 μs |     3.3 KB
 | Hyperbee.JsonNode        |   1.945 μs |  0.2311 μs |  0.0127 μs |     3.6 KB
 | JsonEverything.JsonNode  |   3.451 μs |  0.4020 μs |  0.0220 μs |    7.38 KB
 | Newtonsoft.JObject       |   4.706 μs |  1.4286 μs |  0.0783 μs |   14.62 KB
 |                          |            |            |            |           
 | `$.store.book[0]`
 | Hyperbee.JsonElement     |   1.392 μs |  0.6219 μs |  0.0341 μs |    2.17 KB
 | JsonCraft.JsonElement    |   1.570 μs |  0.2170 μs |  0.0119 μs |    2.44 KB
 | JsonCons.JsonElement     |   1.783 μs |  0.9320 μs |  0.0511 μs |    3.21 KB
 | Hyperbee.JsonNode        |   1.870 μs |  0.6578 μs |  0.0361 μs |    2.83 KB
 | JsonEverything.JsonNode  |   2.832 μs |  0.6614 μs |  0.0363 μs |    5.68 KB
 | Newtonsoft.JObject       |   4.697 μs |  0.9290 μs |  0.0509 μs |   14.48 KB
 |                          |            |            |            |           
 | `$`
 | Hyperbee.JsonElement     |   1.316 μs |  0.7199 μs |  0.0395 μs |    2.17 KB
 | JsonCraft.JsonElement    |   1.368 μs |  0.0907 μs |  0.0050 μs |    2.22 KB
 | JsonEverything.JsonNode  |   1.438 μs |  0.1670 μs |  0.0092 μs |    1.88 KB
 | Hyperbee.JsonNode        |   1.440 μs |  0.2725 μs |  0.0149 μs |    1.75 KB
 | JsonCons.JsonElement     |   1.456 μs |  0.1789 μs |  0.0098 μs |    2.94 KB
 | Newtonsoft.JObject       |   4.331 μs |  1.0036 μs |  0.0550 μs |   13.98 KB

Benchmarks with issues:
  JsonPathParseAndSelectEvaluator.JsonCraft.JsonElement: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.isbn]]
  JsonPathParseAndSelectEvaluator.Newtonsoft.JObject: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.isbn]]
  JsonPathParseAndSelectEvaluator.JsonCraft.JsonElement: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.price == 8.99 && @.category == 'fiction']]
  JsonPathParseAndSelectEvaluator.Newtonsoft.JObject: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.price == 8.99 && @.category == 'fiction']]
  JsonPathParseAndSelectEvaluator.JsonCraft.JsonElement: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$.store.book[?@.price == 8.99]]
  JsonPathParseAndSelectEvaluator.Newtonsoft.JObject: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$.store.book[?@.price == 8.99]]
```
