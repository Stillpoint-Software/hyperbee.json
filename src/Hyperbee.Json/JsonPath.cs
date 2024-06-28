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

using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Memory;

namespace Hyperbee.Json;

// https://www.rfc-editor.org/rfc/rfc9535.html
// https://www.rfc-editor.org/rfc/rfc9535.html#appendix-A

public static class JsonPath<TNode>
{
    [DebuggerDisplay( "Value = {Value}, First = ({Segment?.Selectors?[0]}), Singular = {Segment?.Singular}, Count = {Segment?.Selectors?.Length}" )]
    private record struct NodeArgs( TNode Value, JsonPathSegment Segment, NodeFlags Flags = NodeFlags.None );

    private enum NodeFlags
    {
        None = 0,
        AfterDescent = 1
    }

    private static readonly ITypeDescriptor<TNode> Descriptor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>();

    public static IEnumerable<TNode> Select( in TNode value, string query )
    {
        return EnumerateMatches( value, value, query );
    }

    internal static IEnumerable<TNode> SelectInternal( in TNode value, TNode root, string query )
    {
        // this overload is required for reentrant filter select evaluations.
        // it is intended for use by `SelectExpressionFactory` implementations.

        return EnumerateMatches( value, root, query );
    }

    private static IEnumerable<TNode> EnumerateMatches( in TNode value, in TNode root, string query )
    {
        // quick outs

        if ( string.IsNullOrWhiteSpace( query ) ) // Consensus: empty query returns empty array
            return [];

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

            var (value, segmentNext, flags) = args;

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

            var nodeKind = accessor.GetNodeKind( value );

            if ( nodeKind == NodeKind.Value )
                continue;

            // try to access object or array using name or index

            if ( segmentCurrent.Singular )
            {
                if ( nodeKind == NodeKind.Object && selectorKind == SelectorKind.Index )
                    continue; // don't allow indexing in to objects

                if ( accessor.TryGetChildValue( value, selector, out var childValue ) )
                {
                    Push( stack, childValue, segmentNext );
                }

                continue;
            }

            // wildcard

            if ( selectorKind == SelectorKind.Wildcard )
            {
                foreach ( var (childValue, childKey, childKind) in accessor.EnumerateChildren( value ) )
                {
                    // optimization: quicker return for final 
                    //
                    // the parser will work without this check, but we would be forcing it
                    // to push and pop values onto the stack that we know will not be used.
                    if ( segmentNext.IsFinal )
                    {
                        // theoretically, we should yield here, but we can't because we need to
                        // preserve the order of the results as per the RFC. so we push the
                        // value onto the stack without prepending the childKey or childKind
                        // to set up for an immediate return on the next iteration.
                        Push( stack, childValue, segmentNext );
                        continue;
                    }

                    Push( stack, value, segmentNext.Prepend( childKey, childKind ) ); // (Name | Index)
                }

                continue;
            }

            // descendant

            if ( selectorKind == SelectorKind.Descendant )
            {
                foreach ( var (childValue, _, _) in accessor.EnumerateChildren( value, includeValues: false ) ) // child arrays or objects only
                {
                    Push( stack, childValue, segmentCurrent ); // Descendant
                }

                // set values only flag for arrays to avoid duplicate processing
                var nodeFlags = nodeKind == NodeKind.Array
                   ? NodeFlags.AfterDescent
                   : NodeFlags.None;

                Push( stack, value, segmentNext, nodeFlags ); // process the current value
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
                        {
                            // optimization: quicker return for tail values
                            if ( segmentNext.IsFinal )
                            {
                                Push( stack, childValue, segmentNext );
                                continue;
                            }

                            Push( stack, value, segmentNext.Prepend( childKey, childKind ) ); // (Name | Index)
                        }
                    }

                    continue;
                }

                // array [name1,name2,...] or [#,#,...] or [start:end:step]

                if ( nodeKind == NodeKind.Array )
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
                        foreach ( var index in EnumerateSlice( value, selector, accessor ) )
                        {
                            Push( stack, accessor.GetElementAt( value, index ), segmentNext );
                        }
                        continue;
                    }

                    // [name1,name2,...]

                    var indexSegment = segmentNext.Prepend( selector, SelectorKind.Name );
                    var length = accessor.GetArrayLength( value );

                    for ( var index = length - 1; index >= 0; index-- )
                    {
                        var childValue = accessor.GetElementAt( value, index );

                        if ( flags == NodeFlags.AfterDescent && accessor.GetNodeKind( childValue ) != NodeKind.Value )
                            continue;

                        Push( stack, childValue, indexSegment );
                    }

                    continue;
                }

                // object [name1,name2,...]

                if ( nodeKind == NodeKind.Object )
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
    private static void Push( Stack<NodeArgs> stack, in TNode value, in JsonPathSegment segment, NodeFlags flags = NodeFlags.None )
    {
        stack.Push( new NodeArgs( value, segment, flags ) );
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static bool Truthy( object value )
    {
        return value is not null and not IConvertible || Convert.ToBoolean( value, CultureInfo.InvariantCulture );
    }

    private static IEnumerable<int> EnumerateSlice( TNode value, string sliceExpr, IValueAccessor<TNode> accessor )
    {
        var length = accessor.GetArrayLength( value );

        if ( length == 0 )
            yield break;

        var (lower, upper, step) = SliceSyntaxHelper.ParseExpression( sliceExpr, length, reverse: true );

        switch ( step )
        {
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
