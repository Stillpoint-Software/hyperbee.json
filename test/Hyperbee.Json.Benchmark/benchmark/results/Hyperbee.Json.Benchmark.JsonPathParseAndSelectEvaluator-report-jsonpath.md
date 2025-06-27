```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4351)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 9.0.301
  [Host]   : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX2 [AttachedDebugger]
  ShortRun : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX2


 | Method                   |       Mean |      Error |     StdDev |  Allocated
 | :----------------------- | ---------: | ---------: | ---------: | ---------:
 | `$..[?(@.price < 10)]`
 | JsonCraft.JsonElement    |   3.841 μs |  0.3111 μs |  0.0171 μs |    3.59 KB
 | JsonCons.JsonElement     |   7.459 μs |  0.6412 μs |  0.0351 μs |   12.77 KB
 | Hyperbee.JsonElement     |   8.284 μs |  2.5497 μs |  0.1398 μs |   20.73 KB
 | Hyperbee.JsonNode        |  11.086 μs |  8.0907 μs |  0.4435 μs |   23.79 KB
 | Newtonsoft.JObject       |  12.153 μs |  2.4881 μs |  0.1364 μs |   25.86 KB
 | JsonEverything.JsonNode  |  23.541 μs |  1.3946 μs |  0.0764 μs |   48.15 KB
 |                          |            |            |            |           
 | `$..['bicycle','price']`
 | JsonCraft.JsonElement    |   3.136 μs |  0.2760 μs |  0.0151 μs |    4.01 KB
 | Hyperbee.JsonElement     |   3.578 μs |  0.4623 μs |  0.0253 μs |    5.37 KB
 | JsonCons.JsonElement     |   3.948 μs |  0.6099 μs |  0.0334 μs |    7.18 KB
 | Hyperbee.JsonNode        |   5.181 μs |  1.8016 μs |  0.0988 μs |    9.23 KB
 | Newtonsoft.JObject       |   7.823 μs |  0.8017 μs |  0.0439 μs |   14.55 KB
 | JsonEverything.JsonNode  |  16.753 μs |  1.5507 μs |  0.0850 μs |    28.5 KB
 |                          |            |            |            |           
 | `$..*`
 | JsonCraft.JsonElement    |   2.497 μs |  0.0903 μs |  0.0049 μs |    2.88 KB
 | Hyperbee.JsonElement     |   3.299 μs |  0.8178 μs |  0.0448 μs |    6.51 KB
 | JsonCons.JsonElement     |   4.176 μs |  0.5887 μs |  0.0323 μs |    8.49 KB
 | Hyperbee.JsonNode        |   5.330 μs |  0.7684 μs |  0.0421 μs |    10.3 KB
 | Newtonsoft.JObject       |   7.784 μs |  0.5303 μs |  0.0291 μs |   14.19 KB
 | JsonEverything.JsonNode  |  21.226 μs |  1.0905 μs |  0.0598 μs |   33.97 KB
 |                          |            |            |            |           
 | `$..author`
 | JsonCraft.JsonElement    |   2.904 μs |  4.2915 μs |  0.2352 μs |    2.88 KB
 | Hyperbee.JsonElement     |   3.068 μs |  0.3672 μs |  0.0201 μs |    5.16 KB
 | JsonCons.JsonElement     |   3.381 μs |  0.6294 μs |  0.0345 μs |    5.55 KB
 | Hyperbee.JsonNode        |   5.030 μs |  0.6100 μs |  0.0334 μs |    9.02 KB
 | Newtonsoft.JObject       |   7.469 μs |  1.7563 μs |  0.0963 μs |    14.2 KB
 | JsonEverything.JsonNode  |  15.752 μs |  3.8966 μs |  0.2136 μs |    26.1 KB
 |                          |            |            |            |           
 | `$..book[?@.isbn]`
 | Hyperbee.JsonElement     |   3.913 μs |  1.5846 μs |  0.0869 μs |     6.8 KB
 | JsonCons.JsonElement     |   4.359 μs |  2.1655 μs |  0.1187 μs |    7.21 KB
 | Hyperbee.JsonNode        |   5.335 μs |  0.7057 μs |  0.0387 μs |   10.62 KB
 | JsonEverything.JsonNode  |  17.200 μs |  1.9836 μs |  0.1087 μs |   29.98 KB
 | JsonCraft.JsonElement    |         NA |         NA |         NA |         NA
 | Newtonsoft.JObject       |         NA |         NA |         NA |         NA
 |                          |            |            |            |           
 | `$..book[?@.price == 8.99 && @.category == 'fiction']`
 | Hyperbee.JsonElement     |   5.135 μs |  1.0302 μs |  0.0565 μs |    9.47 KB
 | JsonCons.JsonElement     |   6.105 μs |  0.6309 μs |  0.0346 μs |    8.52 KB
 | Hyperbee.JsonNode        |   7.002 μs |  3.0008 μs |  0.1645 μs |   13.41 KB
 | JsonEverything.JsonNode  |  21.845 μs |  3.9271 μs |  0.2153 μs |   39.52 KB
 | JsonCraft.JsonElement    |         NA |         NA |         NA |         NA
 | Newtonsoft.JObject       |         NA |         NA |         NA |         NA
 |                          |            |            |            |           
 | `$..book[0,1]`
 | JsonCraft.JsonElement    |   2.936 μs |  0.6578 μs |  0.0361 μs |    3.09 KB
 | Hyperbee.JsonElement     |   3.157 μs |  0.8302 μs |  0.0455 μs |    5.16 KB
 | JsonCons.JsonElement     |   3.691 μs |  0.1430 μs |  0.0078 μs |    6.15 KB
 | Hyperbee.JsonNode        |   4.972 μs |  0.6586 μs |  0.0361 μs |    9.02 KB
 | Newtonsoft.JObject       |   7.441 μs |  0.5961 μs |  0.0327 μs |   14.45 KB
 | JsonEverything.JsonNode  |  15.523 μs |  5.4908 μs |  0.3010 μs |   26.41 KB
 |                          |            |            |            |           
 | `$..book[0]`
 | JsonCraft.JsonElement    |   2.825 μs |  0.0934 μs |  0.0051 μs |       3 KB
 | Hyperbee.JsonElement     |   3.114 μs |  0.4106 μs |  0.0225 μs |    5.16 KB
 | JsonCons.JsonElement     |   3.451 μs |  0.2921 μs |  0.0160 μs |    5.63 KB
 | Hyperbee.JsonNode        |   4.691 μs |  1.4048 μs |  0.0770 μs |    9.02 KB
 | Newtonsoft.JObject       |   7.720 μs |  0.8748 μs |  0.0480 μs |   14.33 KB
 | JsonEverything.JsonNode  |  15.314 μs |  3.3459 μs |  0.1834 μs |   26.02 KB
 |                          |            |            |            |           
 | `$.store..price`
 | JsonCraft.JsonElement    |   2.912 μs |  1.3083 μs |  0.0717 μs |    3.13 KB
 | Hyperbee.JsonElement     |   2.993 μs |  0.4534 μs |  0.0249 μs |     4.8 KB
 | JsonCons.JsonElement     |   3.446 μs |  0.9305 μs |  0.0510 μs |    5.62 KB
 | Hyperbee.JsonNode        |   4.721 μs |  0.9516 μs |  0.0522 μs |     8.7 KB
 | Newtonsoft.JObject       |   7.723 μs |  1.1978 μs |  0.0657 μs |   14.34 KB
 | JsonEverything.JsonNode  |  15.966 μs |  1.1138 μs |  0.0610 μs |   26.63 KB
 |                          |            |            |            |           
 | `$.store.*`
 | JsonCraft.JsonElement    |   1.882 μs |  0.5586 μs |  0.0306 μs |    2.49 KB
 | JsonCons.JsonElement     |   2.151 μs |  0.1067 μs |  0.0058 μs |    3.31 KB
 | Hyperbee.JsonElement     |   2.167 μs |  0.6457 μs |  0.0354 μs |    2.88 KB
 | Hyperbee.JsonNode        |   2.435 μs |  0.9690 μs |  0.0531 μs |    2.95 KB
 | JsonEverything.JsonNode  |   3.047 μs |  0.4674 μs |  0.0256 μs |     4.8 KB
 | Newtonsoft.JObject       |   6.576 μs |  0.8290 μs |  0.0454 μs |   14.43 KB
 |                          |            |            |            |           
 | `$.store.bicycle.color`
 | Hyperbee.JsonElement     |   1.920 μs |  0.6179 μs |  0.0339 μs |     2.3 KB
 | JsonCraft.JsonElement    |   2.032 μs |  0.7770 μs |  0.0426 μs |    2.49 KB
 | JsonCons.JsonElement     |   2.216 μs |  0.1473 μs |  0.0081 μs |    3.27 KB
 | Hyperbee.JsonNode        |   2.383 μs |  1.2149 μs |  0.0666 μs |    2.88 KB
 | JsonEverything.JsonNode  |   3.556 μs |  0.5152 μs |  0.0282 μs |    5.74 KB
 | Newtonsoft.JObject       |   6.700 μs |  1.6404 μs |  0.0899 μs |   14.49 KB
 |                          |            |            |            |           
 | `$.store.book[-1:]`
 | JsonCraft.JsonElement    |   2.004 μs |  0.3158 μs |  0.0173 μs |    2.58 KB
 | Hyperbee.JsonElement     |   2.087 μs |  0.1721 μs |  0.0094 μs |    2.47 KB
 | JsonCons.JsonElement     |   2.399 μs |  0.3533 μs |  0.0194 μs |    3.56 KB
 | Hyperbee.JsonNode        |   2.467 μs |  0.9814 μs |  0.0538 μs |    2.97 KB
 | JsonEverything.JsonNode  |   3.679 μs |  0.4354 μs |  0.0239 μs |    5.72 KB
 | Newtonsoft.JObject       |   6.472 μs |  1.5636 μs |  0.0857 μs |   14.52 KB
 |                          |            |            |            |           
 | `$.store.book[:2]`
 | JsonCraft.JsonElement    |   2.010 μs |  0.1463 μs |  0.0080 μs |    2.58 KB
 | Hyperbee.JsonElement     |   2.128 μs |  0.4884 μs |  0.0268 μs |    2.47 KB
 | JsonCons.JsonElement     |   2.382 μs |  0.0585 μs |  0.0032 μs |    3.59 KB
 | Hyperbee.JsonNode        |   2.450 μs |  0.1728 μs |  0.0095 μs |    2.97 KB
 | JsonEverything.JsonNode  |   4.019 μs |  2.1096 μs |  0.1156 μs |    6.02 KB
 | Newtonsoft.JObject       |   6.899 μs |  0.6775 μs |  0.0371 μs |   14.51 KB
 |                          |            |            |            |           
 | `$.store.book[?(@.author && @.title)]`
 | JsonCraft.JsonElement    |   2.567 μs |  0.2239 μs |  0.0123 μs |     3.3 KB
 | Hyperbee.JsonElement     |   3.362 μs |  0.2369 μs |  0.0130 μs |    5.52 KB
 | JsonCons.JsonElement     |   3.805 μs |  0.8769 μs |  0.0481 μs |    5.63 KB
 | Hyperbee.JsonNode        |   5.128 μs |  1.1406 μs |  0.0625 μs |    9.23 KB
 | Newtonsoft.JObject       |   7.514 μs |  1.6892 μs |  0.0926 μs |   16.18 KB
 | JsonEverything.JsonNode  |   9.261 μs |  3.4741 μs |  0.1904 μs |   18.32 KB
 |                          |            |            |            |           
 | `$.store.book[?(@.category == 'fiction')]`
 | JsonCraft.JsonElement    |   2.734 μs |  0.3242 μs |  0.0178 μs |    3.38 KB
 | Hyperbee.JsonElement     |   3.315 μs |  0.3024 μs |  0.0166 μs |    5.09 KB
 | JsonCons.JsonElement     |   3.426 μs |  1.0773 μs |  0.0590 μs |    5.05 KB
 | Hyperbee.JsonNode        |   5.003 μs |  0.8363 μs |  0.0458 μs |    8.89 KB
 | Newtonsoft.JObject       |   7.213 μs |  1.1931 μs |  0.0654 μs |   15.74 KB
 | JsonEverything.JsonNode  |   8.898 μs |  1.8821 μs |  0.1032 μs |   16.49 KB
 |                          |            |            |            |           
 | `$.store.book[?(@.price < 10)].title`
 | JsonCraft.JsonElement    |   3.108 μs |  1.8864 μs |  0.1034 μs |    3.37 KB
 | Hyperbee.JsonElement     |   3.353 μs |  0.4814 μs |  0.0264 μs |     5.1 KB
 | JsonCons.JsonElement     |   4.008 μs |  0.5005 μs |  0.0274 μs |    5.27 KB
 | Hyperbee.JsonNode        |   5.285 μs |  0.7662 μs |  0.0420 μs |    8.78 KB
 | Newtonsoft.JObject       |   7.687 μs |  1.0056 μs |  0.0551 μs |   15.89 KB
 | JsonEverything.JsonNode  |   9.513 μs |  6.3528 μs |  0.3482 μs |   17.38 KB
 |                          |            |            |            |           
 | `$.store.book[?(@.price > 10 && @.price < 20)]`
 | JsonCraft.JsonElement    |   3.475 μs |  0.0797 μs |  0.0044 μs |    3.82 KB
 | Hyperbee.JsonElement     |   4.010 μs |  0.6169 μs |  0.0338 μs |    6.55 KB
 | JsonCons.JsonElement     |   5.102 μs |  1.1449 μs |  0.0628 μs |    6.28 KB
 | Hyperbee.JsonNode        |   6.086 μs |  1.3252 μs |  0.0726 μs |   10.27 KB
 | Newtonsoft.JObject       |   8.050 μs |  0.6497 μs |  0.0356 μs |   16.69 KB
 | JsonEverything.JsonNode  |  11.612 μs |  0.2688 μs |  0.0147 μs |   22.27 KB
 |                          |            |            |            |           
 | `$.store.book[?@.price == 8.99]`
 | Hyperbee.JsonElement     |   3.162 μs |  0.9502 μs |  0.0521 μs |     4.9 KB
 | JsonCons.JsonElement     |   3.822 μs |  0.0869 μs |  0.0048 μs |    5.02 KB
 | Hyperbee.JsonNode        |   4.889 μs |  1.5106 μs |  0.0828 μs |    8.58 KB
 | JsonEverything.JsonNode  |   8.310 μs |  2.0101 μs |  0.1102 μs |   15.47 KB
 | JsonCraft.JsonElement    |         NA |         NA |         NA |         NA
 | Newtonsoft.JObject       |         NA |         NA |         NA |         NA
 |                          |            |            |            |           
 | `$.store.book['category','author']`
 | JsonCraft.JsonElement    |   2.084 μs |  0.4663 μs |  0.0256 μs |    2.95 KB
 | JsonCons.JsonElement     |   2.480 μs |  0.0770 μs |  0.0042 μs |    3.61 KB
 | Hyperbee.JsonElement     |   2.569 μs |  1.1453 μs |  0.0628 μs |    2.67 KB
 | JsonEverything.JsonNode  |   3.309 μs |  0.2755 μs |  0.0151 μs |    5.41 KB
 | Hyperbee.JsonNode        |   4.142 μs |  1.4670 μs |  0.0804 μs |    6.42 KB
 | Newtonsoft.JObject       |   6.737 μs |  0.5860 μs |  0.0321 μs |   14.85 KB
 |                          |            |            |            |           
 | `$.store.book[*].author`
 | JsonCraft.JsonElement    |   2.256 μs |  1.4487 μs |  0.0794 μs |    2.63 KB
 | Hyperbee.JsonElement     |   2.469 μs |  0.7492 μs |  0.0411 μs |    3.12 KB
 | JsonCons.JsonElement     |   2.496 μs |  0.3427 μs |  0.0188 μs |    3.59 KB
 | Hyperbee.JsonNode        |   4.088 μs |  0.0348 μs |  0.0019 μs |    6.83 KB
 | Newtonsoft.JObject       |   7.169 μs |  1.8980 μs |  0.1040 μs |   14.64 KB
 | JsonEverything.JsonNode  |   7.511 μs |  0.2592 μs |  0.0142 μs |   12.45 KB
 |                          |            |            |            |           
 | `$.store.book[*]`
 | JsonCraft.JsonElement    |   2.048 μs |  1.8693 μs |  0.1025 μs |    2.48 KB
 | Hyperbee.JsonElement     |   2.179 μs |  0.6664 μs |  0.0365 μs |    2.71 KB
 | JsonCons.JsonElement     |   2.246 μs |  0.1811 μs |  0.0099 μs |     3.4 KB
 | Hyperbee.JsonNode        |   2.672 μs |  0.2099 μs |  0.0115 μs |    3.17 KB
 | JsonEverything.JsonNode  |   4.315 μs |  0.0253 μs |  0.0014 μs |    6.61 KB
 | Newtonsoft.JObject       |   6.632 μs |  1.6876 μs |  0.0925 μs |   14.49 KB
 |                          |            |            |            |           
 | `$.store.book[0,1]`
 | Hyperbee.JsonElement     |   2.049 μs |  0.0126 μs |  0.0007 μs |    2.47 KB
 | JsonCraft.JsonElement    |   2.056 μs |  0.2169 μs |  0.0119 μs |    2.64 KB
 | Hyperbee.JsonNode        |   2.435 μs |  0.5524 μs |  0.0303 μs |    2.97 KB
 | JsonCons.JsonElement     |   2.554 μs |  1.3052 μs |  0.0715 μs |    3.77 KB
 | JsonEverything.JsonNode  |   3.987 μs |  0.4523 μs |  0.0248 μs |    6.07 KB
 | Newtonsoft.JObject       |   6.648 μs |  0.7321 μs |  0.0401 μs |   14.59 KB
 |                          |            |            |            |           
 | `$.store.book[0].title`
 | Hyperbee.JsonElement     |   1.897 μs |  0.1620 μs |  0.0089 μs |    2.27 KB
 | JsonCraft.JsonElement    |   2.055 μs |  0.2876 μs |  0.0158 μs |    2.55 KB
 | JsonCons.JsonElement     |   2.318 μs |  0.3233 μs |  0.0177 μs |    3.35 KB
 | Hyperbee.JsonNode        |   2.575 μs |  0.1096 μs |  0.0060 μs |    3.63 KB
 | JsonEverything.JsonNode  |   4.534 μs |  1.5492 μs |  0.0849 μs |    7.38 KB
 | Newtonsoft.JObject       |   6.734 μs |  2.0745 μs |  0.1137 μs |   14.62 KB
 |                          |            |            |            |           
 | `$.store.book[0]`
 | Hyperbee.JsonElement     |   1.931 μs |  0.1500 μs |  0.0082 μs |    2.27 KB
 | JsonCraft.JsonElement    |   1.996 μs |  0.1804 μs |  0.0099 μs |    2.48 KB
 | Hyperbee.JsonNode        |   2.204 μs |  0.1747 μs |  0.0096 μs |    2.86 KB
 | JsonCons.JsonElement     |   2.221 μs |  0.1388 μs |  0.0076 μs |    3.26 KB
 | JsonEverything.JsonNode  |   3.592 μs |  0.3654 μs |  0.0200 μs |    5.68 KB
 | Newtonsoft.JObject       |   6.892 μs |  2.7132 μs |  0.1487 μs |   14.48 KB
 |                          |            |            |            |           
 | `$`
 | JsonCraft.JsonElement    |   1.732 μs |  0.0529 μs |  0.0029 μs |    2.26 KB
 | Hyperbee.JsonElement     |   1.751 μs |  0.0628 μs |  0.0034 μs |    2.27 KB
 | JsonEverything.JsonNode  |   1.767 μs |  0.0675 μs |  0.0037 μs |    1.88 KB
 | Hyperbee.JsonNode        |   1.790 μs |  0.1222 μs |  0.0067 μs |    1.78 KB
 | JsonCons.JsonElement     |   1.929 μs |  0.1539 μs |  0.0084 μs |    2.98 KB
 | Newtonsoft.JObject       |   6.587 μs |  0.7309 μs |  0.0401 μs |   14.01 KB

Benchmarks with issues:
  JsonPathParseAndSelectEvaluator.JsonCraft.JsonElement: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.isbn]]
  JsonPathParseAndSelectEvaluator.Newtonsoft.JObject: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.isbn]]
  JsonPathParseAndSelectEvaluator.JsonCraft.JsonElement: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.price == 8.99 && @.category == 'fiction']]
  JsonPathParseAndSelectEvaluator.Newtonsoft.JObject: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.price == 8.99 && @.category == 'fiction']]
  JsonPathParseAndSelectEvaluator.JsonCraft.JsonElement: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$.store.book[?@.price == 8.99]]
  JsonPathParseAndSelectEvaluator.Newtonsoft.JObject: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$.store.book[?@.price == 8.99]]
```
