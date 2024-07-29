using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Core;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Patch;

public static class JsonDiff<TNode>
{
    private readonly record struct DiffOperation( TNode Source, TNode Target, string Path );

    private static readonly ITypeDescriptor<TNode> Descriptor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>();

    public static IEnumerable<PatchOperation> Diff( object source, object target )
    {
        switch ( source )
        {
            case TNode sourceNode when target is TNode targetNode:
                return InternalDiff( sourceNode, targetNode );
            default:
                {
                    if ( typeof( TNode ) == typeof( JsonElement ) )
                    {
                        var sourceElement = JsonSerializer.SerializeToElement( source );
                        var targetElement = JsonSerializer.SerializeToElement( target );

                        return JsonDiff<JsonElement>.InternalDiff( sourceElement, targetElement );
                    }

                    if ( typeof( TNode ) == typeof( JsonNode ) )
                    {
                        var sourceNode = JsonSerializer.SerializeToNode( source );
                        var targetNode = JsonSerializer.SerializeToNode( target );

                        return JsonDiff<JsonNode>.InternalDiff( sourceNode, targetNode );
                    }

                    throw new NotSupportedException( $"Type {typeof( TNode )} is not supported." );
                }
        }
    }

    public static IEnumerable<PatchOperation> Diff( TNode source, TNode target )
    {
        return InternalDiff( source, target );
    }

    private static PatchOperation[] InternalDiff( TNode source, TNode target )
    {
        var stack = new Stack<DiffOperation>( 8 );
        var operations = new List<PatchOperation>( 8 );

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
                        ProcessArrayDiff( operation, stack, operations );
                        break;

                    case NodeKind.Value:
                    default:
                        ProcessValueDiff( operation, operations );

                        break;
                }
            }
        }

        return [.. operations];
    }

    private static void ProcessObjectDiff( DiffOperation operation, Stack<DiffOperation> stack, ICollection<PatchOperation> operations )
    {
        var accessor = Descriptor.ValueAccessor;

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

    private static void ProcessArrayDiff( DiffOperation operation, Stack<DiffOperation> stack, ICollection<PatchOperation> operations )
    {
        var accessor = Descriptor.ValueAccessor;

        var source = accessor.EnumerateArray( operation.Source ).ToArray();
        var target = accessor.EnumerateArray( operation.Target ).ToArray();

        var row = source.Length;
        var col = target.Length;

        var matrix = Matrix<int>.StackSize( row, col ) <= 1024
            ? new Matrix<int>( stackalloc int[(row + 1) * (col + 1)], row + 1, col + 1 )
            : new Matrix<int>( row + 1, col + 1 );

        try
        {
            CalculateLevenshteinMatrix( matrix, source, target );

            while ( row > 0 || col > 0 )
            {
                if ( row > 0 && matrix[row, col] == matrix[row - 1, col] + 1 )
                {
                    var path = Combine( operation.Path, (row - 1).ToString() );
                    operations.Add( new PatchOperation( PatchOperationType.Remove, path, null, null ) );
                    row--;
                }
                else if ( col > 0 && matrix[row, col] == matrix[row, col - 1] + 1 )
                {
                    var path = Combine( operation.Path, (col - 1).ToString() );
                    operations.Add( new PatchOperation( PatchOperationType.Add, path, null, target[col - 1] ) );
                    col--;
                }
                else if ( row > 0 && col > 0 )
                {
                    if ( matrix[row, col] == matrix[row - 1, col - 1] )
                    {
                        row--;
                        col--;
                    }
                    else
                    {
                        var sourceKind = accessor.GetNodeKind( source[row - 1] );
                        var targetKind = accessor.GetNodeKind( target[col - 1] );
                        var path = Combine( operation.Path, (row - 1).ToString() );

                        if ( sourceKind != targetKind )
                        {
                            // If they don't match they are always a replacement.
                            operations.Add( new PatchOperation( PatchOperationType.Replace, path, null, target[col - 1] ) );
                        }
                        else
                        {
                            // We already check if these were values when calculating the matrix,
                            // so we know this are objects or arrays, and we need further processing.
                            stack.Push( new DiffOperation( source[row - 1], target[col - 1], path ) );
                        }

                        row--;
                        col--;
                    }
                }
            }
        }
        finally
        {
            matrix.Dispose();
        }
    }

    private static void ProcessValueDiff( DiffOperation operation, ICollection<PatchOperation> operations )
    {
        if ( Descriptor.NodeActions.DeepEquals( operation.Source, operation.Target ) )
            return;

        operations.Add( new PatchOperation { Operation = PatchOperationType.Replace, Path = operation.Path, Value = operation.Target } );
    }

    private static void CalculateLevenshteinMatrix( Matrix<int> matrix, TNode[] source, TNode[] target )
    {
        var accessor = Descriptor.ValueAccessor;

        for ( var row = 0; row <= source.Length; row++ )
            matrix[row, 0] = row;

        for ( var col = 0; col <= target.Length; col++ )
            matrix[0, col] = col;

        for ( var row = 1; row <= source.Length; row++ )
        {
            for ( int col = 1; col <= target.Length; col++ )
            {
                var cost = 1;
                if ( accessor.TryGetValue( source[row - 1], out var sourceValue ) &&
                   accessor.TryGetValue( target[col - 1], out var targetValue ) )
                {
                    if ( Equals( targetValue, sourceValue ) )
                        cost = 0;
                }

                // Calculate the cost of deletion, insertion, and replacement
                var remove = matrix[row - 1, col] + 1;
                var add = matrix[row, col - 1] + 1;
                var replace = matrix[row - 1, col - 1] + cost;

                matrix[row, col] = Math.Min( Math.Min( remove, add ), replace );
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
