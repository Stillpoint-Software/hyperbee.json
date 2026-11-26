namespace Hyperbee.Json.Shared;

/// <summary>
/// Character classifiers that netstandard2.1 does not carry on <see cref="char"/>.
/// </summary>
/// <remarks>
/// Declared under its own type name rather than as extensions on <see cref="char"/>,
/// because the framework members are statics on the type and C# cannot add statics to an
/// existing type. On net10.0 and net11.0 these forward to the framework implementation.
/// </remarks>
internal static class CharEx
{
#if NETSTANDARD
    /// <summary>
    /// True when <paramref name="value"/> is 0-9, a-f or A-F.
    /// </summary>
    public static bool IsAsciiHexDigit( char value ) =>
        value is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F';
#else
    /// <inheritdoc cref="char.IsAsciiHexDigit(char)" />
    public static bool IsAsciiHexDigit( char value ) => char.IsAsciiHexDigit( value );
#endif
}
