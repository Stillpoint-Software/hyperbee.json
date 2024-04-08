using System.Buffers;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Dynamic;

namespace Hyperbee.Json.Extensions;

public static class JsonNodeExtensions
{
    // To operations

    public static dynamic ToDynamic( this JsonNode value ) => new DynamicJsonNode( ref value );

    public static T ToObject<T>( this JsonNode value, JsonSerializerOptions options = null )
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

    public static T GetNumber<T>( this JsonNode value )
        where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>, INumber<T> 
    {
        var source = value.AsValue();

        if ( typeof(T) == typeof(int) || typeof(T) == typeof(long) || typeof(T) == typeof(short) || typeof(T) == typeof(byte) )
        {
            if ( source.TryGetValue<T>( out var result ) )
                return result;

            // the value may contain a decimal. convert to integer without rounding.
            // ChangeType rounds values. Cast to integer first to truncate.
            var truncated = (long) source.GetValue<float>(); 
            var converted = Convert.ChangeType( truncated, typeof(T) );
            return (T) converted;
        }

        if ( typeof(T) == typeof(float) )
            return (T) (IConvertible) source.GetValue<float>();

        throw new NotSupportedException();
    }
}