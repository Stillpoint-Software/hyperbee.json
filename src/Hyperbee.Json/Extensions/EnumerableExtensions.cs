namespace Hyperbee.Json.Extensions;

public static class EnumerableExtensions
{
    public static T OneOrDefault<T>( this IEnumerable<T> source )
    {
        ArgumentNullException.ThrowIfNull( source, nameof( source ) );

        using var iterator = source.GetEnumerator();

        if ( !iterator.MoveNext() ) // No elements
            return default;

        var singleItem = iterator.Current;
        return iterator.MoveNext() ? default : singleItem;
    }
}
