using System.Text.Json;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Path;
using Hyperbee.Json.Pointer;
using Hyperbee.Json.Query;

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

    public bool TryGetFromPointer( in JsonElement node, JsonSegment segment, out JsonElement value ) =>
        SegmentPointer<JsonElement>.TryGetFromPointer( node, segment, out _, out value );

    public bool DeepEquals( JsonElement left, JsonElement right ) =>
        left.DeepEquals( right );

    public IEnumerable<(JsonElement Value, string Key)> GetChildren( in JsonElement value, bool complexTypesOnly = false )
    {
        // allocating is faster than using yield return and less memory intensive.
        // using stack results in fewer overall allocations than calling reverse,
        // which internally allocates, and then discards, a new array.

        switch ( value.ValueKind )
        {
            case JsonValueKind.Array:
                {
                    var length = value.GetArrayLength();
                    var results = new Stack<(JsonElement, string)>( length ); // stack will reverse items

                    for ( var index = 0; index < length; index++ )
                    {
                        var child = value[index];

                        if ( complexTypesOnly && child.ValueKind is not (JsonValueKind.Array or JsonValueKind.Object) )
                            continue;

                        results.Push( (child, IndexHelper.GetIndexString( index )) );
                    }

                    return results;
                }
            case JsonValueKind.Object:
                {
                    var results = new Stack<(JsonElement, string)>(); // stack will reverse items

                    foreach ( var child in value.EnumerateObject() )
                    {
                        if ( complexTypesOnly && child.Value.ValueKind is not (JsonValueKind.Array or JsonValueKind.Object) )
                            continue;

                        results.Push( (child.Value, child.Name) );
                    }

                    return results;
                }
        }

        return [];
    }
}
