using System;

namespace Hyperbee.Json.Tests.TestSupport;

internal static partial class TestHelper
{
    public static string MinifyJson( ReadOnlySpan<char> input )
    {
        Span<char> buffer = new char[input.Length];
        int bufferIndex = 0;
        bool insideString = false;
        bool escapeNext = false;

        foreach ( char ch in input )
        {
            switch ( ch )
            {
                case '\\':
                    if ( insideString ) escapeNext = !escapeNext;
                    buffer[bufferIndex++] = ch;
                    break;

                case '\"':
                    if ( !escapeNext )
                        insideString = !insideString;

                    escapeNext = false;
                    buffer[bufferIndex++] = ch;
                    break;

                case '\r':
                case '\n':
                case '\t':
                case ' ':
                    if ( insideString )
                        buffer[bufferIndex++] = ch;

                    break;

                default:
                    escapeNext = false;
                    buffer[bufferIndex++] = ch;
                    break;
            }
        }

        return new string( buffer[..bufferIndex] );
    }
}
