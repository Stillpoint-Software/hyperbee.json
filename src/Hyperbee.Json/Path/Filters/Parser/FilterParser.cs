#region License

// This code is adapted from an algorithm published in MSDN Magazine, October 2015.
// Original article: "A Split-and-Merge Expression Parser in C#" by Vassili Kaplan.
// URL: https://learn.microsoft.com/en-us/archive/msdn-magazine/2015/october/csharp-a-split-and-merge-expression-parser-in-csharp
//  
// Adapted for use in this project under the terms of the Microsoft Public License (Ms-PL).
// https://opensource.org/license/ms-pl-html

#endregion

using System.Buffers;
using System.Diagnostics;
using System.Linq.Expressions;
using Hyperbee.Expressions.Compiler;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Path.Filters.Parser.Expressions;
using Hyperbee.Json.Path.Filters.Values;

namespace Hyperbee.Json.Path.Filters.Parser;

public abstract class FilterParser
{
    public const char EndLine = '\0'; // use null instead of newline
    public const char ArgClose = ')';
    public const char ArgComma = ',';
}

public class FilterParser<TNode> : FilterParser
{
    internal static readonly ITypeDescriptor<TNode> Descriptor =
        JsonTypeDescriptorRegistry.GetDescriptor<TNode>();

    internal static readonly ParameterExpression RuntimeContextExpression =
        Expression.Parameter( typeof( FilterRuntimeContext<TNode> ), "runtimeContext" ); // must use a common instance

    public static Func<FilterRuntimeContext<TNode>, bool> Compile( ReadOnlySpan<char> filter )
    {
        var expression = Parse( filter );
        var lambda = Expression.Lambda<Func<FilterRuntimeContext<TNode>, bool>>( expression, RuntimeContextExpression );
        return HyperbeeCompiler.Compile( lambda );
    }

    internal static Expression Parse( ReadOnlySpan<char> filter )
    {
        filter = filter.Trim(); // remove leading and trailing whitespace simplifies parsing

        var pos = 0;
        var parenDepth = 0;
        var state = new ParserState( filter, [], ref pos, ref parenDepth, Operator.NonOperator, EndLine );

        var expression = Parse( ref state );

        return TruthyExpression.IsTruthyExpression( expression );
    }

    internal static Expression Parse( ref ParserState state ) // recursion entrypoint
    {
        if ( state.EndOfBuffer )
            throw new NotSupportedException( $"Invalid filter: \"{state.Buffer}\"." );

        // parse the expression
        var pool = ArrayPool<ExprItem>.Shared;
        var items = pool.Rent( 8 );
        var count = 0;

        try
        {
            do
            {
                MoveNext( ref state );

                if ( count == items.Length )
                {
                    var bigger = pool.Rent( items.Length * 2 );
                    Array.Copy( items, bigger, count );
                    pool.Return( items, clearArray: true );
                    items = bigger;
                }

                items[count++] = GetExprItem( ref state ); // may cause recursion
            } while ( state.IsParsing );

            // check for paren mismatch
            if ( state.EndOfBuffer && state.ParenDepth != 0 )
                throw new NotSupportedException( $"Unbalanced parenthesis in filter: \"{state.Buffer}\"." );

            // merge the expressions
            var head = 1;
            return Merge( in state, ref items[0], items, count, ref head );
        }
        finally
        {
            pool.Return( items, clearArray: true );
        }
    }

    private static ExprItem GetExprItem( ref ParserState state )
    {
        switch ( true )
        {
            case true when NotExpressionFactory.TryGetExpression<TNode>( ref state, out var expression, out var compareConstraint ):
            case true when ParenExpressionFactory.TryGetExpression<TNode>( ref state, out expression, out compareConstraint ):
            case true when SelectExpressionFactory.TryGetExpression<TNode>( ref state, out expression, out compareConstraint ):
            case true when FunctionExpressionFactory.TryGetExpression( ref state, out expression, out compareConstraint, Descriptor ):
            case true when LiteralExpressionFactory.TryGetExpression<TNode>( ref state, out expression, out compareConstraint ):
            case true when JsonExpressionFactory.TryGetExpression( ref state, out expression, out compareConstraint, Descriptor ):
                MoveNextOperator( ref state );
                return new ExprItem( expression, state.Operator, compareConstraint );
            default:
                throw new NotSupportedException( $"Unsupported operator: {state.Operator}." );
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
            itemEnd = state.Pos; // save Pos before calling NextCharacter 

            NextCharacter( ref state, itemStart, out var nextChar, ref quote ); // will advance state.Pos

            if ( IsFinished( in state, nextChar, ref itemEnd ) )
            {
                break;
            }
        }

        state.SetItem( itemStart, itemEnd );

        return;

        static bool IsFinished( in ParserState state, char ch, ref int itemEnd )
        {
            // order of operations matters

            bool result = true switch
            {
                true when state.BracketDepth != 0 => false,
                true when !state.Operator.IsNonOperator() => true,
                true when ch == state.TerminalCharacter => true, // [ '\0' or ',' or ')' ]
                _ => false
            };

            if ( result || !state.EndOfBuffer )
                return result;

            // not finished, but at end-of-buffer
            itemEnd = state.Pos;
            return true;
        }
    }

