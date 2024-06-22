using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser;

internal class LiteralFunction : FilterFunction
{
    public override Expression GetExpression( ReadOnlySpan<char> filter, ReadOnlySpan<char> item, ref int start, ref int from, ParseExpressionContext context )
    {
        // Check for known literals (true, false, null) first

        if ( item.Equals( "true", StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( true );

        if ( item.Equals( "false", StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( false );

        if ( item.Equals( "null", StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( null );

        // Check for quoted strings

        if ( TryRemoveQuotes( ref item ) )
            return Expression.Constant( item.ToString() );

        // Check for numbers
        // TODO: Currently assuming all numbers are floats since we don't know what's in the data or the other side of the operator yet.

        if ( float.TryParse( item, out float result ) )
            return Expression.Constant( result );

        throw new ArgumentException( $"Unsupported literal: {item.ToString()}" );

        static bool TryRemoveQuotes( ref ReadOnlySpan<char> input )
        {
            if ( !IsQuoted( input ) )
                return false;

            input = input[1..^1];
            return true;

            static bool IsQuoted( ReadOnlySpan<char> input )
            {
                return input.Length >= 2 && ((input[0] == '"' && input[^1] == '"') || (input[0] == '\'' && input[^1] == '\''));
            }
        }
    }
}
