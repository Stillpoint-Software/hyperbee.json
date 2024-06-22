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

public class FilterExpressionParser
{
    public const char EndLine = '\n';
    public const char EndArg = ')';
    public const char ArgSeparator = ',';

    private static readonly MethodInfo ObjectEquals = typeof( object ).GetMethod( "Equals", [typeof( object ), typeof( object )] );

    public static Func<TNode, TNode, bool> Compile<TNode>( ReadOnlySpan<char> filter, ITypeDescriptor typeDescriptor )
    {
        var currentParam = Expression.Parameter( typeof( TNode ) );
        var rootParam = Expression.Parameter( typeof( TNode ) );
        var expressionContext = new ParseExpressionContext( currentParam, rootParam, typeDescriptor );
        var expression = Parse( filter, expressionContext );

        return Expression
            .Lambda<Func<TNode, TNode, bool>>( expression, currentParam, rootParam )
            .Compile();
    }

    public static Expression Parse( ReadOnlySpan<char> filter, ParseExpressionContext context )
    {
        var start = 0;
        var from = 0;
        var expression = Parse( filter, ref start, ref from, EndLine, context );

        return FilterTruthyExpression.IsTruthyExpression( expression );
    }

    internal static Expression Parse( ReadOnlySpan<char> filter, ref int start, ref int from, char to = EndLine, ParseExpressionContext context = null )
    {
        if ( from >= filter.Length || filter[from] == to )
        {
            throw new ArgumentException( "Invalid filter", nameof( filter ) );
        }

        var tokens = new List<FilterToken>();
        ReadOnlySpan<char> currentPath = null;
        char? quote = null;

        do
        {
            Next( filter, ref start, ref from, ref quote, out var result );

            var (ch, type) = result;

            // special handling for "!"
            if ( type == FilterTokenType.Not )
            {
                tokens.Add( new FilterToken( null, type!.Value ) );
                continue;
            }

            if ( StillCollecting( currentPath, ch, type, to ) )
            {
                currentPath = filter[start..from].Trim();

                if ( from < filter.Length && filter[from] != to )
                    continue;
            }

            start = from;

            // get Expression for current path
            // `ParseFilterFunction` may result in recursively call to `Parse` for nested expressions

            var expression = ParseFilterFunction( filter, currentPath, ref start, ref from, type, context );

            var filterType = ValidType( type )
                ? type!.Value
                : UpdateType( filter, ref start, ref from, type, to );

            tokens.Add( new FilterToken( expression, filterType ) );

            currentPath = null;

        } while ( from < filter.Length && filter[from] != to );

        if ( from < filter.Length && (filter[from] == EndArg || filter[from] == to) )
        {
            // This happens when called recursively: move one char forward.
            from++;
            start = from;
        }

        var baseToken = tokens[0];
        var index = 1;

        return Merge( baseToken, ref index, tokens, context );
    }

    internal static Expression ParseFilterFunction( ReadOnlySpan<char> filter, ReadOnlySpan<char> currentPath, ref int start, ref int from, FilterTokenType? type, ParseExpressionContext context )
    {
        // Call to `GetExpression` may recursively call `Parse`

        var function = GetFunction( currentPath, type, context.Descriptor );
        return function.GetExpression( filter, currentPath, ref start, ref from, context );

        // Parse filter function based on current path and type

        static FilterFunction GetFunction( ReadOnlySpan<char> currentPath, FilterTokenType? type, ITypeDescriptor descriptor )
        {
            if ( currentPath.Length == 0 && type == FilterTokenType.OpenParen )
                return new ParenFunction(); // causes recursion

            if ( currentPath[0] == '$' || currentPath[0] == '@' )
                return descriptor.GetSelectFunction();

            if ( descriptor.Functions.TryGetCreator( currentPath.ToString(), out var creator ) )
                return creator();

            return new LiteralFunction();
        }
    }

