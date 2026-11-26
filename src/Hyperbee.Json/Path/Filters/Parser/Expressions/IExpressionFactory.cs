using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Path.Filters.Parser.Expressions;

internal interface IExpressionFactory
{
    // netstandard2.1 predates static abstract interface members (CS8919). Nothing
    // constrains a type parameter to IExpressionFactory, so the member only documents
    // the shape the factories share; every call site invokes the static method on the
    // concrete factory. Dropping the declaration on this one target therefore costs
    // no dispatch, only the compiler check that each factory still matches.
#if !NETSTANDARD
    static abstract bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, out CompareConstraint compareConstraint, ITypeDescriptor<TNode> descriptor );
#endif
}
