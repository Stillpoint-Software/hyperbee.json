using System.Text.Json;

namespace Hyperbee.Json.Extensions;

public static class JsonElementExtensions
{
    // Deep Equals/Compare extensions

    public static bool DeepEquals( this JsonElement elmA, JsonElement elmB, JsonDocumentOptions options = default )
    {
        var comparer = new JsonElementDeepEqualityComparer( options.MaxDepth );
        return comparer.Equals( elmA, elmB );
    }
}
