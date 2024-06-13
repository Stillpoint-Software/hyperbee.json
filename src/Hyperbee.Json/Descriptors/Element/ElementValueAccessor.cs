using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Hyperbee.Json.Descriptors.Element;

internal class ElementValueAccessor : IValueAccessor<JsonElement>
{
    public IEnumerable<(JsonElement, string)> EnumerateChildren( JsonElement value, bool includeValues = true )
    {
        switch ( value.ValueKind )
        {
            case JsonValueKind.Array:
                {
                    for ( var index = value.GetArrayLength() - 1; index >= 0; index-- )
                    {
                        var child = value[index];

                        if ( includeValues || child.ValueKind is JsonValueKind.Array or JsonValueKind.Object )
                            yield return (child, index.ToString());
                    }

                    break;
                }
            case JsonValueKind.Object:
                {
                    if ( includeValues )
                    {
                        foreach ( var child in value.EnumerateObject().Reverse() )
                            yield return (child.Value, child.Name);
                    }
                    else
                    {
                        foreach ( var child in value.EnumerateObject().Where( property => property.Value.ValueKind is JsonValueKind.Array or JsonValueKind.Object ).Reverse() )
                            yield return (child.Value, child.Name);
                    }

                    break;
                }
        }
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public JsonElement GetElementAt( in JsonElement value, int index )
    {
        return value[index];
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool IsObjectOrArray( in JsonElement value )
    {
        return value.ValueKind is JsonValueKind.Array or JsonValueKind.Object;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool IsArray( in JsonElement value, out int length )
    {
        if ( value.ValueKind == JsonValueKind.Array )
        {
            length = value.GetArrayLength();
            return true;
        }

        length = 0;
        return false;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool IsObject( in JsonElement value )
    {
        return value.ValueKind is JsonValueKind.Object;
    }

    public bool TryGetChildValue( in JsonElement value, ReadOnlySpan<char> childKey, out JsonElement childValue )
    {
        switch ( value.ValueKind )
        {
            case JsonValueKind.Object:
                if ( value.TryGetProperty( childKey, out childValue ) )
                    return true;
                break;

            case JsonValueKind.Array:
                var index = TryParseInt( childKey ) ?? -1;

                if ( index >= 0 && index < value.GetArrayLength() )
                {
                    childValue = value[index];
                    return true;
                }

                break;

            default:
                if ( !IsPathOperator( childKey ) )
                    throw new ArgumentException( $"Invalid child type '{childKey.ToString()}'. Expected child to be Object, Array or a path selector.", nameof( value ) );
                break;
        }

        childValue = default;
        return false;

        static bool IsPathOperator( ReadOnlySpan<char> x ) => x == "*" || x == ".." || x == "$";

        static int? TryParseInt( ReadOnlySpan<char> numberString )
        {
            return numberString == null ? null : int.TryParse( numberString, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n ) ? n : null;
        }
    }
}
