using System.Collections.Concurrent;
using Hyperbee.Json.Filters.Parser;
using Microsoft.CSharp.RuntimeBinder;

namespace Hyperbee.Json.Filters;

public record FilterRuntimeContext<TNode>( TNode Current, TNode Root );

public sealed class FilterEvaluator<TNode> : IFilterEvaluator<TNode>
{
    private static readonly ConcurrentDictionary<string, Func<FilterRuntimeContext<TNode>, bool>> Compiled = new();

    public bool Evaluate( string filter, TNode current, TNode root )
    {
        var compiled = Compiled.GetOrAdd( filter, _ => FilterParser<TNode>.Compile( filter ) );

        try
        {
            var runtimeContext = new FilterRuntimeContext<TNode>( current, root );
            return compiled( runtimeContext );
        }
        catch ( RuntimeBinderException )
        {
            return false; // missing members should act falsy
        }
        catch ( NotSupportedException )
        {
            throw;
        }
        catch ( Exception ex )
        {
            throw new FilterException( "Error compiling filter expression.", ex );
        }
    }
}

