using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Filters.Parser;

[Flags]
public enum Operator
{
    None = 0x0,

    // Base non-operator token
    NonOperator = 0x1,

    OpenParen = 0x2,
    ClosedParen = 0x4,
    Not = 0x6,
    Equals = 0x8,
    NotEquals = 0xA,
    LessThan = 0xC,
    LessThanOrEqual = 0xE,
    GreaterThan = 0x10,
    GreaterThanOrEqual = 0x12,
    Or = 0x14,
    And = 0x16,

    // Specific non-operator tokens
    Whitespace = 0x18 | NonOperator,
    Quotes = 0x1A | NonOperator,
    Segment = 0x1C | NonOperator,

    EndOfBuffer = 0x1E | NonOperator,
}

public static class OperatorExtensions
{
    public static bool IsNonOperator( this Operator op )
    {
        return (op & Operator.NonOperator) == Operator.NonOperator;
    }
}
