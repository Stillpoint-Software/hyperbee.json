using System.Linq.Expressions;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser;

public static class ComparerBinder<TNode>
{
    private static readonly Expression BindComparerFunc = Expression.Constant( (Func<FilterParserContext<TNode>, IValueType, IValueType>) BindComparer );
    
    internal static Expression BindComparerExpression( FilterParserContext<TNode> parserContext, Expression expression )
    {
        if ( expression == null )
            return null;

        var parserContextExp = Expression.Constant( parserContext );

        return Expression.Invoke( BindComparerFunc, parserContextExp,
            Expression.Convert( expression, typeof( IValueType ) ) );
    }

    internal static IValueType BindComparer( FilterParserContext<TNode> parserContext, IValueType item )
    {
        item.Comparer = parserContext.Descriptor.Comparer;
        return item;
    }
}
