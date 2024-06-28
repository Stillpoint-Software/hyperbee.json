using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Hyperbee.Json.Descriptors.Element.Functions;
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

    public object GetAsValueOther( IEnumerable<JsonElement> elements )
    {
        var element = elements.FirstOrDefault();

        return element.ValueKind switch
        {
            JsonValueKind.Number => element.GetSingle(),
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Object => element,
            JsonValueKind.Array => element,
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            JsonValueKind.Undefined => false,
            _ => false
        };
    }

    public bool TryGetObjects( ReadOnlySpan<char> item, out IEnumerable<JsonElement> elements )
    {
        var bytes = Encoding.UTF8.GetBytes( item.ToArray() );
        var reader = new Utf8JsonReader( bytes );

        try
        {
            if ( JsonDocument.TryParseValue( ref reader, out var document ) )
            {
                elements = [document.RootElement];
                return true;
            }
        }
        catch
        {
            // ignored
        }

        elements = default;
        return false;
    }

    public bool DeepEquals( JsonElement left, JsonElement right )
    {
        return left.DeepEquals( right );
    }
}
