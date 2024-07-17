namespace Hyperbee.Json.Filters.Parser;

[Flags]
public enum Operator
{
    None = 0x0,

    // Flags
    NonOperator = 0x1,  // 0001
    Comparison = 0x2,   // 0010
    Logical = 0x4,      // 0100
    Parenthesis = 0x8,  // 1000

    // Parenthesis Operators
    OpenParen = 0x10 | Parenthesis,
    ClosedParen = 0x20 | Parenthesis,

    // Logical Operators
    Not = 0x30 | Logical,
    Or = 0x40 | Logical,
    And = 0x50 | Logical,

    // Comparison Operators
    Equals = 0x60 | Comparison,
    NotEquals = 0x70 | Comparison,
    LessThan = 0x80 | Comparison,
    LessThanOrEqual = 0x90 | Comparison,
    GreaterThan = 0xA0 | Comparison,
    GreaterThanOrEqual = 0xB0 | Comparison,

    // Specific non-operators
    Whitespace = 0xC0 | NonOperator,
    Quotes = 0xD0 | NonOperator,
    Token = 0xE0 | NonOperator,
    Bracket = 0xF0 | NonOperator,
}

internal static class OperatorExtensions
{
    public static bool IsNonOperator( this Operator op )
    {
        return (op & Operator.NonOperator) == Operator.NonOperator;
    }

    public static bool IsComparison( this Operator op )
    {
        return (op & Operator.Comparison) == Operator.Comparison;
    }

    public static bool IsLogical( this Operator op )
    {
        return (op & Operator.Logical) == Operator.Logical;
    }
}
