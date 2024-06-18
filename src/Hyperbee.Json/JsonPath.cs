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
using System.Runtime.CompilerServices;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Memory;

namespace Hyperbee.Json;

// https://ietf-wg-jsonpath.github.io/draft-ietf-jsonpath-base/draft-ietf-jsonpath-base.html
// https://github.com/ietf-wg-jsonpath/draft-ietf-jsonpath-base

public static class JsonPath<TNode>
{
    private record struct NodeArgs( in TNode Value, in JsonPathSegment Segment );

    private static readonly ITypeDescriptor<TNode> Descriptor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>();

    public static IEnumerable<TNode> Select( in TNode value, string query )
    {
        return EnumerateMatches( value, value, query );
    }

    internal static IEnumerable<TNode> Select( in TNode value, TNode root, string query )
    {
        return EnumerateMatches( value, root, query );
    }

    private static IEnumerable<TNode> EnumerateMatches( in TNode value, in TNode root, string query )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace( query );

        // quick out

        if ( query == "$" || query == "@" )
            return [value];

        // tokenize

        var segmentNext = JsonPathQueryTokenizer.Tokenize( query );

        if ( !segmentNext.IsFinal )
        {
            var selector = segmentNext.Selectors[0].Value; // first selector in segment

            if ( selector == "$" || selector == "@" )
                segmentNext = segmentNext.Next;
        }

        return EnumerateMatches( root, new NodeArgs( value, segmentNext ) );
    }

    private static IEnumerable<TNode> EnumerateMatches( TNode root, NodeArgs args )
    {
        var stack = new Stack<NodeArgs>( 16 );

        var (accessor, filterEvaluator) = Descriptor;

        do
        {
            // deconstruct the next args node

            var (value, segmentNext) = args;

            if ( segmentNext.IsFinal )
            {
                yield return value;
                continue;
            }

            // get the current segment, and then move the segments
            // reference to the next segment in the list

            var segmentCurrent = segmentNext; // get current segment
            var (selector, selectorKind) = segmentCurrent.Selectors[0]; // first selector in segment;

            segmentNext = segmentNext.Next;

            // make sure we have a complex value

            if ( !accessor.IsObjectOrArray( value ) )
                throw new InvalidOperationException( "Object or Array expected." );

            // try to access object or array using KEY value

            if ( segmentCurrent.Singular )
            {
                if ( accessor.TryGetChildValue( value, selector, out var childValue ) )
                {
                    Push( stack, childValue, segmentNext );
                }

                continue;
            }

            // wildcard

            if ( selectorKind == SelectorKind.Wildcard )
            {
                foreach ( var (_, childKey, childKind) in accessor.EnumerateChildren( value ) )
                {
                    Push( stack, value, segmentNext.Prepend( childKey, childKind ) ); // (Name | Index)
                }

                continue;
            }

            // descendant

            if ( selectorKind == SelectorKind.Descendant )
            {
                foreach ( var (childValue, _, _) in accessor.EnumerateChildren( value, includeValues: false ) ) // child arrays or objects only
                {
                    Push( stack, childValue, segmentNext.Prepend( "..", SelectorKind.Descendant ) ); // Descendant
                }

                Push( stack, value, segmentNext );
                continue;
            }

            // group

            for ( var i = 0; i < segmentCurrent.Selectors.Length; i++ ) // using 'for' for performance
            {
                if ( i != 0 )
                    (selector, selectorKind) = segmentCurrent.Selectors[i];

                // [?exp]

                if ( selectorKind == SelectorKind.Filter )
                {
                    foreach ( var (childValue, childKey, childKind) in accessor.EnumerateChildren( value ) )
                    {
                        var filter = selector[1..]; // remove '?'
                        var result = filterEvaluator.Evaluate( filter, childValue, root );

                        if ( Truthy( result ) )
                            Push( stack, value, segmentNext.Prepend( childKey, childKind ) ); // (Name | Index)
                    }

                    continue;
                }

                // [name1,name2,...] or [#,#,...] or [start:end:step]

                if ( accessor.IsArray( value, out var length ) )
                {
                    if ( selectorKind == SelectorKind.Index )
                    {
                        // [#,#,...] 
                        Push( stack, accessor.GetElementAt( value, int.Parse( selector ) ), segmentNext );
                        continue;
                    }

                    // [start:end:step] Python slice syntax
                    if ( selectorKind == SelectorKind.Slice )
                    {
                        foreach ( var index in EnumerateSlice( value, selector ) )
                            Push( stack, accessor.GetElementAt( value, index ), segmentNext );
                        continue;
                    }

                    // [name1,name2,...]
                    foreach ( var index in EnumerateArrayIndices( length ) )
                        Push( stack, accessor.GetElementAt( value, index ), segmentNext.Prepend( selector, SelectorKind.Name ) ); // Name

                    continue;
                }

                // [name1,name2,...]

                if ( accessor.IsObject( value ) )
                {
                    if ( selectorKind == SelectorKind.Slice || selectorKind == SelectorKind.Index )
                        continue;

                    // [name1,name2,...]
                    if ( accessor.TryGetChildValue( value, selector, out var childValue ) )
                        Push( stack, childValue, segmentNext );
                }
            }

        } while ( stack.TryPop( out args ) );

        yield break;

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static void Push( Stack<NodeArgs> n, in TNode v, in JsonPathSegment s ) => n.Push( new NodeArgs( v, s ) );
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static bool Truthy( object value )
    {
        return value is not null and not IConvertible || Convert.ToBoolean( value, CultureInfo.InvariantCulture );
    }

    private static IEnumerable<int> EnumerateArrayIndices( int length )
    {
        for ( var index = length - 1; index >= 0; index-- )
            yield return index;
    }

    private static IEnumerable<int> EnumerateSlice( TNode value, string sliceExpr )
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
}
