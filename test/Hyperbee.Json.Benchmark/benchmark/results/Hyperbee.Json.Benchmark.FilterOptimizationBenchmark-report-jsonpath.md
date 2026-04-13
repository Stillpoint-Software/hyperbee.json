```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8117/25H2/2025Update/HudsonValley2)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.104
  [Host]  : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3
  .NET 10 : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3


 | Method    |          Mean |         Error |        StdDev |  Allocated
 | :-------- | ------------: | ------------: | ------------: | ---------:
 | `!@.isbn`
 | Evaluate  |      74.20 ns |      37.57 ns |      2.059 ns |       80 B
 | Compile   |   9,213.35 ns |   3,989.60 ns |    218.684 ns |     5623 B
 |           |               |               |               |           
 | `@.author && @.title`
 | Evaluate  |     171.09 ns |      54.46 ns |      2.985 ns |      304 B
 | Compile   |  13,802.41 ns |  30,709.56 ns |  1,683.295 ns |     7095 B
 |           |               |               |               |           
 | `@.category == 'fiction'`
 | Evaluate  |     165.90 ns |     306.88 ns |     16.821 ns |      312 B
 | Compile   |  11,377.25 ns |  35,512.21 ns |  1,946.544 ns |     6192 B
 |           |               |               |               |           
 | `@.price < 10`
 | Evaluate  |     171.30 ns |      30.95 ns |      1.696 ns |      280 B
 | Compile   |  12,221.72 ns |  36,320.44 ns |  1,990.846 ns |     6118 B
 |           |               |               |               |           
 | `@.price > 10 && @.price < 20`
 | Evaluate  |     396.08 ns |   1,083.75 ns |     59.404 ns |      584 B
 | Compile   |  12,714.44 ns |  57,649.43 ns |  3,159.960 ns |     9040 B
 |           |               |               |               |           
 | `length(@.title) > 10`
 | Evaluate  |     137.58 ns |      70.99 ns |      3.891 ns |      272 B
 | Compile   |  13,458.85 ns |   8,779.57 ns |    481.238 ns |     7368 B
```
