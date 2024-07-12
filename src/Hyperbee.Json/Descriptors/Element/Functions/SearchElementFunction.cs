using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class SearchElementFunction() : FilterExtensionFunction( SearchMethodInfo, FilterExtensionInfo.MustNotCompare )
{
    public const string Name = "search";
    private static readonly MethodInfo SearchMethodInfo = GetMethod<SearchElementFunction>( nameof( Search ) );

    public static INodeType Search( INodeType input, INodeType regex )
    {
        return input switch
        {
            NodesType<JsonElement> nodes when regex is ValueType<string> stringValue =>
                SearchImpl( nodes, stringValue.Value ),
            NodesType<JsonElement> nodes when regex is NodesType<JsonElement> stringValue =>
                SearchImpl( nodes, stringValue.Value.FirstOrDefault().GetString() ),
            _ => Constants.False
        };
    }

    public static INodeType SearchImpl( NodesType<JsonElement> nodes, string regex )
    {
        var value = nodes.FirstOrDefault();

        if ( value.ValueKind != JsonValueKind.String )
            return Constants.False;

        var stringValue = value.GetString() ?? string.Empty;

        var regexPattern = new Regex( IRegexp.ConvertToIRegexp( regex ) );
        return new ValueType<bool>( regexPattern.IsMatch( stringValue ) );
    }
}
