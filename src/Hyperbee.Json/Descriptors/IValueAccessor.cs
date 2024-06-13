namespace Hyperbee.Json.Descriptors;

public interface IValueAccessor<TElement>
{
    IEnumerable<(TElement, string)> EnumerateChildren( TElement value, bool includeValues = true );
    TElement GetElementAt( in TElement value, int index );
    bool IsObjectOrArray( in TElement current );
    bool IsArray( in TElement current, out int length );
    bool IsObject( in TElement current );
    bool TryGetChildValue( in TElement current, string childKey, out TElement childValue );
}
