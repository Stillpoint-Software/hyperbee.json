﻿using System.Text.RegularExpressions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

internal static partial class JsonPathFilterTokenizerRegex
{
    [GeneratedRegex( @"([a-z][a-z0-9_]*)\s*\(\s*((?:[^,()]+(?:\s*,\s*)?)*)\s*\)?" )]
    internal static partial Regex RegexFunction();

    [GeneratedRegex( "^[0-9*]+$" )]
    internal static partial Regex RegexNumber();

    [GeneratedRegex( @"^""(?:[^""\\]|\\.)*""$" )]
    internal static partial Regex RegexQuotedDouble();

    [GeneratedRegex( @"^'(?:[^'\\]|\\.)*'$" )]
    internal static partial Regex RegexQuoted();
}