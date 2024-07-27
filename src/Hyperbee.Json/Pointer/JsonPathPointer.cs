using Hyperbee.Json.Descriptors;

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
    private static readonly ITypeDescriptor<TNode> Descriptor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>();

    public static TNode FromPointer( TNode root, ReadOnlySpan<char> pointer )
    {
        var query = JsonPathQueryParser.Parse( pointer );
        var segment = query.Segments.Next; // skip the root segment

        return TryGetFromPointer( root, segment, out var value ) ? value : default;
    }

    public static bool TryGetFromPointer( TNode root, ReadOnlySpan<char> pointer, out TNode value )
    {
        var query = JsonPathQueryParser.Parse( pointer );
        var segment = query.Segments.Next; // skip the root segment

        return TryGetFromPointer( root, segment, out value );
    }

    internal static bool TryGetFromPointer( TNode root, JsonPathSegment segment, out TNode value )
    {
        if ( !segment.IsNormalized )
            throw new NotSupportedException( "Unsupported JsonPath pointer query format." );

        var accessor = Descriptor.ValueAccessor;

        var current = root;
        value = default;

        while ( !segment.IsFinal )
        {
            var (selectorValue, selectorKind) = segment.Selectors[0];

            switch ( selectorKind )
            {
                case SelectorKind.Name:
                    {
                        if ( accessor.GetNodeKind( current ) != NodeKind.Object )
                            return false;

                        if ( !accessor.TryGetProperty( current, selectorValue, out var child ) )
                            return false;

                        current = child;
                        break;
                    }

                case SelectorKind.Index:
                    {
                        if ( accessor.GetNodeKind( current ) != NodeKind.Array )
                            return false;

                        var length = accessor.GetArrayLength( current );
                        var index = int.Parse( selectorValue );

                        if ( index < 0 )
                            index = length + index;

                        if ( index < 0 || index >= length )
                            return false;

                        current = accessor.IndexAt( current, index );
                        break;
                    }

                default:
                    throw new NotSupportedException( $"Unsupported {nameof( SelectorKind )}." );
            }

            segment = segment.Next;
        }

        value = current;
        return true;
    }
}
