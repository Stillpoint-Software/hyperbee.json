using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser;

internal class ParenFunction( ParseExpressionContext context ) : FilterFunction
{
    protected override Expression Evaluate( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        return JsonPathExpression.Parse( data, ref start, ref from, JsonPathExpression.EndArg, context );
    }
}
