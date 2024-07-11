using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class MatchNodeFunction() : FilterExtensionFunction( argumentCount: 2 )
{
    public const string Name = "match";
    private static readonly Expression MatchExpression = Expression.Constant( (Func<INodeType, INodeType, INodeType>) Match );

    protected override Expression GetExtensionExpression( Expression[] arguments, bool[] argumentInfo )
    {
        return Expression.Invoke( MatchExpression,
            Expression.Convert( arguments[0], typeof( INodeType ) ),
            Expression.Convert( arguments[1], typeof( INodeType ) ) );
    }

    public static INodeType Match( INodeType input, INodeType regex )
    {
        return input switch
        {
            NodesType<JsonNode> nodes when regex is ValueType<string> stringValue =>
                Match( nodes, stringValue.Value ),
            NodesType<JsonNode> nodes when regex is NodesType<JsonNode> stringValue =>
                Match( nodes, stringValue.Value.FirstOrDefault()?.GetValue<string>() ),
            _ => ValueType.False
        };
    }

    private static INodeType Match( NodesType<JsonNode> nodes, string regex )
    {
        var value = nodes.FirstOrDefault();

        if ( value?.GetValueKind() != JsonValueKind.String )
            return ValueType.False;

        var stringValue = value.GetValue<string>();

        var regexPattern = new Regex( $"^{IRegexp.ConvertToIRegexp( regex )}$" );
        return new ValueType<bool>( regexPattern.IsMatch( stringValue ) );
    }
}