    private static void MoveNextOperator( ref ParserState state ) // move to the next operator
    {
        if ( state.Operator.IsLogical() || state.Operator.IsComparison() || state.Operator.IsMath() )
            return;

        // Determine if we should stop looking for an operator.
        //
        // When IsParsing is false (Previous == TerminalCharacter), we've hit a potential stopping point.
        // However, ')' as a terminal character is ambiguous - it could be:
        //   1. A function's closing paren: `length(@.x)` - should continue to find `> 10` in `(length(@.x) > 10)`
        //   2. The outer expression's closing paren - should stop
        //   3. A function argument's closing paren - should stop
        //
        // We return early (stop) when:
        //   - TerminalCharacter is not ')' (e.g., ',' is unambiguous), OR
        //   - ParenDepth == 0 (we're at the outermost level, so ')' closes the expression), OR
        //   - IsArgument is true (we're parsing a function argument, so ')' is definitely ours)
        //
        // Otherwise, we fall through to the while loop to continue scanning for operators.

        if ( !state.IsParsing && (state.TerminalCharacter != ArgClose || state.ParenDepth == 0 || state.IsArgument) )
        {
            state.Operator = Operator.NonOperator;
            return;
        }

        char? quoteChar = null;
        var start = state.Pos;

        while ( !(state.Operator.IsLogical() || state.Operator.IsComparison() || state.Operator.IsMath()) && !state.EndOfBuffer )
        {
            NextCharacter( ref state, start, out _, ref quoteChar );
        }
    }

    private static void NextCharacter( ref ParserState state, int start, out char nextChar, ref char? quoteChar )
    {
        // Read next character
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
            case 'i' when Next( ref state, 'n' ):
                state.Operator = IsInOperator( state )
                    ? Operator.In
                    : Operator.Token; // `in` must be surrounded by whitespace
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
            case '+':
                state.Operator = IsAddSubtractOperator( state, start )
                    ? Operator.Add
                    : Operator.Token; // ignore +1 -1 1e+2 1e-2
                break;
            case '-':
                state.Operator = IsAddSubtractOperator( state, start )
                    ? Operator.Subtract
                    : Operator.Token; // ignore +1 -1 1e+2 1e-2
                break;
            case '*':
                state.Operator = IsMultiplyOperator( state, start )
                    ? Operator.Multiply
                    : Operator.Token; // ignore .* [* ,*
                break;
            case '%':
                state.Operator = Operator.Modulus;
                break;
            case '/':
                state.Operator = Operator.Divide;
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

        // Helper method to check if `in` is a valid operator
        static bool IsInOperator( in ParserState state )
        {
            // ` in ` must be surrounded by whitespace

            var span = state.Buffer[(state.Pos - 3)..(state.Pos + 1)];
            return span.Length == 4 && char.IsWhiteSpace( span[0] ) && char.IsWhiteSpace( span[^1] );
        }

        // Helper method to check if the operator is a valid add or subtract operator
        static bool IsAddSubtractOperator( in ParserState state, int start )
        {
            // exclude +1 -1 1e+2 1e-2 .1

            var span = state.Buffer[start..state.Pos];

            return !span.IsEmpty &&
                   span[0] != '+' && span[0] != '-' && span[0] != '.' &&
                   span.Length >= 2 && span[^2] != 'e' && span[^2] != 'E';
        }

        // Helper method to check if the operator is a valid multiply operator
        static bool IsMultiplyOperator( in ParserState state, int start )
        {
            // exclude `.*` and `,*` and `[*`

            var span = state.Buffer[start..(state.Pos - 1)];

            for ( var i = span.Length - 1; i >= 0; i-- )
            {
                var c = span[i];
                if ( char.IsWhiteSpace( c ) )
                    continue;

                if ( c == '[' || c == '.' || c == ',' )
                    return false;

                break;
            }

            return true;
        }
    }

