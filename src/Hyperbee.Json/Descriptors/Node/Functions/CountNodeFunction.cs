using System.Reflection;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class CountNodeFunction() : FilterExtensionFunction( CountMethodInfo, FilterExtensionInfo.MustCompare )
{
    public const string Name = "count";
    private static readonly MethodInfo CountMethodInfo = GetMethod<CountNodeFunction>( nameof( Count ) );

    public static INodeType Count( INodeType arg )
    {
        if ( arg.Kind != NodeTypeKind.NodeList )
            throw new NotSupportedException( $"Function {Name} must be a node list." );

        var nodes = (NodesType<JsonNode>) arg;

        if ( nodes.IsNormalized && !nodes.Any() )
            return new ValueType<float>( 1 );

        return new ValueType<float>( nodes.Count() );
    }
}
