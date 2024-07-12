using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Types;

public interface INodeTypeComparer
{
    public int Compare( INodeType left, INodeType right, Operator operation );
}

public class NodeTypeComparer<TNode>( IValueAccessor<TNode> accessor ) : INodeTypeComparer
{
    private const float Tolerance = 1e-6F; // Define a tolerance for float comparisons

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
    public int Compare( INodeType left, INodeType right, Operator operation )
    {
        if ( left is NodesType<TNode> leftEnumerable && right is NodesType<TNode> rightEnumerable )
        {
            return CompareEnumerables( leftEnumerable, rightEnumerable );
        }

        if ( left is NodesType<TNode> leftEnumerable1 )
        {
            var compare = CompareEnumerableToValue( leftEnumerable1, right, out var typeMismatch, out var nodeCount );
            return AdjustResult( compare, nodeCount, operation, typeMismatch );
        }

        if ( right is NodesType<TNode> rightEnumerable1 )
        {
            var compare = CompareEnumerableToValue( rightEnumerable1, left, out var typeMismatch, out var nodeCount );
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
            if ( TryGetValueType( accessor, leftEnumerator.Current, out var leftItemValue ) &&
                 TryGetValueType( accessor, rightEnumerator.Current, out var rightItemValue ) )
                return CompareValues( leftItemValue, rightItemValue, out _ );

            if ( !accessor.DeepEquals( leftEnumerator.Current, rightEnumerator.Current ) )
                return -1; // Elements are not deeply equal
        }

        if ( rightEnumerator.MoveNext() )
            return -1; // Right has more elements, so left is less

        return 0; // Sequences are equal
    }

    private int CompareEnumerableToValue( IEnumerable<TNode> enumeration, INodeType value, out bool typeMismatch, out int nodeCount )
    {
        nodeCount = 0;
        typeMismatch = false;
        var lastCompare = -1;

        foreach ( var item in enumeration )
        {
            nodeCount++;

            if ( !TryGetValueType( accessor, item, out var itemValue ) )
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

    private static int CompareValues( INodeType left, INodeType right, out bool typeMismatch )
    {
        typeMismatch = false;

        if ( left is null or Null or Nothing && right is null or Null or Nothing )
        {
            return 0;
        }

        if ( left?.GetType() != right?.GetType() )
        {
            typeMismatch = true; // Type mismatch: important for non-equality comparisons
            return -1;
        }

        if ( left is ValueType<string> leftStringValue && right is ValueType<string> rightStringValue )
        {
            return string.Compare( leftStringValue.Value, rightStringValue.Value, StringComparison.Ordinal );
        }

        if ( left is ValueType<bool> leftBoolValue && right is ValueType<bool> rightBoolValue )
        {
            return leftBoolValue.Value.CompareTo( rightBoolValue.Value );
        }

        if ( left is ValueType<float> leftFloatValue && right is ValueType<float> rightfloatValue )
        {
            return Math.Abs( leftFloatValue.Value - rightfloatValue.Value ) < Tolerance ? 0 : leftFloatValue.Value.CompareTo( rightfloatValue.Value );
        }

        return Comparer<object>.Default.Compare( left, right );

    }

    private static bool TryGetValueType( IValueAccessor<TNode> accessor, TNode node, out INodeType nodeType )
    {
        if ( accessor.TryGetValueFromNode( node, out var itemValue ) )
        {
            nodeType = itemValue switch
            {
                string itemString => new ValueType<string>( itemString ),
                bool itemBool => new ValueType<bool>( itemBool ),
                float itemFloat => new ValueType<float>( itemFloat ),
                null => ValueType.Null,
                _ => throw new NotSupportedException( "Unsupported value type." )
            };
            return true;
        }

        nodeType = null;
        return false;
    }
}
