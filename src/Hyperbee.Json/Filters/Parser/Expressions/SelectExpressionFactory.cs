using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class SelectExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, FilterContext<TNode> context )
    {
        expression = ExpressionHelper<TNode>.GetExpression( state.Item, context );
        return expression != null;
    }

    static class ExpressionHelper<TNode>
    {
        private static readonly Expression SelectExpression = Expression.Constant( (Func<TNode, TNode, string, FilterContext<TNode>, IEnumerable<TNode>>) Select );

        public static Expression GetExpression( ReadOnlySpan<char> item, FilterContext<TNode> context )
        {
            if ( item.IsEmpty )
                return null;

            if ( item[0] != '$' && item[0] != '@' )
                return null;

            var queryExp = Expression.Constant( item.ToString() );
            var contextExpr = Expression.Constant( context );

            if ( item[0] == '$' ) // Current becomes root
                context = context with { Current = context.Root };

            return Expression.Invoke( SelectExpression, context.Current, context.Root, queryExp, contextExpr ); //BF may just want to pass context
        }

        private static IEnumerable<TNode> Select( TNode current, TNode root, string query, FilterContext<TNode> context )
        {
            context.IsSingularQuery = FilterParser.IsNonSingularQuery( query ); //BF Thinking this may be the key to identifying if the query is a non-singular or not for comparands
            return JsonPath<TNode>.SelectInternal( current, root, query );
        }
    }
}
