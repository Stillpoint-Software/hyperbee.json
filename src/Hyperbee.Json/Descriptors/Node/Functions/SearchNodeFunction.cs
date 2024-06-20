using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class SearchNodeFunction( string methodName, ParseExpressionContext context )
    : FilterExtensionFunction( methodName, 2, context )
{
    public const string Name = "search";

    private static readonly MethodInfo SearchMethod;

    static SearchNodeFunction()
    {
        SearchMethod = typeof( SearchNodeFunction ).GetMethod( nameof( Search ), [typeof( IEnumerable<JsonNode> ), typeof( string )] );
    }

    public override Expression GetExtensionExpression( string methodName, Expression[] arguments, ParseExpressionContext context )
    {
        if ( arguments.Length != 2 )
        {
            return Expression.Throw( Expression.Constant( new ArgumentException( $"{Name} function has invalid parameter count." ) ) );
        }

        return Expression.Call( SearchMethod, arguments[0], arguments[1] );
    }

    public static bool Search( IEnumerable<JsonNode> nodes, string regex )
    {
        var nodeValue = nodes.FirstOrDefault()?.GetValue<string>();
        if ( nodeValue == null )
        {
            return false;
        }

        var regexPattern = new Regex( regex.Trim( '\"', '\'' ) );
        return regexPattern.IsMatch( nodeValue );
    }
}
