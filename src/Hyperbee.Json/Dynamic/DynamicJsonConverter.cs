/*
    A converter that provides limited support for serializing to and from dynamic objects.
    It does not directly support enums; see TryReadValueHandler.

    dynamic jobject = JsonSerializer.Deserialize<dynamic>( jsonString, new JsonSerializerOptions
    {
        Converters = { new DynamicJsonConverter() }
    });
*/

using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hyperbee.Json.Dynamic;

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

    public delegate bool TryReadJsonValue( ref Utf8JsonReader reader, JsonTokenType tokenType, JsonSerializerOptions options, string jsonPath, out IConvertible value );
    public TryReadJsonValue TryReadValueHandler { get; set; }

    private readonly JsonPath _jsonPath = new();
    
    public override dynamic Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        try
        {
            return ReadInternal( ref reader, typeToConvert, options );
        }
        finally
        {
            _jsonPath.Clear();
        }
    }

    private dynamic ReadInternal( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        return reader.TokenType switch
        {
            JsonTokenType.StartObject => ReadObject( ref reader, typeToConvert, options ),
            JsonTokenType.StartArray => ReadArray( ref reader, typeToConvert, options ),
            JsonTokenType.Number or JsonTokenType.String => ReadPrimitive( ref reader, options ),
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Null => null,
            _ => throw new JsonException()
        };
    }

    private IDictionary<string, object> ReadObject( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
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
                _jsonPath.Push( propertyName );
                expando[propertyName!] = ReadInternal( ref reader, typeToConvert, options );
            }
            finally
            {
                _jsonPath.Pop();
            }
        }

        throw new JsonException();
    }

    private IList<object> ReadArray( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        IList<object> array = [];
        int index = 0;

        while ( reader.Read() )
        {
            if ( reader.TokenType == JsonTokenType.EndArray )
                return array;

            try
            {
                _jsonPath.Push( $"[{index++}]" );
                array.Add( ReadInternal( ref reader, typeToConvert, options ) );
            }
            finally
            {
                _jsonPath.Pop();
            }
        }

        throw new JsonException();
    }

    private dynamic ReadPrimitive( ref Utf8JsonReader reader, JsonSerializerOptions options )
    {
        if ( TryReadValueHandler != null && TryReadValueHandler( ref reader, reader.TokenType, options, _jsonPath.Current, out var value ) )
            return value;

        return reader.TokenType switch
        {
            JsonTokenType.Number when reader.TryGetInt64( out var l ) => l,
            JsonTokenType.Number => reader.GetDouble(),
            JsonTokenType.String when reader.TryGetDateTime( out var datetime ) => datetime,
            JsonTokenType.String => reader.GetString(),
            _ => throw new JsonException()
        };
    }

    public override void Write( Utf8JsonWriter writer, dynamic value, JsonSerializerOptions options )
    {
        try
        {
            WriteInternal( writer, null, value, options );
        }
        finally
        {
            _jsonPath.Clear();
        }
    }

    private void WriteInternal( Utf8JsonWriter writer, string name, dynamic value, JsonSerializerOptions options )
    {
        switch ( value )
        {
            case null:
                writer.WriteNull( name );
                break;
            case string s:
                writer.WriteString( name, s );
                break;
            case long l:
                writer.WriteNumber( name, l );
                break;
            case double d:
                writer.WriteNumber( name, d );
                break;
            case bool b:
                writer.WriteBoolean( name, b );
                break;
            case IDictionary<string, object> dict:
                WriteDictionary( writer, name, dict, options );
                break;
            case IEnumerable<object> list:
                WriteList( writer, name, list, options );
                break;
            default:
                writer.WriteString( name, value.ToString() );
                break;
        }
    }

    private void WriteDictionary( Utf8JsonWriter writer, string name, IDictionary<string, object> dict, JsonSerializerOptions options )
    {
        if ( name != null )
            writer.WriteStartObject( name );
        else
            writer.WriteStartObject();

        foreach ( var kvp in dict )
        {
            WriteInternal( writer, kvp.Key, kvp.Value, options );
        }

        writer.WriteEndObject();
    }

    private void WriteList( Utf8JsonWriter writer, string name, IEnumerable<object> list, JsonSerializerOptions options )
    {
        if ( name != null )
            writer.WriteStartArray( name );
        else
            writer.WriteStartArray();

        foreach ( var item in list )
        {
            WriteInternal( writer, null, item, options );
        }

        writer.WriteEndArray();
    }
}
