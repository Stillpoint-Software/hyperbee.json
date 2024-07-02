namespace Hyperbee.Json.Descriptors;

public interface IValueAccessor<TNode>
{
    IEnumerable<(TNode, string, SelectorKind)> EnumerateChildren( TNode value, bool includeValues = true );
    TNode GetElementAt( in TNode value, int index );
    NodeKind GetNodeKind( in TNode value );
    int GetArrayLength( in TNode value );
    bool TryGetChildValue( in TNode value, string childSelector, out TNode childValue );
    bool TryParseNode( ReadOnlySpan<char> item, out TNode value );
    bool DeepEquals( TNode left, TNode right );
    bool TryGetValueFromNode( TNode item, out object o );
}
