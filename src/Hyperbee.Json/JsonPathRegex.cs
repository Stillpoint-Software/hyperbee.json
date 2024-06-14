using System.Text.RegularExpressions;

namespace Hyperbee.Json;

internal partial class JsonPathRegex
{
    // generated regex

    [GeneratedRegex( "^(-?[0-9]*):?(-?[0-9]*):?(-?[0-9]*)$" )]
    internal static partial Regex RegexSlice();

    [GeneratedRegex( @"^\?\(?(.*?)\)?$" )]
    internal static partial Regex RegexPathFilter();

    [GeneratedRegex( @"^[0-9*]+$" )]
    internal static partial Regex RegexNumber();
}
