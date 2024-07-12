using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class ValueNodeFunction() : FilterExtensionFunction( ValueMethodInfo, FilterExtensionInfo.MustCompare )
{
    public const string Name = "value";
    private static readonly MethodInfo ValueMethodInfo = GetMethod<ValueNodeFunction>( nameof(Value) );

    public static INodeType Value( INodeType arg )
    {
        if ( arg.Kind != NodeTypeKind.NodeList )
            throw new NotSupportedException( $"Function {Name} does not support kind {arg.Kind}" );

        var nodeArray = ((NodesType<JsonNode>) arg).ToArray();

        if ( nodeArray.Length != 1 )
            return ValueType.Nothing;

        var node = nodeArray.FirstOrDefault();

        return node?.GetValueKind() switch
        {
            JsonValueKind.Number => new ValueType<float>( node.GetValue<float>() ),
            JsonValueKind.String => new ValueType<string>( node.GetValue<string>() ),
            JsonValueKind.Object or JsonValueKind.Array => new ValueType<bool>( IsNotEmpty( node ) ),
            JsonValueKind.True => ValueType.True,
            JsonValueKind.False or JsonValueKind.Null or JsonValueKind.Undefined => ValueType.False,
            _ => ValueType.False
        };

        static bool IsNotEmpty( JsonNode node )
        {
            return node.GetValueKind() switch
            {
                JsonValueKind.Array => node.AsArray().Count != 0,
                JsonValueKind.Object => node.AsObject().Count != 0,
                _ => false
            };
        }
    }
}
