```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.203
  [Host]   : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2
  ShortRun : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2


 | Method                |        Mean |       Error |     StdDev |  Allocated
 | :-------------------- | ----------: | ----------: | ---------: | ---------:
 | JsonDiff_JsonNode     |    778.8 ns |    703.7 ns |   38.57 ns |     1.2 KB
 | JsonDiff_JsonElement  |  1,370.3 ns |  5,661.7 ns |  310.33 ns |    1.66 KB
 |                       |             |             |            |           
 | JsonDiff_JsonNode     |  1,013.3 ns |    781.3 ns |   42.82 ns |     1.3 KB
 | JsonDiff_JsonElement  |  1,304.2 ns |    347.2 ns |   19.03 ns |    1.93 KB
```
