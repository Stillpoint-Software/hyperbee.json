using System.Text.Json;

namespace Hyperbee.Json;

public class JsonPathResolver
{
    private readonly JsonElement _rootElement;
    private readonly JsonElementPositionComparer _comparer = new();
    private readonly Dictionary<int, (int parentId, string segment)> _parentMap = [];

    public JsonPathResolver( JsonDocument rootDocument )
        : this( rootDocument.RootElement )
    {
    }

    public JsonPathResolver( JsonElement rootElement )
    {
        _rootElement = rootElement;

        // avoid allocating full paths for every node by building
        // a dictionary of (parentId, segment) pairs.

        _parentMap[GetUniqueId( _rootElement )] = (-1, "$"); // seed parent map with root
    }

    public string GetPath( in JsonElement targetElement )
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

    private static int GetUniqueId( in JsonElement element )
    {
        return JsonElementInternal.GetIdx( element );
    }

    private static string BuildPath( in int elementId, Dictionary<int, (int parentId, string segment)> parentMap )
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

    // We want a fast comparer that will tell us if two JsonElements point to the same exact
    // backing data in the parent JsonDocument. JsonElement is a struct, and a value comparison
    // for equality won't give us reliable results and would be expensive.
    //
    private class JsonElementPositionComparer : IEqualityComparer<JsonElement>
    {
        public bool Equals( JsonElement x, JsonElement y )
        {
            // check for quick out

            if ( x.ValueKind != y.ValueKind )
                return false;

            // The internal JsonElement constructor takes parent and idx arguments that are saved as fields.
            // 
            // idx: is an index used to get the position of the JsonElement in the backing data.
            // parent: is the owning JsonDocument (could be null in an enumeration).
            //
            // These arguments are stored in private fields and are not exposed. While not ideal, we will
            // directly access these fields through dynamic methods to use for our comparison. If microsoft
            // provides Parent and Location in the future we will remove this.

            // check parent documents

            // The JsonElement ctor notes that parent may be null in some enumeration conditions.
            // This check may not be reliable. If so, should be ok to remove the parent check.

            var xParent = JsonElementInternal.GetParent( x );
            var yParent = JsonElementInternal.GetParent( y );

            if ( !ReferenceEquals( xParent, yParent ) )
                return false;

            // check idx values

            return JsonElementInternal.GetIdx( x ) == JsonElementInternal.GetIdx( y );
        }

        public int GetHashCode( JsonElement obj )
        {
            var parent = JsonElementInternal.GetParent( obj );
            var idx = JsonElementInternal.GetIdx( obj );

            return HashCode.Combine( parent, idx );
        }
    }
}
