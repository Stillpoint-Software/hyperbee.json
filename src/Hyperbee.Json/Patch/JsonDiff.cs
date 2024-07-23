using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Patch;

public static class JsonDiff<TNode>
{
    private readonly record struct DiffOperation( TNode Source, TNode Target, string Path );

    private static readonly IValueAccessor<TNode> Accessor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>().Accessor;

    public static IEnumerable<JsonPatchOperation> Diff( TNode source, TNode target )
    {
        var stack = new Stack<DiffOperation>( 6 );
        stack.Push( new DiffOperation( source, target, String.Empty ) );

        while ( stack.Count > 0 )
        {
            var operation = stack.Pop();

            var sourceKind = Accessor.GetNodeKind( operation.Source );
            var targetKind = Accessor.GetNodeKind( operation.Target );

            if ( sourceKind != targetKind )
            {
                yield return new JsonPatchOperation
                {
                    Operation = JsonPatchOperationType.Replace,
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
                            var propertyPath = operation.Path + "/" + name;

                            if ( !Accessor.TryGetChild( operation.Target, name, SelectorKind.Undefined, out var targetValue ) )
                            {
                                yield return new JsonPatchOperation
                                {
                                    Operation = JsonPatchOperationType.Remove,
                                    Path = propertyPath
                                };
                            }
                            else
                            {
                                stack.Push( new DiffOperation( value, targetValue, operation.Path + "/" + name ) );
                            }
                        }

                        foreach ( var (value, name, _) in Accessor.EnumerateChildren( operation.Target ) )
                        {
                            var propertyPath = operation.Path + "/" + name;

                            if ( !Accessor.TryGetChild( operation.Source, name, SelectorKind.Undefined, out var _ ) )
                            {
                                yield return new JsonPatchOperation
                                {
                                    Operation = JsonPatchOperationType.Add,
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
                            var indexPath = operation.Path + "/" + i;

                            if ( i >= sourceLength )
                            {
                                for ( int j = i; j < targetLength; j++ )
                                {
                                    if ( Accessor.TryGetElementAt( operation.Target, i, out var targetValue ) )
                                    {
                                        yield return new JsonPatchOperation { Operation = JsonPatchOperationType.Add, Path = indexPath, Value = targetValue };
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
                                    yield return new JsonPatchOperation { Operation = JsonPatchOperationType.Remove, Path = indexPath };
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
                            yield return new JsonPatchOperation
                            {
                                Operation = JsonPatchOperationType.Replace,
                                Path = operation.Path,
                                Value = operation.Target
                            };
                        }

                        break;
                }
            }
        }
    }
}
