using System.Collections.Immutable;
using System.Globalization;
using Hyperbee.Json.Evaluators;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Memory;
using Hyperbee.Json.Tokenizer;

namespace Hyperbee.Json;

internal abstract class JsonPathVisitorBase<TElement, TResult>
{
    internal IEnumerable<TResult> ExpressionVisitor( VisitorArgs args, JsonPathEvaluator<TElement> evaluator )
    {
        var initialArgs = args;

        var nodes = new Stack<VisitorArgs>( 4 );
        void PushNode( in TElement v, in IImmutableStack<JsonPathToken> t, string p ) => nodes.Push( new VisitorArgs( v, initialArgs.Root, t, p ) );

        do
        {
            // deconstruct the next args node

            var (current, tokens, path) = args;

            if ( tokens.IsEmpty )
            {
                if ( !string.IsNullOrEmpty( path ) )
                    yield return CreateResult( current, path );

                continue;
            }

            // pop the next token from the stack

            tokens = tokens.Pop( out var token );
            var selector = token.Selectors.First().Value;

            // make sure we have a container value

            if ( !IsObjectOrArray( current ) )
                throw new InvalidOperationException( "Object or Array expected." );

            // try to access object or array using KEY value

            if ( token.Singular )
            {
                if ( TryGetChildValue( current, selector, out var childValue ) )
                    PushNode( childValue, tokens, GetPath( current, path, selector ) );

                continue;
            }

            // wildcard

            if ( selector == "*" )
            {
                foreach ( var childKey in EnumerateKeys( current ) )
                    PushNode( current, tokens.Push( childKey, SelectorKind.UnspecifiedSingular ), path ); // (Dot | Index)
                continue;
            }

            // descendant

            if ( selector == ".." )
            {
                foreach ( var (childValue, childKey) in EnumerateChildValues( current ) )
                {
                    if ( IsObjectOrArray( childValue ) )
                        PushNode( childValue, tokens.Push( "..", SelectorKind.UnspecifiedGroup ), GetPath( current, path, childKey ) ); // Descendant
                }


                // foreach ( var childKey in EnumerateKeys( current ) )
                // {
                //     if ( !TryGetChildValue( current, childKey, out var childValue ) )
                //         continue;
                //
                //     if ( IsObjectOrArray( childValue ) )
                //         PushNode( childValue, tokens.Push( "..", SelectorKind.UnspecifiedGroup ), GetPath( current, path, childKey ) ); // Descendant
                // }

                PushNode( current, tokens, path );
                continue;
            }

            // union

            foreach ( var childSelector in token.Selectors.Select( x => x.Value ) )
            {
                // [(exp)]

                if ( childSelector.Length > 2 && childSelector[0] == '(' && childSelector[^1] == ')' )
                {
                    if ( evaluator( childSelector, current, args.Root, GetPath( current, path ) ) is not string evalSelector )
                        continue;

                    var selectorKind = evalSelector != "*" && evalSelector != ".." && !JsonPathRegex.RegexSlice().IsMatch( evalSelector ) // (Dot | Index) | Wildcard, Descendant, Slice 
                        ? SelectorKind.UnspecifiedSingular
                        : SelectorKind.UnspecifiedGroup;

                    PushNode( current, tokens.Push( evalSelector, selectorKind ), path );
                    continue;
                }

                // [?(exp)]

                if ( childSelector.Length > 3 && childSelector[0] == '?' && childSelector[1] == '(' && childSelector[^1] == ')' )
                {
                    foreach ( var (childValue, childKey) in EnumerateChildValues( current ) )
                    {
                        var childContext = GetPath( current, path, childKey );

                        var filter = evaluator( JsonPathRegex.RegexPathFilter().Replace( childSelector, "$1" ), childValue, args.Root, childContext );

                        // treat the filter result as truthy if the evaluator returned a non-convertible object instance. 
                        if ( filter is not null and not IConvertible || Convert.ToBoolean( filter, CultureInfo.InvariantCulture ) )
                            PushNode( current, tokens.Push( childKey, SelectorKind.UnspecifiedSingular ), path ); // (Name | Index)
                    }

                    continue;
                }

                // [name1,name2,...] or [#,#,...] or [start:end:step]

                if ( IsArray( current, out var length ) )
                {
                    if ( JsonPathRegex.RegexNumber().IsMatch( childSelector ) )
                    {
                        // [#,#,...] 
                        PushNode( GetElementAt( current, int.Parse( childSelector ) ), tokens, GetPath( current, path, childSelector ) );
                        continue;
                    }

                    // [start:end:step] Python slice syntax
                    if ( JsonPathRegex.RegexSlice().IsMatch( childSelector ) )
                    {
                        foreach ( var index in EnumerateSlice( current, childSelector ) )
                            PushNode( GetElementAt( current, index ), tokens, GetPath( path, index ) );
                        continue;
                    }

                    // [name1,name2,...]
                    foreach ( var index in EnumerateArrayIndices( length ) )
                        PushNode( GetElementAt( current, index ), tokens.Push( childSelector, SelectorKind.UnspecifiedSingular ), GetPath( path, index ) ); // Name

                    continue;
                }

                // [name1,name2,...]

                if ( IsObject( current ) )
                {
                    if ( JsonPathRegex.RegexSlice().IsMatch( childSelector ) || JsonPathRegex.RegexNumber().IsMatch( childSelector ) )
                        continue;

                    // [name1,name2,...]
                    if ( TryGetChildValue( current, childSelector, out var childValue ) )
                        PushNode( childValue, tokens, GetPath( current, path, childSelector ) );
                }
            }

        } while ( nodes.TryPop( out args ) );
    }

    private static string GetPath( string prefix, int childKey ) => $"{prefix}[{childKey}]";

    internal static IEnumerable<int> EnumerateArrayIndices( int length )
    {
        for ( var index = length - 1; index >= 0; index-- )
            yield return index;
    }

    internal abstract IEnumerable<string> EnumerateKeys( TElement value );

    internal abstract IEnumerable<(TElement, string)> EnumerateChildValues( TElement value );

    internal IEnumerable<int> EnumerateSlice( TElement value, string sliceExpr )
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

    internal abstract TElement GetElementAt( TElement value, int index );

    internal abstract bool IsObjectOrArray( TElement current );
    internal abstract bool IsArray( TElement current, out int length );
    internal abstract bool IsObject( TElement current );

    internal abstract TResult CreateResult( TElement current, string path );

    internal abstract bool TryGetChildValue( in TElement current, ReadOnlySpan<char> childKey, out TElement childValue );

    internal abstract string GetPath( TElement current, string path, string selector );
    internal abstract string GetPath( TElement current, string path );

    internal sealed class VisitorArgs( in TElement value, in TElement root, in IImmutableStack<JsonPathToken> tokens, string path )
    {
        public readonly TElement Value = value;
        public readonly TElement Root = root;
        public readonly IImmutableStack<JsonPathToken> Tokens = tokens;
        public readonly string Path = path;

        public void Deconstruct( out TElement value, out IImmutableStack<JsonPathToken> tokens, out string path )
        {
            value = Value;
            tokens = Tokens;
            path = Path;
        }
    }
}
