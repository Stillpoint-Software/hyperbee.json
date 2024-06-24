using System.Linq.Expressions;
using System.Text.Json;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class SearchElementFunction() : FilterExtensionFunction( argumentCount: 2 )
{
    public const string Name = "search";
    private static readonly Expression SearchExpression = Expression.Constant( (Func<IEnumerable<JsonElement>, string, bool>) Search );

    protected override Expression GetExtensionExpression( Expression[] arguments )
    {
        return Expression.Invoke( SearchExpression, arguments[0], arguments[1] );
    }

    public static bool Search( IEnumerable<JsonElement> elements, string regex )
    {
        var value = elements.FirstOrDefault().GetString();

        if ( value == null )
        {
            return false;
        }

        var regexPattern = new Regex( regex.Trim( '\"', '\'' ) );
        return regexPattern.IsMatch( value );
    }
}
