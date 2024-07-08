using System.Linq.Expressions;

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

            var query = item.ToString();
            expressionItemContext.NonSingleQuery = IsNonSingularQuery( query ); //BF nsq - identify non-singular-query (nsq) for comparand operations

            var queryExp = Expression.Constant( query );
            var contextExp = Expression.Constant( context );

            if ( item[0] == '$' ) // Current becomes root
                context = context with { Current = context.Root };

            return Expression.Invoke( SelectExpression, context.Current, context.Root, queryExp, contextExp ); //BF nsq - may just want to pass context
        }

        private static IEnumerable<TNode> Select( TNode current, TNode root, string query, FilterContext<TNode> context )
        {
            return JsonPath<TNode>.SelectInternal( current, root, query );
        }

        private static bool IsNonSingularQuery( ReadOnlySpan<char> query ) //BF nsq
        {
            bool inQuotes = false;
            char quoteChar = '\0';

            // Check for any special characters that would indicate a non-singular query

            for ( var i = 0; i < query.Length; i++ )
            {
                char current = query[i];

                if ( inQuotes )
                {
                    if ( current != '\\' && current == quoteChar )
                    {
                        inQuotes = false;
                        quoteChar = '\0';
                    }

                    continue;
                }

                switch ( current )
                {
                    case '\'':
                    case '"':
                        quoteChar = current;
                        inQuotes = true;
                        continue;
                    case '*':
                    case ',':
                    case ':':
                        return true;
                    case '.':
                        if ( i + 1 < query.Length && query[i + 1] == '.' ) // ..
                            return true;
                        break;
                }
            }

            return false;
        }
    }
}
