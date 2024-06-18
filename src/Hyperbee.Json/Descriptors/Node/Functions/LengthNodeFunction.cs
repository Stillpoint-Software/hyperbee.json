using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class LengthNodeFunction( string methodName, ParseExpressionContext context )
    : FilterExtensionFunction( methodName, 1, context )
{
    public const string Name = "length";

    private static readonly MethodInfo LengthMethod;

    static LengthNodeFunction()
    {
        LengthMethod = typeof( LengthNodeFunction ).GetMethod( nameof( Length ), [typeof( IEnumerable<JsonNode> )] );
    }

    public override Expression GetExtensionExpression( string methodName, Expression[] arguments, ParseExpressionContext context )
    {
        if ( arguments.Length != 1 )
        {
            return Expression.Throw( Expression.Constant( new ArgumentException( $"{Name} function has invalid parameter count." ) ) );
        }

        return Expression.Call( LengthMethod, arguments[0] );
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
