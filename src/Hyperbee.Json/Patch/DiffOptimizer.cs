using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Patch;

public interface IDiffOptimizer<in TNode>
{
    List<PatchOperation> OptimizeDiff( TNode source, TNode target, IEnumerable<PatchOperation> operations );
}

public class DiffOptimizer<TNode> : IDiffOptimizer<TNode>
{
    private static readonly ITypeDescriptor<TNode> Descriptor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>();

    public List<PatchOperation> OptimizeDiff( TNode source, TNode target, IEnumerable<PatchOperation> operations )
    {
        var optimizedOperations = new List<PatchOperation>( operations );

        // Optimize array operations
        optimizedOperations = OptimizeArrayOperations( source, target, optimizedOperations );

        // Remove redundant operations in nested objects
        optimizedOperations = RemoveRedundantObjectOperations( optimizedOperations );

        // Combine consecutive operations
        optimizedOperations = CombineConsecutiveOperations( optimizedOperations );

        return optimizedOperations;
    }

    // Iterate over the patch operations to find and optimize array operations.
    //
    // For each Replace operation targeting an array, it calculates the minimal
    // set of operations needed to transform the source array to the target array
    // using Levenshtein distance. Adds these optimized operations to the list.
    //
    // Before:
    // Add /a/0 1
    // Remove /a/2
    // Replace /a/3 5
    //
    // After (example optimized):
    // Replace /a/0 1
    // Replace /a/1 3
    // Replace /a/2 5
    //
    private List<PatchOperation> OptimizeArrayOperations( TNode source, TNode target, List<PatchOperation> operations )
    {
        // hacked together to figure our the pattern
        //
        // rather than optimize existing array operations we can compute the optimal operations
        // for the entire array and add them to the list of operations.
        // we need the source and target arrays.

        var operationPath = operations[0].Path; // grab the first operation to get the path
        var parentPath = GetParentPath( operationPath );

        var sourceArray = GetParent( source, parentPath );

        if ( Descriptor.ValueAccessor.GetNodeKind( sourceArray ) != NodeKind.Array )
            return [];

        var targetArray = GetParent( target, parentPath );

        if ( Descriptor.ValueAccessor.GetNodeKind( targetArray ) != NodeKind.Array )
            return [];

        return LevenshteinOptimize( sourceArray, targetArray, parentPath );


        static List<PatchOperation> LevenshteinOptimize( TNode source, TNode target, string path )
        {
            var sourceArray = Descriptor.ValueAccessor.EnumerateArray( source ).ToList();
            var targetArray = Descriptor.ValueAccessor.EnumerateArray( target ).ToList();

            var distance = CalculateLevenshteinDistance( sourceArray, targetArray );
            var operations = GetLevenshteinOperations( distance, sourceArray, targetArray, path );

            return operations;
        }
    }

    // Calculate the Levenshtein distance between two lists of nodes.
    //
    // The Levenshtein distance is a measure of the number of single-element
    // edits (insertions, deletions, or substitutions) required to change one
    // list into the other.
    //
    // Parameters:
    // - source: The source list of nodes.
    // - target: The target list of nodes.
    //
    // Returns:
    // - A 2D array where each element [i, j] represents the Levenshtein distance
    //   between the first i elements of the source list and the first j elements
    //   of the target list.
    //
    // Example:
    // - Source: [A, B, C]
    // - Target: [A, C, D]
    //
    // The resulting distance matrix would look like:
    //
    //  i (rows) is source
    //  j (columns) is target
    //
    //          A B C
    //      j 0 1 2 3
    //    i  --------
    //    0 | 0 1 2 3
    //  A 1 | 1 0 1 2
    //  C 2 | 2 1 1 2
    //  D 3 | 3 2 2 2
    // 
    // Explanation:
    // - The cell [i, j] contains the minimum number of operations needed to
    //   transform the first i elements of the source list into the first j
    //   elements of the target list.
    //
    // - The value of each cell is computed based on the values of its neighboring
    //   cells: the cell directly above ([i-1, j]), the cell directly to the left
    //   ([i, j-1]), and the cell diagonally above and to the left ([i-1, j-1]).
    //
    // - If source[i-1] equals target[j-1], the cost is 0, otherwise, it is 1.
    //
    // - The value of each cell is the minimum of:
    //   - The value of the cell above plus 1 (deletion)
    //   - The value of the cell to the left plus 1 (insertion)
    //   - The value of the diagonal cell plus the cost (substitution)
    //
    private static int[,] CalculateLevenshteinDistance( IList<TNode> source, IList<TNode> target )
    {
        var distance = new int[source.Count + 1, target.Count + 1];

        for ( var i = 0; i <= source.Count; i++ )
            distance[i, 0] = i;

        for ( var j = 0; j <= target.Count; j++ )
            distance[0, j] = j;

        for ( var i = 1; i <= source.Count; i++ )
        {
            for ( var j = 1; j <= target.Count; j++ )
            {
                var cost = Descriptor.NodeActions.DeepEquals( source[i - 1], target[j - 1] ) ? 0 : 1;
                
                distance[i, j] = Math.Min(
                    Math.Min( distance[i - 1, j] + 1, distance[i, j - 1] + 1 ),
                    distance[i - 1, j - 1] + cost );
            }
        }

        return distance;
    }

