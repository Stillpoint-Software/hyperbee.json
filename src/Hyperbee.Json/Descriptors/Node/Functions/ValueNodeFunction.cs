using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class ValueNodeFunction() : FilterExtensionFunction( argumentCount: 1 )
{
    public const string Name = "value";
    public static readonly Expression ValueExpression = Expression.Constant( (Func<IEnumerable<JsonNode>, object>) Value );

    protected override Expression GetExtensionExpression( Expression[] arguments )
    {
        return Expression.Invoke( ValueExpression, arguments[0] );
    }

    public static object Value( IEnumerable<JsonNode> nodes )
    {
        var node = nodes.FirstOrDefault();

        return node?.GetValueKind() switch
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
