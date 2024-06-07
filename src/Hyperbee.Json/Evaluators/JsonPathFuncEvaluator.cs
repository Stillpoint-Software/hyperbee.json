namespace Hyperbee.Json.Evaluators;

public class JsonPathFuncEvaluator<TType> : IJsonPathFilterEvaluator<TType>
{
    private readonly JsonPathEvaluator<TType> _evaluator;

    public JsonPathFuncEvaluator( JsonPathEvaluator<TType> evaluator )
    {
        _evaluator = evaluator;
    }

    public object Evaluator( string script, TType current, TType root, string basePath )
    {
        return _evaluator?.Invoke( script, current, root, basePath );
    }
}
