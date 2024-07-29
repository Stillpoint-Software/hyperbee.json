using System.Buffers;
using Hyperbee.Json.Core;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Patch;

public static class JsonDiff<TNode>
{
    private readonly record struct DiffOperation( TNode Source, TNode Target, string Path );

    private static readonly ITypeDescriptor<TNode> Descriptor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>();

    public static IEnumerable<PatchOperation> Diff( TNode source, TNode target )
    {
        var operations = InternalDiff( source, target );
        return operations;
    }

    private static PatchOperation[] InternalDiff( TNode source, TNode target )
    {
        var stack = new Stack<DiffOperation>( 8 );
        var operations = new List<PatchOperation>();

        stack.Push( new DiffOperation( source, target, string.Empty ) );

        var accessor = Descriptor.ValueAccessor;

        while ( stack.Count > 0 )
        {
            var operation = stack.Pop();

            var sourceKind = accessor.GetNodeKind( operation.Source );
            var targetKind = accessor.GetNodeKind( operation.Target );

            if ( sourceKind != targetKind )
            {
                operations.Add( new PatchOperation { Operation = PatchOperationType.Replace, Path = operation.Path, Value = operation.Target } );
            }
            else
            {
                switch ( sourceKind )
                {
                    case NodeKind.Object:
                        ProcessObjectDiff( operation, stack, operations );
                        break;

                    case NodeKind.Array:
                        ProcessArrayDiff( operation, operations );
                        break;

                    case NodeKind.Value:
                    default:
                        if ( !Descriptor.NodeActions.DeepEquals( operation.Source, operation.Target ) )
                        {
                            operations.Add( new PatchOperation { Operation = PatchOperationType.Replace, Path = operation.Path, Value = operation.Target } );
                        }

                        break;
                }
            }
        }

        return [.. operations];
    }

    private static void ProcessObjectDiff( DiffOperation operation, Stack<DiffOperation> stack, List<PatchOperation> operations )
    {
        var (accessor, _) = Descriptor;

        foreach ( var (value, name) in accessor.EnumerateObject( operation.Source ) )
        {
            var propertyPath = Combine( operation.Path, name );

            if ( accessor.TryGetProperty( operation.Target, name, out var targetValue ) )
            {
                stack.Push( new DiffOperation( value, targetValue, propertyPath ) );
            }
            else
            {
                operations.Add( new PatchOperation { Operation = PatchOperationType.Remove, Path = propertyPath } );
            }
        }

        foreach ( var (value, name) in accessor.EnumerateObject( operation.Target ) )
        {
            var propertyPath = Combine( operation.Path, name );

            if ( accessor.TryGetProperty( operation.Source, name, out _ ) )
            {
                continue;
            }

            operations.Add( new PatchOperation { Operation = PatchOperationType.Add, Path = propertyPath, Value = value } );
        }
    }

    private static void ProcessArrayDiff( DiffOperation operation, List<PatchOperation> operations )
    {
        var (accessor, _) = Descriptor;

        var source = accessor.EnumerateArray( operation.Source ).ToArray();
        var target = accessor.EnumerateArray( operation.Target ).ToArray();

        var maxLength = (source.Length + 1) * (target.Length + 1);

        Span<int> matrix = maxLength <= 512
            ? stackalloc int[maxLength]
            : ArrayPool<int>.Shared.Rent( maxLength );

        CalculateLevenshteinMatrix( matrix, source, target );

        var i = source.Length;
        var j = target.Length;

        while ( i > 0 || j > 0 )
        {
            var currentPos = i * (target.Length + 1) + j;

            if ( i > 0 && matrix[currentPos] == matrix[(i - 1) * (target.Length + 1) + j] + 1 )
            {
                var path = Combine( operation.Path, (i - 1).ToString() );
                operations.Add( new PatchOperation( PatchOperationType.Remove, path, null, null ) );
                i--;
            }
            else if ( j > 0 && matrix[currentPos] == matrix[i * (target.Length + 1) + (j - 1)] + 1 )
            {
                var path = Combine( operation.Path, (j - 1).ToString() );
                operations.Add( new PatchOperation( PatchOperationType.Add, path, null, target[j - 1] ) );
                j--;
            }
            else if ( i > 0 && j > 0 )
            {
                if ( matrix[currentPos] == matrix[(i - 1) * (target.Length + 1) + (j - 1)] )
                {
                    i--;
                    j--;
                }
                else
                {
                    var path = Combine( operation.Path, (i - 1).ToString() );
                    operations.Add( new PatchOperation( PatchOperationType.Replace, path, null, target[j - 1] ) );
                    i--;
                    j--;
                }
            }
        }

        operations.Reverse();
    }

    private static void CalculateLevenshteinMatrix( Span<int> matrix, TNode[] source, TNode[] target )
    {
        for ( int row = 0; row <= source.Length; row++ )
            matrix[row * (target.Length + 1)] = row;
        for ( int col = 0; col <= target.Length; col++ )
            matrix[col] = col;

        for ( int row = 1; row <= source.Length; row++ )
        {
            for ( int col = 1; col <= target.Length; col++ )
            {
                var cost = Descriptor.NodeActions.DeepEquals( source[row - 1], target[col - 1] ) ? 0 : 1;

                // Calculate the cost of deletion, insertion, and replacement
                var delete = matrix[(row - 1) * (target.Length + 1) + col] + 1;
                var insert = matrix[row * (target.Length + 1) + (col - 1)] + 1;
                var replace = matrix[(row - 1) * (target.Length + 1) + (col - 1)] + cost;

                matrix[row * (target.Length + 1) + col] = Math.Min(
                    Math.Min( delete, insert ), replace );
            }
        }
    }

    private static string Combine( ReadOnlySpan<char> initial, ReadOnlySpan<char> path )
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
