using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;

namespace Hyperbee.Json;

internal class JsonElementPositionComparer : IEqualityComparer<JsonElement>
{
    static readonly Func<JsonElement, int> __getIdx;
    static readonly Func<JsonElement, JsonDocument> __getParent;

    static JsonElementPositionComparer()
    {
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

        // Create DynamicMethod for _idx field

        var idxField = typeof( JsonElement ).GetField( "_idx", BindingFlags.NonPublic | BindingFlags.Instance );

        if ( idxField == null )
            throw new MissingFieldException( nameof( JsonElement ), "_idx" );

        var getIdxDynamicMethod = new DynamicMethod( "GetIdx", typeof( int ), [typeof( JsonElement )], typeof( JsonElement ) );
        var ilIdx = getIdxDynamicMethod.GetILGenerator();
        ilIdx.Emit( OpCodes.Ldarg_0 );
        ilIdx.Emit( OpCodes.Ldfld, idxField );
        ilIdx.Emit( OpCodes.Ret );

        __getIdx = (Func<JsonElement, int>) getIdxDynamicMethod.CreateDelegate( typeof( Func<JsonElement, int> ) );

        // Create DynamicMethod for _parent field 

        var parentField = typeof( JsonElement ).GetField( "_parent", BindingFlags.NonPublic | BindingFlags.Instance );

        if ( parentField == null )
            throw new MissingFieldException( nameof( JsonElement ), "_parent" );

        var getParentDynamicMethod = new DynamicMethod( "GetParent", typeof( JsonDocument ), [typeof( JsonElement )], typeof( JsonElement ) );
        var ilParent = getParentDynamicMethod.GetILGenerator();
        ilParent.Emit( OpCodes.Ldarg_0 );
        ilParent.Emit( OpCodes.Ldfld, parentField );
        ilParent.Emit( OpCodes.Ret );

        __getParent = (Func<JsonElement, JsonDocument>) getParentDynamicMethod.CreateDelegate( typeof( Func<JsonElement, JsonDocument> ) );
    }

    public bool Equals( JsonElement x, JsonElement y )
    {
        // check for quick out

        if ( x.ValueKind != y.ValueKind )
            return false;

        // check parent documents

        // BF: JsonElement ctor notes that parent may be null in some enumeration conditions.
        // This check may not be reliable. If so, should be ok to remove the parent check.

        var xParent = __getParent( x );
        var yParent = __getParent( y );

        if ( !ReferenceEquals( xParent, yParent ) ) 
            return false;

        // check idx values

        return __getIdx( x ) == __getIdx( y );
    }

    public int GetHashCode( JsonElement obj )
    {
        var parent = __getParent( obj );
        var idx = __getIdx( obj );

        return HashCode.Combine( parent, idx );
    }
}
