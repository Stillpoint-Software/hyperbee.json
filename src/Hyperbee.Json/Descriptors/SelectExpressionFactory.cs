using System.Linq.Expressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors;

internal abstract class FilterExpressionFactory
{
    internal abstract Expression GetExpression( ref ParserState state, FilterContext executionContext );
}

internal class SelectExpressionFactory<TNode> : FilterExpressionFactory
{
    private static readonly Expression SelectExpression = Expression.Constant( (Func<TNode, TNode, string, IEnumerable<TNode>>) Select );

    internal override Expression GetExpression( ref ParserState state, FilterContext executionContext )
    {
        var queryExp = Expression.Constant( state.Item.ToString() );

        if ( state.Item[0] == '$' ) // Current becomes root
            executionContext = executionContext with { Current = executionContext.Root };

        return Expression.Invoke( SelectExpression, executionContext.Current, executionContext.Root, queryExp );
    }

    public static IEnumerable<TNode> Select( TNode current, TNode root, string query )
    {
        return JsonPath<TNode>.SelectInternal( current, root, query );
    }
}
