namespace Hyperbee.Json.Filters.Parser;

[Flags]
public enum CompareConstraint
{
    None = 0x00,
    MustCompare = 0x01,
    MustNotCompare = 0x02,
    ExpectNormalized = 0x10,

    // Scope

    Function = 0x100,
    Literal = 0x200
}
