using System.Collections;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Patch;

// https://datatracker.ietf.org/doc/html/rfc6902/

public class JsonPatch( params PatchOperation[] operations ) : IEnumerable<PatchOperation>
{
    private readonly List<PatchOperation> _operations = [.. operations];

    public void Apply( JsonNode node ) => Apply( node, this );

    public static void Apply( JsonNode node, IEnumerable<PatchOperation> patches )
    {
        var undoOperations = new Stack<PatchOperation>();

        try
        {
            ApplyInternal( node, patches, undoOperations.Push );
        }
        catch
        {
            try
            {
                ApplyInternal( node, undoOperations, null );
            }
            catch
            {
                throw new JsonPatchException( "Failed patch rollback." );
            }

            throw;
        }
    }

    public static void ApplyInternal( JsonNode node, IEnumerable<PatchOperation> patches, Action<PatchOperation> undo )
    {
        foreach ( var patch in patches )
        {
            switch ( patch.Operation )
            {
                case PatchOperationType.Add:
                    AddOperation( node, patch, undo );
                    break;

                case PatchOperationType.Copy:
                    CopyOperation( node, patch, undo );
                    break;

                case PatchOperationType.Move:
                    MoveOperation( node, patch, undo );
                    break;

                case PatchOperationType.Remove:
                    RemoveOperation( node, patch, undo );
                    break;

                case PatchOperationType.Replace:
                    ReplaceOperation( node, patch, undo );
                    break;

                case PatchOperationType.Test:
                    TestOperation( node, patch );
                    break;

                default:
                    throw new JsonPatchException( $"'{patch.Operation}' is an invalid operation." );
            }
        }
    }

    private static void AddOperation( JsonNode node, PatchOperation patch, Action<PatchOperation> undo )
    {
        var segment = GetSegments( patch.Path );

        var target = node.FromJsonPointer( segment, out var name, out var parent );

        ThrowLocationDoesNotExist( patch.Path, parent );

        switch ( parent )
        {
            case JsonObject _ when target != null:
                undo?.Invoke( new PatchOperation( PatchOperationType.Replace, patch.Path, null, target ) );
                target.ReplaceWith( PatchValue( patch ) );
                break;

            case JsonObject jsonObject:
                undo?.Invoke( new PatchOperation( PatchOperationType.Remove, patch.Path, null, null ) );
                jsonObject.Add( name, PatchValue( patch ) );
                break;

            case JsonArray jsonArray:

                if ( name == "-" ) // special segment name for end of array
                {
                    var endPath = string.Concat( patch.Path[..^1], jsonArray.Count );
                    undo?.Invoke( new PatchOperation( PatchOperationType.Remove, endPath, null, null ) );
                    jsonArray.Add( PatchValue( patch ) );
                }
                else if ( int.TryParse( name, out var index ) )
                {
                    if ( index < 0 || index > jsonArray.Count )
                        throw new JsonPatchException( $"The target location '{patch.Path}' was out of range." );

                    undo?.Invoke( new PatchOperation( PatchOperationType.Remove, patch.Path, null, null ) );
                    jsonArray.Insert( index, PatchValue( patch ) );
                }
                else
                {
                    throw new JsonPatchException( $"The target location '{patch.Path}' was an invalid index." );
                }

                break;
        }
    }

