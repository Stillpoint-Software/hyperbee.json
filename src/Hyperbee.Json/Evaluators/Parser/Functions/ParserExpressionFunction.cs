using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public abstract class ParserExpressionFunction<TType>( string methodName, string[] arguments, Expression currentExpression, Expression rootExpression, IJsonPathScriptEvaluator<TType> evaluator, string context = null ) : ParserFunction<TType>
{
    public abstract Expression GetExpression( string methodName, string[] arguments, Expression currentExpression, Expression rootExpression, IJsonPathScriptEvaluator<TType> evaluator, string context );

    protected override Expression Evaluate( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        return GetExpression( methodName, arguments, currentExpression, rootExpression, evaluator, context );
    }
}
