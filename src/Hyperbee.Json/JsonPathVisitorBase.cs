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

public abstract class JsonPathVisitorBase<TElement>
{
    internal IEnumerable<TElement> ExpressionVisitor( in TElement value, in TElement root, string query, IJsonPathFilterEvaluator<TElement> filterEvaluator )
    {
        if ( string.IsNullOrWhiteSpace( query ) )
            throw new ArgumentNullException( nameof( query ) );

        if ( filterEvaluator == null )
            throw new ArgumentNullException( nameof( filterEvaluator ) );

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

        return ExpressionVisitor( root, new VisitorArgs( value, tokens ), filterEvaluator );
    }

    private IEnumerable<TElement> ExpressionVisitor( TElement root, VisitorArgs args, IJsonPathFilterEvaluator<TElement> filterEvaluator )
    {
        var stack = new Stack<VisitorArgs>( 4 );

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

            if ( !IsObjectOrArray( current ) )
                throw new InvalidOperationException( "Object or Array expected." );

            // try to access object or array using KEY value

            if ( token.Singular )
            {
                if ( TryGetChildValue( current, selector, out var childValue ) )
                    Push( stack, childValue, tokens );

                continue;
            }

            // wildcard

            if ( selector == "*" )
            {
                foreach ( var (_, childKey) in EnumerateChildValues( current ) )
                {
                    Push( stack, current, tokens.Push( new( childKey, SelectorKind.UnspecifiedSingular ) ) ); // (Dot | Index)
                }

                continue;
            }

            // descendant

            if ( selector == ".." )
            {
                foreach ( var (childValue, _) in EnumerateChildValues( current ) )
                {
                    if ( IsObjectOrArray( childValue ) )
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
                    if ( filterEvaluator.Evaluate( childSelector, current, root ) is not string evalSelector )
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
                    foreach ( var (childValue, childKey) in EnumerateChildValues( current ) )
                    {
                        var filter = filterEvaluator.Evaluate( JsonPathRegex.RegexPathFilter().Replace( childSelector, "$1" ), childValue, root );

                        // treat the filter result as truthy if the evaluator returned a non-convertible object instance. 
                        if ( filter is not null and not IConvertible || Convert.ToBoolean( filter, CultureInfo.InvariantCulture ) )
                            Push( stack, current, tokens.Push( new( childKey, SelectorKind.UnspecifiedSingular ) ) ); // (Name | Index)
                    }

                    continue;
                }

                // [name1,name2,...] or [#,#,...] or [start:end:step]

                if ( IsArray( current, out var length ) )
                {
                    if ( JsonPathRegex.RegexNumber().IsMatch( childSelector ) )
                    {
                        // [#,#,...] 
                        Push( stack, GetElementAt( current, int.Parse( childSelector ) ), tokens );
                        continue;
                    }

                    // [start:end:step] Python slice syntax
                    if ( JsonPathRegex.RegexSlice().IsMatch( childSelector ) )
                    {
                        foreach ( var index in EnumerateSlice( current, childSelector ) )
                            Push( stack, GetElementAt( current, index ), tokens );
                        continue;
                    }

                    // [name1,name2,...]
                    foreach ( var index in EnumerateArrayIndices( length ) )
                        Push( stack, GetElementAt( current, index ), tokens.Push( new( childSelector, SelectorKind.UnspecifiedSingular ) ) ); // Name

                    continue;
                }

                // [name1,name2,...]

                if ( IsObject( current ) )
                {
                    if ( JsonPathRegex.RegexSlice().IsMatch( childSelector ) || JsonPathRegex.RegexNumber().IsMatch( childSelector ) )
                        continue;

                    // [name1,name2,...]
                    if ( TryGetChildValue( current, childSelector, out var childValue ) )
                        Push( stack, childValue, tokens );
                }
            }

        } while ( stack.TryPop( out args ) );

        yield break;

        static void Push( Stack<VisitorArgs> s, in TElement v, in IImmutableStack<JsonPathToken> t ) => s.Push( new VisitorArgs( v, t ) );
    }

    private static IEnumerable<int> EnumerateArrayIndices( int length )
    {
        for ( var index = length - 1; index >= 0; index-- )
            yield return index;
    }

    private IEnumerable<int> EnumerateSlice( TElement value, string sliceExpr )
    {
        if ( !IsArray( value, out var length ) )
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

    // abstract methods

    internal abstract IEnumerable<(TElement, string)> EnumerateChildValues( TElement value );
    internal abstract TElement GetElementAt( TElement value, int index );
    internal abstract bool IsObjectOrArray( TElement current );
    internal abstract bool IsArray( TElement current, out int length );
    internal abstract bool IsObject( TElement current );
    internal abstract bool TryGetChildValue( in TElement current, ReadOnlySpan<char> childKey, out TElement childValue );

    // visitor context

    private sealed class VisitorArgs( in TElement value, in IImmutableStack<JsonPathToken> tokens )
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
