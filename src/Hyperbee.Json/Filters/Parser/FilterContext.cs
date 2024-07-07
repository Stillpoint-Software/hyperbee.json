using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser;

internal record FilterContext<TNode>
{
    public FilterContext( ITypeDescriptor<TNode> descriptor )
    {
        Descriptor = descriptor;
    }

    public ParameterExpression Current { get; init; } = Expression.Parameter( typeof( TNode ), "current" );
    public ParameterExpression Root { get; } = Expression.Parameter( typeof( TNode ), "root" );
    public ITypeDescriptor<TNode> Descriptor { get; }

    public bool NonSingularQuery { get; internal set; }
}
