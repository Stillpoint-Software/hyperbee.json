using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class LiteralExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, FilterContext<TNode> context )
    {
        expression = GetLiteralExpression( state.Item, context );
        return expression != null;
    }

    private static ConstantExpression GetLiteralExpression<TNode>( ReadOnlySpan<char> item, FilterContext<TNode> context )
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
        //
        // The current design treats all numbers are floats since we don't
        // know what's in the data or the other side of the operator yet.
        
        return float.TryParse( item, out float result ) 
            ? Expression.Constant( result ) 
            : null;
    }
}
