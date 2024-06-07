﻿using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Nodes;

internal class JsonNodePathVisitor : JsonPathVisitorBase<JsonNode, JsonNode>
{
    internal override IEnumerable<string> EnumerateKeys( JsonNode value )
    {
        return value switch
        {
            JsonArray valueArray => EnumerateArrayIndices( valueArray.Count ).Select( x => x.ToString() ),
            JsonObject valueObject => EnumeratePropertyNames( valueObject ),
            _ => throw new NotSupportedException()
        };
    }

    internal override IEnumerable<(JsonNode, string)> EnumerateChildValues( JsonNode value )
    {
        switch ( value )
        {
            case JsonArray arrayValue:
            {
                for ( var index = arrayValue.Count - 1; index >= 0; index-- )
                {
                    yield return (value[index], index.ToString());
                }

                break;
            }
            case JsonObject objectValue:
            {
                foreach ( var result in ProcessProperties( objectValue.GetEnumerator() ) )
                    yield return result;

                break;
            }
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

    private static IEnumerable<string> EnumeratePropertyNames( JsonNode value )
    {
        foreach ( var result in ProcessProperties( (value as JsonObject).GetEnumerator() ) )
            yield return result;

        yield break;

        static IEnumerable<string> ProcessProperties( IEnumerator<KeyValuePair<string, JsonNode>> enumerator )
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

            yield return property.Key;
        }
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    internal override JsonNode GetElementAt( JsonNode value, int index )
    {
        return value[index];
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    internal override bool IsObjectOrArray( JsonNode value )
    {
        return value is JsonObject or JsonArray;
    }

    internal override bool IsArray( JsonNode value, out int length )
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
    internal override bool IsObject( JsonNode value )
    {
        return value is JsonObject;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    internal override JsonNode CreateResult( JsonNode value, string path )
    {
        return value;
    }

    internal override bool TryGetChildValue( in JsonNode value, ReadOnlySpan<char> childKey, out JsonNode childValue )
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

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    internal override string GetPath( JsonNode value, string path, string selector )
    {
        return value.GetPath();
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    internal override string GetPath( JsonNode value, string path )
    {
        return value.GetPath();
    }
}
