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

using System.Globalization;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Memory;

namespace Hyperbee.Json;

// https://ietf-wg-jsonpath.github.io/draft-ietf-jsonpath-base/draft-ietf-jsonpath-base.html
// https://github.com/ietf-wg-jsonpath/draft-ietf-jsonpath-base

public sealed class JsonPath<TElement>
{
    private static readonly ITypeDescriptor<TElement> Descriptor = JsonTypeDescriptorRegistry.GetDescriptor<TElement>();

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

        if ( query == "$" || query == "@" )
            return [value];

        // tokenize

        var segments = JsonPathQueryTokenizer.Tokenize( query );

        if ( !segments.IsEmpty )
        {
            var selector = segments.Selectors[0].Value; // first selector in segment

            if ( selector == "$" || selector == "@" )
                segments = segments.Next;
        }

        return EnumerateMatches( root, new NodeArgs( value, segments ) );
    }

    private static IEnumerable<TElement> EnumerateMatches( TElement root, NodeArgs args )
    {
        var stack = new Stack<NodeArgs>( 16 );

        var filterEvaluator = Descriptor.FilterEvaluator;
        var accessor = Descriptor.Accessor;

        do
        {
            // deconstruct the next args node

            var (current, segments) = args;

            if ( segments.IsEmpty )
            {
                yield return current;
                continue;
            }

            // get the current segment as out, and then move the
            // segments reference to the next segment in the list

            segments = segments.MoveNext( out var segment );
            var selector = segment.Selectors[0].Value; // first selector in segment;

            // make sure we have a complex value

            if ( !accessor.IsObjectOrArray( current ) )
                throw new InvalidOperationException( "Object or Array expected." );

            // try to access object or array using KEY value

            if ( segment.Singular )
            {
                if ( accessor.TryGetChildValue( current, selector, out var childValue ) )
                    Push( stack, childValue, segments );

                continue;
            }

            // wildcard

            if ( selector == "*" )
            {
                foreach ( var (_, childKey) in accessor.EnumerateChildren( current ) )
                {
                    Push( stack, current, segments.Insert( childKey, SelectorKind.UnspecifiedSingular ) ); // (Dot | Index)
                }

                continue;
            }

            // descendant

            if ( selector == ".." )
            {
                foreach ( var (childValue, _) in accessor.EnumerateChildren( current, includeValues: false ) ) // child arrays or objects only
                {
                    Push( stack, childValue, segments.Insert( "..", SelectorKind.UnspecifiedGroup ) ); // Descendant
                }

                Push( stack, current, segments );
                continue;
            }

            // union

            for ( var i = 0; i < segment.Selectors.Length; i++ ) // using 'for' for performance
            {
                var childSelector = segment.Selectors[i].Value;

                // [(exp)]

                if ( childSelector.Length > 2 && childSelector[0] == '(' && childSelector[^1] == ')' )
                {
                    if ( filterEvaluator.Evaluate( childSelector, current, root ) is not string filterSelector )
                        continue;

                    var selectorKind = filterSelector != "*" && filterSelector != ".." && !JsonPathRegex.RegexSlice().IsMatch( filterSelector ) // (Dot | Index) | Wildcard, Descendant, Slice 
                        ? SelectorKind.UnspecifiedSingular
                        : SelectorKind.UnspecifiedGroup;

                    Push( stack, current, segments.Insert( filterSelector, selectorKind ) );
                    continue;
                }

                // [?(exp)]

                if ( childSelector.Length > 3 && childSelector[0] == '?' && childSelector[1] == '(' && childSelector[^1] == ')' )
                {
                    foreach ( var (childValue, childKey) in accessor.EnumerateChildren( current ) )
                    {
                        var filterValue = filterEvaluator.Evaluate( JsonPathRegex.RegexPathFilter().Replace( childSelector, "$1" ), childValue, root );

                        if ( Truthy( filterValue ) )
                            Push( stack, current, segments.Insert( childKey, SelectorKind.UnspecifiedSingular ) ); // (Name | Index)
                    }

                    continue;
                }

                // [name1,name2,...] or [#,#,...] or [start:end:step]

                if ( accessor.IsArray( current, out var length ) )
                {
                    if ( JsonPathRegex.RegexNumber().IsMatch( childSelector ) )
                    {
                        // [#,#,...] 
                        Push( stack, accessor.GetElementAt( current, int.Parse( childSelector ) ), segments );
                        continue;
                    }

                    // [start:end:step] Python slice syntax
                    if ( JsonPathRegex.RegexSlice().IsMatch( childSelector ) )
                    {
                        foreach ( var index in EnumerateSlice( current, childSelector ) )
                            Push( stack, accessor.GetElementAt( current, index ), segments );
                        continue;
                    }

                    // [name1,name2,...]
                    foreach ( var index in EnumerateArrayIndices( length ) )
                        Push( stack, accessor.GetElementAt( current, index ), segments.Insert( childSelector, SelectorKind.UnspecifiedSingular ) ); // Name

                    continue;
                }

                // [name1,name2,...]

                if ( accessor.IsObject( current ) )
                {
                    if ( JsonPathRegex.RegexSlice().IsMatch( childSelector ) || JsonPathRegex.RegexNumber().IsMatch( childSelector ) )
                        continue;

                    // [name1,name2,...]
                    if ( accessor.TryGetChildValue( current, childSelector, out var childValue ) )
                        Push( stack, childValue, segments );
                }
            }

        } while ( stack.TryPop( out args ) );

        yield break;

        static void Push( Stack<NodeArgs> s, in TElement v, in JsonPathSegment t ) => s.Push( new NodeArgs( v, t ) );
    }

    private static bool Truthy( object value )
    {
        return value is not null and not IConvertible || Convert.ToBoolean( value, CultureInfo.InvariantCulture );
    }

    private static IEnumerable<int> EnumerateArrayIndices( int length )
    {
        for ( var index = length - 1; index >= 0; index-- )
            yield return index;
    }

    private static IEnumerable<int> EnumerateSlice( TElement value, string sliceExpr )
    {
        if ( !Descriptor.Accessor.IsArray( value, out var length ) )
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

    private sealed class NodeArgs( in TElement value, in JsonPathSegment segment )
    {
        public readonly TElement Value = value;
        public readonly JsonPathSegment Segment = segment;

        public void Deconstruct( out TElement value, out JsonPathSegment segment )
        {
            value = Value;
            segment = Segment;
        }
    }

}
