using System.Text.Json;

#region License
// C# Implementation of IEqualityComparer<JsonElement>
//
// [1] https://stackoverflow.com/a/60592310
// [2] https://dotnetfiddle.net/ijrDBZ
//
// Attribution-ShareAlike 4.0 International (CC BY-SA 4.0) License
//
// https://creativecommons.org/licenses/by-sa/4.0/
//
#endregion

namespace Hyperbee.Json;

// example 1:
//
// var comparer = new JsonElementDeepEqualsComparer();
// using var doc1 = JsonDocument.Parse( referenceJson );
// using var doc2 = JsonDocument.Parse( resultJson );
//
// var result = comparer.Equals( doc1.RootElement, doc2.RootElement )
//
// example 2:
//
// using var doc1 = JsonDocument.Parse( referenceJson );
// using var doc2 = JsonDocument.Parse( resultJson );
//
// var result = doc1.RootElement.IsEquivalentTo( doc2.RootElement );
//
// example 3:
//
// var result = JsonHelper.Compare( referenceJson, resultJson );

public class JsonElementDeepEqualsComparer : IEqualityComparer<JsonElement>
{
    public JsonElementDeepEqualsComparer()
    {
    }

    public JsonElementDeepEqualsComparer( int maxHashDepth ) => MaxHashDepth = maxHashDepth;

    private int MaxHashDepth { get; }

    public bool Equals( JsonElement x, JsonElement y )
    {
        if ( x.ValueKind != y.ValueKind )
            return false;

        switch ( x.ValueKind )
        {
            case JsonValueKind.Null:
            case JsonValueKind.True:
            case JsonValueKind.False:
            case JsonValueKind.Undefined:
                return true;

            // Compare the raw values of numbers, and text.
            //
            // Note this means that 0.0 will differ from 0.00 -- which may be correct as deserializing either
            // to `decimal` will result in subtly different results.
            //
            // Newtonsoft's JValue.Compare(JTokenType valueType, object? objA, object? objB) has logic for
            // detecting "equivalent" values, you may want to examine it to see if anything there is required here.
            // https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Linq/JValue.cs#L246

            case JsonValueKind.Number:
                return x.GetRawText() == y.GetRawText();

            case JsonValueKind.String:
                return x.GetString() == y.GetString(); // GetRawText() doesn't resolve JSON escape sequences.

            case JsonValueKind.Array:
                return x.EnumerateArray().SequenceEqual( y.EnumerateArray(), this );

            case JsonValueKind.Object:
                
                // JsonDocument supports duplicate property names.
                //
                // https://tools.ietf.org/html/rfc8259#section-4 indicates such objects are allowed but not
                // recommended, and when they arise, interpretation of identically-named properties is
                // order-dependent. So stably sorting by name then comparing values seems the way to go.

                var xProperties = x.EnumerateObject().OrderBy( p => p.Name, StringComparer.Ordinal );
                var yProperties = y.EnumerateObject().OrderBy( p => p.Name, StringComparer.Ordinal );

                using ( var xEnumerator = xProperties.GetEnumerator() )
                using ( var yEnumerator = yProperties.GetEnumerator() )
                {
                    while ( true )
                    {
                        var xMoved = xEnumerator.MoveNext();
                        var yMoved = yEnumerator.MoveNext();

                        // check if both enumerations finished at the same time
                        if ( !xMoved && !yMoved )
                            return true;

                        // if only one enumeration finished, they are not equal
                        if ( xMoved != yMoved )
                            return false;

                        var px = xEnumerator.Current;
                        var py = yEnumerator.Current;

                        if ( px.Name != py.Name )
                            return false;

                        if ( !Equals( px.Value, py.Value ) )
                            return false;
                    }
                }

            default:
                throw new JsonException( $"Unknown {nameof( JsonValueKind )} {x.ValueKind}." );
        }
    }

    public int GetHashCode( JsonElement obj )
    {
        var hash = new HashCode();
        ComputeHashCode( obj, ref hash, 0 );

        return hash.ToHashCode();
    }

    private void ComputeHashCode( JsonElement obj, ref HashCode hash, int depth )
    {
        hash.Add( obj.ValueKind );

        switch ( obj.ValueKind )
        {
            case JsonValueKind.Null:
            case JsonValueKind.True:
            case JsonValueKind.False:
            case JsonValueKind.Undefined:
                break;

            case JsonValueKind.Number:
                hash.Add( obj.GetRawText() );
                break;

            case JsonValueKind.String:
                hash.Add( obj.GetString() );
                break;

            case JsonValueKind.Array:

                if ( depth != MaxHashDepth || MaxHashDepth < 1 )
                {
                    foreach ( var item in obj.EnumerateArray() )
                        ComputeHashCode( item, ref hash, depth + 1 );
                }
                else
                {
                    hash.Add( obj.GetArrayLength() );
                }

                break;

            case JsonValueKind.Object:
                foreach ( var property in obj.EnumerateObject().OrderBy( p => p.Name, StringComparer.Ordinal ) )
                {
                    hash.Add( property.Name );

                    if ( depth != MaxHashDepth )
                        ComputeHashCode( property.Value, ref hash, depth + 1 );
                }

                break;

            default:
                throw new JsonException( $"Unknown {nameof( JsonValueKind )} {obj.ValueKind}." );
        }
    }
}
