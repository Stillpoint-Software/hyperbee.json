#region License

// This code is adapted from an algorithm published in MSDN Magazine, October 2015.
// Original article: "A Split-and-Merge Expression Parser in C#" by Vassili Kaplan.
// URL: https://learn.microsoft.com/en-us/archive/msdn-magazine/2015/october/csharp-a-split-and-merge-expression-parser-in-csharp
//  
// Adapted for use in this project under the terms of the Microsoft Public License (Ms-PL).
// https://opensource.org/license/ms-pl-html

#endregion

using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Filters.Parser.Expressions;

namespace Hyperbee.Json.Filters.Parser;

public abstract class FilterParser
{
    public const char EndLine = '\n';
    public const char EndArg = ')';
    public const char ArgSeparator = ',';

    protected static readonly MethodInfo ObjectEquals = typeof( object ).GetMethod( "Equals", [typeof( object ), typeof( object )] );
}

public class FilterParser<TNode> : FilterParser
{
    public static Func<TNode, TNode, bool> Compile( ReadOnlySpan<char> filter, ITypeDescriptor<TNode> descriptor )
    {
        var context = new FilterContext<TNode>( descriptor );

        var expression = Parse( filter, context );

        return Expression.Lambda<Func<TNode, TNode, bool>>( expression, context.Current, context.Root ).Compile();
    }

    internal static Expression Parse( ReadOnlySpan<char> filter, FilterContext<TNode> context )
    {
        var pos = 0;
        var state = new ParserState( filter, [], ref pos, Operator.Nop, EndLine );

        var expression = Parse( ref state, context );

        return FilterTruthyExpression.IsTruthyExpression( expression );
    }

    internal static Expression Parse( ref ParserState state, FilterContext<TNode> context ) // recursion entrypoint
    {
        // validate input
        if ( context == null )
            throw new ArgumentNullException( nameof( context ) );

        if ( state.EndOfBuffer || state.IsTerminal )
            throw new ArgumentException( $"Invalid filter: \"{state.Buffer}\"", nameof( state ) );

        // parse the expression
        var items = new List<ExprItem>();

        do
        {
            MoveNext( ref state );
            items.Add( GetExprItem( ref state, context ) );

        } while ( state.IsParsing );

        // advance to next character for recursive calls.
        if ( !state.EndOfBuffer && state.IsTerminal )
            state.Pos++;

        // merge the expressions
        var baseItem = items[0];
        var index = 1;

        return Merge( baseItem, ref index, items, context.Descriptor );
    }

    private static ExprItem GetExprItem( ref ParserState state, FilterContext<TNode> context )
    {
        if ( NotExpressionFactory.TryGetExpression( ref state, out var expression, context ) )
            return ExprItem( ref state, expression );

        if ( ParenExpressionFactory.TryGetExpression( ref state, out expression, context ) ) // will recurse.
            return ExprItem( ref state, expression );

        if ( SelectExpressionFactory.TryGetExpression( ref state, out expression, context ) )
            return ExprItem( ref state, expression );

        if ( FunctionExpressionFactory.TryGetExpression( ref state, out expression, context ) ) // may recurse for each function argument.
            return ExprItem( ref state, expression );

        if ( LiteralExpressionFactory.TryGetExpression( ref state, out expression, context ) )
            return ExprItem( ref state, expression );

        if ( JsonExpressionFactory.TryGetExpression( ref state, out expression, context ) )
            return ExprItem( ref state, expression );

        throw new NotSupportedException( $"Unsupported literal: {state.Buffer.ToString()}" );

        // Helper method to create an expression item
        static ExprItem ExprItem( ref ParserState state, Expression expression )
        {
            UpdateOperator( ref state );
            return new ExprItem( expression, state.Operator );
        }
    }

