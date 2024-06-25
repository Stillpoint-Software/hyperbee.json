using System.Linq.Expressions;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class CountNodeFunction() : FilterExtensionFunction( argumentCount: 1 )
{
    public const string Name = "count";
    private static readonly Expression CountExpression = Expression.Constant( (Func<IEnumerable<JsonNode>, float>) Count );

    protected override Expression GetExtensionExpression( Expression[] arguments )
    {
        return Expression.Invoke( CountExpression, arguments[0] );
    }

    public static float Count( IEnumerable<JsonNode> nodes )
    {
        return nodes.Count();
    }
}
