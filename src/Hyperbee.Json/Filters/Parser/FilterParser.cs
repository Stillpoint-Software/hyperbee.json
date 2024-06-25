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

public class FilterParser
{
    public const char EndLine = '\n';
    public const char EndArg = ')';
    public const char ArgSeparator = ',';

    private static readonly MethodInfo ObjectEquals = typeof( object ).GetMethod( "Equals", [typeof( object ), typeof( object )] );

    public static Func<TNode, TNode, bool> Compile<TNode>( ReadOnlySpan<char> filter, ITypeDescriptor descriptor )
    {
        var currentParam = Expression.Parameter( typeof( TNode ) );
        var rootParam = Expression.Parameter( typeof( TNode ) );
        var context = new FilterContext( currentParam, rootParam, descriptor );

        var expression = Parse( filter, context );

        return Expression.Lambda<Func<TNode, TNode, bool>>( expression, currentParam, rootParam ).Compile();
    }

    internal static Expression Parse( ReadOnlySpan<char> filter, FilterContext context )
    {
        var pos = 0;
        var state = new ParserState( filter, [], ref pos, Operator.Nop, EndLine );

        var expression = Parse( ref state, context );

        return FilterTruthyExpression.IsTruthyExpression( expression );
    }

    internal static Expression Parse( ref ParserState state, FilterContext context )
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
            items.Add( GetExpr( ref state, context ) );

        } while ( state.IsParsing );

        // advance to next character for recursive calls.
        if ( !state.EndOfBuffer && state.IsTerminal )
            state.Pos++;

        // merge the expressions
        var baseItem = items[0];
        var index = 1;

        return Merge( baseItem, ref index, items, context.Descriptor );
    }

    private static ExprItem GetExpr( ref ParserState state, FilterContext context )
    {
        if ( NotExprItem.TryGetItem( ref state, ExprItemFactoryImpl, out var exprItem, context ) )
            return exprItem;

        if ( ParenExprItem.TryGetItem( ref state, ExprItemFactoryImpl, out exprItem, context ) ) // will recurse.
            return exprItem;

        if ( SelectExprItem.TryGetItem( ref state, ExprItemFactoryImpl, out exprItem, context ) )
            return exprItem;

        if ( FunctionExprItem.TryGetItem( ref state, ExprItemFactoryImpl, out exprItem, context ) ) // may recurse for each function argument.
            return exprItem;

        if ( LiteralExprItem.TryGetItem( ref state, ExprItemFactoryImpl, out exprItem, context ) )
            return exprItem;

        throw new ArgumentException( $"Unsupported literal: {state.Buffer.ToString()}" );

        // Helper method to create an expression item
        static ExprItem ExprItemFactoryImpl( ref ParserState state, Expression expression )
        {
            UpdateOperator( ref state );
            return new( expression, state.Operator );
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

        state.Item = state.Buffer[itemStart..itemEnd].TrimEnd(); // set item
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

    private static Expression Merge( ExprItem current, ref int index, List<ExprItem> items, ITypeDescriptor descriptor, bool mergeOneOnly = false )
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

    private static void MergeItems( ExprItem left, ExprItem right, ITypeDescriptor descriptor )
    {
        // Ensure both expressions are value expressions
        left.Expression = descriptor.GetValueExpression( left.Expression );
        right.Expression = descriptor.GetValueExpression( right.Expression );

        // Determine if we are comparing numerical values so that we can use the correct comparison method
        bool isNumerical = IsNumerical( left.Expression?.Type ) || IsNumerical( right.Expression.Type );

        left.Expression = left.Operator switch
        {
            Operator.Equals => CompareConvert( isNumerical ? Expression.Equal : Equal, left.Expression, right.Expression, isNumerical ),
            Operator.NotEquals => CompareConvert( isNumerical ? Expression.NotEqual : NotEqual, left.Expression, right.Expression, isNumerical ),

            // Assume/force numerical
            Operator.GreaterThan => CompareConvert( Expression.GreaterThan, left.Expression, right.Expression ),
            Operator.GreaterThanOrEqual => CompareConvert( Expression.GreaterThanOrEqual, left.Expression, right.Expression ),
            Operator.LessThan => CompareConvert( Expression.LessThan, left.Expression, right.Expression ),
            Operator.LessThanOrEqual => CompareConvert( Expression.LessThanOrEqual, left.Expression, right.Expression ),

            Operator.And => Expression.AndAlso( left.Expression!, right.Expression ),
            Operator.Or => Expression.OrElse( left.Expression!, right.Expression ),

            Operator.Not => Expression.Not( right.Expression ),
            _ => left.Expression
        };

        // Wrap left expression in a try-catch block to handle exceptions
        left.Expression = left.Expression == null
            ? left.Expression
            : Expression.TryCatchFinally(
                left.Expression,
                Expression.Empty(), // Ensure finally block is present
                Expression.Catch( typeof( Exception ), Expression.Constant( false ) )
            );

        left.Operator = right.Operator;
        return;

        // Helper method to determine if a type is numerical
        static bool IsNumerical( Type type ) => type == typeof( float ) || type == typeof( int );

        // Helper methods to create comparison expressions
        static Expression Equal( Expression l, Expression r ) => Expression.Call( ObjectEquals, l, r );
        static Expression NotEqual( Expression l, Expression r ) => Expression.Not( Equal( l, r ) );
    }

    private static Expression CompareConvert( Func<Expression, Expression, Expression> compare, Expression left, Expression right, bool isNumerical = true )
    {
        if ( isNumerical )
        {
            left = ConvertNumericalToFloat( left );
            right = ConvertNumericalToFloat( right );
        }
        else
        {
            // Handle object to string conversion
            if ( left.Type == typeof( object ) && right.Type == typeof( string ) )
                return compare( Convert<string>( left ), right );

            if ( left.Type == typeof( string ) && right.Type == typeof( object ) )
                return compare( left, Convert<string>( right ) );
        }

        return compare( left, right );

        // Helper method to convert an expression to a specified type
        static Expression Convert<TType>( Expression expression ) => Expression.Convert( expression, typeof( TType ) );

        // Helper method to convert numerical types to float
        static Expression ConvertNumericalToFloat( Expression expression )
        {
            if ( expression.Type == typeof( object ) ||
                 expression.Type == typeof( int ) ||
                 expression.Type == typeof( short ) ||
                 expression.Type == typeof( long ) ||
                 expression.Type == typeof( double ) ||
                 expression.Type == typeof( decimal ) )
            {
                return Convert<float>( expression );
            }

            return expression;
        }
    }
}
