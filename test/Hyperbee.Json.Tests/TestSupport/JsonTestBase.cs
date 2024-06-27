using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Tests.TestSupport;

public class JsonTestBase
{
    protected static string DocumentDefault { get; set; } = "JsonPath.json";

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

        var stream = GetManifestStream( filename );

        if ( type == typeof(JsonDocument) )
            return (TType) (object) JsonDocument.Parse( stream! );

        if ( type == typeof(JsonNode) )
            return (TType) (object) JsonNode.Parse( stream! );

        throw new NotSupportedException();
    }

    public static IJsonPathSource GetDocumentFromResource( Type target, string filename = null )
    {
        var source = ReadJsonString( filename );
        return GetDocumentFromSource( target, source );
    }

    public static IJsonPathSource GetDocumentFromSource( Type target, string source )
    {
        if ( target == typeof( JsonDocument ) )
            return new JsonDocumentSource( source );

        if ( target == typeof( JsonNode ) )
            return new JsonNodeSource( source );

        throw new NotSupportedException();
    }
}
