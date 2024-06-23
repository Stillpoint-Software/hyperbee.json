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
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser;

public class FilterParser
{
    public const char EndLine = '\n';
    public const char EndArg = ')';
    public const char ArgSeparator = ',';

    private static readonly MethodInfo ObjectEquals = typeof( object ).GetMethod( "Equals", [typeof( object ), typeof( object )] );

    public static Func<TNode, TNode, bool> Compile<TNode>( ReadOnlySpan<char> filter, ITypeDescriptor typeDescriptor )
    {
        var currentParam = Expression.Parameter( typeof( TNode ) );
        var rootParam = Expression.Parameter( typeof( TNode ) );
        var executionContext = new FilterExecutionContext( currentParam, rootParam, typeDescriptor );
        var expression = Parse( filter, executionContext );

        return Expression.Lambda<Func<TNode, TNode, bool>>( expression, currentParam, rootParam ).Compile();
    }

    internal static Expression Parse( ReadOnlySpan<char> buffer, FilterExecutionContext executionContext )
    {
        var from = 0;
        var expression = Parse( buffer, ref from, EndLine, executionContext );

        return FilterTruthyExpression.IsTruthyExpression( expression );
    }

    internal static Expression Parse( ReadOnlySpan<char> buffer, ref int pos, char terminal, FilterExecutionContext executionContext )
    {
        if ( executionContext == null )
            throw new ArgumentNullException( nameof( executionContext ) );

        if ( pos >= buffer.Length || buffer[pos] == terminal )
            throw new ArgumentException( "Invalid filter", nameof( buffer ) );

        var tokens = new List<FilterToken>( 8 );

        while ( pos < buffer.Length && buffer[pos] != terminal )
        {
            var tokenSpan = GetNextTokenSpan( buffer, ref pos, terminal, out var tokenType );

            switch ( tokenType )
            {
                case FilterTokenType.Not:
                    {
                        tokens.Add( new FilterToken( null, FilterTokenType.Not ) );
                        break;
                    }
                case FilterTokenType.OpenParen when tokenSpan.IsEmpty:
                    {
                        var expr = Parse( buffer, ref pos, EndArg, executionContext );
                        var nextType = GetNextTokenType( tokenType, buffer, ref pos, terminal );
                        tokens.Add( new FilterToken( expr, nextType ) );
                        break;
                    }
                default:
                    {
                        if ( !TryGetFunctionExpression( buffer, tokenSpan, ref pos, out var expr, executionContext ) )
                            expr = GetLiteralExpression( tokenSpan );

                        var nextType = GetNextTokenType( tokenType, buffer, ref pos, terminal );
                        tokens.Add( new FilterToken( expr, nextType ) );
                        break;
                    }
            }
        }

        // Advance to next non-whitespace for recursive calls.
        if ( pos < buffer.Length && (buffer[pos] == EndArg || buffer[pos] == terminal) )
        {
            do
                pos++;
            while ( pos < buffer.Length && char.IsWhiteSpace( buffer[pos] ) );
        }

        var baseToken = tokens[0];
        var index = 1;

        return Merge( baseToken, ref index, tokens, executionContext.Descriptor );
    }

    private static ReadOnlySpan<char> GetNextTokenSpan( ReadOnlySpan<char> buffer, ref int pos, char terminal, out FilterTokenType tokenType )
    {
        char? quote = null;

        // remove leading whitespace
        while ( pos < buffer.Length && char.IsWhiteSpace( buffer[pos] ) )
            pos++;

        // check for end of filter
        if ( pos >= buffer.Length )
        {
            tokenType = FilterTokenType.Unassigned;
            return [];
        }

        var tokenStart = pos;
        int tokenEnd;

        while ( true )
        {
            tokenEnd = pos; // assign before the call to Next

            Next( buffer, ref pos, ref quote, out var nextChar, out tokenType );

            if ( IsFinishedCollecting( pos - tokenStart, nextChar, tokenType, terminal ) )
                break;

            if ( pos < buffer.Length && buffer[pos] != terminal )
                continue;

            tokenEnd = pos; // include the terminal character
            break;
        }

        var value = buffer[tokenStart..tokenEnd].TrimEnd();
        return value;

        static bool IsFinishedCollecting( int count, char ch, FilterTokenType tokenType, char terminal )
        {
            // Order of operations matters here
            if ( count == 0 && ch == EndArg )
                return false;

            if ( tokenType != FilterTokenType.Unassigned && tokenType != FilterTokenType.ClosedParen )
                return true;

            if ( ch == terminal || ch == EndArg || ch == EndLine )
                return true;

            return false;
        }
    }

