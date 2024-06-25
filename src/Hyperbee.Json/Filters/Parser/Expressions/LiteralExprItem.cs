﻿using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class LiteralExprItem : IExprItem
{
    public static bool TryGetItem( ref ParserState state, ExprItemFactory exprItemCreator, out ExprItem exprItem, FilterContext context )
    {
        var expression = GetLiteralExpression( state.Item );

        if ( expression != null )
        {
            exprItem = exprItemCreator( ref state, expression );
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
}