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
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Filters.Parser.Expressions;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser;

public abstract class FilterParser
{
    public const char EndLine = '\0'; // use null instead of newline
    public const char ArgClose = ')';
    public const char ArgComma = ',';
}

public class FilterParser<TNode> : FilterParser
{
    internal static readonly ParameterExpression RuntimeContextExpression = Expression.Parameter( typeof( FilterRuntimeContext<TNode> ), "runtimeContext" );

    public static Func<FilterRuntimeContext<TNode>, bool> Compile( ReadOnlySpan<char> filter, ITypeDescriptor<TNode> descriptor )
    {
        var expression = Parse( filter, descriptor );
        return Expression.Lambda<Func<FilterRuntimeContext<TNode>, bool>>( expression, RuntimeContextExpression ).Compile();
    }

    internal static Expression Parse( ReadOnlySpan<char> filter, ITypeDescriptor<TNode> descriptor )
    {
        filter = filter.Trim(); // remove leading and trailing whitespace to simplify parsing

        var pos = 0;
        var parenDepth = 0;
        var state = new ParserState( filter, [], ref pos, ref parenDepth, Operator.NonOperator, EndLine );

        var expression = Parse( ref state, descriptor );

        return FilterTruthyExpression.IsTruthyExpression( expression );
    }

    internal static Expression Parse( ref ParserState state, ITypeDescriptor<TNode> descriptor ) // recursion entrypoint
    {
        // validate input
        if ( descriptor == null )
            throw new ArgumentNullException( nameof( descriptor ) );

        if ( state.EndOfBuffer )
            throw new NotSupportedException( $"Invalid filter: \"{state.Buffer}\"." );

        // parse the expression
        var items = new List<ExprItem>();

        do
        {
            MoveNext( ref state );
            items.Add( GetExprItem( ref state, descriptor ) ); // will recurse for nested expressions

        } while ( state.IsParsing );

        // check for paren mismatch
        if ( state.EndOfBuffer && state.ParenDepth != 0 )
            throw new NotSupportedException( $"Unbalanced parenthesis in filter: \"{state.Buffer}\"." );

        // merge the expressions
        var baseItem = items[0];
        var index = 1;

        return Merge( in state, baseItem, ref index, items, descriptor );
    }


    private static ExprItem GetExprItem( ref ParserState state, ITypeDescriptor<TNode> descriptor )
    {
        var expressionInfo = new ExpressionInfo();

        if ( NotExpressionFactory.TryGetExpression( ref state, out var expression, ref expressionInfo, descriptor ) )
            return ExprItem( ref state, expression, expressionInfo );

        if ( ParenExpressionFactory.TryGetExpression( ref state, out expression, ref expressionInfo, descriptor ) ) // will recurse.
            return ExprItem( ref state, expression, expressionInfo );

        if ( SelectExpressionFactory.TryGetExpression( ref state, out expression, ref expressionInfo, descriptor ) )
            return ExprItem( ref state, expression, expressionInfo );

        if ( FunctionExpressionFactory.TryGetExpression( ref state, out expression, ref expressionInfo, descriptor ) ) // may recurse for each function argument.
            return ExprItem( ref state, expression, expressionInfo );

        if ( LiteralExpressionFactory.TryGetExpression( ref state, out expression, ref expressionInfo, descriptor ) )
            return ExprItem( ref state, expression, expressionInfo );

        if ( JsonExpressionFactory.TryGetExpression( ref state, out expression, ref expressionInfo, descriptor ) )
            return ExprItem( ref state, expression, expressionInfo );

        throw new NotSupportedException( $"Unsupported literal: {state.Buffer.ToString()}" );

        // Helper method to create an expression item
        static ExprItem ExprItem( ref ParserState state, Expression expression, ExpressionInfo expressionInfo )
        {
            MoveNextOperator( ref state ); // will set state.Operator
            return new ExprItem( expression, state.Operator, expressionInfo );
        }
    }

