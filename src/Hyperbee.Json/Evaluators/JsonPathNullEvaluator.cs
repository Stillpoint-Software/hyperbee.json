namespace Hyperbee.Json.Evaluators;

public class JsonPathNullEvaluator<TType> : IJsonPathScriptEvaluator<TType>
{
    public object Evaluator( string script, TType current, string context )
    {
        return null;
    }
}