    private static void MoveNext( ref ParserState state )
    {
        char? quote = null;

        // remove leading whitespace
        while ( !state.EndOfBuffer && char.IsWhiteSpace( state.Current ) )
            state.Pos++;

        // check for end of buffer
        if ( state.EndOfBuffer )
        {
            state.Operator = Operator.Nop;
            state.Item = [];
            return;
        }

        // read next item
        var itemStart = state.Pos;
        int itemEnd;

        while ( true )
        {
            itemEnd = state.Pos; // assign before the call to NextCharacter

            NextCharacter( ref state, out var nextChar, ref quote );

            if ( IsFinished( state.Pos - itemStart, nextChar, state.Operator, state.Terminal ) )
                break;

            if ( !state.EndOfBuffer && !state.IsTerminal )
                continue;

            itemEnd = state.Pos; // fall-through: include the terminal character
            break;
        }

        state.SetItem( itemStart, itemEnd );
        return;

        // Helper method to determine if item parsing is finished
        static bool IsFinished( int count, char ch, Operator op, char terminal )
        {
            // order of operations matters here
            if ( count == 0 && ch == EndArg )
                return false;

            if ( op != Operator.Nop && op != Operator.ClosedParen )
                return true;

            if ( ch == terminal || ch == EndArg || ch == EndLine )
                return true;

            return false;
        }
    }

    private static void NextCharacter( ref ParserState state, out char nextChar, ref char? quoteChar )
    {
        nextChar = state.Buffer[state.Pos++];

        switch ( nextChar )
        {
            case '&' when Next( ref state, '&' ):
                state.Operator = Operator.And;
                break;
            case '|' when Next( ref state, '|' ):
                state.Operator = Operator.Or;
                break;
            case '=' when Next( ref state, '=' ):
                state.Operator = Operator.Equals;
                break;
            case '!' when Next( ref state, '=' ):
                state.Operator = Operator.NotEquals;
                break;
            case '>' when Next( ref state, '=' ):
                state.Operator = Operator.GreaterThanOrEqual;
                break;
            case '<' when Next( ref state, '=' ):
                state.Operator = Operator.LessThanOrEqual;
                break;
            case '>':
                state.Operator = Operator.GreaterThan;
                break;
            case '<':
                state.Operator = Operator.LessThan;
                break;
            case '!':
                state.Operator = Operator.Not;
                break;
            case '(':
                state.Operator = Operator.OpenParen;
                break;
            case ')':
                state.Operator = Operator.ClosedParen;
                break;
            case ' ' or '\t' when quoteChar == null:
                state.Operator = Operator.Nop;
                break;
            case '\'' or '\"' when state.Pos > 0 && state.Previous != '\\':
                quoteChar = quoteChar == null ? nextChar : null;
                state.Operator = Operator.Nop;
                break;
            default:
                state.Operator = Operator.Nop;
                break;
        }

        return;

        // Helper method to check if the next character is the expected character
        static bool Next( ref ParserState state, char expected )
        {
            if ( state.EndOfBuffer || state.Current != expected )
                return false;

            state.Pos++;
            return true;
        }
    }

    private static void UpdateOperator( ref ParserState state )
    {
        if ( !IsParenOrNop( state.Operator ) )
            return;

        if ( state.EndOfBuffer )
        {
            state.Operator = Operator.Nop;
            return;
        }

        if ( state.IsTerminal )
        {
            state.Operator = Operator.ClosedParen;
            return;
        }

        char? quoteChar = null;
        var startPos = state.Pos;

        while ( IsParenOrNop( state.Operator ) && !state.EndOfBuffer )
        {
            NextCharacter( ref state, out _, ref quoteChar );
        }

        if ( IsParen( state.Operator ) && state.Pos > startPos )
        {
            state.Pos--;
        }

        return;

        // Helper method to determine if an operator is a parenthesis or a no-op
        static bool IsParenOrNop( Operator op ) => op is Operator.OpenParen or Operator.ClosedParen or Operator.Nop;
        static bool IsParen( Operator op ) => op is Operator.OpenParen or Operator.ClosedParen;
    }

