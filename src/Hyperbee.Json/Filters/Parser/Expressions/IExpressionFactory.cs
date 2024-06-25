using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal interface IExpressionFactory
{
    static abstract bool TryGetExpression( ref ParserState state, out Expression expression, FilterContext context );
}
