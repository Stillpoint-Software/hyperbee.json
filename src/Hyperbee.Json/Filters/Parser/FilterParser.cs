﻿#region License

// This code is adapted from an algorithm published in MSDN Magazine, October 2015.
// Original article: "A Split-and-Merge Expression Parser in C#" by Vassili Kaplan.
// URL: https://learn.microsoft.com/en-us/archive/msdn-magazine/2015/october/csharp-a-split-and-merge-expression-parser-in-csharp
//  
// Adapted for use in this project under the terms of the Microsoft Public License (Ms-PL).
// https://opensource.org/license/ms-pl-html

#endregion

using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using Hyperbee.Json.Descriptors;

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
        var executionContext = new FilterExecutionContext( currentParam, rootParam, descriptor );

        var expression = Parse( filter, executionContext );

        return Expression.Lambda<Func<TNode, TNode, bool>>( expression, currentParam, rootParam ).Compile();
    }

    internal static Expression Parse( ReadOnlySpan<char> filter, FilterExecutionContext executionContext )
    {
        var from = 0;
        var expression = Parse( filter, ref from, EndLine, executionContext );

        return FilterTruthyExpression.IsTruthyExpression( expression );
    }

    internal static Expression Parse( ReadOnlySpan<char> filter, ref int pos, char terminal, FilterExecutionContext executionContext )
    {
        if ( executionContext == null )
            throw new ArgumentNullException( nameof( executionContext ) );

        if ( pos >= filter.Length || filter[pos] == terminal )
            throw new ArgumentException( "Invalid filter", nameof( filter ) );

        var items = new List<ExprItem>( 8 );

        while ( pos < filter.Length && filter[pos] != terminal )
        {
            var tokenSpan = GetNextToken( filter, ref pos, terminal, out var tokenType );

            // process token

            if ( tokenType == TokenType.Not )
            {
                items.Add( new ExprItem( null, TokenType.Not ) );
                continue;
            }

            if ( TryGetParenExprItem( filter, tokenSpan, ref pos, tokenType, terminal, out var exprItem, executionContext ) )
            {
                items.Add( exprItem );
                continue;
            }

            if ( TryGetSelectExprItem( filter, tokenSpan, ref pos, tokenType, terminal, out exprItem, executionContext ) )
            {
                items.Add( exprItem );
                continue;
            }

            if ( TryGetFunctionExprItem( filter, tokenSpan, ref pos, tokenType, terminal, out exprItem, executionContext ) )
            {
                items.Add( exprItem );
                continue;
            }

            if ( TryGetLiteralExprItem( filter, tokenSpan, ref pos, tokenType, terminal, out exprItem, executionContext ) )
            {
                items.Add( exprItem );
                continue;
            }

            throw new ArgumentException( $"Unsupported literal: {filter.ToString()}" );
        }

        // Advance to next character for recursive calls.
        if ( pos < filter.Length && (filter[pos] == EndArg || filter[pos] == terminal) )
            pos++;

        var baseItem = items[0];
        var index = 1;

        return Merge( baseItem, ref index, items, executionContext.Descriptor );
    }

    private static ReadOnlySpan<char> GetNextToken( ReadOnlySpan<char> filter, ref int pos, char terminal, out TokenType tokenType )
    {
        char? quote = null;

        // remove leading whitespace
        while ( pos < filter.Length && char.IsWhiteSpace( filter[pos] ) )
            pos++;

        // check for end of filter
        if ( pos >= filter.Length )
        {
            tokenType = TokenType.Nop;
            return [];
        }

        var tokenStart = pos;
        int tokenEnd;

        while ( true )
        {
            tokenEnd = pos; // assign before the call to Next

            GetNextCharacter( filter, ref pos, out tokenType, out var nextChar, ref quote );

            if ( IsFinishedCollecting( pos - tokenStart, nextChar, tokenType, terminal ) )
                break;

            if ( pos < filter.Length && filter[pos] != terminal )
                continue;

            tokenEnd = pos; // include the terminal character
            break;
        }

        var value = filter[tokenStart..tokenEnd].TrimEnd();
        return value;

        static bool IsFinishedCollecting( int count, char ch, TokenType tokenType, char terminal )
        {
            // Order of operations matters here
            if ( count == 0 && ch == EndArg )
                return false;

            if ( tokenType != TokenType.Nop && tokenType != TokenType.ClosedParen )
                return true;

            if ( ch == terminal || ch == EndArg || ch == EndLine )
                return true;

            return false;
        }
    }

    private static TokenType GetNextTokenType( TokenType tokenType, ReadOnlySpan<char> filter, ref int pos, char terminal )
    {
        if ( IsValid( tokenType ) )
            return tokenType;

        var nextType = tokenType;

        if ( pos >= filter.Length )
            return TokenType.Nop;

        if ( filter[pos] == EndArg || filter[pos] == terminal )
            return TokenType.ClosedParen;

        char? quoteChar = null;
        var posTmp = pos;
        var eof = false;

        while ( !IsValid( nextType ) && !eof )
        {
            GetNextCharacter( filter, ref posTmp, out nextType, out _, ref quoteChar );

            if ( posTmp >= filter.Length )
                eof = true;
        }

        if ( IsValid( nextType ) || (eof && nextType == TokenType.Nop) ) // Nop here means we are at the end of the filter
        {
            pos = posTmp;
        }
        else if ( posTmp > pos )
        {
            pos = posTmp - 1;
        }

        return nextType;

        static bool IsValid( TokenType tokenType )
        {
            return tokenType is not (TokenType.Nop or TokenType.OpenParen or TokenType.ClosedParen);
        }
    }

    private static void GetNextCharacter( ReadOnlySpan<char> filter, ref int pos, out TokenType tokenType, out char nextChar, ref char? quoteChar )
    {
        nextChar = filter[pos++];

        switch ( nextChar )
        {
            case '&' when NextCharacter( filter, pos, '&' ):
                pos++;
                tokenType = TokenType.And;
                break;
            case '|' when NextCharacter( filter, pos, '|' ):
                pos++;
                tokenType = TokenType.Or;
                break;
            case '=' when NextCharacter( filter, pos, '=' ):
                pos++;
                tokenType = TokenType.Equals;
                break;
            case '!' when NextCharacter( filter, pos, '=' ):
                pos++;
                tokenType = TokenType.NotEquals;
                break;
            case '>' when NextCharacter( filter, pos, '=' ):
                pos++;
                tokenType = TokenType.GreaterThanOrEqual;
                break;
            case '<' when NextCharacter( filter, pos, '=' ):
                pos++;
                tokenType = TokenType.LessThanOrEqual;
                break;
            case '>':
                tokenType = TokenType.GreaterThan;
                break;
            case '<':
                tokenType = TokenType.LessThan;
                break;
            case '!':
                tokenType = TokenType.Not;
                break;
            case '(':
                tokenType = TokenType.OpenParen;
                break;
            case ')':
                tokenType = TokenType.ClosedParen;
                break;
            case ' ' or '\t' when quoteChar == null:
                tokenType = TokenType.Nop;
                break;
            case '\'' or '\"' when pos > 0 && filter[pos - 1] != '\\':
                quoteChar = quoteChar == null ? nextChar : null;
                tokenType = TokenType.Nop;
                break;
            default:
                tokenType = TokenType.Nop;
                break;
        }

        return;

        static bool NextCharacter( ReadOnlySpan<char> filter, int pos, char expected )
        {
            return pos < filter.Length && filter[pos] == expected;
        }
    }

    private static Expression Merge( ExprItem current, ref int index, List<ExprItem> tokens, ITypeDescriptor descriptor, bool mergeOneOnly = false )
    {
        while ( index < tokens.Count )
        {
            var next = tokens[index++];

            while ( !CanMergeTokens( current, next ) )
            {
                Merge( next, ref index, tokens, descriptor, mergeOneOnly: true ); // recursive call
            }

            MergeTokens( current, next, descriptor );

            if ( mergeOneOnly )
                return current.Expression;
        }

        return current.Expression;

        static bool CanMergeTokens( ExprItem left, ExprItem right )
        {
            // "Not" can never be a right side operator
            return right.TokenType != TokenType.Not && GetPriority( left.TokenType ) >= GetPriority( right.TokenType );
        }

        static int GetPriority( TokenType type )
        {
            return type switch
            {
                TokenType.Not => 1,
                TokenType.And or TokenType.Or => 2,
                TokenType.Equals or TokenType.NotEquals or TokenType.GreaterThan or TokenType.GreaterThanOrEqual or TokenType.LessThan or TokenType.LessThanOrEqual => 3,
                _ => 0,
            };
        }
    }

    private static void MergeTokens( ExprItem left, ExprItem right, ITypeDescriptor descriptor )
    {
        // Ensure both expressions are value expressions
        left.Expression = descriptor.GetValueExpression( left.Expression );
        right.Expression = descriptor.GetValueExpression( right.Expression );

        // Determine if we are comparing numerical values so that we can use the correct comparison method
        bool isNumerical = IsNumerical( left.Expression?.Type ) || IsNumerical( right.Expression.Type );

        left.Expression = left.TokenType switch
        {
            TokenType.Equals => CompareConvert( isNumerical ? Expression.Equal : Equal, left.Expression, right.Expression, isNumerical ),
            TokenType.NotEquals => CompareConvert( isNumerical ? Expression.NotEqual : NotEqual, left.Expression, right.Expression, isNumerical ),

            // Assume/force numerical
            TokenType.GreaterThan => CompareConvert( Expression.GreaterThan, left.Expression, right.Expression ),
            TokenType.GreaterThanOrEqual => CompareConvert( Expression.GreaterThanOrEqual, left.Expression, right.Expression ),
            TokenType.LessThan => CompareConvert( Expression.LessThan, left.Expression, right.Expression ),
            TokenType.LessThanOrEqual => CompareConvert( Expression.LessThanOrEqual, left.Expression, right.Expression ),

            TokenType.And => Expression.AndAlso( left.Expression!, right.Expression ),
            TokenType.Or => Expression.OrElse( left.Expression!, right.Expression ),

            TokenType.Not => Expression.Not( right.Expression ),
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

        left.TokenType = right.TokenType;
        return;

        static bool IsNumerical( Type type ) => type == typeof( float ) || type == typeof( int );

        // Use Equal Method vs equal operator
        static Expression Equal( Expression l, Expression r ) => Expression.Call( ObjectEquals, l, r );
        static Expression NotEqual( Expression l, Expression r ) => Expression.Not( Equal( l, r ) );
    }

    private static Expression CompareConvert( Func<Expression, Expression, Expression> compare, Expression left, Expression right, bool isNumerical = true )
    {
        if ( isNumerical )
        {
            if ( left.Type == typeof( object ) )
                left = Expression.Convert( left, typeof( float ) );

            if ( right.Type == typeof( object ) )
                right = Expression.Convert( right, typeof( float ) );

            if ( left.Type == typeof( int ) )
                left = Expression.Convert( left, typeof( float ) );

            if ( right.Type == typeof( int ) )
                right = Expression.Convert( right, typeof( float ) );
        }

        if ( left.Type == typeof( object ) && right.Type == typeof( string ) )
            return compare( Expression.Convert( left, typeof( string ) ), right );

        if ( left.Type == typeof( string ) && right.Type == typeof( object ) )
            return compare( left, Expression.Convert( right, typeof( string ) ) );

        return compare( left, right );
    }

    private static bool TryGetParenExprItem( ReadOnlySpan<char> filter, ReadOnlySpan<char> tokenSpan, ref int pos, TokenType tokenType, char terminal, out ExprItem exprItem, FilterExecutionContext executionContext )
    {
        if ( tokenType == TokenType.OpenParen && tokenSpan.IsEmpty )
        {
            var expression = Parse( filter, ref pos, EndArg, executionContext ); // will recurse.
            var nextType = GetNextTokenType( tokenType, filter, ref pos, terminal );
            exprItem = new ExprItem( expression, nextType );
            return true;
        }

        exprItem = null;
        return false;
    }

    private static bool TryGetLiteralExprItem( ReadOnlySpan<char> filter, ReadOnlySpan<char> tokenSpan, ref int pos, TokenType tokenType, char terminal, out ExprItem exprItem, FilterExecutionContext executionContext )
    {
        var expr = GetLiteralExpression( tokenSpan );

        if ( expr != null )
        {
            var nextType = GetNextTokenType( tokenType, filter, ref pos, terminal );
            exprItem = new ExprItem( expr, nextType );
            return true;
        }

        exprItem = null;
        return false;
    }

    private static ConstantExpression GetLiteralExpression( ReadOnlySpan<char> item )
    {
        // Check for known literals (true, false, null) first

        if ( item.Equals( "true", StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( true );

        if ( item.Equals( "false", StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( false );

        if ( item.Equals( "null", StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( null );

        // Check for quoted strings

        if ( item.Length >= 2 && (item[0] == '"' && item[^1] == '"' || item[0] == '\'' && item[^1] == '\'') )
            return Expression.Constant( item[1..^1].ToString() ); // remove quotes

        // Check for numbers
        // TODO: Currently assuming all numbers are floats since we don't know what's in the data or the other side of the operator yet.

        if ( float.TryParse( item, out float result ) )
            return Expression.Constant( result );

        return null;
    }

    private static bool TryGetSelectExprItem( ReadOnlySpan<char> filter, ReadOnlySpan<char> item, ref int pos, TokenType tokenType, char terminal, out ExprItem exprItem, FilterExecutionContext executionContext )
    {
        if ( item[0] == '$' || item[0] == '@' )
        {
            var expression = executionContext.Descriptor
                .GetSelectFunction()
                .GetExpression( filter, item, ref pos, executionContext ); // may cause `Select` recursion.

            var nextType = GetNextTokenType( tokenType, filter, ref pos, terminal );
            exprItem = new ExprItem( expression, nextType );
            return true;
        }

        exprItem = null;
        return false;
    }

    private static bool TryGetFunctionExprItem( ReadOnlySpan<char> filter, ReadOnlySpan<char> item, ref int pos, TokenType tokenType, char terminal, out ExprItem exprItem, FilterExecutionContext executionContext )
    {
        if ( executionContext.Descriptor.Functions.TryGetCreator( item.ToString(), out var functionCreator ) )
        {
            var expression = functionCreator()
                .GetExpression( filter, item, ref pos, executionContext ); // will recurse for each function argument.

            var nextType = GetNextTokenType( tokenType, filter, ref pos, terminal );
            exprItem = new ExprItem( expression, nextType );
            return true;
        }

        exprItem = null;
        return false;
    }

    private enum TokenType
    {
        Nop = 0, // used to represent an unassigned token
        OpenParen,
        ClosedParen,
        Not,
        Equals,
        NotEquals,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        Or,
        And
    }

    private class ExprItem( Expression expression, TokenType tokenType )
    {
        public Expression Expression { get; set; } = expression;
        public TokenType TokenType { get; set; } = tokenType;
    }
}
