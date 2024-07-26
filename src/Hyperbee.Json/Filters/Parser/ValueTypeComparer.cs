using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser;

public interface IValueTypeComparer
{
    public int Compare( IValueType left, IValueType right, Operator operation );

    public bool Exists( IValueType node );

    public bool In( IValueType left, IValueType right );
}

public class ValueTypeComparer<TNode> : IValueTypeComparer
{
    private const float Tolerance = 1e-6F; // Define a tolerance for float comparisons

    private static readonly ITypeDescriptor<TNode> Descriptor =
        JsonTypeDescriptorRegistry.GetDescriptor<TNode>();

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
    public int Compare( IValueType left, IValueType right, Operator operation )
    {
        ThrowIfNotNormalized( left );
        ThrowIfNotNormalized( right );

        if ( left is NodeList<TNode> leftEnumerable && right is NodeList<TNode> rightEnumerable )
        {
            return CompareEnumerables( leftEnumerable, rightEnumerable );
        }

        if ( left is NodeList<TNode> leftEnumerable1 )
        {
            var compare = CompareEnumerableToValue( leftEnumerable1, right, out var typeMismatch, out var nodeCount );
            return AdjustResult( compare, nodeCount, operation, typeMismatch );
        }

        if ( right is NodeList<TNode> rightEnumerable1 )
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
                true when operation == Operator.LessThan || operation == Operator.LessThanOrEqual => compare < 0 ? -compare : compare,
                true when operation == Operator.GreaterThan || operation == Operator.GreaterThanOrEqual => compare > 0 ? -compare : compare,
                _ => compare
            };
        }

        static void ThrowIfNotNormalized( IValueType nodeType )
        {
            if ( nodeType is NodeList<TNode> { IsNormalized: false } )
                throw new NotSupportedException( "Unsupported non-single query." );
        }
    }

    public bool In( IValueType left, IValueType right )
    {
        if ( right is not NodeList<TNode> rightList )
            throw new NotSupportedException( "The right side of `in` must be a node list." );

        var rightNode = rightList.OneOrDefault();
        var accessor = Descriptor.ValueAccessor;

        if ( rightNode == null || accessor.GetNodeKind( rightNode ) != NodeKind.Array )
            return false;

        return Contains( this, accessor, left, rightNode );

        static IEnumerable<TNode> EnumerateChildren( IValueAccessor<TNode> accessor, TNode node )
        {
            return accessor.GetNodeKind( node ) switch
            {
                NodeKind.Array => accessor.EnumerateArray( node ),
                NodeKind.Object => accessor.EnumerateObject( node ).Select( x => x.Item1 ),
                _ => []
            };
        }

        static bool Contains( IValueTypeComparer comparer, IValueAccessor<TNode> accessor, IValueType left, TNode rightNode )
        {
            return EnumerateChildren( accessor, rightNode )
                .Select( rightChild => GetComparand( accessor, rightChild ) )
                .Select( comparand => comparer.Compare( left, comparand, Operator.Equals ) )
                .Any( result => result == 0 );
        }

        static IValueType GetComparand( IValueAccessor<TNode> accessor, TNode childValue )
        {
            return accessor.GetNodeKind( childValue ) switch
            {
                NodeKind.Value => TryGetValue( accessor, childValue, out var comparand ) ? comparand
                    : throw new NotSupportedException( "Unsupported value type." ),

                _ => new NodeList<TNode>( [childValue], true )
            };
        }
    }

    public bool Exists( IValueType node )
    {
        return node switch
        {
            ScalarValue<bool> boolValue => boolValue.Value,
            ScalarValue<int> intValue => intValue.Value != 0,
            ScalarValue<float> floatValue => floatValue.Value != 0,
            ScalarValue<string> stringValue => !string.IsNullOrEmpty( stringValue.Value ),
            NodeList<TNode> nodes => nodes.Any(),
            _ => false
        };
    }

    private static int CompareEnumerables( IEnumerable<TNode> left, IEnumerable<TNode> right )
    {
        using var leftEnumerator = left.GetEnumerator();
        using var rightEnumerator = right.GetEnumerator();

        var (valueAccessor, nodeAccessor) = Descriptor;

        while ( leftEnumerator.MoveNext() )
        {
            if ( !rightEnumerator.MoveNext() )
                return 1; // Left has more elements, so it is greater

            // if the values can be extracted, compare the values directly
            if ( TryGetValue( valueAccessor, leftEnumerator.Current, out var leftItemValue ) &&
                 TryGetValue( valueAccessor, rightEnumerator.Current, out var rightItemValue ) )
                return CompareValues( leftItemValue, rightItemValue, out _ );

            if ( !nodeAccessor.DeepEquals( leftEnumerator.Current, rightEnumerator.Current ) )
                return -1; // Elements are not deeply equal
        }

        if ( rightEnumerator.MoveNext() )
            return -1; // Right has more elements, so left is less

        return 0; // Sequences are equal
    }

    private static int CompareEnumerableToValue( IEnumerable<TNode> enumeration, IValueType value, out bool typeMismatch, out int nodeCount )
    {
        nodeCount = 0;
        typeMismatch = false;
        var lastCompare = -1;

        var accessor = Descriptor.ValueAccessor;

        foreach ( var item in enumeration )
        {
            nodeCount++;

            if ( !TryGetValue( accessor, item, out var itemValue ) )
                continue; // Skip if value cannot be extracted

            lastCompare = CompareValues( itemValue, value, out typeMismatch );

            if ( lastCompare == 0 )
                return 0; // Return 0 if any node matches the value
        }

        if ( nodeCount == 0 )
        {
            if ( value.ValueKind == ValueKind.Nothing ) // Considered equal
                return 0;

            return -1;
        }

        return nodeCount != 1 ? -1 : lastCompare; // Return the last comparison if there is only one node
    }

    private static int CompareValues( IValueType left, IValueType right, out bool typeMismatch )
    {
        typeMismatch = false;

        if ( IsNullOrNothing( left ) && IsNullOrNothing( right ) )
        {
            return 0;
        }

        if ( IsTypeMismatch( left, right ) && !IsFloatToIntOperation( left, right ) )
        {
            typeMismatch = true; // Type mismatch: important for non-equality comparisons
            return -1;
        }

        return left switch
        {
            ScalarValue<string> leftStringValue when right is ScalarValue<string> rightStringValue =>
                string.Compare( leftStringValue.Value, rightStringValue.Value, StringComparison.Ordinal ),

            ScalarValue<bool> leftBoolValue when right is ScalarValue<bool> rightBoolValue =>
                leftBoolValue.Value.CompareTo( rightBoolValue.Value ),

            ScalarValue<float> leftFloatValue when right is ScalarValue<float> rightFloatValue =>
                Math.Abs( leftFloatValue.Value - rightFloatValue.Value ) < Tolerance ? 0 : leftFloatValue.Value.CompareTo( rightFloatValue.Value ),

            ScalarValue<int> leftIntValue when right is ScalarValue<int> rightIntValue =>
                leftIntValue.Value.CompareTo( rightIntValue.Value ),

            ScalarValue<int> leftIntValue when right is ScalarValue<float> rightFloatValue =>
                Math.Abs( leftIntValue.Value - rightFloatValue.Value ) < Tolerance ? 0 : rightFloatValue.Value.CompareTo( leftIntValue.Value ),

            ScalarValue<float> leftFloatValue when right is ScalarValue<int> rightIntValue =>
                Math.Abs( leftFloatValue.Value - rightIntValue.Value ) < Tolerance ? 0 : leftFloatValue.Value.CompareTo( rightIntValue.Value ),

            _ => Comparer<object>.Default.Compare( left, right )
        };

        // Helpers
        static bool IsTypeMismatch( IValueType left, IValueType right ) => left?.GetType() != right?.GetType();
        static bool IsNullOrNothing( IValueType value ) => value.ValueKind == ValueKind.Null || value.ValueKind == ValueKind.Nothing;

        static bool IsFloatToIntOperation( IValueType left, IValueType right ) =>
            left is ScalarValue<int> && right is ScalarValue<float> || left is ScalarValue<float> && right is ScalarValue<int>;
    }

    private static bool TryGetValue( IValueAccessor<TNode> accessor, TNode node, out IValueType nodeType )
    {
        if ( accessor.TryGetValue( node, out var itemValue ) )
        {
            nodeType = itemValue switch
            {
                string itemString => Scalar.Value( itemString ),
                bool itemBool => Scalar.Value( itemBool ),
                float itemFloat => Scalar.Value( itemFloat ),
                int itemInt => Scalar.Value( itemInt ),
                null => Scalar.Null,
                _ => throw new NotSupportedException( "Unsupported value type." )
            };
            return true;
        }

        nodeType = null;
        return false;
    }
}
