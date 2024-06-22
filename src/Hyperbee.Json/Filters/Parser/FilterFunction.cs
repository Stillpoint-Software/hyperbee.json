using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser;

public abstract class FilterFunction
{
    public abstract Expression GetExpression( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from, ParseExpressionContext context );
}
