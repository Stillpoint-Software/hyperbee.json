```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4351)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 9.0.301
  [Host]   : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX2 [AttachedDebugger]
  ShortRun : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX2


 | Method                |      Mean |      Error |    StdDev |  Allocated
 | :-------------------- | --------: | ---------: | --------: | ---------:
 | JsonDiff_JsonNode     |  572.1 ns |  164.76 ns |   9.03 ns |     1.2 KB
 | JsonDiff_JsonElement  |  704.5 ns |  219.53 ns |  12.03 ns |    1.66 KB
 |                       |           |            |           |           
 | JsonDiff_JsonNode     |  704.4 ns |  267.47 ns |  14.66 ns |     1.3 KB
 | JsonDiff_JsonElement  |  905.6 ns |   73.54 ns |   4.03 ns |    1.93 KB
```
