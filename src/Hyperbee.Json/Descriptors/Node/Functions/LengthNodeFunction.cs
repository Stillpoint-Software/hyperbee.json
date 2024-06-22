using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class LengthNodeFunction() : FilterExtensionFunction( argumentCount: 1 )
{
    private static readonly Expression LengthExpression = Expression.Constant( (Func<IEnumerable<JsonNode>, float>) Length );

    public const string Name = "length";

    public override Expression GetExtensionExpression( Expression[] arguments )
    {
        return Expression.Invoke( LengthExpression, arguments[0] );
    }

    public static float Length( IEnumerable<JsonNode> nodes )
    {
        var node = nodes.FirstOrDefault();
        return node?.GetValueKind() switch
        {
            JsonValueKind.String => node.GetValue<string>()?.Length ?? 0,
            JsonValueKind.Array => node.AsArray().Count,
            JsonValueKind.Object => node.AsObject().Count,
            _ => 0
        };
    }
}
