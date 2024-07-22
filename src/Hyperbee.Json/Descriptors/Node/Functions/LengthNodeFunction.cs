using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class LengthNodeFunction() : ExtensionFunction( LengthMethod, CompareConstraint.MustCompare | CompareConstraint.ExpectNormalized )
{
    public const string Name = "length";
    private static readonly MethodInfo LengthMethod = GetMethod<LengthNodeFunction>( nameof( Length ) );

    public static IValueType Length( IValueType argument )
    {
        switch ( argument.ValueKind )
        {
            case ValueKind.Scalar when argument.TryGetValue<string>( out var value ):
                return Scalar.Value( value.Length );

            case ValueKind.NodeList when argument.TryGetNode<JsonNode>( out var node ):
                return node?.GetValueKind() switch
                {
                    JsonValueKind.String => Scalar.Value( node.GetValue<string>()?.Length ?? 0 ),
                    JsonValueKind.Array => Scalar.Value( node.AsArray().Count ),
                    JsonValueKind.Object => Scalar.Value( node.AsObject().Count ),
                    _ => Scalar.Nothing
                };
        }

        return Scalar.Nothing;
    }
}
