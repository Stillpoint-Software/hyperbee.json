using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Hyperbee.Json.Descriptors.Types;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;
using ValueType = Hyperbee.Json.Descriptors.Types.ValueType;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class MatchElementFunction() : FilterExtensionFunction( MatchMethodInfo, FilterExtensionInfo.MustNotCompare )
{
    public const string Name = "match";
    private static readonly MethodInfo MatchMethodInfo = GetMethod<MatchElementFunction>( nameof( Match ) );

    public static INodeType Match( INodeType input, INodeType regex )
    {
        return input switch
        {
            NodesType<JsonElement> nodes when regex is ValueType<string> stringValue =>
                MatchImpl( nodes, stringValue.Value ),
            NodesType<JsonElement> nodes when regex is NodesType<JsonElement> stringValue =>
                MatchImpl( nodes, stringValue.Value.FirstOrDefault().GetString() ),
            _ => ValueType.False
        };
    }

    public static INodeType MatchImpl( NodesType<JsonElement> nodes, string regex )
    {
        var value = nodes.FirstOrDefault();

        if ( value.ValueKind != JsonValueKind.String )
            return ValueType.False;

        var stringValue = value.GetString() ?? string.Empty;

        var regexPattern = new Regex( $"^{IRegexp.ConvertToIRegexp( regex )}$" );
        return new ValueType<bool>( regexPattern.IsMatch( stringValue ) );
    }
}
