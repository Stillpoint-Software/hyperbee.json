namespace Hyperbee.Json.Evaluators;

public class JsonPathFuncEvaluator<TType>( JsonPathEvaluator<TType> evaluator ) : IJsonPathFilterEvaluator<TType>
{
    public object Evaluator( string script, TType current, TType root) => 
        evaluator?.Invoke( script, current, root );
}
