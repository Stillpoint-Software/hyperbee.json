using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Tests.TestSupport;

// Test helper to assist with getting, and normalizing, json values
//
// JsonElement and JsonNode return values differently.
// Provide a common interface for value retrieval to simplify unit tests.

internal static class JsonValueHelper
{
    // JsonElement Values

    public static bool GetBoolean( JsonElement value ) => value.GetBoolean();

    public static int GetInt32( JsonElement value ) => value.GetInt32();

    public static string GetString( JsonElement value, bool minify = false )
    {
        if ( value.ValueKind != JsonValueKind.Object && value.ValueKind != JsonValueKind.Array )
            return value.GetString();

        var result = value.ToString();
        return minify ? MinifyJson( result ) : result;
    }

    // JsonNode Values

    public static bool GetBoolean( JsonNode value ) => value.AsValue().GetValue<bool>();

    public static int GetInt32( JsonNode value ) => value.AsValue().GetValue<int>();

    public static string GetString( JsonNode value, bool minify = false )
    {
        if ( value is not JsonObject && value is not JsonArray )
            return value.AsValue().GetValue<string>();

        var options = new JsonSerializerOptions { WriteIndented = false };

        var result = value.ToJsonString( options );
        return minify ? MinifyJson( result ) : result;
    }

    // Json string helpers

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
