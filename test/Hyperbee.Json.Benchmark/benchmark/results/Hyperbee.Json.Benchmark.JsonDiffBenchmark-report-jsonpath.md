```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i9-12900HK 2.50GHz, 1 CPU, 20 logical and 14 physical cores
.NET SDK 10.0.101
  [Host]   : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3 DEBUG
  ShortRun : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3


 | Method                |      Mean |      Error |    StdDev |  Allocated
 | :-------------------- | --------: | ---------: | --------: | ---------:
 | JsonDiff_JsonNode     |  438.2 ns |   45.95 ns |   2.52 ns |     1.3 KB
 | JsonDiff_JsonElement  |  663.2 ns |  538.12 ns |  29.50 ns |    1.93 KB
 |                       |           |            |           |           
 | JsonDiff_JsonNode     |  347.0 ns |  117.11 ns |   6.42 ns |     1.2 KB
 | JsonDiff_JsonElement  |  472.1 ns |  175.95 ns |   9.64 ns |    1.66 KB
```
