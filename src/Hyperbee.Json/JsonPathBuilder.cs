using System.Text.Json;

namespace Hyperbee.Json;

public class JsonPathBuilder
{
    private readonly JsonElement _rootElement;
    private readonly JsonElementPositionComparer _comparer = new();
    private readonly Dictionary<int, (int parentId, string segment)> _parentMap = [];

    public JsonPathBuilder( JsonDocument rootDocument )
        : this( rootDocument.RootElement )
    {
    }

    public JsonPathBuilder( JsonElement rootElement )
    {
        _rootElement = rootElement;

        // we will avoid allocating full paths for every node by
        // building a dictionary cache of (parentId, segment) pairs.

        _parentMap[GetIdx( _rootElement )] = (-1, "$"); // seed parent map with root
    }

    public string GetPath( JsonElement targetElement )
    {
        // quick out

        var targetId = GetIdx( targetElement );

        if ( _parentMap.ContainsKey( targetId ) )
            return BuildPath( targetId, _parentMap );

        // take a walk

        var stack = new Stack<JsonElement>( [_rootElement] );

        while ( stack.Count > 0 )
        {
            var currentElement = stack.Pop();
            var elementId = GetIdx( currentElement );

            if ( _comparer.Equals( currentElement, targetElement ) )
                return BuildPath( elementId, _parentMap );

            switch ( currentElement.ValueKind )
            {
                case JsonValueKind.Object:
                    foreach ( var property in currentElement.EnumerateObject() )
                    {
                        var childElementId = GetIdx( property.Value );

                        if ( !_parentMap.ContainsKey( childElementId ) )
                            _parentMap[childElementId] = (elementId, $".{property.Name}");

                        stack.Push( property.Value );
                    }
                    break;

                case JsonValueKind.Array:
                    var arrayIdx = 0;
                    foreach ( var element in currentElement.EnumerateArray() )
                    {
                        var childElementId = GetIdx( element );

                        if ( !_parentMap.ContainsKey( childElementId ) )
                            _parentMap[childElementId] = (elementId, $"[{arrayIdx}]");

                        stack.Push( element );
                        arrayIdx++;
                    }
                    break;
            }
        }

        return null; // target not found
    }

    private static int GetIdx( JsonElement element )
    {
        return JsonElementPositionComparer.GetIdx( element ); // Not ideal, but neither is creating multiple dynamic methods. Discuss how to handle.
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

        return null;
    }
*/
}
