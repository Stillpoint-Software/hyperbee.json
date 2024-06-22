using System.Linq.Expressions;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class MatchNodeFunction() : FilterExtensionFunction( argumentCount: 2 )
{
    public const string Name = "match";
    private static readonly Expression MatchExpression = Expression.Constant( (Func<IEnumerable<JsonNode>, string, bool>) Match );

    protected override Expression GetExtensionExpression( Expression[] arguments )
    {
        return Expression.Invoke( MatchExpression, arguments[0], arguments[1] );
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
