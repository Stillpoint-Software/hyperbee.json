```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i9-12900HK 2.50GHz, 1 CPU, 20 logical and 14 physical cores
.NET SDK 10.0.101
  [Host]   : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3 [AttachedDebugger]
  ShortRun : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3


 | Method                   |       Mean |       Error |     StdDev |  Allocated
 | :----------------------- | ---------: | ----------: | ---------: | ---------:
 | `$..[?(@.price < 10)]`
 | JsonCraft.JsonElement    |   2.957 μs |   1.1397 μs |  0.0625 μs |    3.59 KB
 | Hyperbee.JsonElement     |   5.221 μs |   0.8505 μs |  0.0466 μs |   15.84 KB
 | JsonCons.JsonElement     |   5.518 μs |   1.3367 μs |  0.0733 μs |   12.73 KB
 | Hyperbee.JsonNode        |   5.915 μs |   0.8714 μs |  0.0478 μs |   18.38 KB
 | Newtonsoft.JObject       |   8.009 μs |   1.7612 μs |  0.0965 μs |   25.86 KB
 | JsonEverything.JsonNode  |  17.850 μs |  12.0238 μs |  0.6591 μs |   48.15 KB
 |                          |            |             |            |           
 | `$..['bicycle','price']`
 | JsonCraft.JsonElement    |   2.309 μs |   0.2498 μs |  0.0137 μs |    4.01 KB
 | Hyperbee.JsonElement     |   2.701 μs |   1.0824 μs |  0.0593 μs |    5.12 KB
 | JsonCons.JsonElement     |   3.058 μs |   0.3820 μs |  0.0209 μs |    7.09 KB
 | Hyperbee.JsonNode        |   3.626 μs |   1.3209 μs |  0.0724 μs |    8.84 KB
 | Newtonsoft.JObject       |   4.990 μs |   1.5961 μs |  0.0875 μs |   14.55 KB
 | JsonEverything.JsonNode  |  12.183 μs |   2.7932 μs |  0.1531 μs |    28.5 KB
 |                          |            |             |            |           
 | `$..*`
 | JsonCraft.JsonElement    |   2.023 μs |   0.4743 μs |  0.0260 μs |    2.88 KB
 | Hyperbee.JsonElement     |   2.390 μs |   0.7132 μs |  0.0391 μs |    6.45 KB
 | JsonCons.JsonElement     |   3.239 μs |   0.8771 μs |  0.0481 μs |    8.45 KB
 | Hyperbee.JsonNode        |   3.864 μs |   2.6714 μs |  0.1464 μs |    9.54 KB
 | Newtonsoft.JObject       |   4.864 μs |   0.7073 μs |  0.0388 μs |   14.19 KB
 | JsonEverything.JsonNode  |  17.367 μs |  10.7988 μs |  0.5919 μs |   33.97 KB
 |                          |            |             |            |           
 | `$..author`
 | JsonCraft.JsonElement    |   2.071 μs |   0.8571 μs |  0.0470 μs |    2.88 KB
 | Hyperbee.JsonElement     |   2.275 μs |   0.5597 μs |  0.0307 μs |     5.1 KB
 | JsonCons.JsonElement     |   2.572 μs |   0.2856 μs |  0.0157 μs |    5.47 KB
 | Hyperbee.JsonNode        |   3.226 μs |   0.6372 μs |  0.0349 μs |    8.64 KB
 | Newtonsoft.JObject       |   4.793 μs |   3.7445 μs |  0.2053 μs |    14.2 KB
 | JsonEverything.JsonNode  |  11.786 μs |   2.5761 μs |  0.1412 μs |    26.1 KB
 |                          |            |             |            |           
 | `$..book[?@.isbn]`
 | Hyperbee.JsonElement     |   2.814 μs |   0.7079 μs |  0.0388 μs |    6.14 KB
 | JsonCons.JsonElement     |   3.348 μs |   0.4197 μs |  0.0230 μs |    7.16 KB
 | Hyperbee.JsonNode        |   3.587 μs |   1.1927 μs |  0.0654 μs |    9.64 KB
 | JsonEverything.JsonNode  |  12.860 μs |   6.6350 μs |  0.3637 μs |   29.98 KB
 | JsonCraft.JsonElement    |         NA |          NA |         NA |         NA
 | Newtonsoft.JObject       |         NA |          NA |         NA |         NA
 |                          |            |             |            |           
 | `$..book[?@.price == 8.99 && @.category == 'fiction']`
 | Hyperbee.JsonElement     |   3.599 μs |   1.1407 μs |  0.0625 μs |    8.28 KB
 | Hyperbee.JsonNode        |   4.593 μs |   2.6977 μs |  0.1479 μs |   11.91 KB
 | JsonCons.JsonElement     |   4.684 μs |   3.2355 μs |  0.1774 μs |    8.48 KB
 | JsonEverything.JsonNode  |  17.000 μs |  14.9488 μs |  0.8194 μs |   39.52 KB
 | JsonCraft.JsonElement    |         NA |          NA |         NA |         NA
 | Newtonsoft.JObject       |         NA |          NA |         NA |         NA
 |                          |            |             |            |           
 | `$..book[0,1]`
 | JsonCraft.JsonElement    |   2.184 μs |   1.1145 μs |  0.0611 μs |    3.09 KB
 | Hyperbee.JsonElement     |   2.316 μs |   0.6324 μs |  0.0347 μs |     5.1 KB
 | JsonCons.JsonElement     |   2.973 μs |   3.2806 μs |  0.1798 μs |    6.06 KB
 | Hyperbee.JsonNode        |   3.363 μs |   2.4201 μs |  0.1327 μs |    8.64 KB
 | Newtonsoft.JObject       |   4.709 μs |   1.1244 μs |  0.0616 μs |   14.45 KB
 | JsonEverything.JsonNode  |  11.585 μs |   1.1525 μs |  0.0632 μs |   26.41 KB
 |                          |            |             |            |           
 | `$..book[0]`
 | JsonCraft.JsonElement    |   2.097 μs |   0.7390 μs |  0.0405 μs |       3 KB
 | Hyperbee.JsonElement     |   2.417 μs |   0.4037 μs |  0.0221 μs |     5.1 KB
 | JsonCons.JsonElement     |   2.767 μs |   1.0765 μs |  0.0590 μs |    5.59 KB
 | Hyperbee.JsonNode        |   3.439 μs |   2.6110 μs |  0.1431 μs |    8.64 KB
 | Newtonsoft.JObject       |   4.730 μs |   0.4614 μs |  0.0253 μs |   14.33 KB
 | JsonEverything.JsonNode  |  11.404 μs |   1.6551 μs |  0.0907 μs |   26.02 KB
 |                          |            |             |            |           
 | `$.store..price`
 | Hyperbee.JsonElement     |   2.174 μs |   0.2046 μs |  0.0112 μs |    4.73 KB
 | JsonCraft.JsonElement    |   2.174 μs |   0.9541 μs |  0.0523 μs |    3.13 KB
 | JsonCons.JsonElement     |   2.657 μs |   1.2199 μs |  0.0669 μs |    5.57 KB
 | Hyperbee.JsonNode        |   3.219 μs |   1.6130 μs |  0.0884 μs |    8.38 KB
 | Newtonsoft.JObject       |   4.751 μs |   0.3461 μs |  0.0190 μs |   14.34 KB
 | JsonEverything.JsonNode  |  12.312 μs |   4.6283 μs |  0.2537 μs |   26.63 KB
 |                          |            |             |            |           
 | `$.store.*`
 | JsonCraft.JsonElement    |   1.415 μs |   0.5540 μs |  0.0304 μs |    2.49 KB
 | Hyperbee.JsonElement     |   1.564 μs |   0.4252 μs |  0.0233 μs |    2.81 KB
 | JsonCons.JsonElement     |   1.699 μs |   0.5680 μs |  0.0311 μs |    3.27 KB
 | Hyperbee.JsonNode        |   1.762 μs |   0.0234 μs |  0.0013 μs |     2.9 KB
 | JsonEverything.JsonNode  |   2.264 μs |   0.3401 μs |  0.0186 μs |     4.8 KB
 | Newtonsoft.JObject       |   4.142 μs |   0.4524 μs |  0.0248 μs |   14.43 KB
 |                          |            |             |            |           
 | `$.store.bicycle.color`
 | Hyperbee.JsonElement     |   1.415 μs |   0.3898 μs |  0.0214 μs |    2.17 KB
 | JsonCraft.JsonElement    |   1.532 μs |   0.2165 μs |  0.0119 μs |    2.45 KB
 | JsonCons.JsonElement     |   1.671 μs |   0.2610 μs |  0.0143 μs |    3.23 KB
 | Hyperbee.JsonNode        |   1.706 μs |   0.4849 μs |  0.0266 μs |    2.88 KB
 | JsonEverything.JsonNode  |   2.629 μs |   1.6792 μs |  0.0920 μs |    5.74 KB
 | Newtonsoft.JObject       |   4.336 μs |   1.2294 μs |  0.0674 μs |   14.49 KB
 |                          |            |             |            |           
 | `$.store.book[-1:]`
 | JsonCraft.JsonElement    |   1.479 μs |   0.1113 μs |  0.0061 μs |    2.58 KB
 | Hyperbee.JsonElement     |   1.515 μs |   0.3284 μs |  0.0180 μs |    2.41 KB
 | JsonCons.JsonElement     |   1.813 μs |   0.0749 μs |  0.0041 μs |    3.52 KB
 | Hyperbee.JsonNode        |   1.821 μs |   0.1880 μs |  0.0103 μs |    2.97 KB
 | JsonEverything.JsonNode  |   2.686 μs |   0.5270 μs |  0.0289 μs |    5.72 KB
 | Newtonsoft.JObject       |   4.390 μs |   0.6167 μs |  0.0338 μs |   14.52 KB
 |                          |            |             |            |           
 | `$.store.book[:2]`
 | JsonCraft.JsonElement    |   1.547 μs |   0.3302 μs |  0.0181 μs |    2.58 KB
 | Hyperbee.JsonElement     |   1.554 μs |   1.0503 μs |  0.0576 μs |    2.41 KB
 | JsonCons.JsonElement     |   1.867 μs |   0.9304 μs |  0.0510 μs |    3.54 KB
 | Hyperbee.JsonNode        |   1.869 μs |   0.2955 μs |  0.0162 μs |    2.97 KB
 | JsonEverything.JsonNode  |   3.112 μs |   1.4792 μs |  0.0811 μs |    6.02 KB
 | Newtonsoft.JObject       |   4.344 μs |   4.3641 μs |  0.2392 μs |   14.51 KB
 |                          |            |             |            |           
 | `$.store.book[?(@.author && @.title)]`
 | JsonCraft.JsonElement    |   1.924 μs |   0.1844 μs |  0.0101 μs |     3.3 KB
 | Hyperbee.JsonElement     |   2.331 μs |   0.8201 μs |  0.0450 μs |    4.18 KB
 | JsonCons.JsonElement     |   2.776 μs |   0.5326 μs |  0.0292 μs |    5.58 KB
 | Hyperbee.JsonNode        |   3.377 μs |   0.4035 μs |  0.0221 μs |    8.08 KB
 | Newtonsoft.JObject       |   4.779 μs |   2.3452 μs |  0.1285 μs |   16.18 KB
 | JsonEverything.JsonNode  |   6.403 μs |   1.9211 μs |  0.1053 μs |   18.32 KB
 |                          |            |             |            |           
 | `$.store.book[?(@.category == 'fiction')]`
 | JsonCraft.JsonElement    |   2.038 μs |   0.5141 μs |  0.0282 μs |    3.38 KB
 | Hyperbee.JsonElement     |   2.317 μs |   0.2970 μs |  0.0163 μs |    4.34 KB
 | JsonCons.JsonElement     |   2.643 μs |   0.8319 μs |  0.0456 μs |    5.01 KB
 | Hyperbee.JsonNode        |   3.393 μs |   0.3912 μs |  0.0214 μs |     8.2 KB
 | Newtonsoft.JObject       |   4.663 μs |   1.3455 μs |  0.0737 μs |   15.74 KB
 | JsonEverything.JsonNode  |   6.502 μs |   4.8220 μs |  0.2643 μs |   16.49 KB
 |                          |            |             |            |           
 | `$.store.book[?(@.price < 10)].title`
 | JsonCraft.JsonElement    |   2.349 μs |   0.1454 μs |  0.0080 μs |    3.37 KB
 | Hyperbee.JsonElement     |   2.379 μs |   0.2508 μs |  0.0137 μs |    4.35 KB
 | JsonCons.JsonElement     |   3.008 μs |   1.7216 μs |  0.0944 μs |    5.18 KB
 | Hyperbee.JsonNode        |   3.453 μs |   0.6436 μs |  0.0353 μs |    8.09 KB
 | Newtonsoft.JObject       |   4.762 μs |   1.8678 μs |  0.1024 μs |   15.89 KB
 | JsonEverything.JsonNode  |   6.891 μs |   1.2944 μs |  0.0709 μs |   17.38 KB
 |                          |            |             |            |           
 | `$.store.book[?(@.price > 10 && @.price < 20)]`
 | JsonCraft.JsonElement    |   2.637 μs |   0.3234 μs |  0.0177 μs |    3.82 KB
 | Hyperbee.JsonElement     |   2.730 μs |   0.9871 μs |  0.0541 μs |    5.37 KB
 | JsonCons.JsonElement     |   3.726 μs |   0.1036 μs |  0.0057 μs |    6.23 KB
 | Hyperbee.JsonNode        |   4.091 μs |   0.3535 μs |  0.0194 μs |    9.14 KB
 | Newtonsoft.JObject       |   5.167 μs |   2.3846 μs |  0.1307 μs |   16.69 KB
 | JsonEverything.JsonNode  |   8.309 μs |   1.7991 μs |  0.0986 μs |   22.02 KB
 |                          |            |             |            |           
 | `$.store.book[?@.price == 8.99]`
 | Hyperbee.JsonElement     |   2.249 μs |   0.4713 μs |  0.0258 μs |    4.15 KB
 | JsonCons.JsonElement     |   2.763 μs |   1.0265 μs |  0.0563 μs |    4.97 KB
 | Hyperbee.JsonNode        |   3.416 μs |   2.2807 μs |  0.1250 μs |    7.89 KB
 | JsonEverything.JsonNode  |   6.221 μs |   5.2637 μs |  0.2885 μs |   15.47 KB
 | JsonCraft.JsonElement    |         NA |          NA |         NA |         NA
 | Newtonsoft.JObject       |         NA |          NA |         NA |         NA
 |                          |            |             |            |           
 | `$.store.book['category','author']`
 | JsonCraft.JsonElement    |   1.575 μs |   0.1797 μs |  0.0099 μs |    2.95 KB
 | Hyperbee.JsonElement     |   1.931 μs |   0.2480 μs |  0.0136 μs |    2.61 KB
 | JsonCons.JsonElement     |   1.955 μs |   0.6808 μs |  0.0373 μs |    3.61 KB
 | JsonEverything.JsonNode  |   2.794 μs |   4.9383 μs |  0.2707 μs |    5.41 KB
 | Hyperbee.JsonNode        |   2.973 μs |   1.8205 μs |  0.0998 μs |    6.42 KB
 | Newtonsoft.JObject       |   4.169 μs |   1.0726 μs |  0.0588 μs |   14.85 KB
 |                          |            |             |            |           
 | `$.store.book[*].author`
 | JsonCraft.JsonElement    |   1.718 μs |   0.1185 μs |  0.0065 μs |    2.63 KB
 | Hyperbee.JsonElement     |   1.910 μs |   0.2440 μs |  0.0134 μs |    3.05 KB
 | JsonCons.JsonElement     |   1.972 μs |   1.2539 μs |  0.0687 μs |    3.55 KB
 | Hyperbee.JsonNode        |   2.945 μs |   0.7789 μs |  0.0427 μs |    6.83 KB
 | Newtonsoft.JObject       |   4.415 μs |   1.5198 μs |  0.0833 μs |   14.64 KB
 | JsonEverything.JsonNode  |   5.322 μs |   0.6684 μs |  0.0366 μs |   12.45 KB
 |                          |            |             |            |           
 | `$.store.book[*]`
 | JsonCraft.JsonElement    |   1.428 μs |   0.1454 μs |  0.0080 μs |    2.45 KB
 | Hyperbee.JsonElement     |   1.707 μs |   0.3096 μs |  0.0170 μs |    2.65 KB
 | JsonCons.JsonElement     |   1.725 μs |   1.0244 μs |  0.0562 μs |    3.35 KB
 | Hyperbee.JsonNode        |   1.948 μs |   0.1280 μs |  0.0070 μs |    3.17 KB
 | JsonEverything.JsonNode  |   3.429 μs |   3.3142 μs |  0.1817 μs |    6.61 KB
 | Newtonsoft.JObject       |   4.222 μs |   0.6171 μs |  0.0338 μs |   14.49 KB
 |                          |            |             |            |           
 | `$.store.book[0,1]`
 | Hyperbee.JsonElement     |   1.542 μs |   0.2708 μs |  0.0148 μs |    2.41 KB
 | JsonCraft.JsonElement    |   1.570 μs |   1.4388 μs |  0.0789 μs |    2.64 KB
 | Hyperbee.JsonNode        |   1.862 μs |   0.2584 μs |  0.0142 μs |    2.97 KB
 | JsonCons.JsonElement     |   1.922 μs |   0.6784 μs |  0.0372 μs |    3.73 KB
 | JsonEverything.JsonNode  |   3.016 μs |   0.8618 μs |  0.0472 μs |    6.07 KB
 | Newtonsoft.JObject       |   4.287 μs |   1.0252 μs |  0.0562 μs |   14.59 KB
 |                          |            |             |            |           
 | `$.store.book[0].title`
 | Hyperbee.JsonElement     |   1.457 μs |   0.3260 μs |  0.0179 μs |    2.17 KB
 | JsonCraft.JsonElement    |   1.768 μs |   1.8446 μs |  0.1011 μs |    2.51 KB
 | JsonCons.JsonElement     |   1.808 μs |   0.5749 μs |  0.0315 μs |     3.3 KB
 | Hyperbee.JsonNode        |   1.926 μs |   0.6973 μs |  0.0382 μs |    3.63 KB
 | JsonEverything.JsonNode  |   3.203 μs |   0.4522 μs |  0.0248 μs |    7.38 KB
 | Newtonsoft.JObject       |   4.559 μs |   3.8857 μs |  0.2130 μs |   14.62 KB
 |                          |            |             |            |           
 | `$.store.book[0]`
 | Hyperbee.JsonElement     |   1.349 μs |   0.5027 μs |  0.0276 μs |    2.17 KB
 | JsonCraft.JsonElement    |   1.493 μs |   0.5471 μs |  0.0300 μs |    2.44 KB
 | Hyperbee.JsonNode        |   1.657 μs |   0.3633 μs |  0.0199 μs |    2.86 KB
 | JsonCons.JsonElement     |   1.733 μs |   0.3863 μs |  0.0212 μs |    3.21 KB
 | JsonEverything.JsonNode  |   2.670 μs |   0.6862 μs |  0.0376 μs |    5.68 KB
 | Newtonsoft.JObject       |   4.155 μs |   1.3012 μs |  0.0713 μs |   14.48 KB
 |                          |            |             |            |           
 | `$`
 | JsonCraft.JsonElement    |   1.299 μs |   0.3820 μs |  0.0209 μs |    2.22 KB
 | Hyperbee.JsonElement     |   1.312 μs |   0.2933 μs |  0.0161 μs |    2.17 KB
 | Hyperbee.JsonNode        |   1.322 μs |   0.3885 μs |  0.0213 μs |    1.75 KB
 | JsonEverything.JsonNode  |   1.361 μs |   0.2123 μs |  0.0116 μs |    1.88 KB
 | JsonCons.JsonElement     |   1.477 μs |   0.6836 μs |  0.0375 μs |    2.94 KB
 | Newtonsoft.JObject       |   3.864 μs |   0.4926 μs |  0.0270 μs |   13.98 KB

Benchmarks with issues:
  JsonPathParseAndSelectEvaluator.JsonCraft.JsonElement: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.isbn]]
  JsonPathParseAndSelectEvaluator.Newtonsoft.JObject: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.isbn]]
  JsonPathParseAndSelectEvaluator.JsonCraft.JsonElement: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.price == 8.99 && @.category == 'fiction']]
  JsonPathParseAndSelectEvaluator.Newtonsoft.JObject: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.price == 8.99 && @.category == 'fiction']]
  JsonPathParseAndSelectEvaluator.JsonCraft.JsonElement: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$.store.book[?@.price == 8.99]]
  JsonPathParseAndSelectEvaluator.Newtonsoft.JObject: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$.store.book[?@.price == 8.99]]
```
