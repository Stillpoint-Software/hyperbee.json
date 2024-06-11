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
using Hyperbee.Json.Evaluators;
using Hyperbee.Json.Memory;
using Hyperbee.Json.Tokenizer;

namespace Hyperbee.Json;

// https://ietf-wg-jsonpath.github.io/draft-ietf-jsonpath-base/draft-ietf-jsonpath-base.html
// https://github.com/ietf-wg-jsonpath/draft-ietf-jsonpath-base

public sealed class JsonPath<TElement>
{
    private static readonly IJsonTypeDescriptor Descriptor = JsonTypeRegistry.GetDescriptor<TElement>();

    private static readonly IJsonValueAccessor<TElement> Accessor = Descriptor.GetAccessor<TElement>();

    private static readonly IJsonPathFilterEvaluator<TElement> FilterEvaluator = Descriptor.GetFilterEvaluator<TElement>();

    public IEnumerable<TElement> Select( in TElement value, string query )
    {
        return EnumerateMatches( value, value, query );
    }

    internal static IEnumerable<TElement> Select( in TElement value, TElement root, string query )
    {
        return EnumerateMatches( value, root, query );
    }

    private static IEnumerable<TElement> EnumerateMatches( in TElement value, in TElement root, string query )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace( query );

        // quick out

        if ( query == "$" )
            return [value];

        // tokenize

        var tokens = JsonPathQueryTokenizer.Tokenize( query );

        if ( !tokens.IsEmpty )
        {
            var selector = tokens.Peek().FirstSelector;

            if ( selector == "$" || selector == "@" )
                tokens = tokens.Pop();
        }

        return EnumerateMatches( root, new ElementArgs( value, tokens ) );
    }

    private static IEnumerable<TElement> EnumerateMatches( TElement root, ElementArgs args )
    {
        var stack = new Stack<ElementArgs>( 4 );

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
            var selector = token.FirstSelector;

            // make sure we have a complex value

            if ( !Accessor.IsObjectOrArray( current ) )
                throw new InvalidOperationException( "Object or Array expected." );

            // try to access object or array using KEY value

            if ( token.Singular )
            {
                if ( Accessor.TryGetChildValue( current, selector, out var childValue ) )
                    Push( stack, childValue, tokens );

                continue;
            }

            // wildcard

            if ( selector == "*" )
            {
                foreach ( var (_, childKey) in Accessor.EnumerateChildValues( current ) )
                {
                    Push( stack, current, tokens.Push( new( childKey, SelectorKind.UnspecifiedSingular ) ) ); // (Dot | Index)
                }

                continue;
            }

            // descendant

            if ( selector == ".." )
            {
                foreach ( var (childValue, _) in Accessor.EnumerateChildValues( current ) )
                {
                    if ( Accessor.IsObjectOrArray( childValue ) )
                        Push( stack, childValue, tokens.Push( new( "..", SelectorKind.UnspecifiedGroup ) ) ); // Descendant
                }

                Push( stack, current, tokens );
                continue;
            }

            // union

            for ( var i = 0; i < token.Selectors.Length; i++ ) // using 'for' for performance
            {
                var childSelector = token.Selectors[i].Value;

                // [(exp)]

                if ( childSelector.Length > 2 && childSelector[0] == '(' && childSelector[^1] == ')' )
                {
                    if ( FilterEvaluator.Evaluate( childSelector, current, root ) is not string evalSelector )
                        continue;

                    var selectorKind = evalSelector != "*" && evalSelector != ".." && !JsonPathRegex.RegexSlice().IsMatch( evalSelector ) // (Dot | Index) | Wildcard, Descendant, Slice 
                        ? SelectorKind.UnspecifiedSingular
                        : SelectorKind.UnspecifiedGroup;

                    Push( stack, current, tokens.Push( new( evalSelector, selectorKind ) ) );
                    continue;
                }

                // [?(exp)]

                if ( childSelector.Length > 3 && childSelector[0] == '?' && childSelector[1] == '(' && childSelector[^1] == ')' )
                {
                    foreach ( var (childValue, childKey) in Accessor.EnumerateChildValues( current ) )
                    {
                        var filter = FilterEvaluator.Evaluate( JsonPathRegex.RegexPathFilter().Replace( childSelector, "$1" ), childValue, root );

                        // treat the filter result as truthy if the evaluator returned a non-convertible object instance. 
                        if ( filter is not null and not IConvertible || Convert.ToBoolean( filter, CultureInfo.InvariantCulture ) )
                            Push( stack, current, tokens.Push( new( childKey, SelectorKind.UnspecifiedSingular ) ) ); // (Name | Index)
                    }

                    continue;
                }

                // [name1,name2,...] or [#,#,...] or [start:end:step]

                if ( Accessor.IsArray( current, out var length ) )
                {
                    if ( JsonPathRegex.RegexNumber().IsMatch( childSelector ) )
                    {
                        // [#,#,...] 
                        Push( stack, Accessor.GetElementAt( current, int.Parse( childSelector ) ), tokens );
                        continue;
                    }

                    // [start:end:step] Python slice syntax
                    if ( JsonPathRegex.RegexSlice().IsMatch( childSelector ) )
                    {
                        foreach ( var index in EnumerateSlice( current, childSelector ) )
                            Push( stack, Accessor.GetElementAt( current, index ), tokens );
                        continue;
                    }

                    // [name1,name2,...]
                    foreach ( var index in EnumerateArrayIndices( length ) )
                        Push( stack, Accessor.GetElementAt( current, index ), tokens.Push( new( childSelector, SelectorKind.UnspecifiedSingular ) ) ); // Name

                    continue;
                }

                // [name1,name2,...]

                if ( Accessor.IsObject( current ) )
                {
                    if ( JsonPathRegex.RegexSlice().IsMatch( childSelector ) || JsonPathRegex.RegexNumber().IsMatch( childSelector ) )
                        continue;

                    // [name1,name2,...]
                    if ( Accessor.TryGetChildValue( current, childSelector, out var childValue ) )
                        Push( stack, childValue, tokens );
                }
            }

        } while ( stack.TryPop( out args ) );

        yield break;

        static void Push( Stack<ElementArgs> s, in TElement v, in IImmutableStack<JsonPathToken> t ) => s.Push( new ElementArgs( v, t ) );
    }

    private static IEnumerable<int> EnumerateArrayIndices( int length )
    {
        for ( var index = length - 1; index >= 0; index-- )
            yield return index;
    }

    private static IEnumerable<int> EnumerateSlice( TElement value, string sliceExpr )
    {
        if ( !Accessor.IsArray( value, out var length ) )
            yield break;

        var (lower, upper, step) = SliceSyntaxHelper.ParseExpression( sliceExpr, length, reverse: true );

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

    private sealed class ElementArgs( in TElement value, in IImmutableStack<JsonPathToken> tokens )
    {
        public readonly TElement Value = value;
        public readonly IImmutableStack<JsonPathToken> Tokens = tokens;

        public void Deconstruct( out TElement value, out IImmutableStack<JsonPathToken> tokens )
        {
            value = Value;
            tokens = Tokens;
        }
    }
}
