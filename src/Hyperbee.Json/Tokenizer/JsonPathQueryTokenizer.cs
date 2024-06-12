using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Hyperbee.Json.Tokenizer;
// https://ietf-wg-jsonpath.github.io/draft-ietf-jsonpath-base/draft-ietf-jsonpath-base.html
// https://github.com/ietf-wg-jsonpath/draft-ietf-jsonpath-base

internal enum SelectorKind
{
    Undefined,

    // dot notation
    Root,
    Dot,

    // union notation
    Name,
    Slice,
    Filter,
    Index,

    // 
    Wildcard,
    Descendant,

    // internal reserved for runtime processing
    UnspecifiedSingular, // singular selector (root, name or index)
    UnspecifiedGroup     // non-singular selector
}

public static partial class JsonPathQueryTokenizer
{
    private static readonly ConcurrentDictionary<string, Segment> JsonPathTokens = new();

    [GeneratedRegex( @"^(-?[0-9]*):?(-?[0-9]*):?(-?[0-9]*)$" )]
    private static partial Regex RegexSlice();

    [GeneratedRegex( @"^\??\((.*?)\)$" )]
    private static partial Regex RegexFilter();

    [GeneratedRegex( @"^[0-9*]+$" )]
    private static partial Regex RegexNumber();

    [GeneratedRegex( @"^""(?:[^""\\]|\\.)*""$" )]
    private static partial Regex RegexQuotedDouble();

    [GeneratedRegex( @"^'(?:[^'\\]|\\.)*'$" )]
    private static partial Regex RegexQuoted();

    private enum Scanner
    {
        Start,
        DotChild,
        UnionStart,
        UnionElementQuoted,
        UnionElementQuotedFinal,
        UnionElement,
        UnionNextElement,
        UnionFinal,
        Final
    }

    private static string GetSelector( Scanner scanner, ReadOnlySpan<char> buffer, int start, int stop )
    {
        var adjust = scanner == Scanner.Final ? 0 : 1; // non-final states have already advanced to the next character, so we need to subtract 1
        var length = stop - start - adjust;
        return length <= 0 ? null : buffer.Slice( start, length ).Trim().ToString();
    }

    private static void InsertToken( ICollection<Segment> tokens, SelectorDescriptor selector )
    {
        if ( selector?.Value == null )
            return;

        InsertToken( tokens, [selector] );
    }

    private static void InsertToken( ICollection<Segment> tokens, SelectorDescriptor[] selectors )
    {
        if ( selectors == null || selectors.Length == 0 )
            return;

        tokens.Add( new Segment( null, selectors ) );
    }

    internal static Segment Tokenize( string query )
    {
        return JsonPathTokens.GetOrAdd( query, x => TokenFactory( x.AsSpan() ) );
    }

    internal static Segment TokenizeNoCache( ReadOnlySpan<char> query )
    {
        return TokenFactory( query );
    }

