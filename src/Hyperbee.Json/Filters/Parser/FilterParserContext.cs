using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser;

internal record FilterParserContext<TNode>( ITypeDescriptor<TNode> Descriptor )
{
    public ParameterExpression RuntimeContext { get; init; } = Expression.Parameter( typeof( FilterRuntimeContext<TNode> ), "runtimeContext" );
}


public record FilterRuntimeContext<TNode>( TNode Current, TNode Root, ITypeDescriptor<TNode> Descriptor );
