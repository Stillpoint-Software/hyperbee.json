using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Hyperbee.Json.Internal;

namespace Hyperbee.Json;

[Flags]
public enum SelectorKind
{
    Undefined = 0x0,

    // subtype
    Singular = 0x1,
    Group = 0x2,

    // selectors
    Root = 0x8 | Singular,
    Name = 0x10 | Singular,
    Index = 0x20 | Singular,
    Slice = 0x40 | Group,
    Filter = 0x80 | Group,
    Wildcard = 0x100 | Group,
    Descendant = 0x200 | Group
}

internal static class JsonPathQueryParser
{
    private static readonly ConcurrentDictionary<string, JsonPathQuery> JsonPathQueries = new();

    private enum State
    {
        Undefined,
        Whitespace,
        Start,
        DotChild,
        UnionStart,
        UnionElement,
        UnionNext,
        Finish,
        Final
    }

    internal static JsonPathQuery Parse( string query, bool allowDotWhitespace = false )
    {
        return JsonPathQueries.GetOrAdd( query, x => QueryFactory( x.AsSpan(), allowDotWhitespace ) );
    }

    internal static JsonPathQuery ParseNoCache( ReadOnlySpan<char> query, bool allowDotWhitespace = false )
    {
        return QueryFactory( query, allowDotWhitespace );
    }

