namespace Hyperbee.Json.Filters.Parser;

[Flags]
public enum Operator
{
    None = 0x0,

    // Flags
    NonOperator = 0x0001,
    Comparison = 0x0002,
    Logical = 0x0004,
    Math = 0x0008,
    Grouping = 0x0010,

    // Grouping Operators
    OpenParen = 0x0100 | Grouping,
    ClosedParen = 0x0200 | Grouping,

    // Logical Operators
    Not = 0x0300 | Logical,
    Or = 0x0400 | Logical,
    And = 0x0500 | Logical,
    In = 0x0600 | Logical,

    // Comparison Operators
    Equals = 0x0700 | Comparison,
    NotEquals = 0x0800 | Comparison,
    LessThan = 0x0900 | Comparison,
    LessThanOrEqual = 0x0A00 | Comparison,
    GreaterThan = 0x0B00 | Comparison,
    GreaterThanOrEqual = 0x0C00 | Comparison,

    // Math Operators
    Add = 0x0D00 | Math,
    Subtract = 0x0E00 | Math,
    Multiply = 0x0F00 | Math,
    Divide = 0x1000 | Math,
    Modulus = 0x1100 | Math,

    // Specific non-operators
    Whitespace = 0x2000 | NonOperator,
    Quotes = 0x2100 | NonOperator,
    Token = 0x2200 | NonOperator,
    Bracket = 0x2300 | NonOperator,
}

internal static class OperatorExtensions
{
    public static bool IsNonOperator( this Operator op ) => op.HasFlag( Operator.NonOperator );
    public static bool IsComparison( this Operator op ) => op.HasFlag( Operator.Comparison );
    public static bool IsLogical( this Operator op ) => op.HasFlag( Operator.Logical );
    public static bool IsMath( this Operator op ) => op.HasFlag( Operator.Math );
}
