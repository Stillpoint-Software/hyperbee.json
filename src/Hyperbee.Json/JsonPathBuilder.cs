#define USE_OPTIMIZED

using System.Text.Json;

namespace Hyperbee.Json;

public class JsonPathBuilder
{
    private readonly JsonElement _rootElement;
    private readonly JsonElementPositionComparer _comparer = new();

    public JsonPathBuilder( JsonDocument rootDocument )
    {
        _rootElement = rootDocument.RootElement;
    }

    public JsonPathBuilder( JsonElement rootElement )
    {
        _rootElement = rootElement;
    }

#if USE_OPTIMIZED

    // avoid allocating full paths for every node by building a dictionary
    // of (parentId, segment) pairs.
    //
    // if we switch parentId to _idx from a simple int counter then we should
    // be able to build a fast path that only walks until it finds an _idx
    // match in the dictionary. this would allow us to move the parentMap
    // to a member and would give us an effective cache mechanism.

    public string GetPath( JsonElement targetElement )
    {
        var stack = new Stack<(int elementId, JsonElement element)>();
        var parentMap = new Dictionary<int, (int parentId, string segment)>();
        var currentId = 0;

        stack.Push( (currentId, _rootElement) );
        parentMap[currentId] = (-1, "$");
        currentId++;

        while ( stack.Count > 0 )
        {
            var (elementId, currentElement) = stack.Pop();

            if ( _comparer.Equals( currentElement, targetElement ) )
                return BuildPath( elementId, parentMap );

            switch ( currentElement.ValueKind )
            {
                case JsonValueKind.Object:
                    foreach ( var property in currentElement.EnumerateObject() )
                    {
                        var childElementId = currentId++;
                        parentMap[childElementId] = (elementId, $".{property.Name}");
                        stack.Push( (childElementId, property.Value) );
                    }
                    break;

                case JsonValueKind.Array:
                    var arrayIdx = 0;
                    foreach ( var element in currentElement.EnumerateArray() )
                    {
                        var childElementId = currentId++;
                        parentMap[childElementId] = (elementId, $"[{arrayIdx}]");
                        stack.Push( (childElementId, element) );
                        arrayIdx++;
                    }
                    break;
            }
        }

        return null; // Target not found
    }

    private static string BuildPath( int elementId, Dictionary<int, (int parentId, string segment)> parentMap )
    {
        var pathSegments = new Stack<string>();
        var currentId = elementId;

        while ( currentId != -1 )
        {
            var (parentId, segment) = parentMap[currentId];
            pathSegments.Push( segment );
            currentId = parentId;
        }

        return string.Join( string.Empty, pathSegments );
    }

#else
    
    // simple implementation
    public string GetPath( JsonElement targetElement )
    {
        var stack = new Stack<(JsonElement element, string path)>( 4 );
        stack.Push( (_rootElement, "$") );

        while ( stack.Count > 0 )
        {
            var (currentElement, currentPath) = stack.Pop();

            if ( _comparer.Equals( currentElement, targetElement ) )
                return currentPath;

            switch ( currentElement.ValueKind )
            {
                case JsonValueKind.Object:
                    foreach ( var property in currentElement.EnumerateObject() )
                    {
                        var newPath = $"{currentPath}.{property.Name}";
                        stack.Push( (property.Value, newPath) );
                    }

                    break;

                case JsonValueKind.Array:
                    var index = 0;
                    foreach ( var element in currentElement.EnumerateArray() )
                    {
                        var newPath = $"{currentPath}[{index++}]";
                        stack.Push( (element, newPath) );
                    }

                    break;
            }
        }

        return null; // Target no
    }

#endif
}
