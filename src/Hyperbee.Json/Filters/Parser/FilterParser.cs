#region License

// This code is adapted from an algorithm published in MSDN Magazine, October 2015.
// Original article: "A Split-and-Merge Expression Parser in C#" by Vassili Kaplan.
// URL: https://learn.microsoft.com/en-us/archive/msdn-magazine/2015/october/csharp-a-split-and-merge-expression-parser-in-csharp
//  
// Adapted for use in this project under the terms of the Microsoft Public License (Ms-PL).
// https://opensource.org/license/ms-pl-html

#endregion

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

        throw new ArgumentException( $"Unsupported literal: {state.Buffer.ToString()}" );

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
        // Ensure both expressions are value expressions
        left.Expression = ExpressionConverter.ConvertToValue( descriptor.Accessor, left.Expression );
        right.Expression = ExpressionConverter.ConvertToValue( descriptor.Accessor, right.Expression );

        left.Expression = left.Operator switch
        {
            Operator.Equals when IsNumerical( left.Expression?.Type ) || IsNumerical( right.Expression.Type ) =>
                Expression.Equal(
                    ExpressionConverter.ConvertToNumber( left.Expression ),
                    ExpressionConverter.ConvertToNumber( right.Expression ) ),

            Operator.NotEquals when IsNumerical( left.Expression?.Type ) || IsNumerical( right.Expression.Type ) =>
                Expression.NotEqual(
                    ExpressionConverter.ConvertToNumber( left.Expression ),
                    ExpressionConverter.ConvertToNumber( right.Expression ) ),

            Operator.GreaterThan =>
                Expression.GreaterThan(
                    ExpressionConverter.ConvertToNumber( left.Expression ),
                    ExpressionConverter.ConvertToNumber( right.Expression ) ),

            Operator.GreaterThanOrEqual =>
                Expression.GreaterThanOrEqual(
                    ExpressionConverter.ConvertToNumber( left.Expression ),
                    ExpressionConverter.ConvertToNumber( right.Expression ) ),

            Operator.LessThan =>
                Expression.LessThan(
                    ExpressionConverter.ConvertToNumber( left.Expression ),
                    ExpressionConverter.ConvertToNumber( right.Expression ) ),

            Operator.LessThanOrEqual =>
                Expression.LessThanOrEqual(
                    ExpressionConverter.ConvertToNumber( left.Expression ),
                    ExpressionConverter.ConvertToNumber( right.Expression ) ),

            Operator.Equals => Equal( left.Expression, right.Expression ),
            Operator.NotEquals => NotEqual( left.Expression, right.Expression ),
            Operator.And => Expression.AndAlso( left.Expression!, right.Expression ),
            Operator.Or => Expression.OrElse( left.Expression!, right.Expression ),
            Operator.Not => Expression.Not( right.Expression ),
            _ => left.Expression
        };

        // Wrap left expression in a try-catch block to handle exceptions
        left.Expression = left.Expression == null
            ? left.Expression
            : Expression.TryCatch(
                left.Expression,
                Expression.Catch( typeof( NotSupportedException ), Expression.Rethrow( left.Expression.Type ) ),
                Expression.Catch( typeof( Exception ), Expression.Constant( false ) )
            );

        left.Operator = right.Operator;
        return;

        // Helper method to determine if a type is numerical
        static bool IsNumerical( Type type ) => type == typeof( float ) || type == typeof( int );

        // Helper methods to create comparison expressions
        static Expression Equal( Expression l, Expression r ) => Expression.Call( FilterParser.ObjectEquals, l, r );
        static Expression NotEqual( Expression l, Expression r ) => Expression.Not( Equal( l, r ) );
    }

    private static class ExpressionConverter
    {
        // Cached delegate for calling IValueAccessor<TNode>GetAsValue( IEnumerable<TNode> nodes ) 

        private static readonly Func<IValueAccessor<TNode>, IEnumerable<TNode>, object> GetAsValueDelegate;

        static ExpressionConverter()
        {
            // Pre-compile the delegate to call the GetAsValue method

            var accessorParam = Expression.Parameter( typeof( IValueAccessor<TNode> ), "accessor" );
            var expressionParam = Expression.Parameter( typeof( IEnumerable<TNode> ), "expression" );

            var methodInfo = typeof( IValueAccessor<TNode> ).GetMethod( nameof( IValueAccessor<TNode>.GetAsValue ) );
            var callExpression = Expression.Call( accessorParam, methodInfo!, expressionParam );

            GetAsValueDelegate = Expression.Lambda<Func<IValueAccessor<TNode>, IEnumerable<TNode>, object>>(
                callExpression, accessorParam, expressionParam ).Compile();
        }

        public static Expression ConvertToValue( IValueAccessor<TNode> accessor, Expression expression )
        {
            if ( expression == null || expression.Type != typeof( IEnumerable<TNode> ) )
                return expression;

            // Create an expression representing the instance of the accessor
            var accessorExpression = Expression.Constant( accessor );

            // Use the compiled delegate to create an expression to call the GetAsValue method
            return Expression.Invoke( Expression.Constant( GetAsValueDelegate ), accessorExpression, expression );
        }

        // Helper method to convert numerical types to float
        public static Expression ConvertToNumber( Expression expression )
        {
            if ( expression.Type == typeof( float ) ) // quick out
                return expression;

            if ( expression.Type == typeof( object ) ||
                 expression.Type == typeof( int ) ||
                 expression.Type == typeof( short ) ||
                 expression.Type == typeof( long ) ||
                 expression.Type == typeof( double ) ||
                 expression.Type == typeof( decimal ) )
            {
                return Expression.Convert( expression, typeof( float ) );
            }

            return expression;
        }
    }

    private class ExprItem( Expression expression, Operator op )
    {
        public Expression Expression { get; set; } = expression;
        public Operator Operator { get; set; } = op;
    }
}
