using System.Linq.Expressions;
using System.Text.Json;
using System.Text.RegularExpressions;
using Hyperbee.Json.Descriptors.Types;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;
using ValueType = Hyperbee.Json.Descriptors.Types.ValueType;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class MatchElementFunction() : FilterExtensionFunction( argumentCount: 2 )
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
            NodesType<JsonElement> nodes when regex is ValueType<string> stringValue =>
                Match( nodes, stringValue.Value ),
            NodesType<JsonElement> nodes when regex is NodesType<JsonElement> stringValue =>
                Match( nodes, stringValue.Value.FirstOrDefault().GetString() ),
            _ => ValueType.False
        };
    }

    public static INodeType Match( NodesType<JsonElement> nodes, string regex )
    {
        var value = nodes.FirstOrDefault();

        if ( value.ValueKind != JsonValueKind.String )
            return ValueType.False;

        var stringValue = value.GetString() ?? string.Empty;

        var regexPattern = new Regex( $"^{IRegexp.ConvertToIRegexp( regex )}$" );
        return new ValueType<bool>( regexPattern.IsMatch( stringValue ) );
    }
}
