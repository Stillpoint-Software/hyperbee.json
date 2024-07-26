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
using Hyperbee.Json.Filters;

// https://www.rfc-editor.org/rfc/rfc9535.html
// https://www.rfc-editor.org/rfc/rfc9535.html#appendix-A

namespace Hyperbee.Json;

public delegate void NodeProcessorDelegate<TNode>( in TNode parent, in TNode value, string key, in JsonPathSegment segment );

public static class JsonPath<TNode>
{
    private static readonly ITypeDescriptor<TNode> Descriptor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>();
    private static readonly FilterRuntime<TNode> FilterRuntime = new();

    [Flags]
    internal enum NodeFlags
    {
        Default = 0,
        AfterDescent = 1
    }

    public static IEnumerable<TNode> Select( in TNode value, string query, NodeProcessorDelegate<TNode> processor = null )
    {
        var compiledQuery = JsonPathQueryParser.Parse( query );
        return EnumerateMatches( value, value, compiledQuery, processor );
    }

    internal static IEnumerable<TNode> SelectInternal( in TNode value, in TNode root, JsonPathQuery compiledQuery, NodeProcessorDelegate<TNode> processor = null )
    {
        // entry point for filter recursive calls

        // explicitly allow dot whitespace for function arguments. This is annoying
        // because the RFC ABNF does not allow whitespace in the query for dot member
        // notation, but it does allow it in the filter for function arguments.

        return EnumerateMatches( value, root, compiledQuery, processor );
    }

    private static IEnumerable<TNode> EnumerateMatches( in TNode value, in TNode root, JsonPathQuery compiledQuery, NodeProcessorDelegate<TNode> processor = null )
    {
        if ( string.IsNullOrWhiteSpace( compiledQuery.Query ) ) // invalid per the RFC ABNF
            return []; // Consensus: return empty array for empty query

        if ( compiledQuery.Query == "$" || compiledQuery.Query == "@" ) // quick out for everything
            return [value];

        var segmentNext = compiledQuery.Segments.Next; // The first segment is always the root; skip it

        if ( Descriptor.NodeAccessor.CanUsePointer && compiledQuery.Normalized ) // we can fast path this
        {
            if ( Descriptor.NodeAccessor.TryGetFromPointer( in value, segmentNext, out var result ) )
                return [result];

            return [];
        }

        return EnumerateMatches( root, new NodeArgs( default, value, default, segmentNext, NodeFlags.Default ), processor );
    }

