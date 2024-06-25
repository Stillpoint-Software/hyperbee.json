using System.Linq.Expressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors;

public class SelectFunction<TNode> : FilterFunction
{
    private static readonly Expression SelectExpression = Expression.Constant( (Func<TNode, TNode, string, IEnumerable<TNode>>) Select );

    public override Expression GetExpression( ref ParserState state, FilterContext context )
    {
        var queryExp = Expression.Constant( state.Item.ToString() );

        if ( state.Item[0] == '$' ) // Current becomes root
            context = context with { Current = context.Root };

        return Expression.Invoke( SelectExpression, context.Current, context.Root, queryExp );
    }

    public static IEnumerable<TNode> Select( TNode current, TNode root, string query )
    {
        return JsonPath<TNode>.SelectInternal( current, root, query );
    }
}
