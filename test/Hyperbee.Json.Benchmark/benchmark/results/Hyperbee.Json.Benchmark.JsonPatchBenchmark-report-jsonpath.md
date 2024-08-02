```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3958/23H2/2023Update/SunValley3)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.302
  [Host]   : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2


 | Method                   |      Mean |      Error |    StdDev |  Allocated
 | :----------------------- | --------: | ---------: | --------: | ---------:
 | Hyperbee_JsonNode        |  292.4 ns |   69.37 ns |   3.80 ns |      584 B
 | Hyperbee_JsonElement     |  295.2 ns |   32.43 ns |   1.78 ns |      584 B
 | JsonEverything_JsonNode  |  506.5 ns |  565.41 ns |  30.99 ns |      728 B
 | AspNetCore_JsonNode      |        NA |         NA |        NA |         NA

Benchmarks with issues:
  JsonPatchBenchmark.AspNetCore_JsonNode: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Source={"name":"John","age":30,"city":"New York"}, Operations=[{ "op":"replace", "path":"/city", "value":"LA" }]]
```
