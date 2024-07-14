using System.Reflection;
using System.Text.Json;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class ValueElementFunction() : FilterExtensionFunction( ValueMethodInfo, FilterExtensionInfo.MustCompare )
{
    public const string Name = "value";
    private static readonly MethodInfo ValueMethodInfo = GetMethod<ValueElementFunction>( nameof( Value ) );

    public static IValueType Value( IValueType argument )
    {
        if ( argument is not NodeList<JsonElement> nodes )
            throw new NotSupportedException( $"Function `{Name}` does not support kind {argument.Kind}" );

        var node = nodes.OneOrDefault();

        return node.ValueKind switch
        {
            JsonValueKind.Number when node.TryGetInt32( out var intValue ) => Scalar.Value( intValue ),
            JsonValueKind.Number => Scalar.Value( node.GetSingle() ),
            JsonValueKind.String => Scalar.Value( node.GetString() ),
            JsonValueKind.Object => Scalar.Value( node.EnumerateObject().Any() ),
            JsonValueKind.Array => Scalar.Value( node.GetArrayLength() != 0 ),
            JsonValueKind.True => Scalar.True,
            JsonValueKind.False => Scalar.False,
            _ => Scalar.Nothing
        };
    }
}
