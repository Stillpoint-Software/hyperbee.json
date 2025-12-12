```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i9-12900HK 2.50GHz, 1 CPU, 20 logical and 14 physical cores
.NET SDK 10.0.101
  [Host]   : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3 DEBUG
  ShortRun : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3


 | Method                    |  Mean |  Error
 | :------------------------ | ----: | -----:
 | FilterParser_JsonElement  |    NA |     NA
 | FilterParser_JsonNode     |    NA |     NA

Benchmarks with issues:
  FilterExpressionParserEvaluator.FilterParser_JsonElement: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=("world" == 'world') && (true || false)]
  FilterExpressionParserEvaluator.FilterParser_JsonNode: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=("world" == 'world') && (true || false)]
```
