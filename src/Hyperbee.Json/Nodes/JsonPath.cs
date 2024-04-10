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
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Hyperbee.Json.Evaluators;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Memory;
using Hyperbee.Json.Tokenizer;

namespace Hyperbee.Json.Nodes;
// https://ietf-wg-jsonpath.github.io/draft-ietf-jsonpath-base/draft-ietf-jsonpath-base.html
// https://github.com/ietf-wg-jsonpath/draft-ietf-jsonpath-base

public sealed partial class JsonPathNode
{
    public static IJsonPathScriptEvaluator<JsonNode> DefaultEvaluator { get; set; } = new JsonPathCSharpNodeEvaluator();
    private readonly IJsonPathScriptEvaluator<JsonNode> _evaluator;

    // generated regex

    [GeneratedRegex( "^(-?[0-9]*):?(-?[0-9]*):?(-?[0-9]*)$" )]
    private static partial Regex RegexSlice();

    [GeneratedRegex( @"^\?\((.*?)\)$" )]
    private static partial Regex RegexPathFilter();

    [GeneratedRegex( @"^[0-9*]+$" )]
    private static partial Regex RegexNumber();

    // ctor

    public JsonPathNode()
        : this( null )
    {
    }

    public JsonPathNode( IJsonPathScriptEvaluator<JsonNode> evaluator )
    {
        _evaluator = evaluator ?? DefaultEvaluator ?? new JsonPathCSharpNodeEvaluator();
    }

    public IEnumerable<JsonNode> Select( in JsonNode value, string query )
    {
        if ( string.IsNullOrWhiteSpace( query ) )
            throw new ArgumentNullException( nameof( query ) );

        // quick out

        if ( query == "$" )
            return new[] { value };

        // tokenize

        var tokens = JsonPathQueryTokenizer.Tokenize( query );

        // initiate the expression walk

        if ( !tokens.IsEmpty && tokens.Peek().Selectors.First().Value == "$" )
            tokens = tokens.Pop();

        return ExpressionVisitor( new VisitorArgs( value, tokens ), _evaluator.Evaluator );
    }

    private static IEnumerable<JsonNode> ExpressionVisitor( VisitorArgs args, JsonPathEvaluator<JsonNode> evaluator )
    {
        var nodes = new Stack<VisitorArgs>( 4 );
        void PushNode( in JsonNode v, in IImmutableStack<JsonPathToken> t ) => nodes.Push( new VisitorArgs( v, t ) );

        do
        {
            // deconstruct the next args node

            var (current, tokens) = args;

            if ( tokens.IsEmpty )
            {
                yield return current;
                continue;
            }

            // pop the next token from the stack

            tokens = tokens.Pop( out var token );
            var selector = token.Selectors.First().Value;

            // make sure we have a container value

            if ( !(current is JsonObject || current is JsonArray) )
                throw new InvalidOperationException( "Object or Array expected." );

            // try to access object or array using KEY value

            if ( token.Singular )
            {
                if ( TryGetChildValue( current, selector, out var childValue ) )
                    PushNode( childValue, tokens );

                continue;
            }

            // wildcard

            if ( selector == "*" )
            {
                foreach ( var childKey in EnumerateKeys( current ) )
                    PushNode( current, tokens.Push( childKey, SelectorKind.UnspecifiedSingular ) ); // (Dot | Index)
                continue;
            }

            // descendant

            if ( selector == ".." )
            {
                foreach ( var childKey in EnumerateKeys( current ) )
                {
                    if ( !TryGetChildValue( current, childKey, out var childValue ) )
                        continue;

                    if ( childValue is JsonObject || childValue is JsonArray )
                        PushNode( childValue, tokens.Push( "..", SelectorKind.UnspecifiedGroup ) ); // Descendant
                }

                PushNode( current, tokens );
                continue;
            }

            // union

            foreach ( var childSelector in token.Selectors.Select( x => x.Value ) )
            {
                // [(exp)]

                if ( childSelector.Length > 2 && childSelector[0] == '(' && childSelector[^1] == ')' )
                {
                    if ( evaluator( childSelector, current, current.GetPath() ) is not string evalSelector )
                        continue;

                    var selectorKind = evalSelector != "*" && evalSelector != ".." && !RegexSlice().IsMatch( evalSelector ) // (Dot | Index) | Wildcard, Descendant, Slice 
                        ? SelectorKind.UnspecifiedSingular
                        : SelectorKind.UnspecifiedGroup;

                    PushNode( current, tokens.Push( evalSelector, selectorKind ) );
                    continue;
                }

                // [?(exp)]

                if ( childSelector.Length > 3 && childSelector[0] == '?' && childSelector[1] == '(' && childSelector[^1] == ')' )
                {
                    foreach ( var childKey in EnumerateKeys( current ) )
                    {
                        if ( !TryGetChildValue( current, childKey, out var childValue ) )
                            continue;

                        var filter = evaluator( RegexPathFilter().Replace( childSelector, "$1" ), childValue, childValue.GetPath() );

                        // treat the filter result as truthy if the evaluator returned a non-convertible object instance. 
                        if ( filter is not null and not IConvertible || Convert.ToBoolean( filter, CultureInfo.InvariantCulture ) )
                            PushNode( current, tokens.Push( childKey, SelectorKind.UnspecifiedSingular ) ); // (Name | Index)
                    }

                    continue;
                }

                // [name1,name2,...] or [#,#,...] or [start:end:step]

                if ( current is JsonArray currentArray )
                {
                    if ( RegexNumber().IsMatch( childSelector ) )
                    {
                        // [#,#,...] 
                        PushNode( current[int.Parse( childSelector )], tokens );
                        continue;
                    }

                    // [start:end:step] Python slice syntax
                    if ( RegexSlice().IsMatch( childSelector ) )
                    {
                        foreach ( var index in EnumerateSlice( current, childSelector ) )
                            PushNode( current[index], tokens );
                        continue;
                    }

                    // [name1,name2,...]
                    foreach ( var index in EnumerateArrayIndicies( currentArray ) )
                        PushNode( current[index], tokens.Push( childSelector, SelectorKind.UnspecifiedSingular ) ); // Name

                    continue;
                }

                // [name1,name2,...]

                if ( current is JsonObject )
                {
                    if ( RegexSlice().IsMatch( childSelector ) || RegexNumber().IsMatch( childSelector ) )
                        continue;

                    // [name1,name2,...]
                    if ( TryGetChildValue( current, childSelector, out var childValue ) )
                        PushNode( childValue, tokens );
                }
            }

        } while ( nodes.TryPop( out args ) );
    }

