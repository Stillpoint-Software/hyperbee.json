using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser;


public class LiteralFunction : FilterFunction
{
    protected override Expression GetExpressionImpl( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        // Check for known literals (true, false, null) first
        if ( item.Equals( KnownLiterals.TrueSpan, StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( true );
        if ( item.Equals( KnownLiterals.FalseSpan, StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( false );
        if ( item.Equals( KnownLiterals.NullSpan, StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( null );

        // Check for quoted strings
        if ( item.Length > 1 && ((item[0] == '"' && item[^1] == '"') || (item[0] == '\'' && item[^1] == '\'')) )
            return Expression.Constant( TrimQuotes( item ).ToString() );

        // Check for numbers
        // TODO: Currently assuming all numbers are floats since we don't know what's in the data or the other side of the operator yet.
        if ( float.TryParse( item, out float result ) )
            return Expression.Constant( result );

        throw new ArgumentException( $"Unsupported literal: {item.ToString()}" );

        static ReadOnlySpan<char> TrimQuotes( ReadOnlySpan<char> input )
        {
            if ( input.Length >= 2 && ((input[0] == '"' && input[^1] == '"') || (input[0] == '\'' && input[^1] == '\'')) )
                return input[1..^1];

            return input;
        }
    }

    internal ref struct KnownLiterals
    {
        internal static ReadOnlySpan<char> TrueSpan => "true".AsSpan();
        internal static ReadOnlySpan<char> FalseSpan => "false".AsSpan();
        internal static ReadOnlySpan<char> NullSpan => "null".AsSpan();
    }
}
