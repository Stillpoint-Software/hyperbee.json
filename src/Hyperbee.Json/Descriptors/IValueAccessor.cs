namespace Hyperbee.Json.Descriptors;

public interface IValueAccessor<TNode>
{
    IEnumerable<(TNode, string)> EnumerateChildren( TNode value, bool includeValues = true );
    TNode GetElementAt( in TNode value, int index );
    bool IsObjectOrArray( in TNode current );
    bool IsArray( in TNode current, out int length );
    bool IsObject( in TNode current );
    bool TryGetChildValue( in TNode current, string childKey, out TNode childValue );
}
