using System.Diagnostics;
using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser.Expressions;

public static class ComparerExpressionFactory<TNode>
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly ConstantExpression CreateComparandExpression;

    static ComparerExpressionFactory()
    {
        // Pre-compile the delegate to call the Comparand constructor

        var accessorParam = Expression.Parameter( typeof( IValueAccessor<TNode> ), "accessor" );
        var valueParam = Expression.Parameter( typeof( object ), "value" );

        var constructorInfo = typeof( Comparand ).GetConstructor( [typeof( IValueAccessor<TNode> ), typeof( object )] );
        var newExpression = Expression.New( constructorInfo!, accessorParam, valueParam );

        var creator = Expression.Lambda<Func<IValueAccessor<TNode>, object, Comparand>>(
            newExpression, accessorParam, valueParam ).Compile();

        CreateComparandExpression = Expression.Constant( creator );
    }

    public static Expression GetComparand( IValueAccessor<TNode> accessor, Expression expression )
    {
        // Handles Not operator since it maybe not have a left side.
        if ( expression == null )
            return null;

        // Create an expression representing the instance of the accessor
        var accessorExpression = Expression.Constant( accessor );

        // Use the compiled delegate to create an expression to call the Comparand constructor
        return Expression.Invoke( CreateComparandExpression, accessorExpression,
            Expression.Convert( expression, typeof( object ) ) );
    }

    [DebuggerDisplay( "Value = {Value}" )]
    public readonly struct Comparand( IValueAccessor<TNode> accessor, object value ) : IComparable<Comparand>, IEquatable<Comparand>
    {
        private const float Tolerance = 1e-6F; // Define a tolerance for float comparisons

        private IValueAccessor<TNode> Accessor { get; } = accessor;

        private object Value { get; } = value;

        public int CompareTo( Comparand other ) => Compare( this, other, Operator.Equals );
        public bool Equals( Comparand other ) => Compare( this, other, Operator.Equals ) == 0;
        public override bool Equals( object obj ) => obj is Comparand other && Equals( other );

        public static bool operator ==( Comparand left, Comparand right ) => Compare( left, right, Operator.Equals ) == 0;
        public static bool operator !=( Comparand left, Comparand right ) => Compare( left, right, Operator.NotEquals ) != 0;
        public static bool operator <( Comparand left, Comparand right ) => Compare( left, right, Operator.LessThan ) < 0;
        public static bool operator >( Comparand left, Comparand right ) => Compare( left, right, Operator.GreaterThan ) > 0;
        public static bool operator <=( Comparand left, Comparand right ) => Compare( left, right, Operator.LessThanOrEqual ) <= 0;
        public static bool operator >=( Comparand left, Comparand right ) => Compare( left, right, Operator.GreaterThanOrEqual ) >= 0;

        public override int GetHashCode()
        {
            if ( Value == null )
                return 0;

            var valueHash = Value switch
            {
                IConvertible convertible => convertible.GetHashCode(),
                IEnumerable<TNode> enumerable => enumerable.GetHashCode(),
                _ => Value.GetHashCode()
            };

            return HashCode.Combine( Value.GetType().GetHashCode(), valueHash );
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
        private static int Compare( Comparand left, Comparand right, Operator operation )
        {
            if ( left.Value is IEnumerable<TNode> leftEnumerable && right.Value is IEnumerable<TNode> rightEnumerable )
            {
                return CompareEnumerables( left.Accessor, leftEnumerable, rightEnumerable, out _ );
            }

            if ( left.Value is IEnumerable<TNode> leftEnumerable1 )
            {
                var compare = CompareEnumerableToValue( left.Accessor, leftEnumerable1, right.Value, out var typeMismatch, out var nodeCount );
                return AdjustResult( compare, nodeCount, operation, typeMismatch );
            }

            if ( right.Value is IEnumerable<TNode> rightEnumerable1 )
            {
                var compare = CompareEnumerableToValue( left.Accessor, rightEnumerable1, left.Value, out var typeMismatch, out var nodeCount );
                return AdjustResult( compare, nodeCount, operation, typeMismatch );
            }

            return CompareValues( left.Value, right.Value, out _ );

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

        private static int CompareEnumerables( IValueAccessor<TNode> accessor, IEnumerable<TNode> left, IEnumerable<TNode> right, out bool typeMismatch )
        {
            using var leftEnumerator = left.GetEnumerator();
            using var rightEnumerator = right.GetEnumerator();

            typeMismatch = false;

            while ( leftEnumerator.MoveNext() )
            {
                if ( !rightEnumerator.MoveNext() )
                    return 1; // Left has more elements, so it is greater

                // if the values can be extracted, compare the values directly
                if ( accessor.TryGetValueFromNode( leftEnumerator.Current, out var leftItemValue ) &&
                     accessor.TryGetValueFromNode( rightEnumerator.Current, out var rightItemValue ) )
                    return CompareValues( leftItemValue, rightItemValue, out typeMismatch );

                if ( !accessor.DeepEquals( leftEnumerator.Current, rightEnumerator.Current ) )
                    return -1; // Elements are not deeply equal
            }

            if ( rightEnumerator.MoveNext() )
                return -1; // Right has more elements, so left is less

            return 0; // Sequences are equal
        }

        private static int CompareEnumerableToValue( IValueAccessor<TNode> accessor, IEnumerable<TNode> enumeration, object value, out bool typeMismatch, out int nodeCount )
        {
            nodeCount = 0;
            typeMismatch = false;
            var lastCompare = -1;

            foreach ( var item in enumeration )
            {
                nodeCount++;

                if ( !accessor.TryGetValueFromNode( item, out var itemValue ) )
                    continue; // Skip if value cannot be extracted

                lastCompare = CompareValues( itemValue, value, out typeMismatch );

                if ( lastCompare == 0 )
                    return 0; // Return 0 if any node matches the value
            }

            if ( nodeCount == 0 && value == null ) //BF - when comparing a missing property to null $[?(@.key==null)] we need to fail
                return -1; // Return 0 if the value is null (no nodes to compare to)

            if ( nodeCount == 0 )
                return -1; // Return -1 if the list is empty (no nodes match the value)

            return nodeCount != 1 ? -1 : lastCompare; // Return the last comparison if there is only one node
        }

        private static int CompareValues( object left, object right, out bool typeMismatch )
        {
            typeMismatch = false;

            if ( left == null && right == null )
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

            return Comparer<object>.Default.Compare( left, right );
        }
    }
}
