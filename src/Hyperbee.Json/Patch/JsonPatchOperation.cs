using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hyperbee.Json.Patch;

public enum JsonPatchOperationType
{
    Add,
    Remove,
    Replace,
    Move,
    Copy,
    Test
}

[JsonConverter( typeof( JsonPatchConverter ) )]
[DebuggerDisplay( "{Operation}, Path = {Path}, Value = {Value}, From = {From}" )]
public readonly record struct JsonPatchOperation( JsonPatchOperationType Operation, string Path, string From, object Value );

public class JsonPatchConverter : JsonConverter<JsonPatchOperation>
{
    public override JsonPatchOperation Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        string op = null;
        string path = null;
        string from = null;
        object value = null;

        while ( reader.Read() )
        {
            if ( reader.TokenType == JsonTokenType.EndObject )
                break;

            if ( reader.TokenType != JsonTokenType.PropertyName )
                continue;

            string propertyName = reader.GetString();
            reader.Read();

            switch ( propertyName )
            {
                case "op":
                    op = reader.GetString();
                    break;
                case "path":
                    path = reader.GetString();
                    break;
                case "from":
                    from = reader.GetString();
                    break;
                case "value":
                    value = JsonSerializer.Deserialize<JsonElement>( ref reader, options );
                    break;
                default:
                    throw new JsonException( $"Unexpected property '{propertyName}'." );
            }
        }

        if ( op == null )
            throw new JsonException( "Missing 'op' property." );

        if ( path == null )
            throw new JsonException( "Missing 'path' property." );

        if ( !Enum.TryParse( op, true, out JsonPatchOperationType operationKind ) )
            throw new JsonException( $"Invalid operation '{op}'." );

        return new JsonPatchOperation { Operation = operationKind, Path = path, From = from, Value = value };
    }

    public override void Write( Utf8JsonWriter writer, JsonPatchOperation value, JsonSerializerOptions options )
    {
        writer.WriteStartObject();
        writer.WriteString( "op", value.Operation.ToString().ToLowerInvariant() );
        writer.WriteString( "path", value.Path );

        switch ( value.Operation )
        {
            case JsonPatchOperationType.Add:
            case JsonPatchOperationType.Replace:
                writer.WritePropertyName( "value" );
                JsonSerializer.Serialize( writer, value.Value, options );
                break;
            case JsonPatchOperationType.Move:
            case JsonPatchOperationType.Copy:
                writer.WriteString( "from", value.From );
                break;
            case JsonPatchOperationType.Remove:
                break;
            case JsonPatchOperationType.Test:
                break;
            default:
                throw new JsonException( $"Invalid operation '{value.Operation}'." );
        }

        writer.WriteEndObject();
    }
}
