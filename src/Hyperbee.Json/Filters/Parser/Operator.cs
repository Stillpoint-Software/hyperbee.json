namespace Hyperbee.Json.Filters.Parser;

[Flags]
public enum Operator
{
    None = 0x0,

    // Flags
    NonOperator = 0x1,
    Comparison = 0x2,
    Logical = 0x4,
    Math = 0x8,
    Grouping = 0x10,

    // Grouping Operators
    OpenParen = 0x100 | Grouping,
    ClosedParen = 0x200 | Grouping,

    // Logical Operators
    Not = 0x300 | Logical,
    Or = 0x400 | Logical,
    And = 0x500 | Logical,
    In = 0x600 | Logical,

    // Comparison Operators
    Equals = 0x700 | Comparison,
    NotEquals = 0x800 | Comparison,
    LessThan = 0x900 | Comparison,
    LessThanOrEqual = 0xA00 | Comparison,
    GreaterThan = 0xB00 | Comparison,
    GreaterThanOrEqual = 0xC00 | Comparison,

    // Math Operators
    Add = 0xD00 | Math,
    Subtract = 0xE00 | Math,
    Multiply = 0xF00 | Math,
    Divide = 0x1000 | Math,

    // Specific non-operators
    Whitespace = 0x1100 | NonOperator,
    Quotes = 0x1200 | NonOperator,
    Token = 0x1300 | NonOperator,
    Bracket = 0x1400 | NonOperator,
}

internal static class OperatorExtensions
{
    public static bool IsNonOperator( this Operator op ) => op.HasFlag( Operator.NonOperator );
    public static bool IsComparison( this Operator op ) => op.HasFlag( Operator.Comparison );
    public static bool IsLogical( this Operator op ) => op.HasFlag( Operator.Logical );
    public static bool IsMath( this Operator op ) => op.HasFlag( Operator.Math );
}
