using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class JsonPathMatchFunction<TType>( string methodName, string[] arguments, Expression currentExpression, Expression rootExpression, IJsonPathScriptEvaluator<TType> evaluator, string context = null ) : ParserExpressionFunction<TType>( methodName, arguments, currentExpression, rootExpression, evaluator, context )
{
    public const string Name = "match";

    // ReSharper disable once StaticMemberInGenericType
    private static readonly MethodInfo MatchMethod;

    static JsonPathMatchFunction()
    {
        MatchMethod = typeof( TType ) == typeof( JsonElement )
            ? typeof( JsonPathMatchFunction<TType> ).GetMethod( nameof( Match ), [typeof( JsonPathElement ), typeof( string )] ) // NOTE: switching to JsonPathElement
            : typeof( JsonPathMatchFunction<TType> ).GetMethod( nameof( Match ), [typeof( TType ), typeof( string )] );
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

        if ( arguments.Length != 2 )
        {
            return Expression.Block(
                Expression.Throw( Expression.Constant( new Exception( $"Invalid use of {Name} function" ) ) ),
                Expression.Constant( 0F )
            );
        }

        var queryExp = Expression.Constant( arguments[0] );
        var regex = Expression.Constant( arguments[1] );
        var evaluatorExp = Expression.Constant( evaluator );

        return Expression.Call(
            MatchMethod,
            Expression.Call( JsonPathHelper<TType>.GetFirstElementMethod,
                currentExpression,
                rootExpression,
                queryExp,
                evaluatorExp )
            , regex );
    }


    public static bool Match( JsonPathElement element, string regex )
    {
        var regexPattern = new Regex( regex.Trim( '\"', '\'' ) );
        var value = element.Value.GetString();

        return value != null && regexPattern.IsMatch( value );
    }

    public static bool Match( JsonNode node, string regex )
    {
        var regexPattern = new Regex( regex.Trim( '\"', '\'' ) );
        var value = node.GetValue<string>();

        return value != null && regexPattern.IsMatch( value );
    }
}
