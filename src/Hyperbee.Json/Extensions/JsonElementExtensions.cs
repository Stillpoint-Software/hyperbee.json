using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Core;

namespace Hyperbee.Json.Extensions;

public static class JsonElementExtensions
{
    // Deep Equals/Compare extensions

    public static bool DeepEquals( this JsonElement element1, JsonElement element2, JsonDocumentOptions options = default )
    {
        var comparer = new JsonElementDeepEqualityComparer( options.MaxDepth );
        return comparer.Equals( element1, element2 );
    }

    public static JsonNode ConvertToNode( this JsonElement element )
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => ConvertToObject( element ),
            JsonValueKind.Array => ConvertToArray( element ),
            JsonValueKind.String => JsonValue.Create( element.GetString() ),
            JsonValueKind.Number => JsonValue.Create( element.GetSingle() ),  // TODO: get best number type
            JsonValueKind.True => JsonValue.Create( true ),
            JsonValueKind.False => JsonValue.Create( false ),
            JsonValueKind.Null => null,
            JsonValueKind.Undefined => null,
            _ => throw new NotSupportedException( $"Unsupported JsonValueKind: {element.ValueKind}" )
        };

        static JsonObject ConvertToObject( JsonElement element )
        {
            var jsonObject = new JsonObject();
            foreach ( JsonProperty property in element.EnumerateObject() )
            {
                jsonObject[property.Name] = ConvertToNode( property.Value );
            }
            return jsonObject;
        }

        static JsonArray ConvertToArray( JsonElement element )
        {
            var jsonArray = new JsonArray();
            foreach ( JsonElement item in element.EnumerateArray() )
            {
                jsonArray.Add( ConvertToNode( item ) );
            }
            return jsonArray;
        }
    }

}