    private static FilterTokenType GetNextTokenType( FilterTokenType tokenType, ReadOnlySpan<char> buffer, ref int pos, char terminal )
    {
        if ( !IsParenOrUnassigned( tokenType ) )
            return tokenType;

        var nextType = tokenType;

        if ( pos >= buffer.Length || buffer[pos] == EndArg || buffer[pos] == terminal )
            return FilterTokenType.ClosedParen;

        var index = pos;
        char? quote = null;

        while ( IsParenOrUnassigned( nextType ) && index < buffer.Length )
        {
            Next( buffer, ref index, ref quote, out _, out nextType );
        }

        pos = !IsParenOrUnassigned( nextType ) ? index : index > pos ? index - 1 : pos;

        return nextType;

        static bool IsParenOrUnassigned( FilterTokenType tokenType )
        {
            return tokenType is FilterTokenType.Unassigned or FilterTokenType.OpenParen or FilterTokenType.ClosedParen;
        }
    }

    private static void Next( ReadOnlySpan<char> buffer, ref int pos, ref char? quoteChar, out char nextChar, out FilterTokenType tokenType )
    {
        nextChar = buffer[pos++];

        switch ( nextChar )
        {
            case '&' when NextCharacter( buffer, pos, '&' ):
                pos++;
                tokenType = FilterTokenType.And;
                break;
            case '|' when NextCharacter( buffer, pos, '|' ):
                pos++;
                tokenType = FilterTokenType.Or;
                break;
            case '=' when NextCharacter( buffer, pos, '=' ):
                pos++;
                tokenType = FilterTokenType.Equals;
                break;
            case '!' when NextCharacter( buffer, pos, '=' ):
                pos++;
                tokenType = FilterTokenType.NotEquals;
                break;
            case '>' when NextCharacter( buffer, pos, '=' ):
                pos++;
                tokenType = FilterTokenType.GreaterThanOrEqual;
                break;
            case '<' when NextCharacter( buffer, pos, '=' ):
                pos++;
                tokenType = FilterTokenType.LessThanOrEqual;
                break;
            case '>':
                tokenType = FilterTokenType.GreaterThan;
                break;
            case '<':
                tokenType = FilterTokenType.LessThan;
                break;
            case '!':
                tokenType = FilterTokenType.Not;
                break;
            case '(':
                tokenType = FilterTokenType.OpenParen;
                break;
            case ')':
                tokenType = FilterTokenType.ClosedParen;
                break;
            case ' ' or '\t' when quoteChar == null:
                tokenType = FilterTokenType.Unassigned;
                break;
            case '\'' or '\"' when pos > 0 && buffer[pos - 1] != '\\':
                quoteChar = quoteChar == null ? nextChar : null;
                tokenType = FilterTokenType.Unassigned;
                break;
            default:
                tokenType = FilterTokenType.Unassigned;
                break;
        }

        return;

        static bool NextCharacter( ReadOnlySpan<char> buffer, int pos, char expected )
        {
            return pos < buffer.Length && buffer[pos] == expected;
        }
    }

