```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3958/23H2/2023Update/SunValley3)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.302
  [Host]   : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2


 | Method                |        Mean |       Error |    StdDev |  Allocated
 | :-------------------- | ----------: | ----------: | --------: | ---------:
 | JsonDiff_JsonNode     |    935.1 ns |    480.7 ns |  26.35 ns |    1.22 KB
 | JsonDiff_JsonElement  |  1,154.6 ns |    699.7 ns |  38.35 ns |    1.66 KB
 |                       |             |             |           |           
 | JsonDiff_JsonNode     |  1,259.1 ns |    529.3 ns |  29.01 ns |    1.35 KB
 | JsonDiff_JsonElement  |  1,595.7 ns |  1,645.0 ns |  90.17 ns |    1.93 KB
```
