using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hyperbee.Json.Patch;

public enum PatchOperationType
{
    Add,
    Copy,
    Move,
    Remove,
    Replace,
    Test
}

[JsonConverter( typeof( JsonPatchConverter ) )]
[DebuggerDisplay( "{Operation}, Path = {Path}, Value = {Value}, From = {From}" )]
public readonly record struct PatchOperation( PatchOperationType Operation, string Path, string From, object Value );

public class JsonPatchConverter : JsonConverter<PatchOperation>
{
    public override PatchOperation Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
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

        if ( !Enum.TryParse( op, true, out PatchOperationType operationKind ) )
            throw new JsonException( $"Invalid operation '{op}'." );

        return new PatchOperation { Operation = operationKind, Path = path, From = from, Value = value };
    }

    public override void Write( Utf8JsonWriter writer, PatchOperation value, JsonSerializerOptions options )
    {
        writer.WriteStartObject();
        writer.WriteString( "op", value.Operation.ToString().ToLowerInvariant() );
        writer.WriteString( "path", value.Path );

        switch ( value.Operation )
        {
            case PatchOperationType.Add:
            case PatchOperationType.Replace:
                writer.WritePropertyName( "value" );
                JsonSerializer.Serialize( writer, value.Value, options );
                break;
            case PatchOperationType.Move:
            case PatchOperationType.Copy:
                writer.WriteString( "from", value.From );
                break;
            case PatchOperationType.Remove:
                break;
            case PatchOperationType.Test:
                break;
            default:
                throw new JsonException( $"Invalid operation '{value.Operation}'." );
        }

        writer.WriteEndObject();
    }
}
