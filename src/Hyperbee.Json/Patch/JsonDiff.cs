using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Internal;

namespace Hyperbee.Json.Patch;

public static class JsonDiff<TNode>
{
    private readonly record struct DiffOperation( TNode Source, TNode Target, string Path );

    private static readonly ITypeDescriptor<TNode> Descriptor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>();
    private static readonly IValueAccessor<TNode> Accessor = Descriptor.ValueAccessor;

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
                        foreach ( var (value, name) in Accessor.EnumerateObject( operation.Source ) )
                        {
                            var propertyPath = Combine( operation.Path, name );

                            if ( !Accessor.TryGetChild( operation.Target, name, out var targetValue ) )
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

                        foreach ( var (value, name) in Accessor.EnumerateObject( operation.Target ) )
                        {
                            var propertyPath = Combine( operation.Path, name );

                            if ( !Accessor.TryGetChild( operation.Source, name, out _ ) )
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

                        if ( !Descriptor.NodeAccessor.DeepEquals( operation.Source, operation.Target ) )
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
            ReadOnlySpan<char> specialChars = ['/', '~'];

            var nextSpecialCharIndex = path.IndexOfAny( specialChars );

            if ( nextSpecialCharIndex == -1 )
                return string.Concat( initial, "/", path ); // span concatenation

            // we have escape characters, so we need to process the path

            var maxCapacity = path.Length + (path.Length - nextSpecialCharIndex);

            var builder = maxCapacity <= 512
                ? new ValueStringBuilder( stackalloc char[maxCapacity] )
                : new ValueStringBuilder( maxCapacity );

            ReadOnlySpan<char> escapeSlash = ['~', '1'];
            ReadOnlySpan<char> escapeTilde = ['~', '0'];

            var start = 0;
            while ( start < path.Length )
            {
                nextSpecialCharIndex = path[start..].IndexOfAny( specialChars );

                if ( nextSpecialCharIndex == -1 )
                {
                    builder.Append( path[start..] );
                    break;
                }

                builder.Append( path[start..nextSpecialCharIndex] );

                switch ( path[start + nextSpecialCharIndex] )
                {
                    case '/':
                        builder.Append( escapeSlash );
                        break;
                    case '~':
                        builder.Append( escapeTilde );
                        break;
                }

                start += nextSpecialCharIndex + 1;
            }

            var result = string.Concat( initial, "/", builder.AsSpan() );
            builder.Dispose();
            return result;
        }
    }
}
