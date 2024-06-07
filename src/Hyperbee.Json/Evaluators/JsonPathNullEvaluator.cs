namespace Hyperbee.Json.Evaluators;

public class JsonPathNullEvaluator<TType> : IJsonPathFilterEvaluator<TType>
{
    public object Evaluator( string script, TType current, TType root ) => null;
}
