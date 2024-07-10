using System.Linq.Expressions;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class CountNodeFunction() : FilterExtensionFunction( argumentCount: 1 )
{
    public const string Name = "count";
    private static readonly Expression CountExpression = Expression.Constant( (Func<INodeType, INodeType>) Count );

    protected override Expression GetExtensionExpression( Expression[] arguments, bool[] argumentInfo )
    {
        return Expression.Invoke( CountExpression,
            Expression.Convert( arguments[0], typeof( INodeType ) ) );
    }

    public static INodeType Count( INodeType arg )
    {
        if ( arg.Kind != NodeTypeKind.NodeList )
            throw new NotSupportedException( $"Function {Name} must be a node list." ); // TODO:

        var nodes = (NodesType<JsonNode>) arg;

        if ( !nodes.NonSingular && !nodes.Any() )
            return new ValueType<float>( 1 );


        return new ValueType<float>( nodes.Count() );
    }
}
