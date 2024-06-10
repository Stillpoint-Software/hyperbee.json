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

        _parentMap[GetUniqueId( _rootElement )] = (-1, "$"); // seed parent map with root
    }

    public string GetPath( JsonElement targetElement )
    {
        // quick out

        var targetId = GetUniqueId( targetElement );

        if ( _parentMap.ContainsKey( targetId ) )
            return BuildPath( targetId, _parentMap );

        // take a walk

        var stack = new Stack<JsonElement>( [_rootElement] );

        while ( stack.Count > 0 )
        {
            var currentElement = stack.Pop();
            var elementId = GetUniqueId( currentElement );

            if ( _comparer.Equals( currentElement, targetElement ) )
                return BuildPath( elementId, _parentMap );

            switch ( currentElement.ValueKind )
            {
                case JsonValueKind.Object:
                    foreach ( var property in currentElement.EnumerateObject() )
                    {
                        var childElementId = GetUniqueId( property.Value );

                        if ( !_parentMap.ContainsKey( childElementId ) )
                            _parentMap[childElementId] = (elementId, $".{property.Name}");

                        stack.Push( property.Value );
                    }
                    break;

                case JsonValueKind.Array:
                    var arrayIdx = 0;
                    foreach ( var element in currentElement.EnumerateArray() )
                    {
                        var childElementId = GetUniqueId( element );

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

    private static int GetUniqueId( JsonElement element )
    {
        return JsonElementInternal.GetIdx( element );
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
}
