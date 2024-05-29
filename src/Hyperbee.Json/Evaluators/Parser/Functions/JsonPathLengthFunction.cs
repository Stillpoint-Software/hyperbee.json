using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class JsonPathLengthFunction<TType>( string methodName, string[] arguments, Expression currentExpression, Expression rootExpression, IJsonPathScriptEvaluator<TType> evaluator, string context = null ) : ParserExpressionFunction<TType>( methodName, arguments, currentExpression, rootExpression, evaluator, context )
{
    public const string Name = "length";

    // ReSharper disable once StaticMemberInGenericType
    private static readonly MethodInfo LengthMethod;

    static JsonPathLengthFunction()
    {
        LengthMethod = typeof( TType ) == typeof( JsonElement )
            ? typeof( JsonPathLengthFunction<TType> ).GetMethod( nameof( Length ), [typeof( JsonPathElement )] ) // NOTE: switching to JsonPathElement
            : typeof( JsonPathLengthFunction<TType> ).GetMethod( nameof( Length ), [typeof( TType )] );
    }

    public override Expression GetExpression( string methodName, string[] arguments, Expression currentExpression, Expression rootExpression, IJsonPathScriptEvaluator<TType> evaluator, string context )
    {
        if ( methodName != Name )
        {
            return Expression.Block(
                Expression.Throw( Expression.Constant( new Exception( $"Invalid function name {methodName} for {Name}" ) ) ),
                Expression.Constant( 0F )
            );
        }

        if ( arguments.Length != 1 )
        {
            return Expression.Block(
                Expression.Throw( Expression.Constant( new Exception( $"Invalid use of {Name} function" ) ) ),
                Expression.Constant( 0F )
            );
        }

        var queryExp = Expression.Constant( arguments[0] );
        var evaluatorExp = Expression.Constant( evaluator );

        return Expression.Call(
            LengthMethod,
            Expression.Call( JsonPathHelper<TType>.GetFirstElementMethod,
                currentExpression,
                rootExpression,
                queryExp,
                evaluatorExp ) );
    }


    public static float Length( JsonPathElement element )
    {
        return element.Value.ValueKind switch
        {
            JsonValueKind.String => element.Value.GetString()?.Length ?? 0,
            JsonValueKind.Array => element.Value.GetArrayLength(),
            JsonValueKind.Object => element.Value.EnumerateObject().Count(),
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
