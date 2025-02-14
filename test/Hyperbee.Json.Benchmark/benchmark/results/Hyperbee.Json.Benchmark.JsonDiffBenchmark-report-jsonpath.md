```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3037)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 9.0.103
  [Host]   : .NET 9.0.2 (9.0.225.6610), X64 RyuJIT AVX2 [AttachedDebugger]
  ShortRun : .NET 9.0.2 (9.0.225.6610), X64 RyuJIT AVX2


 | Method                |      Mean |      Error |    StdDev |  Allocated
 | :-------------------- | --------: | ---------: | --------: | ---------:
 | JsonDiff_JsonNode     |  473.9 ns |   83.44 ns |   4.57 ns |     1.2 KB
 | JsonDiff_JsonElement  |  595.7 ns |  283.98 ns |  15.57 ns |    1.66 KB
 |                       |           |            |           |           
 | JsonDiff_JsonNode     |  591.9 ns |  262.91 ns |  14.41 ns |     1.3 KB
 | JsonDiff_JsonElement  |  775.5 ns |   75.61 ns |   4.14 ns |    1.93 KB
```
