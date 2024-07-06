using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Hyperbee.Json;

[Flags]
public enum SelectorKind
{
    Undefined = 0x0,

    // subtype
    Singular = 0x1,
    Group = 0x2,

    // dot notation
    Root = 0x4 | Singular,
    DotName = 0x8 | Singular,

    // union notation
    Name = 0x10 | Singular,
    Index = 0x20 | Singular,
    Slice = 0x40 | Group,
    Filter = 0x80 | Group,

    // 
    Wildcard = 0x100 | Group,
    Descendant = 0x200 | Group
}

internal static class JsonPathQueryParser
{
    private static readonly ConcurrentDictionary<string, JsonPathSegment> JsonPathTokens = new();

    private enum State
    {
        Undefined,
        Whitespace,
        Start,
        DotChild,
        UnionStart,
        UnionQuotedFinal,
        UnionElement,
        UnionNext,
        UnionFinal,
        QuotedName,
        FinalSelector,
        Final
    }

    private static string GetSelector( State state, ReadOnlySpan<char> buffer, int start, int stop )
    {
        var adjust = state == State.FinalSelector || state == State.Final ? 0 : 1; // non-final states have already advanced to the next character, so we need to subtract 1
        var length = stop - start - adjust;
        return length <= 0 ? null : buffer.Slice( start, length ).Trim().ToString();
    }

    private static void InsertToken( ICollection<JsonPathSegment> tokens, SelectorDescriptor selector )
    {
        if ( selector?.Value == null )
            return;

        InsertToken( tokens, [selector] );
    }

    private static void InsertToken( ICollection<JsonPathSegment> tokens, SelectorDescriptor[] selectors )
    {
        if ( selectors == null || selectors.Length == 0 )
            return;

        tokens.Add( new JsonPathSegment( selectors ) );
    }

    internal static JsonPathSegment Parse( string query )
    {
        return JsonPathTokens.GetOrAdd( query, x => TokenFactory( x.AsSpan() ) );
    }

    internal static JsonPathSegment ParseNoCache( ReadOnlySpan<char> query )
    {
        return TokenFactory( query );
    }

    private static JsonPathSegment TokenFactory( ReadOnlySpan<char> query )
    {
        var tokens = new List<JsonPathSegment>();

        if ( StartsOrEndsWithWhitespace( query ) ) // RFC
            throw new NotSupportedException( "Query cannot start or end with whitespace." );

        var i = 0;
        var n = query.Length;

        var selectorStart = 0;

        var bracketDepth = 0;
        var parenDepth = 0;
        var literalDelimiter = '\'';
        var selectors = new List<SelectorDescriptor>();

        char[] whitespaceTerminators = [];

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
                    state = State.FinalSelector;
                c = '\0'; // Set char to null terminator to signal end of input
            }

            // process character

            SelectorKind selectorKind;
            string selectorValue;

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

