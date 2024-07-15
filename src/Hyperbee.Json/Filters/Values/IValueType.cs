using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Filters.Values;

public interface IValueType
{
    public ValueKind ValueKind { get; }
    public IValueTypeComparer Comparer { get; set; }
}

public struct Null : IValueType
{
    public readonly ValueKind ValueKind => ValueKind.Null;
    public IValueTypeComparer Comparer { get; set; }
}

public struct Nothing : IValueType
{
    public readonly ValueKind ValueKind => ValueKind.Nothing;
    public IValueTypeComparer Comparer { get; set; }
}
