using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class LiteralExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo exprInfo, ITypeDescriptor<TNode> descriptor )
    {
        expression = GetLiteralExpression( state.Item );

        if ( expression == null )
            return false;

        exprInfo.Kind = ExpressionKind.Literal;
        return true;
    }

    private static ConstantExpression GetLiteralExpression( ReadOnlySpan<char> item )
    {
        // Check for known literals (true, false, null) first

        if ( item.Equals( "true", StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( Scalar.True );

        if ( item.Equals( "false", StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( Scalar.False );

        if ( item.Equals( "null", StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( Scalar.Null );

        // Check for quoted strings

        if ( item.Length >= 2 && (item[0] == '"' && item[^1] == '"' || item[0] == '\'' && item[^1] == '\'') )
            return Expression.Constant( Scalar.Value( item[1..^1].ToString() ) ); // remove quotes

        // Check for numbers

        if ( int.TryParse( item, out int intResult ) )
            return Expression.Constant( Scalar.Value( intResult ) );

        if ( item.Length > 0 && item[^1] == '.' ) // incomplete floating-point number. we can parse it but the RFC doesn't like it.
            throw new NotSupportedException( $"Incomplete floating-point number `{item.ToString()}`" );

        return float.TryParse( item, out float result )
            ? Expression.Constant( Scalar.Value( result ) )
            : null;
    }
}
