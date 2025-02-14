```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3037)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 9.0.103
  [Host]   : .NET 9.0.2 (9.0.225.6610), X64 RyuJIT AVX2 [AttachedDebugger]
  ShortRun : .NET 9.0.2 (9.0.225.6610), X64 RyuJIT AVX2


 | Method                   |      Mean |      Error |   StdDev |  Allocated
 | :----------------------- | --------: | ---------: | -------: | ---------:
 | Hyperbee_JsonNode        |  139.2 ns |   26.87 ns |  1.47 ns |      520 B
 | Hyperbee_JsonElement     |  145.1 ns |   26.35 ns |  1.44 ns |      520 B
 | JsonEverything_JsonNode  |  232.5 ns |  116.18 ns |  6.37 ns |      968 B
 | AspNetCore_JsonNode      |  441.0 ns |  173.80 ns |  9.53 ns |     1024 B
```
