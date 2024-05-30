using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class JsonPathMatchFunction<TType>( string methodName, IList<string> arguments, ParseExpressionContext<TType> context ) : ParserExpressionFunction<TType>( methodName, arguments, context )
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

    public override Expression GetExpression( string methodName, IList<string> arguments, ParseExpressionContext<TType> context )
    {
        if ( methodName != Name )
        {
            return Expression.Block(
                Expression.Throw( Expression.Constant( new Exception( $"Invalid function name {methodName} for {Name}" ) ) ),
                Expression.Constant( 0F )
            );
        }

        if ( arguments.Count != 2 )
        {
            return Expression.Block(
                Expression.Throw( Expression.Constant( new Exception( $"Invalid use of {Name} function" ) ) ),
                Expression.Constant( 0F )
            );
        }

        var queryExp = Expression.Constant( arguments[0] );
        var regex = Expression.Constant( arguments[1] );
        var evaluatorExp = Expression.Constant( context.Evaluator );

        return Expression.Call(
            MatchMethod,
            Expression.Call( JsonPathHelper<TType>.GetFirstElementMethod,
                context.Current,
                context.Root,
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
