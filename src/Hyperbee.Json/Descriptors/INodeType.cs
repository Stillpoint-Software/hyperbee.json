using System.Collections;

namespace Hyperbee.Json.Descriptors;

public interface INodeType
{
    NodeTypeKind Kind { get; }
}

public readonly struct ValueType<T>( T value, bool isNothing = false ) : INodeType
{
    public NodeTypeKind Kind => NodeTypeKind.Value;
    public T Value { get; } = value;
    public bool IsNothing => isNothing;
}

public readonly struct NodesType<T>( IEnumerable<T> value, bool nonSingular ) : INodeType, IEnumerable<T>
{
    public bool NonSingular => nonSingular;
    public NodeTypeKind Kind => NodeTypeKind.NodeList;
    public IEnumerable<T> Value { get; } = value;
    public IEnumerator<T> GetEnumerator()
    {
        return Value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public readonly struct Nothing : INodeType
{
    public NodeTypeKind Kind => NodeTypeKind.Nothing;
}

public enum NodeTypeKind
{
    Nothing,
    Value,
    Node,
    NodeList
}
