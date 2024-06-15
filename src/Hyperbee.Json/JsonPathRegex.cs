using System.Text.RegularExpressions;

namespace Hyperbee.Json;

internal partial class JsonPathRegex
{
    // generated regex

    [GeneratedRegex( @"^(-?\d*):?(-?\d*):?(-?\d*)$" )]
    internal static partial Regex RegexSlice();

    [GeneratedRegex( @"^\?\(?(.*)\)?$" )]
    internal static partial Regex RegexPathFilter();

    [GeneratedRegex( @"^[\d*]+$" )]
    internal static partial Regex RegexNumber();

    [GeneratedRegex( @"^""?:[^""\\]|\\.*""$" )]
    internal static partial Regex RegexQuotedDouble();

    [GeneratedRegex( @"^'?:[^'\\]|\\.*'$" )]
    internal static partial Regex RegexQuoted();
}
