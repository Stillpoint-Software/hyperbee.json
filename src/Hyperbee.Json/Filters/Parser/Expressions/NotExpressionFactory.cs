using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class NotExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, FilterContext<TNode> context )
    {
        expression = null;
        return state.Operator == Operator.Not;
    }
}
