using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Dynamic;
using Hyperbee.Json.Internal;

namespace Hyperbee.Json.Extensions;

public static class JsonHelper
{
    // conversion

    public static ReadOnlySpan<char> ConvertToBracketNotation( ReadOnlySpan<char> path )
    {
        var segments = JsonPathQueryParser.ParseNoCache( path );

        var builder = new StringBuilder();

        foreach ( var token in segments.AsEnumerable() )
        {
            builder.Append( '[' );

            foreach ( var selector in token.Selectors )
            {
                switch ( selector.SelectorKind )
                {
                    case SelectorKind.Root:
                        builder.Append( "'$'" );
                        break;
                    case SelectorKind.DotName:
                    case SelectorKind.Name:
                        builder.Append( $"'{selector.Value}'" );
                        break;
                    case SelectorKind.Wildcard:
                        builder.Append( '*' );
                        break;
                    case SelectorKind.Descendant:
                        builder.Append( ".." );
                        break;
                    case SelectorKind.Slice:
                    case SelectorKind.Filter:
                    case SelectorKind.Index:
                        builder.Append( selector.Value );
                        break;

                    case SelectorKind.Undefined:
                    default:
                        throw new NotSupportedException( $"Unsupported {nameof( SelectorKind )}." );
                }
            }

            builder.Append( ']' );
        }

        return builder.ToString();
    }

    public static dynamic ConvertToDynamic( JsonNode value ) => new DynamicJsonNode( ref value );
    public static dynamic ConvertToDynamic( JsonElement value, string path = null ) => new DynamicJsonElement( ref value, path );
    public static dynamic ConvertToDynamic( JsonDocument value ) => ConvertToDynamic( value.RootElement, "$" );

    public static T ConvertToObject<T>( JsonElement value, JsonSerializerOptions options = null )
        where T : new()
    {
        var bufferWriter = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter( bufferWriter );

        value.WriteTo( writer );
        writer.Flush();

        var reader = new Utf8JsonReader( bufferWriter.WrittenSpan );
        return JsonSerializer.Deserialize<T>( ref reader, options );
    }

    public static T ConvertToObject<T>( JsonNode value, JsonSerializerOptions options = null )
        where T : new()
    {
        var bufferWriter = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter( bufferWriter );

        value.WriteTo( writer );
        writer.Flush();

        var reader = new Utf8JsonReader( bufferWriter.WrittenSpan );
        return JsonSerializer.Deserialize<T>( ref reader, options );
    }

    //BF not sure where to put this

    internal static void Unescape( ReadOnlySpan<char> span, ref SpanBuilder builder, bool singleString = true )
    {
        if ( singleString )
        {
            if ( span.Length < 2 || (span[0] != '\'' && span[0] != '"') || (span[^1] != '\'' && span[^1] != '"') )
                throw new ArgumentException( "Quoted strings must start and end with a quote." );

            UnescapeQuotedString( span[1..^1], span[0], ref builder ); // unquote and unescape
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