    private static Expression Merge( FilterToken current, ref int index, List<FilterToken> listToMerge, ITypeDescriptor descriptor, bool mergeOneOnly = false )
    {
        while ( index < listToMerge.Count )
        {
            var next = listToMerge[index++];

            while ( !CanMergeTokens( current, next ) )
            {
                Merge( next, ref index, listToMerge, descriptor, mergeOneOnly: true ); // recursive call
            }

            MergeTokens( current, next, descriptor );

            if ( mergeOneOnly )
                return current.Expression;
        }

        return current.Expression;

        static bool CanMergeTokens( FilterToken left, FilterToken right )
        {
            // "Not" can never be a right side operator
            return right.TokenType != FilterTokenType.Not && GetPriority( left.TokenType ) >= GetPriority( right.TokenType );
        }

        static int GetPriority( FilterTokenType type )
        {
            return type switch
            {
                FilterTokenType.Not => 1,
                FilterTokenType.And or FilterTokenType.Or => 2,
                FilterTokenType.Equals or FilterTokenType.NotEquals or FilterTokenType.GreaterThan or FilterTokenType.GreaterThanOrEqual or FilterTokenType.LessThan or FilterTokenType.LessThanOrEqual => 3,
                _ => 0,
            };
        }
    }

    private static void MergeTokens( FilterToken left, FilterToken right, ITypeDescriptor descriptor )
    {
        // Ensure both expressions are value expressions
        left.Expression = descriptor.GetValueExpression( left.Expression );
        right.Expression = descriptor.GetValueExpression( right.Expression );

        // Determine if we are comparing numerical values so that we can use the correct comparison method
        bool isNumerical = IsNumerical( left.Expression?.Type ) || IsNumerical( right.Expression.Type );

        left.Expression = left.TokenType switch
        {
            FilterTokenType.Equals => CompareConvert( isNumerical ? Expression.Equal : Equal, left.Expression, right.Expression, isNumerical ),
            FilterTokenType.NotEquals => CompareConvert( isNumerical ? Expression.NotEqual : NotEqual, left.Expression, right.Expression, isNumerical ),

            // Assume/force numerical
            FilterTokenType.GreaterThan => CompareConvert( Expression.GreaterThan, left.Expression, right.Expression ),
            FilterTokenType.GreaterThanOrEqual => CompareConvert( Expression.GreaterThanOrEqual, left.Expression, right.Expression ),
            FilterTokenType.LessThan => CompareConvert( Expression.LessThan, left.Expression, right.Expression ),
            FilterTokenType.LessThanOrEqual => CompareConvert( Expression.LessThanOrEqual, left.Expression, right.Expression ),

            FilterTokenType.And => Expression.AndAlso( left.Expression!, right.Expression ),
            FilterTokenType.Or => Expression.OrElse( left.Expression!, right.Expression ),

            FilterTokenType.Not => Expression.Not( right.Expression ),
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

    private static bool TryGetFunctionExpression( ReadOnlySpan<char> buffer, ReadOnlySpan<char> item, ref int pos, out Expression expression, FilterExecutionContext executionContext )
    {
        // select 
        if ( item[0] == '$' || item[0] == '@' )
        {
            expression = executionContext.Descriptor
                .GetSelectFunction()
                .GetExpression( buffer, item, ref pos, executionContext ); // may cause `Select` recursion.
            return true;
        }

        // function
        if ( executionContext.Descriptor.Functions.TryGetCreator( item.ToString(), out var functionCreator ) )
        {
            expression = functionCreator()
                .GetExpression( buffer, item, ref pos, executionContext ); // will recurse for each function argument.
            return true;
        }

        expression = null;
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

        throw new ArgumentException( $"Unsupported literal: {item.ToString()}" );
    }

    private enum FilterTokenType
    {
        Unassigned = 0, // used to represent an unassigned token
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

    private class FilterToken( Expression expression, FilterTokenType tokenType )
    {
        public Expression Expression { get; set; } = expression;
        public FilterTokenType TokenType { get; set; } = tokenType;
    }
}