    private static JsonPathQuery QueryFactory( ReadOnlySpan<char> query, bool allowDotWhitespace = false )
    {
        var tokens = new List<JsonPathSegment>();

        if ( StartsOrEndsWithWhitespace( query ) ) // RFC
            throw new NotSupportedException( "Query cannot start or end with whitespace." );

        var i = 0;
        var n = query.Length;

        var selectorStart = 0;

        var inQuotes = false;
        var quoteChar = '\'';
        bool escaped = false;
        var bracketDepth = 0;
        var parenDepth = 0;
        char[] whitespaceTerminators = [];

        var selectors = new List<SelectorDescriptor>();

        var state = State.Start;
        State returnState = State.Undefined;

        do
        {
            // read next character
            char c;

            if ( i < n )
            {
                c = query[i++];
            }
            else // end of input
            {
                if ( state != State.Whitespace ) // whitespace is a sub-state, allow it to exit
                    state = State.Finish;
                c = '\0'; // Set char to null terminator to signal end of input
            }

            // process character
            ReadOnlySpan<char> selectorSpan;
            SelectorKind selectorKind;

            switch ( state )
            {
                case State.Start:
                    switch ( c )
                    {
                        case '@': // Technically invalid, but allows `@` to work on sub queries without changing tokenizer 
                        case '$':

                            if ( query[^1] == '.' && query[^2] == '.' )
                                throw new NotSupportedException( "`..` cannot be the last segment." );

                            InsertToken( tokens, new SelectorDescriptor { SelectorKind = SelectorKind.Root, Value = c.ToString() } );

                            whitespaceTerminators = ['.', '['];
                            state = State.Whitespace;
                            returnState = State.DotChild;
                            break;
                        default:
                            throw new NotSupportedException( $"Invalid character `{c}` at pos {i - 1}." );
                    }

                    break;

                case State.Whitespace:
                    switch ( c )
                    {
                        case ' ':
                        case '\t':
                        case '\n':
                        case '\r':
                            break;
                        default:

                            if ( c != '\0' && whitespaceTerminators.Length > 0 && !whitespaceTerminators.Contains( c ) )
                                throw new NotSupportedException( $"Invalid character `{c}` at pos {i - 1}." );

                            whitespaceTerminators = [];
                            state = returnState; // transition back to the appropriate state
                            selectorStart = i; // start of the next selector

                            i--; // replay character
                            break;
                    }

                    break;

                case State.DotChild:
                    switch ( c )
                    {
                        case '[': // end-of-child
                            selectorSpan = GetSelectorSpan( state, query, selectorStart, i );
                            selectorKind = selectorSpan switch
                            {
                                "$" when tokens.Count != 0 => throw new NotSupportedException( $"Invalid use of root `$` at pos {i - 1}." ),
                                "@" when tokens.Count != 0 => throw new NotSupportedException( $"Invalid use of local root `$` at pos {i - 1}." ),
                                "*" => SelectorKind.Wildcard,
                                _ => SelectorKind.Name
                            };

                            if ( selectorKind == SelectorKind.Name && !selectorSpan.IsEmpty )
                            {
                                ThrowIfQuoted( selectorSpan );
                                ThrowIfNotValidUnquotedName( selectorSpan );
                            }

                            InsertToken( tokens, GetSelectorDescriptor( selectorKind, selectorSpan ) );

                            state = State.Whitespace;
                            i--; // replay character
                            returnState = State.UnionStart;
                            break;

                        case '.': // end-of-child
                            if ( i == n )
                                throw new NotSupportedException( $"Missing character after `.` at pos {i - 1}." );

                            selectorSpan = GetSelectorSpan( state, query, selectorStart, i );
                            selectorKind = selectorSpan switch
                            {
                                "$" when tokens.Count != 0 => throw new NotSupportedException( $"Invalid use of root `$` at pos {i - 1}." ),
                                "@" when tokens.Count != 0 => throw new NotSupportedException( $"Invalid use of local root `$` at pos {i - 1}." ),
                                "*" => SelectorKind.Wildcard,
                                _ => SelectorKind.Name
                            };

                            if ( selectorKind == SelectorKind.Name && !selectorSpan.IsEmpty ) // can be null after a union
                            {
                                ThrowIfQuoted( selectorSpan );
                                ThrowIfNotValidUnquotedName( selectorSpan );
                            }

                            InsertToken( tokens, GetSelectorDescriptor( selectorKind, selectorSpan ) );

                            if ( i < n && query[i] == '.' ) // peek next character
                            {
                                InsertToken( tokens, GetSelectorDescriptor( SelectorKind.Descendant, ".." ) );
                                i++;
                            }

                            selectorStart = i;
                            break;

                        case '\'':
                        case '"':
                            throw new NotSupportedException( $"Quoted member names are not allowed in dot notation at pos {i - 1}." );
                        case ' ':
                        case '\t':
                        case '\n':
                        case '\r':
                            if ( !allowDotWhitespace ) // filter dot notation allows whitespace, query dot notation does not
                                throw new NotSupportedException( $"Invalid whitespace in object notation at pos {i - 1}." );
                            break;
                    }

                    break;

                case State.UnionStart:
                    switch ( c )
                    {
                        case '.': // .. descendant
                            if ( i > n || query[i] != '.' )
                                throw new NotSupportedException( $"Invalid `.` in bracket expression at pos {i - 1}." );

                            state = State.UnionNext;
                            InsertToken( tokens, new SelectorDescriptor { SelectorKind = SelectorKind.Descendant, Value = ".." } );
                            i++;
                            break;
                        default:
                            state = State.UnionElement;
                            selectorStart = i;
                            bracketDepth = 1;
                            break;
                    }

                    break;

                case State.UnionElement:

                    if ( inQuotes )
                    {
                        if ( c == '\\' ) // handle escaping
                        {
                            escaped = true;
                            i++; // advance past the escaped character
                        }
                        else if ( c == quoteChar )
                        {
                            inQuotes = false;
                        }

                        continue;
                    }

                    switch ( c )
                    {
                        case '\'':
                        case '"':
                            quoteChar = c;
                            inQuotes = true;
                            break;

                        case '[': // handle nested `[` (not called for first bracket)
                            bracketDepth++;
                            break;
                        case '(': // handle nested `(` (not called for first bracket)
                            parenDepth++;
                            break;
                        case ')':
                            parenDepth--;
                            break;
                        case ',':
                        case ']':
                            if ( c == ']' && bracketDepth-- > 1 ) // handle nested `]`
                                break;
                            if ( c == ',' && bracketDepth > 1 )
                                break;
                            if ( parenDepth > 0 )
                                break;

                            // get the selector
                            selectorSpan = GetSelectorSpan( state, query, selectorStart, i );
                            selectorStart = i;

                            if ( selectorSpan.IsEmpty ) // [] is not valid
                                throw new NotSupportedException( "Invalid bracket expression syntax. Bracket expression cannot be empty." );

                            // validate the selector and get its kind
                            selectorKind = GetValidatedSelectorKind( selectorSpan );

                            // create the selector descriptor
                            SelectorDescriptor descriptor;

                            switch ( selectorKind )
                            {
                                case SelectorKind.Undefined:
                                    throw new NotSupportedException( $"Invalid bracket expression syntax. Unrecognized selector format at pos {i - 1}." );

                                case SelectorKind.Name:
                                    ThrowIfNotValidQuotedName( selectorSpan );
                                    if ( !escaped )
                                    {
                                        descriptor = GetSelectorDescriptor( selectorKind, selectorSpan[1..^1], nullable: false ); // unquote
                                    }
                                    else
                                    {
                                        var builder = new SpanBuilder( selectorSpan.Length );
                                        SpanHelper.Unescape( selectorSpan, ref builder, SpanUnescapeOptions.SingleThenUnquote ); // unescape and then unquote
                                        descriptor = GetSelectorDescriptor( selectorKind, builder, nullable: false );
                                        builder.Dispose();
                                        escaped = false;
                                    }
                                    break;

                                case SelectorKind.Filter:
                                    if ( !escaped )
                                    {
                                        descriptor = GetSelectorDescriptor( selectorKind, selectorSpan );
                                    }
                                    else
                                    {
                                        var builder = new SpanBuilder( selectorSpan.Length );
                                        SpanHelper.Unescape( selectorSpan, ref builder, SpanUnescapeOptions.Mixed ); // unescape one or more strings
                                        descriptor = GetSelectorDescriptor( selectorKind, builder );
                                        builder.Dispose();
                                        escaped = false;
                                    }
                                    break;

                                default:
                                    descriptor = GetSelectorDescriptor( selectorKind, selectorSpan );
                                    break;
                            }

                            selectors.Insert( 0, descriptor );

                            // continue parsing the union

                            switch ( c )
                            {
                                case ',':
                                    whitespaceTerminators = [];
                                    state = State.Whitespace;
                                    returnState = State.UnionNext;
                                    break;
                                case ']':
                                    InsertToken( tokens, [.. selectors] );
                                    selectors.Clear();

                                    whitespaceTerminators = ['.', '['];
                                    state = State.Whitespace;
                                    returnState = State.DotChild;
                                    break;
                            }

                            break;
                    }

                    break;

                case State.UnionNext:
                    switch ( c )
                    {
                        case ']':
                            if ( i < n && query[i] != '.' && query[i] != '[' )
                                throw new NotSupportedException( $"Invalid character after `]` at pos {i - 1}." );
                            state = State.DotChild;
                            selectorStart = i;
                            break;
                        case '\'':
                        case '"':
                            state = State.UnionElement;
                            quoteChar = c;
                            selectorStart = i - 1; // capture the quote character
                            inQuotes = true;
                            break;
                        default:
                            state = State.UnionElement;
                            i--; // replay character
                            selectorStart = i;
                            break;
                    }

                    break;

                case State.Finish:
                    selectorSpan = GetSelectorSpan( state, query, selectorStart, i );
                    if ( !selectorSpan.IsEmpty )
                    {
                        var finalKind = selectorSpan switch
                        {
                            "*" => SelectorKind.Wildcard,
                            ".." => SelectorKind.Descendant,
                            _ => SelectorKind.Name
                        };

                        if ( finalKind == SelectorKind.Name )
                        {
                            ThrowIfQuoted( selectorSpan );
                            ThrowIfNotValidUnquotedName( selectorSpan );
                        }

                        InsertToken( tokens, GetSelectorDescriptor( finalKind, selectorSpan ) );
                    }

                    state = State.Final;
                    break;

                default:
                    throw new InvalidOperationException();
            }
        } while ( state != State.Final );

        // return tokenized query as a segment list

        return BuildJsonPathQuery( query, tokens );

        static bool StartsOrEndsWithWhitespace( ReadOnlySpan<char> span )
        {
            return !span.IsEmpty && (char.IsWhiteSpace( span[0] ) || char.IsWhiteSpace( span[^1] ));
        }
    }

