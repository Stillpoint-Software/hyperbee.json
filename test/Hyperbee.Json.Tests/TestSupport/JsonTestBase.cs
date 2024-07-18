using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Tests.TestSupport;

public class JsonTestBase
{
    protected static string DocumentDefault { get; set; } = "BookStore.json";

    protected static string ReadJsonString( string resourceName = null )
    {
        using var stream = GetManifestStream( resourceName );
        using var reader = new StreamReader( stream! );
        return reader.ReadToEnd();
    }

    private static Stream GetManifestStream( string resourceName = null )
    {
        resourceName ??= DocumentDefault;

        return Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream( $"Hyperbee.Json.Tests.TestDocuments.{resourceName}" );
    }

    public static TType GetDocument<TType>( string resourceName = null )
    {
        var type = typeof( TType );

        var stream = GetManifestStream( resourceName );

        if ( type == typeof( JsonDocument ) )
            return (TType) (object) JsonDocument.Parse( stream! );

        if ( type == typeof( JsonElement ) )
            return (TType) (object) JsonDocument.Parse( stream! ).RootElement;

        if ( type == typeof( JsonNode ) )
            return (TType) (object) JsonNode.Parse( stream! );

        throw new NotSupportedException();
    }

    public static IJsonDocument GetDocumentAdapter( Type target )
    {
        var source = ReadJsonString();
        return GetDocumentAdapter( target, source );
    }

    public static IJsonDocument GetDocumentAdapter( Type target, string source )
    {
        if ( target == typeof( JsonDocument ) )
            return new JsonPathDocument( source );

        if ( target == typeof( JsonNode ) )
            return new JsonPathNode( source );

        throw new NotSupportedException();
    }
}
