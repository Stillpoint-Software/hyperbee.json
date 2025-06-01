```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.203
  [Host]   : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2
  ShortRun : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2


 | Method                    |  Mean |  Error
 | :------------------------ | ----: | -----:
 | FilterParser_JsonElement  |    NA |     NA
 | FilterParser_JsonNode     |    NA |     NA

Benchmarks with issues:
  FilterExpressionParserEvaluator.FilterParser_JsonElement: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=("world" == 'world') && (true || false)]
  FilterExpressionParserEvaluator.FilterParser_JsonNode: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=("world" == 'world') && (true || false)]
```