                case State.QuotedName:
                    if ( c == '\\' ) // handle escaping
                    {
                        i++; // advance past the escaped character
                    }
                    else if ( c == literalDelimiter )
                    {
                        state = returnState; // transition back to the appropriate state
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
                        case '[':
                            selectorValue = GetSelector( state, query, selectorStart, i );
                            selectorKind = selectorValue switch
                            {
                                "$" when tokens.Count != 0 => throw new NotSupportedException( $"Invalid use of root `$` at pos {i - 1}." ),
                                "@" when tokens.Count != 0 => throw new NotSupportedException( $"Invalid use of local root `$` at pos {i - 1}." ),
                                "*" => SelectorKind.Wildcard,
                                _ => SelectorKind.DotName
                            };

                            if ( selectorKind == SelectorKind.DotName && selectorValue != null )
                            {
                                ThrowIfQuoted( selectorValue );
                                ThrowIfNotValidUnquotedName( selectorValue );
                            }

                            InsertToken( tokens, new SelectorDescriptor { SelectorKind = selectorKind, Value = selectorValue } );

                            state = State.Whitespace;
                            returnState = State.UnionStart;
                            break;
                        case '.':
                            if ( i == n )
                                throw new NotSupportedException( $"Missing character after `.` at pos {i - 1}." );

                            selectorValue = GetSelector( state, query, selectorStart, i );
                            selectorKind = selectorValue switch
                            {
                                "$" when tokens.Count != 0 => throw new NotSupportedException( $"Invalid use of root `$` at pos {i - 1}." ),
                                "@" when tokens.Count != 0 => throw new NotSupportedException( $"Invalid use of local root `$` at pos {i - 1}." ),
                                "*" => SelectorKind.Wildcard,
                                _ => SelectorKind.DotName
                            };

                            if ( selectorKind == SelectorKind.DotName && selectorValue != null ) // can be null after a union
                            {
                                ThrowIfQuoted( selectorValue );
                                ThrowIfNotValidUnquotedName( selectorValue );
                            }

                            InsertToken( tokens, new SelectorDescriptor { SelectorKind = selectorKind, Value = selectorValue } );

                            if ( i < n && query[i] == '.' ) // peek next character
                            {
                                InsertToken( tokens, new SelectorDescriptor { SelectorKind = SelectorKind.Descendant, Value = ".." } );
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
                            throw new NotSupportedException( $"Invalid whitespace in object notation at pos {i - 1}." );
                    }

                    break;

                case State.UnionStart:
                    switch ( c )
                    {
                        //case '*':
                        //    state = State.UnionFinal;
                        //    InsertToken( tokens, new SelectorDescriptor { SelectorKind = SelectorKind.Wildcard, Value = "*" } );
                        //    break;
                        case '.':
                            if ( i > n || query[i] != '.' )
                                throw new NotSupportedException( $"Invalid `.` in bracket expression at pos {i - 1}." );

                            state = State.UnionFinal;
                            InsertToken( tokens, new SelectorDescriptor { SelectorKind = SelectorKind.Descendant, Value = ".." } );
                            i++;
                            break;
                        case '\'':
                        case '"':
                            state = State.QuotedName;
                            returnState = State.UnionQuotedFinal;
                            literalDelimiter = c;
                            selectorStart = i - 1;
                            bracketDepth = 1;
                            break;
                        default:
                            state = State.UnionElement;
                            i--; // replay character
                            selectorStart = i;
                            bracketDepth = 1;
                            break;
                    }

                    break;

                case State.UnionQuotedFinal:
                    switch ( c )
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n':
                            break;
                        case ']':
                        case ',':
                            state = State.UnionElement;
                            i--; // replay character

                            break;
                        default: // invalid characters after end of string
                            throw new NotSupportedException( $"Invalid bracket literal at pos {i - 1}." );
                    }

                    break;

                case State.UnionElement:
                    switch ( c )
                    {
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
                            if ( c == ']' && --bracketDepth > 0 ) // handle nested `]`
                                break;
                            if ( parenDepth > 0 )
                                break;

                            // get the child item atom

                            selectorValue = GetSelector( state, query, selectorStart, i );
                            selectorStart = i;

                            // validate the extracted atom value shape

                            if ( string.IsNullOrEmpty( selectorValue ) ) // [] is not valid
                                throw new NotSupportedException( "Invalid bracket expression syntax. Bracket expression cannot be empty." );

                            selectorKind = GetSelectorKind( selectorValue );

                            switch ( selectorKind )
                            {
                                case SelectorKind.Undefined:
                                    throw new NotSupportedException( $"Invalid bracket expression syntax. Unrecognized selector format at pos {i - 1}." );
                                case SelectorKind.Name:
                                    ThrowIfNotValidQuotedName( selectorValue );
                                    selectorValue = UnquoteAndUnescape( selectorValue );
                                    break;
                            }

                            selectors.Insert( 0, new SelectorDescriptor { SelectorKind = selectorKind, Value = selectorValue } );

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
                case State.UnionFinal:
                    switch ( c )
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n':
                            break;
                        case ']':
                            if ( i < n && query[i] != '.' && query[i] != '[' )
                                throw new NotSupportedException( $"Invalid character after `]` at pos {i - 1}." );
                            state = State.DotChild;
                            selectorStart = i;
                            break;
                        case '\'':
                        case '"':
                            if ( state != State.UnionNext )
                                throw new NotSupportedException( $"Invalid bracket syntax at pos {i - 1}." );

                            returnState = State.UnionQuotedFinal;
                            state = State.QuotedName;
                            literalDelimiter = c;
                            selectorStart = i - 1;
                            break;
                        default:
                            if ( state != State.UnionNext )
                                throw new NotSupportedException( $"Invalid bracket syntax at pos {i - 1}." );

                            state = State.UnionElement;
                            i--; // replay character
                            selectorStart = i;

                            break;
                    }

