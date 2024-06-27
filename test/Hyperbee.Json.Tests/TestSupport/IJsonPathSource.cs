using System.Collections.Generic;

namespace Hyperbee.Json.Tests.TestSupport;

public interface IJsonPathSource
{
    IEnumerable<dynamic> Select( string query );
    dynamic GetPropertyFromPath( string pathLiteral );
}
