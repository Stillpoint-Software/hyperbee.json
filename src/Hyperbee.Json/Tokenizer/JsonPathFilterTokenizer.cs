namespace Hyperbee.Json.Tokenizer;
// https://ietf-wg-jsonpath.github.io/draft-ietf-jsonpath-base/draft-ietf-jsonpath-base.html
// https://github.com/ietf-wg-jsonpath/draft-ietf-jsonpath-base

/* BF TODO

internal static class JsonPathFilterTokenizer // STUB for filter parser
{
    private static readonly ConcurrentDictionary<string, IImmutableStack<JsonPathToken>> JsonPathTokens = new();

    private static readonly Regex RegexSlice = new( @"^(-?[0-9]*):?(-?[0-9]*):?(-?[0-9]*)$", RegexOptions.Compiled);
    private static readonly Regex RegexFilter = new( @"^\??\((.*?)\)$", RegexOptions.Compiled);
    private static readonly Regex RegexNumber = new( @"^[0-9*]+$", RegexOptions.Compiled);
    private static readonly Regex RegexQuotedDouble = new( @"^""(?:[^""\\]|\\.)*""$", RegexOptions.Compiled);
    private static readonly Regex RegexQuoted = new( @"^'(?:[^'\\]|\\.)*'$", RegexOptions.Compiled);

    private enum Scanner
    {
        Start,
        DotChild,
        UnionStart,
        UnionChildLiteral,
        UnionChildLiteralFinal,
        UnionChild,
        UnionNext,
        UnionFinal,
        Final
    }

    private enum SelectorKind
    {
        Quoted,
        Slice,
        Filter,
        Index,
        Wildcard,
        Tree,
        Unknown
    }

    private enum OperationKind
    {
        Equals,
        NotEquals,
        LessThan,
        LessThanOrEquals,
        GreaterThan,
        GreaterThanOrEquals,
        Match,
        In,
        NotIn,
        SubsetOf,
        AnyOf,
        NoneOf,
        Size,
        Empty,
        And,
        Or
    }

    private enum ValueKind
    {
        Undefined,
        Root,
        Current,
        Literal
    }

    private record Operation
    {
        public OperationKind OperationKind { get; set; }
        public Identifier Left { get; set; }
        public Identifier Right { get; set; }
    }

    private record Identifier
    {
        public ValueKind ValueKind { get; set; }
        public string Value { get; set; }
    }
}

*/