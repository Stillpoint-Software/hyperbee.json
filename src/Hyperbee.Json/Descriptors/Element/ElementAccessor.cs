using System.Text.Json;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Descriptors.Element;

internal class ElementAccessor : INodeAccessor<JsonElement>
{
    public bool CanUsePointer => true;

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

    public bool TryGetFromPointer( in JsonElement element, JsonPathSegment segment, out JsonElement childValue ) =>
        element.TryGetFromJsonPathPointer( segment, out childValue );

    public bool DeepEquals( JsonElement left, JsonElement right ) =>
        left.DeepEquals( right );
}
