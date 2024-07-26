using System.Text.Json;

namespace Hyperbee.Json.Descriptors;

public interface IParserAccessor<TNode>
{
    bool TryParse( ref Utf8JsonReader reader, out TNode value );
}
