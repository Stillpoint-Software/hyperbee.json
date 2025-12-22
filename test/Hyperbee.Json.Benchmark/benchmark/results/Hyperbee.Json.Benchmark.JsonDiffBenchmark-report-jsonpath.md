```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i9-12900HK 2.50GHz, 1 CPU, 20 logical and 14 physical cores
.NET SDK 10.0.101
  [Host]   : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3 [AttachedDebugger]
  ShortRun : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3


 | Method                |      Mean |      Error |   StdDev |  Allocated
 | :-------------------- | --------: | ---------: | -------: | ---------:
 | JsonDiff_JsonNode     |  431.2 ns |  109.04 ns |  5.98 ns |     1.3 KB
 | JsonDiff_JsonElement  |  636.4 ns |   70.49 ns |  3.86 ns |    1.93 KB
 |                       |           |            |          |           
 | JsonDiff_JsonNode     |  335.6 ns |   80.57 ns |  4.42 ns |     1.2 KB
 | JsonDiff_JsonElement  |  507.5 ns |  180.81 ns |  9.91 ns |    1.66 KB
```
