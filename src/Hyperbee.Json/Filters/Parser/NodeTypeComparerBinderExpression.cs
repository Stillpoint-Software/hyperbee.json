using System.Linq.Expressions;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser;

public static class NodeTypeComparerBinderExpression<TNode>
{
    private static readonly Expression BindComparerExpressionConst = Expression.Constant( (Func<FilterParserContext<TNode>, INodeType, INodeType>) BindComparer );
    internal static Expression BindComparerExpression( FilterParserContext<TNode> parserContext, Expression expression )
    {
        if ( expression == null )
            return null;

        var parserContextExp = Expression.Constant( parserContext );

        return Expression.Invoke( BindComparerExpressionConst, parserContextExp,
            Expression.Convert( expression, typeof( INodeType ) ) );
    }

    internal static INodeType BindComparer( FilterParserContext<TNode> parserContext, INodeType item )
    {
        item.Comparer = parserContext.Descriptor.Comparer;
        return item;
    }
}
