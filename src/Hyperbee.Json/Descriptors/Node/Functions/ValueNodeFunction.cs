using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class ValueNodeFunction( string methodName, ParseExpressionContext context )
    : FilterExtensionFunction( methodName, 1, context )
{
    public const string Name = "value";

    public static readonly MethodInfo ValueMethod;

    static ValueNodeFunction()
    {
        ValueMethod = typeof( ValueNodeFunction ).GetMethod( nameof( Value ), [typeof( IEnumerable<JsonNode> )] );
    }

    public override Expression GetExtensionExpression( string methodName, Expression[] arguments, ParseExpressionContext context )
    {
        if ( arguments.Length != 1 )
        {
            return Expression.Throw( Expression.Constant( new ArgumentException( $"{Name} function has invalid parameter count." ) ) );
        }

        return Expression.Call( ValueMethod, arguments[0] );
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
    }

    private static bool IsNotEmpty( JsonNode node )
    {
        return node.GetValueKind() switch
        {
            JsonValueKind.Array => node.AsArray().Count != 0,
            JsonValueKind.Object => node.AsObject().Count != 0,
            _ => false
        };
    }
}
