using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class MatchNodeFunction( string methodName, ParseExpressionContext context ) :
    FilterExtensionFunction( methodName, 2, context )
{
    public const string Name = "match";

    private static readonly MethodInfo MatchMethod;

    static MatchNodeFunction()
    {
        MatchMethod = typeof( MatchNodeFunction ).GetMethod( nameof( Match ), [typeof( IEnumerable<JsonNode> ), typeof( string )] );
    }

    public override Expression GetExtensionExpression( string methodName, Expression[] arguments, ParseExpressionContext context )
    {
        if ( arguments.Length != 2 )
        {
            return Expression.Throw( Expression.Constant( new ArgumentException( $"{Name} function has invalid parameter count." ) ) );
        }

        return Expression.Call( MatchMethod, arguments[0], arguments[1] );
    }

    public static bool Match( IEnumerable<JsonNode> nodes, string regex )
    {
        var nodeValue = nodes.FirstOrDefault()?.GetValue<string>();
        if ( nodeValue == null )
        {
            return false;
        }

        var regexPattern = new Regex( regex.Trim( '\"', '\'' ) );
        var value = $"^{nodeValue}$";

        return regexPattern.IsMatch( value );
    }
}
