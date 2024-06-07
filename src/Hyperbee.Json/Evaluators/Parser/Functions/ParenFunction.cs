using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class ParenFunction<TType>( ParseExpressionContext<TType> context ) : ParserFunction<TType>
{
    protected override Expression Evaluate( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        return JsonPathExpression.Parse( data, ref start, ref from, JsonPathExpression.EndArg, context );
    }
}
