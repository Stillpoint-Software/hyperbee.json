```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3958/23H2/2023Update/SunValley3)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.302
  [Host]   : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2


 | Method                 |         Mean |       Error |     StdDev |  Allocated
 | :--------------------- | -----------: | ----------: | ---------: | ---------:
 | ConvertToJsonNodeFast  |     15.06 ns |    0.839 ns |   0.046 ns |       64 B
 | ConvertFromValue       |    143.81 ns |  158.759 ns |   8.702 ns |      160 B
 | ConvertToJsonNode      |  3,066.37 ns |  529.399 ns |  29.018 ns |     4688 B
```
