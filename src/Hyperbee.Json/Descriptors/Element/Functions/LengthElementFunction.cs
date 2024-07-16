using System.Reflection;
using System.Text.Json;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class LengthElementFunction() : ExtensionFunction( LengthMethod, ExtensionInfo.MustCompare | ExtensionInfo.ExpectNormalized )
{
    public const string Name = "length";
    private static readonly MethodInfo LengthMethod = GetMethod<LengthElementFunction>( nameof( Length ) );

    public static IValueType Length( IValueType input )
    {
        switch ( input.ValueKind )
        {
            case ValueKind.Scalar when input.TryGetValue<string>( out var stringValue ):
                return Scalar.Value( stringValue.Length );

            case ValueKind.NodeList when input.TryGetNode<JsonElement>( out var node ):
                return node.ValueKind switch
                {
                    JsonValueKind.String => Scalar.Value( node.GetString()?.Length ?? 0 ),
                    JsonValueKind.Array => Scalar.Value( node.GetArrayLength() ),
                    JsonValueKind.Object => Scalar.Value( node.EnumerateObject().Count() ),
                    _ => Scalar.Nothing
                };
        }

        return Scalar.Nothing;
    }
}
