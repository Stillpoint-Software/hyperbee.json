#region License
// C# Implementation of JSONPath[1]
//
// [1] http://goessner.net/articles/JsonPath/
// [2] https://github.com/atifaziz/JSONPath
//
// The MIT License
//
// Copyright (c) 2019 Brenton Farmer. All rights reserved.
// Portions Copyright (c) 2007 Atif Aziz. All rights reserved.
// Portions Copyright (c) 2007 Stefan Goessner (goessner.net)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
#endregion

using System.Collections.Immutable;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Hyperbee.Json.Evaluators;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Memory;
using Hyperbee.Json.Tokenizer;

namespace Hyperbee.Json;
// https://ietf-wg-jsonpath.github.io/draft-ietf-jsonpath-base/draft-ietf-jsonpath-base.html
// https://github.com/ietf-wg-jsonpath/draft-ietf-jsonpath-base

public sealed partial class JsonPath
{
    public static IJsonPathScriptEvaluator<JsonElement> DefaultEvaluator { get; set; } = new JsonPathCSharpElementEvaluator();
    private readonly IJsonPathScriptEvaluator<JsonElement> _evaluator;

    // generated regex

    [GeneratedRegex( "^(-?[0-9]*):?(-?[0-9]*):?(-?[0-9]*)$" )]
    private static partial Regex RegexSlice();

    [GeneratedRegex( @"^\?\((.*?)\)$" )]
    private static partial Regex RegexPathFilter();

    [GeneratedRegex( @"^[0-9*]+$" )]
    private static partial Regex RegexNumber();

    // property names can be simple (@.property) if they contain no SpecialCharacters,
    // otherwise they require bracket notation (@['property']).

    private static readonly char[] SpecialCharacters = ['.', ' ', '\'', '/', '"', '[', ']', '(', ')', '\t', '\n', '\r', '\f', '\b', '\\', '\u0085', '\u2028', '\u2029'];
    private static string GetPath( string prefix, int childKey ) => $"{prefix}[{childKey}]";

    private static string GetPath( string prefix, string childKey, JsonValueKind tokenKind )
    {
        if ( tokenKind == JsonValueKind.Array )
            return $"{prefix}[{childKey}]";

        return childKey.IndexOfAny( SpecialCharacters ) == -1 ? $"{prefix}.{childKey}" : $@"{prefix}['{childKey}']";
    }

    // ctor

    public JsonPath()
        : this( null )
    {
    }

    public JsonPath( IJsonPathScriptEvaluator<JsonElement> evaluator )
    {
        _evaluator = evaluator ?? DefaultEvaluator ?? new JsonPathCSharpElementEvaluator();
    }

    public IEnumerable<JsonElement> Select( in JsonElement value, string query )
    {
        return SelectPath( value, query ).Select( x => x.Value );
    }

    public IEnumerable<JsonPathElement> SelectPath( in JsonElement value, string query )
    {
        if ( string.IsNullOrWhiteSpace( query ) )
            throw new ArgumentNullException( nameof( query ) );

        // quick out

        if ( query == "$" )
            return new[] { new JsonPathElement( value, query ) };

        // tokenize

        var tokens = JsonPathQueryTokenizer.Tokenize( query );

        // initiate the expression walk

        if ( !tokens.IsEmpty && tokens.Peek().Selectors.First().Value == "$" )
            tokens = tokens.Pop();

        return ExpressionVisitor( new VisitorArgs( value, tokens, "$" ), _evaluator.Evaluator );
    }

