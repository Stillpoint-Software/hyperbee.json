namespace Hyperbee.Json.Filters.Values;

public struct ValueType<T>( T value ) : INodeType where T : IConvertible, IComparable<T>
{
    public readonly NodeTypeKind Kind => NodeTypeKind.Value;

    public INodeTypeComparer Comparer { get; set; }

    public T Value { get; } = value;
}
