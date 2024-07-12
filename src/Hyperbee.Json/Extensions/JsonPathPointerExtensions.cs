using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Extensions;

// DISTINCT from JsonPath these extensions are intended to facilitate 'diving' for Json Properties using
// normalized paths. a normalized path is an absolute path that references a single element.
// similar to JsonPointer but using JsonPath notation.
//
// syntax supports absolute paths; dotted notation, quoted names, and simple bracketed array accessors only.
//
// Json path style wildcard '*', '..', and '[a,b]' multi-result selector notations are NOT supported.

public static class JsonPathPointerExtensions
{
    public static JsonElement FromJsonPathPointer( this JsonElement jsonElement, ReadOnlySpan<char> pointer )
    {
        var query = JsonPathQueryParser.Parse( pointer );
        var segment = query.Segments.Next; // skip the root segment

        return TryGetFromJsonPathPointer( jsonElement, segment, out var value ) ? value : default;
    }

    internal static bool TryGetFromJsonPathPointer( this JsonElement jsonElement, JsonPathSegment segment, out JsonElement value )
    {
        if ( !segment.IsNormalized )
            throw new NotSupportedException( "Unsupported JsonPath pointer query format." );

        var current = jsonElement;
        value = default;

        while ( !segment.IsFinal )
        {
            var (selectorValue, selectorKind) = segment.Selectors[0];

            switch ( selectorKind )
            {
                case SelectorKind.Name:
                    {
                        if ( current.ValueKind != JsonValueKind.Object )
                            return false;

                        if ( !current.TryGetProperty( selectorValue, out var child ) )
                            return false;

                        current = child;
                        break;
                    }

                case SelectorKind.Index:
                    {
                        if ( current.ValueKind != JsonValueKind.Array )
                            return false;

                        var length = current.GetArrayLength();
                        var index = int.Parse( selectorValue );

                        if ( index < 0 )
                            index = length + index;

                        if ( index < 0 || index >= length )
                            return false;

                        current = current[index];
                        break;
                    }

                default:
                    throw new NotSupportedException( $"Unsupported {nameof( SelectorKind )}." );
            }

            segment = segment.Next;
        }

        value = current;
        return true;
    }

    public static JsonNode FromJsonPathPointer( this JsonNode jsonNode, ReadOnlySpan<char> pointer )
    {
        var query = JsonPathQueryParser.Parse( pointer );
        var segment = query.Segments.Next; // skip the root segment

        return TryGetFromJsonPathPointer( jsonNode, segment, out var value ) ? value : default;
    }

    public static bool TryGetFromJsonPathPointer( this JsonNode jsonNode, JsonPathSegment segment, out JsonNode value )
    {
        if ( !segment.IsNormalized )
            throw new NotSupportedException( "Unsupported JsonPath pointer query format." );

        var current = jsonNode;
        value = default;

        while ( !segment.IsFinal )
        {
            var (selectorValue, selectorKind) = segment.Selectors[0];

            switch ( selectorKind )
            {
                case SelectorKind.Name:
                    {
                        if ( current is not JsonObject jsonObject )
                            return false;

                        if ( !jsonObject.TryGetPropertyValue( selectorValue, out var child ) )
                            return false;

                        current = child;
                        break;
                    }

                case SelectorKind.Index:
                    {
                        if ( current is not JsonArray jsonArray )
                            return false;

                        var length = jsonArray.Count;
                        var index = int.Parse( selectorValue );

                        if ( index < 0 )
                            index = length + index;

                        if ( index < 0 || index >= length )
                            return false;

                        current = jsonArray[index];
                        break;
                    }

                default:
                    throw new NotSupportedException( $"Unsupported {nameof( SelectorKind )}." );
            }

            segment = segment.Next;
        }

        value = current;
        return true;
    }
}
