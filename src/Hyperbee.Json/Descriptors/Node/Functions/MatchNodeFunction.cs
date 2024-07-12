using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class MatchNodeFunction() : FilterExtensionFunction( MatchMethodInfo, FilterExtensionInfo.MustNotCompare )
{
    public const string Name = "match";
    private static readonly MethodInfo MatchMethodInfo = GetMethod<MatchNodeFunction>( nameof( Match ) );

    public static INodeType Match( INodeType input, INodeType regex )
    {
        return input switch
        {
            NodesType<JsonNode> nodes when regex is ValueType<string> stringValue =>
                MatchImpl( nodes, stringValue.Value ),
            NodesType<JsonNode> nodes when regex is NodesType<JsonNode> stringValue =>
                MatchImpl( nodes, stringValue.Value.FirstOrDefault()?.GetValue<string>() ),
            _ => Constants.False
        };
    }

    private static INodeType MatchImpl( NodesType<JsonNode> nodes, string regex )
    {
        var value = nodes.FirstOrDefault();

        if ( value?.GetValueKind() != JsonValueKind.String )
            return Constants.False;

        var stringValue = value.GetValue<string>();

        var regexPattern = new Regex( $"^{IRegexp.ConvertToIRegexp( regex )}$" );
        return new ValueType<bool>( regexPattern.IsMatch( stringValue ) );
    }
}

