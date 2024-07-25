using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Patch;

public static class JsonDiff<TNode>
{
    private readonly record struct DiffOperation( TNode Source, TNode Target, string Path );

    private static readonly IValueAccessor<TNode> Accessor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>().Accessor;

    public static IEnumerable<PatchOperation> Diff( TNode source, TNode target )
    {
        var stack = new Stack<DiffOperation>( 6 );
        stack.Push( new DiffOperation( source, target, string.Empty ) );

        while ( stack.Count > 0 )
        {
            var operation = stack.Pop();

            var sourceKind = Accessor.GetNodeKind( operation.Source );
            var targetKind = Accessor.GetNodeKind( operation.Target );

            if ( sourceKind != targetKind )
            {
                yield return new PatchOperation
                {
                    Operation = PatchOperationType.Replace,
                    Path = operation.Path,
                    Value = operation.Target
                };
            }
            else
            {
                switch ( sourceKind )
                {
                    case NodeKind.Object:
                        foreach ( var (value, name, _) in Accessor.EnumerateChildren( operation.Source ) )
                        {
                            var propertyPath = Combine( operation.Path, name );

                            // TODO: does the accessor really need selector kind?
                            if ( !Accessor.TryGetChild( operation.Target, name, SelectorKind.Undefined, out var targetValue ) )
                            {
                                yield return new PatchOperation
                                {
                                    Operation = PatchOperationType.Remove,
                                    Path = propertyPath
                                };
                            }
                            else
                            {
                                stack.Push( new DiffOperation( value, targetValue, propertyPath ) );
                            }
                        }

                        foreach ( var (value, name, _) in Accessor.EnumerateChildren( operation.Target ) )
                        {
                            var propertyPath = Combine( operation.Path, name );

                            if ( !Accessor.TryGetChild( operation.Source, name, SelectorKind.Undefined, out var _ ) )
                            {
                                yield return new PatchOperation
                                {
                                    Operation = PatchOperationType.Add,
                                    Path = propertyPath,
                                    Value = value
                                };
                            }
                        }

                        break;

                    case NodeKind.Array:
                        var sourceLength = Accessor.GetArrayLength( operation.Source );
                        var targetLength = Accessor.GetArrayLength( operation.Target );
                        int maxLength = Math.Max( sourceLength, targetLength );

                        for ( int i = 0; i < maxLength; i++ )
                        {
                            var indexPath = Combine( operation.Path, i.ToString() );

                            if ( i >= sourceLength )
                            {
                                for ( int j = i; j < targetLength; j++ )
                                {
                                    if ( Accessor.TryGetElementAt( operation.Target, i, out var targetValue ) )
                                    {
                                        yield return new PatchOperation { Operation = PatchOperationType.Add, Path = indexPath, Value = targetValue };
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException( "Failed to get element at index." );
                                    }
                                }

                                break;
                            }

                            if ( i >= targetLength )
                            {
                                for ( int j = i; j < sourceLength; j++ )
                                {
                                    yield return new PatchOperation { Operation = PatchOperationType.Remove, Path = indexPath };
                                }

                                break;
                            }

                            if ( Accessor.TryGetElementAt( operation.Source, i, out var sourceItemValue ) &&
                                Accessor.TryGetElementAt( operation.Target, i, out var targetItemValue ) )
                            {
                                stack.Push( new DiffOperation( sourceItemValue, targetItemValue, indexPath ) );
                            }

                        }

                        break;

                    case NodeKind.Value:
                    default:

                        if ( !Accessor.DeepEquals( operation.Source, operation.Target ) )
                        {
                            yield return new PatchOperation
                            {
                                Operation = PatchOperationType.Replace,
                                Path = operation.Path,
                                Value = operation.Target
                            };
                        }

                        break;
                }
            }
        }

        yield break;

        static string Combine( ReadOnlySpan<char> initial, ReadOnlySpan<char> path )
        {
            // Allocate enough space for worst case scenario
            var index = initial.Length;
            var escapedPath = new char[(index + 1 + path.Length * 2)];

            initial.CopyTo( escapedPath );
            escapedPath[index++] = '/';

            foreach ( var c in path )
            {
                switch ( c )
                {
                    case '/':
                        escapedPath[index++] = '~';
                        escapedPath[index++] = '1';
                        break;
                    case '~':
                        escapedPath[index++] = '~';
                        escapedPath[index++] = '0';
                        break;
                    default:
                        escapedPath[index++] = c;
                        break;
                }
            }

            return escapedPath.AsSpan( 0, index ).ToString();
        }
    }
}
