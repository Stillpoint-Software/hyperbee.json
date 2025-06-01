```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.203
  [Host]   : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2
  ShortRun : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2


 | Method                   |      Mean |      Error |    StdDev |  Allocated
 | :----------------------- | --------: | ---------: | --------: | ---------:
 | Hyperbee_JsonElement     |  240.7 ns |   95.55 ns |   5.24 ns |      616 B
 | Hyperbee_JsonNode        |  265.3 ns |  430.49 ns |  23.60 ns |      616 B
 | JsonEverything_JsonNode  |  444.3 ns |  246.87 ns |  13.53 ns |      968 B
 | AspNetCore_JsonNode      |  743.4 ns |  801.29 ns |  43.92 ns |     1024 B
```
