using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;

namespace Hyperbee.Json;

internal static class JsonElementInternal
{
    internal static readonly Func<JsonElement, int> GetIdx;
    internal static readonly Func<JsonElement, JsonDocument> GetParent;

    static JsonElementInternal()
    {
        // Create DynamicMethod for _idx field

        const string idxName = "_idx";

        var idxField = typeof( JsonElement ).GetField( idxName, BindingFlags.NonPublic | BindingFlags.Instance );

        if ( idxField == null )
            throw new MissingFieldException( nameof( JsonElement ), idxName );

        var getIdxDynamicMethod = new DynamicMethod( nameof( GetIdx ), typeof( int ), [typeof( JsonElement )], typeof( JsonElement ) );
        var ilIdx = getIdxDynamicMethod.GetILGenerator();
        ilIdx.Emit( OpCodes.Ldarg_0 );
        ilIdx.Emit( OpCodes.Ldfld, idxField );
        ilIdx.Emit( OpCodes.Ret );

        GetIdx = (Func<JsonElement, int>) getIdxDynamicMethod.CreateDelegate( typeof( Func<JsonElement, int> ) );

        // Create DynamicMethod for _parent field 

        const string parentName = "_parent";

        var parentField = typeof( JsonElement ).GetField( parentName, BindingFlags.NonPublic | BindingFlags.Instance );

        if ( parentField == null )
            throw new MissingFieldException( nameof( JsonElement ), parentName );

        var getParentDynamicMethod = new DynamicMethod( nameof( GetParent ), typeof( JsonDocument ), [typeof( JsonElement )], typeof( JsonElement ) );
        var ilParent = getParentDynamicMethod.GetILGenerator();
        ilParent.Emit( OpCodes.Ldarg_0 );
        ilParent.Emit( OpCodes.Ldfld, parentField );
        ilParent.Emit( OpCodes.Ret );

        GetParent = (Func<JsonElement, JsonDocument>) getParentDynamicMethod.CreateDelegate( typeof( Func<JsonElement, JsonDocument> ) );
    }
}
