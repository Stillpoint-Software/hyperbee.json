namespace Hyperbee.Json.Descriptors;

public interface IValueAccessor<TNode>
{
    NodeKind GetNodeKind( in TNode value );

    IEnumerable<(TNode, string)> EnumerateObject( in TNode value, bool excludeValues = false );
    IEnumerable<TNode> EnumerateArray( in TNode value, bool excludeValues = false );

    int GetArrayLength( in TNode value );
    TNode IndexAt( in TNode value, int index );
    bool TryGetIndexAt( in TNode value, int index, out TNode item );
    bool TryGetProperty( in TNode value, string propertyName, out TNode propertyValue );
    bool TryGetValue( TNode node, out IConvertible value );
}
