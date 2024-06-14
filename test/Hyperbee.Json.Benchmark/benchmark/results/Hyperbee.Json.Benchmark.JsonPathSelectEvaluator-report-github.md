```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22621.3593/22H2/2022Update/SunValley2)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.202
  [Host]   : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method                                   | Filter               | Mean       | Error      | StdDev    | Gen0   | Gen1   | Allocated |
|----------------------------------------- |--------------------- |-----------:|-----------:|----------:|-------:|-------:|----------:|
| JsonPath_Newtonsoft_JObject              | $..bo(...).99)] [27] |   2.159 μs |   1.442 μs | 0.0790 μs | 0.3929 |      - |   1.81 KB |
| JsonPath_ExpressionEvaluator_JsonElement | $..bo(...).99)] [27] |   9.236 μs |   5.298 μs | 0.2904 μs | 2.0447 | 0.0153 |   9.42 KB |
| JsonPath_ExpressionEvaluator_JsonNode    | $..bo(...).99)] [27] |  10.865 μs |   2.530 μs | 0.1387 μs | 2.7618 | 0.0153 |  12.71 KB |
| JsonPath_CSharpEvaluator_JsonNode        | $..bo(...).99)] [27] | 329.290 μs | 102.773 μs | 5.6333 μs | 4.3945 |      - |  22.12 KB |
| JsonPath_CSharpEvaluator_JsonElement     | $..bo(...).99)] [27] | 340.260 μs |  69.324 μs | 3.7999 μs | 3.9063 |      - |  20.14 KB |
