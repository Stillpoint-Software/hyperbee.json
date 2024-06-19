using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Validators;

namespace Hyperbee.Json.Benchmark;

public class Config : ManualConfig
{
    public Config()
    {
        AddJob( Job.ShortRun );
        AddExporter( MarkdownExporter.GitHub );
        AddValidator( JitOptimizationsValidator.DontFailOnError );
        AddLogger( ConsoleLogger.Default );
        AddColumnProvider(
            DefaultColumnProviders.Job,
            DefaultColumnProviders.Params,
            DefaultColumnProviders.Descriptor,
            DefaultColumnProviders.Metrics,
            DefaultColumnProviders.Statistics
        );

        // Customize the summary style to prevent truncation
        WithSummaryStyle( SummaryStyle.Default.WithMaxParameterColumnWidth( 50 ) );
        
        AddDiagnoser( MemoryDiagnoser.Default );

        Orderer = new FastestToSlowestByParamOrderer();
        ArtifactsPath = "benchmark";
    }
}
