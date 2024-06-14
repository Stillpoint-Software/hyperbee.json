using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hyperbee.Json.Dynamic;
/*
    A converter that provides limited support for serializing to and from dynamic objects.
    It does not directly support enums; see TryReadValueHandler.

    dynamic jobject = JsonSerializer.Deserialize<dynamic>( jsonString, new JsonSerializerOptions
    {
        Converters = { new DynamicJsonConverter() }
    });
*/

public class DynamicJsonConverter : JsonConverter<dynamic>
{
    private class JsonPath
    {
        private readonly Stack<string> _path = new();

        public void Clear() => _path.Clear();
        public string Current => _path.TryPeek( out var path ) ? path : string.Empty;
        public void Push( string part ) => _path.Push( _path.Count == 0 ? part : $"{Current}.{part}" );
        public void Pop() => _path.Pop();
    }

    private readonly JsonPath JPath = new();

    public delegate bool TryReadJsonValue( ref Utf8JsonReader reader, JsonTokenType tokenType, JsonSerializerOptions options, string jsonPath, out IConvertible value );

    public TryReadJsonValue TryReadValueHandler { get; set; }

    public override dynamic Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        try
        {
            return InternalRead( ref reader, typeToConvert, options );
        }
        finally
        {
            JPath.Clear();
        }
    }

    private dynamic InternalRead( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        switch ( reader.TokenType )
        {
            case JsonTokenType.StartObject:
                IDictionary<string, object> expando = new ExpandoObject();

                while ( reader.Read() )
                {
                    if ( reader.TokenType == JsonTokenType.EndObject )
                        return expando;

                    if ( reader.TokenType != JsonTokenType.PropertyName )
                        throw new JsonException();

                    var propertyName = reader.GetString();
                    reader.Read();

                    try
                    {
                        JPath.Push( $"{propertyName}" );
                        expando[propertyName!] = InternalRead( ref reader, typeToConvert, options );
                    }
                    finally
                    {
                        JPath.Pop();
                    }
                }

                return expando;


            case JsonTokenType.StartArray:
                IList<object> array = [];

                var i = 0;
                while ( reader.Read() )
                {
                    if ( reader.TokenType == JsonTokenType.EndArray )
                        return array;

                    try
                    {
                        JPath.Push( $"[{i++}]" );
                        array.Add( InternalRead( ref reader, typeToConvert, options ) );
                    }
                    finally
                    {
                        JPath.Pop();
                    }
                }

                return array.Count == 0 ? null : array;

            case JsonTokenType.Number:
            case JsonTokenType.String:
                if ( TryReadValueHandler != null && TryReadValueHandler( ref reader, reader.TokenType, options, JPath.Current, out var value ) )
                    return value;

                if ( TryReadValue( ref reader, reader.TokenType, options, JPath.Current, out value ) )
                    return value;

                throw new InvalidDataException();

            case JsonTokenType.True:
                return true;
            case JsonTokenType.False:
                return false;

            case JsonTokenType.None:
            case JsonTokenType.Null:
                return null;

            case JsonTokenType.Comment:
                break;

            case JsonTokenType.PropertyName:
            case JsonTokenType.EndObject:
            case JsonTokenType.EndArray:
                throw new InvalidDataException();
        }

        throw new InvalidDataException();
    }

    public bool TryReadValue( ref Utf8JsonReader reader, JsonTokenType tokenType, JsonSerializerOptions options, string jsonPath, out IConvertible value )
    {
        switch ( tokenType )
        {
            case JsonTokenType.Number:
                {
                    if ( reader.TryGetInt64( out var number ) )
                        value = number;
                    else
                        value = reader.GetDouble();

                    return true;
                }
            case JsonTokenType.String:
                {
                    if ( reader.TryGetDateTime( out var datetime ) )
                        value = datetime;
                    else
                        value = reader.GetString();

                    return true;
                }
            default:
                {
                    value = null;
                    return false;
                }
        }
    }

    public override void Write( Utf8JsonWriter writer, object value, JsonSerializerOptions options )
    {
        try
        {
            InternalWrite( writer, null, value, options );
        }
        finally
        {
            JPath.Clear();
        }
    }

    private static void InternalWrite( Utf8JsonWriter writer, string name, object value, JsonSerializerOptions options )
    {
        switch ( value )
        {
            case IList<object> array:
                {
                    if ( name != null )
                        writer.WriteStartArray( name );
                    else
                        writer.WriteStartArray();

                    foreach ( var v in array )
                    {
                        InternalWrite( writer, null, v, options );
                    }

                    writer.WriteEndArray();
                    break;
                }
            case IDictionary<string, object> dictionary:
                {
                    if ( name != null )
                        writer.WriteStartObject( name );
                    else
                        writer.WriteStartObject();

                    foreach ( var (n, v) in dictionary )
                    {
                        InternalWrite( writer, n, v, options );
                    }

                    writer.WriteEndObject();
                    break;
                }
            case decimal valueDecimal:
                {
                    writer.WriteNumber( name, valueDecimal );
                    break;
                }
            case double valueDouble:
                {
                    writer.WriteNumber( name, valueDouble );
                    break;
                }
            case int valueInt:
                {
                    writer.WriteNumber( name, valueInt );
                    break;
                }
            case long valueLong:
                {
                    writer.WriteNumber( name, valueLong );
                    break;
                }
            case bool valueBoolean:
                {
                    writer.WriteBoolean( name, valueBoolean );
                    break;
                }
            case Enum valueEnum:
                {
                    if ( options?.GetConverter( valueEnum.GetType() ) is JsonConverter<Enum> converter )
                        converter.Write( writer, valueEnum, options );
                    else
                        writer.WriteString( name, value.ToString() );
                    break;
                }
            default:
                {
                    writer.WriteString( name, value.ToString() );
                    break;
                }
        }
    }
}
