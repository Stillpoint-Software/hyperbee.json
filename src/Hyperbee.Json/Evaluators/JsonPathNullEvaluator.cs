namespace Hyperbee.Json.Evaluators;

public class JsonPathNullEvaluator<TType> : IJsonPathFilterEvaluator<TType>
{
    public object Evaluator( string filter, TType current, TType root ) => null;
}
