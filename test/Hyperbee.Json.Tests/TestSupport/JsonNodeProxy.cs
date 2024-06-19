using System.Collections.Generic;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Tests.TestSupport;

public class JsonNodeProxy( string source ) : IJsonPathProxy
{
    protected JsonNode Internal { get; set; } = JsonNode.Parse( source );
    public object Source => Internal;
    public IEnumerable<dynamic> Select( string query ) => Internal.Select( query );

    public dynamic GetPropertyFromKey( string pathLiteral ) => Internal.GetPropertyFromPath( pathLiteral );

    public IEnumerable<object> ArrayEmpty => [];
}
