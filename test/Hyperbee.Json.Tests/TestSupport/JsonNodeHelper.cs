using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Tests.TestSupport;

public class JsonPathNode( string source ) : IJsonDocument
{
    private JsonNode Document { get; } = JsonNode.Parse( source );
    public IEnumerable<dynamic> Select( string query ) => Document.Select( query );

    public dynamic FromJsonPathPointer( string pathLiteral ) => Document.FromJsonPathPointer( pathLiteral );
}

internal static partial class TestHelper
{
    public static bool GetBoolean( JsonNode value ) => value.AsValue().GetValue<bool>();

    public static int GetInt32( JsonNode value ) => value.AsValue().GetValue<int>();

    public static string GetString( JsonNode value, bool minify = false )
    {
        if ( value is not JsonObject && value is not JsonArray )
            return value.AsValue().GetValue<string>();

        var options = new JsonSerializerOptions { WriteIndented = false };

        var result = value.ToJsonString( options );
        return minify ? MinifyJson( result ) : result;
    }
}
