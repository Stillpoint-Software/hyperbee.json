using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Hyperbee.Json.Tests.TestSupport;

// test helper to assist with getting, and normalizing, json values
//
// JsonElement and JsonNode return values differently.
// this helper provides a common interface for value
// retrieval that simplifies unit testing.

internal static class JsonValueHelper
{
    // JsonElement Values

    public static bool GetBoolean( JsonElement value ) => value.GetBoolean();

    public static int GetInt32( JsonElement value ) => value.GetInt32();

    public static string GetString( JsonElement value, bool minify = false )
    {
        if ( value.ValueKind == JsonValueKind.Object || value.ValueKind == JsonValueKind.Array )
        {
            var result = value.ToString();
            return minify ? MinifyJsonString( result ) : result;
        }

        return value.GetString();
    }

    // JsonNode Values

    public static bool GetBoolean( JsonNode value ) => value.AsValue().GetValue<bool>();

    public static int GetInt32( JsonNode value ) => value.AsValue().GetValue<int>();

    public static string GetString( JsonNode value, bool minify = false )
    {
        if ( value is JsonObject || value is JsonArray )
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = false
            };

            var result = value.ToJsonString( options );
            return minify ? MinifyJsonString( result ) : result;
        }

        return value.AsValue().GetValue<string>();
    }

    // Json string helpers

    public static string MinifyJsonString( string json )
    {
        const string minifyPattern = "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+";
        return Regex.Replace( json, minifyPattern, "$1" ); 
    }
}