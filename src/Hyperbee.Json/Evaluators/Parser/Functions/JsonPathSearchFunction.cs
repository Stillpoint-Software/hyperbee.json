using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class JsonPathSearchFunction<TType>( string methodName, IList<string> arguments, ParseExpressionContext<TType> context ) : ParserExpressionFunction<TType>( methodName, arguments, context )
{
    public const string Name = "search";

    // ReSharper disable once StaticMemberInGenericType
    private static readonly MethodInfo SearchMethod;

    static JsonPathSearchFunction()
    {
        SearchMethod = typeof( JsonPathSearchFunction<TType> ).GetMethod( nameof( Search ), [typeof( TType ), typeof( string )] );
    }

    public override Expression GetExpression( string methodName, IList<string> arguments, ParseExpressionContext<TType> context )
    {
        if ( arguments.Count != 2 )
        {
            return Expression.Throw( Expression.Constant( new ArgumentException( $"{Name} function has invalid parameter count." ) ) );
        }

        var queryExp = Expression.Constant( arguments[0] );
        var regex = Expression.Constant( arguments[1] );

        return Expression.Call(
            SearchMethod,
            Expression.Call( JsonPathHelper<TType>.GetFirstElementMethod,
                context.Current,
                context.Root,
                queryExp )
            , regex );
    }


    public static bool Search( JsonElement element, string regex )
    {
        var regexPattern = new Regex( regex.Trim( '\"', '\'' ) );
        var value = element.GetString();

        // TODO: Talk to BF about how search is different from match
        return value != null && regexPattern.IsMatch( value );
    }

    public static bool Search( JsonNode node, string regex )
    {
        var regexPattern = new Regex( regex.Trim( '\"', '\'' ) );
        var value = node.GetValue<string>();

        // TODO: Talk to BF about how search is different from match
        return value != null && regexPattern.IsMatch( value );
    }

}
