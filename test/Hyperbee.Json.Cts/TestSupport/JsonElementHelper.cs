using System.Text.Json;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Cts.TestSupport;

public class JsonPathDocument( string source ) : IJsonDocument
{
    private JsonDocument Document { get; } = JsonDocument.Parse( source );

    public dynamic Root => Document.RootElement;
    public IEnumerable<dynamic> Select( string query ) => Document.Select( query ).Cast<object>();
}

internal static class JsonElementHelper
{
    private static JsonElement ConvertToJsonArraySet( JsonElement jsonElement )
    {
        // Check if the jsonElement is already a JsonArray containing a JsonArray
        if ( jsonElement.ValueKind == JsonValueKind.Array && jsonElement[0].ValueKind == JsonValueKind.Array )
        {
            return jsonElement; // already a set
        }

        // Create a new JsonArray set
        using var stream = new MemoryStream();
        using ( var writer = new Utf8JsonWriter( stream ) )
        {
            writer.WriteStartArray();
            writer.WriteStartArray();

            foreach ( JsonElement element in jsonElement.EnumerateArray() )
            {
                element.WriteTo( writer );
            }

            writer.WriteEndArray();
            writer.WriteEndArray();
        }

        stream.Seek( 0, SeekOrigin.Begin );
        return JsonDocument.Parse( stream ).RootElement;
    }

    private static JsonElement ConvertToJsonArray( IEnumerable<JsonElement> nodes )
    {
        return CreateJsonArray( nodes ).RootElement;

        static JsonDocument CreateJsonArray( IEnumerable<JsonElement> elements )
        {
            using var stream = new MemoryStream();
            using ( var writer = new Utf8JsonWriter( stream ) )
            {
                writer.WriteStartArray();
                foreach ( var element in elements )
                {
                    element.WriteTo( writer );
                }

                writer.WriteEndArray();
            }

            stream.Seek( 0, SeekOrigin.Begin );
            return JsonDocument.Parse( stream );
        }
    }

    public static bool MatchAny( IEnumerable<JsonElement> results, JsonElement expected )
    {
        var expectedSet = ConvertToJsonArraySet( expected );
        var compare = ConvertToJsonArray( results );
        return expectedSet.EnumerateArray().Any( expect => expect.DeepEquals( compare ) );
    }

    public static bool MatchOne( IEnumerable<JsonElement> results, JsonElement expected )
    {
        var compare = ConvertToJsonArray( results );
        return expected.DeepEquals( compare );
    }
}
