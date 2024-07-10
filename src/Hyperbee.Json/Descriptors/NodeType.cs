using System.Collections;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors;


public interface INodeType : IComparable<INodeType>, IEquatable<INodeType>
{
    public NodeTypeKind Kind { get; }
}

public enum NodeTypeKind
{
    Null,
    Nothing,
    Value,
    Node,
    NodeList
}

public readonly struct ValueType<T>( T value ) : INodeType where T : IConvertible, IComparable<T>
{
    public NodeTypeKind Kind => NodeTypeKind.Value;
    public T Value { get; } = value;

    public static ValueType<TValue> GetValueType<TValue>( TValue value ) where TValue : IConvertible, IComparable<TValue> => new(value);
    public static implicit operator ValueType<T>( T value ) => GetValueType( value );

    public int CompareTo( INodeType other )
    {
        throw new NotImplementedException();
    }

    public bool Equals( INodeType other )
    {
        throw new NotImplementedException();
    }
}

public readonly struct NodesType<TNode>( IEnumerable<TNode> value, bool nonSingular ) : INodeType, IEnumerable<TNode>
{
    public bool NonSingular => nonSingular;
    public NodeTypeKind Kind => NodeTypeKind.NodeList;
    public IEnumerable<TNode> Value { get; } = value;

    public IEnumerator<TNode> GetEnumerator()
    {
        return Value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int CompareTo( INodeType other )
    {
        throw new NotImplementedException();
    }

    public bool Equals( INodeType other )
    {
        throw new NotImplementedException();
    }
}

public readonly struct Nothing : INodeType
{
    public NodeTypeKind Kind => NodeTypeKind.Nothing;
    public int CompareTo( INodeType other )
    {
        return other switch
        {
            Null n => 0,
            ValueType<string> stringValue => stringValue.Value == null ? 0 : -1,
            Nothing or ValueType<bool> or ValueType<float> => -1,
            _ => -1
        };
    }

    public bool Equals( INodeType other ) => other?.Kind == NodeTypeKind.Nothing;
}

public readonly struct Null : INodeType
{
    public NodeTypeKind Kind => NodeTypeKind.Null;
    public int CompareTo( INodeType other )
    {
        return other switch
        {
            Null n => 0,
            ValueType<string> stringValue => stringValue.Value == null ? 0 : -1,
            Nothing or ValueType<bool> or ValueType<float> => -1,
            _ => -1
        };
    }

    public bool Equals( INodeType other ) => other?.Kind == NodeTypeKind.Null;
}

public static class ValueType
{
    public static ValueType<bool> True { get; } = new(true);
    public static ValueType<bool> False { get; } = new(false);

    public static Null Null { get; } = new();
    public static Nothing Nothing { get; } = new();

}

/*
public class NodeTypeComparer<TNode> : EqualityComparer<NodeType>, IComparer<NodeType>
{
    private const float Tolerance = 1e-6F; // Define a tolerance for float comparisons

    public override bool Equals( NodeType x, NodeType y )
    {
        if ( x.Kind != y.Kind )
        {
            return false;
        }

        return x switch
        {
            ValueType<bool> vb when y is ValueType<bool> vb2 => vb.Value == vb2.Value,
            ValueType<string> vs when y is ValueType<string> vs2 => vs.Value == vs2.Value,
            ValueType<float> vf when y is ValueType<float> vf2 => Math.Abs( vf.Value - vf2.Value ) < Tolerance,
            NodesType<TNode> nx when y is NodesType<TNode> ny => nx.SequenceEqual( ny.Value ),
            Nothing _ when y is Nothing => true,
            _ => false
        };
    }

    public override int GetHashCode( NodeType obj )
    {
        return obj.Kind switch
        {
            NodeTypeKind.Value => obj switch
            {
                ValueType<bool> vb => vb.Value.GetHashCode(),
                ValueType<string> vs => vs.Value.GetHashCode(),
                ValueType<float> vf => vf.Value.GetHashCode(),
                _ => 0
            },
            NodeTypeKind.NodeList => obj is NodesType<TNode> n ? n.Value.GetHashCode() : 0,
            NodeTypeKind.Nothing => Int32.MinValue,
            NodeTypeKind.Null => 0,
            _ => obj.GetHashCode()
        };
    }

    public int Compare( NodeType x, NodeType y )
    {
        if ( x.Kind != y.Kind )
        {
            return x.Kind.CompareTo( y.Kind );
        }

        return x switch
        {
            ValueType<bool> vb when y is ValueType<bool> vb2 => vb.Value.CompareTo( vb2.Value ),
            ValueType<string> vs when y is ValueType<string> vs2 => string.Compare( vs.Value, vs2.Value, StringComparison.Ordinal ),
            ValueType<float> vf when y is ValueType<float> vf2 => vf.Value.CompareTo( vf2.Value ),
            NodesType<TNode> nx when y is NodesType<TNode> ny => Comparer<TNode>.Default.Compare( nx.FirstOrDefault(), ny.FirstOrDefault() ),
            Nothing _ when y is Nothing => 0,
            Null _ when y is Null => 0,
            _ => throw new ArgumentException( "Incompatible types for comparison" )
        };
    }
}
*/
