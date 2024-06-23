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

        return Expression
            .Lambda<Func<TNode, TNode, bool>>( expression, currentParam, rootParam )
            .Compile();
    }

    internal static Expression Parse( ReadOnlySpan<char> filter, FilterExecutionContext executionContext )
    {
        var start = 0;
        var from = 0;
        var expression = Parse( filter, ref start, ref from, EndLine, executionContext );

        return FilterTruthyExpression.IsTruthyExpression( expression );
    }

    internal static Expression Parse( ReadOnlySpan<char> filter, ref int start, ref int from, char terminal, FilterExecutionContext executionContext )
    {
        if ( executionContext == null )
            throw new ArgumentNullException( nameof( executionContext ) );

        if ( from >= filter.Length || filter[from] == terminal )
            throw new ArgumentException( "Invalid filter", nameof( filter ) );

        // Process tokens
        var tokens = new List<FilterToken>( 8 );

        while ( from < filter.Length && filter[from] != terminal )
        {
            var tokenSpan = GetNextTokenSpan( filter, ref start, ref from, terminal, out var tokenType );

            switch ( tokenType )
            {
                case FilterTokenType.Not: // special case for "!"
                    {
                        tokens.Add( new FilterToken( null, FilterTokenType.Not ) );
                        break;
                    }
                case FilterTokenType.OpenParen when tokenSpan.IsEmpty:
                    {
                        var expr = Parse( filter, ref start, ref from, EndArg, executionContext ); // recurse
                        var nextType = GetNextTokenType( tokenType, filter, ref start, ref from, terminal );
                        tokens.Add( new FilterToken( expr, nextType ) );
                        break;
                    }
                default:
                    {
                        if ( !TryGetFunctionExpression( filter, tokenSpan, ref start, ref from, out var expr, executionContext ) ) // may recurse
                            expr = GetLiteralExpression( tokenSpan );

                        var nextType = GetNextTokenType( tokenType, filter, ref start, ref from, terminal );
                        tokens.Add( new FilterToken( expr, nextType ) );
                        break;
                    }
            }
        }

        // This is needed for recursive calls: advance to next non-whitespace.
        if ( from < filter.Length && (filter[from] == EndArg || filter[from] == terminal) )
        {
            do // skip whitespace
                from++;
            while ( from < filter.Length && char.IsWhiteSpace( filter[from] ) );

            start = from;
        }

        var baseToken = tokens[0];
        var index = 1;

        return Merge( baseToken, ref index, tokens, executionContext.Descriptor );
    }

    private static ReadOnlySpan<char> GetNextTokenSpan( ReadOnlySpan<char> filter, ref int start, ref int from, char terminal, out FilterTokenType tokenType )
    {
        char? quote = null;

        // skip leading whitespace
        while ( from < filter.Length && char.IsWhiteSpace( filter[from] ) )
        {
            start++;
            from++;
        }

        // check for end of filter
        if ( from >= filter.Length )
        {
            tokenType = FilterTokenType.Unassigned;
            return [];
        }

        var tokenStart = from;
        int tokenFrom;

        while ( true )
        {
            tokenFrom = from; // assign before the call to Next

            Next( filter, ref start, ref from, ref quote, out var nextChar, out tokenType );

            if ( IsFinishedCollecting( from - tokenStart, nextChar, tokenType, terminal ) )
                break;

            if ( from < filter.Length && filter[from] != terminal )
                continue;

            tokenFrom = from; // include the terminal character
            break;
        }

        // Update start to reflect the current position
        start = from;
        var value = filter[tokenStart..tokenFrom].TrimEnd();
        return value;

        static bool IsFinishedCollecting( int collected, char ch, FilterTokenType tokenType, char terminal )
        {
            // order of operations matters here
            if ( collected == 0 && ch == EndArg )
                return false;

            if ( tokenType != FilterTokenType.Unassigned && tokenType != FilterTokenType.ClosedParen )
                return true;

            if ( ch == terminal || ch == EndArg || ch == EndLine )
                return true;

            return false;
        }
    }

    private static FilterTokenType GetNextTokenType( FilterTokenType tokenType, ReadOnlySpan<char> filter, ref int start, ref int from, char terminal )
    {
        if ( !IsParenOrUnassigned( tokenType ) )
            return tokenType;

        // update

        var nextType = tokenType;

        if ( from >= filter.Length || filter[from] == EndArg || filter[from] == terminal )
            return FilterTokenType.ClosedParen;

        var index = from;
        char? quote = null;

        while ( IsParenOrUnassigned( nextType ) && index < filter.Length )
        {
            Next( filter, ref start, ref index, ref quote, out _, out nextType );
        }

        if ( IsParenOrUnassigned( nextType ) )
            from = index > from ? index - 1 : from;
        else
            from = index;

        return nextType;

        static bool IsParenOrUnassigned( FilterTokenType tokenType )
        {
            return tokenType is FilterTokenType.Unassigned or FilterTokenType.OpenParen or FilterTokenType.ClosedParen;
        }
    }

    private static void Next( ReadOnlySpan<char> data, ref int start, ref int from, ref char? quoteChar, out char nextChar, out FilterTokenType tokenType )
    {
        nextChar = data[from++];

        switch ( nextChar )
        {
            case '&' when ValidNextCharacter( data, from, '&' ):
                start = ++from;
                tokenType = FilterTokenType.And;
                break;
            case '|' when ValidNextCharacter( data, from, '|' ):
                start = ++from;
                tokenType = FilterTokenType.Or;
                break;
            case '=' when ValidNextCharacter( data, from, '=' ):
                start = ++from;
                tokenType = FilterTokenType.Equals;
                break;
            case '!' when ValidNextCharacter( data, from, '=' ):
                start = ++from;
                tokenType = FilterTokenType.NotEquals;
                break;
            case '>' when ValidNextCharacter( data, from, '=' ):
                start = ++from;
                tokenType = FilterTokenType.GreaterThanOrEqual;
                break;
            case '<' when ValidNextCharacter( data, from, '=' ):
                start = ++from;
                tokenType = FilterTokenType.LessThanOrEqual;
                break;
            case '>':
                start = from;
                tokenType = FilterTokenType.GreaterThan;
                break;
            case '<':
                start = from;
                tokenType = FilterTokenType.LessThan;
                break;
            case '!':
                start = from;
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
            case '\'' or '\"' when from > 0 && data[from - 1] != '\\':
                quoteChar = quoteChar == null ? nextChar : null;
                tokenType = FilterTokenType.Unassigned;
                break;
            default:
                tokenType = FilterTokenType.Unassigned;
                break;
        }

        return;

        static bool ValidNextCharacter( ReadOnlySpan<char> data, int from, char expected )
        {
            return from < data.Length && data[from] == expected;
        }
    }

    private static Expression Merge( FilterToken current, ref int index, List<FilterToken> listToMerge, ITypeDescriptor descriptor, bool mergeOneOnly = false )
    {
        while ( index < listToMerge.Count )
        {
            var next = listToMerge[index++];

            while ( !CanMergeTokens( current, next ) )
            {
                // Merge next token with the following token
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
            return right.Type != FilterTokenType.Not && GetPriority( left.Type ) >= GetPriority( right.Type );
        }

        static int GetPriority( FilterTokenType type )
        {
            return type switch
            {
                FilterTokenType.Not => 1,
                FilterTokenType.And or
                    FilterTokenType.Or => 2,
                FilterTokenType.Equals or
                    FilterTokenType.NotEquals or
                    FilterTokenType.GreaterThan or
                    FilterTokenType.GreaterThanOrEqual or
                    FilterTokenType.LessThan or
                    FilterTokenType.LessThanOrEqual => 3,
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

        left.Expression = left.Type switch
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

        left.Type = right.Type;
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

    private static bool TryGetFunctionExpression( ReadOnlySpan<char> filter, ReadOnlySpan<char> item, ref int start, ref int from, out Expression expression, FilterExecutionContext executionContext )
    {
        // select 
        if ( item[0] == '$' || item[0] == '@' )
        {
            expression = executionContext.Descriptor
                .GetSelectFunction()
                .GetExpression( filter, item, ref start, ref from, executionContext ); // may cause `Select` recursion.
            return true;
        }

        // function
        if ( executionContext.Descriptor.Functions.TryGetCreator( item.ToString(), out var functionCreator ) )
        {
            expression = functionCreator()
                .GetExpression( filter, item, ref start, ref from, executionContext ); // recursive call(s) to `Parse` for arguments.
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

    private record FilterToken( Expression Expression, FilterTokenType Type );
    //{
    //    public Expression Expression { get; set; } = Expression;
    //    public FilterTokenType Type { get; set; } = Type;
    //}
}
