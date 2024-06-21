using System.Linq.Expressions;
using System.Text.Json;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class MatchElementFunction( ParseExpressionContext context ) : FilterExtensionFunction( argumentCount: 2, context )
{
    public const string Name = "match";
    private static readonly Expression MatchExpression = Expression.Constant( (Func<IEnumerable<JsonElement>, string, bool>) Match );

    public override Expression GetExtensionExpression( Expression[] arguments, ParseExpressionContext context )
    {
        return Expression.Invoke( MatchExpression, arguments[0], arguments[1] );
    }

    public static bool Match( IEnumerable<JsonElement> elements, string regex )
    {
        var elementValue = elements.FirstOrDefault().GetString();
        if ( elementValue == null )
        {
            return false;
        }

        var regexPattern = new Regex( regex.Trim( '\"', '\'' ) );
        var value = $"^{elementValue}$";

        return regexPattern.IsMatch( value );
    }
}