    private static IEnumerable<JsonPathElement> ExpressionVisitor( VisitorArgs args, JsonPathEvaluator<JsonElement> evaluator )
    {
        var nodes = new Stack<VisitorArgs>( 4 );
        void PushNode( in JsonElement v, in IImmutableStack<JsonPathToken> t, string p ) => nodes.Push( new VisitorArgs( v, t, p ) );

        do
        {
            // deconstruct the next args node

            var (current, tokens, path) = args;

            if ( tokens.IsEmpty )
            {
                if ( !string.IsNullOrEmpty( path ) )
                    yield return new JsonPathElement( current, path );

                continue;
            }

            // pop the next token from the stack

            tokens = tokens.Pop( out var token );
            var selector = token.Selectors.First().Value;

            // make sure we have a container value

            if ( !current.IsObjectOrArray() )
                throw new InvalidOperationException( "Object or Array expected." );

            // try to access object or array using KEY value

            if ( token.Singular )
            {
                if ( TryGetChildValue( current, selector, out var childValue ) )
                    PushNode( childValue, tokens, GetPath( path, selector, current.ValueKind ) );

                continue;
            }

            // wildcard

            if ( selector == "*" )
            {
                foreach ( var childKey in EnumerateKeys( current ) )
                    PushNode( current, tokens.Push( childKey, SelectorKind.UnspecifiedSingular ), path );
                continue;
            }

            // descendant 

            if ( selector == ".." )
            {
                foreach ( var childKey in EnumerateKeys( current ) )
                {
                    if ( !TryGetChildValue( current, childKey, out var childValue ) )
                        continue;

                    if ( childValue.IsObjectOrArray() )
                        PushNode( childValue, tokens.Push( "..", SelectorKind.UnspecifiedGroup ), GetPath( path, childKey, current.ValueKind ) );
                }

                PushNode( current, tokens, path );
                continue;
            }

            // union

            foreach ( var childSelector in token.Selectors.Select( x => x.Value ) )
            {
                // [(exp)]

                if ( childSelector.Length > 2 && childSelector[0] == '(' && childSelector[^1] == ')' )
                {
                    if ( evaluator( childSelector, current, path[(path.LastIndexOf( ';' ) + 1)..] ) is not string evalSelector )
                        continue;

                    var selectorKind = evalSelector != "*" && evalSelector != ".." && !RegexSlice().IsMatch( evalSelector )
                        ? SelectorKind.UnspecifiedSingular
                        : SelectorKind.UnspecifiedGroup;

                    PushNode( current, tokens.Push( evalSelector, selectorKind ), path );
                    continue;
                }

                // [?(exp)]

                if ( childSelector.Length > 3 && childSelector[0] == '?' && childSelector[1] == '(' && childSelector[^1] == ')' )
                {
                    foreach ( var childKey in EnumerateKeys( current ) )
                    {
                        if ( !TryGetChildValue( current, childKey, out var childValue ) )
                            continue;

                        var childContext = GetPath( path, childKey, current.ValueKind );
                        var filter = evaluator( RegexPathFilter().Replace( childSelector, "$1" ), childValue, childContext );

                        // treat the filter result as truthy if the evaluator returned a non-convertible object instance. 
                        if ( filter is not null and not IConvertible || Convert.ToBoolean( filter, CultureInfo.InvariantCulture ) )
                            PushNode( current, tokens.Push( childKey, SelectorKind.UnspecifiedSingular ), path );
                    }

                    continue;
                }

                // [name1,name2,...] or [#,#,...] or [start:end:step]

                if ( current.ValueKind == JsonValueKind.Array )
                {
                    if ( RegexNumber().IsMatch( childSelector ) )
                    {
                        // [#,#,...] 
                        PushNode( current[int.Parse( childSelector )], tokens, GetPath( path, childSelector, JsonValueKind.Array ) );
                        continue;
                    }

                    // [start:end:step] Python slice syntax
                    if ( RegexSlice().IsMatch( childSelector ) )
                    {
                        foreach ( var index in EnumerateSlice( current, childSelector ) )
                            PushNode( current[index], tokens, GetPath( path, index ) );
                        continue;
                    }

                    // [name1,name2,...]
                    foreach ( var index in EnumerateArrayIndicies( current ) )
                        PushNode( current[index], tokens.Push( childSelector, SelectorKind.UnspecifiedSingular ), GetPath( path, index ) );

                    continue;
                }

                // [name1,name2,...]

                if ( current.ValueKind == JsonValueKind.Object )
                {
                    if ( RegexSlice().IsMatch( childSelector ) || RegexNumber().IsMatch( childSelector ) )
                        continue;

                    // [name1,name2,...]
                    if ( TryGetChildValue( current, childSelector, out var childValue ) )
                        PushNode( childValue, tokens, GetPath( path, childSelector, JsonValueKind.Object ) );
                }
            }

        } while ( nodes.TryPop( out args ) );
    }

    // because we are using stack processing we will enumerate object members and array
    // indicies in reverse order. this preserves the logical (left-to-right, top-down)
    // order of the match results that are returned to the user.

    private static IEnumerable<string> EnumerateKeys( JsonElement value )
    {
        return value.ValueKind switch
        {
            JsonValueKind.Array => EnumerateArrayIndicies( value ).Select( x => x.ToString() ),
            JsonValueKind.Object => EnumeratePropertyNames( value ),
            _ => throw new NotSupportedException()
        };
    }

    private static IEnumerable<int> EnumerateArrayIndicies( JsonElement value )
    {
        for ( var index = value.GetArrayLength() - 1; index >= 0; index-- )
            yield return index;
    }

    private static IEnumerable<string> EnumeratePropertyNames( JsonElement value )
    {
        // Select() before the Reverse() to reduce size of allocation
        return value.EnumerateObject().Select( x => x.Name ).Reverse();
    }

    private static IEnumerable<int> EnumerateSlice( JsonElement value, string sliceExpr )
    {
        if ( value.ValueKind != JsonValueKind.Array )
            yield break;

        var (lower, upper, step) = SliceSyntaxHelper.ParseExpression( sliceExpr, value.GetArrayLength(), reverse: true );

        switch ( step )
        {
            case 0:
                yield break;

            case > 0:
                for ( var index = lower; index < upper; index += step )
                    yield return index;

                break;
            case < 0:
                for ( var index = upper; index > lower; index += step )
                    yield return index;

                break;
        }
    }

    private static bool TryGetChildValue( in JsonElement value, ReadOnlySpan<char> childKey, out JsonElement childValue )
    {
        static int? TryParseInt( ReadOnlySpan<char> numberString )
        {
            return numberString == null ? null : int.TryParse( numberString, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n ) ? n : null;
        }

        static bool IsPathOperator( ReadOnlySpan<char> x ) => x == "*" || x == ".." || x == "$";

        switch ( value.ValueKind )
        {
            case JsonValueKind.Object:
                if ( value.TryGetProperty( childKey, out childValue ) )
                    return true;
                break;

            case JsonValueKind.Array:
                var index = TryParseInt( childKey ) ?? -1;

                if ( index >= 0 && index < value.GetArrayLength() )
                {
                    childValue = value[index];
                    return true;
                }

                break;

            default:
                if ( !IsPathOperator( childKey ) )
                    throw new ArgumentException( $"Invalid child type '{childKey.ToString()}'. Expected child to be Object, Array or a path selector.", nameof( value ) );
                break;
        }

        childValue = default;
        return false;
    }

    private sealed class VisitorArgs
    {
        public readonly JsonElement Value;
        public readonly IImmutableStack<JsonPathToken> Tokens;
        public readonly string Path;

        public VisitorArgs( in JsonElement value, in IImmutableStack<JsonPathToken> tokens, string path )
        {
            Tokens = tokens;
            Value = value;
            Path = path;
        }

        public void Deconstruct( out JsonElement value, out IImmutableStack<JsonPathToken> tokens, out string path )
        {
            value = Value;
            tokens = Tokens;
            path = Path;
        }
    }

}
