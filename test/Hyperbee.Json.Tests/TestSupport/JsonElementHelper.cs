using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Pointer;

namespace Hyperbee.Json.Tests.TestSupport;

public class JsonElementDocument( string source ) : IJsonDocument
{
    private JsonDocument Document { get; } = JsonDocument.Parse( source );
    public IEnumerable<dynamic> Select( string query ) => Document.Select( query ).Cast<object>();
    public dynamic FromJsonPathPointer( string pointer ) => JsonPathPointer<JsonElement>.FromPointer( Document.RootElement, pointer );
}

public static class JsonElementExtensions
{
    public static JsonElement FromJsonPathPointer( this JsonElement source, string pointer )
    {
        return JsonPathPointer<JsonElement>.FromPointer( source, pointer );
    }
}

internal static partial class TestHelper
{
    // JsonElement Values

    public static bool GetBoolean( JsonElement value ) => value.GetBoolean();

    public static int GetInt32( JsonElement value ) => value.GetInt32();

    public static float GetSingle( JsonElement value ) => value.GetSingle();

    public static string GetString( JsonElement value, bool minify = false )
    {
        if ( value.ValueKind != JsonValueKind.Object && value.ValueKind != JsonValueKind.Array )
            return value.GetString();

        var result = value.ToString();
        return minify ? MinifyJson( result ) : result;
    }
}
