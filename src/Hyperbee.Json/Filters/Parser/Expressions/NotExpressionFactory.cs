using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class NotExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo exprInfo, ITypeDescriptor<TNode> _ = null )
    {
        expression = null;

        if ( state.Operator != Operator.Not )
            return false;

        exprInfo.Kind = ExpressionKind.Not;
        return true;
    }
}
