using System.Linq.Expressions;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class SearchNodeFunction( ParseExpressionContext context ) : FilterExtensionFunction( argumentCount: 2, context )
{
    public const string Name = "search";
    private static readonly Expression SearchExpression = Expression.Constant( (Func<IEnumerable<JsonNode>, string, bool>) Search );

    public override Expression GetExtensionExpression( Expression[] arguments, ParseExpressionContext context )
    {
        return Expression.Invoke( SearchExpression, arguments[0], arguments[1] );
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
