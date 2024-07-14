using System.Reflection;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class CountNodeFunction() : FilterExtensionFunction( CountMethodInfo, FilterExtensionInfo.MustCompare )
{
    public const string Name = "count";
    private static readonly MethodInfo CountMethodInfo = GetMethod<CountNodeFunction>( nameof( Count ) );

    public static ScalarValue<int> Count( IValueType argument )
    {
        if ( argument.ValueKind != ValueKind.NodeList )
            throw new NotSupportedException( $"Function `{Name}` must be a node list." );

        var nodes = (NodeList<JsonNode>) argument;

        if ( nodes.IsNormalized && !nodes.Any() )
            return 1;

        return nodes.Count();
    }
}
