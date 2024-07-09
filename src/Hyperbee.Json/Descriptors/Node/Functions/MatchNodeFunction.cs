using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class MatchNodeFunction() : FilterExtensionFunction( argumentCount: 2 )
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
        if ( input is NodesType<JsonNode> nodes )
        {
            return regex switch
            {
                NodesType<JsonNode> { NonSingular: false } nodeType => Match( nodes, nodeType.FirstOrDefault().GetValue<string>() ),
                ValueType<string> stringType => Match( nodes, stringType.Value ),
                ValueType<object> objectType => Match( nodes, objectType.Value as string ),
                _ => false
            };
        }

        return false;
    }

    public static bool Match( NodesType<JsonNode> nodes, string regex )
    {
        var value = nodes.FirstOrDefault();

        if ( value == null )
        {
            return false;
        }

        if( value.GetValueKind() != JsonValueKind.String )
        {
            return false;
        }

        var stringValue = value.GetValue<string>();

        var regexPattern = new Regex( $"^{regex.Trim( '\"', '\'' )}$" );
        return regexPattern.IsMatch( stringValue );
    }
}