    private static Expression Merge( ExprItem current, ref int index, List<ExprItem> items, ITypeDescriptor<TNode> descriptor, bool mergeOneOnly = false )
    {
        while ( index < items.Count )
        {
            var next = items[index++];

            while ( !CanMergeItems( current, next ) )
            {
                Merge( next, ref index, items, descriptor, mergeOneOnly: true ); // recursive call
            }

            MergeItems( current, next, descriptor );

            if ( mergeOneOnly )
                return current.Expression;
        }

        return current.Expression;

        // Helper method to determine if two items can be merged
        static bool CanMergeItems( ExprItem left, ExprItem right )
        {
            // "Not" can never be a right side operator
            return right.Operator != Operator.Not && GetPriority( left.Operator ) >= GetPriority( right.Operator );
        }

        // Helper method to get the priority of an operator
        static int GetPriority( Operator type )
        {
            return type switch
            {
                Operator.Not => 1,
                Operator.And or
                    Operator.Or => 2,
                Operator.Equals or
                    Operator.NotEquals or
                    Operator.GreaterThan or
                    Operator.GreaterThanOrEqual or
                    Operator.LessThan or
                    Operator.LessThanOrEqual => 3,
                _ => 0,
            };
        }
    }

    private static void MergeItems( ExprItem left, ExprItem right, ITypeDescriptor<TNode> descriptor )
    {
        switch ( left.Operator )
        {
            case Operator.Equals:
                left.Expression = JsonComparerExpressionFactory.GetComparand( descriptor.Accessor, left.Expression );
                right.Expression = JsonComparerExpressionFactory.GetComparand( descriptor.Accessor, right.Expression );

                left.Expression = Expression.Equal( left.Expression, right.Expression );
                break;
            case Operator.NotEquals:
                left.Expression = JsonComparerExpressionFactory.GetComparand( descriptor.Accessor, left.Expression );
                right.Expression = JsonComparerExpressionFactory.GetComparand( descriptor.Accessor, right.Expression );

                left.Expression = Expression.NotEqual( left.Expression, right.Expression );
                break;
            case Operator.GreaterThan:
                left.Expression = JsonComparerExpressionFactory.GetComparand( descriptor.Accessor, left.Expression );
                right.Expression = JsonComparerExpressionFactory.GetComparand( descriptor.Accessor, right.Expression );

                left.Expression = Expression.GreaterThan( left.Expression, right.Expression );
                break;
            case Operator.GreaterThanOrEqual:
                left.Expression = JsonComparerExpressionFactory.GetComparand( descriptor.Accessor, left.Expression );
                right.Expression = JsonComparerExpressionFactory.GetComparand( descriptor.Accessor, right.Expression );

                left.Expression = Expression.GreaterThanOrEqual( left.Expression, right.Expression );
                break;
            case Operator.LessThan:
                left.Expression = JsonComparerExpressionFactory.GetComparand( descriptor.Accessor, left.Expression );
                right.Expression = JsonComparerExpressionFactory.GetComparand( descriptor.Accessor, right.Expression );

                left.Expression = Expression.LessThan( left.Expression, right.Expression );
                break;
            case Operator.LessThanOrEqual:
                left.Expression = JsonComparerExpressionFactory.GetComparand( descriptor.Accessor, left.Expression );
                right.Expression = JsonComparerExpressionFactory.GetComparand( descriptor.Accessor, right.Expression );

                left.Expression = Expression.LessThanOrEqual( left.Expression, right.Expression );
                break;
            case Operator.And:
                left.Expression = Expression.AndAlso(
                    FilterTruthyExpression.IsTruthyExpression( left.Expression! ),
                    FilterTruthyExpression.IsTruthyExpression( right.Expression )
                );
                break;
            case Operator.Or:
                left.Expression = Expression.OrElse(
                    FilterTruthyExpression.IsTruthyExpression( left.Expression! ),
                    FilterTruthyExpression.IsTruthyExpression( right.Expression )
                );
                break;
            case Operator.Not:
                left.Expression = Expression.Not(
                    FilterTruthyExpression.IsTruthyExpression( right.Expression )
                );
                break;
            case Operator.Nop:
            case Operator.OpenParen:
            case Operator.ClosedParen:
            default:
                left.Expression = left.Expression;
                break;
        }

        // Wrap left expression in a try-catch block to handle exceptions
        left.Expression = left.Expression == null
            ? left.Expression
            : Expression.TryCatch(
                left.Expression,
                Expression.Catch( typeof( NotSupportedException ), Expression.Rethrow( left.Expression.Type ) ),
                Expression.Catch( typeof( Exception ), Expression.Constant( false ) )
            );

        left.Operator = right.Operator;
    }

