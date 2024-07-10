using System.Linq.Expressions;
using System.Text.Json;

using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class CountElementFunction() : FilterExtensionFunction( argumentCount: 1 )
{
    public const string Name = "count";
    private static readonly Expression CountExpression = Expression.Constant( (Func<INodeType, INodeType>) Count );

    protected override Expression GetExtensionExpression( Expression[] arguments, bool[] argumentInfo )
    {
        return Expression.Invoke( CountExpression, arguments[0] );
    }

    public static INodeType Count( INodeType input )
    {
        switch ( input )
        {
            case NodesType<JsonElement> nodes:
                if ( !nodes.NonSingular && !nodes.Any() )
                    return new ValueType<float>( 1F );

                return new ValueType<float>( nodes.Count() );
            case NodesType<string> stringValue:
            case NodesType<float> floatValue:
            default:
                return new ValueType<float>( 1F );
        }
    }
}
