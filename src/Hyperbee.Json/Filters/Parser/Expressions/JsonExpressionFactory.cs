using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class JsonExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo exprInfo, ITypeDescriptor<TNode> descriptor )
    {
        if ( descriptor.Accessor.TryParseNode( state.Item.ToString(), out var node ) )
        {
            expression = Expression.Constant( new NodeList<TNode>( [node], isNormalized: true ) );
            exprInfo.Kind = ExpressionKind.Json;
            return true;
        }

        expression = null;
        return false;
    }
}
