using Hyperbee.Json.Path;

namespace Hyperbee.Json.Pointer;

public static class JsonPointer<TNode>
{
    public static TNode FromPointer( TNode root, ReadOnlySpan<char> pointer, bool rfc6902 = false )
    {
        var query = JsonPathQueryParser.ParseRfc6901( pointer, rfc6902 );
        var segment = query.Segments.Next; // skip the root segment

        return JsonPathPointer<TNode>.TryGetFromPointer( root, segment, out _, out var value ) ? value : default;
    }

    public static bool TryGetFromPointer( TNode root, ReadOnlySpan<char> pointer, out TNode value, bool rfc6902 = false )
    {
        var query = JsonPathQueryParser.ParseRfc6901( pointer, rfc6902 );
        var segment = query.Segments.Next; // skip the root segment
        return JsonPathPointer<TNode>.TryGetFromPointer( root, segment, out _, out value );
    }
}
