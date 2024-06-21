using System.Linq.Expressions;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class CountNodeFunction( ParseExpressionContext context ) : FilterExtensionFunction( argumentCount: 1, context )
{
    public const string Name = "count";
    private static readonly Expression CountExpression = Expression.Constant( (Func<IEnumerable<JsonNode>, float>) Count );

    public override Expression GetExtensionExpression( Expression[] arguments, ParseExpressionContext context )
    {
        return Expression.Invoke( CountExpression, arguments[0] );
    }

    public static float Count( IEnumerable<JsonNode> nodes )
    {
        return nodes.Count();
    }
}
