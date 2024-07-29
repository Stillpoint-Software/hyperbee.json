using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Patch;

public interface IPatchOptimizer<in TNode>
{
    List<PatchOperation> OptimizePatch( TNode source, List<PatchOperation> operations );
}

public class PatchOptimizer<TNode> : IPatchOptimizer<TNode>
{
    private static readonly ITypeDescriptor<TNode> Descriptor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>();

    public List<PatchOperation> OptimizePatch( TNode source, List<PatchOperation> operations )
    {
        var optimizedOperations = RemoveNoOpOperations( source, operations );
        optimizedOperations = BatchArrayOperations( optimizedOperations );
        optimizedOperations = MergeSequentialOperations( optimizedOperations );

        return optimizedOperations;
    }

    // Removes no-op operations, such as adding a value that already exists or
    // removing a value that is not present.
    //
    // Before:
    // Add /a 1 (if 1 is already at /a)
    // Remove /b (if /b does not exist)
    //
    // After:
    // (no operation)
    //
    private List<PatchOperation> RemoveNoOpOperations( TNode source, List<PatchOperation> operations )
    {
        var optimizedOperations = new List<PatchOperation>();

        for ( var index = 0; index < operations.Count; index++ )
        {
            var operation = operations[index];

            if ( !IsNoOp( source, operation ) )
                optimizedOperations.Add( operation );
        }

        return optimizedOperations;
    }

    // Example placeholder method for IsNoOp
    private bool IsNoOp( TNode source, PatchOperation operation )
    {
        switch ( operation.Operation )
        {
            // Check for Add operation that adds a value which is already present
            case PatchOperationType.Add:
                {
                    var existingValue = GetNodeByPath( source, operation.Path );

                    if ( existingValue != null && Descriptor.NodeActions.DeepEquals( existingValue, (TNode) operation.Value ) )
                        return true;
                    break;
                }
            // Check for Remove operation that removes a value which is not present
            case PatchOperationType.Remove:
                {
                    var existingValue = GetNodeByPath( source, operation.Path );

                    if ( existingValue == null )
                        return true;
                    break;
                }
            // Check for Replace operation that replaces a value with the same value
            case PatchOperationType.Replace:
                {
                    var existingValue = GetNodeByPath( source, operation.Path );

                    if ( existingValue != null && Descriptor.NodeActions.DeepEquals( existingValue, (TNode) operation.Value ) )
                        return true;
                    break;
                }
        }

        return false;
    }

    // Example method to get the node by its JSON pointer path
    private TNode GetNodeByPath( TNode source, string path )
    {
        var accessor = Descriptor.ValueAccessor;

        var segments = path.Trim( '/' ).Split( '/' );
        TNode currentNode = source;

        foreach ( var segment in segments )
        {
            var nodeKind = Descriptor.ValueAccessor.GetNodeKind( currentNode );
            switch ( nodeKind )
            {
                case NodeKind.Object:
                    {
                        if ( !accessor.TryGetProperty( currentNode, segment, out var nextNode ) )
                            return default;

                        currentNode = nextNode;
                        break;
                    }
                case NodeKind.Array:
                    {
                        var length = accessor.GetArrayLength( currentNode );

                        if ( !int.TryParse( segment, out var index ) || index < 0 || index >= length )
                            return default;

                        currentNode = accessor.IndexAt( currentNode, index );
                        break;
                    }
                default:
                    return default;
            }
        }

        return currentNode;
    }

    // Batch similar operations on the same array to reduce overhead.
    //
    // Before:
    // Add /a/0 1
    // Add /a/1 2
    // Add /a/2 3
    //
    // After:
    // BatchAdd /a [1, 2, 3]
    //
    private List<PatchOperation> BatchArrayOperations( List<PatchOperation> operations )
    {
        var optimizedOperations = new List<PatchOperation>();
        var arrayBatches = new Dictionary<string, List<object>>();

        foreach ( var operation in operations )
        {
            if ( operation.Operation == PatchOperationType.Add && operation.Path.EndsWith( "/-" ) )
            {
                var path = operation.Path[..^2];

                if ( !arrayBatches.TryGetValue( path, out var value ) )
                {
                    value = ([]);
                    arrayBatches[path] = value;
                }

                value.Add( operation.Value );
            }
            else
            {
                optimizedOperations.Add( operation );
            }
        }

        foreach ( var batch in arrayBatches )
        {
            optimizedOperations.Add( new PatchOperation
            {
                Operation = PatchOperationType.Add,
                Path = $"{batch.Key}/-",
                Value = batch.Value
            } );
        }

        return optimizedOperations;
    }

    // Merge sequential operations on the same property or array index.
    //
    // Before:
    // Replace /a 1
    // Replace /a 2
    //
    // After:
    // Replace /a 2
    //
    private List<PatchOperation> MergeSequentialOperations( List<PatchOperation> operations )
    {
        var optimizedOperations = new List<PatchOperation>();
        var lastOperationMap = new Dictionary<string, PatchOperation>();

        foreach ( var operation in operations )
        {
            if ( lastOperationMap.TryAdd( operation.Path, operation ) )
            {
                continue;
            }

            var lastOperation = lastOperationMap[operation.Path];

            if ( lastOperation.Operation == PatchOperationType.Replace && operation.Operation == PatchOperationType.Replace )
            {
                // Merge the two replace operations
                lastOperationMap[operation.Path] = operation;
            }
            else
            {
                optimizedOperations.Add( lastOperation );
                lastOperationMap[operation.Path] = operation;
            }
        }

        optimizedOperations.AddRange( lastOperationMap.Values );
        return optimizedOperations;
    }
}
