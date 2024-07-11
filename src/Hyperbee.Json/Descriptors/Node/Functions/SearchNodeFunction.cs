using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class SearchNodeFunction() : FilterExtensionFunction( argumentCount: 2 )
{
    public const string Name = "search";
    private static readonly Expression SearchExpression = Expression.Constant( (Func<INodeType, INodeType, INodeType>) Search );

    protected override Expression GetExtensionExpression( Expression[] arguments, bool[] argumentInfo )
    {
        return Expression.Invoke( SearchExpression,
            Expression.Convert( arguments[0], typeof( INodeType ) ),
            Expression.Convert( arguments[1], typeof( INodeType ) ) );
    }

    public static INodeType Search( INodeType input, INodeType regex )
    {
        return input switch
        {
            NodesType<JsonNode> nodes when regex is ValueType<string> stringValue =>
                Search( nodes, stringValue.Value ),
            NodesType<JsonNode> nodes when regex is NodesType<JsonNode> stringValue =>
                Search( nodes, stringValue.Value.FirstOrDefault()?.GetValue<string>() ),
            _ => ValueType.False
        };
    }

    public static INodeType Search( NodesType<JsonNode> nodes, string regex )
    {
        var value = nodes.FirstOrDefault();

        if ( value?.GetValueKind() != JsonValueKind.String )
            return ValueType.False;

        var regexPattern = new Regex( IRegexp.ConvertToIRegexp( regex ) );
        return new ValueType<bool>( regexPattern.IsMatch( value.GetValue<string>() ) );
    }
}
