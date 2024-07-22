using System.Reflection;
using System.Text.Json;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class LengthElementFunction() : ExtensionFunction( LengthMethod, CompareConstraint.MustCompare | CompareConstraint.ExpectNormalized )
{
    public const string Name = "length";
    private static readonly MethodInfo LengthMethod = GetMethod<LengthElementFunction>( nameof( Length ) );

    public static IValueType Length( IValueType argument )
    {
        switch ( argument.ValueKind )
        {
            case ValueKind.Scalar when argument.TryGetValue<string>( out var value ):
                return Scalar.Value( value.Length );

            case ValueKind.NodeList when argument.TryGetNode<JsonElement>( out var node ):
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
