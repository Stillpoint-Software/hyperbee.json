using System.Reflection;
using System.Text.Json;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class LengthElementFunction() : ExtensionFunction( LengthMethod, CompareConstraint.MustCompare | CompareConstraint.ExpectNormalized )
{
    public const string Name = "length";
    private static readonly MethodInfo LengthMethod = GetMethod<LengthElementFunction>( nameof( Length ) );

    public static ScalarValue<int> Length( IValueType argument )
    {
        return argument.ValueKind switch
        {
            ValueKind.Scalar when argument.TryGetValue<string>( out var value ) => value.Length,
            ValueKind.NodeList when argument.TryGetNode<JsonElement>( out var node ) => node.ValueKind switch
            {
                JsonValueKind.String => node.GetString()?.Length ?? 0,
                JsonValueKind.Array => node.GetArrayLength(),
                JsonValueKind.Object => node.EnumerateObject().Count(),
                _ => Scalar.Nothing
            },
            _ => Scalar.Nothing
        };
    }
}
