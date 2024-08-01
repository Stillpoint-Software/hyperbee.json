using System.Collections.Generic;

namespace Hyperbee.Json.Tests.TestSupport;

public interface IJsonDocument
{
    IEnumerable<dynamic> Select( string query );
    dynamic FromJsonPathPointer( string pointer );
}
