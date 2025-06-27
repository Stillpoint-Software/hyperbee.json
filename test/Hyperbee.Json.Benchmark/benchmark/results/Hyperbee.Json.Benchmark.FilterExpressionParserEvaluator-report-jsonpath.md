```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4351)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 9.0.301
  [Host]   : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX2 [AttachedDebugger]
  ShortRun : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX2


 | Method                    |  Mean |  Error
 | :------------------------ | ----: | -----:
 | FilterParser_JsonElement  |    NA |     NA
 | FilterParser_JsonNode     |    NA |     NA

Benchmarks with issues:
  FilterExpressionParserEvaluator.FilterParser_JsonElement: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=("world" == 'world') && (true || false)]
  FilterExpressionParserEvaluator.FilterParser_JsonNode: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=("world" == 'world') && (true || false)]
```