    private static void Next( ReadOnlySpan<char> data, ref int start, ref int from, ref char? quote, out (char NextChar, FilterTokenType? Type) result )
    {
        var nextChar = data[from++];
        switch ( nextChar )
        {
            case '&' when ValidNextCharacter( data, from, '&' ):
                from++;
                start = from;
                result = (nextChar, FilterTokenType.And);
                break;
            case '|' when ValidNextCharacter( data, from, '|' ):
                from++;
                start = from;
                result = (nextChar, FilterTokenType.Or);
                break;
            case '=' when ValidNextCharacter( data, from, '=' ):
                from++;
                start = from;
                result = (nextChar, FilterTokenType.Equals);
                break;
            case '!' when ValidNextCharacter( data, from, '=' ):
                from++;
                start = from;
                result = (nextChar, FilterTokenType.NotEquals);
                break;
            case '>' when ValidNextCharacter( data, from, '=' ):
                from++;
                start = from;
                result = (nextChar, FilterTokenType.GreaterThanOrEqual);
                break;
            case '<' when ValidNextCharacter( data, from, '=' ):
                from++;
                start = from;
                result = (nextChar, FilterTokenType.LessThanOrEqual);
                break;
            case '>':
                start = from;
                result = (nextChar, FilterTokenType.GreaterThan);
                break;
            case '<':
                start = from;
                result = (nextChar, FilterTokenType.LessThan);
                break;
            case '!':
                start = from;
                result = (nextChar, FilterTokenType.Not);
                break;
            case '(':
                result = (nextChar, FilterTokenType.OpenParen);
                break;
            case ')':
                result = (nextChar, FilterTokenType.ClosedParen);
                break;
            case ' ' or '\t' when quote == null:
                result = (nextChar, null);
                break;
            case '\'' or '\"' when from > 0 && data[from - 1] != '\\':
                quote = quote == null ? nextChar : null;
                result = (nextChar, null);
                break;
            default:
                result = (nextChar, null);
                break;
        }
    }

    private static bool ValidNextCharacter( ReadOnlySpan<char> data, int from, char expected )
    {
        return from < data.Length && data[from] == expected;
    }

    private static bool StillCollecting( ReadOnlySpan<char> item, char ch, FilterTokenType? type, char to )
    {
        var stopCollecting = to is EndArg or EndLine
            ? EndArg
            : to;

        if ( item.Length == 0 && ch == EndArg )
            return true;

        if ( ValidType( type ) || ch == stopCollecting )
            return false;

        return type != FilterTokenType.OpenParen;
    }

    private static bool ValidType( FilterTokenType? type )
    {
        return type is FilterTokenType.Not or
            FilterTokenType.Equals or
            FilterTokenType.NotEquals or
            FilterTokenType.GreaterThanOrEqual or
            FilterTokenType.GreaterThan or
            FilterTokenType.LessThanOrEqual or
            FilterTokenType.LessThan or
            FilterTokenType.Or or
            FilterTokenType.And;
    }

    private static FilterTokenType UpdateType( ReadOnlySpan<char> item, ref int start, ref int from, FilterTokenType? type, char to )
    {
        var startType = type;

        if ( from >= item.Length || item[from] == EndArg || item[from] == to )
            return FilterTokenType.ClosedParen;

        var index = from;
        char? quote = null;

        while ( !ValidType( startType ) && index < item.Length )
        {
            Next( item, ref start, ref index, ref quote, out var result );
            startType = result.Type;
        }

        from = ValidType( startType ) ? index
            : index > from ? index - 1
            : from;

        return startType!.Value;
    }

    private static Expression Merge( FilterToken current, ref int index, List<FilterToken> listToMerge, ParseExpressionContext context, bool mergeOneOnly = false )
    {
        while ( index < listToMerge.Count )
        {
            var next = listToMerge[index++];

            while ( !CanMergeTokens( current, next ) )
            {
                Merge( next, ref index, listToMerge, context, mergeOneOnly: true );
            }

            MergeTokens( current, next, context );

            if ( mergeOneOnly )
            {
                return current.Expression;
            }
        }
        return current.Expression;
    }

    private static bool CanMergeTokens( FilterToken left, FilterToken right )
    {
        // "Not" can never be a right side operator
        return right.Type != FilterTokenType.Not && GetPriority( left.Type ) >= GetPriority( right.Type );
    }

    private static int GetPriority( FilterTokenType type )
    {
        return type switch
        {
            FilterTokenType.Not => 1,
            FilterTokenType.And or FilterTokenType.Or => 2,
            FilterTokenType.Equals or FilterTokenType.NotEquals or FilterTokenType.GreaterThan or FilterTokenType.GreaterThanOrEqual or FilterTokenType.LessThan or FilterTokenType.LessThanOrEqual => 3,
            _ => 0,
        };
    }

    private static void MergeTokens( FilterToken left, FilterToken right, ParseExpressionContext context )
    {
        // Ensure both expressions are value expressions
        left.Expression = context.Descriptor.GetValueExpression( left.Expression );
        right.Expression = context.Descriptor.GetValueExpression( right.Expression );

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

    private static bool IsNumerical( Type type )
    {
        return type == typeof( int ) || type == typeof( float );
    }

    internal enum FilterTokenType
    {
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

    internal class FilterToken( Expression expression, FilterTokenType type )
    {
        public Expression Expression { get; set; } = expression;
        public FilterTokenType Type { get; set; } = type;
    }
}
