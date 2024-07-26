using System.Text.Json;

namespace Hyperbee.Json.Descriptors.Element;

internal class ElementParserAccessor : IParserAccessor<JsonElement>
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
}
