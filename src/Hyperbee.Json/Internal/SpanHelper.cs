namespace Hyperbee.Json.Internal;

internal enum SpanUnescapeOptions
{
    Single,
    SingleThenUnquote,
    Mixed
}

internal static class SpanHelper
{
    internal static void Unescape( ReadOnlySpan<char> span, ref SpanBuilder builder, SpanUnescapeOptions options )
    {
        if ( options == SpanUnescapeOptions.Single || options == SpanUnescapeOptions.SingleThenUnquote )
        {
            if ( span.Length < 2 || span[0] != '\'' && span[0] != '"' || span[^1] != '\'' && span[^1] != '"' )
                throw new ArgumentException( "Quoted strings must start and end with a quote." );

            if ( options == SpanUnescapeOptions.SingleThenUnquote )
                UnescapeQuotedString( span[1..^1], span[0], ref builder ); // unquote and unescape
            else
                UnescapeQuotedString( span, span[0], ref builder ); // unquote
        }
        else
        {
            // Scan for, and unescape, quoted strings
            for ( var i = 0; i < span.Length; i++ )
            {
                var current = span[i];

                if ( current == '\'' || current == '"' )
                {
                    builder.Append( current );

                    var endQuotePos = UnescapeQuotedString( span[++i..], current, ref builder ); // unescape

                    if ( endQuotePos == -1 ) // we expect a closing quote
                        throw new ArgumentException( "Closing quote not found in quoted string." );

                    i += endQuotePos;

                    builder.Append( current );
                }
                else
                {
                    builder.Append( current );
                }
            }
        }
    }

    private static int UnescapeQuotedString( ReadOnlySpan<char> span, char quoteChar, ref SpanBuilder builder )
    {
        for ( var i = 0; i < span.Length; i++ )
        {
            if ( span[i] == quoteChar )
            {
                // return after the closing quote
                return i;
            }

            if ( span[i] == '\\' && i + 1 < span.Length )
            {
                i++;
                switch ( span[i] )
                {
                    case '\'':
                        builder.Append( '\'' );
                        break;
                    case '"':
                        builder.Append( '"' );
                        break;
                    case '\\':
                        builder.Append( '\\' );
                        break;
                    case '/':
                        builder.Append( '/' );
                        break;
                    case 'b':
                        builder.Append( '\b' );
                        break;
                    case 'f':
                        builder.Append( '\f' );
                        break;
                    case 'n':
                        builder.Append( '\n' );
                        break;
                    case 'r':
                        builder.Append( '\r' );
                        break;
                    case 't':
                        builder.Append( '\t' );
                        break;
                    case 'u' when i + 4 < span.Length:
                        builder.Append( ConvertHexToChar( span.Slice( i + 1, 4 ) ) );
                        i += 4;
                        break;
                    default:
                        throw new ArgumentException( $"Invalid escape sequence `\\{span[i]}` in quoted string." );
                }
            }
            else
            {
                builder.Append( span[i] );
            }
        }

        return -1; // no closing quote

        static char ConvertHexToChar( ReadOnlySpan<char> hexSpan )
        {
            if ( hexSpan.Length != 4 )
            {
                throw new ArgumentException( "Hex span must be exactly 4 characters long." );
            }

            var value = 0;
            for ( var i = 0; i < hexSpan.Length; i++ )
            {
                value = (value << 4) + hexSpan[i] switch
                {
                    >= '0' and <= '9' => hexSpan[i] - '0',
                    >= 'a' and <= 'f' => hexSpan[i] - 'a' + 10,
                    >= 'A' and <= 'F' => hexSpan[i] - 'A' + 10,
                    _ => throw new ArgumentException( "Invalid hex digit." )
                };
            }

            return (char) value;
        }
    }
}
