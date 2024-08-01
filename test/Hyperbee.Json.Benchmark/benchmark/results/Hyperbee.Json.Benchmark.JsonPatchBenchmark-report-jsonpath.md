```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22621.3880/22H2/2022Update/SunValley2)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.303
  [Host]   : .NET 8.0.7 (8.0.724.31311), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.7 (8.0.724.31311), X64 RyuJIT AVX2


 | Method                   |        Mean |      Error |    StdDev |  Allocated
 | :----------------------- | ----------: | ---------: | --------: | ---------:
 | Hyperbee_JsonNode        |    231.6 ns |   55.49 ns |   3.04 ns |      368 B
 | JsonEverything_JsonNode  |    405.4 ns |  135.07 ns |   7.40 ns |      728 B
 | Hyperbee_JsonElement     |    553.7 ns |   37.61 ns |   2.06 ns |      928 B
 | AspNetCore_JsonNode      |    744.9 ns |  519.69 ns |  28.49 ns |     1072 B
 |                          |             |            |           |           
 | JsonEverything_JsonNode  |    442.3 ns |  316.54 ns |  17.35 ns |      672 B
 | Hyperbee_JsonElement     |    543.2 ns |  206.29 ns |  11.31 ns |      872 B
 | AspNetCore_JsonNode      |  1,257.9 ns |  476.58 ns |  26.12 ns |     1648 B
 | Hyperbee_JsonNode        |          NA |         NA |        NA |         NA

Benchmarks with issues:
  JsonPatchBenchmark.Hyperbee_JsonNode: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Source={"name":"John","age":30,"city":"New York"}, Operations=[{ "op":"remove", "path":"/age" }]]
```
