using System.Collections.Concurrent;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Filters.Parser;
using Microsoft.CSharp.RuntimeBinder;

namespace Hyperbee.Json.Filters;

public record FilterRuntimeContext<TNode>( TNode Current, TNode Root, ITypeDescriptor<TNode> Descriptor );

public sealed class FilterEvaluator<TNode>( ITypeDescriptor<TNode> descriptor ) : IFilterEvaluator<TNode>
{
    private static readonly ConcurrentDictionary<string, Func<FilterRuntimeContext<TNode>, bool>> Compiled = new();

    public ITypeDescriptor<TNode> Descriptor { get; } = descriptor;

    public bool Evaluate( string filter, TNode current, TNode root )
    {
        // Feature: split type descriptor into design/parse and runtime.  (functions and json parsing are design time)
        var compiled = Compiled.GetOrAdd( filter, _ => FilterParser<TNode>.Compile( filter, Descriptor ) );

        try
        {
            var runtimeContext = new FilterRuntimeContext<TNode>( current, root, Descriptor );
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

