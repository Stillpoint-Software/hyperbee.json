namespace Hyperbee.Json.Extensions;

public static class ListExtensions
{
    internal static IEnumerable<T> EnumerateReverse<T>( this IList<T> list )
    {
        for ( var i = list.Count - 1; i >= 0; i-- )
            yield return list[i];
    }
}
