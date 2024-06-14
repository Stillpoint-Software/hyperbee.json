namespace Hyperbee.Json.Filters;

[Serializable]
public class FilterEvaluatorException : Exception
{
    public FilterEvaluatorException()
        : base( "JsonPath filter evaluator exception." )
    {
    }

    public FilterEvaluatorException( string message )
        : base( message )
    {
    }

    public FilterEvaluatorException( string message, Exception innerException )
        : base( message, innerException )
    {
    }
}
