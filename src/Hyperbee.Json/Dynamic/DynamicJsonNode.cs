using System.Dynamic;
using System.Numerics;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Dynamic;

public class DynamicJsonNode : DynamicObject
{
    private readonly JsonNode Node;

    public static implicit operator short( DynamicJsonNode proxy ) => GetNumber<short>( proxy.Node );
    public static implicit operator int( DynamicJsonNode proxy ) => GetNumber<int>( proxy.Node );
    public static implicit operator long( DynamicJsonNode proxy ) => GetNumber<long>( proxy.Node );
    public static implicit operator double( DynamicJsonNode proxy ) => proxy.Node.GetValue<double>();
    public static implicit operator decimal( DynamicJsonNode proxy ) => proxy.Node.GetValue<decimal>();
    public static implicit operator bool( DynamicJsonNode proxy ) => proxy.Node.GetValue<bool>();
    public static implicit operator byte( DynamicJsonNode proxy ) => proxy.Node.GetValue<byte>();
    public static implicit operator sbyte( DynamicJsonNode proxy ) => proxy.Node.GetValue<sbyte>();
    public static implicit operator float( DynamicJsonNode proxy ) => proxy.Node.GetValue<float>();
    public static implicit operator Guid( DynamicJsonNode proxy ) => proxy.Node.GetValue<Guid>();
    public static implicit operator DateTime( DynamicJsonNode proxy ) => proxy.Node.GetValue<DateTime>();
    public static implicit operator DateTimeOffset( DynamicJsonNode proxy ) => proxy.Node.GetValue<DateTimeOffset>();
    public static implicit operator string( DynamicJsonNode proxy ) => proxy.Node.GetValue<string>();

    public DynamicJsonNode( ref JsonNode node ) => Node = node;

    public override bool TryConvert( ConvertBinder binder, out object result )
    {
        result = Node;
        return true;
    }

    public override bool TryGetIndex( GetIndexBinder binder, object[] indexes, out object result )
    {
        if ( Node is JsonArray )
        {
            var resultValue = Node[(int) indexes[0]];
            result = new DynamicJsonNode( ref resultValue );
            return true;
        }

        result = null;
        return false;
    }

    public override bool TryGetMember( GetMemberBinder binder, out object result )
    {
        ArgumentNullException.ThrowIfNull( binder );

        switch ( Node )
        {
            case JsonObject jObject when jObject.TryGetPropertyValue( binder.Name, out var resultValue ):
                result = new DynamicJsonNode( ref resultValue );
                return true;
            case JsonArray jArray:
                var arrayValue = jArray[binder.Name];
                result = new DynamicJsonNode( ref arrayValue );
                return true;
            default:
                result = null;
                return false;
        }
    }

    public override bool TrySetIndex( SetIndexBinder binder, object[] indexes, object value )
    {
        Node[(int) indexes[0]] = JsonValue.Create( value );
        return true;
    }

    public override bool TrySetMember( SetMemberBinder binder, object value )
    {
        ArgumentNullException.ThrowIfNull( binder );

        Node[binder.Name] = JsonValue.Create( value ); // this works with values, objects, and null
        return true;
    }

    public override bool TryInvokeMember( InvokeMemberBinder binder, object[] args, out object result )
    {
        ArgumentNullException.ThrowIfNull( binder );

        if ( binder.Name.Equals( "path", StringComparison.OrdinalIgnoreCase ) )
        {
            result = Node.GetPath();
            return true;
        }

        result = null;
        return false;
    }

    private static T GetNumber<T>( JsonNode value )
        where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>, INumber<T>
    {
        var source = value.AsValue();

        if ( typeof( T ) == typeof( int ) || typeof( T ) == typeof( long ) || typeof( T ) == typeof( short ) || typeof( T ) == typeof( byte ) )
        {
            if ( source.TryGetValue<T>( out var result ) )
                return result;

            // the value may contain a decimal. convert to integer without rounding.
            // ChangeType rounds values. Cast to integer first to truncate.
            var truncated = (long) source.GetValue<float>();
            var converted = Convert.ChangeType( truncated, typeof( T ) );
            return (T) converted;
        }

        if ( typeof( T ) == typeof( float ) )
            return (T) (IConvertible) source.GetValue<float>();

        throw new NotSupportedException();
    }
}
