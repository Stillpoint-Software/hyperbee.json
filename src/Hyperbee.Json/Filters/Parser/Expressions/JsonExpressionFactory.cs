using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class JsonExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, FilterContext<TNode> context )
    {
        if ( context.Descriptor.Accessor.TryParseNode( state.Item.ToString(), out var node ) )
        {
            expression = Expression.Constant( new[] { node } );
            return true;
        }

        expression = null;
        return false;
    }
}
