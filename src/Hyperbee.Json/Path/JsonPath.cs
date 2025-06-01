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

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Hyperbee.Json.Core;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Path.Filters;
using Hyperbee.Json.Query;

// https://www.rfc-editor.org/rfc/rfc9535.html
// https://www.rfc-editor.org/rfc/rfc9535.html#appendix-A

namespace Hyperbee.Json.Path;

internal static class IndexHelper
{
    private const int LookupLength = 64;
    private static readonly string[] IndexLookup = Enumerable.Range( 0, LookupLength ).Select( i => i.ToString() ).ToArray();

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public static string GetIndexString( int index ) => index < 64 ? IndexLookup[index] : index.ToString();
}

public delegate void NodeProcessorDelegate<TNode>( in TNode parent, in TNode value, string key, in JsonSegment segment );

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
        var compiledQuery = JsonQueryParser.Parse( query );
        return EnumerateMatches( value, value, compiledQuery, processor );
    }

    internal static IEnumerable<TNode> SelectInternal( in TNode value, in TNode root, JsonQuery compiledQuery, NodeProcessorDelegate<TNode> processor = null )
    {
        // entry point for filter recursive calls

        // explicitly allow dot whitespace for function arguments. This is annoying
        // because the RFC ABNF does not allow whitespace in the query for dot member
        // notation, but it does allow it in the filter for function arguments.

        return EnumerateMatches( value, root, compiledQuery, processor );
    }

    private static IEnumerable<TNode> EnumerateAllDescendants( TNode node, NodeProcessorDelegate<TNode> processor )
    {
        var kind = Descriptor.ValueAccessor.GetNodeKind( node );
        
        if ( kind != NodeKind.Object && kind != NodeKind.Array )
        {
            yield return node;
            yield break;
        }

        var stack = new Stack<TNode>( 8 );
        var current = node;

        do
        {
            // ..
            foreach ( var child in Descriptor.NodeActions.GetChildren( current, ChildEnumerationOptions.Reverse | ChildEnumerationOptions.ComplexTypesOnly ) )
                stack.Push( child );

            // *
            if ( processor == null )
            {
                foreach ( var child in Descriptor.NodeActions.GetChildren( current, ChildEnumerationOptions.None ) )
                    yield return child;
            }
            else
            {
                foreach ( var (child, key) in Descriptor.NodeActions.GetChildrenWithName( current, ChildEnumerationOptions.None ) )
                {
                    processor.Invoke( current, child, key, default ); 
                    yield return child;
                }
            }

        } while ( stack.TryPop( out current ) );
    }

    private static IEnumerable<TNode> EnumerateMatches( in TNode value, in TNode root, JsonQuery compiledQuery, NodeProcessorDelegate<TNode> processor = null )
    {
        if ( string.IsNullOrWhiteSpace( compiledQuery.Query ) ) // invalid per the RFC ABNF
            return []; // Consensus: return empty array for empty query

        if ( compiledQuery.Query == "$" || compiledQuery.Query == "@" ) // quick out for everything
            return [value];

        if ( compiledQuery.Query == "$..*" ) // Fast path for $..*
            return EnumerateAllDescendants( value, processor );

        var segmentNext = compiledQuery.Segments.Next; // The first segment is always the root; skip it

        if ( compiledQuery.Normalized ) // we can fast path this
        {
            if ( Descriptor.NodeActions.TryGetFromPointer( in value, segmentNext, out var result ) )
                return [result];

            return [];
        }

        return EnumerateMatches( root, new NodeArgs( default, value, default, segmentNext, NodeFlags.Default ), processor );
    }

    private static IEnumerable<TNode> EnumerateMatches( TNode root, NodeArgs args, NodeProcessorDelegate<TNode> processor = null )
    {
        using var stack = new NodeArgsStack( 32 );
        var accessor = Descriptor.ValueAccessor;

        do
        {
            // deconstruct next args

            var (parent, value, key, segmentNext, flags) = args;

            ProcessArgs:
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
            
            var selectors = segmentCurrent.Selectors;
            var (selector, selectorKind) = selectors[0];

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
                {
                    // optimization: quicker return for final
                    if ( segmentNext.IsFinal )
                    {
                        processor?.Invoke( in value, childValue, selector, segmentNext );
                        yield return childValue;
                        continue;
                    }

                    // optimization: avoid immediate push pop
                    //
                    // replaces stack.Push( value, childValue, selector, segmentNext );

                    (parent, value, key, segmentNext, flags) = (value, childValue, selector, segmentNext, NodeFlags.Default);
                    goto ProcessArgs;
                }

                continue;
            }

            // group selector

            var selectorCount = selectors.Length;

            for ( var i = 0; i < selectorCount; i++ ) // using 'for' for performance
            {
                if ( i > 0 ) // we already have the first selector
                    (selector, selectorKind) = selectors[i];

                switch ( selectorKind )
                {
                    // descendant
                    case SelectorKind.Descendant:
                    {
                        var children = Descriptor.NodeActions.GetChildrenWithName( value, ChildEnumerationOptions.Reverse | ChildEnumerationOptions.ComplexTypesOnly );
                        stack.PushMany( value, children, segmentCurrent, NodeFlags.AfterDescent );

                        // Union Processing After Descent: If a union operator immediately follows a
                        // descendant operator, the union should only process simple values. This is
                        // to prevent duplication of complex objects that would result from both the
                        // current node and the union processing the same items.

                        // optimization: avoid immediate push pop
                        //
                        // this is safe because descendant only ever has one selector.
                        // replaces stack.Push( value, childValue, selector, segmentNext );

                        (parent, value, key, segmentNext, flags) = (parent, value, null, segmentNext, NodeFlags.AfterDescent); // process the current value
                        goto ProcessArgs;
                    }

                    // wildcard
                    case SelectorKind.Wildcard:
                    {
                        var childKind = GetSelectorKind( nodeKind );

                        foreach ( var (childValue, childKey) in Descriptor.NodeActions.GetChildrenWithName( value, ChildEnumerationOptions.Reverse ) )
                        {
                            // optimization: quicker return for final 
                            //
                            // the parser will work without this check, but we would be forcing it
                            // to push and pop values onto the stack that we know will not be used.
                            if ( segmentNext.IsFinal )
                            {
                                // if we didn't care about RFC order, we could just yield here. to
                                // order of the results as per the RFC we need to push the current
                                // value onto the stack without prepending the childKey or childKind
                                // to set up for an immediate return on the next iteration.
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
                        var childKind = GetSelectorKind( nodeKind );

                        foreach ( var (childValue, childKey) in Descriptor.NodeActions.GetChildrenWithName( value, ChildEnumerationOptions.Reverse ) )
                        {
                            if ( !FilterRuntime.Evaluate( selector[1..], childValue, root ) ) // remove the leading '?' character
                                continue;

                            // optimization: quicker return for tail values
                            if ( segmentNext.IsFinal )
                            {
                                // yielding would not preserve the order of the results as per the RFC.
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

                        if ( accessor.TryGetIndexAt( value, int.Parse( selector ), out var childValue ) )
                        {
                            stack.Push( value, childValue, selector, segmentNext );
                        }

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
                            var childValue = accessor.IndexAt( value, index );
                            stack.Push( value, childValue, index, segmentNext );
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
                            var childValue = accessor.IndexAt( value, index );

                            if ( flags == NodeFlags.AfterDescent && accessor.GetNodeKind( childValue ) != NodeKind.Value )
                                continue;

                            stack.Push( value, childValue, index, indexSegment );
                        }

                        continue;
                    }

                    // Object: [name1,name2,...] Names over object
                    case SelectorKind.Name when nodeKind == NodeKind.Object:
                    {
                        if ( accessor.TryGetProperty( value, selector, out var childValue ) )
                        {
                            stack.Push( value, childValue, selector, segmentNext );
                        }

                        continue;
                    }

                    default:
                    {
                        throw new NotSupportedException( $"Unsupported {nameof(SelectorKind)}." );
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
                if ( accessor.TryGetProperty( value, childSelector, out childValue ) )
                    return true;
                break;

            case NodeKind.Array when selectorKind == SelectorKind.Index:
                if ( int.TryParse( childSelector, out var index ) && accessor.TryGetIndexAt( value, index, out childValue ) )
                    return true;
                break;
        }

        childValue = default;
        return false;
    }

    private static (int Upper, int Lower, int Step) GetSliceRange( IValueAccessor<TNode> accessor, in TNode value, string sliceExpr )
    {
        var length = accessor.GetArrayLength( value );

        if ( length == 0 )
            return (0, 0, 0);

        var (lower, upper, step) = SliceSyntaxHelper.ParseExpression( sliceExpr, length, reverse: true );

        if ( step < 0 )
            (lower, upper) = (upper, lower);

        return (upper, lower, step);
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static SelectorKind GetSelectorKind( NodeKind nodeKind )
    {
        return nodeKind switch
        {
            NodeKind.Object => SelectorKind.Name,
            NodeKind.Array => SelectorKind.Index,
            _ => throw new ArgumentOutOfRangeException( nameof(nodeKind), nodeKind, $"{nameof(NodeKind)} must be an object or an array." )
        };
    }

    private readonly record struct NodeArgs( in TNode Parent, in TNode Value, string Key, JsonSegment Segment, NodeFlags Flags );

    [DebuggerDisplay( "{_array}" )]
    private sealed class NodeArgsStack : IDisposable
    {
        [DebuggerBrowsable( DebuggerBrowsableState.RootHidden )]
        private NodeArgs[] _array;
        private int _count;
        private bool _disposed;

        public NodeArgsStack(int capacity = 16)
        {
            _array = ArrayPool<NodeArgs>.Shared.Rent(capacity);
            _count = 0;
            _disposed = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(in TNode parent, in TNode value, string key, in JsonSegment segment, NodeFlags flags = NodeFlags.Default)
        {
            EnsureNotDisposed();

            if (_count == _array.Length)
                Grow();

            _array[_count++] = new NodeArgs(parent, value, key, segment, flags);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(in TNode parent, in TNode value, int index, in JsonSegment segment, NodeFlags flags = NodeFlags.Default)
        {
            Push(parent, value, IndexHelper.GetIndexString(index), segment, flags);
        }

        public void PushMany( in TNode parent, in IEnumerable<(TNode Value, string Key)> items, in JsonSegment segment, NodeFlags flags = NodeFlags.Default )
        {
            EnsureNotDisposed();

            foreach ( var (value, key) in items )
                Push( parent, value, key, segment, flags );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPop( out NodeArgs args )
        {
            EnsureNotDisposed();

            if ( _count == 0 )
            {
                args = default;
                return false;
            }

            var i = --_count;
            
            args = _array[i];
            //_array[i] = default; // clear entry

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Grow()
        {
            int newSize = _array.Length * 2;
            var newArray = ArrayPool<NodeArgs>.Shared.Rent(newSize);
            Array.Copy(_array, newArray, _array.Length);
            ArrayPool<NodeArgs>.Shared.Return(_array, clearArray: true);
            _array = newArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureCapacity(int min)
        {
            if ( _array.Length >= min )
                return;

            var newSize = Math.Max(_array.Length * 2, min);
            var newArray = ArrayPool<NodeArgs>.Shared.Rent(newSize);
            Array.Copy(_array, newArray, _array.Length);
            ArrayPool<NodeArgs>.Shared.Return(_array, clearArray: true);
            _array = newArray;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        private void EnsureNotDisposed()
        {
            if ( !_disposed )
                return;

            throw new ObjectDisposedException(nameof(NodeArgsStack));
        }

        public void Dispose()
        {
            if ( _disposed )
                return;

            ArrayPool<NodeArgs>.Shared.Return(_array, clearArray: true);
            _array = null;
            _disposed = true;
        }
    }

    //private sealed class NodeArgsStack( int capacity = 8 )
    //{
    //    [DebuggerBrowsable( DebuggerBrowsableState.RootHidden )]
    //    private readonly Stack<NodeArgs> _stack = new(capacity);

    //    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    //    public void Push( in TNode parent, in TNode value, string key, in JsonSegment segment, NodeFlags flags = NodeFlags.Default )
    //    {
    //        _stack.Push( new NodeArgs( parent, value, key, segment, flags ) );
    //    }

    //    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    //    public void Push( in TNode parent, in TNode value, int index, in JsonSegment segment, NodeFlags flags = NodeFlags.Default )
    //    {
    //        _stack.Push( new NodeArgs( parent, value, IndexHelper.GetIndexString( index ), segment, flags ) );
    //    }

    //    public void PushMany( in TNode parent, in IEnumerable<(TNode Value, string Key)> items, in JsonSegment segment, NodeFlags flags = NodeFlags.Default )
    //    {
    //        foreach ( var (value, key) in items )
    //        {
    //            _stack.Push( new NodeArgs( parent, value, key, segment, flags ) );
    //        }
    //    }

    //    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    //    public bool TryPop( out NodeArgs args )
    //    {
    //        return _stack.TryPop( out args );
    //    }
    //}
}


