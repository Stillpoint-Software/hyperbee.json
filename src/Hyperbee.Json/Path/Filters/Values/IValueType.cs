
namespace Hyperbee.Json.Path.Filters.Values;

public interface IValueType
{
    public ValueKind ValueKind { get; }
}

public readonly struct Null : IValueType
{
    public ValueKind ValueKind => ValueKind.Null;
}

public readonly struct Nothing : IValueType
{
    public ValueKind ValueKind => ValueKind.Nothing;
}
