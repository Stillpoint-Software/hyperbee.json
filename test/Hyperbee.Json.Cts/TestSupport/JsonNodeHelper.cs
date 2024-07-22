using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Cts.TestSupport;

public class JsonNodeDocument( string source ) : IJsonDocument
{
    private JsonNode? Document { get; } = JsonNode.Parse( source );

    public dynamic Root => Document!;
    public IEnumerable<dynamic> Select( string query ) => Document.Select( query );
}

internal static class JsonNodeHelper
{
    private static JsonArray ConvertToJsonArraySet( JsonNode jsonNode )
    {
        if ( jsonNode is JsonArray jsonArray && jsonArray[0] is JsonArray )
            return jsonArray; // already a set

        JsonArray jsonArraySet = new JsonArray( jsonNode );

        return jsonArraySet;
    }

    private static JsonArray ConvertToJsonArray( IEnumerable<JsonNode> nodes, bool force = false )
    {
        var nodeArray = nodes.ToArray();

        if ( !force && nodeArray.Length == 1 && nodeArray[0] is JsonArray array )
            return array; // already an array

        var jsonArray = new JsonArray();

        foreach ( var node in nodeArray )
        {
            jsonArray.Add( CopyNode( node ) );
        }

        return jsonArray;

        static JsonNode? CopyNode( JsonNode? node )
        {
            return node == null ? null : JsonNode.Parse( node.ToJsonString() );
        }
    }

    public static bool MatchAny( IEnumerable<JsonNode> results, JsonNode expected )
    {
        var expectedSet = ConvertToJsonArraySet( expected );
        var compare = ConvertToJsonArray( results );
        return expectedSet.Any( expect => JsonNode.DeepEquals( expect, compare ) );
    }

    public static bool MatchOne( IEnumerable<JsonNode> results, JsonNode expected )
    {
        var expect = expected as JsonArray;
        var compare = ConvertToJsonArray( results, force: true );
        return JsonNode.DeepEquals( expect, compare );
    }
}

