using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.Json;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser.Expressions;

public static class JsonComparerExpressionFactory<TNode>
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly ConstantExpression CreateComparandExpression;

    static JsonComparerExpressionFactory()
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

        public int CompareTo( Comparand other ) => Compare( this, other );
        public bool Equals( Comparand other ) => Compare( this, other ) == 0;
        public override bool Equals( object obj ) => obj is Comparand other && Equals( other );

        public static bool operator ==( Comparand left, Comparand right ) => Compare( left, right ) == 0;
        public static bool operator !=( Comparand left, Comparand right ) => Compare( left, right ) != 0;
        public static bool operator <( Comparand left, Comparand right ) => Compare( left, right, lessThan: true ) < 0;
        public static bool operator >( Comparand left, Comparand right ) => Compare( left, right ) > 0;
        public static bool operator <=( Comparand left, Comparand right ) => Compare( left, right, lessThan: true ) <= 0;
        public static bool operator >=( Comparand left, Comparand right ) => Compare( left, right ) >= 0;

        public override int GetHashCode()
        {
            if ( Value == null )
                return 0;

            var valueHash = Value switch
            {
                IConvertible convertible => convertible.GetHashCode(),
                IEnumerable<JsonElement> enumerable => enumerable.GetHashCode(),
                JsonElement jsonElement => jsonElement.ValueKind.GetHashCode(),
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
         *    - Sequence equality should consider deep equality of JsonElement items.
         *    - Return 0 if sequences are equal.
         *    - Return -1 if the left sequence is less.
         *    - Return 1 if the left sequence is greater.
         *
         * 4. Compare NodeList to Value:
         *    - A NodeList is equal to a value if any element in the NodeList matches the value.
         *    - Return 0 if any element matches the value.
         *    - Return -1 if the value is less than all elements.
         *    - Return 1 if the value is greater than all elements.
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
        private static int Compare( Comparand left, Comparand right, bool lessThan = false )
        {
            if ( left.Value is IEnumerable<TNode> leftEnumerable && right.Value is IEnumerable<TNode> rightEnumerable )
            {
                return CompareEnumerables( left.Accessor, leftEnumerable, rightEnumerable );
            }

            if ( left.Value is IEnumerable<TNode> leftEnumerable1 )
            {
                var compare = CompareEnumerableToValue( left.Accessor, leftEnumerable1, right.Value, out var count );

                if ( compare == int.MinValue )
                    return lessThan ? 1 : -1; // If the list is empty, return 1 for less than, -1 for greater than

                return count == 1 ? compare : compare * -1; // If only one element in the list, return the comparison result
            }

            if ( right.Value is IEnumerable<TNode> rightEnumerable1 )
            {
                var compare = CompareEnumerableToValue( left.Accessor, rightEnumerable1, left.Value, out var count );

                if ( compare == int.MinValue )
                    return lessThan ? 1 : -1; // If the list is empty, return 1 for less than, -1 for greater than

                return count == 1 ? compare : compare * -1; // If only one element in the list, return the comparison result
            }

            return CompareValues( left.Value, right.Value );
        }

        private static int CompareEnumerables( IValueAccessor<TNode> accessor, IEnumerable<TNode> left, IEnumerable<TNode> right )
        {
            using var leftEnumerator = left.GetEnumerator();
            using var rightEnumerator = right.GetEnumerator();

            while ( leftEnumerator.MoveNext() )
            {
                if ( !rightEnumerator.MoveNext() )
                    return 1; // Left has more elements, so it is greater

                if ( !accessor.DeepEquals( leftEnumerator.Current, rightEnumerator.Current ) )
                    return -1; // Elements are not deeply equal
            }

            if ( rightEnumerator.MoveNext() )
                return -1; // Right has more elements, so left is less

            return 0; // Sequences are equal
        }

        private static int CompareEnumerableToValue( IValueAccessor<TNode> accessor, IEnumerable<TNode> enumeration, object value, out int count )
        {
            count = 0;
            var lastCompare = -1;
            foreach ( var item in enumeration )
            {
                count++;

                if ( !accessor.TryGetValueFromNode( item, out var itemValue ) )
                    continue; // Skip if value cannot be extracted

                lastCompare = CompareValues( itemValue, value );
                if ( lastCompare == 0 )
                    return 0; // Return 0 if any element matches the value
            }

            if ( count == 0 )
                return int.MinValue; // Return min if the list is empty (no elements match the value)

            // if only one element in the list, return the comparison result as is
            return count == 1 ? lastCompare : -count;
        }

        private static int CompareValues( object left, object right )
        {
            if ( left == null && right == null )
            {
                return 0;
            }

            if ( left?.GetType() != right?.GetType() )
            {
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
