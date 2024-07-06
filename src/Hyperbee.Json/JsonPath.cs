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

// https://www.rfc-editor.org/rfc/rfc9535.html
// https://www.rfc-editor.org/rfc/rfc9535.html#appendix-A

namespace Hyperbee.Json;

public delegate void NodeProcessorDelegate<TNode>( in TNode parent, in TNode value, string key, in JsonPathSegment segment );

public static class JsonPath<TNode>
{
    [Flags]
    internal enum NodeFlags
    {
        Default = 0,
        AfterDescent = 1
    }

    private static readonly ITypeDescriptor<TNode> Descriptor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>();

    public static IEnumerable<TNode> Select( in TNode value, string query, NodeProcessorDelegate<TNode> processor = null )
    {
        return EnumerateMatches( value, value, query, processor );
    }

    internal static IEnumerable<TNode> SelectInternal( in TNode value, TNode root, string query, NodeProcessorDelegate<TNode> processor = null )
    {
        return EnumerateMatches( value, root, query, processor );
    }

    private static IEnumerable<TNode> EnumerateMatches( in TNode value, in TNode root, string query, NodeProcessorDelegate<TNode> processor = null )
    {
        if ( string.IsNullOrWhiteSpace( query ) ) // invalid per the RFC ABNF
            return []; // Consensus: return empty array for empty query

        if ( query == "$" || query == "@" ) // quick out for everything
            return [value];

        var segmentNext = JsonPathQueryParser.Parse( query ).Next; // The first segment is always the root; skip it

        return EnumerateMatches( root, new NodeArgs( default, value, default, segmentNext, NodeFlags.Default ), processor );
    }

    private static IEnumerable<TNode> EnumerateMatches( TNode root, NodeArgs args, NodeProcessorDelegate<TNode> processor = null )
    {
        var stack = new NodeArgsStack();

        var (accessor, filterEvaluator) = Descriptor;

        do
        {
            // deconstruct next args

            var (parent, value, key, segmentNext, flags) = args;

            // call node processor if it exists and the `key` is not null.
            // the key is null when a descent has re-pushed the descent target.
            // this should be safe to skip; we will see its values later.

            if ( key != null )
                processor?.Invoke( parent, value, key, segmentNext );

            // yield matches
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

            if ( segmentCurrent.IsSingular )
            {
                if ( nodeKind == NodeKind.Object && selectorKind == SelectorKind.Index )
                    continue; // don't allow indexing in to objects

                if ( accessor.TryGetChildValue( value, selector, out var childValue ) )
                {
                    stack.Push( value, childValue, selector, segmentNext );
                }

                continue;
            }

            //// wildcard

            //if ( selectorKind == SelectorKind.Wildcard )
            //{
            //    foreach ( var (childValue, childKey, childKind) in accessor.EnumerateChildren( value ) )
            //    {
            //        // optimization: quicker return for final 
            //        //
            //        // the parser will work without this check, but we would be forcing it
            //        // to push and pop values onto the stack that we know will not be used.
            //        if ( segmentNext.IsFinal )
            //        {
            //            // we could just yield here, but we can't because we want to preserve
            //            // the order of the results as per the RFC. so we push the current
            //            // value onto the stack without prepending the childKey or childKind
            //            // to set up for an immediate return on the next iteration.
            //            //Push( stack, value, childValue, childKey, segmentNext );
            //            stack.Push( value, childValue, childKey, segmentNext );
            //            continue;
            //        }

            //        stack.Push( parent, value, childKey, segmentNext.Prepend( childKey, childKind ) ); // (Name | Index)
            //    }

            //    continue;
            //}

            // descendant

            if ( selectorKind == SelectorKind.Descendant )
            {
                foreach ( var (childValue, childKey, _) in accessor.EnumerateChildren( value, includeValues: false ) ) // child arrays or objects only
                {
                    stack.Push( value, childValue, childKey, segmentCurrent ); // Descendant
                }

                // Union Processing After Descent: If a union operator immediately follows a
                // descendant operator, the union should only process simple values. This is
                // to prevent duplication of complex objects that would result from both the
                // current node and the union processing the same items.

                stack.Push( parent, value, null, segmentNext, NodeFlags.AfterDescent ); // process the current value
                continue;
            }

            // group

            for ( var i = 0; i < segmentCurrent.Selectors.Length; i++ ) // using 'for' for performance
            {
                if ( i != 0 )
                    (selector, selectorKind) = segmentCurrent.Selectors[i];


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
                            // we could just yield here, but we can't because we want to preserve
                            // the order of the results as per the RFC. so we push the current
                            // value onto the stack without prepending the childKey or childKind
                            // to set up for an immediate return on the next iteration.
                            //Push( stack, value, childValue, childKey, segmentNext );
                            stack.Push( value, childValue, childKey, segmentNext );
                            continue;
                        }

                        stack.Push( parent, value, childKey, segmentNext.Prepend( childKey, childKind ) ); // (Name | Index)
                    }

                    continue;
                }