    private static IEnumerable<TNode> EnumerateMatches( TNode root, NodeArgs args, NodeProcessorDelegate<TNode> processor = null )
    {
        var stack = new NodeArgsStack();
        var accessor = Descriptor.ValueAccessor;

        do
        {
            // deconstruct next args

            var (parent, value, key, segmentNext, flags) = args;

            // call node processor if it exists and the `key` is not null.
            // the key is null when a descent has re-pushed the descent target.
            // this should be safe to skip; we will see its values later.

            if ( key != null )
                processor?.Invoke( parent, value, key, segmentNext );

            // yield match

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

            // singular selector

            if ( segmentCurrent.IsSingular )
            {
                if ( nodeKind == NodeKind.Object && selectorKind == SelectorKind.Index )
                    continue; // don't allow indexing in to objects

                // try to access object or array using name or index
                if ( TryGetChild( accessor, value, nodeKind, selector, selectorKind, out var childValue ) )
                    stack.Push( value, childValue, selector, segmentNext );

                continue;
            }

            // group selector

            for ( var i = 0; i < segmentCurrent.Selectors.Length; i++ ) // using 'for' for performance
            {
                if ( i > 0 ) // we already have the first selector
                    (selector, selectorKind) = segmentCurrent.Selectors[i];

                switch ( selectorKind )
                {
                    // descendant
                    case SelectorKind.Descendant:
                        {
                            foreach ( var (childValue, childKey, _) in EnumerateChildren( accessor, value, nodeKind, includeValues: false ) ) // child arrays or objects only
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

                    // wildcard
                    case SelectorKind.Wildcard:
                        {
                            foreach ( var (childValue, childKey, childKind) in EnumerateChildren( accessor, value, nodeKind ) )
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
                    case SelectorKind.Filter:
                        {
                            foreach ( var (childValue, childKey, childKind) in EnumerateChildren( accessor, value, nodeKind ) )
                            {
                                if ( !FilterRuntime.Evaluate( selector[1..], childValue, root ) ) // remove the leading '?' character
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

                    // Array: [#,#,...] 
                    case SelectorKind.Index:
                        {
                            if ( nodeKind != NodeKind.Array )
                                continue;

                            if ( accessor.TryGetElementAt( value, int.Parse( selector ), out var childValue ) )
                                stack.Push( value, childValue, selector, segmentNext );
                            continue;
                        }

                    // Array: [start:end:step] Python slice syntax
                    case SelectorKind.Slice:
                        {
                            if ( nodeKind != NodeKind.Array )
                                continue;

                            var (upper, lower, step) = GetSliceRange( accessor, value, selector );

                            for ( var index = lower; step > 0 ? index < upper : index > upper; index += step )
                            {
                                if ( accessor.TryGetElementAt( value, index, out var childValue ) )
                                    stack.Push( value, childValue, index.ToString(), segmentNext );
                            }

                            continue;
                        }

                    // Array: [name1,name2,...] Names over array
                    case SelectorKind.Name when nodeKind == NodeKind.Array:
                        {
                            var indexSegment = segmentNext.Prepend( selector, SelectorKind.Name );
                            var length = accessor.GetArrayLength( value );

                            for ( var index = length - 1; index >= 0; index-- )
                            {
                                if ( !accessor.TryGetElementAt( value, index, out var childValue ) )
                                    continue;

                                if ( flags == NodeFlags.AfterDescent && accessor.GetNodeKind( childValue ) != NodeKind.Value )
                                    continue;

                                stack.Push( value, childValue, index.ToString(), indexSegment );
                            }

                            continue;
                        }

                    // Object: [name1,name2,...] Names over object
                    case SelectorKind.Name when nodeKind == NodeKind.Object:
                        {
                            if ( TryGetChild( accessor, value, nodeKind, selector, selectorKind, out var childValue ) )
                                stack.Push( value, childValue, selector, segmentNext );

                            continue;
                        }

                    default:
                        {
                            throw new NotSupportedException( $"Unsupported {nameof( SelectorKind )}." );
                        }

                } // end switch
            } // end for group selector

        } while ( stack.TryPop( out args ) );
    }

    private static bool TryGetChild( IValueAccessor<TNode> accessor, in TNode value, NodeKind nodeKind, string childSelector, SelectorKind selectorKind, out TNode childValue )
    {
        switch ( nodeKind )
        {
            case NodeKind.Object:
                if ( accessor.TryGetChild( value, childSelector, out childValue ) )
                    return true;

                break;

            case NodeKind.Array:
                if ( selectorKind == SelectorKind.Name )
                    break;

                if ( int.TryParse( childSelector, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index ) )
                {
                    var arrayLength = accessor.GetArrayLength( value );
                    if ( index < 0 ) // flip negative index to positive
                        index = arrayLength + index;

                    if ( index >= 0 && index < arrayLength )
                    {
                        if ( accessor.TryGetElementAt( value, index, out childValue ) )
                            return true;
                    }
                }

                break;
            default:
                if ( !IsPathOperator( childSelector ) )
                    throw new ArgumentException( $"Invalid child type '{childSelector}'. Expected child to be Object, Array or a path selector.", nameof( value ) );

                break;
        }

        childValue = default;
        return false;

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static bool IsPathOperator( ReadOnlySpan<char> x )
        {
            return x.Length switch
            {
                1 => x[0] == '*',
                2 => x[0] == '.' && x[1] == '.',
                3 => x[0] == '$',
                _ => false
            };
        }
    }

    private static IEnumerable<(TNode, string, SelectorKind)> EnumerateChildren( IValueAccessor<TNode> accessor, TNode value, NodeKind nodeKind, bool includeValues = true )
    {
        switch ( nodeKind )
        {
            case NodeKind.Object:
                {
                    var results = new Stack<(TNode, string, SelectorKind)>(); // stack will reverse the list
                    foreach ( var (child, key) in accessor.EnumerateObject( value ) )
                    {
                        var childKind = accessor.GetNodeKind( child );
                        if ( includeValues || childKind is NodeKind.Array or NodeKind.Object )
                            results.Push( (child, key, SelectorKind.Name) );
                    }

                    return results;
                }

            case NodeKind.Array:
                {
                    var length = accessor.GetArrayLength( value );
                    var results = new (TNode, string, SelectorKind)[length];

                    var reverseIndex = length - 1;
                    for ( var index = 0; index < length; index++, reverseIndex-- )
                    {
                        if ( !accessor.TryGetElementAt( value, index, out var child ) )
                            continue;

                        var childKind = accessor.GetNodeKind( child );
                        if ( includeValues || childKind is NodeKind.Array or NodeKind.Object )
                            results[reverseIndex] = (child, index.ToString(), SelectorKind.Index);
                    }

                    return results;
                }
            default:
                return [];
        }
    }

    private static (int Upper, int Lower, int Step) GetSliceRange( IValueAccessor<TNode> accessor, TNode value, string sliceExpr )
    {
        var length = accessor.GetArrayLength( value );

        if ( length == 0 )
            return (0, 0, 0);

        var (lower, upper, step) = JsonPathSliceSyntaxHelper.ParseExpression( sliceExpr, length, reverse: true );

        if ( step < 0 )
            (lower, upper) = (upper, lower);

        return (upper, lower, step);
    }

    [DebuggerDisplay( "Parent = {Parent}, Value = {Value}, {Segment}" )]
    private readonly record struct NodeArgs( TNode Parent, TNode Value, string Key, JsonPathSegment Segment, NodeFlags Flags );

    [DebuggerDisplay( "{_stack}" )]
    private sealed class NodeArgsStack( int capacity = 8 )
    {
        [DebuggerBrowsable( DebuggerBrowsableState.RootHidden )]
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