    // because we are using stack processing we will enumerate object members and array
    // indicies in reverse order. this preserves the logical (left-to-right, top-down)
    // order of the match results that are returned to the user.

    private static IEnumerable<string> EnumerateKeys( JsonNode value )
    {
        return value switch
        {
            JsonArray valueArray => EnumerateArrayIndicies( valueArray ).Select( x => x.ToString() ),
            JsonObject valueObject => EnumeratePropertyNames( valueObject ),
            _ => throw new NotSupportedException()
        };
    }

    private static IEnumerable<int> EnumerateArrayIndicies( JsonArray value )
    {
        for ( var index = value.Count - 1; index >= 0; index-- )
            yield return index;
    }

    private static IEnumerable<string> EnumeratePropertyNames( JsonObject value )
    {
        // Select() before the Reverse() to reduce size of allocation
        return value.Select( x => x.Key ).Reverse();
    }

    private static IEnumerable<int> EnumerateSlice( JsonNode value, string sliceExpr )
    {
        if ( value is not JsonArray valueArray )
            yield break;

        var (lower, upper, step) = SliceSyntaxHelper.ParseExpression( sliceExpr, valueArray.Count, reverse: true );

        switch ( step )
        {
            case 0:
                {
                    yield break;
                }
            case > 0:
                {
                    for ( var index = lower; index < upper; index += step )
                        yield return index;
                    break;
                }
            case < 0:
                {
                    for ( var index = upper; index > lower; index += step )
                        yield return index;
                    break;
                }
        }
    }

    private static bool TryGetChildValue( in JsonNode value, ReadOnlySpan<char> childKey, out JsonNode childValue )
    {
        static int? TryParseInt( ReadOnlySpan<char> numberString )
        {
            return numberString == null ? null : int.TryParse( numberString, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n ) ? n : null;
        }

        static bool IsPathOperator( ReadOnlySpan<char> x ) => x == "*" || x == ".." || x == "$";

        switch ( value )
        {
            case JsonObject valueObject:
                {
                    if ( valueObject.TryGetPropertyValue( childKey.ToString(), out childValue ) )
                        return true;
                    break;
                }
            case JsonArray valueArray:
                {
                    var index = TryParseInt( childKey ) ?? -1;

                    if ( index >= 0 && index < valueArray.Count )
                    {
                        childValue = value[index];
                        return true;
                    }

                    break;
                }
            default:
                {
                    if ( !IsPathOperator( childKey ) )
                        throw new ArgumentException( $"Invalid child type '{childKey.ToString()}'. Expected child to be Object, Array or a path selector.", nameof( value ) );
                    break;
                }
        }

        childValue = default;
        return false;
    }

    private sealed class VisitorArgs
    {
        public readonly JsonNode Value;
        public readonly IImmutableStack<JsonPathToken> Tokens;

        public VisitorArgs( in JsonNode value, in IImmutableStack<JsonPathToken> tokens )
        {
            Tokens = tokens;
            Value = value;
        }

        public void Deconstruct( out JsonNode value, out IImmutableStack<JsonPathToken> tokens )
        {
            value = Value;
            tokens = Tokens;
        }
    }
}
