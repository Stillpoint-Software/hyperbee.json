namespace Hyperbee.Json.Internal;

/// <summary>
/// String builders over spans that netstandard2.1 does not carry on <see cref="string"/>.
/// </summary>
/// <remarks>
/// <c>string.Concat</c> gained its span overloads in .NET Core 3.0 but they are not part
/// of the netstandard2.1 contract. On net10.0 and net11.0 this forwards to the framework
/// overload, which writes straight into the new string; under NETSTANDARD it does the
/// same thing by hand with one <c>string.Create</c>, so neither path builds an
/// intermediate string.
/// </remarks>
internal static class StringEx
{
    public static string Concat( ReadOnlySpan<char> first, ReadOnlySpan<char> second, ReadOnlySpan<char> third )
    {
#if NETSTANDARD
        var length = first.Length + second.Length + third.Length;

        if ( length == 0 )
            return string.Empty;

        var buffer = new char[length];
        var target = buffer.AsSpan();

        first.CopyTo( target );
        second.CopyTo( target[first.Length..] );
        third.CopyTo( target[(first.Length + second.Length)..] );

        return new string( buffer );
#else
        return string.Concat( first, second, third );
#endif
    }
}
