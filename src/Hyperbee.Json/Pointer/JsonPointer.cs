namespace Hyperbee.Json.Pointer;

public static class JsonPointer<TNode>
{
    public static TNode FromPointer( TNode root, ReadOnlySpan<char> pointer )
    {
        var query = JsonPathQueryParser.ParseRfc6901( pointer );
        var segment = query.Segments.Next; // skip the root segment

        return JsonPathPointer<TNode>.TryGetFromPointer( root, segment, out var value ) ? value : default;
    }

    public static bool TryGetFromPointer( TNode root, ReadOnlySpan<char> pointer, out TNode value )
    {
        var query = JsonPathQueryParser.ParseRfc6901( pointer );
        var segment = query.Segments.Next; // skip the root segment
        return JsonPathPointer<TNode>.TryGetFromPointer( root, segment, out value );
    }
}
