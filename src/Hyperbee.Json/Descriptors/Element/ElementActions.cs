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

    public IEnumerable<JsonElement> GetChildren( JsonElement value, ChildEnumerationOptions options )
    {
        bool complexTypesOnly = options.HasFlag( ChildEnumerationOptions.ComplexTypesOnly );
        bool reverse = options.HasFlag( ChildEnumerationOptions.Reverse );

        // allocating is faster than using yield return and less memory intensive.
        // using a collection results in fewer overall allocations than calling
        // LINQ reverse, which internally allocates, and then discards, a new array.

        List<JsonElement> results;

        switch ( value.ValueKind )
        {
            case JsonValueKind.Array:
                {
                    var length = value.GetArrayLength();
                    results = new List<JsonElement>( length );

                    for ( var index = 0; index < length; index++ )
                    {
                        var child = value[index];

                        if ( complexTypesOnly && child.ValueKind is not (JsonValueKind.Array or JsonValueKind.Object) )
                            continue;

                        results.Add( child );
                    }

                    return reverse ? results.EnumerateReverse() : results;
                }
            case JsonValueKind.Object:
                {
                    results = new List<JsonElement>( 8 );

                    foreach ( var child in value.EnumerateObject() )
                    {
                        if ( complexTypesOnly && child.Value.ValueKind is not (JsonValueKind.Array or JsonValueKind.Object) )
                            continue;

                        results.Add( child.Value );
                    }

                    return reverse ? results.EnumerateReverse() : results;
                }
        }

        return [];
    }

    public IEnumerable<(JsonElement Value, string Key)> GetChildrenWithName( in JsonElement value, ChildEnumerationOptions options )
    {
        bool complexTypesOnly = options.HasFlag( ChildEnumerationOptions.ComplexTypesOnly );
        bool reverse = options.HasFlag( ChildEnumerationOptions.Reverse );

        // allocating is faster than using yield return and less memory intensive.
        // using a collection results in fewer overall allocations than calling
        // LINQ reverse, which internally allocates, and then discards, a new array.

        List<(JsonElement, string)> results;

        switch ( value.ValueKind )
        {
            case JsonValueKind.Array:
                {
                    var length = value.GetArrayLength();
                    results = new List<(JsonElement, string)>( length );

                    for ( var index = 0; index < length; index++ )
                    {
                        var child = value[index];

                        if ( complexTypesOnly && child.ValueKind is not (JsonValueKind.Array or JsonValueKind.Object) )
                            continue;

                        results.Add( (child, IndexHelper.GetIndexString( index )) );
                    }

                    return reverse ? results.EnumerateReverse() : results;
                }
            case JsonValueKind.Object:
                {
                    results = new List<(JsonElement, string)>( 8 );

                    foreach ( var child in value.EnumerateObject() )
                    {
                        if ( complexTypesOnly && child.Value.ValueKind is not (JsonValueKind.Array or JsonValueKind.Object) )
                            continue;

                        results.Add( (child.Value, child.Name) );
                    }

                    return reverse ? results.EnumerateReverse() : results;
                }
        }

        return [];
    }
}

