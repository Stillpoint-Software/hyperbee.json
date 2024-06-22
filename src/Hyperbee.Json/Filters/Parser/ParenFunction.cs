using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser;

internal class ParenFunction : FilterFunction
{
    protected override Expression GetExpressionImpl( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from, ParseExpressionContext context )
    {
        return FilterExpressionParser.Parse( data, ref start, ref from, FilterExpressionParser.EndArg, context );
    }
}
