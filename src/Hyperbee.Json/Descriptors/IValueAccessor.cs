namespace Hyperbee.Json.Descriptors;

public enum NodeKind
{
    Object,
    Array,
    Value
}

public interface IValueAccessor<TNode>
{
    IEnumerable<(TNode, string, SelectorKind)> EnumerateChildren( TNode value, bool includeValues = true );
    TNode GetElementAt( in TNode value, int index );
    NodeKind GetNodeKind( in TNode value );
    int GetArrayLength( in TNode value );
    bool TryGetChildValue( in TNode value, string childSelector, out TNode childValue );
    object GetAsValue( IEnumerable<TNode> elements );
    object GetAsValueOther( IEnumerable<TNode> elements );
    bool TryGetObjects( ReadOnlySpan<char> item, out IEnumerable<TNode> elements );
    bool DeepEquals( TNode left, TNode right );
}
