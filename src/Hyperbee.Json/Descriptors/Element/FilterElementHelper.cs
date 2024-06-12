using System.Reflection;
using System.Text.Json;

namespace Hyperbee.Json.Descriptors.Element;

public static class FilterElementHelper
{
    public static readonly MethodInfo SelectFirstElementValueMethod;
    public static readonly MethodInfo SelectFirstMethod;
    public static readonly MethodInfo SelectElementsMethod;

    static FilterElementHelper()
    {
        var thisType = typeof( FilterElementHelper );

        SelectFirstElementValueMethod = thisType.GetMethod( nameof( SelectFirstElementValue ), [typeof( JsonElement ), typeof( JsonElement ), typeof( string )] );
        SelectFirstMethod = thisType.GetMethod( nameof( SelectFirst ), [typeof( JsonElement ), typeof( JsonElement ), typeof( string )] );
        SelectElementsMethod = thisType.GetMethod( nameof( SelectElements ), [typeof( JsonElement ), typeof( JsonElement ), typeof( string )] );
    }

    private static bool IsNotEmpty( JsonElement element )
    {
        return element.ValueKind switch
        {
            JsonValueKind.Array => element.EnumerateArray().Any(),
            JsonValueKind.Object => element.EnumerateObject().Any(),
            _ => false
        };
    }

    public static object SelectFirstElementValue( JsonElement current, JsonElement root, string query )
    {
        var element = SelectFirst( current, root, query );

        return element.ValueKind switch
        {
            JsonValueKind.Number => element.GetSingle(),
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Object => IsNotEmpty( element ),
            JsonValueKind.Array => IsNotEmpty( element ),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => false,
            JsonValueKind.Undefined => false,
            _ => false
        };
    }

    public static JsonElement SelectFirst( JsonElement current, JsonElement root, string query )
    {
        return SelectElements( current, root, query )
            .FirstOrDefault();
    }

    public static IEnumerable<JsonElement> SelectElements( JsonElement current, JsonElement root, string query )
    {
        return JsonPath<JsonElement>
            .Select( current, root, query );
    }
}
