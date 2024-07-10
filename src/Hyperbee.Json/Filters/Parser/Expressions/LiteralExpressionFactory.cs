using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;
using ValueType = Hyperbee.Json.Descriptors.ValueType;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class LiteralExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo expressionInfo, FilterParserContext<TNode> parserContext )
    {
        expression = GetLiteralExpression( state.Item );

        if ( expression == null )
            return false;

        expressionInfo.Kind = ExpressionKind.Literal;
        return true;

    }

    private static ConstantExpression GetLiteralExpression( ReadOnlySpan<char> item )
    {
        // Check for known literals (true, false, null) first

        if ( item.Equals( "true", StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( ValueType.True );

        if ( item.Equals( "false", StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( ValueType.False );

        if ( item.Equals( "null", StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( ValueType.Null );

        // Check for quoted strings

        if ( item.Length >= 2 && (item[0] == '"' && item[^1] == '"' || item[0] == '\'' && item[^1] == '\'') )
            return Expression.Constant( new ValueType<string>( item[1..^1].ToString() ) ); // remove quotes

        // Check for numbers
        //
        // The current design treats all numbers are floats since we don't
        // know what's in the data or the other side of the operator yet.

        if ( item.Length > 0 && item[^1] == '.' ) // incomplete floating-point number. we can parse it but the RFC doesn't like it.
            throw new NotSupportedException( $"Incomplete floating-point number `{item.ToString()}`" );

        return float.TryParse( item, out float result )
            ? Expression.Constant( new ValueType<float>( result ) )
            : null;
    }
}