    // Generates the list of patch operations based on the Levenshtein distance matrix.
    // This method creates the minimal set of operations (add, remove, replace) required
    // to transform the source list into the target list.
    //
    // Parameters:
    // - distance: The Levenshtein distance matrix computed by CalculateLevenshteinDistance.
    // - source: The source list of nodes (rows of the matrix).
    // - target: The target list of nodes (columns of the matrix).
    // - path: The base path for the operations.
    //
    // Returns:
    // - A list of patch operations representing the minimal edits required to
    //   transform the source list into the target list.
    //
    // Example:
    // - Source: [A, B, C]
    // - Target: [A, C, D]
    //
    // The resulting operations might look like:
    // - Remove /1 (Remove B)
    // - Add /2 D (Add D at position 2)
    // - Replace /1 C (Replace B with C at position 1)
    //
    // Explanation:
    // - The method starts from the bottom-right corner of the matrix, which
    //   represents the distance between the entire source and target lists.
    // - It traces back through the matrix to determine the sequence of operations
    //   that led to the computed distance, adding operations to the list as it goes.
    // - If the current cell [i, j] equals the cell above it [i-1, j] plus 1, it means
    //   the optimal operation was a deletion, so it adds a Remove operation.
    // - If the current cell [i, j] equals the cell to the left [i, j-1] plus 1, it means
    //   the optimal operation was an insertion, so it adds an Add operation.
    // - If the current cell [i, j] equals the diagonal cell [i-1, j-1] plus the cost,
    //   it means the optimal operation was a substitution (or no change if cost is 0),
    //   so it adds a Replace operation.
    //
    // Detailed Example:
    //
    // - Source: [A, B, C]
    // - Target: [A, C, D]
    //
    // Distance Matrix:
    //
    //     j 0 1 2 3
    //   i  -------------
    //   0 | 0 1 2 3
    //   1 | 1 0 1 2
    //   2 | 2 1 1 2
    //   3 | 3 2 2 2
    //
    // Tracing back the operations:
    //
    // - Start at distance[3, 3] (value 2): Replace C with D
    // - Move to distance[2, 2] (value 1): No change, C == C
    // - Move to distance[1, 2] (value 1): Add D at position 2
    // - Move to distance[1, 1] (value 0): No change, A == A
    // - Move to distance[0, 1] (value 1): Remove B
    //
    
    //private static List<PatchOperation> GetLevenshteinOperations( int[,] distance, IList<TNode> source, IList<TNode> target, string path )
    //{
    //    var operations = new List<PatchOperation>();
    //    var i = source.Count;
    //    var j = target.Count;
    //
    //    while ( i > 0 || j > 0 )
    //    {
    //        if ( i > 0 && distance[i, j] == distance[i - 1, j] + 1 )
    //        {
    //            operations.Add( new PatchOperation( PatchOperationType.Remove, $"{path}/{i - 1}", null, null ) );
    //            i--;
    //        }
    //        else if ( j > 0 && distance[i, j] == distance[i, j - 1] + 1 )
    //        {
    //            operations.Add( new PatchOperation( PatchOperationType.Add, $"{path}/{i}", null, target[j - 1] ) );
    //            j--;
    //        }
    //        else if ( i > 0 && j > 0 )
    //        {
    //            operations.Add( new PatchOperation( PatchOperationType.Replace, $"{path}/{i - 1}", null, target[j - 1] ) );
    //            i--;
    //            j--;
    //        }
    //    }
    //
    //    operations.Reverse();
    //    return operations;
    //}

