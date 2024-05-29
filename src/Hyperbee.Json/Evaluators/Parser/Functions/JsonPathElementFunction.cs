using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class JsonPathElementFunction<TType> : ParserFunction<TType>
{
    public IJsonPathScriptEvaluator<TType> Evaluator { get; set; }
    public Expression CurrentExpression { get; set; }
    public Expression RootExpression { get; set; }

    protected override Expression Evaluate( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        var queryExp = Expression.Constant( item.ToString() );
        var evaluatorExp = Expression.Constant( Evaluator );

        // Create a call expression for the extension method
        return Expression.Call( JsonPathHelper<TType>.GetFirstElementValueMethod, CurrentExpression, RootExpression, queryExp, evaluatorExp );
    }
}
