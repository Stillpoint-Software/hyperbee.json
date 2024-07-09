using System.Linq.Expressions;
using System.Text.Json;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class MatchElementFunction() : FilterExtensionFunction( argumentCount: 2 )
{
    public const string Name = "match";
    private static readonly Expression MatchExpression = Expression.Constant( (Func<INodeType, INodeType, bool>) Match );

    protected override Expression GetExtensionExpression( Expression[] arguments, bool[] argumentInfo )
    {
        return Expression.Invoke( MatchExpression,
            Expression.Convert( arguments[0], typeof( INodeType ) ),
            Expression.Convert( arguments[1], typeof( INodeType ) ) );
    }

    public static bool Match( INodeType input, INodeType regex )
    {
        if ( input is NodesType<JsonElement> nodes )
        {
            return regex switch
            {
                NodesType<JsonElement> { NonSingular: false } nodeType => Match( nodes, nodeType.FirstOrDefault().GetString() ),
                ValueType<string> stringType => Match( nodes, stringType.Value ),
                ValueType<object> objectType => Match( nodes, objectType.Value as string ),
                _ => false
            };
        }

        return false;
    }

    public static bool Match( NodesType<JsonElement> nodes, string regex )
    {
        var value = nodes.FirstOrDefault();

        if ( value.ValueKind != JsonValueKind.String )
        {
            return false;
        }

        var stringValue = value.GetString();

        var regexPattern = new Regex( $"^{regex.Trim( '\"', '\'' )}$" );
        return regexPattern.IsMatch( stringValue );
    }
}
