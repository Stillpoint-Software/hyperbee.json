using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json;

internal class JsonDocumentPathVisitor : JsonPathVisitorBase<JsonElement, JsonPathElement>
{
    protected readonly char[] SpecialCharacters = ['.', ' ', '\'', '/', '"', '[', ']', '(', ')', '\t', '\n', '\r', '\f', '\b', '\\', '\u0085', '\u2028', '\u2029'];

    internal override IEnumerable<string> EnumerateKeys( JsonElement value )
    {
        return value.ValueKind switch
        {
            JsonValueKind.Array => EnumerateArrayIndices( value.GetArrayLength() ).Select( x => x.ToString() ),
            JsonValueKind.Object => EnumeratePropertyNames( value ),
            _ => throw new NotSupportedException()
        };
    }


    internal override IEnumerable<(JsonElement, string)> EnumerateChildValues( JsonElement value )
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
                    foreach ( var result in ProcessProperties( value.EnumerateObject() ) )
                        yield return result;

                    break;
                }
        }

        yield break;

        static IEnumerable<(JsonElement, string)> ProcessProperties( JsonElement.ObjectEnumerator enumerator )
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

            yield return (property.Value, property.Name);
        }
    }

    private static IEnumerable<string> EnumeratePropertyNames( JsonElement value )
    {
        foreach ( var result in ProcessPropertyNames( value.EnumerateObject() ) )
            yield return result;

        yield break;

        static IEnumerable<string> ProcessPropertyNames( JsonElement.ObjectEnumerator enumerator )
        {
            if ( !enumerator.MoveNext() )
            {
                yield break;
            }

            var property = enumerator.Current;

            foreach ( var result in ProcessPropertyNames( enumerator ) )
            {
                yield return result;
            }

            yield return property.Name;
        }
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    internal override JsonElement GetElementAt( JsonElement value, int index )
    {
        return value[index];
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    internal override bool IsObjectOrArray( JsonElement value )
    {
        return value.IsObjectOrArray();
    }

    internal override bool IsArray( JsonElement value, out int length )
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
    internal override bool IsObject( JsonElement value )
    {
        return value.ValueKind is JsonValueKind.Object;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    internal override JsonPathElement CreateResult( JsonElement value, string path )
    {
        return new JsonPathElement( value, path );
    }

    internal override bool TryGetChildValue( in JsonElement value, ReadOnlySpan<char> childKey, out JsonElement childValue )
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

    internal override string GetPath( JsonElement value, string prefix, string childKey )
    {
        if ( value.ValueKind == JsonValueKind.Array )
            return $"{prefix}[{childKey}]";

        return childKey.IndexOfAny( SpecialCharacters ) == -1 ? $"{prefix}.{childKey}" : $@"{prefix}['{childKey}']";
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    internal override string GetPath( JsonElement value, string path )
    {
        return path[(path.LastIndexOf( ';' ) + 1)..];
    }
}