    private static void CopyOperation( JsonNode node, PatchOperation patch, Action<PatchOperation> undo )
    {
        if ( patch.From is null )
            throw new JsonPatchException( "The 'from' property was missing." );

        var segment = GetSegments( patch.Path );

        var fromSegment = GetSegments( patch.From );

        ThrowCycleDetected( segment, fromSegment );

        var from = node.FromJsonPointer( fromSegment, out var fromName, out var fromParent );

        ThrowLocationDoesNotExist( patch.From, fromParent );
        ThrowLocationDoesNotExist( patch.From, from );

        _ = node.FromJsonPointer( segment, out var name, out var parent );

        ThrowLocationDoesNotExist( patch.Path, name );

        switch ( fromParent )
        {
            case JsonObject:
                switch ( parent )
                {
                    case JsonObject jsonObject:
                        undo?.Invoke( new PatchOperation( PatchOperationType.Remove, patch.Path, null, null ) );
                        jsonObject.Add( name, from.DeepClone() );
                        break;

                    case JsonArray jsonArray:

                        if ( int.TryParse( name, out var targetIndex ) )
                        {
                            if ( targetIndex < 0 || targetIndex > jsonArray.Count )
                                throw new JsonPatchException( $"The target location '{patch.Path}' was out of range." );

                            undo?.Invoke( new PatchOperation( PatchOperationType.Remove, patch.Path, null, null ) );
                            if ( targetIndex == jsonArray.Count )
                                jsonArray.Add( from.DeepClone() );
                            else
                                jsonArray.Insert( targetIndex, from.DeepClone() );
                        }

                        break;

                    default:
                        throw new JsonPatchException( $"The target location '{patch.Path}' was out of range." );
                }

                break;

            case JsonArray fromParentArray:
                if ( int.TryParse( fromName, out var fromIndex ) )
                {
                    if ( fromIndex < 0 || fromIndex > fromParentArray.Count )
                        throw new JsonPatchException( $"The target location '{patch.From}' was out of range." );

                    switch ( parent )
                    {
                        case JsonObject jsonObject:
                            undo?.Invoke( new PatchOperation( PatchOperationType.Remove, patch.Path, null, null ) );
                            jsonObject.Add( name, from.DeepClone() );
                            break;

                        case JsonArray jsonArray:
                            if ( int.TryParse( name, out var targetIndex ) )
                            {
                                if ( targetIndex < 0 || targetIndex > fromParentArray.Count )
                                    throw new JsonPatchException( $"The target location '{patch.Path}' was out of range." );

                                undo?.Invoke( new PatchOperation( PatchOperationType.Remove, patch.Path, null, null ) );

                                if ( targetIndex == jsonArray.Count )
                                    jsonArray.Add( from.DeepClone() );
                                else
                                    jsonArray.Insert( targetIndex, from.DeepClone() );
                            }

                            break;

                        default:
                            throw new JsonPatchException( $"The target location '{patch.Path}' was out of range." );
                    }
                }
                else
                {
                    throw new JsonPatchException( $"The target location '{patch.Path}' was an invalid index." );
                }

                break;
        }
    }

    private static void MoveOperation( JsonNode node, PatchOperation patch, Action<PatchOperation> undo )
    {
        if ( patch.From is null )
            throw new JsonPatchException( "The 'from' property was missing." );

        var segment = GetSegments( patch.Path );
        var fromSegment = GetSegments( patch.From );

        ThrowCycleDetected( segment, fromSegment );

        var from = node.FromJsonPointer( fromSegment, out var fromName, out var fromParent );

        ThrowLocationDoesNotExist( patch.From, fromParent );
        ThrowLocationDoesNotExist( patch.From, from );

        _ = node.FromJsonPointer( segment, out var moveName, out var parent );

        ThrowLocationDoesNotExist( patch.Path, parent );

        switch ( fromParent )
        {
            case JsonObject fromParentObject:

                switch ( parent )
                {
                    case JsonObject parentObject:
                        fromParentObject.Remove( fromName );
                        parentObject.Add( moveName, from );
                        break;

                    case JsonArray parentArray:
                        if ( int.TryParse( moveName, out var targetIndex ) )
                        {
                            if ( targetIndex < 0 || targetIndex > parentArray.Count )
                                throw new JsonPatchException( $"The target location '{patch.Path}' was out of range." );

                            fromParentObject.Remove( fromName );
                            if ( targetIndex == parentArray.Count )
                                parentArray.Add( from );
                            else
                                parentArray.Insert( targetIndex, from );
                        }

                        break;

                    default:
                        throw new JsonPatchException( $"The target location '{patch.Path}' was out of range." );
                }

                break;

            case JsonArray fromParentArray:
                if ( int.TryParse( fromName, out var fromIndex ) )
                {
                    if ( fromIndex < 0 || fromIndex > fromParentArray.Count )
                        throw new JsonPatchException( $"The target location '{patch.From}' was out of range." );

                    switch ( parent )
                    {
                        case JsonObject parentObject:
                            fromParentArray.RemoveAt( fromIndex );
                            parentObject.Add( moveName, from );
                            break;

                        case JsonArray parentArray:
                            if ( int.TryParse( moveName, out var targetIndex ) )
                            {
                                if ( targetIndex < 0 || targetIndex > fromParentArray.Count )
                                    throw new JsonPatchException( $"The target location '{patch.Path}' was out of range." );

                                fromParentArray.RemoveAt( fromIndex );
                                if ( targetIndex == parentArray.Count )
                                    parentArray.Add( from );
                                else
                                    parentArray.Insert( targetIndex, from );
                            }

                            break;

                        default:
                            throw new JsonPatchException( $"The target location '{patch.Path}' was out of range." );
                    }
                }
                else
                {
                    throw new JsonPatchException( $"The target location '{patch.Path}' was an invalid index." );
                }

                break;
        }

        // invert direction
        undo?.Invoke( new PatchOperation( PatchOperationType.Move, patch.From, patch.Path, null ) );
    }

