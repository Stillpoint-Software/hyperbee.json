namespace Hyperbee.Json.Query;

[Flags]
public enum SelectorKind
{
    Undefined = 0x0,

    // subtype
    Singular = 0x1,
    Group = 0x2,

    // selectors
    Root = 0x8 | Singular,
    Name = 0x10 | Singular,
    Index = 0x20 | Singular,
    Slice = 0x40 | Group,
    Filter = 0x80 | Group,
    Wildcard = 0x100 | Group,
    Descendant = 0x200 | Group
}
