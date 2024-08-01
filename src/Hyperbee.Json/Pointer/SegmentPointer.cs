using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Query;

namespace Hyperbee.Json.Pointer;

public static class SegmentPointer<TNode>
{
    private static readonly ITypeDescriptor<TNode> Descriptor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>();

    internal static TNode FromPointer( TNode root, JsonSegment segment, out TNode parent )
    {
        return TryGetFromPointer( root, segment, out parent, out var value ) ? value : default;
    }

    internal static bool TryGetFromPointer( TNode root, JsonSegment segment, out TNode parent, out TNode value )
    {
        if ( !segment.IsNormalized )
            throw new NotSupportedException( "Unsupported pointer query format." );

        if ( !segment.IsFinal && segment.Selectors[0].SelectorKind == SelectorKind.Root )
            segment = segment.Next; // skip the root segment

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
