namespace Hyperbee.Json.Descriptors;

public interface IValueAccessor<TNode>
{
    NodeKind GetNodeKind( in TNode value );

    IEnumerable<(TNode, string)> EnumerateObject( TNode value );
    IEnumerable<TNode> EnumerateArray( TNode value );

    int GetArrayLength( in TNode value );
    bool TryGetIndexAt( in TNode value, int index, out TNode element );
    bool TryGetProperty( in TNode value, string name, out TNode childValue );
    bool TryGetValue( TNode item, out IConvertible value );
}
