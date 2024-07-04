using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Extensions;

// DISTINCT from JsonPath these extensions are intended to facilitate 'diving' for Json Properties using
// normalized paths. a normalized path is an absolute path that references a single element.
// similar to JsonPointer but using JsonPath notation.
//
// syntax supports absolute paths; dotted notation, quoted names, and simple bracketed array accessors only.
//
// Json path style wildcard '*', '..', and '[a,b]' multi-result selector notations are NOT supported.
//
// examples:
//  $.prop1.prop2
//  $.prop1[0]
//  $.prop1[0].prop2
//  $.prop1['prop.2']
//
//  also supports quoted member-name for dot child
//
//  $.'prop.2'
//  $.prop1.'prop.2'[0].prop3

public static class JsonPathPointerExtensions
{
    public static JsonElement FromJsonPathPointer( this JsonElement jsonElement, ReadOnlySpan<char> pointer )
    {
        if ( IsNullOrUndefined( jsonElement ) || pointer.IsEmpty )
            return default;

        var splitter = new JsonPathPointerSplitter( pointer );

        while ( splitter.TryMoveNext( out var name ) )
        {
            if ( jsonElement.ValueKind == JsonValueKind.Array && int.TryParse( name, out var index ) )
            {
                jsonElement = jsonElement.EnumerateArray().ElementAtOrDefault( index );
                continue;
            }

            jsonElement = jsonElement.TryGetProperty( name!, out var value ) ? value : default;

            if ( IsNullOrUndefined( jsonElement ) )
                return default;
        }

        return jsonElement;

        static bool IsNullOrUndefined( JsonElement value ) => value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined;
    }

    public static JsonNode FromJsonPathPointer( this JsonNode jsonNode, ReadOnlySpan<char> pointer )
    {
        if ( jsonNode == null || pointer.IsEmpty )
            return default;

        var splitter = new JsonPathPointerSplitter( pointer );

        while ( splitter.TryMoveNext( out var name ) )
        {
            if ( jsonNode is JsonArray valueArray && int.TryParse( name, out var index ) )
            {
                jsonNode = valueArray[index];

                if ( jsonNode == null )
                    return default;

                continue;
            }

            jsonNode = jsonNode.AsObject().TryGetPropertyValue( name!.ToString(), out var value ) ? value : default;

            if ( jsonNode == null )
                return default;
        }

        return jsonNode;
    }

    private ref struct JsonPathPointerSplitter  //TODO Support escaping of \' and bracket counting in literals. Add to unit tests.
    {
        // zero allocation helper that splits a json path in to parts

        // this splitter only works on simple property 'keys' it does not work
        // with complex selectors ( '..', '*', '[a,b,c]' ).

        private ReadOnlySpan<char> _span;
        private Scanner _scanner;

        private enum Scanner
        {
            Default,
            Quoted,
            Bracket,
            Trailing
        }

        private enum SpanAction
        {
            ReadNext,
            TruncateLeadingCharacter,
            YieldIdentifier
        }

        private enum BracketContent
        {
            Undefined,
            Quoted,
            Number
        }

        internal JsonPathPointerSplitter( ReadOnlySpan<char> span )
        {
            if ( !span.StartsWith( "$" ) )
                throw new NotSupportedException( "Path must start with `$`." );

            span = span.StartsWith( "$." ) ? span[2..] : span[1..]; // eat the leading $

            _span = span;
            _scanner = Scanner.Default;
        }

        private void TakeIdentifier( int i, out ReadOnlySpan<char> identifier )
        {
            identifier = i > 0 ? _span[..i].Trim( '\'' ) : default;
            _span = _span[Math.Min( i + 1, _span.Length )..];
        }

        // ReSharper disable once RedundantAssignment
        private void TakeLeadingCharacter( ref int i )
        {
            _span = _span[1..];
            i = 0;
        }

        public bool TryMoveNext( out ReadOnlySpan<char> identifier )
        {
            identifier = default;
            var i = 0;

            var bracketContent = BracketContent.Undefined;

            do
            {
                if ( _span.IsEmpty || i >= _span.Length )
                    return false;

                var c = _span[i];
                var action = SpanAction.ReadNext;

                switch ( _scanner )
                {
                    case Scanner.Default:
                        switch ( c )
                        {
                            case '\'':
                                _scanner = Scanner.Quoted;
                                break;
                            case '[':
                                _scanner = Scanner.Bracket;
                                action = SpanAction.YieldIdentifier;
                                break;
                            case '.':
                                action = SpanAction.YieldIdentifier;
                                break;
                            case ' ':
                            case '\t':
                            case ']':
                            case '$' when i > 0:
                                throw new JsonException( $"Invalid character '{c}' at pos {i}." );
                            default:
                                if ( i + 1 == _span.Length ) // take if at the end
                                {
                                    i++; // capture the final character
                                    action = SpanAction.YieldIdentifier;
                                }

                                break;
                        }

                        break;
                    case Scanner.Quoted:
                        switch ( c )
                        {
                            case '\'':
                                _scanner = Scanner.Trailing;
                                action = SpanAction.YieldIdentifier;
                                break;
                        }

                        break;
                    case Scanner.Bracket:
                        switch ( c )
                        {
                            case ']':
                                _scanner = Scanner.Trailing;
                                action = SpanAction.YieldIdentifier;
                                break;
                            case var _ when bracketContent == BracketContent.Undefined:
                                if ( c == '\'' )
                                    bracketContent = BracketContent.Quoted;
                                else if ( char.IsNumber( c ) )
                                    bracketContent = BracketContent.Number;
                                else
                                    throw new JsonException( $"Invalid character '{c}' in bracket at pos {i}." );
                                break;
                            case var _ when bracketContent == BracketContent.Number && !char.IsNumber( c ):
                                throw new JsonException( $"Invalid non-numeric {c}' in bracket at pos {i}." );
                        }

                        break;
                    case Scanner.Trailing:
                        switch ( c )
                        {
                            case '[':
                                _scanner = Scanner.Bracket;
                                break;
                            case '.':
                                _scanner = Scanner.Default;
                                break;
                            default:
                                throw new JsonException( $"Invalid character '{c}' after identifier at pos {i}." );
                        }

                        action = SpanAction.TruncateLeadingCharacter;
                        break;
                }

                switch ( action )
                {
                    case SpanAction.ReadNext:
                        i++;
                        break;
                    case SpanAction.TruncateLeadingCharacter:
                        TakeLeadingCharacter( ref i );
                        break;
                    case SpanAction.YieldIdentifier:
                        TakeIdentifier( i, out identifier );
                        break;
                }

            } while ( identifier.IsEmpty );

            return true;
        }
    }
}
