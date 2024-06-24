using System.Linq.Expressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors;

public class SelectFunction<TNode> : FilterFunction
{
    private static readonly Expression SelectExpression = Expression.Constant( (Func<TNode, TNode, string, IEnumerable<TNode>>) Select );

    public override Expression GetExpression( ReadOnlySpan<char> filter, ReadOnlySpan<char> item, ref int pos, FilterExecutionContext executionContext )
    {
        var queryExp = Expression.Constant( item.ToString() );

        if ( item[0] == '$' ) // Current becomes root
            executionContext = executionContext with { Current = executionContext.Root };

        return Expression.Invoke( SelectExpression, executionContext.Current, executionContext.Root, queryExp );
    }

    public static IEnumerable<TNode> Select( TNode current, TNode root, string query )
    {
        return JsonPath<TNode>.SelectInternal( current, root, query );
    }
}
