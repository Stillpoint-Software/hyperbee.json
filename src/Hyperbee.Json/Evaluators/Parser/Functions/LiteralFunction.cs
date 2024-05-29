using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;


public class LiteralFunction<TType> : ParserFunction<TType>
{
    protected override Expression Evaluate( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        // strings double or single
        if ( JsonPathFilterTokenizerRegex.RegexQuotedDouble().IsMatch( item ) )
            return Expression.Constant( TrimQuotes( item ).ToString() );
        if ( JsonPathFilterTokenizerRegex.RegexQuoted().IsMatch( item ) )
            return Expression.Constant( TrimQuotes( item ).ToString() );

        // known literals (true, false, null)
        if ( item.Equals( KnownLiterals.TrueSpan, StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( true );
        if ( item.Equals( KnownLiterals.FalseSpan, StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( false );
        if ( item.Equals( KnownLiterals.NullSpan, StringComparison.OrdinalIgnoreCase ) )
            return Expression.Constant( null );

        // numbers
        // TODO: Currently assuming all numbers are floats since we don't know what's in the data or the other side of the operator yet.
        return Expression.Constant( float.Parse( item ) );

        static ReadOnlySpan<char> TrimQuotes( ReadOnlySpan<char> input )
        {
            if ( input.Length < 2 )
                return input;

            if ( (input[0] == '\'' && input[^1] == '\'') || (input[0] == '\"' && input[^1] == '\"') )
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
