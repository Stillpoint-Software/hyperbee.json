using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Internal;

namespace Hyperbee.Json.Patch;

public static class JsonDiff<TNode>
{
    private readonly record struct DiffOperation( TNode Source, TNode Target, string Path );

    private static readonly ITypeDescriptor<TNode> Descriptor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>();

    public static IEnumerable<PatchOperation> Diff( TNode source, TNode target, bool optimize = false )
    {
        var operations = InternalDiff( source, target );
        return operations;
    }

    private static IEnumerable<PatchOperation> InternalDiff( TNode source, TNode target )
    {
        var stack = new Stack<DiffOperation>( 8 );
        stack.Push( new DiffOperation( source, target, string.Empty ) );

        var accessor = Descriptor.ValueAccessor;

        while ( stack.Count > 0 )
        {
            var operation = stack.Pop();

            var sourceKind = accessor.GetNodeKind( operation.Source );
            var targetKind = accessor.GetNodeKind( operation.Target );

            if ( sourceKind != targetKind )
            {
                yield return new PatchOperation { Operation = PatchOperationType.Replace, Path = operation.Path, Value = operation.Target };
            }
            else
            {
                switch ( sourceKind )
                {
                    case NodeKind.Object:
                        foreach ( var (value, name) in accessor.EnumerateObject( operation.Source ) )
                        {
                            var propertyPath = Combine( operation.Path, name );

                            if ( !accessor.TryGetProperty( operation.Target, name, out var targetValue ) )
                            {
                                yield return new PatchOperation { Operation = PatchOperationType.Remove, Path = propertyPath };
                            }
                            else
                            {
                                stack.Push( new DiffOperation( value, targetValue, propertyPath ) );
                            }
                        }

                        foreach ( var (value, name) in accessor.EnumerateObject( operation.Target ) )
                        {
                            var propertyPath = Combine( operation.Path, name );

                            if ( !accessor.TryGetProperty( operation.Source, name, out _ ) )
                            {
                                yield return new PatchOperation { Operation = PatchOperationType.Add, Path = propertyPath, Value = value };
                            }
                        }

                        break;

                    case NodeKind.Array:
                        var sourceLength = accessor.GetArrayLength( operation.Source );
                        var targetLength = accessor.GetArrayLength( operation.Target );
                        int maxLength = Math.Max( sourceLength, targetLength );

                        for ( int i = 0; i < maxLength; i++ )
                        {
                            var indexPath = Combine( operation.Path, i.ToString() );

                            if ( i >= sourceLength )
                            {
                                for ( int j = i; j < targetLength; j++ )
                                {
                                    if ( accessor.TryGetIndexAt( operation.Target, i, out var targetValue ) )
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

                            if ( accessor.TryGetIndexAt( operation.Source, i, out var sourceItemValue ) &&
                                 accessor.TryGetIndexAt( operation.Target, i, out var targetItemValue ) )
                            {
                                stack.Push( new DiffOperation( sourceItemValue, targetItemValue, indexPath ) );
                            }

                        }

                        break;

                    case NodeKind.Value:
                    default:

                        if ( !Descriptor.NodeActions.DeepEquals( operation.Source, operation.Target ) )
                        {
                            yield return new PatchOperation { Operation = PatchOperationType.Replace, Path = operation.Path, Value = operation.Target };
                        }

                        break;
                }
            }
        }

        yield break;

        static string Combine( ReadOnlySpan<char> initial, ReadOnlySpan<char> path )
        {
            // Count special characters

            var specialCharCount = 0;
            for ( var i = 0; i < path.Length; i++ )
            {
                if ( path[i] == '/' || path[i] == '~' )
                    specialCharCount++;
            }

            if ( specialCharCount == 0 )
                return string.Concat( initial, "/", path );

            // Process special characters

            var size = path.Length + specialCharCount;

            var builder = size <= 512
                ? new ValueStringBuilder( stackalloc char[size] )
                : new ValueStringBuilder( size );

            ReadOnlySpan<char> escapeSlash = ['~', '1'];
            ReadOnlySpan<char> escapeTilde = ['~', '0'];

            var start = 0;
            for ( var i = 0; i < path.Length; i++ )
            {
                switch ( path[i] )
                {
                    case '/':
                        if ( i > start )
                            builder.Append( path[start..i] );

                        builder.Append( escapeSlash );
                        start = i + 1;
                        break;

                    case '~':
                        if ( i > start )
                            builder.Append( path[start..i] );

                        builder.Append( escapeTilde );
                        start = i + 1;
                        break;
                }
            }

            if ( start < path.Length ) // Append remaining
                builder.Append( path[start..] );

            var result = string.Concat( initial, "/", builder.AsSpan() );
            builder.Dispose();
            return result;
        }
    }
}
