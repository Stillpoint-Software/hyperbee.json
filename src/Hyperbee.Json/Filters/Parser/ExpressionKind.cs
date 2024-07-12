namespace Hyperbee.Json.Filters.Parser;

internal enum ExpressionKind
{
    Unspecified,
    Function,
    Json,
    Literal,
    Not,
    Paren,
    Select,
    Merged
}
