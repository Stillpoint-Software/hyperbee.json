using System.Collections.Concurrent;

namespace Hyperbee.Json.Query;

[Flags]
public enum SelectorKind
{
    Undefined = 0x0,

    // subtype
    Singular = 0x1,
    Group = 0x2,

    // selectors
    Root = 0x8 | Singular,
    Name = 0x10 | Singular,
    Index = 0x20 | Singular,
    Slice = 0x40 | Group,
    Filter = 0x80 | Group,
    Wildcard = 0x100 | Group,
    Descendant = 0x200 | Group
}

[Flags]
public enum JsonQueryParserOptions
{
    Rfc9535 = 1,
    Rfc6901 = 2,
    Rfc6902 = 4,

    Rfc9535AllowDotWhitespace = Rfc9535 | 8
}

public record JsonQuery( string Query, JsonSegment Segments, bool Normalized );

internal static class JsonQueryParser
{
    private static readonly ConcurrentDictionary<string, JsonQuery> JsonPathQueries = new();

    internal static void Clear() => JsonPathQueries.Clear();

    internal static JsonQuery Parse( ReadOnlySpan<char> query, JsonQueryParserOptions options = JsonQueryParserOptions.Rfc9535 )
    {
        return Parse( query.ToString(), options );
    }

    internal static JsonQuery Parse( string query, JsonQueryParserOptions options = JsonQueryParserOptions.Rfc9535 )
    {
        return JsonPathQueries.GetOrAdd( query, x =>
        {
            switch ( options )
            {
                case JsonQueryParserOptions.Rfc9535:
                case JsonQueryParserOptions.Rfc9535AllowDotWhitespace:
                    return Rfc9535QueryFactory.Parse( x.AsSpan(), options );

                case JsonQueryParserOptions.Rfc6901:
                case JsonQueryParserOptions.Rfc6902:
                    return Rfc6901QueryFactory.Parse( x.AsSpan(), options );

                default:
                    throw new ArgumentOutOfRangeException( nameof( options ) );
            }
        } );
    }
}
