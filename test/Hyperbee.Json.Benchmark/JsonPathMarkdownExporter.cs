using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;

namespace Hyperbee.Json.Benchmark;

// Custom exporter that groups tests by filter and displays only specified columns
public class JsonPathMarkdownExporter : ExporterBase
{
    protected override string FileExtension => "md";
    protected override string FileNameSuffix => "-jsonpath";

    protected bool UseCodeBlocks = true;
    protected string CodeBlockStart = "```";
    protected string CodeBlockEnd = "```";
    protected string TableHeaderSeparator = " | ";
    protected string TableColumnSeparator = " | ";
    protected bool ColumnsStartWithSeparator = true;

    // Property to specify which columns to display
    public List<string> VisibleColumns { get; set; } =
    [
        "Method",
        "Mean",
        "Error",
        "StdDev",
        "Gen0",
        "Gen1",
        "Allocated"
    ];

    public override void ExportToLog( Summary summary, ILogger logger )
    {
        if ( UseCodeBlocks )
        {
            logger.WriteLine( CodeBlockStart );
        }

        logger.WriteLine();
        foreach ( string infoLine in summary.HostEnvironmentInfo.ToFormattedString() )
        {
            logger.WriteLineInfo( infoLine );
        }

        logger.WriteLineInfo( summary.AllRuntimes );
        logger.WriteLine();

        PrintTable( summary, logger, summary.Style );

        var benchmarksWithTroubles = summary.Reports.Where( x => !x.GetResultRuns().Any() ).Select( x => x.BenchmarkCase ).ToList();
        if ( benchmarksWithTroubles.Count > 0 )
        {
            logger.WriteLine();
            logger.WriteLineError( "Benchmarks with issues:" );

            foreach ( var benchmarkWithTroubles in benchmarksWithTroubles )
            {
                logger.WriteLineError( "  " + benchmarkWithTroubles.DisplayInfo );
            }
        }

        if ( UseCodeBlocks )
        {
            logger.WriteLine( CodeBlockEnd );
        }
    }

    private void PrintTable( Summary summary, ILogger logger, SummaryStyle style )
    {
        var table = summary.Table;
        var columns = table.Columns.Where( x => x.NeedToShow && x.OriginalColumn.ColumnName != "Filter" && VisibleColumns.Contains( x.OriginalColumn.ColumnName ) ).ToArray();
        var filterColumn = table.Columns.FirstOrDefault( x => x.Header == "Filter" );

        if ( table.FullContent.Length == 0 )
        {
            logger.WriteLineError( "There are no benchmarks found " );
            logger.WriteLine();
            return;
        }

        if ( columns.Length == 0 )
        {
            logger.WriteLine();
            logger.WriteLine( "There are no columns to show " );
            return;
        }

        logger.WriteLine();

        PrintHeader( columns, logger );

        int rowCounter = 0;

        foreach ( var line in table.FullContent )
        {
            if ( table.FullContentStartOfLogicalGroup[rowCounter] )
            {
                if ( rowCounter > 0 )
                {
                    PrintEmptyLine( columns, logger );
                }

                if ( filterColumn != null )
                {
                    var filter = line[filterColumn.Index].Replace( "`", "" );
                    logger.WriteLine( $"{TableHeaderSeparator}`{filter}`" );
                }
            }

            PrintLine( line, columns, logger, style );
            rowCounter++;
        }
    }

    private void PrintHeader( SummaryTable.SummaryTableColumn[] columns, ILogger logger )
    {
        var header = string.Join( TableHeaderSeparator, columns.Select( x => FormatHeaderCell( x.Header, x ) ) );
        var separator = string.Join( TableHeaderSeparator, columns.Select( GetColumnAlignment ) );

        if ( ColumnsStartWithSeparator )
        {
            header = TableHeaderSeparator + header;
            separator = TableHeaderSeparator + separator;
        }

        logger.WriteLine( header );
        logger.WriteLine( separator );
    }

    private static string FormatHeaderCell( string header, SummaryTable.SummaryTableColumn column )
    {
        return column.OriginalColumn.IsNumeric
            ? header.PadLeft( column.Width )
            : header.PadRight( column.Width );
    }

    private static string GetColumnAlignment( SummaryTable.SummaryTableColumn column )
    {
        return column.OriginalColumn.IsNumeric
            ? new string( '-', column.Width - 1 ) + ":"
            : ":" + new string( '-', column.Width - 1 );
    }

    private void PrintLine( string[] line, SummaryTable.SummaryTableColumn[] columns, ILogger logger, SummaryStyle style )
    {
        var formattedLine = string.Join( TableColumnSeparator, columns.Select( ( x, index ) => FormatCell( line[x.Index], x, style ) ) );

        if ( ColumnsStartWithSeparator )
        {
            formattedLine = TableColumnSeparator + formattedLine;
        }

        logger.WriteLine( formattedLine );
    }

    private void PrintEmptyLine( SummaryTable.SummaryTableColumn[] columns, ILogger logger )
    {
        var emptyLine = string.Join( TableColumnSeparator, columns.Select( x => new string( ' ', x.Width ) ) );

        if ( ColumnsStartWithSeparator )
        {
            emptyLine = TableColumnSeparator + emptyLine;
        }

        logger.WriteLine( emptyLine );
    }

    private static string FormatCell( string cell, SummaryTable.SummaryTableColumn column, SummaryStyle style )
    {
        if ( string.IsNullOrEmpty( cell ) )
            return string.Empty;

        cell = column.OriginalColumn.IsNumeric
            ? cell.PadLeft( column.Width )
            : cell.PadRight( column.Width );

        if ( cell.Length > style.MaxParameterColumnWidth )
        {
            cell = cell[..style.MaxParameterColumnWidth] + "...";
        }

        return cell;
    }
}
