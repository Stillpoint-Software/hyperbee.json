using System.Collections.Immutable;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Hyperbee.Json.Benchmark.Helpers;

public class FastestToSlowestByParamOrderer : IOrderer
{
    public IEnumerable<BenchmarkCase> GetExecutionOrder(
        ImmutableArray<BenchmarkCase> benchmarksCase,
        IEnumerable<BenchmarkLogicalGroupRule> order = null ) =>
        benchmarksCase
            .OrderByDescending( benchmark => benchmark.Parameters["X"] )
            .ThenBy( benchmark => benchmark.Descriptor.WorkloadMethodDisplayInfo );

    public IEnumerable<BenchmarkCase> GetSummaryOrder( ImmutableArray<BenchmarkCase> benchmarksCase, Summary summary ) =>
        benchmarksCase
            .OrderBy( benchmark => benchmark.Parameters.DisplayInfo )
            .ThenBy( benchmark => summary[benchmark]?.ResultStatistics?.Mean ?? double.MaxValue );

    public string GetHighlightGroupKey( BenchmarkCase benchmarkCase ) => null;

    public string GetLogicalGroupKey( ImmutableArray<BenchmarkCase> allBenchmarksCases, BenchmarkCase benchmarkCase ) =>
        benchmarkCase.Parameters.DisplayInfo;

    public IEnumerable<IGrouping<string, BenchmarkCase>> GetLogicalGroupOrder(
        IEnumerable<IGrouping<string, BenchmarkCase>> logicalGroups,
        IEnumerable<BenchmarkLogicalGroupRule> order = null ) =>
        logicalGroups.OrderBy( it => it.Key );

    public bool SeparateLogicalGroups => true;
}
