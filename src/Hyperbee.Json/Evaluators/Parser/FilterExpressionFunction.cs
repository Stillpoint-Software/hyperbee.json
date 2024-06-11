using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser;

public abstract class FilterExpressionFunction(
    string methodName,
    IList<string> arguments,
    ParseExpressionContext context
) : FilterFunction
{
    public abstract Expression GetExpression( string methodName, IList<string> arguments, ParseExpressionContext context );

    protected override Expression Evaluate( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        return GetExpression( methodName, arguments, context );
    }
}
