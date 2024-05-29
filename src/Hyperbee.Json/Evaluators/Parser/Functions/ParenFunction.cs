using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class ParenFunction<TType> : ParserFunction<TType>
{
    public IJsonPathScriptEvaluator<TType> Evaluator { get; set; }
    public Expression CurrentExpression { get; set; }
    public Expression RootExpression { get; set; }
    protected override Expression Evaluate( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        return JsonPathExpression.Parse( data, ref start, ref from, JsonPathExpression.EndArg, CurrentExpression, RootExpression, Evaluator );
    }
}
