using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Tests.TestSupport;

public class JsonDocumentProxy( string source ) : IJsonPathProxy
{
    protected JsonDocument Internal { get; set; } = JsonDocument.Parse( source );
    public object Source => Internal;
    public IEnumerable<dynamic> Select( string query ) => Internal.Select( query ).Cast<object>();
    public dynamic GetPropertyFromKey( string pathLiteral ) => Internal.RootElement.GetPropertyFromPath( pathLiteral );
    public IEnumerable<object> ArrayEmpty => Array.Empty<JsonElement>().Cast<object>();
}