    private static void RemoveOperation( JsonNode node, PatchOperation patch, Action<PatchOperation> undo )
    {
        var segment = GetSegments( patch.Path );

        var removeTarget = node.FromJsonPointer( segment, out var removeName, out var removeParent );

        ThrowLocationDoesNotExist( patch.Path, removeTarget );

        switch ( removeParent )
        {
            case JsonObject parentObject:
                undo?.Invoke( new PatchOperation( PatchOperationType.Add, patch.Path, null, removeTarget ) );
                parentObject.Remove( removeTarget.GetPropertyName() );
                break;

            case JsonArray parentArray:
                if ( int.TryParse( removeName, out var index ) )
                {
                    if ( index < 0 || index > parentArray.Count )
                        throw new JsonPatchException( $"The target location '{patch.Path}' was out of range." );

                    undo?.Invoke( new PatchOperation( PatchOperationType.Add, patch.Path, null, removeTarget ) );
                    parentArray.RemoveAt( index );
                }
                else
                {
                    throw new JsonPatchException( $"The target location '{patch.Path}' was an invalid index." );
                }

                break;
        }
    }

    private static void ReplaceOperation( JsonNode node, PatchOperation patch, Action<PatchOperation> undo )
    {
        var segment = GetSegments( patch.Path );
        var replaceTarget = node.FromJsonPointer( segment, out _, out var replaceParent );

        ThrowLocationDoesNotExist( patch.Path, replaceParent );
        ThrowLocationDoesNotExist( patch.Path, replaceTarget );

        undo?.Invoke( new PatchOperation( PatchOperationType.Replace, patch.Path, null, replaceTarget.DeepClone() ) );

        replaceTarget.ReplaceWith( PatchValue( patch ) );
    }

    private static void TestOperation( JsonNode node, PatchOperation patch )
    {
        var segment = GetSegments( patch.Path );

        var target = node.FromJsonPointer( segment, out _, out var parent );

        ThrowLocationDoesNotExist( patch.Path, target );
        ThrowLocationDoesNotExist( patch.Path, parent );

        if ( !JsonNode.DeepEquals( target, PatchValue( patch ) ) )
            throw new JsonPatchException( $"The target location's value '{patch.Value}' is not equal the value." );
    }

    private static JsonPathSegment GetSegments( string path )
    {
        if ( path == null )
            throw new JsonPatchException( "The 'path' property was missing." );

        var query = JsonPathQueryParser.ParseRfc6901( path );
        return query.Segments.Next; // skip the root segment
    }

    private static void ThrowCycleDetected( JsonPathSegment segment, JsonPathSegment fromSegment )
    {
        // TODO: compare segments, cannot move to child of self
        if ( segment == fromSegment )
            throw new JsonPatchException( "The 'from' property cannot be a child of the 'path' property." );
    }

    private static void ThrowLocationDoesNotExist( string path, JsonNode node )
    {
        if ( node is null )
            throw new JsonPatchException( $"The target location '{path}' did not exist." );
    }

    private static JsonNode PatchValue( PatchOperation patch )
    {
        if ( patch.Value is null )
            throw new JsonPatchException( "The 'value' property was missing." );

        return patch.Value as JsonNode ?? JsonValue.Create( patch.Value );
    }

    public IEnumerator<PatchOperation> GetEnumerator()
    {
        return _operations.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

}

public static class JsonPathExtensions
{
    public static JsonNode FromJsonPointer( this JsonNode jsonNode, JsonPathSegment segment, out string name, out JsonNode parent )
    {
        if ( !segment.IsNormalized )
            throw new NotSupportedException( "Unsupported JsonPath pointer query format." );

        var current = jsonNode;

        name = default;
        parent = default;

        while ( !segment.IsFinal )
        {
            var (selectorValue, selectorKind) = segment.Selectors[0];

            name = selectorValue;
            parent = current;

            // Handle special segment name for end of array
            var isEndIndex = selectorValue == "-";
            selectorKind = isEndIndex
                ? SelectorKind.Index
                : selectorKind;

            switch ( selectorKind )
            {
                case SelectorKind.Name:
                    {
                        if ( current is null )
                            break;

                        if ( current is not JsonObject jsonObject )
                            throw new JsonPatchException( "The target location was not of the correct type." );

                        jsonObject.TryGetPropertyValue( selectorValue, out var child );

                        current = child;
                        break;
                    }
                case SelectorKind.Index:
                    {
                        if ( current is null )
                            break;

                        if ( current is not JsonArray jsonArray )
                            throw new JsonPatchException( "The target location was not of the correct type." );

                        var length = jsonArray.Count;
                        var index = isEndIndex ? length : int.Parse( selectorValue );

                        if ( index < 0 || index >= length )
                        {
                            current = default;
                            break;
                        }

                        current = jsonArray[index];
                        break;
                    }
                default:
                    throw new NotSupportedException( $"Unsupported {nameof( SelectorKind )}." );
            }

            if ( !segment.Next.IsFinal )
                parent = current;

            segment = segment.Next;
        }

        return current;
    }
}

public class JsonPatchException( string message ) : Exception( message );
