using System.Collections.Generic;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Tests.TestSupport;

public class JsonNodeProxy : IJsonPathProxy
{
    public JsonNodeProxy( string source ) => Internal = JsonNode.Parse( source );

    protected JsonNode Internal { get; set; }
    public object Source => Internal;
    public IEnumerable<dynamic> Select( string query ) => Internal.Select( query );

    public dynamic GetPropertyFromKey( string pathLiteral ) => Internal.GetPropertyFromKey( pathLiteral );

    public IEnumerable<object> ArrayEmpty => [];
}
