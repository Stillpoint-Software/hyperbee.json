using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public static class JsonPathHelper<TType>
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly MethodInfo GetFirstElementValueMethod;

    // ReSharper disable once StaticMemberInGenericType
    public static readonly MethodInfo GetFirstElementMethod;

    // ReSharper disable once StaticMemberInGenericType
    public static readonly MethodInfo IsTruthyMethod;

    // ReSharper disable once StaticMemberInGenericType
    public static readonly MethodInfo SelectMethod;

    static JsonPathHelper()
    {
        var thisType = typeof( JsonPathHelper<TType> );

        GetFirstElementValueMethod = thisType.GetMethod( nameof( GetFirstElementValue ), [typeof( TType ), typeof( TType ), typeof( string ), typeof( IJsonPathFilterEvaluator<TType> )] );
        GetFirstElementMethod = thisType.GetMethod( nameof( GetFirstElement ), [typeof( TType ), typeof( TType ), typeof( string ), typeof( IJsonPathFilterEvaluator<TType> )] );
        SelectMethod = thisType.GetMethod( nameof( Select ), [typeof( TType ), typeof( TType ), typeof( string ), typeof( IJsonPathFilterEvaluator<TType> )] );

        IsTruthyMethod = thisType.GetMethod( nameof( IsTruthy ) );
    }

    public static bool IsTruthy( object obj ) => !IsFalsy( obj );

    public static bool IsFalsy( object obj )
    {
        return obj switch
        {
            null => true,
            bool boolValue => !boolValue,
            string str => string.IsNullOrEmpty( str ) || str == "false",
            Array array => array.Length == 0,
            IConvertible convertible => !Convert.ToBoolean( convertible ),
            _ => false
        };
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

    private static bool IsNotEmpty( JsonNode node )
    {
        return node.GetValueKind() switch
        {
            JsonValueKind.Array => node.AsArray().Count != 0,
            JsonValueKind.Object => node.AsObject().Count != 0,
            _ => false
        };
    }

    public static object GetFirstElementValue( JsonElement current, JsonElement root, string query, IJsonPathFilterEvaluator<JsonElement> evaluator )
    {
        var first = GetFirstElement( current, root, query, evaluator );

        return first.ValueKind switch
        {
            JsonValueKind.Number => first.GetSingle(),
            JsonValueKind.String => first.GetString(),
            JsonValueKind.Object => IsNotEmpty( first ),
            JsonValueKind.Array => IsNotEmpty( first ),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => false,
            JsonValueKind.Undefined => false,
            _ => false
        };
    }

    public static object GetFirstElementValue( JsonNode current, JsonNode root, string query, IJsonPathFilterEvaluator<JsonNode> evaluator )
    {
        var first = GetFirstElement( current, root, query, evaluator );

        return first?.GetValueKind() switch
        {
            JsonValueKind.Number => first.GetNumber<float>(),
            JsonValueKind.String => first.GetValue<string>(),
            JsonValueKind.Object => IsNotEmpty( first ),
            JsonValueKind.Array => IsNotEmpty( first ),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => false,
            JsonValueKind.Undefined => false,
            _ => false
        };
    }

    public static JsonElement GetFirstElement( JsonElement current, JsonElement root, string query, IJsonPathFilterEvaluator<JsonElement> evaluator )
    {
        return new JsonPath( evaluator )
            .Select( current, root, query )
            .FirstOrDefault();
    }

    public static JsonNode GetFirstElement( JsonNode current, JsonNode root, string query, IJsonPathFilterEvaluator<JsonNode> evaluator )
    {
        return new Nodes.JsonPathNode( evaluator )
            .Select( current, root, query )
            .FirstOrDefault();
    }

    public static IEnumerable<JsonElement> Select( JsonElement current, JsonElement root, string query, IJsonPathFilterEvaluator<JsonElement> evaluator )
    {
        return new JsonPath( evaluator )
            .Select( current, root, query );
    }

    public static IEnumerable<JsonNode> Select( JsonNode current, JsonNode root, string query, IJsonPathFilterEvaluator<JsonNode> evaluator )
    {
        return new Nodes.JsonPathNode( evaluator )
            .Select( current, root, query );
    }

}
