using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Hyperbee.Json.Filters;

namespace Hyperbee.Json.Descriptors.Element;

public class ElementValueAccessor : IValueAccessor<JsonElement>
{
    public IEnumerable<(JsonElement, string)> EnumerateChildValues( JsonElement value )
    {
        switch ( value.ValueKind )
        {
            case JsonValueKind.Array:
                {
                    for ( var index = value.GetArrayLength() - 1; index >= 0; index-- )
                    {
                        yield return (value[index], index.ToString());
                    }

                    break;
                }
            case JsonValueKind.Object:
                {
                    foreach ( var result in ReverseProperties( value.EnumerateObject() ) )
                        yield return result;

                    break;
                }
        }

        yield break;

        static IEnumerable<(JsonElement, string)> ReverseProperties( JsonElement.ObjectEnumerator enumerator )
        {
            if ( !enumerator.MoveNext() )
            {
                yield break;
            }

            var property = enumerator.Current;

            foreach ( var result in ReverseProperties( enumerator ) )
            {
                yield return result;
            }

            yield return (property.Value, property.Name);
        }
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public JsonElement GetElementAt( JsonElement value, int index )
    {
        return value[index];
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool IsObjectOrArray( JsonElement value )
    {
        return value.ValueKind is JsonValueKind.Array or JsonValueKind.Object;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool IsArray( JsonElement value, out int length )
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
    public bool IsObject( JsonElement value )
    {
        return value.ValueKind is JsonValueKind.Object;
    }

    public bool TryGetChildValue( in JsonElement value, ReadOnlySpan<char> childKey, out JsonElement childValue )
    {
        static int? TryParseInt( ReadOnlySpan<char> numberString )
        {
            return numberString == null ? null : int.TryParse( numberString, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n ) ? n : null;
        }

        static bool IsPathOperator( ReadOnlySpan<char> x ) => x == "*" || x == ".." || x == "$";

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
    }
}
