using System.Text.Json;
using Hyperbee.Json.Core;

namespace Hyperbee.Json.Extensions;

public static class JsonElementExtensions
{
    public static bool DeepEquals( this JsonElement element1, JsonElement element2, JsonDocumentOptions options = default )
    {
        var comparer = new JsonElementDeepEqualityComparer( options.MaxDepth );
        return comparer.Equals( element1, element2 );
    }
}
