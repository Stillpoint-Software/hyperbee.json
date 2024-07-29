using System.Diagnostics.Contracts;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Path;

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
        return FromPointer( root, pointer, out _ );
    }

    public static TNode FromPointer( TNode root, ReadOnlySpan<char> pointer, out TNode parent )
    {
        var query = JsonQueryParser.Parse( pointer );
        var segment = query.Segments.Next; // skip the root segment

        return TryGetFromPointer( root, segment, out parent, out var value ) ? value : default;
    }

    internal static TNode FromPointer( TNode root, JsonPathSegment segment, out TNode parent )
    {
        return TryGetFromPointer( root, segment, out parent, out var value ) ? value : default;
    }

    public static bool TryGetFromPointer( TNode root, ReadOnlySpan<char> pointer, out TNode value )
    {
        return TryGetFromPointer( root, pointer, out _, out value );
    }

    public static bool TryGetFromPointer( TNode root, ReadOnlySpan<char> pointer, out TNode parent, out TNode value )
    {
        var query = JsonQueryParser.Parse( pointer );
        var segment = query.Segments.Next; // skip the root segment

        return TryGetFromPointer( root, segment, out parent, out value );
    }

    internal static bool TryGetFromPointer( TNode root, JsonPathSegment segment, out TNode parent, out TNode value )
    {
        if ( !segment.IsNormalized )
            throw new NotSupportedException( "Unsupported JsonPath pointer query format." );

        var accessor = Descriptor.ValueAccessor;

        value = default;
        parent = default;

        var current = root;
        var currentParent = parent;

        var typeMismatch = false;

        while ( !segment.IsFinal )
        {
            var (selectorValue, selectorKind) = segment.Selectors[0];

            currentParent = current;

            switch ( selectorKind )
            {
                case SelectorKind.Name:
                    {
                        if ( accessor.GetNodeKind( current ) != NodeKind.Object )
                        {
                            typeMismatch = true;
                            goto NotFound;
                        }

                        if ( !accessor.TryGetProperty( current, selectorValue, out current ) )
                            goto NotFound;
                        break;
                    }

                case SelectorKind.Index:
                    {
                        if ( accessor.GetNodeKind( current ) != NodeKind.Array )
                        {
                            typeMismatch = true;
                            goto NotFound;
                        }

                        var length = accessor.GetArrayLength( current );

                        var index = selectorValue == "-" // rfc6902 index append support
                            ? length
                            : int.Parse( selectorValue );

                        if ( index < 0 )
                            index = length + index;

                        if ( index < 0 || index >= length ) // out of bounds
                            goto NotFound;

                        current = accessor.IndexAt( current, index );
                        break;
                    }

                default:
                    throw new NotSupportedException( $"Unsupported {nameof( SelectorKind )}." );
            }

            segment = segment.Next;
        }

        value = current;
        parent = currentParent;

        return true;

NotFound:
// return parent if final segment fails.
// this is required for patch.
        if ( segment.Next.IsFinal && !typeMismatch )
            parent = currentParent;

        value = default;
        return false;
    }
}
