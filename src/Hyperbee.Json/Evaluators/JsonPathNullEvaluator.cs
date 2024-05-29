namespace Hyperbee.Json.Evaluators;

public class JsonPathNullEvaluator<TType> : IJsonPathScriptEvaluator<TType>
{
    public object Evaluator( string script, TType current, TType root, string context )
    {
        return null;
    }
}
