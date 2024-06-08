using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class JsonPathLengthFunction<TType>( string methodName, IList<string> arguments, ParseExpressionContext<TType> context ) : ParserExpressionFunction<TType>( methodName, arguments, context )
{
    public const string Name = "length";

    // ReSharper disable once StaticMemberInGenericType
    private static readonly MethodInfo LengthMethod;

    static JsonPathLengthFunction()
    {
        LengthMethod = typeof( JsonPathLengthFunction<TType> ).GetMethod( nameof( Length ), [typeof( TType )] );
    }

    public override Expression GetExpression( string methodName, IList<string> arguments, ParseExpressionContext<TType> context )
    {
        if ( arguments.Count != 1 )
        {
            return //Expression.Block(
                Expression.Throw( Expression.Constant( new ArgumentException( $"{Name} function has invalid parameter count." ) ) );//,
                //Expression.Constant( 0F )
            //);
        }

        var queryExp = Expression.Constant( arguments[0] );
        var evaluatorExp = Expression.Constant( context.Evaluator );

        return Expression.Call(
            LengthMethod,
            Expression.Call( JsonPathHelper<TType>.GetFirstElementMethod,
                context.Current,
                context.Root,
                queryExp,
                evaluatorExp ) );
    }


    public static float Length( JsonElement element )
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString()?.Length ?? 0,
            JsonValueKind.Array => element.GetArrayLength(),
            JsonValueKind.Object => element.EnumerateObject().Count(),
            _ => 0
        };
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
