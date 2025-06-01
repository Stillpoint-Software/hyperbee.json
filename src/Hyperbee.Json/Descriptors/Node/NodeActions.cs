using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Path;
using Hyperbee.Json.Pointer;
using Hyperbee.Json.Query;

namespace Hyperbee.Json.Descriptors.Node;

internal class NodeActions : INodeActions<JsonNode>
{
    public bool TryParse( ref Utf8JsonReader reader, out JsonNode node )
    {
        try
        {
            node = JsonNode.Parse( ref reader );
            return true;
        }
        catch
        {
            node = null;
            return false;
        }
    }

    public bool TryGetFromPointer( in JsonNode node, JsonSegment segment, out JsonNode value ) =>
        SegmentPointer<JsonNode>.TryGetFromPointer( node, segment, out _, out value );

    public bool DeepEquals( JsonNode left, JsonNode right ) =>
        JsonNode.DeepEquals( left, right );

    public IEnumerable<JsonNode> GetChildren( JsonNode value, ChildEnumerationOptions options )
    {
        bool complexTypesOnly = options.HasFlag( ChildEnumerationOptions.ComplexTypesOnly );
        bool reverse = options.HasFlag( ChildEnumerationOptions.Reverse );

        // allocating is faster than using yield return and less memory intensive.
        // using a collection results in fewer overall allocations than calling
        // LINQ reverse, which internally allocates, and then discards, a new array.

        List<JsonNode> results;

        switch ( value )
        {
            case JsonArray jsonArray:
                {
                    var length = jsonArray.Count;
                    results = new List<JsonNode>( length );

                    for ( var index = 0; index < length; index++ )
                    {
                        var child = value[index];

                        if ( complexTypesOnly && child is not (JsonArray or JsonObject) )
                            continue;

                        results.Add( child );
                    }

                    return reverse ? results.EnumerateReverse() : results;
                }
            case JsonObject jsonObject:
                {
                    results = new List<JsonNode>( 8 );

                    foreach ( var child in jsonObject )
                    {
                        if ( complexTypesOnly && child.Value is not (JsonArray or JsonObject) )
                            continue;

                        results.Add( child.Value );
                    }

                    return reverse ? results.EnumerateReverse() : results;
                }
        }

        return [];
    }

    public IEnumerable<(JsonNode Value, string Key)> GetChildrenWithName( in JsonNode value, ChildEnumerationOptions options )
    {
        bool complexTypesOnly = options.HasFlag( ChildEnumerationOptions.ComplexTypesOnly );
        bool reverse = options.HasFlag( ChildEnumerationOptions.Reverse );

        // allocating is faster than using yield return and less memory intensive.
        // using a collection results in fewer overall allocations than calling
        // LINQ reverse, which internally allocates, and then discards, a new array.

        List<(JsonNode, string)> results;

        switch ( value )
        {
            case JsonArray jsonArray:
                {
                    var length = jsonArray.Count;
                    results = new List<(JsonNode, string)>( length );

                    for ( var index = 0; index < length; index++ )
                    {
                        var child = value[index];

                        if ( complexTypesOnly && child is not (JsonArray or JsonObject) )
                            continue;

                        results.Add( (child, IndexHelper.GetIndexString( index )) );
                    }

                    return reverse ? results.EnumerateReverse() : results;
                }
            case JsonObject jsonObject:
                {
                    results = new List<(JsonNode, string)>( 8 );

                    foreach ( var child in jsonObject )
                    {
                        if ( complexTypesOnly && child.Value is not (JsonArray or JsonObject) )
                            continue;

                        results.Add( (child.Value, child.Key) );
                    }

                    return reverse ? results.EnumerateReverse() : results;
                }
        }

        return [];
    }
}
