using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser;

internal class ParenFunction : FilterFunction
{
    public override Expression GetExpression( ReadOnlySpan<char> filter, ReadOnlySpan<char> item, ref int start, ref int from, ParseExpressionContext context )
    {
        return FilterExpressionParser.Parse( filter, ref start, ref from, FilterExpressionParser.EndArg, context );
    }
}
