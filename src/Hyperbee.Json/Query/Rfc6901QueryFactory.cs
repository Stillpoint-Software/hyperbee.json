using Hyperbee.Json.Core;

namespace Hyperbee.Json.Query;

internal static class Rfc6901QueryFactory
{
    internal static JsonQuery Parse( ReadOnlySpan<char> query, JsonQueryParserOptions options )
    {
        // RFC 6901 - JSON Pointer

        var segments = new List<JsonSegment>();

        // Process the root '/' or '#/'

        switch ( query.IsEmpty )
        {
            case false when query.StartsWith( "/" ):
                AppendSegment( segments, SelectorKind.Root, "/" );
                query = query[1..];
                break;
            case false when query.StartsWith( "#/" ):
                AppendSegment( segments, SelectorKind.Root, "#/" );
                query = query[2..];
                break;
        }

        // Split the query by '/' 

        bool rfc6902 = options.HasFlag( JsonQueryParserOptions.Rfc6902 );
        var splitter = new SpanSplitter<char>( query, '/' );

        while ( splitter.TryMoveNext( out var part ) )
        {
            var decodedPart = DecodeJsonPointerPart( part );

            var selectorKind = int.TryParse( decodedPart, out _ ) || (rfc6902 && decodedPart == "-")
                ? SelectorKind.Index
                : SelectorKind.Name;

            AppendSegment( segments, selectorKind, decodedPart );
        }

        return JsonSegment.LinkSegments( query, segments );
    }

    private static void AppendSegment( List<JsonSegment> segments, SelectorKind selectorKind, string value )
    {
        var selector = new SelectorDescriptor { SelectorKind = selectorKind, Value = value };
        var segment = new JsonSegment( [selector] );

        segments.Add( segment );
    }

    private static string DecodeJsonPointerPart( ReadOnlySpan<char> part )
    {
        var builder = new ValueStringBuilder( stackalloc char[256] );

        for ( int i = 0; i < part.Length; i++ )
        {
            if ( part[i] != '~' || i + 1 >= part.Length )
            {
                builder.Append( part[i] );
                continue;
            }

            switch ( part[i + 1] )
            {
                case '1':
                    builder.Append( '/' );
                    i++;
                    break;
                case '0':
                    builder.Append( '~' );
                    i++;
                    break;
                default:
                    builder.Append( part[i] );
                    break;
            }
        }

        return builder.ToString();
    }
}
