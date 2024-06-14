using System.Dynamic;
using System.Text.Json;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Dynamic;

public class DynamicJsonElement : DynamicObject
{
    private readonly JsonElement Value;
    private readonly string Path;

    public static implicit operator double( DynamicJsonElement proxy ) => proxy.Value.GetDouble();
    public static implicit operator decimal( DynamicJsonElement proxy ) => proxy.Value.GetDecimal();
    public static implicit operator short( DynamicJsonElement proxy ) => proxy.Value.GetNumberAsInt16();
    public static implicit operator int( DynamicJsonElement proxy ) => proxy.Value.GetNumberAsInt32();
    public static implicit operator long( DynamicJsonElement proxy ) => proxy.Value.GetNumberAsInt64();
    public static implicit operator bool( DynamicJsonElement proxy ) => proxy.Value.GetBoolean();
    public static implicit operator byte( DynamicJsonElement proxy ) => proxy.Value.GetByte();
    public static implicit operator sbyte( DynamicJsonElement proxy ) => proxy.Value.GetSByte();
    public static implicit operator float( DynamicJsonElement proxy ) => proxy.Value.GetSingle();
    public static implicit operator Guid( DynamicJsonElement proxy ) => proxy.Value.GetGuid();
    public static implicit operator DateTime( DynamicJsonElement proxy ) => proxy.Value.GetDateTime();
    public static implicit operator DateTimeOffset( DynamicJsonElement proxy ) => proxy.Value.GetDateTimeOffset();
    public static implicit operator string( DynamicJsonElement proxy ) => proxy.Value.GetString();

    public DynamicJsonElement( ref JsonElement value, string path )
    {
        Value = value;
        Path = path ?? string.Empty;
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
            result = new DynamicJsonElement( ref resultValue, Path + $"[{indexes[0]}]" );
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
                result = new DynamicJsonElement( ref resultValue, Path + $"['{binder.Name}']" );
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
            result = Path;
            return true;
        }

        result = null;
        return false;
    }
}
