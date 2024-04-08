namespace Hyperbee.Json.Evaluators;

[Serializable]
public class JsonPathEvaluatorException : Exception
{
    public JsonPathEvaluatorException()
        : base( "JsonPath evaluator exception." )
    {
    }

    public JsonPathEvaluatorException( string message )
        : base( message )
    {
    }

    public JsonPathEvaluatorException( string message, Exception innerException )
        : base( message, innerException )
    {
    }
}