﻿#region License

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

    internal static bool IsNonSingularQuery( ReadOnlySpan<char> query ) // BF WIP
    {
        bool inSingleQuotes = false;
        bool inDoubleQuotes = false;

        for ( var i = 0; i < query.Length; i++ )
        {
            char current = query[i];

            switch ( current )
            {
                case '\'' when !inDoubleQuotes:
                    inSingleQuotes = !inSingleQuotes;
                    break;
                case '"' when !inSingleQuotes:
                    inDoubleQuotes = !inDoubleQuotes;
                    break;
            }

            if ( inSingleQuotes || inDoubleQuotes )
                continue;

            if ( query[i..].StartsWith( ".*".AsSpan() ) || query[i..].StartsWith( "..".AsSpan() ) ||
                 query[i..].StartsWith( "[".AsSpan() ) || query[i..].StartsWith( "]".AsSpan() ) )
            {
                return true;
            }
        }

        return false;
    }
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
            var prevOp = MoveNext( ref state );
            var exprItem = GetExprItem( ref state, context );

            ThrowIfConstantIsNotCompared( prevOp, exprItem, in state );

            items.Add( exprItem );

        } while ( state.IsParsing );

        // advance to next character for recursive calls.
        if ( !state.EndOfBuffer && state.IsTerminal )
            state.Pos++;

        // merge the expressions
        var baseItem = items[0];
        var index = 1;

        return Merge( baseItem, ref index, items, context.Descriptor );
    }

    private static void ThrowIfConstantIsNotCompared( Operator prevOp, ExprItem exprItem, in ParserState state )
    {
        // unless the expression is an argument, constants must be compared
        if ( !state.IsArgument && exprItem.Expression is ConstantExpression &&
             !IsComparisonOperator( prevOp ) && !IsComparisonOperator( exprItem.Operator ) )
            throw new NotSupportedException( $"Unsupported literal without comparison: {state.Buffer.ToString()}" );

        return;

        static bool IsComparisonOperator( Operator op ) =>
            op == Operator.Equals || op == Operator.NotEquals ||
            op == Operator.GreaterThan || op == Operator.GreaterThanOrEqual ||
            op == Operator.LessThan || op == Operator.LessThanOrEqual;
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

    private static Operator MoveNext( ref ParserState state )
    {
        char? quote = null;
        var prevOp = state.Operator;

        // remove leading whitespace
        while ( !state.EndOfBuffer && char.IsWhiteSpace( state.Current ) )
            state.Pos++;

        // check for end of buffer
        if ( state.EndOfBuffer )
        {
            state.Operator = Operator.EndOfBuffer;
            state.Item = [];
            return prevOp;
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
        return prevOp;

        // Helper method to determine if item parsing is finished
        static bool IsFinished( int count, char ch, Operator op, char terminal )
        {
            // order of operations matters here
            if ( count == 0 && ch == EndArg )
                return false;

            if ( !op.IsNonOperator() && op != Operator.ClosedParen )
                return true;

            if ( ch == terminal || ch == EndArg || ch == EndLine )
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
                left.Expression = ComparerExpressionFactory<TNode>.GetComparand( descriptor.Accessor, left.Expression );
                right.Expression = ComparerExpressionFactory<TNode>.GetComparand( descriptor.Accessor, right.Expression );

                left.Expression = Expression.Equal( left.Expression, right.Expression );
                break;
            case Operator.NotEquals:
                left.Expression = ComparerExpressionFactory<TNode>.GetComparand( descriptor.Accessor, left.Expression );
                right.Expression = ComparerExpressionFactory<TNode>.GetComparand( descriptor.Accessor, right.Expression );

                left.Expression = Expression.NotEqual( left.Expression, right.Expression );
                break;
            case Operator.GreaterThan:
                left.Expression = ComparerExpressionFactory<TNode>.GetComparand( descriptor.Accessor, left.Expression );
                right.Expression = ComparerExpressionFactory<TNode>.GetComparand( descriptor.Accessor, right.Expression );

                left.Expression = Expression.GreaterThan( left.Expression, right.Expression );
                break;
            case Operator.GreaterThanOrEqual:
                left.Expression = ComparerExpressionFactory<TNode>.GetComparand( descriptor.Accessor, left.Expression );
                right.Expression = ComparerExpressionFactory<TNode>.GetComparand( descriptor.Accessor, right.Expression );

                left.Expression = Expression.GreaterThanOrEqual( left.Expression, right.Expression );
                break;
            case Operator.LessThan:
                left.Expression = ComparerExpressionFactory<TNode>.GetComparand( descriptor.Accessor, left.Expression );
                right.Expression = ComparerExpressionFactory<TNode>.GetComparand( descriptor.Accessor, right.Expression );

                left.Expression = Expression.LessThan( left.Expression, right.Expression );
                break;
            case Operator.LessThanOrEqual:
                left.Expression = ComparerExpressionFactory<TNode>.GetComparand( descriptor.Accessor, left.Expression );
                right.Expression = ComparerExpressionFactory<TNode>.GetComparand( descriptor.Accessor, right.Expression );

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
    }


    public static string Unescape( ReadOnlySpan<char> span)
    {
        // Estimate the maximum length of the unescaped string
        int maxLength = span.Length;

        // Use stackalloc for the temporary destination span if the length is small enough
        Span<char> destination = maxLength <= 256 ? stackalloc char[maxLength] : new char[maxLength];
        int written = 0;

        for (int i = 0; i < span.Length; i++)
        {
            if (span[i] == '\\' && i + 1 < span.Length)
            {
                // Handle escaping
                i++;
                switch (span[i])
                {
                    case '"':
                    case '\\':
                    case '/':
                        destination[written++] = span[i];
                        break;
                    case 'b':
                        destination[written++] = '\b';
                        break;
                    case 'f':
                        destination[written++] = '\f';
                        break;
                    case 'n':
                        destination[written++] = '\n';
                        break;
                    case 'r':
                        destination[written++] = '\r';
                        break;
                    case 't':
                        destination[written++] = '\t';
                        break;
                    case 'u' when i + 4 < span.Length && IsHexDigit(span[i + 1]) && IsHexDigit(span[i + 2]) && IsHexDigit(span[i + 3]) && IsHexDigit(span[i + 4]):
                        destination[written++] = (char)Convert.ToInt32(span.Slice(i + 1, 4).ToString(), 16);
                        i += 4;
                        break;
                    default:
                        // If not a recognized escape sequence, treat as literal
                        destination[written++] = '\\';
                        destination[written++] = span[i];
                        break;
                }
            }
            else
            {
                destination[written++] = span[i];
            }
        }

        return new string(destination[..written]);

        static bool IsHexDigit( char c )
        {
            return (c >= '0' && c <= '9') ||
                   (c >= 'A' && c <= 'F') ||
                   (c >= 'a' && c <= 'f');
        }

    }

    private class ExprItem( Expression expression, Operator op )
    {
        public Expression Expression { get; set; } = expression;
        public Operator Operator { get; set; } = op;
    }
}