    private static Segment TokenFactory( ReadOnlySpan<char> query )
    {
        // transform jsonpath patterns like "$.store.book[*]..author" to an array of tokens [ $, store, book, *, .., author ]

        var tokens = new List<Segment>();

        var i = 0;
        var n = query.Length;

        var selectorStart = 0;

        var bracketDepth = 0;
        var parenDepth = 0;
        var literalDelimiter = '\'';
        var selectors = new List<SelectorDescriptor>();

        var scanner = Scanner.Start;

        do
        {
            var c = query[i++];

            SelectorKind selectorKind;
            string selectorValue;

            switch ( scanner )
            {
                case Scanner.Start:
                    switch ( c )
                    {
                        case ' ':
                        case '\t':
                            break;
                        case '@':  // Technically invalid, but allows `@` to work on sub queries without changing tokenizer 
                        case '$':
                            if ( i < n && query[i] != '.' && query[i] != '[' )
                                throw new NotSupportedException( "Invalid character after `$`." );
                            scanner = Scanner.DotChild;
                            break;
                        default:
                            throw new NotSupportedException( "`$` expected." );
                    }

                    break;

                case Scanner.DotChild:
                    switch ( c )
                    {
                        case '[':
                            scanner = Scanner.UnionStart;

                            selectorValue = GetSelector( scanner, query, selectorStart, i );
                            selectorKind = selectorValue switch
                            {
                                "$" when tokens.Count != 0 => throw new NotSupportedException( $"Invalid use of root `$` at pos {i - 1}." ),
                                "$" => SelectorKind.Root,
                                "*" => SelectorKind.Wildcard,
                                _ => SelectorKind.Dot
                            };

                            InsertToken( tokens, new SelectorDescriptor
                            {
                                SelectorKind = selectorKind,
                                Value = selectorValue
                            } );

                            break;
                        case '.':

                            if ( i == n )
                                throw new NotSupportedException( $"Missing character after `.` at pos {i - 1}." );

                            selectorValue = GetSelector( scanner, query, selectorStart, i );
                            selectorKind = selectorValue switch
                            {
                                "$" when tokens.Count != 0 => throw new NotSupportedException( $"Invalid use of root `$` at pos {i - 1}." ),
                                "$" => SelectorKind.Root,
                                "*" => SelectorKind.Wildcard,
                                _ => SelectorKind.Dot
                            };

                            InsertToken( tokens, new SelectorDescriptor
                            {
                                SelectorKind = selectorKind,
                                Value = selectorValue
                            } );

                            if ( i <= n && query[i] == '.' )
                            {
                                InsertToken( tokens, new SelectorDescriptor
                                {
                                    SelectorKind = SelectorKind.Descendant,
                                    Value = ".."
                                } );

                                i++;
                            }

                            selectorStart = i;
                            break;
                        case ' ':
                        case '\t':
                            throw new NotSupportedException( $"Invalid whitespace in object notation at pos {i - 1}." );
                    }

                    break;

                case Scanner.UnionStart:
                    switch ( c )
                    {
                        case ' ':
                        case '\t':
                            break;
                        case '*':
                            scanner = Scanner.UnionFinal;
                            InsertToken( tokens, new SelectorDescriptor
                            {
                                SelectorKind = SelectorKind.Wildcard,
                                Value = "*"
                            } );
                            break;
                        case '.':
                            if ( i > n || query[i] != '.' )
                                throw new NotSupportedException( $"Invalid `.` in bracket expression at pos {i - 1}." );

                            scanner = Scanner.UnionFinal;
                            InsertToken( tokens, new SelectorDescriptor
                            {
                                SelectorKind = SelectorKind.Descendant,
                                Value = ".."
                            } );
                            i++;
                            break;
                        case '\'':
                        case '"':
                            scanner = Scanner.UnionElementQuoted;
                            literalDelimiter = c;
                            selectorStart = i - 1;
                            bracketDepth = 1;
                            break;
                        default:
                            scanner = Scanner.UnionElement;
                            i--; // replay character
                            selectorStart = i;
                            bracketDepth = 1;
                            break;
                    }

                    break;

                case Scanner.UnionElementQuoted:
                    if ( c == '\\' ) // handle escaping
                    {
                        i++; // advance past the escaped character
                    }
                    else if ( c == literalDelimiter )
                    {
                        scanner = Scanner.UnionElementQuotedFinal;
                    }

                    break;

                case Scanner.UnionElementQuotedFinal:
                    switch ( c )
                    {
                        case ' ':
                        case '\t':
                            break;
                        case ']':
                        case ',':
                            scanner = Scanner.UnionElement;
                            i--; // replay character
                            break;
                        default: // invalid characters after end of string
                            throw new NotSupportedException( $"Invalid bracket literal at pos {i - 1}." );
                    }

                    break;

                case Scanner.UnionElement:
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

                            selectorValue = GetSelector( scanner, query, selectorStart, i );
                            selectorStart = i;

                            // validate the extracted atom value shape

                            if ( string.IsNullOrEmpty( selectorValue ) ) // [] is not valid
                                throw new NotSupportedException( "Invalid bracket expression syntax. Bracket expression cannot be empty." );

                            selectorKind = GetElementSelectorKind( selectorValue );

                            if ( selectorKind == SelectorKind.Undefined )
                                throw new NotSupportedException( $"Invalid bracket expression syntax. Unrecognized selector format at pos {i - 1}." );

                            if ( selectorKind == SelectorKind.Name )
                            {
                                selectorValue = selectorValue[1..^1]; // remove surrounding quotes
                                selectorValue = Regex.Unescape( selectorValue ); // unescape selector
                            }

                            selectors.Insert( 0, new SelectorDescriptor
                            {
                                SelectorKind = selectorKind,
                                Value = selectorValue
                            } );

                            // continue parsing the union

                            switch ( c )
                            {
                                case ',':
                                    scanner = Scanner.UnionNextElement;
                                    break;
                                case ']':
                                    scanner = Scanner.DotChild;
                                    InsertToken( tokens, [.. selectors] );
                                    selectors.Clear();
                                    break;
                            }

                            break;
                    }

                    break;

                case Scanner.UnionNextElement:
                case Scanner.UnionFinal:
                    switch ( c )
                    {
                        case ' ':
                        case '\t':
                            break;
                        case ']':
                            scanner = Scanner.DotChild;
                            selectorStart = i;
                            break;
                        case '\'':
                        case '"':
                            if ( scanner != Scanner.UnionNextElement )
                                throw new NotSupportedException( $"Invalid bracket syntax at pos {i - 1}." );

                            scanner = Scanner.UnionElementQuoted;
                            literalDelimiter = c;
                            selectorStart = i - 1;
                            break;
                        default:
                            if ( scanner != Scanner.UnionNextElement )
                                throw new NotSupportedException( $"Invalid bracket syntax at pos {i - 1}." );

                            scanner = Scanner.UnionElement;
                            i--; // replay character
                            selectorStart = i;

                            break;
                    }

                    break;

                default:
                    throw new InvalidOperationException();
            }
        } while ( i < n );

        // handle the trailing bits
        scanner = Scanner.Final;

        var finalSelector = GetSelector( scanner, query, selectorStart, i );

        if ( finalSelector != null )
        {
            var finalKind = finalSelector switch
            {
                "*" => SelectorKind.Wildcard,
                ".." => SelectorKind.Descendant,
                _ => SelectorKind.Dot
            };

            InsertToken( tokens, new SelectorDescriptor
            {
                SelectorKind = finalKind,
                Value = finalSelector
            } );
        }

        // finished
        //return ImmutableStack.Create( ((IEnumerable<JsonPathSegments>) tokens).Reverse().ToArray() );

        for ( var index = 0; index < tokens.Count; index++ )
        {
            tokens[index].Next = index == tokens.Count - 1
                ? Segment.TerminalSegment
                : tokens[index + 1];
        }

        return tokens.First();
    }

    private static SelectorKind GetElementSelectorKind( string selector )
    {
        if ( RegexFilter().IsMatch( selector ) )
            return SelectorKind.Filter;

        if ( RegexNumber().IsMatch( selector ) )
            return SelectorKind.Index;

        if ( RegexSlice().IsMatch( selector ) )
            return SelectorKind.Slice;

        if ( RegexQuotedDouble().IsMatch( selector ) || RegexQuoted().IsMatch( selector ) )
            return SelectorKind.Name;

        return selector switch
        {
            "*" => SelectorKind.Wildcard,
            ".." => SelectorKind.Descendant,
            _ => SelectorKind.Undefined
        };
    }
}
