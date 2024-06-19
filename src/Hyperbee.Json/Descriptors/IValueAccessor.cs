namespace Hyperbee.Json.Descriptors;

public interface IValueAccessor<TNode>
{
    IEnumerable<(TNode, string, SelectorKind)> EnumerateChildren( TNode value, bool includeValues = true );
    TNode GetElementAt( in TNode value, int index );
    bool IsObjectOrArray( in TNode value );
    bool IsArray( in TNode value, out int length );
    bool IsObject( in TNode value );
    bool TryGetChildValue( in TNode value, string childKey, out TNode childValue );
}
