using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class NotExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo expressionInfo, FilterParserContext<TNode> parserContext )
    {
        expression = null;

        if ( state.Operator != Operator.Not )
            return false;

        expressionInfo.Kind = ExpressionKind.Not;
        return true;
    }
}
