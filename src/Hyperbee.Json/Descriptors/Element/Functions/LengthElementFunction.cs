using System.Reflection;
using System.Text.Json;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class LengthElementFunction() : FilterExtensionFunction( LengthMethod, FilterExtensionInfo.MustCompare | FilterExtensionInfo.ExpectNormalized )
{
    public const string Name = "length";
    private static readonly MethodInfo LengthMethod = GetMethod<LengthElementFunction>( nameof( Length ) );

    public static IValueType Length( IValueType argument )
    {
        if ( argument.TryGetValue<string>( out var stringValue ) )
        {
            return new ScalarValue<float>( stringValue.Length );
        }

        if ( argument.TryGetNode<JsonElement>( out var node ) )
        {
            return node.ValueKind switch
            {
                JsonValueKind.String => new ScalarValue<float>( node.GetString()?.Length ?? 0 ),
                JsonValueKind.Array => new ScalarValue<float>( node.GetArrayLength() ),
                JsonValueKind.Object => new ScalarValue<float>( node.EnumerateObject().Count() ),
                _ => Scalar.Nothing
            };
        }

        return Scalar.Nothing;
    }
}
