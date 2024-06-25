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

// https://www.rfc-editor.org/rfc/rfc9535.html

public static class JsonPath<TNode>
{
    private record struct NodeArgs( in TNode Value, in JsonPathSegment Segment );

    private static readonly ITypeDescriptor<TNode> Descriptor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>();

    public static IEnumerable<TNode> Select( in TNode value, string query )
    {
        return EnumerateMatches( value, value, query );
    }

    internal static IEnumerable<TNode> SelectInternal( in TNode value, TNode root, string query )
    {
        // this overload is required for reentrant filter select evaluations.
        // it is intended for use by nameof(FilterFunction) implementations.

        return EnumerateMatches( value, root, query );
    }

    private static IEnumerable<TNode> EnumerateMatches( in TNode value, in TNode root, string query )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace( query );

        // quick out

        if ( query == "$" || query == "@" )
            return [value];

        // tokenize

        var segmentNext = JsonPathQueryParser.Parse( query );

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
            // deconstruct next args

            var (value, segmentNext) = args;

            if ( segmentNext.IsFinal )
            {
                yield return value;
                continue;
            }

            // get the current segment, and then move the segments
            // reference to the next segment in the list

            var segmentCurrent = segmentNext; // get current segment
            var (selector, selectorKind) = segmentCurrent.Selectors[0]; // first selector in segment

            segmentNext = segmentNext.Next;

            // make sure we have a complex value

            if ( !accessor.IsObjectOrArray( value ) )
                throw new InvalidOperationException( "Object or Array expected." );

            // try to access object or array using name or index

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

                // we can reduce push/pop operations, and related allocations, if we check
                // segmentNext.IsFinal and directly yield when true where possible. 
                //
                // if ( segmentNext.IsFinal && !childValue.IsObjectOrArray() )
                //    yield return childValue;
                // else
                //    Push( stack, value, segmentNext.Prepend( childKey, childKind ) );                 
                //
                // unfortunately, this optimization impacts result ordering. the rfc states 
                // result order should be as close to json document order as possible. for
                // that reason, we chose not to implement this type of performance optimization.
            }

            // descendant

            if ( selectorKind == SelectorKind.Descendant )
            {
                var descendantSegment = segmentNext.Prepend( "..", SelectorKind.Descendant );
                foreach ( var (childValue, _, _) in accessor.EnumerateChildren( value, includeValues: false ) ) // child arrays or objects only
                {
                    Push( stack, childValue, descendantSegment ); // Descendant
                }

                Push( stack, value, segmentNext ); // process the current value
                continue;
            }

            // group

            for ( var i = 0; i < segmentCurrent.Selectors.Length; i++ ) // use 'for' for performance
            {
                if ( i != 0 )
                    (selector, selectorKind) = segmentCurrent.Selectors[i];

                // [?exp]

                if ( selectorKind == SelectorKind.Filter )
                {
                    foreach ( var (childValue, childKey, childKind) in accessor.EnumerateChildren( value ) )
                    {
                        var result = filterEvaluator.Evaluate( selector[1..], childValue, root ); // remove leading '?'

                        if ( Truthy( result ) )
                            Push( stack, value, segmentNext.Prepend( childKey, childKind ) ); // (Name | Index)
                    }

                    continue;
                }

                // array [name1,name2,...] or [#,#,...] or [start:end:step]

                if ( accessor.IsArray( value, out var length ) )
                {
                    // [#,#,...] 

                    if ( selectorKind == SelectorKind.Index )
                    {
                        Push( stack, accessor.GetElementAt( value, int.Parse( selector ) ), segmentNext );
                        continue;
                    }

                    // [start:end:step] Python slice syntax

                    if ( selectorKind == SelectorKind.Slice )
                    {
                        ProcessSlice( stack, value, selector, segmentNext, accessor );
                        continue;
                    }

                    // [name1,name2,...]

                    var indexSegment = segmentNext.Prepend( selector, SelectorKind.Name );
                    for ( var index = length - 1; index >= 0; index-- )
                    {
                        Push( stack, accessor.GetElementAt( value, index ), indexSegment );
                    }

                    continue;
                }

                // object [name1,name2,...]

                if ( accessor.IsObject( value ) )
                {
                    if ( selectorKind == SelectorKind.Slice || selectorKind == SelectorKind.Index )
                        continue;

                    if ( accessor.TryGetChildValue( value, selector, out var childValue ) )
                        Push( stack, childValue, segmentNext );
                }
            }

        } while ( stack.TryPop( out args ) );
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static void Push( Stack<NodeArgs> stack, in TNode value, in JsonPathSegment segment )
    {
        stack.Push( new NodeArgs( value, segment ) );
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static bool Truthy( object value )
    {
        return value is not null and not IConvertible || Convert.ToBoolean( value, CultureInfo.InvariantCulture );
    }

    private static void ProcessSlice( Stack<NodeArgs> stack, TNode value, string sliceExpr, JsonPathSegment segmentNext, IValueAccessor<TNode> accessor )
    {
        if ( !accessor.IsArray( value, out var length ) )
            return;

        var (lower, upper, step) = SliceSyntaxHelper.ParseExpression( sliceExpr, length, reverse: true );

        switch ( step )
        {
            case > 0:
                {
                    for ( var index = lower; index < upper; index += step )
                    {
                        Push( stack, accessor.GetElementAt( value, index ), segmentNext );
                    }

                    break;
                }
            case < 0:
                {
                    for ( var index = upper; index > lower; index += step )
                    {
                        Push( stack, accessor.GetElementAt( value, index ), segmentNext );
                    }

                    break;
                }
        }
    }
}
