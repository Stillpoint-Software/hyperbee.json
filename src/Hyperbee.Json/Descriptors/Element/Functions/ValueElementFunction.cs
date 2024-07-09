using System.Linq.Expressions;
using System.Text.Json;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class ValueElementFunction() : FilterExtensionFunction( argumentCount: 1 )
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

        var nodeArray = ((NodesType<JsonElement>) arg).ToArray();

        if ( nodeArray.Length != 1 )
            return new ValueType<object>( null, true );

        var node = nodeArray.FirstOrDefault();

        return new ValueType<object>( node.ValueKind switch
        {
            JsonValueKind.Number => node.GetSingle(),
            JsonValueKind.String => node.GetString(),
            JsonValueKind.Object => IsNotEmpty( node ),
            JsonValueKind.Array => IsNotEmpty( node ),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => false,
            JsonValueKind.Undefined => false,
            _ => false
        } );

        static bool IsNotEmpty( JsonElement node )
        {
            return node.ValueKind switch
            {
                JsonValueKind.Array => node.EnumerateArray().Count() != 0,
                JsonValueKind.Object => node.EnumerateObject().Count() != 0,
                _ => false
            };
        }
    }
}
