using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Dynamic;

namespace Hyperbee.Json.Extensions;

public static class JsonHelper
{
    // conversion

    public static dynamic ConvertToDynamic( JsonNode value ) => new DynamicJsonNode( ref value );
    public static dynamic ConvertToDynamic( JsonElement value, string path = null ) => new DynamicJsonElement( ref value, path );
    public static dynamic ConvertToDynamic( JsonDocument value ) => ConvertToDynamic( value.RootElement, "$" );

    public static T ConvertToObject<T>( JsonElement value, JsonSerializerOptions options = null )
        where T : new()
    {
        var bufferWriter = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter( bufferWriter );

        value.WriteTo( writer );
        writer.Flush();

        var reader = new Utf8JsonReader( bufferWriter.WrittenSpan );
        return JsonSerializer.Deserialize<T>( ref reader, options );
    }

    public static T ConvertToObject<T>( JsonNode value, JsonSerializerOptions options = null )
        where T : new()
    {
        var bufferWriter = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter( bufferWriter );

        value.WriteTo( writer );
        writer.Flush();

        var reader = new Utf8JsonReader( bufferWriter.WrittenSpan );
        return JsonSerializer.Deserialize<T>( ref reader, options );
    }
}
