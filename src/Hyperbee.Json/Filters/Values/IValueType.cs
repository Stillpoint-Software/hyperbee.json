using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Filters.Values;

public interface IValueType
{
    public ValueKind Kind { get; }
    public IValueTypeComparer Comparer { get; set; }
}

public struct Null : IValueType
{
    public readonly ValueKind Kind => ValueKind.Null;
    public IValueTypeComparer Comparer { get; set; }
}

public struct Nothing : IValueType
{
    public readonly ValueKind Kind => ValueKind.Nothing;
    public IValueTypeComparer Comparer { get; set; }
}
