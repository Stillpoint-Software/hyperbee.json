using System.Linq.Expressions;
using Hyperbee.Json.Descriptors.Types;
using Hyperbee.Json.Internal;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class SelectExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo itemContext, FilterParserContext<TNode> parserContext )
    {
        expression = ExpressionHelper<TNode>.GetExpression( state.Item, parserContext, ref itemContext );

        if ( expression == null )
            return false;

        itemContext.Kind = ExpressionKind.Select;
        return true;
    }

    static class ExpressionHelper<TNode>
    {
        private static readonly Expression SelectExpression = Expression.Constant( (Func<FilterRuntimeContext<TNode>, string, bool, INodeType>) Select );

        public static Expression GetExpression( ReadOnlySpan<char> item, FilterParserContext<TNode> parserContext, ref ExpressionInfo expressionInfo )
        {
            if ( item.IsEmpty )
                return null;

            if ( item[0] != '$' && item[0] != '@' )
                return null;

            expressionInfo.NonSingularQuery = QueryHelper.IsNonSingular( item ); //BF nsq

            return Expression.Invoke(
                SelectExpression,
                parserContext.RuntimeContext,
                Expression.Constant( item.ToString() ),
                Expression.Constant( expressionInfo.NonSingularQuery ) );
        }

        private static INodeType Select( FilterRuntimeContext<TNode> runtimeContext, string query, bool nonSingular )
        {
            // Current becomes root
            return query[0] == '$'
                ? new NodesType<TNode>( JsonPath<TNode>.SelectInternal( runtimeContext.Root, runtimeContext.Root, query ), nonSingular /*runtimeContext.NonSingular */)
                : new NodesType<TNode>( JsonPath<TNode>.SelectInternal( runtimeContext.Current, runtimeContext.Root, query ), nonSingular /*runtimeContext.NonSingular */ );
        }

    }
}
