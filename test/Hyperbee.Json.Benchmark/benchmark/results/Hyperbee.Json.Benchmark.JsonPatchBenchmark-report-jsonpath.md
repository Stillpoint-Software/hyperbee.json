```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i9-12900HK 2.50GHz, 1 CPU, 20 logical and 14 physical cores
.NET SDK 10.0.101
  [Host]   : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3 [AttachedDebugger]
  ShortRun : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3


 | Method                   |       Mean |      Error |    StdDev |  Allocated
 | :----------------------- | ---------: | ---------: | --------: | ---------:
 | Hyperbee_JsonNode        |   78.55 ns |  14.146 ns |  0.775 ns |      392 B
 | Hyperbee_JsonElement     |   83.08 ns |  11.340 ns |  0.622 ns |      392 B
 | JsonEverything_JsonNode  |  184.07 ns |   2.770 ns |  0.152 ns |      968 B
 | AspNetCore_JsonNode      |  325.06 ns |  51.946 ns |  2.847 ns |     1024 B
```
