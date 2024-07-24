using System.Text;

namespace Hyperbee.Json.Extensions;

public static class JsonPathPointerConverter
{
    public static string ConvertJsonPathToJsonPointer( ReadOnlySpan<char> jsonPath, bool asFragment = false )
    {
        if ( jsonPath.IsEmpty || jsonPath.SequenceEqual( "$".AsSpan() ) )
        {
            return asFragment ? "#/" : "/";
        }

        var jsonPointer = new StringBuilder( asFragment ? "#/" : "/" );
        var i = 0;

        while ( i < jsonPath.Length )
        {
            switch ( jsonPath[i] )
            {
                case '$':
                    i++;
                    break;
                case '[':
                    i++;
                    var quote = jsonPath[i];
                    switch ( quote )
                    {
                        case '\'':
                        case '"':
                            {
                                i++;
                                var start = i;
                                while ( i < jsonPath.Length && (jsonPath[i] != quote || (i > start && jsonPath[i - 1] == '\\')) )
                                {
                                    i++;
                                }

                                JsonPointerAppendEscaped( jsonPointer, jsonPath[start..i], true );
                                i += 2; // Skip the closing ']'
                                break;
                            }
                        default:
                            {
                                var start = i;
                                while ( i < jsonPath.Length && jsonPath[i] != ']' )
                                {
                                    i++;
                                }

                                jsonPointer.Append( jsonPath[start..i] ).Append( '/' );
                                i++;
                                break;
                            }
                    }

                    break;
                case '.':
                    i++;
                    var startDot = i;
                    while ( i < jsonPath.Length && jsonPath[i] != '.' && jsonPath[i] != '[' )
                    {
                        i++;
                    }

                    JsonPointerAppendEscaped( jsonPointer, jsonPath[startDot..i], false );
                    break;
                default:
                    throw new InvalidOperationException( $"Unexpected character '{jsonPath[i]}' in JSONPath." );
            }
        }

        if ( jsonPointer.Length > 1 )
        {
            // Remove trailing slash
            jsonPointer.Length--;
        }

        return jsonPointer.ToString();
    }

    private static void JsonPointerAppendEscaped( StringBuilder jsonPointer, ReadOnlySpan<char> itemSpan, bool isQuoted )
    {
        var replacementCount = 0;

        foreach ( var c in itemSpan )
        {
            if ( (isQuoted && c == '/') || (c == '~' || c == '/') )
            {
                replacementCount++;
            }
        }

        if ( replacementCount == 0 )
        {
            jsonPointer.Append( itemSpan ).Append( '/' );
            return;
        }

        var length = itemSpan.Length + replacementCount;
        var buffer = length <= 256 ? stackalloc char[length] : new char[length];

        var bufferIndex = 0;
        for ( var i = 0; i < itemSpan.Length; i++ )
        {
            if ( itemSpan[i] == '\\' && isQuoted )
            {
                // Skip the escape character and append the next character directly
                buffer[bufferIndex++] = itemSpan[++i];
            }
            else
            {
                switch ( itemSpan[i] )
                {
                    case '~':
                        buffer[bufferIndex++] = '~';
                        buffer[bufferIndex++] = '0';
                        break;
                    case '/':
                        buffer[bufferIndex++] = '~';
                        buffer[bufferIndex++] = '1';
                        break;
                    default:
                        buffer[bufferIndex++] = itemSpan[i];
                        break;
                }
            }
        }

        jsonPointer.Append( buffer[..bufferIndex] ).Append( '/' );
    }

    public static string ConvertJsonPointerToJsonPath( ReadOnlySpan<char> jsonPointer )
    {
        if ( jsonPointer.IsEmpty || jsonPointer is "/" or "#/" )
        {
            return "$";
        }

        var jsonPath = new StringBuilder( "$" );

        if ( jsonPointer[0] == '#' )
        {
            jsonPointer = jsonPointer[1..];
        }

        var i = 0;

        while ( i < jsonPointer.Length )
        {
            switch ( jsonPointer[i] )
            {
                case '/':
                    {
                        i++;
                        var start = i;

                        while ( i < jsonPointer.Length && jsonPointer[i] != '/' )
                        {
                            i++;
                        }

                        var itemSpan = jsonPointer[start..i];
                        var item = new StringBuilder();

                        for ( var j = 0; j < itemSpan.Length; j++ )
                        {
                            if ( itemSpan[j] != '~' )
                            {
                                item.Append( itemSpan[j] );
                                continue;
                            }

                            switch ( itemSpan[j + 1] )
                            {
                                case '1':
                                    item.Append( '/' );
                                    j++;
                                    break;
                                case '0':
                                    item.Append( '~' );
                                    j++;
                                    break;
                                default:
                                    item.Append( '~' );
                                    break;
                            }
                        }
                    }

                        var itemStr = item.ToString();

                    // Helper method to determine if a property name is simple (no special characters)

                    static bool HasSpecialCharacters( ReadOnlySpan<char> propertyName )
                    {
                        foreach ( var c in propertyName )
                        {
                            if ( !char.IsLetterOrDigit( c ) && c != '_' )
                            {
                                return true;
                            }
                        }

                        return false;
                    }
            }

            private static void JsonPathAppendEscaped( StringBuilder jsonPath, ReadOnlySpan<char> itemSpan )
            {
                jsonPath.Append( "['" );

                int lastPos = 0;
                for ( int j = 0; j < itemSpan.Length; j++ )
                {
                    if ( itemSpan[j] != '\'' )
                    {
                        continue;
                    }

                    if ( j > lastPos )
                    {
                        jsonPath.Append( itemSpan[lastPos..j] ); // Append the chunk before the escape character
                    }

                    jsonPath.Append( "\\'" );
                    lastPos = j + 1; // Update the last position to after the escape character
                }

                // Append any remaining part of the string after the last escape character
                if ( lastPos < itemSpan.Length )
                {
                    jsonPath.Append( itemSpan[lastPos..] );
                }

                jsonPath.Append( "']" );
            }
        }
