namespace Hyperbee.Json.Evaluators;

public class JsonPathFuncEvaluator<TType>( JsonPathEvaluator<TType> func ) : IJsonPathFilterEvaluator<TType>
{
    public object Evaluator( string filter, TType current, TType root ) => func?.Invoke( filter, current, root );
}
