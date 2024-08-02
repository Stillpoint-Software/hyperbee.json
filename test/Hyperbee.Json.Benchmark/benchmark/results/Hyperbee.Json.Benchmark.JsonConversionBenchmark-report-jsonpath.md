```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3958/23H2/2023Update/SunValley3)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.302
  [Host]   : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2


 | Method                         |      Mean |     Error |     StdDev |  Allocated
 | :----------------------------- | --------: | --------: | ---------: | ---------:
 | ConvertUsingConvertToNode      |  2.867 μs |  1.878 μs |  0.1029 μs |    4.58 KB
 | ConvertUsingArrayBufferWriter  |  3.861 μs |  3.174 μs |  0.1740 μs |    7.37 KB
```
