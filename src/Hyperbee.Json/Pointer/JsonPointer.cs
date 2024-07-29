using Hyperbee.Json.Query;

namespace Hyperbee.Json.Pointer;

public static class JsonPointer<TNode>
{
    public static TNode FromPointer( TNode root, ReadOnlySpan<char> pointer, bool rfc6902 = false )
    {
        var options = rfc6902
            ? JsonQueryParserOptions.Rfc6902
            : JsonQueryParserOptions.Rfc6901;

        var query = JsonQueryParser.Parse( pointer, options );
        var segment = query.Segments.Next; // skip the root segment

        return SegmentPointer<TNode>.TryGetFromPointer( root, segment, out _, out var value ) ? value : default;
    }

    public static bool TryGetFromPointer( TNode root, ReadOnlySpan<char> pointer, out TNode value, bool rfc6902 = false )
    {
        var options = rfc6902
            ? JsonQueryParserOptions.Rfc6902
            : JsonQueryParserOptions.Rfc6901;

        var query = JsonQueryParser.Parse( pointer, options );
        var segment = query.Segments.Next; // skip the root segment
        return SegmentPointer<TNode>.TryGetFromPointer( root, segment, out _, out value );
    }
}
