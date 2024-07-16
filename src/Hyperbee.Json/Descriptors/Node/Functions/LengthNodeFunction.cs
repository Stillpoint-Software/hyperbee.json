using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class LengthNodeFunction() : ExtensionFunction( LengthMethod, ExtensionInfo.MustCompare | ExtensionInfo.ExpectNormalized )
{
    public const string Name = "length";
    private static readonly MethodInfo LengthMethod = GetMethod<LengthNodeFunction>( nameof( Length ) );

    public static IValueType Length( IValueType input )
    {
        switch ( input.ValueKind )
        {
            case ValueKind.Scalar when input.TryGetValue<string>( out var stringValue ):
                return Scalar.Value( stringValue.Length );

            case ValueKind.NodeList when input.TryGetNode<JsonNode>( out var node ):
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
