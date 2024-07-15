using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Descriptors.Element;

internal class ElementValueAccessor : IValueAccessor<JsonElement>
{
    public IEnumerable<(JsonElement, string, SelectorKind)> EnumerateChildren( JsonElement value, bool includeValues = true )
    {
        switch ( value.ValueKind )
        {
            case JsonValueKind.Array:
                {
                    for ( var index = value.GetArrayLength() - 1; index >= 0; index-- )
                    {
                        var child = value[index];

                        if ( includeValues || child.ValueKind is JsonValueKind.Array or JsonValueKind.Object )
                            yield return (child, index.ToString(), SelectorKind.Index);
                    }

                    break;
                }
            case JsonValueKind.Object:
                {
                    if ( includeValues )
                    {
                        foreach ( var child in value.EnumerateObject().Reverse() )
                            yield return (child.Value, child.Name, SelectorKind.Name);
                    }
                    else
                    {
                        foreach ( var child in value.EnumerateObject().Where( property => property.Value.ValueKind is JsonValueKind.Array or JsonValueKind.Object ).Reverse() )
                            yield return (child.Value, child.Name, SelectorKind.Name);
                    }

                    break;
                }
        }
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool TryGetElementAt( in JsonElement value, int index, out JsonElement element )
    {
        element = default;

        if ( index < 0 ) // flip negative index to positive
            index = value.GetArrayLength() + index;

        if ( index < 0 || index >= value.GetArrayLength() ) // out of bounds
            return false;

        element = value[index];
        return true;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public NodeKind GetNodeKind( in JsonElement value )
    {
        return value.ValueKind switch
        {
            JsonValueKind.Object => NodeKind.Object,
            JsonValueKind.Array => NodeKind.Array,
            _ => NodeKind.Value
        };
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public int GetArrayLength( in JsonElement value )
    {
        return value.ValueKind == JsonValueKind.Array
            ? value.GetArrayLength()
            : 0;
    }

    public bool TryGetChildValue( in JsonElement value, string childSelector, SelectorKind selectorKind, out JsonElement childValue )
    {
        switch ( value.ValueKind )
        {
            case JsonValueKind.Object:
                if ( value.TryGetProperty( childSelector, out childValue ) )
                    return true;
                break;

            case JsonValueKind.Array:
                if ( selectorKind == SelectorKind.Name )
                    break;

                if ( int.TryParse( childSelector, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index ) )
                {
                    var arrayLength = value.GetArrayLength();

                    if ( index < 0 ) // flip negative index to positive
                        index = arrayLength + index;

                    if ( index >= 0 && index < arrayLength )
                    {
                        childValue = value[index];
                        return true;
                    }
                }

                break;

            default:
                if ( !IsPathOperator( childSelector ) )
                    throw new ArgumentException( $"Invalid child type '{childSelector}'. Expected child to be Object, Array or a path selector.", nameof( value ) );
                break;
        }

        childValue = default;
        return false;

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static bool IsPathOperator( ReadOnlySpan<char> x )
        {
            return x.Length switch
            {
                1 => x[0] == '*',
                2 => x[0] == '.' && x[1] == '.',
                3 => x[0] == '$',
                _ => false
            };
        }
    }

    public bool DeepEquals( JsonElement left, JsonElement right )
    {
        return left.DeepEquals( right );
    }

    public bool TryParseNode( ReadOnlySpan<char> item, out JsonElement element )
    {
        var bytes = Encoding.UTF8.GetBytes( item.ToArray() );
        var reader = new Utf8JsonReader( bytes );

        try
        {
            if ( JsonDocument.TryParseValue( ref reader, out var document ) )
            {
                element = document.RootElement;
                return true;
            }
        }
        catch
        {
            // ignored: fall through
        }

        element = default;
        return false;
    }

    public bool TryGetValueFromNode( JsonElement element, out object value )
    {
        switch ( element.ValueKind )
        {
            case JsonValueKind.String:
                value = element.GetString();
                break;
            case JsonValueKind.Number:
                if ( element.TryGetInt32( out int intValue ) )
                {
                    value = intValue;
                    break;
                }

                if ( element.TryGetSingle( out float floatValue ) )
                {
                    value = floatValue;
                    break;
                }

                value = false;
                return false;

            case JsonValueKind.True:
                value = true;
                break;
            case JsonValueKind.False:
                value = false;
                break;
            case JsonValueKind.Null:
                value = null;
                break;
            default:
                value = false;
                return false;
        }

        return true;
    }


    public bool TryGetFromPointer( in JsonElement element, JsonPathSegment segment, out JsonElement childValue )
    {
        return element.TryGetFromJsonPathPointer( segment, out childValue );
    }
}
