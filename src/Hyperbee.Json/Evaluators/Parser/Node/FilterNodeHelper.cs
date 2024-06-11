using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Evaluators.Parser.Node;

public static class FilterNodeHelper
{
    public static readonly MethodInfo SelectFirstElementValueMethod;
    public static readonly MethodInfo SelectFirstMethod;
    public static readonly MethodInfo SelectElementsMethod;

    static FilterNodeHelper()
    {
        var thisType = typeof( FilterNodeHelper );

        SelectFirstElementValueMethod = thisType.GetMethod( nameof( SelectFirstElementValue ), [typeof( JsonNode ), typeof( JsonNode ), typeof( string )] );
        SelectFirstMethod = thisType.GetMethod( nameof( SelectFirst ), [typeof( JsonNode ), typeof( JsonNode ), typeof( string )] );
        SelectElementsMethod = thisType.GetMethod( nameof( SelectElements ), [typeof( JsonNode ), typeof( JsonNode ), typeof( string )] );
    }

    private static bool IsNotEmpty( JsonNode node )
    {
        return node.GetValueKind() switch
        {
            JsonValueKind.Array => node.AsArray().Count != 0,
            JsonValueKind.Object => node.AsObject().Count != 0,
            _ => false
        };
    }

    public static object SelectFirstElementValue( JsonNode current, JsonNode root, string query )
    {
        var node = SelectFirst( current, root, query );

        return node?.GetValueKind() switch
        {
            JsonValueKind.Number => node.GetNumber<float>(),
            JsonValueKind.String => node.GetValue<string>(),
            JsonValueKind.Object => IsNotEmpty( node ),
            JsonValueKind.Array => IsNotEmpty( node ),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => false,
            JsonValueKind.Undefined => false,
            _ => false
        };
    }

    public static JsonNode SelectFirst( JsonNode current, JsonNode root, string query )
    {
        return JsonPath<JsonNode>
            .Select( current, root, query )
            .FirstOrDefault();
    }

    public static IEnumerable<JsonNode> SelectElements( JsonNode current, JsonNode root, string query )
    {
        return JsonPath<JsonNode>
            .Select( current, root, query );
    }
}
