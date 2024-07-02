namespace Hyperbee.Json.Filters.Parser;

public enum Operator
{
    Nop = 0, // used to represent an unassigned token
    OpenParen,
    ClosedParen,
    Not,
    Equals,
    NotEquals,
    LessThan,
    LessThanOrEqual,
    GreaterThan,
    GreaterThanOrEqual,
    Or,
    And
}
