using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Internal;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class SelectExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo itemContext, FilterContext<TNode> context )
    {
        expression = ExpressionHelper<TNode>.GetExpression( state.Item, context, ref itemContext );

        if ( expression == null )
            return false;

        itemContext.Kind = ExpressionKind.Select;
        return true;
    }

    static class ExpressionHelper<TNode>
    {
        private static readonly Expression SelectExpression = Expression.Constant( (Func<TNode, TNode, string, bool, INodeType>) Select );

        public static Expression GetExpression( ReadOnlySpan<char> item, FilterContext<TNode> context, ref ExpressionInfo expressionInfo )
        {
            if ( item.IsEmpty )
                return null;

            if ( item[0] != '$' && item[0] != '@' )
                return null;

            expressionInfo.NonSingularQuery = QueryHelper.IsNonSingular( item ); //BF nsq

            var queryExp = Expression.Constant( item.ToString() );
            var nonSingularQueryExp = Expression.Constant( expressionInfo.NonSingularQuery );

            if ( item[0] == '$' ) // Current becomes root
                context = context with { Current = context.Root };

            return Expression.Invoke( SelectExpression, context.Current, context.Root, queryExp, nonSingularQueryExp );
        }

        private static INodeType Select( TNode current, TNode root, string query, bool nonSingularQuery )
        {
            return new NodesType<TNode>( JsonPath<TNode>.SelectInternal( current, root, query ), nonSingularQuery );
        }

    }
}
