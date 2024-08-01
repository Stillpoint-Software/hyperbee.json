using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Path.Filters.Parser.Expressions;

internal class NotExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, out CompareConstraint compareConstraint, ITypeDescriptor<TNode> _ = null )
    {
        compareConstraint = CompareConstraint.None;
        expression = null;

        return state.Operator == Operator.Not;
    }
}
