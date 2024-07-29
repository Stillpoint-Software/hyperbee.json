using System.Dynamic;
using System.Text.Json;
using Hyperbee.Json.Core;

namespace Hyperbee.Json.Dynamic;

public class DynamicJsonElement : DynamicObject
{
    private readonly JsonElement Value;
    private readonly JsonPathBuilder PathBuilder = new();

    public static implicit operator double( DynamicJsonElement proxy ) => proxy.Value.GetDouble();
    public static implicit operator decimal( DynamicJsonElement proxy ) => proxy.Value.GetDecimal();
    public static implicit operator short( DynamicJsonElement proxy ) => GetNumberAsInt16( proxy.Value );
    public static implicit operator int( DynamicJsonElement proxy ) => GetNumberAsInt32( proxy.Value );
    public static implicit operator long( DynamicJsonElement proxy ) => GetNumberAsInt64( proxy.Value );
    public static implicit operator bool( DynamicJsonElement proxy ) => proxy.Value.GetBoolean();
    public static implicit operator byte( DynamicJsonElement proxy ) => proxy.Value.GetByte();
    public static implicit operator sbyte( DynamicJsonElement proxy ) => proxy.Value.GetSByte();
    public static implicit operator float( DynamicJsonElement proxy ) => proxy.Value.GetSingle();
    public static implicit operator Guid( DynamicJsonElement proxy ) => proxy.Value.GetGuid();
    public static implicit operator DateTime( DynamicJsonElement proxy ) => proxy.Value.GetDateTime();
    public static implicit operator DateTimeOffset( DynamicJsonElement proxy ) => proxy.Value.GetDateTimeOffset();
    public static implicit operator string( DynamicJsonElement proxy ) => proxy.Value.GetString();

    public DynamicJsonElement( ref JsonElement value )
    {
        Value = value;
    }

    public override bool TryConvert( ConvertBinder binder, out object result )
    {
        result = Value;
        return true;
    }

    public override bool TryGetIndex( GetIndexBinder binder, object[] indexes, out object result )
    {
        if ( Value.ValueKind == JsonValueKind.Array )
        {
            var resultValue = Value[(int) indexes[0]];
            result = new DynamicJsonElement( ref resultValue );
            return true;
        }

        result = null;
        return false;
    }

    public override bool TryGetMember( GetMemberBinder binder, out object result )
    {
        ArgumentNullException.ThrowIfNull( binder );

        if ( Value.ValueKind is JsonValueKind.Array or JsonValueKind.Object )
        {
            if ( Value.TryGetProperty( binder.Name, out var resultValue ) )
            {
                result = new DynamicJsonElement( ref resultValue );
                return true;
            }
        }

        result = null;
        return false;
    }

    public override bool TryInvokeMember( InvokeMemberBinder binder, object[] args, out object result )
    {
        ArgumentNullException.ThrowIfNull( binder );

        if ( binder.Name.Equals( "path", StringComparison.OrdinalIgnoreCase ) )
        {
            result = PathBuilder.GetPath( Value );
            return true;
        }

        result = null;
        return false;
    }

    // Value extensions

    private static short GetNumberAsInt16( JsonElement value )
    {
        if ( value.TryGetInt16( out var number ) )
            return number;

        return (short) value.GetDouble(); // for cases where the number contains fractional digits
    }

    private static int GetNumberAsInt32( JsonElement value )
    {
        if ( value.TryGetInt32( out var number ) )
            return number;

        return (int) value.GetDouble(); // for cases where the number contains fractional digits
    }

    private static long GetNumberAsInt64( JsonElement value )
    {
        if ( value.TryGetInt64( out var number ) )
            return number;

        return (long) value.GetDouble(); // for cases where the number contains fractional digits
    }
}
