﻿using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Tests.TestSupport;

public class JsonDocumentSource( string source ) : IJsonPathSource
{
    private JsonDocument Document { get; set; } = JsonDocument.Parse( source );
    public IEnumerable<dynamic> Select( string query ) => Document.Select( query ).Cast<object>();
    public dynamic FromJsonPathPointer( string pathLiteral ) => Document.RootElement.FromJsonPathPointer( pathLiteral );
}