    private static List<PatchOperation> GetLevenshteinOperations( int[,] distance, IList<TNode> source, IList<TNode> target, string path )
    {
        var operations = new List<PatchOperation>();
        var i = source.Count;
        var j = target.Count;

        while ( i > 0 || j > 0 )
        {
            if ( i > 0 && distance[i, j] == distance[i - 1, j] + 1 )
            {
                operations.Add( new PatchOperation( PatchOperationType.Remove, $"{path}/{i - 1}", null, null ) );
                i--;
            }
            else if ( j > 0 && distance[i, j] == distance[i, j - 1] + 1 )
            {
                operations.Add( new PatchOperation( PatchOperationType.Add, $"{path}/{j - 1}", null, target[j - 1] ) );
                j--;
            }
            else if ( i > 0 && j > 0 )
            {
                if ( distance[i, j] == distance[i - 1, j - 1] )
                {
                    i--;
                    j--;
                }
                else
                {
                    operations.Add( new PatchOperation( PatchOperationType.Replace, $"{path}/{i - 1}", null, target[j - 1] ) );
                    i--;
                    j--;
                }
            }
        }

        operations.Reverse();
        return operations;
    }

    private TNode GetParent( TNode source, string parentPath )
    {
        var parentSegment = GetSegments( parentPath );

        if ( !Descriptor.NodeActions.TryGetFromPointer( source, parentSegment, out var parent ) )
            return default;

        return parent;
    }

    private static string GetParentPath( string path )
    {
        var segments = path.Split( '/' );

        return segments.Length <= 1
            ? string.Empty
            : string.Join( '/', segments.Take( segments.Length - 1 ) );
    }

    private static JsonPathSegment GetSegments( string path )
    {
        if ( path == null )
            throw new JsonPatchException( "The 'path' property was missing." );

        var query = JsonPathQueryParser.ParseRfc6901( path, rfc6902: true );
        return query.Segments.Next; // skip the root segment
    }

    // Remove redundant operations within nested objects.
    //
    // If an entire object is replaced, removes individual property operations
    // within that object.
    //
    // Before:
    // Replace /a
    // Replace /a/b
    //
    // After:
    // Replace /a
    //
    private static List<PatchOperation> RemoveRedundantObjectOperations( List<PatchOperation> operations )
    {
        var optimizedOperations = new List<PatchOperation>();
        var pathsToRemove = new HashSet<string>();

        foreach ( var operation in operations )
        {
            if ( operation.Operation != PatchOperationType.Replace || Descriptor.ValueAccessor.GetNodeKind( (TNode) operation.Value ) != NodeKind.Object )
            {
                continue;
            }

            var propertyPath = GetParentPath( operation.Path );
            pathsToRemove.Add( propertyPath );
        }

        foreach ( var operation in operations )
        {
            if ( pathsToRemove.Contains( operation.Path ) )
            {
                continue;
            }

            optimizedOperations.Add( operation );
        }

        return optimizedOperations;
    }

    // Combine consecutive operations targeting the same object or property.
    //
    // Before:
    // Add /a
    // Remove /a
    // Add /b
    // Replace /b
    //
    // After:
    // Replace /b
    //
    private static List<PatchOperation> CombineConsecutiveOperations( List<PatchOperation> operations )
    {
        var combinedOperations = new List<PatchOperation>();
        var operationDict = new Dictionary<string, PatchOperation>();

        foreach ( var operation in operations )
        {
            if ( operationDict.TryAdd( operation.Path, operation ) )
            {
                continue;
            }

            var existingOperation = operationDict[operation.Path];

            if ( existingOperation.Operation == PatchOperationType.Add && operation.Operation == PatchOperationType.Remove )
            {
                // Remove the add operation as it is nullified by the remove operation
                operationDict.Remove( operation.Path );
            }
            else
            {
                // Update the existing operation
                operationDict[operation.Path] = operation;
            }
        }

        combinedOperations.AddRange( operationDict.Values );
        return combinedOperations;
    }
}

