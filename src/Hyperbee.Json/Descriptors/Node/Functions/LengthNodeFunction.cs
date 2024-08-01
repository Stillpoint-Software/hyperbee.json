using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Path.Filters.Parser;
using Hyperbee.Json.Path.Filters.Values;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class LengthNodeFunction() : ExtensionFunction( LengthMethod, CompareConstraint.MustCompare | CompareConstraint.ExpectNormalized )
{
    public const string Name = "length";
    private static readonly MethodInfo LengthMethod = GetMethod<LengthNodeFunction>( nameof( Length ) );

    public static ScalarValue<int> Length( IValueType argument )
    {
        return argument.ValueKind switch
        {
            ValueKind.Scalar when argument.TryGetValue<string>( out var value ) => value.Length,
            ValueKind.NodeList when argument.TryGetNode<JsonNode>( out var node ) => node?.GetValueKind() switch
            {
                JsonValueKind.String => node.GetValue<string>()?.Length ?? 0,
                JsonValueKind.Array => node.AsArray().Count,
                JsonValueKind.Object => node.AsObject().Count,
                _ => Scalar.Nothing
            },
            _ => Scalar.Nothing
        };
    }
}
