using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class JsonExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, FilterContext<TNode> context )
    {
        if ( context.Descriptor.Accessor.TryGetNodeList( state.Item.ToString(), out var json ) )
        {
            expression = Expression.Constant( json );
            return true;
        }

        expression = null;
        return false;
    }
}
