using System.Collections.Generic;

namespace Hyperbee.Json.Tests.TestSupport;

public interface IJsonPathProxy
{
    object Source { get; }
    IEnumerable<dynamic> Select( string query );
    dynamic GetPropertyFromPath( string pathLiteral );
    IEnumerable<object> ArrayEmpty { get; }
}
