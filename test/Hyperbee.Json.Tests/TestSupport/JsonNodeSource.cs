using System.Collections.Generic;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Tests.TestSupport;

public class JsonNodeSource( string source ) : IJsonPathSource
{
    protected JsonNode Internal { get; set; } = JsonNode.Parse( source );
    public IEnumerable<dynamic> Select( string query ) => Internal.Select( query );

    public dynamic GetPropertyFromPath( string pathLiteral ) => Internal.GetPropertyFromPath( pathLiteral );
}
