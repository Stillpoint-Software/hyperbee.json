using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Evaluators.Parser.Node;

internal class JsonNodeValueAccessor : IJsonValueAccessor<JsonNode>
{
    public IEnumerable<(JsonNode, string)> EnumerateChildValues( JsonNode value )
    {
        switch ( value )
        {
            case JsonArray arrayValue:
                for ( var index = arrayValue.Count - 1; index >= 0; index-- )
                {
                    yield return (value[index], index.ToString());
                }

                break;
            case JsonObject objectValue:
                foreach ( var result in ProcessProperties( objectValue.GetEnumerator() ) )
                    yield return result;

                break;
        }

        yield break;

        static IEnumerable<(JsonNode, string)> ProcessProperties( IEnumerator<KeyValuePair<string, JsonNode>> enumerator )
        {
            if ( !enumerator.MoveNext() )
            {
                yield break;
            }

            var property = enumerator.Current;

            foreach ( var result in ProcessProperties( enumerator ) )
            {
                yield return result;
            }

            yield return (property.Value, property.Key);
        }
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public JsonNode GetElementAt( JsonNode value, int index )
    {
        return value[index];
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool IsObjectOrArray( JsonNode value )
    {
        return value is JsonObject or JsonArray;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public bool IsArray( JsonNode value, out int length )
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
    public bool IsObject( JsonNode value )
    {
        return value is JsonObject;
    }

    public bool TryGetChildValue( in JsonNode value, ReadOnlySpan<char> childKey, out JsonNode childValue )
    {
        static int? TryParseInt( ReadOnlySpan<char> numberString )
        {
            return numberString == null ? null : int.TryParse( numberString, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n ) ? n : null;
        }

        static bool IsPathOperator( ReadOnlySpan<char> x ) => x == "*" || x == ".." || x == "$";

        switch ( value )
        {
            case JsonObject valueObject:
                {
                    if ( valueObject.TryGetPropertyValue( childKey.ToString(), out childValue ) )
                        return true;

                    break;
                }
            case JsonArray valueArray:
                {
                    var index = TryParseInt( childKey ) ?? -1;

                    if ( index >= 0 && index < valueArray.Count )
                    {
                        childValue = value[index];
                        return true;
                    }

                    break;
                }
            default:
                {
                    if ( !IsPathOperator( childKey ) )
                        throw new ArgumentException( $"Invalid child type '{childKey.ToString()}'. Expected child to be Object, Array or a path selector.", nameof( value ) );

                    break;
                }
        }

        childValue = default;
        return false;
    }
}
