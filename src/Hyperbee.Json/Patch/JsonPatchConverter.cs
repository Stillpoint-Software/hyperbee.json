using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hyperbee.Json.Patch;

public class JsonPatchConverter : JsonConverter<JsonPatch>
{
    public override JsonPatch Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        if ( reader.TokenType != JsonTokenType.StartArray )
            throw new JsonException();

        var operations = new List<PatchOperation>();
        while ( reader.Read() )
        {
            if ( reader.TokenType == JsonTokenType.EndArray )
                break;

            var operation = JsonSerializer.Deserialize<PatchOperation>( ref reader, options );
            operations.Add( operation );
        }

        return new JsonPatch( [.. operations] );
    }

    public override void Write( Utf8JsonWriter writer, JsonPatch value, JsonSerializerOptions options )
    {
        writer.WriteStartArray();

        foreach ( var operation in value )
        {
            JsonSerializer.Serialize( writer, operation, options );
        }

        writer.WriteEndArray();
    }
}
