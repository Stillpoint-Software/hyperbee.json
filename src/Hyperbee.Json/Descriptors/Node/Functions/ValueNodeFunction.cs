using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class ValueNodeFunction() : FilterExtensionFunction( argumentCount: 1 )
{
    public const string Name = "value";
    public static readonly Expression ValueExpression = Expression.Constant( (Func<INodeType, ValueType<object>>) Value );

    protected override Expression GetExtensionExpression( Expression[] arguments, bool[] argumentInfo )
    {
        return Expression.Invoke( ValueExpression,
            Expression.Convert( arguments[0], typeof( INodeType ) ) );
    }

    public static ValueType<object> Value( INodeType arg )
    {
        if ( arg.Kind != NodeTypeKind.NodeList )
            throw new NotSupportedException( $"Function {Name} does not support kind {arg.Kind}" );

        var nodeArray = ((NodesType<JsonNode>) arg).ToArray();

        if ( nodeArray.Length != 1 )
            return new ValueType<object>( null, true );

        var node = nodeArray.FirstOrDefault();

        return new ValueType<object>( node?.GetValueKind() switch
        {
            JsonValueKind.Number => node.GetNumber<float>(),
            JsonValueKind.String => node.GetValue<string>(),
            JsonValueKind.Object => IsNotEmpty( node ),
            JsonValueKind.Array => IsNotEmpty( node ),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => false,
            JsonValueKind.Undefined => false,
            _ => false
        } );

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
