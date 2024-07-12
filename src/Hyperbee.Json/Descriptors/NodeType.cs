using System.Collections;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors;


public interface INodeType : IComparable<INodeType>, IEquatable<INodeType>
{
    public NodeTypeKind Kind { get; }

    public void SetComparer( INodeTypeComparer comparer );

    public object GetValue();
}

public enum NodeTypeKind
{
    Null,
    Nothing,
    Value,
    Node,
    NodeList
}

public struct ValueType<T>( T value ) : INodeType where T : IConvertible, IComparable<T>
{
    private INodeTypeComparer _comparer;

    public readonly NodeTypeKind Kind => NodeTypeKind.Value;

    public void SetComparer( INodeTypeComparer comparer )
    {
        _comparer = comparer;
    }

    public readonly object GetValue() => Value;

    public T Value { get; } = value;

    public static ValueType<TValue> GetValueType<TValue>( TValue value ) where TValue : IConvertible, IComparable<TValue> => new( value );
    public static implicit operator ValueType<T>( T value ) => GetValueType( value );

    public readonly int CompareTo( INodeType other ) => _comparer.Compare( this, other, Operator.Equals );

    public readonly bool Equals( INodeType other ) => _comparer.Compare( this, other, Operator.Equals ) == 0;

    private readonly bool Equals( ValueType<T> other ) => Equals( _comparer, other._comparer ) && EqualityComparer<T>.Default.Equals( Value, other.Value );

    public override readonly bool Equals( object obj ) => obj is ValueType<T> other && Equals( other );

    public override readonly int GetHashCode()
    {
        return HashCode.Combine( _comparer, Value );
    }

    public readonly int Compare( INodeType right, Operator operation ) => _comparer.Compare( this, right, operation );

    public static bool operator ==( ValueType<T> left, ValueType<T> right ) => left.Compare( right, Operator.Equals ) == 0;
    public static bool operator !=( ValueType<T> left, ValueType<T> right ) => left.Compare( right, Operator.NotEquals ) != 0;
    public static bool operator <( ValueType<T> left, ValueType<T> right ) => left.Compare( right, Operator.LessThan ) < 0;
    public static bool operator >( ValueType<T> left, ValueType<T> right ) => left.Compare( right, Operator.GreaterThan ) > 0;
    public static bool operator <=( ValueType<T> left, ValueType<T> right ) => left.Compare( right, Operator.LessThanOrEqual ) <= 0;
    public static bool operator >=( ValueType<T> left, ValueType<T> right ) => left.Compare( right, Operator.GreaterThanOrEqual ) >= 0;
}

public struct NodesType<TNode>( IEnumerable<TNode> value, bool nonSingular ) : INodeType, IEnumerable<TNode>
{
    private INodeTypeComparer _comparer;
    public readonly bool NonSingular => nonSingular;
    public readonly NodeTypeKind Kind => NodeTypeKind.NodeList;

    public void SetComparer( INodeTypeComparer comparer )
    {
        _comparer = comparer;
    }

    public readonly object GetValue() => Value;

    public IEnumerable<TNode> Value { get; } = value;

    public readonly IEnumerator<TNode> GetEnumerator()
    {
        return Value.GetEnumerator();
    }

    readonly IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public readonly int Compare( INodeType right, Operator operation ) => _comparer.Compare( this, right, operation );

    // Direct compare
    public static bool operator ==( NodesType<TNode> left, NodesType<TNode> right ) => left.Compare( right, Operator.Equals ) == 0;
    public static bool operator !=( NodesType<TNode> left, NodesType<TNode> right ) => left.Compare( right, Operator.NotEquals ) != 0;
    public static bool operator <( NodesType<TNode> left, NodesType<TNode> right ) => left.Compare( right, Operator.LessThan ) < 0;
    public static bool operator >( NodesType<TNode> left, NodesType<TNode> right ) => left.Compare( right, Operator.GreaterThan ) > 0;
    public static bool operator <=( NodesType<TNode> left, NodesType<TNode> right ) => left.Compare( right, Operator.LessThanOrEqual ) <= 0;
    public static bool operator >=( NodesType<TNode> left, NodesType<TNode> right ) => left.Compare( right, Operator.GreaterThanOrEqual ) >= 0;

