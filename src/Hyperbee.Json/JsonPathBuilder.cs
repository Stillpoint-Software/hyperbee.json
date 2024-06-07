using System.Text.Json;

namespace Hyperbee.Json;

public class JsonPathBuilder
{
    private readonly JsonElement _rootElement;

    public JsonPathBuilder( JsonDocument rootDocument )
    {
        _rootElement = rootDocument.RootElement;
    }

    public JsonPathBuilder( JsonElement rootElement )
    {
        _rootElement = rootElement;
    }

    public string GetPath( JsonElement targetElement )
    {
        var comparer = new JsonElementPositionComparer();

        var stack = new Stack<(JsonElement element, string path)>( 4 );
        stack.Push( (_rootElement, string.Empty) );

        while ( stack.Count > 0 )
        {
            var (currentElement, currentPath) = stack.Pop();

            if ( comparer.Equals( currentElement, targetElement ) )
                return currentPath;

            switch ( currentElement.ValueKind )
            {
                case JsonValueKind.Object:
                    foreach ( JsonProperty property in currentElement.EnumerateObject() )
                    {
                        var newPath = string.IsNullOrEmpty( currentPath ) ? property.Name : $"{currentPath}.{property.Name}";
                        stack.Push( (property.Value, newPath) );
                    }

                    break;

                case JsonValueKind.Array:
                    var index = 0;
                    foreach ( JsonElement element in currentElement.EnumerateArray() )
                    {
                        var newPath = $"{currentPath}[{index}]";
                        stack.Push( (element, newPath) );
                        index++;
                    }

                    break;
            }
        }

        return null; // Target element not found in the JSON document
    }
}
