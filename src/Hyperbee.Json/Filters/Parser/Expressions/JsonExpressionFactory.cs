using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class JsonExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo expressionInfo, FilterContext<TNode> context )
    {
        if ( context.Descriptor.Accessor.TryParseNode( state.Item.ToString(), out var node ) )
        {
            expression = Expression.Constant( new NodesType<TNode>( [node], false ) );
            expressionInfo.Kind = ExpressionKind.Json;
            return true;
        }

        expression = null;
        return false;
    }
}
