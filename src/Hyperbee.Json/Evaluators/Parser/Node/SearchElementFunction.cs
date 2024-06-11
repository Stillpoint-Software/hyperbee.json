using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Hyperbee.Json.Evaluators.Parser.Node;

public class SearchNodeFunction( string methodName, IList<string> arguments, ParseExpressionContext context ) : FilterExpressionFunction( methodName, arguments, context )
{
    public const string Name = "search";

    private static readonly MethodInfo SearchMethod;

    static SearchNodeFunction()
    {
        SearchMethod = typeof( SearchNodeFunction ).GetMethod( nameof( Search ), [typeof( JsonNode ), typeof( string )] );
    }

    public override Expression GetExpression( string methodName, IList<string> arguments, ParseExpressionContext context )
    {
        if ( arguments.Count != 2 )
        {
            return Expression.Throw( Expression.Constant( new ArgumentException( $"{Name} function has invalid parameter count." ) ) );
        }

        var queryExp = Expression.Constant( arguments[0] );
        var regex = Expression.Constant( arguments[1] );

        return Expression.Call(
            SearchMethod,
            Expression.Call( FilterNodeHelper.SelectFirstMethod,
                context.Current,
                context.Root,
                queryExp )
            , regex );
    }

    public static bool Search( JsonNode node, string regex )
    {
        var regexPattern = new Regex( regex.Trim( '\"', '\'' ) );
        var value = node.GetValue<string>();

        return value != null && regexPattern.IsMatch( value );
    }

}
