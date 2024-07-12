namespace Hyperbee.Json.Descriptors.Types;

public struct ValueType<T>( T value ) : INodeType where T : IConvertible, IComparable<T>
{
    public readonly NodeTypeKind Kind => NodeTypeKind.Value;

    public INodeTypeComparer Comparer { get; set; }

    public T Value { get; } = value;
}

public static class ValueType
{
    public static ValueType<bool> True { get; } = new( true );
    public static ValueType<bool> False { get; } = new( false );

    public static Null Null { get; } = new();
    public static Nothing Nothing { get; } = new();
}
