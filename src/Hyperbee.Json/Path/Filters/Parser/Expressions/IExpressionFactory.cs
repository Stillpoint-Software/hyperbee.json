using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Path.Filters.Parser.Expressions;

internal interface IExpressionFactory
{
    static abstract bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, out CompareConstraint compareConstraint, ITypeDescriptor<TNode> descriptor );
}
