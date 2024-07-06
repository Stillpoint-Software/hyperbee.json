using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Dynamic;

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

    //
    internal static ReadOnlySpan<char> Unescape( ReadOnlySpan<char> span)
    {
        // Estimate the maximum length of the unescaped string
        int maxLength = span.Length;

        // Use stackalloc for the temporary destination span if the length is small enough
        Span<char> destination = new char[maxLength];
        int written = 0;

        for (int i = 0; i < span.Length; i++)
        {
            if (span[i] == '\\' && i + 1 < span.Length)
            {
                // Handle escaping
                i++;
                switch (span[i])
                {
                    case '"':
                    case '\\':
                    case '/':
                        destination[written++] = span[i];
                        break;
                    case 'b':
                        destination[written++] = '\b';
                        break;
                    case 'f':
                        destination[written++] = '\f';
                        break;
                    case 'n':
                        destination[written++] = '\n';
                        break;
                    case 'r':
                        destination[written++] = '\r';
                        break;
                    case 't':
                        destination[written++] = '\t';
                        break;
                    case 'u' when i + 4 < span.Length && IsHexDigit(span[i + 1]) && IsHexDigit(span[i + 2]) && IsHexDigit(span[i + 3]) && IsHexDigit(span[i + 4]):
                        destination[written++] = (char)Convert.ToInt32(span.Slice(i + 1, 4).ToString(), 16);
                        i += 4;
                        break;
                    default:
                        // If not a recognized escape sequence, treat as literal
                        destination[written++] = '\\';
                        destination[written++] = span[i];
                        break;
                }
            }
            else
            {
                destination[written++] = span[i];
            }
        }

        return destination[..written];

        static bool IsHexDigit( char c )
        {
            return (c >= '0' && c <= '9') ||
                   (c >= 'A' && c <= 'F') ||
                   (c >= 'a' && c <= 'f');
        }

    }

     
}
