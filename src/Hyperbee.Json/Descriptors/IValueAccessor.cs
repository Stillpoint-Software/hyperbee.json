namespace Hyperbee.Json.Descriptors;

public interface IValueAccessor<TElement>
{
    IEnumerable<(TElement, string)> EnumerateChildValues( TElement value );
    TElement GetElementAt( TElement value, int index );
    bool IsObjectOrArray( TElement current );
    bool IsArray( TElement current, out int length );
    bool IsObject( TElement current );
    bool TryGetChildValue( in TElement current, ReadOnlySpan<char> childKey, out TElement childValue );
}
