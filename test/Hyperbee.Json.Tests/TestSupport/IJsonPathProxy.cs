using System.Collections.Generic;

namespace Hyperbee.Json.Tests.TestSupport;

public interface IJsonPathProxy
{
    IEnumerable<dynamic> Select( string query );
    dynamic GetPropertyFromPath( string pathLiteral );
    IEnumerable<object> ArrayEmpty { get; }
}
