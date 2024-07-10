﻿using System.Collections.Concurrent;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Filters.Parser;
using Microsoft.CSharp.RuntimeBinder;

namespace Hyperbee.Json.Filters;

public sealed class FilterEvaluator<TNode> : IFilterEvaluator<TNode>
{
    private static readonly ConcurrentDictionary<string, Func<FilterRuntimeContext<TNode>, bool>> Compiled = new();

    private readonly ITypeDescriptor<TNode> _typeDescriptor;

    public FilterEvaluator( ITypeDescriptor<TNode> typeDescriptor )
    {
        _typeDescriptor = typeDescriptor;
    }

    public object Evaluate( string filter, TNode current, TNode root )
    {
        // TODO: split type descriptor into design/parse and runtime.  (functions and json parsing are design time)
        var compiled = Compiled.GetOrAdd( filter, _ => FilterParser<TNode>.Compile( filter, _typeDescriptor ) );

        try
        {
            // TODO: get NonSingular from segments
            var runtimeContext = new FilterRuntimeContext<TNode>( current, root, _typeDescriptor, NonSingular: false );
            return compiled( runtimeContext );
        }
        catch ( RuntimeBinderException )
        {
            return null; // missing members should act falsy
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

