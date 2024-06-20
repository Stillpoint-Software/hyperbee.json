using System.Globalization;

namespace Hyperbee.Json.Memory;

internal static class SliceSyntaxHelper
{
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

        var step = ParsePart( stepSpan, defaultValue: 1 );

        if ( step == 0 ) // step 0 should return an empty array
            return (0, 0, 0);

        var start = ParsePart( startSpan, defaultValue: step > 0 ? 0 : length - 1 );
        var end = ParsePart( endSpan, defaultValue: step > 0 ? length : -length - 1 );

        return GetBoundedValues( start, end, step, length, reverse );

        // helper to parse string part to an int
        
        static int ParsePart( ReadOnlySpan<char> part, int defaultValue )
        {
            if ( !part.IsEmpty )
                return int.TryParse( part, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n ) ? n : defaultValue;

            return defaultValue;
        }
    }

    // helper to get bounded values 
    // per https://datatracker.ietf.org/doc/rfc9535/ 2.3.4.2.2.

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

        return reverse ? ReverseBoundedValues( lower, upper, step ) : (lower, upper, step);

        static int Normalize( int value, int length ) => value >= 0 ? value : length + value;
    }

    // rewrite the slice to execute in reverse order

    private static (int Lower, int Upper, int Step) ReverseBoundedValues( int lower, int upper, int step )
    {
        step *= -1;

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