    private static Expression Merge( in ParserState state, ref ExprItem left, ExprItem[] items, int count, ref int head, bool mergeOneOnly = false )
    {
        if ( head >= count )
        {
            ThrowIfInvalidCompare( in state, in left ); // single item, no recursion
        }
        else
        {
            while ( head < count )
            {
                ref var right = ref items[head++];

                while ( !CanMergeItems( in left, in right ) )
                {
                    Merge( in state, ref right, items, count, ref head, mergeOneOnly: true ); // recursive call - right becomes left
                }

                ThrowIfInvalidCompare( in state, in left, in right );

                MergeItems( ref left, in right );

                if ( mergeOneOnly )
                    return left.Expression;
            }
        }

        return left.Expression;

        // Helper method to determine if two items can be merged
        static bool CanMergeItems( in ExprItem left, in ExprItem right )
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
                    Operator.In or
                    Operator.NotEquals or
                    Operator.GreaterThan or
                    Operator.GreaterThanOrEqual or
                    Operator.LessThan or
                    Operator.LessThanOrEqual => 4,
                Operator.Add or
                    Operator.Subtract => 5,
                Operator.Multiply or
                    Operator.Divide or
                    Operator.Modulus => 6,
                _ => 0,
            };
        }
    }

    private static void MergeItems( ref ExprItem left, in ExprItem right )
    {
        var leftExpr = ConvertOrDefault( left.Expression, typeof( IValueType ) );
        var rightExpr = ConvertOrDefault( right.Expression, typeof( IValueType ) );

        left.Expression = left.Operator switch
        {
            Operator.Equals => CompareExpression<TNode>.Equal( leftExpr, rightExpr ),
            Operator.NotEquals => CompareExpression<TNode>.NotEqual( leftExpr, rightExpr ),
            Operator.GreaterThan => CompareExpression<TNode>.GreaterThan( leftExpr, rightExpr ),
            Operator.GreaterThanOrEqual => CompareExpression<TNode>.GreaterThanOrEqual( leftExpr, rightExpr ),
            Operator.LessThan => CompareExpression<TNode>.LessThan( leftExpr, rightExpr ),
            Operator.LessThanOrEqual => CompareExpression<TNode>.LessThanOrEqual( leftExpr, rightExpr ),

            Operator.And => CompareExpression<TNode>.And( leftExpr, rightExpr ),
            Operator.Or => CompareExpression<TNode>.Or( leftExpr, rightExpr ),
            Operator.Not => CompareExpression<TNode>.Not( rightExpr ),

            Operator.In => CompareExpression<TNode>.In( leftExpr, rightExpr ),

            Operator.Add => MathExpression<TNode>.Add( leftExpr, rightExpr ),
            Operator.Subtract => MathExpression<TNode>.Subtract( leftExpr, rightExpr ),
            Operator.Multiply => MathExpression<TNode>.Multiply( leftExpr, rightExpr ),
            Operator.Divide => MathExpression<TNode>.Divide( leftExpr, rightExpr ),
            Operator.Modulus => MathExpression<TNode>.Modulus( leftExpr, rightExpr ),

            _ => throw new InvalidOperationException( $"Invalid operator {left.Operator}." )
        };

        left.Operator = right.Operator;
        left.CompareConstraint = CompareConstraint.None;

        return;

        static Expression ConvertOrDefault( Expression expression, Type type ) =>
            expression == null ? null : Expression.Convert( expression, type );
    }

    // Throw helpers

    private static void ThrowIfInvalidCompare( in ParserState state, in ExprItem left )
    {
        ThrowIfLeftLiteralInvalidCompare( in state, in left );
        ThrowIfFunctionInvalidCompare( in state, in left );
    }

    private static void ThrowIfInvalidCompare( in ParserState state, in ExprItem left, in ExprItem right )
    {
        ThrowIfLeftLiteralInvalidCompare( in state, in left );
        ThrowIfRightLiteralInvalidCompare( in state, in left, in right );
        ThrowIfFunctionInvalidCompare( in state, in left );
    }

    private static void ThrowIfLeftLiteralInvalidCompare( in ParserState state, in ExprItem left )
    {
        const CompareConstraint literalMustCompare = CompareConstraint.Literal | CompareConstraint.MustCompare;

        if ( state.IsArgument || left.Operator.IsMath() )
            return;

        if ( (left.CompareConstraint & literalMustCompare) == literalMustCompare && !left.Operator.IsComparison() )
            throw new NotSupportedException( $"Unsupported literal without comparison: {state.Buffer.ToString()}." );
    }

    private static void ThrowIfRightLiteralInvalidCompare( in ParserState state, in ExprItem left, in ExprItem right )
    {
        const CompareConstraint literalMustCompare = CompareConstraint.Literal | CompareConstraint.MustCompare;

        if ( state.IsArgument || left.Operator.IsMath() )
            return;

        if ( (right.CompareConstraint & literalMustCompare) == literalMustCompare && !left.Operator.IsComparison() )
            throw new NotSupportedException( $"Unsupported literal without comparison: {state.Buffer.ToString()}." );
    }

    private static void ThrowIfFunctionInvalidCompare( in ParserState state, in ExprItem item )
    {
        const CompareConstraint functionMustCompare = CompareConstraint.Function | CompareConstraint.MustCompare;
        const CompareConstraint functionMustNotCompare = CompareConstraint.Function | CompareConstraint.MustNotCompare;

        if ( state.IsArgument )
            return;

        if ( (item.CompareConstraint & functionMustCompare) == functionMustCompare && !item.Operator.IsComparison() )
            throw new NotSupportedException( $"Function must compare: {state.Buffer.ToString()}." );

        if ( (item.CompareConstraint & functionMustNotCompare) == functionMustNotCompare && item.Operator.IsComparison() )
            throw new NotSupportedException( $"Function must not compare: {state.Buffer.ToString()}." );
    }

    // ExprItem

    [DebuggerDisplay( "{CompareConstraint}, Operator = {Operator}" )]
    private struct ExprItem( Expression expression, Operator op, CompareConstraint compareConstraint )
    {
        public CompareConstraint CompareConstraint = compareConstraint;
        public Expression Expression = expression;
        public Operator Operator = op;
    }
}
