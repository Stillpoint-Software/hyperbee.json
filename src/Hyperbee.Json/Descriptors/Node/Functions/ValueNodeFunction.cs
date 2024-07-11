using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class ValueNodeFunction() : FilterExtensionFunction( argumentCount: 1 )
{
    public const string Name = "value";
    public static readonly Expression ValueExpression = Expression.Constant( (Func<INodeType, INodeType>) Value );

    protected override Expression GetExtensionExpression( Expression[] arguments, bool[] argumentInfo )
    {
        return Expression.Invoke( ValueExpression,
            Expression.Convert( arguments[0], typeof( INodeType ) ) );
    }

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
