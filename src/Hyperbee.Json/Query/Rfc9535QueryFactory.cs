﻿using System.Runtime.CompilerServices;
using Hyperbee.Json.Core;

namespace Hyperbee.Json.Query;

internal static class Rfc9535QueryFactory
{
    private enum State
    {
        Undefined,
        Whitespace,
        Start,
        DotChild,
        UnionItem,
        UnionNext,
        Finish,
        Final
    }

    internal static JsonQuery Parse( ReadOnlySpan<char> query, JsonQueryParserOptions options )
    {
        bool allowDotWhitespace = options == JsonQueryParserOptions.Rfc9535AllowDotWhitespace;

        // RFC - query cannot start or end with whitespace
        if ( !query.IsEmpty && (char.IsWhiteSpace( query[0] ) || char.IsWhiteSpace( query[^1] )) )
            throw new NotSupportedException( "Query cannot start or end with whitespace." );

        var i = 0;
        var n = query.Length;

        var selectorStart = 0;

        var inQuotes = false;
        var inFilter = false;
        var quoteChar = '\'';
        var escaped = false;
        var bracketDepth = 0;
        var parenDepth = 0;

        Span<char> whitespaceTerminators = ['\0', '\0']; // '\0' is used as a sentinel value
        var whiteSpaceReplay = true;

        var segments = new List<JsonSegment>();
        var selectors = new List<SelectorDescriptor>();

        var state = State.Start;
        var returnState = State.Undefined;

        do
        {
            // Read next character
            var c = i < n ? query[i++] : '\0';

            if ( state != State.Whitespace && c == '\0' ) // whitespace is a sub-state, allow it to exit
                state = State.Finish; // end of input

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

                            InsertSegment( segments, new SelectorDescriptor { SelectorKind = SelectorKind.Root, Value = c.ToString() } );

                            whitespaceTerminators[0] = '.';
                            whitespaceTerminators[1] = '[';
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

                            if ( c != '\0' && whitespaceTerminators[0] != '\0' && !whitespaceTerminators.Contains( c ) )
                                throw new NotSupportedException( $"Invalid character `{c}` at pos {i - 1}." );

                            whitespaceTerminators[0] = '\0'; // reset
                            state = returnState; // transition back to the appropriate state
                            selectorStart = i; // start of the next selector

                            if ( whiteSpaceReplay )
                                i--; // replay character

                            whiteSpaceReplay = true;

                            break;
                    }

                    break;

                case State.DotChild:
                    switch ( c )
                    {
                        case '[': // end-of-child
                        case '.': // end-of-child

                            if ( i == n && c == '.' ) // dot( . ) is not allowed at the end of the query
                                throw new NotSupportedException( $"Missing character after `.` at pos {i - 1}." );

                            selectorSpan = GetSelectorSpan( state, query, selectorStart, i );
                            selectorKind = selectorSpan switch
                            {
                                "$" => throw new NotSupportedException( $"Invalid use of root `$` at pos {i - 1}." ),
                                "@" => throw new NotSupportedException( $"Invalid use of local root `$` at pos {i - 1}." ),
                                "*" => SelectorKind.Wildcard,
                                _ => SelectorKind.Name
                            };

                            if ( selectorKind == SelectorKind.Name && !selectorSpan.IsEmpty ) // can be null after a union
                            {
                                ThrowIfQuoted( selectorSpan );
                                ThrowIfInvalidUnquotedName( selectorSpan );
                            }

                            InsertSegment( segments, GetSelectorDescriptor( selectorKind, selectorSpan ) );

                            // continue parsing next child
                            switch ( c )
                            {
                                case '.': // continue dot child
                                    if ( i < n && query[i] == '.' ) // peek next character for `..`
                                    {
                                        InsertSegment( segments, GetSelectorDescriptor( SelectorKind.Descendant, ".." ) );
                                        i++; // advance past second `.`
                                    }

                                    selectorStart = i;
                                    break;

                                case '[': // transition to union
                                    state = State.Whitespace;
                                    whiteSpaceReplay = false;
                                    returnState = State.UnionItem;
                                    bracketDepth = 1;
                                    i--; // replay character
                                    break;
                            }

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

                case State.UnionItem:

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
                            selectorKind = GetUnionSelectorKind( selectorSpan );

                            // create the selector descriptor
                            SelectorDescriptor descriptor;

                            switch ( selectorKind )
                            {
                                case SelectorKind.Undefined:
                                    throw new NotSupportedException( $"Invalid bracket expression syntax. Unrecognized selector format at pos {i - 1}." );

                                case SelectorKind.Name:
                                    ThrowIfInvalidQuotedName( selectorSpan );

                                    if ( escaped )
                                    {
                                        descriptor = GetUnescapedSelectorDescriptor( selectorKind, selectorSpan, nullable: false, SpanUnescapeOptions.SingleThenUnquote ); // unescape and then unquote
                                        escaped = false;
                                    }
                                    else
                                    {
                                        descriptor = GetSelectorDescriptor( selectorKind, selectorSpan[1..^1], nullable: false ); // unquote
                                    }

                                    break;

                                case SelectorKind.Filter:

                                    if ( escaped )
                                    {
                                        descriptor = GetUnescapedSelectorDescriptor( selectorKind, selectorSpan, nullable: true, SpanUnescapeOptions.Multiple ); // unescape one or more strings
                                        escaped = false;
                                    }
                                    else
                                    {
                                        descriptor = GetSelectorDescriptor( selectorKind, selectorSpan );
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
                                    state = State.Whitespace;
                                    returnState = State.UnionNext;
                                    break;
                                case ']':
                                    InsertSegment( segments, [.. selectors] );
                                    selectors.Clear();

                                    whitespaceTerminators[0] = '.';
                                    whitespaceTerminators[1] = '[';
                                    state = State.Whitespace;
                                    returnState = State.DotChild;
                                    break;
                            }

                            break;

                        case '?':
                            if ( !inQuotes )
                                inFilter = true;
                            break;

                        case '.': // descent in brackets is illegal except within a filter expr
                            if ( i < n && query[i] == '.' && !inFilter )
                                throw new NotSupportedException( $"Invalid `..` in bracket expression at pos {i - 1}." );
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
                            state = State.UnionItem;
                            quoteChar = c;
                            selectorStart = i - 1; // capture the quote character
                            inQuotes = true;
                            inFilter = false;
                            break;
                        default:
                            state = State.UnionItem;
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
                            ThrowIfInvalidUnquotedName( selectorSpan );
                        }

                        InsertSegment( segments, GetSelectorDescriptor( finalKind, selectorSpan ) );
                    }

                    state = State.Final;
                    break;

                default:
                    throw new InvalidOperationException();
            }
        } while ( state != State.Final );

        return JsonSegment.LinkSegments( query, segments );
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static SelectorDescriptor GetSelectorDescriptor( SelectorKind selectorKind, ReadOnlySpan<char> selectorSpan, bool nullable = true )
    {
        var selectorValue = selectorSpan.IsEmpty && nullable ? null : selectorSpan.ToString();
        return new SelectorDescriptor { SelectorKind = selectorKind, Value = selectorValue };
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static SelectorDescriptor GetSelectorDescriptor( SelectorKind selectorKind, in ValueStringBuilder builder, bool nullable = true )
    {
        var selectorValue = builder.IsEmpty && !nullable ? null : builder.ToString();
        return new SelectorDescriptor { SelectorKind = selectorKind, Value = selectorValue };
    }

    private static SelectorDescriptor GetUnescapedSelectorDescriptor( SelectorKind selectorKind, ReadOnlySpan<char> selectorSpan, bool nullable, SpanUnescapeOptions unescapeOptions )
    {
        // SpanBuilder must be disposed, but it is a ref struct, so we can't use `using`

        var builder = new ValueStringBuilder( stackalloc char[512] );

        try
        {
            SpanHelper.Unescape( selectorSpan, ref builder, unescapeOptions ); // unescape and then unquote
            return GetSelectorDescriptor( selectorKind, builder, nullable );
        }
        finally // ensure builder is disposed
        {
            builder.Dispose();
        }
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static ReadOnlySpan<char> GetSelectorSpan( State state, ReadOnlySpan<char> buffer, int start, int stop )
    {
        var adjust = state == State.Finish || state == State.Final ? 0 : 1; // non-final states have already advanced to the next character, so we need to subtract 1
        var length = stop - start - adjust;
        return length <= 0 ? [] : buffer.Slice( start, length ).Trim();
    }

    private static SelectorKind GetUnionSelectorKind( ReadOnlySpan<char> selector )
    {
        // selector order matters

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
    private static void InsertSegment( List<JsonSegment> segments, params SelectorDescriptor[] selectors )
    {
        if ( selectors == null || selectors.Length == 0 || selectors.Length == 1 && selectors[0]?.Value == null )
            return; // ignore null and empty selectors. this is valid in some cases like `].` and `..`

        segments.Add( new JsonSegment( selectors ) );
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static bool IsFilter( ReadOnlySpan<char> input )
    {
        // Check if the input starts with '?' and is at least two characters long
        return input.Length > 1 && input[0] == '?';
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
            var length = span.Length;

            if ( idx < length && span[idx] == '-' )
                idx++;

            while ( idx < length && char.IsDigit( span[idx] ) )
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
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static void SkipWhitespace( ReadOnlySpan<char> span, ref int idx )
        {
            var length = span.Length;
            while ( idx < length && char.IsWhiteSpace( span[idx] ) )
                idx++;
        }
    }

    private static bool IsValidNumber( ReadOnlySpan<char> input, out bool isValid, out string reason )
    {
        isValid = true;
        reason = string.Empty;

        var length = input.Length;

        if ( length == 0 )
        {
            isValid = false;
            reason = "Input is empty.";
            return false;
        }

        var start = 0;

        // Handle optional leading negative sign
        if ( input[0] == '-' )
        {
            start = 1;
            if ( length == 1 )
            {
                isValid = false;
                reason = "Invalid negative number.";
                return false;
            }
        }

        // Check for leading zeros
        if ( input[start] == '0' && length > start + 1 )
        {
            isValid = false;
            reason = "Leading zeros are not allowed.";
            return false;
        }

        // Check if all remaining characters are digits
        for ( var i = start; i < length; i++ )
        {
            var c = input[i];

            if ( c >= '0' && c <= '9' )
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

    private static void ThrowIfInvalidUnquotedName( ReadOnlySpan<char> name )
    {
        if ( name.IsEmpty )
            throw new NotSupportedException( "Selector name cannot be null." );

        // Validate the first character
        if ( !IsValidFirstChar( name[0] ) )
            throw new NotSupportedException( $"Selector name cannot start with `{name[0]}`." );

        // Validate subsequent characters
        for ( var i = 1; i < name.Length; i++ )
        {
            if ( !IsValidSubsequentChar( name[i] ) )
                throw new NotSupportedException( $"Selector name cannot contain `{name[i]}`." );
        }

        return;

        static bool IsValidFirstChar( char c ) => char.IsLetter( c ) || c == '_' || c >= 0x80;
        static bool IsValidSubsequentChar( char c ) => char.IsLetterOrDigit( c ) || c == '_' || c == '-' || c >= 0x80;
    }

    private static void ThrowIfInvalidQuotedName( ReadOnlySpan<char> name )
    {
        if ( name.IsEmpty )
            throw new NotSupportedException( "Selector name cannot be empty." );

        var quoteChar = name[0];
        if ( name.Length < 2 || quoteChar != '"' && quoteChar != '\'' || name[^1] != quoteChar )
            throw new NotSupportedException( "Quoted name must start and end with the same quote character, either double or single quote." );

        for ( var i = 1; i < name.Length - 1; i++ )
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

            for ( var i = 2; i < 6; i++ )
            {
                if ( !char.IsAsciiHexDigit( span[i] ) )
                    return false;
            }

            return true;
        }
    }
}