    private static void MoveNext( ref ParserState state ) // move to the next item
    {
        char? quote = null;

        // remove leading whitespace
        while ( !state.EndOfBuffer && char.IsWhiteSpace( state.Current ) )
            state.Pos++;

        // check for end of buffer
        if ( state.EndOfBuffer )
        {
            state.Operator = Operator.NonOperator;
            state.Item = [];
            return;
        }

        // read next item
        var itemStart = state.Pos;
        int itemEnd;

        while ( true )
        {
            itemEnd = state.Pos; // store before calling NextCharacter 

            NextCharacter( ref state, out var nextChar, ref quote ); // will advance state.Pos

            if ( IsFinished( in state, nextChar ) )
            {
                break;
            }

            if ( state.EndOfBuffer )
            {
                itemEnd = state.Pos; // include the final character
                break;
            }
        }

        state.SetItem( itemStart, itemEnd );

        return;

        // Helper method to determine if item parsing is finished
        static bool IsFinished( in ParserState state, char ch )
        {
            // order of operations matters

            if ( state.BracketDepth != 0 )
                return false;

            if ( state.Operator.IsNonOperator() == false )
                return true;

            return ch == state.Terminal; // terminal character [ '\0' or ',' or ')' ]
        }
    }

    private static void MoveNextOperator( ref ParserState state ) // move to the next operator
    {
        if ( state.Operator.IsLogical() || state.Operator.IsComparison() )
        {
            return;
        }

        if ( !state.IsParsing )
        {
            state.Operator = Operator.NonOperator;
            return;
        }

        char? quoteChar = null;

        while ( !(state.Operator.IsLogical() || state.Operator.IsComparison()) && !state.EndOfBuffer )
        {
            NextCharacter( ref state, out _, ref quoteChar );
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
                quoteChar = null; // Exiting a quoted string
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
                state.ParenDepth++;
                state.Operator = Operator.OpenParen;
                break;
            case ')':
                state.ParenDepth--;
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
                state.Operator = Operator.Token;
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

    private static Expression Merge( in ParserState state, ExprItem left, ref int index, List<ExprItem> items, ITypeDescriptor<TNode> descriptor, bool mergeOneOnly = false )
    {
        if ( items.Count == 1 )
        {
            ThrowIfInvalidComparison( in state, left, null ); // single item, no recursion
        }
        else
        {
            while ( index < items.Count )
            {
                var right = items[index++];

                while ( !CanMergeItems( left, right ) )
                {
                    Merge( in state, right, ref index, items, descriptor, mergeOneOnly: true ); // recursive call - right becomes left
                }

                ThrowIfInvalidComparison( in state, left, right );

                MergeItems( left, right, descriptor );

                if ( mergeOneOnly )
                    return left.Expression;
            }
        }

        return left.Expression;

        // Helper method to determine if two items can be merged
        static bool CanMergeItems( ExprItem left, ExprItem right )
        {
            // "Not" can never be a right side operator
            return right.Operator != Operator.Not && GetPrecedence( left.Operator ) >= GetPrecedence( right.Operator );
        }

        // Helper method to get the priority of an operator
        static int GetPrecedence( Operator type )
        {
            return type switch // higher number means greater precedence
            {
                Operator.Not => 1,
                Operator.Or => 2,
                Operator.And => 3,
                Operator.Equals or
                    Operator.NotEquals or
                    Operator.GreaterThan or
                    Operator.GreaterThanOrEqual or
                    Operator.LessThan or
                    Operator.LessThanOrEqual => 4,
                _ => 0,
            };
        }
    }

    private static void MergeItems( ExprItem left, ExprItem right, ITypeDescriptor<TNode> descriptor )
    {
        left.Expression = BindComparerExpression( descriptor, left.Expression );
        right.Expression = BindComparerExpression( descriptor, right.Expression );

        left.Expression = left.Operator switch
        {
            Operator.Equals => CompareExpression<TNode>.Equal( left.Expression, right.Expression ),
            Operator.NotEquals => CompareExpression<TNode>.NotEqual( left.Expression, right.Expression ),
            Operator.GreaterThan => CompareExpression<TNode>.GreaterThan( left.Expression, right.Expression ),
            Operator.GreaterThanOrEqual => CompareExpression<TNode>.GreaterThanOrEqual( left.Expression, right.Expression ),
            Operator.LessThan => CompareExpression<TNode>.LessThan( left.Expression, right.Expression ),
            Operator.LessThanOrEqual => CompareExpression<TNode>.LessThanOrEqual( left.Expression, right.Expression ),
            Operator.And => CompareExpression<TNode>.And( left.Expression, right.Expression ),
            Operator.Or => CompareExpression<TNode>.Or( left.Expression, right.Expression ),
            Operator.Not => CompareExpression<TNode>.Not( right.Expression ),
            _ => throw new InvalidOperationException( $"Invalid operator {left.Operator}" )
        };

        left.Expression = ConvertBoolToScalarExpression( left.Expression );

        left.Operator = right.Operator;
        left.ExpressionInfo.Kind = ExpressionKind.Merged;

        return;

        static Expression ConvertBoolToScalarExpression( Expression leftExpression )
        {
            // convert bool result to Scalar.True or Scalar.False
            Expression conditionalExpression = Expression.Condition(
                leftExpression,
                Expression.Constant( Scalar.True, typeof( IValueType ) ),
                Expression.Constant( Scalar.False, typeof( IValueType ) )
            );

            return conditionalExpression;
        }

        static Expression BindComparerExpression( ITypeDescriptor<TNode> descriptor, Expression expression )
        {
            // Create an Expression that does:
            //
            // static IValueType BindComparerExpression(ITypeDescriptor<TNode> descriptor, IValueType value)
            // {
            //    value.Comparer = parserContext.Descriptor.Comparer;
            //    return value;
            // }

            if ( expression == null )
                return null;

            var valueVariable = Expression.Variable( typeof( IValueType ), "value" );

            var valueAssign = Expression.Assign(
                valueVariable,
                Expression.Convert( expression, typeof( IValueType ) ) );

            var comparerAssign = Expression.Assign(
                Expression.PropertyOrField( valueVariable, "Comparer" ),
                Expression.Constant( descriptor.Comparer, typeof( IValueTypeComparer ) )
            );

            return Expression.Block(
                [valueVariable],
                valueAssign,
                comparerAssign,
                valueVariable
            );
        }
    }

    // Throw helpers

    private static void ThrowIfInvalidComparison( in ParserState state, ExprItem left, ExprItem right )
    {
        ThrowIfConstantIsNotCompared( in state, left, right );
        ThrowIfFunctionInvalidCompare( in state, left );
    }

    private static void ThrowIfFunctionInvalidCompare( in ParserState state, ExprItem item )
    {
        if ( state.IsArgument )
            return;

        if ( item.ExpressionInfo.Kind != ExpressionKind.Function )
            return;

        if ( (item.ExpressionInfo.FunctionInfo & FilterExtensionInfo.MustCompare) == FilterExtensionInfo.MustCompare &&
             !item.Operator.IsComparison() )
        {
            throw new NotSupportedException( $"Function must compare: {state.Buffer.ToString()}." );
        }

        if ( (item.ExpressionInfo.FunctionInfo & FilterExtensionInfo.MustNotCompare) == FilterExtensionInfo.MustNotCompare &&
             item.Operator.IsComparison() )
        {
            throw new NotSupportedException( $"Function must not compare: {state.Buffer.ToString()}." );
        }
    }

    private static void ThrowIfConstantIsNotCompared( in ParserState state, ExprItem left, ExprItem right )
    {
        if ( state.IsArgument )
            return;

        if ( left.ExpressionInfo.Kind == ExpressionKind.Literal && !left.Operator.IsComparison() )
            throw new NotSupportedException( $"Unsupported literal without comparison: {state.Buffer.ToString()}." );

        if ( right != null && right.ExpressionInfo.Kind == ExpressionKind.Literal && !left.Operator.IsComparison() )
            throw new NotSupportedException( $"Unsupported literal without comparison: {state.Buffer.ToString()}." );
    }

    // ExprItem

    [DebuggerDisplay( "Operator = {Operator}, {ExpressionInfo.Kind}" )]
    private class ExprItem( Expression expression, Operator op, ExpressionInfo expressionInfo )
    {
        public ExpressionInfo ExpressionInfo { get; } = expressionInfo;
        public Expression Expression { get; set; } = expression;
        public Operator Operator { get; set; } = op;
    }
}
