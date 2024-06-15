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

public static class JsonPath<TNode>
{
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

        var segments = JsonPathQueryTokenizer.Tokenize( query );

        if ( !segments.IsEmpty )
        {
            var selector = segments.Selectors[0].Value; // first selector in segment

            if ( selector == "$" || selector == "@" )
                segments = segments.Next;
        }

        return EnumerateMatches( root, new NodeArgs( value, segments ) );
    }

    private static IEnumerable<TNode> EnumerateMatches( TNode root, NodeArgs args )
    {
        var stack = new Stack<NodeArgs>( 16 );

        var (accessor, filterEvaluator) = Descriptor;

        do
        {
            // deconstruct the next args node

            var (current, segments) = args;

            if ( segments.IsEmpty )
            {
                yield return current;
                continue;
            }

            // get the current segment, and then move the segments
            // reference to the next segment in the list

            var segment = segments; // get current segment
            var (selector, selectorKind) = segment.Selectors[0]; // first selector in segment;

            segments = segments.Next;

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

            if ( selectorKind == SelectorKind.Wildcard )
            {
                foreach ( var (_, childKey) in accessor.EnumerateChildren( current ) )
                {
                    Push( stack, current, segments.Insert( childKey, GetSelectorKindNameOrIndex( childKey ) ) ); // (Name | Index)
                }

                continue;
            }

            // descendant

            if ( selectorKind == SelectorKind.Descendant )
            {
                foreach ( var (childValue, _) in accessor.EnumerateChildren( current, includeValues: false ) ) // child arrays or objects only
                {
                    Push( stack, childValue, segments.Insert( "..", SelectorKind.Descendant ) ); // Descendant
                }

                Push( stack, current, segments );
                continue;
            }

            // union

            for ( var i = 0; i < segment.Selectors.Length; i++ ) // using 'for' for performance
            {
                var (unionSelector,unionSelectorKind) = segment.Selectors[i]; 

                // [(exp)]
                
                /* //BF keep until we validate [(<expr>)] path is not needed
                 
                if ( childSelector.Length > 2 && childSelector[0] == '(' && childSelector[^1] == ')' )
                {
                    if ( filterEvaluator.Evaluate( childSelector, current, root ) is not string filterSelector )
                        continue;

                    var filterSelectorKind = GetSelectorKind( filterSelector );

                    Push( stack, current, segments.Insert( filterSelector, filterSelectorKind ) );
                    continue;
                }
                
                */
                
                // [?exp]

                if ( unionSelectorKind == SelectorKind.Filter )
                {
                    foreach ( var (childValue, childKey) in accessor.EnumerateChildren( current ) )
                    {
                        var filterSelector = JsonPathRegex.RegexPathFilter().Replace( unionSelector, "$1" ); // remove '?(' and ')' //BF: should this be the evaluator's responsibility?
                        var filterValue = filterEvaluator.Evaluate( filterSelector, childValue, root );

                        if ( Truthy( filterValue ) )
                            Push( stack, current, segments.Insert( childKey, GetSelectorKindNameOrIndex( childKey ) ) ); // (Name | Index)
                    }

                    continue;
                }

                // [name1,name2,...] or [#,#,...] or [start:end:step]

                if ( accessor.IsArray( current, out var length ) )
                {
                    if ( unionSelectorKind == SelectorKind.Index )
                    {
                        // [#,#,...] 
                        Push( stack, accessor.GetElementAt( current, int.Parse( unionSelector ) ), segments );
                        continue;
                    }

                    // [start:end:step] Python slice syntax
                    if ( unionSelectorKind == SelectorKind.Slice )
                    {
                        foreach ( var index in EnumerateSlice( current, unionSelector ) )
                            Push( stack, accessor.GetElementAt( current, index ), segments );
                        continue;
                    }

                    // [name1,name2,...]
                    foreach ( var index in EnumerateArrayIndices( length ) )
                        Push( stack, accessor.GetElementAt( current, index ), segments.Insert( unionSelector, SelectorKind.Name ) ); // Name

                    continue;
                }

                // [name1,name2,...]

                if ( accessor.IsObject( current ) )
                {
                    if ( unionSelectorKind == SelectorKind.Slice || unionSelectorKind == SelectorKind.Index )
                        continue;

                    // [name1,name2,...]
                    if ( accessor.TryGetChildValue( current, unionSelector, out var childValue ) )
                        Push( stack, childValue, segments );
                }
            }

        } while ( stack.TryPop( out args ) );

        yield break;

        static void Push( Stack<NodeArgs> n, in TNode v, in JsonPathSegment s ) => n.Push( new NodeArgs( v, s ) );
    }

    /* //BF keep until we validate [(<expr>)] path is not needed
     
    private static SelectorKind GetSelectorKind( string selector )
    {
        if ( selector[0] == '?' )
            return SelectorKind.Filter;

        if ( JsonPathRegex.RegexNumber().IsMatch( selector ) )
            return SelectorKind.Index;

        if ( JsonPathRegex.RegexSlice().IsMatch( selector ) )
            return SelectorKind.Slice;

        return selector switch
        {
            "*" => SelectorKind.Wildcard,
            ".." => SelectorKind.Descendant,
            _ => SelectorKind.Name
        };
    }
    */
    
    private static SelectorKind GetSelectorKindNameOrIndex( string selector )
    {
        return JsonPathRegex.RegexNumber().IsMatch( selector ) ? SelectorKind.Index : SelectorKind.Name;
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

    private sealed class NodeArgs( in TNode value, in JsonPathSegment segment )
    {
        public readonly TNode Value = value;
        public readonly JsonPathSegment Segment = segment;

        public void Deconstruct( out TNode value, out JsonPathSegment segment )
        {
            value = Value;
            segment = Segment;
        }
    }

}
