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
                state = State.FinalSelector;
                c = '\0'; // Add null terminator to signal end of input
            }

            // process character

            SelectorKind selectorKind;
            string selectorValue;

            switch ( state )
            {
                case State.Start:
                    switch ( c )
                    {
                        case ' ':
                        case '\t':
                            break;
                        case '@': // Technically invalid, but allows `@` to work on sub queries without changing tokenizer 
                        case '$':
                            if ( i < n && query[i] != '.' && query[i] != '[' )
                                throw new NotSupportedException( "Invalid character after `$`." );

                            if ( query[^1] == '.' && query[^2] == '.' )
                                throw new NotSupportedException( "`..` cannot be the last segment." );

                            state = State.DotChild;
                            break;
                        default:
                            throw new NotSupportedException( "`$` expected." );
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

                case State.DotChild:
                    switch ( c )
                    {
                        case '[':
                            state = State.UnionStart;

                            selectorValue = GetSelector( state, query, selectorStart, i );
                            selectorKind = selectorValue switch
                            {
                                "$" when tokens.Count != 0 => throw new NotSupportedException( $"Invalid use of root `$` at pos {i - 1}." ),
                                "$" => SelectorKind.Root,
                                "@" when tokens.Count != 0 => throw new NotSupportedException( $"Invalid use of local root `$` at pos {i - 1}." ),
                                "@" => SelectorKind.Root,
                                "*" => SelectorKind.Wildcard,
                                _ => SelectorKind.DotName
                            };

                            if ( selectorKind == SelectorKind.DotName && selectorValue != null )
                            {
                                ThrowIfQuoted( selectorValue );
                                ThrowIfNotValidUnquotedName( selectorValue );
                            }

                            InsertToken( tokens, new SelectorDescriptor { SelectorKind = selectorKind, Value = selectorValue } );

                            break;
                        case '.':
                            if ( i == n )
                                throw new NotSupportedException( $"Missing character after `.` at pos {i - 1}." );

                            selectorValue = GetSelector( state, query, selectorStart, i );
                            selectorKind = selectorValue switch
                            {
                                "$" when tokens.Count != 0 => throw new NotSupportedException( $"Invalid use of root `$` at pos {i - 1}." ),
                                "$" => SelectorKind.Root,
                                "@" when tokens.Count != 0 => throw new NotSupportedException( $"Invalid use of local root `$` at pos {i - 1}." ),
                                "@" => SelectorKind.Root,
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
                            throw new NotSupportedException( $"Invalid whitespace in object notation at pos {i - 1}." );
                        case '\0':
                            state = State.FinalSelector;
                            i--; // step back to process the last character
                            break;
                    }

                    break;

                case State.UnionStart:
                    switch ( c )
                    {
                        case ' ':
                        case '\t':
                            break;
                        case '*':
                            state = State.UnionFinal;
                            InsertToken( tokens, new SelectorDescriptor { SelectorKind = SelectorKind.Wildcard, Value = "*" } );
                            break;
                        case '.':
                            if ( i > n || query[i] != '.' )
                                throw new NotSupportedException( $"Invalid `.` in bracket expression at pos {i - 1}." );

                            state = State.UnionFinal;
                            InsertToken( tokens, new SelectorDescriptor { SelectorKind = SelectorKind.Descendant, Value = ".." } );
                            i++;
                            break;
                        case '\'':
                        case '"':
                            returnState = State.UnionQuotedFinal;
                            state = State.QuotedName;
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

                            selectorValue = selectorKind switch
                            {
                                SelectorKind.Undefined => throw new NotSupportedException( $"Invalid bracket expression syntax. Unrecognized selector format at pos {i - 1}." ),
                                SelectorKind.Name => UnquoteAndUnescape( selectorValue ),
                                _ => selectorValue
                            };

                            selectors.Insert( 0, new SelectorDescriptor { SelectorKind = selectorKind, Value = selectorValue } );

                            // continue parsing the union

                            switch ( c )
                            {
                                case ',':
                                    state = State.UnionNext;
                                    break;
                                case ']':
                                    if ( i < n && query[i] != '.' && query[i] != '[' )
                                        throw new NotSupportedException( $"Invalid character after `]` at pos {i - 1}." );
                                    state = State.DotChild;
                                    InsertToken( tokens, selectors.ToArray() );
                                    selectors.Clear();
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
        if ( IsQuoted( selector ) )
            return SelectorKind.Name;

        if ( IsIndex( selector ) )
            return SelectorKind.Index;

        if ( IsFilter( selector ) )
            return SelectorKind.Filter;

        if ( IsSlice( selector ) )
            return SelectorKind.Slice;

        return selector switch
        {
            "*" => SelectorKind.Wildcard,
            ".." => SelectorKind.Descendant,
            _ => SelectorKind.Undefined
        };
    }

    private static bool IsSlice( ReadOnlySpan<char> input )
    {
        var index = 0;

        // First part (optional number)
        if ( !IsOptionalNumber( input, ref index ) )
            return false;

        // Optional colon
        if ( index < input.Length && input[index] == ':' )
        {
            index++;

            // Second part (optional number)
            if ( !IsOptionalNumber( input, ref index ) )
                return false;

            // Optional second colon
            if ( index < input.Length && input[index] == ':' )
            {
                index++;

                // Third part (optional number)
                if ( !IsOptionalNumber( input, ref index ) )
                    return false;
            }
        }

        var result = index == input.Length;
        return result;

        static bool IsOptionalNumber( ReadOnlySpan<char> span, ref int idx )
        {
            var start = idx;

            if ( idx < span.Length && (span[idx] == '-' || span[idx] == '+') )
                idx++;

            while ( idx < span.Length && char.IsDigit( span[idx] ) )
                idx++;

            var isValid = idx > start || start == idx;
            return isValid; // True if there was a number or just an optional sign
        }
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

    private static bool IsIndex( ReadOnlySpan<char> input )
    {
        foreach ( var ch in input )
        {
            if ( !char.IsDigit( ch ) )
                return false;
        }

        return true;
    }

    private static bool IsQuoted( ReadOnlySpan<char> input )
    {
        return (input.Length > 1 &&
                input[0] == '"' && input[^1] == '"' ||
                input[0] == '\'' && input[^1] == '\'');
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
