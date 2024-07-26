using Hyperbee.Json.Internal;

namespace Hyperbee.Json.Filters;

public static class IRegexp
{
    public static string ConvertToIRegexp( ReadOnlySpan<char> pattern )
    {
        // RFC-9535 States that regular expressions must conform to the I-Regexp format (RFC-9485).

        if ( pattern.IsEmpty )
            return string.Empty;

        // First loop: count the number of dots that need to be replaced
        var inCharacterClass = false;
        var dotCount = 0;

        for ( var i = 0; i < pattern.Length; i++ )
        {
            var currentChar = pattern[i];

            switch ( currentChar )
            {
                case '\\':
                    i++; // Skip the next character
                    break;
                case '[':
                    inCharacterClass = true;
                    break;
                case ']' when inCharacterClass:
                    inCharacterClass = false;
                    break;
                case '.' when !inCharacterClass:
                    dotCount++;
                    break;
            }
        }

        if ( dotCount == 0 )
            return pattern.ToString();

        // The replacement pattern for dots
        var replacement = @"(?:[^\r\n]|\p{Cs}\p{Cs})".AsSpan();
        var replacementLength = replacement.Length - 1;

        var newSize = pattern.Length + dotCount * replacementLength;

        var builder = newSize <= 512
            ? new ValueStringBuilder( stackalloc char[newSize] )
            : new ValueStringBuilder( newSize );

        // Second loop: process the pattern and build the result
        inCharacterClass = false;
        var start = 0;

        for ( var i = 0; i < pattern.Length; i++ )
        {
            var currentChar = pattern[i];

            switch ( currentChar )
            {
                case '\\':
                    i++; // Skip the next character
                    break;
                case '[':
                    inCharacterClass = true;
                    break;
                case ']' when inCharacterClass:
                    inCharacterClass = false;
                    break;
                case '.' when !inCharacterClass:
                    if ( i > start )
                        builder.Append( pattern.Slice( start, i - start ) );

                    builder.Append( replacement );
                    start = i + 1;
                    break;
            }
        }

        if ( start < pattern.Length ) // Append remaining
            builder.Append( pattern[start..] );

        return builder.ToString();
    }
}
