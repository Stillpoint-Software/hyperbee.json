using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Extensions;

public static class JsonPathSelectExtensions
{
    public static IEnumerable<JsonNode> Select( this JsonNode node, string query )
    {
        return JsonPath<JsonNode>.Select( node, query );
    }

    public static IEnumerable<JsonElement> Select( this JsonElement element, string query )
    {
        return JsonPath<JsonElement>.Select( element, query );
    }

    public static IEnumerable<JsonElement> Select( this JsonDocument document, string query )
    {
        return JsonPath<JsonElement>.Select( document.RootElement, query );
    }

    public static IEnumerable<(JsonElement Node, string Path)> SelectPath( this JsonDocument document, string query )
    {
        return document.RootElement.SelectPath( query );
    }

    public static IEnumerable<(JsonElement Node, string Path)> SelectPath( this JsonElement element, string query )
    {
        var pathBuilder = new JsonPathBuilder( element );

        foreach ( var result in JsonPath<JsonElement>.Select( element, query, NodeProcessor ) )
        {
            yield return (result, pathBuilder.GetPath( result ));
        }

        yield break;

        void NodeProcessor( in JsonElement parent, in JsonElement value, string key, in JsonPathSegment segment )
        {
            pathBuilder.InsertItem( parent, value, key ); // seed the path builder with the parent and value
        }
    }
}
