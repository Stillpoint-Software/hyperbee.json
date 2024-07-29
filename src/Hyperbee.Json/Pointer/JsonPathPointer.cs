using Hyperbee.Json.Descriptors;
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
        return FromPointer( root, pointer, out _ );
    }

    public static TNode FromPointer( TNode root, ReadOnlySpan<char> pointer, out TNode parent )
    {
        var query = JsonQueryParser.Parse( pointer );
        var segment = query.Segments.Next; // skip the root segment

        return SegmentPointer<TNode>.TryGetFromPointer( root, segment, out parent, out var value ) ? value : default;
    }

    public static bool TryGetFromPointer( TNode root, ReadOnlySpan<char> pointer, out TNode value )
    {
        return TryGetFromPointer( root, pointer, out _, out value );
    }

    public static bool TryGetFromPointer( TNode root, ReadOnlySpan<char> pointer, out TNode parent, out TNode value )
    {
        var query = JsonQueryParser.Parse( pointer );
        var segment = query.Segments.Next; // skip the root segment

        return SegmentPointer<TNode>.TryGetFromPointer( root, segment, out parent, out value );
    }
}