    internal static class JsonComparerExpressionFactory
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
        public class Comparand( IValueAccessor<TNode> accessor, object value ) : IComparable<Comparand>, IEquatable<Comparand>
        {
            private const float Tolerance = 1e-6F; // Define a tolerance for float comparisons

            private IValueAccessor<TNode> Accessor { get; } = accessor;

            private object Value { get; } = value;

            public int CompareTo( Comparand other ) => Compare( this, other );
            public bool Equals( Comparand other ) => Compare( this, other ) == 0;
            public override bool Equals( object obj ) => obj is Comparand other && Equals( other );

            public static bool operator ==( Comparand left, Comparand right ) => Compare( left, right ) == 0;
            public static bool operator !=( Comparand left, Comparand right ) => Compare( left, right ) != 0;
            public static bool operator <( Comparand left, Comparand right ) => Compare( left, right ) < 0;
            public static bool operator >( Comparand left, Comparand right ) => Compare( left, right ) > 0;
            public static bool operator <=( Comparand left, Comparand right ) => Compare( left, right ) <= 0;
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
            private static int Compare( Comparand left, Comparand right )
            {
                if ( left.Value is IEnumerable<JsonElement> leftEnumerable && right.Value is IEnumerable<JsonElement> rightEnumerable )
                {
                    return CompareEnumerables( leftEnumerable, rightEnumerable );
                }

                if ( left.Value is IEnumerable<JsonElement> leftEnumerable1 )
                {
                    return CompareEnumerableToValue( leftEnumerable1, right.Value );
                }

                if ( right.Value is IEnumerable<JsonElement> rightEnumerable1 )
                {
                    return CompareEnumerableToValue( rightEnumerable1, left.Value );
                }

                return CompareValues( left.Value, right.Value );
            }

            private static int CompareEnumerables( IEnumerable<JsonElement> left, IEnumerable<JsonElement> right )
            {
                using var leftEnumerator = left.GetEnumerator();
                using var rightEnumerator = right.GetEnumerator();

                while ( leftEnumerator.MoveNext() )
                {
                    if ( !rightEnumerator.MoveNext() )
                        return 1; // Left has more elements, so it is greater

                    if ( !leftEnumerator.Current.DeepEquals( rightEnumerator.Current ) )
                        return -1; // Elements are not deeply equal
                }

                if ( rightEnumerator.MoveNext() )
                    return -1; // Right has more elements, so left is less

                return 0; // Sequences are equal
            }

            private static int CompareEnumerableToValue( IEnumerable<JsonElement> enumeration, object value )
            {
                foreach ( var item in enumeration )
                {
                    if ( !TryGetValueFromNode( item, out var itemValue ) )
                        continue; // Skip if value cannot be extracted

                    if ( CompareValues( itemValue, value ) == 0 )
                        return 0; // Return 0 if any element matches the value
                }

                // If no elements match the value, return -1
                return -1;
            }

            private static int CompareValues( object left, object right )
            {
                if ( left.GetType() != right.GetType() )
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

            // THESE GO TO THE ACCESSOR

            private static bool TryGetValueFromNode( JsonElement element, out object value )
            {
                switch ( element.ValueKind )
                {
                    case JsonValueKind.String:
                        value = element.GetString();
                        break;
                    case JsonValueKind.Number:
                        value = element.GetSingle();
                        break;
                    case JsonValueKind.True:
                        value = true;
                        break;
                    case JsonValueKind.False:
                        value = false;
                        break;
                    case JsonValueKind.Null:
                        value = null;
                        break;
                    default:
                        value = false;
                        return false;
                }

                return true;
            }
        }
    }

    private class ExprItem( Expression expression, Operator op )
    {
        public Expression Expression { get; set; } = expression;
        public Operator Operator { get; set; } = op;
    }
}
