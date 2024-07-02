using System.Collections.Generic;

namespace Hyperbee.Json.Tests.TestSupport;

public interface IJsonPathSource
{
    IEnumerable<dynamic> Select( string query );
    dynamic FromJsonPathPointer( string pathLiteral );
}
