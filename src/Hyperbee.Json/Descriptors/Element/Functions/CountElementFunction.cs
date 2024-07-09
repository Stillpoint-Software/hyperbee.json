using System.Linq.Expressions;
using System.Text.Json;

using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class CountElementFunction() : FilterExtensionFunction( argumentCount: 1 )
{
    public const string Name = "count";
    private static readonly Expression CountExpression = Expression.Constant( (Func<INodeType, ValueType<float>>) Count );

    protected override Expression GetExtensionExpression( Expression[] arguments, bool[] argumentInfo )
    {
        return Expression.Invoke( CountExpression, arguments[0] );
    }

    public static ValueType<float> Count( INodeType arg )
    {
        if ( arg.Kind != NodeTypeKind.NodeList )
            return new ValueType<float>( 0 );

        var nodes = (NodesType<JsonElement>) arg;

        if ( !nodes.NonSingular && !nodes.Any() )
            return new ValueType<float>( 1 );

        return new ValueType<float>( nodes.Count() );
    }
}
