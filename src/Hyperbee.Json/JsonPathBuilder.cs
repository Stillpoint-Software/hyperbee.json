using System.Runtime.CompilerServices;
using System.Text;
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
                        var itemId = GetUniqueId( property.Value );

                        if ( _parentMap.ContainsKey( itemId ) )
                            continue;

                        _parentMap[itemId] = (elementId, $".{property.Name}");
                        stack.Push( property.Value );
                    }
                    break;

                case JsonValueKind.Array:
                    var arrayIdx = 0;
                    foreach ( var item in currentElement.EnumerateArray() )
                    {
                        var itemId = GetUniqueId( item );

                        if ( _parentMap.ContainsKey( itemId ) )
                            continue;

                        _parentMap[itemId] = (elementId, $"[{arrayIdx++}]");
                        stack.Push( item );
                    }
                    break;
            }
        }

        return null; // target not found
    }

    // This method is called by `SelectPath` to pre-seed the parent map.
    // This is an optimization that allows us to leverage the select path
    // walk so that we won't have to walk again when `BuildPath` is called.
    internal void InsertItem( in JsonElement parentElement, in JsonElement itemElement, string itemKey )
    {
        var itemId = GetUniqueId( itemElement );

        if ( _parentMap.ContainsKey( itemId ) )
            return;

        var parentId = parentElement.ValueKind == JsonValueKind.Undefined
            ? GetUniqueId( _rootElement )
            : GetUniqueId( parentElement );

        itemKey = parentElement.ValueKind == JsonValueKind.Array
            ? $"[{itemKey}]"
            : $".{itemKey}";

        _parentMap[itemId] = (parentId, itemKey);
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static int GetUniqueId( in JsonElement element )
    {
        return JsonElementInternal.GetIdx( element );
    }

    private static string BuildPath( in int elementId, Dictionary<int, (int parentId, string segment)> parentMap )
    {
        var pathBuilder = new StringBuilder( 64 );

        RecursiveBuildPath( elementId );

        return pathBuilder.ToString();

        void RecursiveBuildPath( int currentId )
        {
            if ( currentId == -1 )
                return;

            var (parentId, segment) = parentMap[currentId];
            RecursiveBuildPath( parentId );
            pathBuilder.Append( segment );
        }
    }

    // We want a fast comparer that will tell us if two JsonElements point to the same exact
    // backing data in the parent JsonDocument. JsonElement is a struct, and a value comparison
    // for equality won't give us reliable results and would be operationally expensive.

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
