using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Evaluators.Parser.Functions;

namespace Hyperbee.Json.Evaluators.Parser;
// Based off Split-and-Merge Expression Parser
// https://learn.microsoft.com/en-us/archive/msdn-magazine/2015/october/csharp-a-split-and-merge-expression-parser-in-csharp
// Handles `filter-selector` in the https://github.com/ietf-wg-jsonpath/draft-ietf-jsonpath-base/blob/main/sourcecode/abnf/jsonpath-collected.abnf#L69

public class JsonPathExpression
{
    public const char EndLine = '\n';
    public const char EndArg = ')';

    private static readonly char[] ValidPathParts = ['.', '$', '@'];

    private static readonly MethodInfo ObjectEquals = typeof( object ).GetMethod( "Equals", [typeof( object ), typeof( object )] );

    public static Func<TType, TType, string, bool> Compile<TType>( ReadOnlySpan<char> filter, IJsonPathFilterEvaluator<TType> evaluator = null )
    {
        var currentParam = Expression.Parameter( typeof( TType ) );
        var rootParam = Expression.Parameter( typeof( TType ) );
        var basePathParam = Expression.Parameter( typeof( string ) );
        var expressionContext = new ParseExpressionContext<TType>( currentParam, rootParam, evaluator, basePathParam );
        var expression = Parse( filter, expressionContext );

        return Expression
            .Lambda<Func<TType, TType, string, bool>>( expression, currentParam, rootParam, basePathParam )
            .Compile();
    }

    public static Expression Parse<TType>( ReadOnlySpan<char> filter, ParseExpressionContext<TType> context )
    {
        var start = 0;
        var from = 0;
        var expression = Parse( filter, ref start, ref from, EndLine, context );

        return expression.Type == typeof( bool )
            ? expression
            : Expression.Call( JsonPathHelper<TType>.IsTruthyMethod!, expression );
    }

