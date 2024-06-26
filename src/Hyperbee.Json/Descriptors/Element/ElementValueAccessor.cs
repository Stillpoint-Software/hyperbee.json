using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Hyperbee.Json.Descriptors.Element.Functions;

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
    public JsonElement GetElementAt( in JsonElement value, int index )
    {
        return value[index];
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

    public bool TryGetChildValue( in JsonElement value, string childSelector, out JsonElement childValue )
    {
        switch ( value.ValueKind )
        {
            case JsonValueKind.Object:
                if ( value.TryGetProperty( childSelector, out childValue ) )
                    return true;
                break;

            case JsonValueKind.Array:
                if ( int.TryParse( childSelector, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index ) )
                {
                    if ( index >= 0 && index < value.GetArrayLength() )
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

    public object GetAsValue( IEnumerable<JsonElement> elements )
    {
        return ValueElementFunction.Value( elements );
    }
}
