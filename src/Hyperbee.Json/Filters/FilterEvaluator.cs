using System.Collections.Concurrent;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Filters.Parser;
using Microsoft.CSharp.RuntimeBinder;

namespace Hyperbee.Json.Filters;

public sealed class FilterEvaluator<TNode> : IFilterEvaluator<TNode>
{
    private readonly IJsonTypeDescriptor _typeDescriptor;

    // ReSharper disable once StaticMemberInGenericType
    private static readonly ConcurrentDictionary<string, Func<TNode, TNode, bool>> Compiled = new();

    public FilterEvaluator( ITypeDescriptor<TNode> typeDescriptor )
    {
        _typeDescriptor = typeDescriptor;
    }

    public object Evaluate( string filter, TNode current, TNode root )
    {
        var compiled = Compiled.GetOrAdd( filter, _ => FilterExpressionParser.Compile<TNode>( filter, _typeDescriptor ) );

        try
        {
            return compiled( current, root );
        }
        catch ( RuntimeBinderException )
        {
            return null; // missing members should act falsy
        }
        catch ( Exception ex )
        {
            throw new FilterEvaluatorException( "Error compiling JsonPath expression.", ex );
        }
    }
}

