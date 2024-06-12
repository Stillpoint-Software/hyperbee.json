using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class LengthNodeFunction( string methodName, IList<string> arguments, ParseExpressionContext context ) : FilterExtensionFunction( methodName, arguments, context )
{
    public const string Name = "length";

    private static readonly MethodInfo LengthMethod;

    static LengthNodeFunction()
    {
        LengthMethod = typeof( LengthNodeFunction ).GetMethod( nameof( Length ), [typeof( JsonNode )] );
    }

    public override Expression GetExtensionExpression( string methodName, IList<string> arguments, ParseExpressionContext context )
    {
        if ( arguments.Count != 1 )
        {
            return Expression.Throw( Expression.Constant( new ArgumentException( $"{Name} function has invalid parameter count." ) ) );
        }

        var queryExp = Expression.Constant( arguments[0] );

        return Expression.Call(
            LengthMethod,
            Expression.Call( FilterNodeHelper.SelectFirstMethod,
                context.Current,
                context.Root,
                queryExp ) );
    }

    public static float Length( JsonNode node )
    {
        return node.GetValueKind() switch
        {
            JsonValueKind.String => node.GetValue<string>()?.Length ?? 0,
            JsonValueKind.Array => node.AsArray().Count,
            JsonValueKind.Object => node.AsObject().Count,
            _ => 0
        };
    }
}
