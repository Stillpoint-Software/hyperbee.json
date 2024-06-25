using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Descriptors.Node.Functions;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Descriptors.Node;

internal class NodeValueAccessor : IValueAccessor<JsonNode>
{
    public IEnumerable<(JsonNode, string, SelectorKind)> EnumerateChildren( JsonNode value, bool includeValues = true )
    {
        switch ( value )
        {
            case JsonArray arrayValue:
                for ( var index = arrayValue.Count - 1; index >= 0; index-- )

                {
                    var child = arrayValue[index];

                    if ( includeValues || child is JsonObject or JsonArray )
                        yield return (child, index.ToString(), SelectorKind.Index);
                }

                break;
            case JsonObject objectValue:

                if ( includeValues )
                {
                    foreach ( var child in objectValue.Reverse() )
                        yield return (child.Value, child.Key, SelectorKind.Name);
                }
                else
                {
                    foreach ( var child in objectValue.Where( property => property.Value is JsonObject or JsonArray ).Reverse() )
                        yield return (child.Value, child.Key, SelectorKind.Name);
                }

                break;
        }
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public JsonNode GetElementAt( in JsonNode value, int index )
    {
        return value[index];
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool IsObjectOrArray( in JsonNode value )
    {
        return value is JsonObject or JsonArray;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool IsArray( in JsonNode value, out int length )
    {
        if ( value is JsonArray jsonArray )
        {
            length = jsonArray.Count;
            return true;
        }

        length = 0;
        return false;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool IsObject( in JsonNode value )
    {
        return value is JsonObject;
    }

    public bool TryGetChildValue( in JsonNode value, string childKey, out JsonNode childValue )
    {
        switch ( value )
        {
            case JsonObject valueObject:
                {
                    if ( valueObject.TryGetPropertyValue( childKey, out childValue ) )
                        return true;

                    break;
                }
            case JsonArray valueArray:
                {
                    if ( int.TryParse( childKey, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index ) )
                    {
                        if ( index >= 0 && index < valueArray.Count )
                        {
                            childValue = value[index];
                            return true;
                        }
                    }

                    break;
                }
            default:
                {
                    if ( !IsPathOperator( childKey ) )
                        throw new ArgumentException( $"Invalid child type '{childKey}'. Expected child to be Object, Array or a path selector.", nameof( value ) );

                    break;
                }
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

    public object GetAsValue( IEnumerable<JsonNode> nodes )
    {
        return ValueNodeFunction.Value( nodes );
    }
}
