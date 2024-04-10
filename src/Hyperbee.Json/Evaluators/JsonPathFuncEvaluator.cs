namespace Hyperbee.Json.Evaluators;

public class JsonPathFuncEvaluator<TType> : IJsonPathScriptEvaluator<TType>
{
    private readonly JsonPathEvaluator<TType> _evaluator;

    public JsonPathFuncEvaluator( JsonPathEvaluator<TType> evaluator )
    {
        _evaluator = evaluator;
    }

    public object Evaluator( string script, TType current, string context )
    {
        return _evaluator?.Invoke( script, current, context );
    }
}
