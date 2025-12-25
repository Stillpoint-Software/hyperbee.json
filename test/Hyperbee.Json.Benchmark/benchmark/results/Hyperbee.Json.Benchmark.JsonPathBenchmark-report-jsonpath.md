```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.101
  [Host]   : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3
  ShortRun : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3


 | Method                |         Mean |        Error |      StdDev |  Allocated
 | :-------------------- | -----------: | -----------: | ----------: | ---------:
 | `$..[?(@.price < 10)]`
 | Hyperbee.JsonElement  |  6,994.02 ns |  3,956.98 ns |  216.896 ns |    14056 B
 |                       |              |              |             |           
 | `$..['bicycle','price']`
 | Hyperbee.JsonElement  |  2,328.61 ns |  1,084.27 ns |   59.432 ns |     3072 B
 |                       |              |              |             |           
 | `$..*`
 | Hyperbee.JsonElement  |  1,668.53 ns |  1,533.68 ns |   84.066 ns |     4432 B
 |                       |              |              |             |           
 | `$..author`
 | Hyperbee.JsonElement  |  1,693.51 ns |  1,479.36 ns |   81.089 ns |     3056 B
 |                       |              |              |             |           
 | `$..book[?@.isbn]`
 | Hyperbee.JsonElement  |  2,478.21 ns |  2,475.52 ns |  135.692 ns |     4120 B
 |                       |              |              |             |           
 | `$..book[?@.price == 8.99 && @.category == 'fiction']`
 | Hyperbee.JsonElement  |  3,911.73 ns |  5,909.55 ns |  323.922 ns |     6312 B
 |                       |              |              |             |           
 | `$..book[0,1]`
 | Hyperbee.JsonElement  |  1,724.88 ns |    561.96 ns |   30.803 ns |     3056 B
 |                       |              |              |             |           
 | `$.store..price`
 | Hyperbee.JsonElement  |  1,568.51 ns |    179.48 ns |    9.838 ns |     2680 B
 |                       |              |              |             |           
 | `$.store.* #First()`
 | Hyperbee.JsonElement  |    440.89 ns |     43.81 ns |    2.401 ns |      752 B
 |                       |              |              |             |           
 | `$.store.*`
 | Hyperbee.JsonElement  |    452.31 ns |    124.85 ns |    6.843 ns |      712 B
 |                       |              |              |             |           
 | `$.store.bicycle.color`
 | Hyperbee.JsonElement  |    159.90 ns |    113.32 ns |    6.212 ns |       80 B
 |                       |              |              |             |           
 | `$.store.book[-1:]`
 | Hyperbee.JsonElement  |    428.00 ns |    934.88 ns |   51.244 ns |      296 B
 |                       |              |              |             |           
 | `$.store.book[:2]`
 | Hyperbee.JsonElement  |    420.74 ns |    397.59 ns |   21.793 ns |      296 B
 |                       |              |              |             |           
 | `$.store.book[?(!@.isbn)]`
 | Hyperbee.JsonElement  |  1,160.92 ns |    337.16 ns |   18.481 ns |     1360 B
 |                       |              |              |             |           
 | `$.store.book[?(@.author && @.title)]`
 | Hyperbee.JsonElement  |  1,646.82 ns |    413.39 ns |   22.659 ns |     2112 B
 |                       |              |              |             |           
 | `$.store.book[?(@.category == 'fiction')]`
 | Hyperbee.JsonElement  |  1,509.30 ns |    596.57 ns |   32.700 ns |     2272 B
 |                       |              |              |             |           
 | `$.store.book[?(@.price < 10 || @.category == 'fiction')]`
 | Hyperbee.JsonElement  |  2,475.60 ns |  1,523.46 ns |   83.506 ns |     3520 B
 |                       |              |              |             |           
 | `$.store.book[?(@.price < 10)].title`
 | Hyperbee.JsonElement  |  1,700.97 ns |    513.45 ns |   28.144 ns |     2288 B
 |                       |              |              |             |           
 | `$.store.book[?(@.price == 8.99)]`
 | Hyperbee.JsonElement  |  1,493.31 ns |    277.47 ns |   15.209 ns |     2080 B
 |                       |              |              |             |           
 | `$.store.book[?(@.price > 10 && @.price < 20)]`
 | Hyperbee.JsonElement  |  2,276.48 ns |  1,976.86 ns |  108.358 ns |     3328 B
 |                       |              |              |             |           
 | `$.store.book[?(length(@.title) > 10)]`
 | Hyperbee.JsonElement  |  1,719.12 ns |    329.32 ns |   18.051 ns |     2056 B
 |                       |              |              |             |           
 | `$.store.book['category','author']`
 | Hyperbee.JsonElement  |  1,140.57 ns |    523.85 ns |   28.714 ns |      504 B
 |                       |              |              |             |           
 | `$.store.book[*].author`
 | Hyperbee.JsonElement  |  1,011.08 ns |    451.88 ns |   24.769 ns |      960 B
 |                       |              |              |             |           
 | `$.store.book[*]`
 | Hyperbee.JsonElement  |    554.85 ns |     86.42 ns |    4.737 ns |      544 B
 |                       |              |              |             |           
 | `$.store.book[0,1]`
 | Hyperbee.JsonElement  |    482.44 ns |    685.33 ns |   37.565 ns |      296 B
 |                       |              |              |             |           
 | `$.store.book[0:3:2]`
 | Hyperbee.JsonElement  |    445.85 ns |    496.92 ns |   27.238 ns |      296 B
 |                       |              |              |             |           
 | `$.store.book[0].title`
 | Hyperbee.JsonElement  |    202.18 ns |    135.14 ns |    7.408 ns |       80 B
 |                       |              |              |             |           
 | `$.store.book[0]`
 | Hyperbee.JsonElement  |    159.31 ns |    127.33 ns |    6.979 ns |       80 B
 |                       |              |              |             |           
 | `$`
 | Hyperbee.JsonElement  |     33.32 ns |     14.04 ns |    0.770 ns |       56 B
```
