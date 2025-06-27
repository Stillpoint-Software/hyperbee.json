```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4351)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 9.0.301
  [Host]   : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX2 [AttachedDebugger]
  ShortRun : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX2


 | Method                   |      Mean |      Error |   StdDev |  Allocated
 | :----------------------- | --------: | ---------: | -------: | ---------:
 | Hyperbee_JsonNode        |  172.9 ns |   44.86 ns |  2.46 ns |      520 B
 | Hyperbee_JsonElement     |  178.8 ns |   76.47 ns |  4.19 ns |      520 B
 | JsonEverything_JsonNode  |  289.7 ns |  108.59 ns |  5.95 ns |      968 B
 | AspNetCore_JsonNode      |  516.7 ns |   44.74 ns |  2.45 ns |     1024 B
```
