using System.Collections.Immutable;
using System.Globalization;
using Hyperbee.Json.Evaluators;
using Hyperbee.Json.Memory;
using Hyperbee.Json.Tokenizer;

namespace Hyperbee.Json;

public abstract class JsonPathVisitorBase<TElement>
{
    internal IEnumerable<TElement> ExpressionVisitor( VisitorArgs args, JsonPathEvaluator<TElement> evaluator )
    {
        var initialArgs = args;

        var nodes = new Stack<VisitorArgs>( 4 );
        void PushNode( in TElement v, in IImmutableStack<JsonPathToken> t ) => nodes.Push( new VisitorArgs( v, initialArgs.Root, t ) );

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
            var selector = token.Selectors.First().Value;

            // make sure we have a container value

            if ( !IsObjectOrArray( current ) )
                throw new InvalidOperationException( "Object or Array expected." );

            // try to access object or array using KEY value

            if ( token.Singular )
            {
                if ( TryGetChildValue( current, selector, out var childValue ) )
                    PushNode( childValue, tokens );

                continue;
            }

            // wildcard

            if ( selector == "*" )
            {
                foreach ( var (_, childKey) in EnumerateChildValues( current ) )
                {
                    PushNode( current, tokens.Push( new JsonPathToken( childKey, SelectorKind.UnspecifiedSingular ) ) ); // (Dot | Index)
                }

                continue;
            }

            // descendant

            if ( selector == ".." )
            {
                foreach ( var (childValue, _) in EnumerateChildValues( current ) )
                {
                    if ( IsObjectOrArray( childValue ) )
                        PushNode( childValue, tokens.Push( new JsonPathToken( "..", SelectorKind.UnspecifiedGroup ) ) ); // Descendant
                }

                PushNode( current, tokens );
                continue;
            }

            // union

            foreach ( var childSelector in token.Selectors.Select( x => x.Value ) )
            {
                // [(exp)]

                if ( childSelector.Length > 2 && childSelector[0] == '(' && childSelector[^1] == ')' )
                {
                    if ( evaluator( childSelector, current, args.Root ) is not string evalSelector )
                        continue;

                    var selectorKind = evalSelector != "*" && evalSelector != ".." && !JsonPathRegex.RegexSlice().IsMatch( evalSelector ) // (Dot | Index) | Wildcard, Descendant, Slice 
                        ? SelectorKind.UnspecifiedSingular
                        : SelectorKind.UnspecifiedGroup;

                    PushNode( current, tokens.Push( new JsonPathToken( evalSelector, selectorKind ) ) );
                    continue;
                }

                // [?(exp)]

                if ( childSelector.Length > 3 && childSelector[0] == '?' && childSelector[1] == '(' && childSelector[^1] == ')' )
                {
                    foreach ( var (childValue, childKey) in EnumerateChildValues( current ) )
                    {
                        var filter = evaluator( JsonPathRegex.RegexPathFilter().Replace( childSelector, "$1" ), childValue, args.Root );

                        // treat the filter result as truthy if the evaluator returned a non-convertible object instance. 
                        if ( filter is not null and not IConvertible || Convert.ToBoolean( filter, CultureInfo.InvariantCulture ) )
                            PushNode( current, tokens.Push( new JsonPathToken( childKey, SelectorKind.UnspecifiedSingular ) ) ); // (Name | Index)
                    }

                    continue;
                }

                // [name1,name2,...] or [#,#,...] or [start:end:step]

                if ( IsArray( current, out var length ) )
                {
                    if ( JsonPathRegex.RegexNumber().IsMatch( childSelector ) )
                    {
                        // [#,#,...] 
                        PushNode( GetElementAt( current, int.Parse( childSelector ) ), tokens );
                        continue;
                    }

                    // [start:end:step] Python slice syntax
                    if ( JsonPathRegex.RegexSlice().IsMatch( childSelector ) )
                    {
                        foreach ( var index in EnumerateSlice( current, childSelector ) )
                            PushNode( GetElementAt( current, index ), tokens );
                        continue;
                    }

                    // [name1,name2,...]
                    foreach ( var index in EnumerateArrayIndices( length ) )
                        PushNode( GetElementAt( current, index ), tokens.Push( new JsonPathToken( childSelector, SelectorKind.UnspecifiedSingular ) ) ); // Name

                    continue;
                }

                // [name1,name2,...]

                if ( IsObject( current ) )
                {
                    if ( JsonPathRegex.RegexSlice().IsMatch( childSelector ) || JsonPathRegex.RegexNumber().IsMatch( childSelector ) )
                        continue;

                    // [name1,name2,...]
                    if ( TryGetChildValue( current, childSelector, out var childValue ) )
                        PushNode( childValue, tokens );
                }
            }

        } while ( nodes.TryPop( out args ) );
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
    
    internal sealed class VisitorArgs( in TElement value, in TElement root, in IImmutableStack<JsonPathToken> tokens )
    {
        public readonly TElement Value = value;
        public readonly TElement Root = root;
        public readonly IImmutableStack<JsonPathToken> Tokens = tokens;

        public void Deconstruct( out TElement value, out IImmutableStack<JsonPathToken> tokens )
        {
            value = Value;
            tokens = Tokens;
        }
    }
}
