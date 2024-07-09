namespace Hyperbee.Json.Internal;

internal static class QueryHelper
{
    public static bool IsNonSingular( ReadOnlySpan<char> query )
    {
        bool inQuotes = false;
        char quoteChar = '\0';

        // Check for any special characters that would indicate a non-singular query

        for ( var i = 0; i < query.Length; i++ )
        {
            char current = query[i];

            if ( inQuotes )
            {
                if ( current != '\\' && current == quoteChar )
                {
                    inQuotes = false;
                    quoteChar = '\0';
                }

                continue;
            }

            switch ( current )
            {
                case '\'':
                case '"':
                    quoteChar = current;
                    inQuotes = true;
                    continue;
                case '*':
                case ',':
                case ':':
                    return true;
                case '.':
                    if ( i + 1 < query.Length && query[i + 1] == '.' ) // ..
                        return true;

                    break;
            }
        }

        return false;
    }
}
