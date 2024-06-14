using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class SearchElementFunction( string methodName, IList<string> arguments, ParseExpressionContext context ) : FilterExtensionFunction( methodName, arguments, context )
{
    public const string Name = "search";

    private static readonly MethodInfo SearchMethod;

    static SearchElementFunction()
    {
        SearchMethod = typeof( SearchElementFunction ).GetMethod( nameof( Search ), [typeof( JsonElement ), typeof( string )] );
    }

    public override Expression GetExtensionExpression( string methodName, IList<string> arguments, ParseExpressionContext context )
    {
        if ( arguments.Count != 2 )
        {
            return Expression.Throw( Expression.Constant( new ArgumentException( $"{Name} function has invalid parameter count." ) ) );
        }

        var queryExp = Expression.Constant( arguments[0] );
        var regex = Expression.Constant( arguments[1] );

        return Expression.Call(
            SearchMethod,
            Expression.Call( FilterElementHelper.SelectFirstMethod,
                context.Current,
                context.Root,
                queryExp )
            , regex );
    }

    public static bool Search( JsonElement element, string regex )
    {
        var regexPattern = new Regex( regex.Trim( '\"', '\'' ) );
        var value = element.GetString();

        return value != null && regexPattern.IsMatch( value );
    }
}
