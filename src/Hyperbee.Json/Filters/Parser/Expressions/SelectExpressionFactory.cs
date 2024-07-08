using System.Linq.Expressions;
using Hyperbee.Json.Internal;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class SelectExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionItemContext expressionItemContext, FilterContext<TNode> context )
    {
        expression = ExpressionHelper<TNode>.GetExpression( state.Item, context, ref expressionItemContext );
        return expression != null;
    }

    static class ExpressionHelper<TNode>
    {
        private static readonly Expression SelectExpression = Expression.Constant( (Func<TNode, TNode, string, FilterContext<TNode>, IEnumerable<TNode>>) Select );

        public static Expression GetExpression( ReadOnlySpan<char> item, FilterContext<TNode> context, ref ExpressionItemContext expressionItemContext )
        {
            if ( item.IsEmpty )
                return null;

            if ( item[0] != '$' && item[0] != '@' )
                return null;

            expressionItemContext.NonSingleQuery = QueryHelper.IsNonSingular( item ); //BF nsq - identify non-singular-query (nsq) for comparand operations

            var queryExp = Expression.Constant( item.ToString() );
            var contextExp = Expression.Constant( context );

            if ( item[0] == '$' ) // Current becomes root
                context = context with { Current = context.Root };

            return Expression.Invoke( SelectExpression, context.Current, context.Root, queryExp, contextExp ); //BF nsq - may just want to pass context
        }

        private static IEnumerable<TNode> Select( TNode current, TNode root, string query, FilterContext<TNode> context )
        {
            return JsonPath<TNode>.SelectInternal( current, root, query );
        }
    }
}
