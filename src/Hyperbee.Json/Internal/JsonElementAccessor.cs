using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;

namespace Hyperbee.Json.Internal;

internal static class JsonElementAccessor
{
    // We need to identify an element's unique metadata location to establish instance identity.
    // Deeply nested elements can have the same value but different locations in the document.
    // Deep compare is not sufficient to establish instance identity in such cases from either a
    // correctness or performance perspective. We can use the private _idx field of JsonElement
    // to identify the element's unique location in the document.
    // 
    // The justifications for this usage are:
    //
    // Performance Necessity: We need to access internal details to significantly improve
    // performance and there is no viable public API that provides the required functionality.
    //
    // Lack of Alternatives: The desired functionality is not exposed by the public API and no
    // alternative methods are available to achieve the same result.
    //
    // Low Risk: The usage is low risk because the internal field is only used to identify the
    // uniqueness of the element in the document. The field is not modified and the document is
    // not mutated. The field is read-only and the document is immutable. Furthermore, the field
    // is only accessed through a delegate that is created once and reused for all instances of
    // JsonElement. The delegate is created using a DynamicMethod and is not exposed to the
    // public API. The delegate is used to access the field in a safe and controlled manner.
    //
    // The internal field is critical to Microsoft's internal implementation and is unlikely to
    // change.

    internal static readonly Func<JsonElement, int> GetIdx;
    internal static readonly Func<JsonElement, JsonDocument> GetParent;

    static JsonElementAccessor()
    {
        // Create DynamicMethod to read the _idx field
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

        // Create DynamicMethod to read the _parent field 
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
