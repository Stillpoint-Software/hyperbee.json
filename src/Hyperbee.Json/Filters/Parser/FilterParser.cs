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
using Hyperbee.Json.Descriptors;
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
                left.Expression = JsonComparerExpressionFactory.GetComparer( descriptor.Accessor, left.Expression );
                right.Expression = JsonComparerExpressionFactory.GetComparer( descriptor.Accessor, right.Expression );

                left.Expression = Expression.Equal( left.Expression, right.Expression );
                break;
            case Operator.NotEquals:
                left.Expression = JsonComparerExpressionFactory.GetComparer( descriptor.Accessor, left.Expression );
                right.Expression = JsonComparerExpressionFactory.GetComparer( descriptor.Accessor, right.Expression );

                left.Expression = Expression.NotEqual( left.Expression, right.Expression );
                break;
            case Operator.GreaterThan:
                left.Expression = JsonComparerExpressionFactory.GetComparer( descriptor.Accessor, left.Expression );
                right.Expression = JsonComparerExpressionFactory.GetComparer( descriptor.Accessor, right.Expression );

                left.Expression = Expression.GreaterThan( left.Expression, right.Expression );
                break;
            case Operator.GreaterThanOrEqual:
                left.Expression = JsonComparerExpressionFactory.GetComparer( descriptor.Accessor, left.Expression );
                right.Expression = JsonComparerExpressionFactory.GetComparer( descriptor.Accessor, right.Expression );

                left.Expression = Expression.GreaterThanOrEqual( left.Expression, right.Expression );
                break;
            case Operator.LessThan:
                left.Expression = JsonComparerExpressionFactory.GetComparer( descriptor.Accessor, left.Expression );
                right.Expression = JsonComparerExpressionFactory.GetComparer( descriptor.Accessor, right.Expression );

                left.Expression = Expression.LessThan( left.Expression, right.Expression );
                break;
            case Operator.LessThanOrEqual:
                left.Expression = JsonComparerExpressionFactory.GetComparer( descriptor.Accessor, left.Expression );
                right.Expression = JsonComparerExpressionFactory.GetComparer( descriptor.Accessor, right.Expression );

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
        private static readonly Func<IValueAccessor<TNode>, object, JsonComparer> GetJsonValueDelegate;

        static JsonComparerExpressionFactory()
        {
            // Pre-compile the delegate to call the JsonComparer.Create method

            var accessorParam = Expression.Parameter( typeof( IValueAccessor<TNode> ), "accessor" );
            var valueParam = Expression.Parameter( typeof( object ), "value" );

            var methodInfo = typeof( JsonComparer ).GetMethod( nameof( JsonComparer.Create ) );
            var callExpression = Expression.Call( methodInfo!, accessorParam, valueParam );

            GetJsonValueDelegate = Expression.Lambda<Func<IValueAccessor<TNode>, object, JsonComparer>>(
                callExpression, accessorParam, valueParam ).Compile();
        }

        public static Expression GetComparer( IValueAccessor<TNode> accessor, Expression expression )
        {
            // Handles Not operator since it maybe not have a left side.
            if ( expression == null )
                return null;

            // Create an expression representing the instance of the accessor
            var accessorExpression = Expression.Constant( accessor );

            // Use the compiled delegate to create an expression to call the GetAsValue method
            return Expression.Invoke( Expression.Constant( GetJsonValueDelegate ), accessorExpression,
                Expression.Convert( expression, typeof( object ) ) );
        }

        [DebuggerDisplay( "Value = {_value}" )]
        public class JsonComparer
        {
            private readonly IValueAccessor<TNode> _accessor;
            private readonly object _value;

            public static JsonComparer Create( IValueAccessor<TNode> accessor, object value ) => new( accessor, value );

            public JsonComparer( IValueAccessor<TNode> accessor, object value )
            {
                _accessor = accessor;
                _value = value;
            }

            public static bool operator ==( JsonComparer left, JsonComparer right ) => Compare( left, right ) == 0;

            public static bool operator !=( JsonComparer left, JsonComparer right )
            {
                return Compare( left, right ) != 0;
            }

            public static bool operator <( JsonComparer left, JsonComparer right )
            {
                return Compare( left, right ) < 0;
            }

            public static bool operator >( JsonComparer left, JsonComparer right )
            {
                return Compare( left, right ) > 0;
            }

            public static bool operator <=( JsonComparer left, JsonComparer right )
            {
                return Compare( left, right ) <= 0;
            }

            public static bool operator >=( JsonComparer left, JsonComparer right )
            {
                return Compare( left, right ) >= 0;
            }

            static int Compare( JsonComparer left, JsonComparer right )
            {
                var isLeftJsonEnumerable = left._value is IEnumerable<TNode>;
                var isRightJsonEnumerable = right._value is IEnumerable<TNode>;

                if ( isRightJsonEnumerable && isLeftJsonEnumerable )
                {
                    return left._accessor.DeepEquals(
                        ((IEnumerable<TNode>) left._value).FirstOrDefault(),
                        ((IEnumerable<TNode>) right._value).FirstOrDefault() ) ? 0 : -1;
                }

                var leftType = left._value?.GetType();
                var rightType = right._value?.GetType();

                if ( leftType == rightType )
                {
                    if ( TryCompare( left._value, right._value, out var result ) )
                    {
                        return result!.Value;
                    }
                }

                if ( isRightJsonEnumerable )
                {
                    var rightValue = right._accessor.GetAsValueOther( (IEnumerable<TNode>) right._value );
                    if ( TryCompare( rightValue, left._value, out var result ) )
                    {
                        return result!.Value;
                    }
                    return CompareTruthy( rightValue, left._value );
                }

                if ( isLeftJsonEnumerable )
                {
                    var leftValue = right._accessor.GetAsValueOther( (IEnumerable<TNode>) left._value );
                    if ( TryCompare( leftValue, right._value, out var result ) )
                    {
                        return result!.Value;
                    }
                    return CompareTruthy( leftValue, right._value );
                }

                return -1;

                static bool TryCompare( object left, object right, out int? compare )
                {
                    switch ( left )
                    {
                        case null:
                            compare = right == null ? 0 : -1;
                            return true;
                        case bool boolValue:
                            compare = boolValue.CompareTo( (bool) right );
                            return true;
                        case float floatValue:
                            compare = floatValue.CompareTo( (float) right );
                            return true;
                        case string strValue:
                            compare = string.Compare( strValue, (string) right, StringComparison.Ordinal );
                            return true;
                        case TNode node:
                            compare = node.Equals( right ) ? 0 : -1;
                            return true;
                    }

                    compare = null;
                    return false;
                }

                static int CompareTruthy( object first, object second )
                {
                    return first switch
                    {
                        bool boolValue => boolValue.CompareTo( (bool) second ),
                        float floatValue => floatValue.CompareTo( (float) second ),
                        string strValue => string.Compare( strValue, (string) second, StringComparison.Ordinal ),
                        _ => FilterTruthyExpression.IsTruthy( first ).CompareTo( FilterTruthyExpression.IsTruthy( second ) )
                    };
                }
            }

            public override bool Equals( object obj )
            {
                if ( ReferenceEquals( this, obj ) )
                {
                    return true;
                }

                if ( ReferenceEquals( obj, null ) )
                {
                    return false;
                }

                throw new NotImplementedException();
            }

            public override int GetHashCode()
            {
                throw new NotImplementedException();
            }
        }
    }

    private class ExprItem( Expression expression, Operator op )
    {
        public Expression Expression { get; set; } = expression;
        public Operator Operator { get; set; } = op;
    }
}