                    break;

                case State.FinalSelector:
                    selectorValue = GetSelector( state, query, selectorStart, i );
                    if ( selectorValue != null )
                    {
                        var finalKind = selectorValue switch
                        {
                            "*" => SelectorKind.Wildcard,
                            ".." => SelectorKind.Descendant,
                            _ => SelectorKind.DotName
                        };

                        if ( finalKind == SelectorKind.DotName )
                        {
                            ThrowIfQuoted( selectorValue );
                            ThrowIfNotValidUnquotedName( selectorValue );
                        }

                        InsertToken( tokens, new SelectorDescriptor { SelectorKind = finalKind, Value = selectorValue } );
                    }

                    state = State.Final;
                    break;

                default:
                    throw new InvalidOperationException();
            }
        } while ( state != State.Final );

        // return tokenized query as a segment list

        return TokensToSegment( tokens );

        static bool StartsOrEndsWithWhitespace( ReadOnlySpan<char> span )
        {
            return !span.IsEmpty && (char.IsWhiteSpace( span[0] ) || char.IsWhiteSpace( span[^1] ));
        }
    }

    private static JsonPathSegment TokensToSegment( IList<JsonPathSegment> tokens )
    {
        if ( tokens == null || tokens.Count == 0 )
            return JsonPathSegment.Final;

        // set the next properties

        for ( var index = 0; index < tokens.Count; index++ )
        {
            tokens[index].Next = index != tokens.Count - 1
                ? tokens[index + 1]
                : JsonPathSegment.Final;
        }

        return tokens.First();
    }

    private static SelectorKind GetSelectorKind( string selector )
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

    private static bool IsIndex( ReadOnlySpan<char> input, out bool isValid, out string reason )
    {
        return IsValidNumber( input, out isValid, out reason );
    }

    private static bool IsQuoted( ReadOnlySpan<char> input )
    {
        return (input.Length > 1 &&
                input[0] == '"' && input[^1] == '"' ||
                input[0] == '\'' && input[^1] == '\'');
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
            if ( !char.IsDigit( input[i] ) )
            {
                isValid = false;
                reason = "Input contains non-digit characters.";
                return false;
            }
        }

        // Try parse to detect overflow
        if ( !long.TryParse( input, out _ ) )
        {
            isValid = false;
            reason = "Input is too large.";
            return false;
        }

        return true; // It's a valid number
    }

    private static void ThrowIfQuoted( string value )
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
            throw new NotSupportedException( "Selector name cannot be null." );

        char quoteChar = name[0];
        if ( name.Length < 2 || (quoteChar != '"' && quoteChar != '\'') || name[^1] != quoteChar )
            throw new NotSupportedException( "Quoted name must start and end with the same quote character, either double or single quote." );

        for ( int i = 1; i < name.Length - 1; i++ )
        {
            if ( name[i] == '\\' )
            {
                // Check if it's a valid escape sequence
                if ( i + 1 >= name.Length - 1 || !IsValidEscapeSequence( name.Slice( i, 2 ), quoteChar ) )
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
                throw new NotSupportedException( $"Control character '\\u{((int) name[i]):x4}' is not allowed in a quoted name." );
            }
        }

        return;

        static bool IsValidEscapeSequence( ReadOnlySpan<char> span, char quoteChar )
        {
            // Valid escape sequences based on the quote character
            return span.Length == 2 && (
                span[1] == quoteChar ||
                span[1] == '\\' ||
                span[1] == '/' || span[1] == 'b' ||
                span[1] == 'f' || span[1] == 'n' ||
                span[1] == 'r' || span[1] == 't' ||
                span[1] == 'u'
            );
        }

        static bool IsValidUnicodeEscapeSequence( ReadOnlySpan<char> span )
        {
            if ( span.Length != 6 || span[1] != 'u' )
                return false;

            for ( int i = 2; i < 6; i++ )
            {
                if ( !Uri.IsHexDigit( span[i] ) )
                    return false;
            }

            return true;
        }
    }

    private static string UnquoteAndUnescape( string value )
    {
        if ( value.Length <= 0 )
            return null;

        value = value.Trim();

        if ( IsQuoted( value ) )
            return Regex.Unescape( value[1..^1] ); // unquote and unescape

        ThrowIfNotValidUnquotedName( value );
        return value;
    }
}
