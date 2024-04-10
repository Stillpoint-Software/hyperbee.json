using System.Buffers;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Dynamic;

namespace Hyperbee.Json.Extensions;

public static class JsonElementExtensions
{
    // Is operations

    public static bool IsNullOrUndefined( this JsonElement value )
    {
        return value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined;
    }

    public static bool IsObjectOrArray( this JsonElement value )
    {
        return value.ValueKind is JsonValueKind.Array or JsonValueKind.Object;
    }

    // To operations

    public static dynamic ToDynamic( this JsonElement value, string path = null ) => new DynamicJsonElement( ref value, path );
    public static dynamic ToDynamic( this JsonDocument value ) => ToDynamic( value.RootElement, "$" );

    public static JsonNode ToJsonNode( this JsonDocument document )
    {
        return ToJsonNode( document.RootElement );
    }

    public static JsonNode ToJsonNode( this JsonElement element )
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => JsonObject.Create( element ),
            JsonValueKind.Array => JsonArray.Create( element ),
            _ => JsonValue.Create( element )
        };
    }
    public static T ToObject<T>( this JsonElement value, JsonSerializerOptions options = null )
        where T : new()
    {
        var bufferWriter = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter( bufferWriter );

        value.WriteTo( writer );
        writer.Flush();

        var reader = new Utf8JsonReader( bufferWriter.WrittenSpan );
        return JsonSerializer.Deserialize<T>( ref reader, options );
    }

    // Value extensions

    public static short GetNumberAsInt16( this JsonElement value )
    {
        if ( value.TryGetInt16( out var number ) )
            return number;

        return (short) value.GetDouble(); // for cases where the number contains fractional digits
    }

    public static int GetNumberAsInt32( this JsonElement value )
    {
        if ( value.TryGetInt32( out var number ) )
            return number;

        return (int) value.GetDouble(); // for cases where the number contains fractional digits
    }

    public static long GetNumberAsInt64( this JsonElement value )
    {
        if ( value.TryGetInt64( out var number ) )
            return number;

        return (long) value.GetDouble(); // for cases where the number contains fractional digits
    }

    // GetDocument()
    //
    // As of net 6, JsonElement._parent is private. Provide a way to get to the parent document. 
    // Use an expression over reflection. The first-time cost is higher but additional calls are faster.

    private static readonly Func<JsonElement, JsonDocument> ParentAccessor = CreateParentAccessor();

    private static Func<JsonElement, JsonDocument> CreateParentAccessor()
    {
        var param = Expression.Parameter( typeof( JsonElement ), "target" );
        var field = Expression.Field( param, "_parent" );
        return Expression.Lambda<Func<JsonElement, JsonDocument>>( field, param ).Compile();
    }

    public static JsonDocument GetDocument( this JsonElement value ) => ParentAccessor( value );
}
