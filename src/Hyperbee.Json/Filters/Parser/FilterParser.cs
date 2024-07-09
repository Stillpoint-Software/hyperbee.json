#region License

// This code is adapted from an algorithm published in MSDN Magazine, October 2015.
// Original article: "A Split-and-Merge Expression Parser in C#" by Vassili Kaplan.
// URL: https://learn.microsoft.com/en-us/archive/msdn-magazine/2015/october/csharp-a-split-and-merge-expression-parser-in-csharp
//  
// Adapted for use in this project under the terms of the Microsoft Public License (Ms-PL).
// https://opensource.org/license/ms-pl-html

#endregion

using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Filters.Parser.Expressions;

namespace Hyperbee.Json.Filters.Parser;

public abstract class FilterParser
{
    public const char EndLine = '\0'; // using null character instead of \n
    public const char EndArg = ')';
    public const char ArgSeparator = ',';
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
        filter = filter.Trim(); // remove leading and trailing whitespace to simplify parsing

        var pos = 0;
        var state = new ParserState( filter, [], ref pos, Operator.NonOperator, EndLine );

        var expression = Parse( ref state, context );

        return FilterTruthyExpression.IsTruthyExpression( expression );
    }

    internal static Expression Parse( ref ParserState state, FilterContext<TNode> context ) // recursion entrypoint
    {
        // validate input
        if ( context == null )
            throw new ArgumentNullException( nameof( context ) );

        if ( state.EndOfBuffer || state.IsTerminal )
            throw new NotSupportedException( $"Invalid filter: \"{state.Buffer}\"." );

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

        return Merge( in state, baseItem, ref index, items, context );
    }


    private static ExprItem GetExprItem( ref ParserState state, FilterContext<TNode> context )
    {
        var expressionInfo = new ExpressionInfo();

        if ( NotExpressionFactory.TryGetExpression( ref state, out var expression, ref expressionInfo, context ) )
            return ExprItem( ref state, expression, expressionInfo );

        if ( ParenExpressionFactory.TryGetExpression( ref state, out expression, ref expressionInfo, context ) ) // will recurse.
            return ExprItem( ref state, expression, expressionInfo );

        if ( SelectExpressionFactory.TryGetExpression( ref state, out expression, ref expressionInfo, context ) )
            return ExprItem( ref state, expression, expressionInfo );

        if ( FunctionExpressionFactory.TryGetExpression( ref state, out expression, ref expressionInfo, context ) ) // may recurse for each function argument.
            return ExprItem( ref state, expression, expressionInfo );

        if ( LiteralExpressionFactory.TryGetExpression( ref state, out expression, ref expressionInfo, context ) )
            return ExprItem( ref state, expression, expressionInfo );

        if ( JsonExpressionFactory.TryGetExpression( ref state, out expression, ref expressionInfo, context ) )
            return ExprItem( ref state, expression, expressionInfo );

        throw new NotSupportedException( $"Unsupported literal: {state.Buffer.ToString()}" );

        // Helper method to create an expression item
        static ExprItem ExprItem( ref ParserState state, Expression expression, ExpressionInfo expressionInfo )
        {
            UpdateOperator( ref state );
            return new ExprItem( expression, state.Operator, expressionInfo );
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
            state.Operator = Operator.EndOfBuffer;
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

            if ( IsFinished( in state, state.Pos - itemStart, nextChar ) )
                break;

            if ( !state.EndOfBuffer && !state.IsTerminal )
                continue;

            itemEnd = state.Pos; // fall-through: include the terminal character
            break;
        }

        state.SetItem( itemStart, itemEnd );

        return;

        // Helper method to determine if item parsing is finished
        static bool IsFinished( in ParserState state, int count, char ch )
        {
            if ( state.BracketDepth != 0 )
                return false;

            // order of operations matters here
            if ( count == 0 && ch == EndArg )
                return false;

            if ( !state.Operator.IsNonOperator() && state.Operator != Operator.ClosedParen )
                return true;

            if ( ch == state.Terminal || ch == EndArg || ch == EndLine )
                return true;

            return false;
        }
    }

    private static void NextCharacter( ref ParserState state, out char nextChar, ref char? quoteChar )
    {
        nextChar = state.Buffer[state.Pos++];

        // Handle escape characters within quotes
        if ( quoteChar.HasValue )
        {
            if ( nextChar == '\\' && state.Pos < state.Buffer.Length )
            {
                nextChar = state.Buffer[state.Pos++];
            }
            else if ( nextChar == quoteChar && (state.Pos <= 1 || state.Buffer[state.Pos - 2] != '\\') )
            {
                quoteChar = null; // Exiting quoted string
            }
            return;
        }

        // Normal character handling
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
            case ' ' or '\t' or '\r' or '\n':
                state.Operator = Operator.Whitespace;
                break;
            case '[':
                state.BracketDepth++;
                state.Operator = Operator.Bracket;
                break;
            case ']':
                state.BracketDepth--;
                state.Operator = Operator.Bracket;
                break;
            case '\'' or '\"':
                quoteChar = nextChar; // Entering a quoted string
                state.Operator = Operator.Quotes;
                break;
            default:
                state.Operator = Operator.Segment;
                break;
        }

        return;

        // Helper method to check if the next character is the expected character
        static bool Next( ref ParserState state, char expected )
        {
            if ( state.EndOfBuffer || state.Buffer[state.Pos] != expected )
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
            state.Operator = Operator.EndOfBuffer;
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
        static bool IsParenOrNop( Operator op ) => (op is Operator.OpenParen or Operator.ClosedParen) || op.IsNonOperator();
        static bool IsParen( Operator op ) => op is Operator.OpenParen or Operator.ClosedParen;
    }

    private static Expression Merge( in ParserState state, ExprItem left, ref int index, List<ExprItem> items, FilterContext<TNode> context, bool mergeOneOnly = false )
    {
        if ( items.Count == 1 )
        {
            ThrowIfInvalid( in state, left, null ); // single item, no recursion
        }
        else
        {
            while ( index < items.Count )
            {
                var right = items[index++];

                while ( !CanMergeItems( left, right ) )
                {
                    Merge( in state, right, ref index, items, context, mergeOneOnly: true ); // recursive call
                }

                ThrowIfInvalid( in state, left, right );
                MergeItems( left, right, context );

                if ( mergeOneOnly )
                    return left.Expression;
            }
        }

        return left.Expression;

        static void ThrowIfInvalid( in ParserState state, ExprItem left, ExprItem right )
        {
            ThrowIfConstantIsNotCompared( in state, left, right );
            ThrowIfNonSingularCompare( in state, left, right );
        }

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

    private static void MergeItems( ExprItem left, ExprItem right, FilterContext<TNode> context )
    {
        switch ( left.Operator )
        {
            case Operator.Equals:
                left.Expression = ComparerExpressionFactory<TNode>.GetComparand( context, left.Expression );
                right.Expression = ComparerExpressionFactory<TNode>.GetComparand( context, right.Expression );

                left.Expression = Expression.Equal( left.Expression, right.Expression );
                break;
            case Operator.NotEquals:
                left.Expression = ComparerExpressionFactory<TNode>.GetComparand( context, left.Expression );
                right.Expression = ComparerExpressionFactory<TNode>.GetComparand( context, right.Expression );

                left.Expression = Expression.NotEqual( left.Expression, right.Expression );
                break;
            case Operator.GreaterThan:
                left.Expression = ComparerExpressionFactory<TNode>.GetComparand( context, left.Expression );
                right.Expression = ComparerExpressionFactory<TNode>.GetComparand( context, right.Expression );

                left.Expression = Expression.GreaterThan( left.Expression, right.Expression );
                break;
            case Operator.GreaterThanOrEqual:
                left.Expression = ComparerExpressionFactory<TNode>.GetComparand( context, left.Expression );
                right.Expression = ComparerExpressionFactory<TNode>.GetComparand( context, right.Expression );

                left.Expression = Expression.GreaterThanOrEqual( left.Expression, right.Expression );
                break;
            case Operator.LessThan:
                left.Expression = ComparerExpressionFactory<TNode>.GetComparand( context, left.Expression );
                right.Expression = ComparerExpressionFactory<TNode>.GetComparand( context, right.Expression );

                left.Expression = Expression.LessThan( left.Expression, right.Expression );
                break;
            case Operator.LessThanOrEqual:
                left.Expression = ComparerExpressionFactory<TNode>.GetComparand( context, left.Expression );
                right.Expression = ComparerExpressionFactory<TNode>.GetComparand( context, right.Expression );

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
            case Operator.NonOperator:
            case Operator.Whitespace:
            case Operator.Quotes:
            case Operator.Segment:
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
        left.ExpressionInfo.Kind = ExpressionKind.Merged;
    }

    private static void ThrowIfNonSingularCompare( in ParserState state, ExprItem left, ExprItem right )
    {
        if ( IsNonSingularCompare( left ) || (right != null && IsNonSingularCompare( right )) )
            throw new NotSupportedException( $"Unsupported non-single query: {state.Buffer.ToString()}." );

        return;

        static bool IsNonSingularCompare( ExprItem item ) =>
            item.ExpressionInfo.NonSingularQuery && item.Operator.IsComparison();
    }

    private static void ThrowIfConstantIsNotCompared( in ParserState state, ExprItem left, ExprItem right )
    {
        if ( state.IsArgument )
            return;

        if ( left.ExpressionInfo.Kind == ExpressionKind.Literal && !IsComparisonOperator( left.Operator ) )
            throw new NotSupportedException( $"Unsupported literal without comparison: {state.Buffer.ToString()}." );

        if ( right != null && right.ExpressionInfo.Kind == ExpressionKind.Literal && !IsComparisonOperator( left.Operator ) )
            throw new NotSupportedException( $"Unsupported literal without comparison: {state.Buffer.ToString()}." );

        return;

        static bool IsComparisonOperator( Operator op ) =>
            op == Operator.Equals || op == Operator.NotEquals ||
            op == Operator.GreaterThan || op == Operator.GreaterThanOrEqual ||
            op == Operator.LessThan || op == Operator.LessThanOrEqual;
    }

    private class ExprItem( Expression expression, Operator op, ExpressionInfo expressionInfo )
    {
        public ExpressionInfo ExpressionInfo { get; } = expressionInfo;
        public Expression Expression { get; set; } = expression;
        public Operator Operator { get; set; } = op;
    }
}
