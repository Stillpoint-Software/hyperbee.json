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

        // Add the custom exporter with specified visible columns
        AddExporter( new JsonPathMarkdownExporter
        {
            VisibleColumns =
            [
                "Method",
                "Mean",
                "Error",
                "StdDev",
                "Allocated"
            ]
        } );

        AddDiagnoser( MemoryDiagnoser.Default );

        Orderer = new FastestToSlowestByParamOrderer();

        // Set the artifacts path to a specific directory in the project

        // Set the artifacts path to a specific directory in the project
        var projectFolder = FindParentFolder( "Hyperbee.Json.Benchmark" );
        ArtifactsPath = Path.Combine( projectFolder, "benchmark" );
    }

    private static string FindParentFolder( string target )
    {
        var currentDirectory = new DirectoryInfo( AppContext.BaseDirectory );

        while ( currentDirectory != null )
        {
            if ( currentDirectory.Name.Equals( target, StringComparison.OrdinalIgnoreCase ) )
            {
                return currentDirectory.FullName;
            }

            currentDirectory = currentDirectory.Parent;
        }

        throw new DirectoryNotFoundException( $"Could not find the target folder '{target}' in the directory tree." );
    }
}
