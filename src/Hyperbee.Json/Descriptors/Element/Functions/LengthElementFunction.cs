using System.Reflection;
using System.Text.Json;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class LengthElementFunction() : FilterExtensionFunction( LengthMethodInfo, FilterExtensionInfo.MustCompare | FilterExtensionInfo.ExpectNormalized )
{
    public const string Name = "length";
    private static readonly MethodInfo LengthMethodInfo = GetMethod<LengthElementFunction>( nameof( Length ) );

    public static INodeType Length( INodeType input )
    {
        return input switch
        {
            NodesType<JsonElement> nodes => LengthImpl( nodes.FirstOrDefault() ),
            ValueType<string> valueString => new ValueType<float>( valueString.Value.Length ),
            Null or Nothing => input,
            _ => ValueType.Nothing
        };
    }

    public static INodeType LengthImpl( object value )
    {
        return value switch
        {
            string str => new ValueType<float>( str.Length ),
            Array array => new ValueType<float>( array.Length ),
            System.Collections.ICollection collection => new ValueType<float>( collection.Count ),
            System.Collections.IEnumerable enumerable => new ValueType<float>( enumerable.Cast<object>().Count() ),
            JsonElement node => node.ValueKind switch
            {
                JsonValueKind.String => new ValueType<float>( node.GetString()?.Length ?? 0 ),
                JsonValueKind.Array => new ValueType<float>( node.EnumerateArray().Count() ),
                JsonValueKind.Object => new ValueType<float>( node.EnumerateObject().Count() ),
                _ => ValueType.Null
            },
            _ => ValueType.Null
        };
    }
}
