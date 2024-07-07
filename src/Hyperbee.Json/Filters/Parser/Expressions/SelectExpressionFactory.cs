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
        private static readonly Expression SelectExpression = Expression.Constant( (Func<TNode, TNode, string, IEnumerable<TNode>>) Select );

        public static Expression GetExpression( ReadOnlySpan<char> item, FilterContext<TNode> context )
        {
            if ( item.IsEmpty )
                return null;

            if ( item[0] != '$' && item[0] != '@' )
                return null;

            var queryExp = Expression.Constant( item.ToString() );

            if ( item[0] == '$' ) // Current becomes root
                context = context with { Current = context.Root };

            return Expression.Invoke( SelectExpression, context.Current, context.Root, queryExp );
        }

        private static IEnumerable<TNode> Select( TNode current, TNode root, string query )
        {
            var group = FilterParser.IsNonSingularQuery( query ); //bsf
            return JsonPath<TNode>.SelectInternal( current, root, query );
        }
    }
}
