namespace Hyperbee.Json.Filters.Values;

public static class Constants
{
    public static ValueType<bool> True { get; } = new( true );
    public static ValueType<bool> False { get; } = new( false );

    public static Null Null { get; } = new();
    public static Nothing Nothing { get; } = new();
}
