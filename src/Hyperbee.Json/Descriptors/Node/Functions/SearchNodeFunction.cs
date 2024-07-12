using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class SearchNodeFunction() : FilterExtensionFunction( SearchMethodInfo, FilterExtensionInfo.MustNotCompare )
{
    public const string Name = "search";
    private static readonly MethodInfo SearchMethodInfo = GetMethod<SearchNodeFunction>( nameof( Search ) );

    public static INodeType Search( INodeType input, INodeType regex )
    {
        return input switch
        {
            NodesType<JsonNode> nodes when regex is ValueType<string> stringValue =>
                SearchImpl( nodes, stringValue.Value ),
            NodesType<JsonNode> nodes when regex is NodesType<JsonNode> stringValue =>
                SearchImpl( nodes, stringValue.Value.FirstOrDefault()?.GetValue<string>() ),
            _ => Constants.False
        };
    }

    public static INodeType SearchImpl( NodesType<JsonNode> nodes, string regex )
    {
        var value = nodes.FirstOrDefault();

        if ( value?.GetValueKind() != JsonValueKind.String )
            return Constants.False;

        var regexPattern = new Regex( IRegexp.ConvertToIRegexp( regex ) );
        return new ValueType<bool>( regexPattern.IsMatch( value.GetValue<string>() ) );
    }
}
