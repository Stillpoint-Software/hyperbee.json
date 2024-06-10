using System.Text.Json;

namespace Hyperbee.Json;

internal class JsonElementPositionComparer : IEqualityComparer<JsonElement>
{
    public bool Equals( JsonElement x, JsonElement y )
    {
        // check for quick out

        if ( x.ValueKind != y.ValueKind )
            return false;

        // We want a fast comparer that will tell us if two JsonElements point to the same exact
        // backing data in the parent JsonDocument. JsonElement is a struct, and a value comparison
        // for equality won't give us reliable results and would be expensive.
        //
        // The internal JsonElement constructor takes parent and idx arguments that are saves as fields.
        // 
        // idx: is an index used to get the position of the JsonElement in the backing data.
        // parent: is the owning JsonDocument (could be null in an enumeration).
        //
        // These arguments are stored in private fields and are not exposed. While note ideal, we
        // will access these fields through dynamic methods to use for our comparison.
        
        // check parent documents

        // BF: JsonElement ctor notes that parent may be null in some enumeration conditions.
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