    internal static Expression Parse<TType>( ReadOnlySpan<char> filter, ref int start, ref int from, char to = EndLine, ParseExpressionContext<TType> context = null )
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
            if ( !TryNext( filter, ref start, ref from, ref quote, out var result ) )
            {
                continue;
            }

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
                {
                    continue;
                }
            }

            start = from;

            // `GetExpression` may call recursively call `Parse` for nested expressions
            var func = new ParserFunction<TType>( currentPath, type, context );
            var expression = func.GetExpression( filter, currentPath, ref start, ref from );

            var filterType = ValidType( type )
                ? type!.Value
                : UpdateType( filter, ref start, ref from, type, to );

            tokens.Add( new FilterToken( expression, filterType ) );

        } while ( from < filter.Length && filter[from] != to );

        if ( from < filter.Length && (filter[from] == EndArg || filter[from] == to) )
        {
            // This happens when called recursively: move one char forward.
            from++;
        }

        var baseToken = tokens[0];
        var index = 1;

        return Merge( baseToken, ref index, tokens );
    }

    private static bool TryNext( ReadOnlySpan<char> data, ref int start, ref int from, ref char? quote, out (char NextChar, FilterTokenType? Type) result )
    {
        var nextChar = data[from++];
        switch ( nextChar )
        {
            case '&' when ValidNextCharacter( data, from, '&' ):
                from++;
                start = from;
                result = (nextChar, FilterTokenType.And);
                return true;
            case '|' when ValidNextCharacter( data, from, '|' ):
                from++;
                start = from;
                result = (nextChar, FilterTokenType.Or);
                return true;
            case '=' when ValidNextCharacter( data, from, '=' ):
                from++;
                start = from;
                result = (nextChar, FilterTokenType.Equals);
                return true;
            case '!' when ValidNextCharacter( data, from, '=' ):
                from++;
                start = from;
                result = (nextChar, FilterTokenType.NotEquals);
                return true;
            case '>' when ValidNextCharacter( data, from, '=' ):
                from++;
                start = from;
                result = (nextChar, FilterTokenType.GreaterThanOrEqual);
                return true;
            case '<' when ValidNextCharacter( data, from, '=' ):
                from++;
                start = from;
                result = (nextChar, FilterTokenType.LessThanOrEqual);
                return true;
            case '>':
                start = from;
                result = (nextChar, FilterTokenType.GreaterThan);
                return true;
            case '<':
                start = from;
                result = (nextChar, FilterTokenType.LessThan);
                return true;
            case '!':
                start = from;
                result = (nextChar, FilterTokenType.Not);
                return true;
            case '(':
                result = (nextChar, FilterTokenType.OpenParen);
                return true;
            case ')':
                result = (nextChar, FilterTokenType.ClosedParen);
                return true;
            case ' ' or '\t' when quote == null:
                result = (nextChar, null);
                return false;  // eat whitespace
            case '\'' or '\"' when from > 0 && data[from - 1] != '\\':
                quote = quote == null ? nextChar : null;
                result = (nextChar, null);
                return true;
            default:
                result = (nextChar, null);
                return true;
        }
    }

    private static bool ValidNextCharacter( ReadOnlySpan<char> data, int from, char expected ) =>
        from < data.Length && data[from] == expected;

    private static bool StillCollecting( ReadOnlySpan<char> item, char ch, FilterTokenType? type, char to )
    {
        var stopCollecting = to is EndArg or EndLine
            ? EndArg
            : to;

        return item.Length == 0 && ch == EndArg ||
               !(ValidType( type ) ||
                   type == FilterTokenType.OpenParen && (item.Length > 0 && ValidPathParts.Contains( item[0] ) || item.Length == 0)
                    || ch == stopCollecting);
    }

    private static bool ValidType( FilterTokenType? type ) =>
        type != null && type switch
        {
            FilterTokenType.Not => true,
            FilterTokenType.Equals => true,
            FilterTokenType.NotEquals => true,
            FilterTokenType.GreaterThanOrEqual => true,
            FilterTokenType.GreaterThan => true,
            FilterTokenType.LessThanOrEqual => true,
            FilterTokenType.LessThan => true,
            FilterTokenType.Or => true,
            FilterTokenType.And => true,
            _ => false
        };

    private static FilterTokenType UpdateType( ReadOnlySpan<char> item, ref int start, ref int from, FilterTokenType? type, char to )
    {
        var startType = type;

        if ( from >= item.Length || item[from] == EndArg || item[from] == to )
        {
            return FilterTokenType.ClosedParen;
        }

        var index = from;
        char? quote = null;
        while ( !ValidType( startType ) && index < item.Length )
        {
            TryNext( item, ref start, ref index, ref quote, out var result );
            startType = result.Type;
        }

        from = ValidType( startType ) ? index
            : index > from ? index - 1
            : from;

        return startType!.Value;
    }

    private static Expression Merge( FilterToken current, ref int index, List<FilterToken> listToMerge, bool mergeOneOnly = false )
    {
        while ( index < listToMerge.Count )
        {
            var next = listToMerge[index++];
            while ( !CanMergeTokens( current, next ) )
            {
                Merge( next, ref index, listToMerge, true /* mergeOneOnly */);
            }
            MergeTokens( current, next );
            if ( mergeOneOnly )
            {
                return current.Expression;
            }
        }
        return current.Expression;
    }

    private static bool CanMergeTokens( FilterToken left, FilterToken right ) =>
        // "Not" can never be a right side operator
        right.Type != FilterTokenType.Not && GetPriority( left.Type ) >= GetPriority( right.Type );

    private static int GetPriority( FilterTokenType type ) =>
        type switch
        {
            FilterTokenType.Not => 1,
            FilterTokenType.And or FilterTokenType.Or => 2,
            FilterTokenType.Equals or FilterTokenType.NotEquals or FilterTokenType.GreaterThan or FilterTokenType.GreaterThanOrEqual or FilterTokenType.LessThan or FilterTokenType.LessThanOrEqual => 3,
            _ => 0,
        };

    private static void MergeTokens( FilterToken left, FilterToken right )
    {
        // TODO: clean up handling numerical, string and object comparing. feels messy.
        var isNumerical = left.Expression != null && IsNumerical( left.Expression.Type ) || IsNumerical( right.Expression.Type );
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

        //TODO: Invalid compares should be false, but is this the best way?
        left.Expression = left.Expression == null
            ? left.Expression
            : Expression.TryCatchFinally( left.Expression, null, [Expression.Catch( typeof( Exception ), Expression.Constant( false ) )] );

        left.Type = right.Type;

        return;

        // Use Equal Method vs equal operator
        static Expression Equal( Expression l, Expression r ) => Expression.Call( ObjectEquals, l, r );
        static Expression NotEqual( Expression l, Expression r ) => Expression.Not( Equal( l, r ) );
    }

    private static Expression CompareConvert( Func<Expression, Expression, Expression> compare, Expression left, Expression right, bool isNumerical = true )
    {
        // TODO: clean up... I don't like that most of the time the type is an object because it's being boxed to support num/string/ etc

        // force numerical check for <, >, =<, =>
        if ( isNumerical && left.Type == typeof( object ) && right.Type == typeof( object ) )
            return compare( Expression.Convert( left, typeof( float ) ), Expression.Convert( right, typeof( float ) ) );

        if ( left.Type == typeof( float ) && right.Type == typeof( object ) )
            return compare( left, Expression.Convert( right, typeof( float ) ) );
        if ( left.Type == typeof( object ) && right.Type == typeof( float ) )
            return compare( Expression.Convert( left, typeof( float ) ), right );
        if ( left.Type == typeof( int ) && right.Type == typeof( object ) )
            return compare( left, Expression.Convert( right, typeof( float ) ) );
        if ( left.Type == typeof( object ) && right.Type == typeof( int ) )
            return compare( Expression.Convert( left, typeof( float ) ), right );
        if ( left.Type == typeof( int ) && right.Type == typeof( int ) )
            return compare( Expression.Convert( left, typeof( float ) ), Expression.Convert( right, typeof( float ) ) );
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
