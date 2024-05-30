using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class JsonPathPathFunction<TType>( string methodName, IList<string> arguments, ParseExpressionContext<TType> context ) : ParserExpressionFunction<TType>( methodName, arguments, context )
{
    public const string Name = "path";

    // ReSharper disable once StaticMemberInGenericType
    private static readonly MethodInfo PathMethod;

    static JsonPathPathFunction()
    {
        PathMethod = typeof( TType ) == typeof( JsonElement )
            ? typeof( JsonPathPathFunction<TType> ).GetMethod( nameof( Path ), [typeof( JsonPathElement ), typeof( string )] ) // NOTE: switching to JsonPathElement
            : typeof( JsonPathPathFunction<TType> ).GetMethod( nameof( Path ), [typeof( TType )] );
    }

    public override Expression GetExpression( string methodName, IList<string> arguments, ParseExpressionContext<TType> context )
    {
        if ( methodName != Name )
        {
            return Expression.Block(
                Expression.Throw( Expression.Constant( new Exception( $"Invalid function name {methodName} for {Name}" ) ) ),
                Expression.Constant( 0F )
            );
        }

        if ( arguments.Count != 1 )
        {
            return Expression.Block(
                Expression.Throw( Expression.Constant( new Exception( $"Invalid use of {Name} function" ) ) ),
                Expression.Constant( 0F )
            );
        }

        var queryExp = Expression.Constant( arguments[0] );
        var evaluatorExp = Expression.Constant( context.Evaluator );

        if ( typeof( TType ) == typeof( JsonElement ) )
        {
            return Expression.Call(
                PathMethod,
                Expression.Call( JsonPathHelper<TType>.GetFirstElementMethod,
                    context.Current,
                    context.Root,
                    queryExp,
                    evaluatorExp ),
                context.BasePath );
        }

        return Expression.Call(
            PathMethod,
            Expression.Call( JsonPathHelper<TType>.GetFirstElementMethod,
                context.Current,
                context.Root,
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
