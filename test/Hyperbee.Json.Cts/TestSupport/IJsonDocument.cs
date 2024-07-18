
namespace Hyperbee.Json.Cts.TestSupport;

public interface IJsonDocument
{
    public dynamic Root { get; }
    IEnumerable<dynamic> Select( string query );
}
