using System.Globalization;
using Hyperbee.Json.Internal;

namespace Hyperbee.Json;

internal static class JsonPathSliceSyntaxHelper
{
    // parse slice expression and return normalized bounds
    public static (int Lower, int Upper, int Step) ParseExpression( ReadOnlySpan<char> sliceExpr, int length, bool reverse = false )
    {
        // parse the slice expression and return normalized bounds

        ReadOnlySpan<char> startSpan = default;
        ReadOnlySpan<char> endSpan = default;
        ReadOnlySpan<char> stepSpan = default;

        var count = 0;
        var splitter = new SpanSplitter<char>( sliceExpr, ':' );

        while ( splitter.TryMoveNext( out var part ) )
        {
            switch ( count++ )
            {
                case 0:
                    startSpan = part;
                    break;
                case 1:
                    endSpan = part;
                    break;
                case 2:
                    stepSpan = part;
                    break;
                default:
                    throw new InvalidOperationException( $"Invalid slice expression '{sliceExpr.ToString()}'. Too many elements." );
            }
        }

        var step = ParseStep( stepSpan, length );

        if ( step == 0 ) // step 0 should return an empty array
            return (0, 0, 0);

        var start = ParsePart( startSpan, defaultValue: step > 0 ? 0 : length - 1 );
        var end = ParsePart( endSpan, defaultValue: step > 0 ? length : -length - 1 );

        return GetBoundedValues( start, end, step, length, reverse );

        // helper to parse string part to an int

        static int ParseStep( ReadOnlySpan<char> part, int length )
        {
            // a little magic for overflow and underflow conditions cause by massive steps.
            // just scope the step to length + 1 or -length - 1.

            if ( !part.IsEmpty && long.TryParse( part, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n ) )
            {
                return n switch
                {
                    > 0 when n > length => length + 1,
                    < 0 when -n > length => -(length + 1),
                    _ => (int) n
                };
            }

            return 1;
        }

        static int ParsePart( ReadOnlySpan<char> part, int defaultValue )
        {
            if ( !part.IsEmpty && int.TryParse( part, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n ) )
                return n;

            return defaultValue;
        }
    }

    // helper to get bounded values 
    // https://www.rfc-editor.org/rfc/rfc9535.html#section-2.3.4.2.2

    private static (int Lower, int Upper, int Step) GetBoundedValues( int start, int end, int step, int length, bool reverse )
    {
        var normalizedStart = Normalize( start, length );
        var normalizedEnd = Normalize( end, length );

        int lower;
        int upper;

        if ( step >= 0 )
        {
            lower = Math.Min( Math.Max( normalizedStart, 0 ), length );
            upper = Math.Min( Math.Max( normalizedEnd, 0 ), length );
        }
        else
        {
            lower = Math.Min( Math.Max( normalizedEnd, -1 ), length - 1 );
            upper = Math.Min( Math.Max( normalizedStart, -1 ), length - 1 );
        }

        return reverse ? ReverseValues( lower, upper, step ) : (lower, upper, step);

        static int Normalize( int value, int length ) => value >= 0 ? value : length + value;
    }

    // rewrite the slice to execute in reverse order

    private static (int Lower, int Upper, int Step) ReverseValues( int lower, int upper, int step )
    {
        step = -step;

        // adjust upper for correct reverse iteration
        // upper may not be lower + (n * step) aligned

        var z = upper - lower; // subtract lower from upper to `shift` upper to a zero offset start
        var r = z % Math.Abs( step );

        if ( r > 0 ) // any remainder means we are not step aligned
        {
            var a = step > 0 ? r : -r;
            upper += a;
            lower += a;
        }
        else
        {
            upper += step;
            lower += step;
        }

        return (lower, upper, step);
    }
}
