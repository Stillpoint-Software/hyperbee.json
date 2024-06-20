using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class MatchElementFunction( string methodName, ParseExpressionContext context )
    : FilterExtensionFunction( methodName, 2, context )
{
    public const string Name = "match";

    private static readonly MethodInfo MatchMethod;

    static MatchElementFunction()
    {
        MatchMethod = typeof( MatchElementFunction ).GetMethod( nameof( Match ), [typeof( IEnumerable<JsonElement> ), typeof( string )] );
    }

    public override Expression GetExtensionExpression( string methodName, Expression[] arguments, ParseExpressionContext context )
    {
        if ( arguments.Length != 2 )
        {
            return Expression.Throw( Expression.Constant( new ArgumentException( $"{Name} function has invalid parameter count." ) ) );
        }

        return Expression.Call( MatchMethod, arguments[0], arguments[1] );
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
