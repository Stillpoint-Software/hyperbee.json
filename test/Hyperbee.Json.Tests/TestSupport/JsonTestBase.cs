using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Tests.TestSupport;

public class JsonTestBase
{
    protected static string DocumentDefault { get; set; } = "JsonPath.json";

    protected static JsonDocument ReadJsonDocument( string filename = null )
    {
        using var stream = GetManifestStream( filename );
        return JsonDocument.Parse( stream! );
    }

    protected static JsonNode ReadJsonNode( string filename = null )
    {
        using var stream = GetManifestStream( filename );
        return JsonNode.Parse( stream! );
    }

    protected static string ReadJsonString( string filename = null )
    {
        using var stream = GetManifestStream( filename );
        using var reader = new StreamReader( stream! );
        return reader.ReadToEnd();
    }

    private static Stream GetManifestStream( string filename = null )
    {
        filename ??= DocumentDefault;

        return Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream( $"Hyperbee.Json.Tests.TestDocuments.{filename}" );
    }

    public static TType GetDocument<TType>( string filename = null )
    {
        var type = typeof(TType);

        if ( type == typeof(JsonDocument) )
            return (TType)(object) ReadJsonDocument( filename );

        //if ( type == typeof(JsonElement) )
        //    return (TType)(object) ReadJsonDocument( filename )?.RootElement;

        if ( type == typeof(JsonNode) )
            return (TType)(object) ReadJsonNode( filename );

        throw new NotSupportedException();
    }

    public static object GetDocument( Type target, string filename = null )
    {
        if ( target == typeof(JsonDocument) )
            return GetDocument<JsonDocument>( filename );

        //if ( target == typeof(JsonElement) )
        //    return GetDocument<JsonDocument>( filename )?.RootElement;

        if ( target == typeof(JsonNode) )
            return GetDocument<JsonNode>( filename );

        throw new NotSupportedException();
    }

    public static IJsonPathProxy GetDocumentProxy( Type target, string filename = null )
    {
        var source = ReadJsonString( filename );
        return GetDocumentProxyFromSource( target, source );
    }

    public static IJsonPathProxy GetDocumentProxyFromSource( Type target, string source )
    {
        if ( target == typeof(JsonDocument) )
            return new JsonDocumentProxy( source );

        if ( target == typeof(JsonNode) )
            return new JsonNodeProxy( source );

        throw new NotSupportedException();
    }
}
