using Hyperbee.Json.Query;

namespace Hyperbee.Json.Pointer;

// DISTINCT from JsonPath these extensions are intended to facilitate 'diving' for Json Properties
// using normalized paths. a normalized path is an absolute path that references a single element.
// Similar to JsonPointer but using JsonPath notation.
//
// syntax supports absolute paths; dotted notation, quoted names, and simple bracketed array accessors only.
//
// Json path style wildcard '*', '..', and '[a,b]' multi-result selector notations are NOT supported.

public static class JsonPathPointer<TNode>
{
    public static TNode FromPointer( TNode root, ReadOnlySpan<char> pointer )
    {
        var query = JsonQueryParser.Parse( pointer );
        return SegmentPointer<TNode>.TryGetFromPointer( root, query.Segments, out _, out var value ) ? value : default;
    }

    public static bool TryGetFromPointer( TNode root, ReadOnlySpan<char> pointer, out TNode value )
    {
        var query = JsonQueryParser.Parse( pointer );
        return SegmentPointer<TNode>.TryGetFromPointer( root, query.Segments, out _, out value );
    }
}
