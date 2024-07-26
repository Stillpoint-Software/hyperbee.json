namespace Hyperbee.Json.Descriptors;

public interface IValueAccessor<TNode>
{
    NodeKind GetNodeKind( in TNode value );

    IEnumerable<(TNode, string)> EnumerateObject( TNode value );
    IEnumerable<(TNode, int)> EnumerateArray( TNode value );

    int GetArrayLength( in TNode value );
    bool TryGetElementAt( in TNode value, int index, out TNode element );
    bool TryGetChild( in TNode value, string name, out TNode childValue );
    bool TryGetValue( TNode item, out IConvertible value );

    bool DeepEquals( TNode left, TNode right );

    bool TryGetFromPointer( in TNode value, JsonPathSegment segment, out TNode childValue );
}
