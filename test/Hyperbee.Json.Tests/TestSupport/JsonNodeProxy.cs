using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Tests.TestSupport;

public class JsonNodeProxy : IJsonPathProxy
{
    public JsonNodeProxy( string source ) => Internal = JsonNode.Parse( source );

    protected JsonNode Internal { get; set; }
    public object Source => Internal;
    public IEnumerable<dynamic> Select( string query ) => Internal.Select( query );
    
    public IEnumerable<JsonPathPair> SelectPath( string query )
    {
        return Internal
            .Select( query )
            .Select( node => new JsonPathPair { 
                Path = node.GetPath(), 
                Value = node 
            } );
    }

    public dynamic GetPropertyFromKey( string pathLiteral ) => Internal.GetPropertyFromKey( pathLiteral );
    
    public IEnumerable<object> ArrayEmpty => Array.Empty<JsonNode>();
}