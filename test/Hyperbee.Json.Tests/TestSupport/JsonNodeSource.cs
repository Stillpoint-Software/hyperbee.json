using System.Collections.Generic;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Tests.TestSupport;

public class JsonNodeSource( string source ) : IJsonPathSource
{
    private JsonNode Document { get; } = JsonNode.Parse( source );
    public IEnumerable<dynamic> Select( string query ) => Document.Select( query );

    public dynamic FromJsonPathPointer( string pathLiteral ) => Document.FromJsonPathPointer( pathLiteral );
}