    // Asymmetric compares
    public static bool operator ==( NodesType<TNode> left, INodeType right ) => left.Compare( right, Operator.Equals ) == 0;
    public static bool operator !=( NodesType<TNode> left, INodeType right ) => left.Compare( right, Operator.NotEquals ) != 0;
    public static bool operator <( NodesType<TNode> left, INodeType right ) => left.Compare( right, Operator.LessThan ) < 0;
    public static bool operator >( NodesType<TNode> left, INodeType right ) => left.Compare( right, Operator.GreaterThan ) > 0;
    public static bool operator <=( NodesType<TNode> left, INodeType right ) => left.Compare( right, Operator.LessThanOrEqual ) <= 0;
    public static bool operator >=( NodesType<TNode> left, INodeType right ) => left.Compare( right, Operator.GreaterThanOrEqual ) >= 0;

    public static bool operator ==( INodeType left, NodesType<TNode> right ) => left!.CompareTo( right ) == 0;
    public static bool operator !=( INodeType left, NodesType<TNode> right ) => left!.CompareTo( right ) != 0;
    public static bool operator <( INodeType left, NodesType<TNode> right ) => left.CompareTo( right ) < 0;
    public static bool operator >( INodeType left, NodesType<TNode> right ) => left.CompareTo( right ) > 0;
    public static bool operator <=( INodeType left, NodesType<TNode> right ) => left.CompareTo( right ) <= 0;
    public static bool operator >=( INodeType left, NodesType<TNode> right ) => left.CompareTo( right ) >= 0;

    public readonly int CompareTo( INodeType other ) => Compare( other, Operator.Equals );
    public readonly bool Equals( INodeType other ) => Compare( other, Operator.Equals ) == 0;

    public override bool Equals( object obj )
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}

