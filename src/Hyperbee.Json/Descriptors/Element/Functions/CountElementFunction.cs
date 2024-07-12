using System.Reflection;
using System.Text.Json;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class CountElementFunction() : FilterExtensionFunction( CountMethodInfo, FilterExtensionInfo.MustCompare )
{
    public const string Name = "count";
    private static readonly MethodInfo CountMethodInfo = GetMethod<CountElementFunction>( nameof( Count ) );

    public static INodeType Count( INodeType input )
    {
        switch ( input )
        {
            case NodesType<JsonElement> nodes:
                if ( !nodes.NonSingular && !nodes.Any() )
                    return new ValueType<float>( 1F );
                return new ValueType<float>( nodes.Count() );
            default:
                return new ValueType<float>( 1F );
        }
    }
}