                // [?exp]

                if ( selectorKind == SelectorKind.Filter )
                {
                    foreach ( var (childValue, childKey, childKind) in accessor.EnumerateChildren( value ) )
                    {
                        var result = filterEvaluator.Evaluate( selector[1..], childValue, root ); // remove the leading '?' character

                        if ( !Truthy( result ) )
                            continue;

                        // optimization: quicker return for tail values
                        if ( segmentNext.IsFinal )
                        {
                            stack.Push( value, childValue, childKey, segmentNext );
                            continue;
                        }

                        stack.Push( parent, value, childKey, segmentNext.Prepend( childKey, childKind ) ); // (Name | Index)
                    }

                    continue;
                }

                // array [name1,name2,...] or [#,#,...] or [start:end:step]

                if ( nodeKind == NodeKind.Array )
                {
                    // [#,#,...] 

                    if ( selectorKind == SelectorKind.Index )
                    {
                        stack.Push( value, accessor.GetElementAt( value, int.Parse( selector ) ), selector, segmentNext );
                        continue;
                    }

                    // [start:end:step] Python slice syntax

                    if ( selectorKind == SelectorKind.Slice )
                    {
                        foreach ( var index in EnumerateSlice( value, selector, accessor ) )
                        {
                            stack.Push( value, accessor.GetElementAt( value, index ), index.ToString(), segmentNext );
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

                        stack.Push( value, childValue, index.ToString(), indexSegment );
                    }

                    continue;
                }

                // object [name1,name2,...]

                if ( nodeKind == NodeKind.Object )
                {
                    if ( selectorKind == SelectorKind.Slice || selectorKind == SelectorKind.Index )
                        continue;

                    if ( accessor.TryGetChildValue( value, selector, out var childValue ) )
                    {
                        stack.Push( value, childValue, selector, segmentNext );
                    }
                }
            }

        } while ( stack.TryPop( out args ) );
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

        var (lower, upper, step) = JsonPathSliceSyntaxHelper.ParseExpression( sliceExpr, length, reverse: true );

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

    [DebuggerDisplay( "Parent = {Parent}, Value = {Value}, First = ({Segment?.Selectors?[0]}), IsSingular = {Segment?.IsSingular}, Count = {Segment?.Selectors?.Length}" )]
    private record struct NodeArgs( TNode Parent, TNode Value, string Key, JsonPathSegment Segment, NodeFlags Flags );

    private sealed class NodeArgsStack( int capacity = 16 )
    {
        private readonly Stack<NodeArgs> _stack = new( capacity );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public void Push( in TNode parent, in TNode value, string key, in JsonPathSegment segment, NodeFlags flags = NodeFlags.Default )
        {
            _stack.Push( new NodeArgs( parent, value, key, segment, flags ) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public bool TryPop( out NodeArgs args )
        {
            return _stack.TryPop( out args );
        }
    }
}
