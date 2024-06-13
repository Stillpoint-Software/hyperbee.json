﻿using System.Text.RegularExpressions;

namespace Hyperbee.Json.Filters.Parser;

internal static partial class FilterTokenizerRegex
{
    [GeneratedRegex( @"([a-z][a-z0-9_]*)\s*\(\s*((?:[^,()]+(?:\s*,\s*)?)*)\s*\)?" )]
    internal static partial Regex RegexFunction();

    [GeneratedRegex( @"^""(?:[^""\\]|\\.)*""$", RegexOptions.ExplicitCapture )]
    internal static partial Regex RegexQuotedDouble();

    [GeneratedRegex( @"^'(?:[^'\\]|\\.)*'$", RegexOptions.ExplicitCapture )]
    internal static partial Regex RegexQuoted();
}