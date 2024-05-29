using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class JsonPathPathFunction<TType>( string methodName, string[] arguments, Expression currentExpression, Expression rootExpression, IJsonPathScriptEvaluator<TType> evaluator, string context = null ) : ParserExpressionFunction<TType>( methodName, arguments, currentExpression, rootExpression, evaluator, context )
{
    public const string Name = "path";

    // ReSharper disable once StaticMemberInGenericType
    private static readonly MethodInfo PathMethod;

    static JsonPathPathFunction()
    {
        PathMethod = typeof(TType) == typeof(JsonElement)
            ? typeof(JsonPathPathFunction<TType>).GetMethod( nameof(Path), [typeof(JsonPathElement), typeof(string)] ) // NOTE: switching to JsonPathElement
            : typeof(JsonPathPathFunction<TType>).GetMethod( nameof(Path), [typeof(TType)] );
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

        if ( typeof(TType) == typeof(JsonElement) )
        {
            return Expression.Call(
                PathMethod,
                Expression.Call( JsonPathHelper<TType>.GetFirstElementMethod,
                    currentExpression,
                    rootExpression,
                    queryExp,
                    evaluatorExp ),
                Expression.Constant( context ) );
        }

        return Expression.Call(
            PathMethod,
            Expression.Call( JsonPathHelper<TType>.GetFirstElementMethod,
                currentExpression,
                rootExpression,
                queryExp,
                evaluatorExp ) );
    }


    public static string Path( JsonPathElement element, string context )
    {
        return element.Path.Replace( "$", context );
    }

    public static string Path( JsonNode node )
    {
        return node.GetPath();
    }
}