public readonly struct Nothing : INodeType
{
    public NodeTypeKind Kind => NodeTypeKind.Nothing;
    public void SetComparer( INodeTypeComparer comparer ) { }
    public object GetValue() => this;

    public int CompareTo( INodeType other )
    {
        return other switch
        {
            Null => 0,
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
    public void SetComparer( INodeTypeComparer comparer ) { }
    public object GetValue() => this;

    public int CompareTo( INodeType other )
    {
        return other switch
        {
            Null => 0,
            ValueType<string> stringValue => stringValue.Value == null ? 0 : -1,
            Nothing or ValueType<bool> or ValueType<float> => -1,
            _ => -1
        };
    }

    public bool Equals( INodeType other ) => other?.Kind == NodeTypeKind.Null;
}

public static class ValueType
{
    public static ValueType<bool> True { get; } = new( true );
    public static ValueType<bool> False { get; } = new( false );

    public static Null Null { get; } = new();
    public static Nothing Nothing { get; } = new();

}


public interface INodeTypeComparer
{
    public int Compare( INodeType left, INodeType right, Operator operation );
}

public class NodeTypeComparer<TNode> : EqualityComparer<INodeType>, INodeTypeComparer, IComparer<INodeType>
{
    private readonly IValueAccessor<TNode> _accessor;

    private const float Tolerance = 1e-6F; // Define a tolerance for float comparisons

    public NodeTypeComparer( IValueAccessor<TNode> accessor )
    {
        _accessor = accessor;
    }

    public override bool Equals( INodeType left, INodeType right )
    {
        return Compare( left, right, Operator.Equals ) == 0;
    }

    public override int GetHashCode( INodeType nodeType )
    {
        if ( nodeType.Equals( null ) )
            return 0;

        var valueHash = nodeType switch
        {
            IConvertible convertible => convertible.GetHashCode(),
            IEnumerable<TNode> enumerable => enumerable.GetHashCode(),
            _ => nodeType.GetHashCode()
        };

        return HashCode.Combine( nodeType.GetType().GetHashCode(), valueHash );
    }

    public int Compare( INodeType left, INodeType right )
    {
        return Compare( left, right, Operator.Equals );
    }

    /*
     * Comparison Rules (according to JSONPath RFC 9535):
     *
     * 1. Compare Value to Value:
     *    - Two values are equal if they are of the same type and have the same value.
     *    - For float comparisons, use a tolerance to handle precision issues.
     *    - Comparisons between different types yield false.
     *
     * 2. Compare Node to Node:
     *    - Since a Node is essentially an enumerable with a single item, compare the single items directly.
     *    - Apply the same value comparison rules to the single items.
     *
     * 3. Compare NodeList to NodeList:
     *    - Two NodeLists are equal if they are sequence equal.
     *    - Sequence equality should consider deep equality of Node items.
     *    - Return 0 if sequences are equal.
     *    - Return -1 if the left sequence is less.
     *    - Return 1 if the left sequence is greater.
     *
     * 4. Compare NodeList to Value:
     *    - A NodeList is equal to a value if any node in the NodeList matches the value.
     *    - Return 0 if any node matches the value.
     *    - Return -1 if the value is less than all nodes.
     *    - Return 1 if the value is greater than all nodes.
     *
     * 5. Compare Value to NodeList:
     *    - Similar to the above, true if the value is found in the NodeList.
     *
     * 6. Compare Node to NodeList and vice versa:
     *    - Since Node is a single item enumerable, treat it similarly to Value in comparison to NodeList.
     *
     * 7. Truthiness Rules:
     *    - Falsy values: null, false, 0, "", NaN.
     *    - Truthy values: Anything not falsy, including non-empty strings, non-zero numbers, true, arrays, and objects.
     *    - Truthiness is generally not used for comparison operators (==, <) in filter expressions.
     *    - Type mismatches (e.g., string vs. number) result in false for equality (==) and true for inequality (!=).
     *
     * Order of Operations:
     * - Check if both are NodeLists.
     * - Check if one is a NodeList and the other is a Value.
     * - Compare directly if both are Values.
     */
    public int Compare( INodeType left, INodeType right, Operator operation ) //BF nsq
    {
        if ( left is NodesType<TNode> leftEnumerable && right is NodesType<TNode> rightEnumerable )
        {
            return CompareEnumerables( leftEnumerable, rightEnumerable );
        }

        if ( left is NodesType<TNode> leftEnumerable1 )
        {
            var compare = CompareEnumerableToValue( leftEnumerable1, right.GetValue(), out var typeMismatch, out var nodeCount );
            return AdjustResult( compare, nodeCount, operation, typeMismatch );
        }

        if ( right is NodesType<TNode> rightEnumerable1 )
        {
            var compare = CompareEnumerableToValue( rightEnumerable1, left.GetValue(), out var typeMismatch, out var nodeCount );
            return AdjustResult( compare, nodeCount, operation, typeMismatch );
        }

        return CompareValues( left, right, out _ );

        static int AdjustResult( int compare, int nodeCount, Operator operation, bool typeMismatch )
        {
            // When comparing a NodeList to a Value, '<' and '>' type operators only have meaning when the
            // NodeList has a single node.
            //
            // 1. When there is a single node, the comparison is based on the unwrapped node value. 
            // This results in a meaningful value to value comparison for equality, and greater-than and
            // less-than operations (if the values are the same type).
            //
            // 2. When there is more than one node, or an empty node list, equality is based on finding the
            // value in the set of nodes. The result is true if the value is found in the set, and false
            // otherwise.
            // 
            // In this case, the result is not meaningful for greater-than and less-than operations, since
            // the comparison is based on the set of nodes, and not on two single values.
            //
            // However, the comparison result will still be used in the context of a greater-than or less-than
            // operation, which will yield indeterminate results based on the left or right order of operands.
            // To handle this, we need to normalize the result of the comparison. In this case, we want to
            // normalize the result so that greater-than and less-than always return false, regardless of the
            // left or right order of the comparands.

            return (nodeCount != 1 || typeMismatch) switch // Test for a non-single value set, or a type comparison mismatch
            {
                true when (operation == Operator.LessThan || operation == Operator.LessThanOrEqual) => compare < 0 ? -compare : compare,
                true when (operation == Operator.GreaterThan || operation == Operator.GreaterThanOrEqual) => compare > 0 ? -compare : compare,
                _ => compare
            };
        }
    }

    private int CompareEnumerables( IEnumerable<TNode> left, IEnumerable<TNode> right )
    {
        using var leftEnumerator = left.GetEnumerator();
        using var rightEnumerator = right.GetEnumerator();

        while ( leftEnumerator.MoveNext() )
        {
            if ( !rightEnumerator.MoveNext() )
                return 1; // Left has more elements, so it is greater

            // if the values can be extracted, compare the values directly
            if ( _accessor.TryGetValueFromNode( leftEnumerator.Current, out var leftItemValue ) &&
                 _accessor.TryGetValueFromNode( rightEnumerator.Current, out var rightItemValue ) )
                return CompareValues( leftItemValue, rightItemValue, out _ );

            if ( !_accessor.DeepEquals( leftEnumerator.Current, rightEnumerator.Current ) )
                return -1; // Elements are not deeply equal
        }

        if ( rightEnumerator.MoveNext() )
            return -1; // Right has more elements, so left is less

        return 0; // Sequences are equal
    }

    private int CompareEnumerableToValue( IEnumerable<TNode> enumeration, object value, out bool typeMismatch, out int nodeCount )
    {
        nodeCount = 0;
        typeMismatch = false;
        var lastCompare = -1;

        foreach ( var item in enumeration )
        {
            nodeCount++;

            if ( !_accessor.TryGetValueFromNode( item, out var itemValue ) )
                continue; // Skip if value cannot be extracted

            lastCompare = CompareValues( itemValue, value, out typeMismatch );

            if ( lastCompare == 0 )
                return 0; // Return 0 if any node matches the value
        }

        if ( nodeCount == 0 && value is Nothing ) //BF - when comparing a missing property to null $[?(@.key==null)] we need to fail
            return 0; // Return 0 if the value is null (no nodes to compare to)

        if ( nodeCount == 0 && (value == null || value is Null) ) //BF - when comparing a missing property to null $[?(@.key==null)] we need to fail
            return -1; // Return 0 if the value is null (no nodes to compare to)

        if ( nodeCount == 0 )
            return -1; // Return -1 if the list is empty (no nodes match the value)

        return nodeCount != 1 ? -1 : lastCompare; // Return the last comparison if there is only one node
    }
    private static int CompareValues( object left, object right, out bool typeMismatch )
    {
        typeMismatch = false;

        if ( left is null or Null && right is null or Null )
        {
            return 0;
        }

        if ( left?.GetType() != right?.GetType() )
        {
            typeMismatch = true; // Type mismatch: important for non-equality comparisons
            return -1;
        }

        if ( left is string leftString && right is string rightString )
        {
            return string.Compare( leftString, rightString, StringComparison.Ordinal );
        }

        if ( left is bool leftBool && right is bool rightBool )
        {
            return leftBool.CompareTo( rightBool );
        }

        if ( left is float leftFloat && right is float rightFloat )
        {
            return Math.Abs( leftFloat - rightFloat ) < Tolerance ? 0 : leftFloat.CompareTo( rightFloat );
        }

        if ( left is INodeType leftValue && right is INodeType rightValue )
        {
            return CompareValues( leftValue.GetValue(), rightValue.GetValue(), out typeMismatch );
        }

        return Comparer<object>.Default.Compare( left, right );

    }

}

