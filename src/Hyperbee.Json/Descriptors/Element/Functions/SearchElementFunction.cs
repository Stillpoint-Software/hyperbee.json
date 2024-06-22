using System.Linq.Expressions;
using System.Text.Json;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class SearchElementFunction() : FilterExtensionFunction( argumentCount: 2 )
{
    private static readonly Expression SearchExpression = Expression.Constant( (Func<IEnumerable<JsonElement>, string, bool>) Search );

    public const string Name = "search";

    public override Expression GetExtensionExpression( Expression[] arguments )
    {
        return Expression.Invoke( SearchExpression, arguments[0], arguments[1] );
    }

    public static bool Search( IEnumerable<JsonElement> elements, string regex )
    {
        var elementValue = elements.FirstOrDefault().GetString();
        if ( elementValue == null )
        {
            return false;
        }

        var regexPattern = new Regex( regex.Trim( '\"', '\'' ) );
        return regexPattern.IsMatch( elementValue );
    }
}
