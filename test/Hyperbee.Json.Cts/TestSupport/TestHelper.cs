using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Cts.TestSupport;

internal static class TestHelper
{
    public static IJsonDocument Parse( Type target, string source )
    {
        if ( target == typeof( JsonElement ) )
            return new JsonElementDocument( source );

        if ( target == typeof( JsonNode ) )
            return new JsonNodeDocument( source );

        throw new NotSupportedException();
    }

    public static bool MatchAny( Type target, IEnumerable<dynamic> results, dynamic expected )
    {
        if ( target == typeof( JsonElement ) )
            return JsonElementHelper.MatchAny( results.Cast<JsonElement>(), expected );

        if ( target == typeof( JsonNode ) )
            return JsonNodeHelper.MatchAny( results.Cast<JsonNode>(), expected );

        throw new NotSupportedException();
    }

    public static bool MatchOne( Type target, IEnumerable<dynamic> results, dynamic expected )
    {
        if ( target == typeof( JsonElement ) )
            return JsonElementHelper.MatchOne( results.Cast<JsonElement>(), expected );

        if ( target == typeof( JsonNode ) )
            return JsonNodeHelper.MatchOne( results.Cast<JsonNode>(), expected );

        throw new NotSupportedException();
    }
}
