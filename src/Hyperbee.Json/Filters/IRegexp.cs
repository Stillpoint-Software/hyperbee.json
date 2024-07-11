namespace Hyperbee.Json.Filters;

public static class IRegexp
{
    public static string ConvertToIRegexp( ReadOnlySpan<char> pattern )
    {
        // RFC-9535 States that regular expressions must conform to the I-Regexp format (RFC-9485)​.
        //
        // This requirement impacts DotNet regex for the dot( . ) character and treatment of Surrogate Pairs.
        //
        // I-Regexp, addresses the expectation for the dot (.) character in regular expressions:
        // The dot( . ) character should match any character except newline characters, including
        // surrogate pairs, which are treated as single characters in the context of matching. 
        //
        // Surrogate pairs are used to represent characters outside the BMP (Basic Multilingual Plane)
        // in UTF-16 encoding. They consist of a high surrogate (D800-DBFF) and a low surrogate (DC00-DFFF),
        // which are combined to represent a single character. DotNet does not handle surrogate pairs nicely.
        //
        // Further, DotNet regex does not match dot( . ) on `\r`, which is an expectation of the RFC-9535
        // compliance test suite.
        //
        // To address this, we need to rewrite the regex pattern to match the dot( . ) character as expected.

        // stackalloc a span to track positions for replacement

        if ( pattern.IsEmpty )
            return string.Empty;

        var patternSize = pattern.Length;
        Span<bool> dotPositions = patternSize > 256
            ? new bool[patternSize]
            : stackalloc bool[patternSize];

        var inCharacterClass = false;
        var dotCount = 0;

        for ( var i = 0; i < pattern.Length; i++ )
        {
            var currentChar = pattern[i];

            switch ( currentChar )
            {
                case '\\':
                    i++;
                    break;
                case '[':
                    inCharacterClass = true;
                    break;
                case ']' when inCharacterClass:
                    inCharacterClass = false;
                    break;
                case '.' when !inCharacterClass:
                    dotPositions[i] = true;
                    dotCount++;
                    break;
            }
        }

        if ( dotCount == 0 )
            return pattern.ToString();

        /*
         * Regex Rewrite Explanation: 
         *
         * 1. Non-Capturing Group `(?: ... )`
         *    - The entire pattern is wrapped in a non-capturing group to group the regex parts together
         *      without capturing the matched text.
         *
         * 2. Negative Lookahead `(?! ... )`
         *    - `(?![\r\n])`: Asserts that what immediately follows is not a carriage return (`\r`) or newline (`\n`).
         *      This solves the problem of matching any character except newline and carriage return characters.
         *
         * 3. Non-Surrogate Character `\P{Cs}`
         *    - `\P{Cs}`: Matches any character that is not in the "Cs" (surrogate) Unicode category.
         *      This ensures we exclude surrogate code points.
         *
         * 4. Surrogate Pair `\p{Cs}\p{Cs}`
         *    - `\p{Cs}`: Matches any character in the "Cs" (surrogate) Unicode category.
         *    - `\p{Cs}\p{Cs}`: Matches a surrogate pair, which consists of two surrogate characters in sequence.
         *
         * Overall Pattern:
         * - The pattern matches either:
         *   1. Any character that is not a surrogate and is not a newline (`\r` or `\n`), or
         *   2. A surrogate pair (two surrogate characters in sequence).
         *
         * This ensures that the regex matches any character except newline and carriage return characters,
         * while correctly handling surrogate pairs which are necessary for certain Unicode characters.
         *
         *
         * Pattern:
         *   (?:
         *       (?![\r\n])   # Negative lookahead to exclude \r and \n
         *       \P{Cs}       # Match any character that is not a surrogate
         *       |
         *       \p{Cs}\p{Cs} # Match a surrogate pair (two surrogates in sequence)
         *   )
         */
        var replacement = @"(?:(?![\r\n])\P{Cs}|\p{Cs}\p{Cs})".AsSpan();

        var newSize = pattern.Length + dotCount * (replacement.Length - 1); // '.' is 1 char, so extra (pattern-length - 1) chars per '.'
        Span<char> buffer = newSize > 512
            ? new char[newSize]
            : stackalloc char[newSize];

        var bufferIndex = 0;

        for ( var i = 0; i < pattern.Length; i++ )
        {
            if ( dotPositions[i] )
            {
                replacement.CopyTo( buffer[bufferIndex..] );
                bufferIndex += replacement.Length;
            }
            else
            {
                buffer[bufferIndex++] = pattern[i];
            }
        }

        return new string( buffer[..bufferIndex] );
    }
}
