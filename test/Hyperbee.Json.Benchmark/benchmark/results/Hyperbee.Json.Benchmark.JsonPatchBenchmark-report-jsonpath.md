```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i9-12900HK 2.50GHz, 1 CPU, 20 logical and 14 physical cores
.NET SDK 10.0.101
  [Host]   : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3 DEBUG
  ShortRun : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3


 | Method                   |       Mean |      Error |     StdDev |  Allocated
 | :----------------------- | ---------: | ---------: | ---------: | ---------:
 | Hyperbee_JsonElement     |   79.45 ns |   34.78 ns |   1.906 ns |      392 B
 | Hyperbee_JsonNode        |   84.85 ns |   64.17 ns |   3.517 ns |      392 B
 | JsonEverything_JsonNode  |  188.61 ns |  319.11 ns |  17.491 ns |      968 B
 | AspNetCore_JsonNode      |  353.32 ns |  106.43 ns |   5.834 ns |     1024 B
```
