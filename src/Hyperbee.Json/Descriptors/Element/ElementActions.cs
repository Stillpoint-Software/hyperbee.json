using System.Text.Json;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Descriptors.Element;

internal class ElementActions : INodeActions<JsonElement>
{
    public bool TryParse( ref Utf8JsonReader reader, out JsonElement element )
    {
        try
        {
            if ( JsonDocument.TryParseValue( ref reader, out var document ) )
            {
                element = document.RootElement;
                return true;
            }
        }
        catch
        {
            // ignored: fall through
        }

        element = default;
        return false;
    }

    public bool TryGetFromPointer( in JsonElement node, JsonPathSegment segment, out JsonElement childValue ) =>
        node.TryGetFromJsonPathPointer( segment, out childValue );

    public bool DeepEquals( JsonElement left, JsonElement right ) =>
        left.DeepEquals( right );
}
