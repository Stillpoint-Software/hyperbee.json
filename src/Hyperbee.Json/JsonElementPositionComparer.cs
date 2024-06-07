using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;

namespace Hyperbee.Json;

internal class JsonElementReferenceComparer : IEqualityComparer<JsonElement>
{
    static readonly Func<JsonElement, int> __getIdx;
    static readonly Func<JsonElement, JsonDocument> __getParent;

    static JsonElementReferenceComparer()
    {
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
        if ( x.ValueKind != y.ValueKind )
            return false;

        var xParent = __getParent( x );
        var yParent = __getParent( y );

        if ( !ReferenceEquals( xParent, yParent ) )
            return false;

        return __getIdx( x ) == __getIdx( y );
    }

    public int GetHashCode( JsonElement obj )
    {
        var parent = __getParent( obj );
        var idx = __getIdx( obj );

        return HashCode.Combine( parent, idx );
    }
}

public class JsonPathFinder
{
    public static string FindJsonPath( JsonElement rootElement, JsonElement targetElement )
    {
        var comparer = new JsonElementReferenceComparer();

        var stack = new Stack<(JsonElement element, string path)>();
        stack.Push( (rootElement, string.Empty) );

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
