```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22621.3880/22H2/2022Update/SunValley2)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.303
  [Host]   : .NET 8.0.7 (8.0.724.31311), X64 RyuJIT AVX2 [AttachedDebugger]
  ShortRun : .NET 8.0.7 (8.0.724.31311), X64 RyuJIT AVX2


 | Method                |        Mean |        Error |    StdDev |  Allocated
 | :-------------------- | ----------: | -----------: | --------: | ---------:
 | JsonDiff_JsonNode     |    946.2 ns |     34.43 ns |   1.89 ns |    1.11 KB
 | JsonDiff_JsonElement  |  1,247.0 ns |    522.15 ns |  28.62 ns |    1.69 KB
 |                       |             |              |           |           
 | JsonDiff_JsonNode     |  1,258.1 ns |    201.71 ns |  11.06 ns |    1.24 KB
 | JsonDiff_JsonElement  |  1,638.2 ns |  1,441.50 ns |  79.01 ns |    1.91 KB
```
