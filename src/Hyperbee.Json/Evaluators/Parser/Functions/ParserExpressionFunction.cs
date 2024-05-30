using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public abstract class ParserExpressionFunction<TType>( string methodName, IList<string> arguments, ParseExpressionContext<TType> context ) : ParserFunction<TType>
{
    public abstract Expression GetExpression( string methodName, IList<string> arguments, ParseExpressionContext<TType> context );

    protected override Expression Evaluate( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        return GetExpression( methodName, arguments, context );
    }
}