    private static JsonPathQuery BuildJsonPathQuery( ReadOnlySpan<char> query, IList<JsonPathSegment> segments )
    {
        if ( segments == null || segments.Count == 0 )
            return new JsonPathQuery( query.ToString(), JsonPathSegment.Final, false );

        // set the next properties

        for ( var index = 0; index < segments.Count; index++ )
        {
            var segment = segments[index];

            segment.Next = index != segments.Count - 1
                ? segments[index + 1]
                : JsonPathSegment.Final;
        }

        var rootSegment = segments.First();
        var normalized = rootSegment.IsNormalized;

        return new JsonPathQuery( query.ToString(), rootSegment, normalized );
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static SelectorDescriptor GetSelectorDescriptor( SelectorKind selectorKind, ReadOnlySpan<char> selectorSpan, bool nullable = true )
    {
        var selectorValue = selectorSpan.IsEmpty && nullable ? null : selectorSpan.ToString();
        return new SelectorDescriptor { SelectorKind = selectorKind, Value = selectorValue };
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static SelectorDescriptor GetSelectorDescriptor( SelectorKind selectorKind, in SpanBuilder builder, bool nullable = true )
    {
        var selectorValue = builder.ToString();

        if ( selectorValue == string.Empty && !nullable )
            selectorValue = null;

        return new SelectorDescriptor { SelectorKind = selectorKind, Value = selectorValue };
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static ReadOnlySpan<char> GetSelectorSpan( State state, ReadOnlySpan<char> buffer, int start, int stop )
    {
        var adjust = state == State.Finish || state == State.Final ? 0 : 1; // non-final states have already advanced to the next character, so we need to subtract 1
        var length = stop - start - adjust;
        return length <= 0 ? [] : buffer.Slice( start, length ).Trim();
    }

    private static SelectorKind GetValidatedSelectorKind( ReadOnlySpan<char> selector )
    {
        switch ( selector )
        {
            case "*":
                return SelectorKind.Wildcard;
            case "..":
                return SelectorKind.Descendant;
        }

        if ( IsQuoted( selector ) )
            return SelectorKind.Name;

        if ( IsIndex( selector, out var isValid, out var reason ) )
        {
            if ( !isValid ) // it is an index, but invalid
                throw new NotSupportedException( reason );

            return SelectorKind.Index;
        }

        if ( IsFilter( selector ) )
            return SelectorKind.Filter;

        if ( IsSlice( selector, out isValid, out reason ) )
        {
            if ( !isValid ) // it is a slice, but invalid
                throw new NotSupportedException( reason );

            return SelectorKind.Slice;
        }

        return SelectorKind.Undefined;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static void InsertToken( ICollection<JsonPathSegment> tokens, SelectorDescriptor selector )
    {
        if ( selector?.Value == null )
            return;

        InsertToken( tokens, [selector] );
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static void InsertToken( ICollection<JsonPathSegment> tokens, SelectorDescriptor[] selectors )
    {
        if ( selectors == null || selectors.Length == 0 )
            return;

        tokens.Add( new JsonPathSegment( selectors ) );
    }

    private static bool IsFilter( ReadOnlySpan<char> input )
    {
        if ( input.Length < 2 || input[0] != '?' )
            return false;

        var start = 1;
        var end = input.Length;

        if ( input[1] == '(' )
        {
            start = 2;
            if ( input[^1] == ')' )
                end--;
        }

        var result = start < end;

        return result;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static bool IsIndex( ReadOnlySpan<char> input, out bool isValid, out string reason )
    {
        return IsValidNumber( input, out isValid, out reason );
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static bool IsQuoted( ReadOnlySpan<char> input )
    {
        return input.Length > 1 && input[0] == '"' && input[^1] == '"' || input[0] == '\'' && input[^1] == '\'';
    }

    private static bool IsSlice( ReadOnlySpan<char> input, out bool isValid, out string reason )
    {
        var index = 0;
        isValid = true;
        reason = string.Empty;
        var partCount = 0;

        SkipWhitespace( input, ref index );

        do
        {
            // Validate each part (optional number)
            if ( !ValidatePart( input, ref index, ref isValid, ref reason ) )
            {
                if ( !isValid )
                    reason = "Invalid number in slice.";
                return partCount > 0; // Return true if at least one colon was found, indicating it was intended as a slice
            }

            partCount++;

            SkipWhitespace( input, ref index );

            // Check for optional colon
            if ( index >= input.Length || input[index] != ':' )
                break;

            index++;
            SkipWhitespace( input, ref index );

        } while ( partCount < 3 && index < input.Length );

        if ( index != input.Length )
        {
            isValid = false;
            reason = "Unexpected characters at the end of slice.";
        }

        return partCount > 0; // Return true if at least one colon was found, indicating it was intended as a slice

        // Helper method to validate each part of the slice
        static bool ValidatePart( ReadOnlySpan<char> span, ref int idx, ref bool isValid, ref string reason )
        {
            SkipWhitespace( span, ref idx );

            var start = idx;

            if ( idx < span.Length && (span[idx] == '-') )
                idx++;

            while ( idx < span.Length && char.IsDigit( span[idx] ) )
                idx++;

            // Allow empty
            if ( start == idx )
                return true;

            // Check for leading zeros in unsigned or signed numbers
            if ( !IsValidNumber( span[start..idx], out isValid, out reason ) )
                return false;

            var isValidNumber = idx > start || start == idx;

            if ( !isValidNumber )
            {
                isValid = false;
                reason = "Invalid number format.";
            }

            return isValidNumber; // True if there was a number or just an optional sign
        }

        // Helper method to skip whitespace
        static void SkipWhitespace( ReadOnlySpan<char> span, ref int idx )
        {
            while ( idx < span.Length && char.IsWhiteSpace( span[idx] ) )
            {
                idx++;
            }
        }
    }

    private static bool IsValidNumber( ReadOnlySpan<char> input, out bool isValid, out string reason )
    {
        isValid = true;
        reason = string.Empty;

        if ( input.Length == 0 )
        {
            isValid = false;
            reason = "Input is empty.";
            return false;
        }

        int start = 0;

        // Handle optional leading negative sign
        if ( input[0] == '-' )
        {
            start = 1;
            if ( input.Length == 1 )
            {
                isValid = false;
                reason = "Invalid negative number.";
                return false;
            }
        }

        // Check for leading zeros
        if ( input[start] == '0' && input.Length > (start + 1) )
        {
            isValid = false;
            reason = "Leading zeros are not allowed.";
            return false;
        }

        // Check if all remaining characters are digits
        for ( var i = start; i < input.Length; i++ )
        {
            if ( char.IsDigit( input[i] ) )
                continue;

            isValid = false;
            reason = "Input contains non-digit characters.";
            return false;
        }

        // Try parse to detect overflow
        if ( long.TryParse( input, out _ ) )
            return true; // It's a valid number

        isValid = false;
        reason = "Input is too large.";
        return false;
    }

    private static void ThrowIfQuoted( ReadOnlySpan<char> value )
    {
        if ( IsQuoted( value ) )
            throw new NotSupportedException( $"Quoted member names are not allowed in dot notation: {value}" );
    }

    private static void ThrowIfNotValidUnquotedName( ReadOnlySpan<char> name )
    {
        if ( name.IsEmpty )
            throw new NotSupportedException( "Selector name cannot be null." );

        // Validate the first character
        if ( !IsValidFirstChar( name[0] ) )
            throw new NotSupportedException( $"Selector name cannot start with `{name[0]}`." );

        // Validate subsequent characters
        for ( int i = 1; i < name.Length; i++ )
        {
            if ( !IsValidSubsequentChar( name[i] ) )
                throw new NotSupportedException( $"Selector name cannot contain `{name[i]}`." );
        }

        return;

        static bool IsValidFirstChar( char c ) => char.IsLetter( c ) || c == '_' || c >= 0x80;
        static bool IsValidSubsequentChar( char c ) => char.IsLetterOrDigit( c ) || c == '_' || c == '-' || c >= 0x80;
    }

    private static void ThrowIfNotValidQuotedName( ReadOnlySpan<char> name )
    {
        if ( name.IsEmpty )
            throw new NotSupportedException( "Selector name cannot be empty." );

        char quoteChar = name[0];
        if ( name.Length < 2 || (quoteChar != '"' && quoteChar != '\'') || name[^1] != quoteChar )
            throw new NotSupportedException( "Quoted name must start and end with the same quote character, either double or single quote." );

        for ( int i = 1; i < name.Length - 1; i++ )
        {
            if ( name[i] == '\\' )
            {
                // Check if it's a valid escape sequence
                if ( i + 1 >= name.Length - 1 || !IsValidEscapeChar( name[i + 1], quoteChar ) )
                    throw new NotSupportedException( "Invalid escape sequence in quoted name." );

                if ( name[i + 1] == 'u' )
                {
                    // Ensure it's a valid Unicode escape sequence (e.g., \u263a)
                    if ( i + 5 >= name.Length - 1 || !IsValidUnicodeEscapeSequence( name.Slice( i, 6 ) ) )
                        throw new NotSupportedException( "Invalid Unicode escape sequence in quoted name." );
                    i += 5; // Skip the Unicode escape sequence
                }
                else
                {
                    i++; // Skip the regular escape character
                }
            }
            else if ( name[i] == quoteChar )
            {
                // Unescaped quotes are not allowed inside the quoted name.
                throw new NotSupportedException( "Unescaped quote characters are not allowed inside a quoted name." );
            }
            else if ( name[i] <= '\u001F' )
            {
                // Control characters (U+0000 to U+001F) are not allowed.
                throw new NotSupportedException( $"Control character '\\u{(int) name[i]:x4}' is not allowed in a quoted name." );
            }
        }

        return;

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static bool IsValidEscapeChar( char escapeChar, char quoteChar )
        {
            return
                escapeChar == quoteChar ||
                escapeChar == '\\' ||
                escapeChar == '/' || escapeChar == 'b' ||
                escapeChar == 'f' || escapeChar == 'n' ||
                escapeChar == 'r' || escapeChar == 't' ||
                escapeChar == 'u'
            ;
        }

        static bool IsValidUnicodeEscapeSequence( ReadOnlySpan<char> span )
        {
            if ( span.Length != 6 || span[1] != 'u' )
                return false;

            for ( int i = 2; i < 6; i++ )
            {
                if ( !char.IsAsciiHexDigit( span[i] ) )
                    return false;
            }

            return true;
        }
    }
}
