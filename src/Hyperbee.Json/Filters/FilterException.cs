namespace Hyperbee.Json.Filters;

[Serializable]
public class FilterException : Exception
{
    public FilterException()
        : base( "JsonPath filter evaluator exception." )
    {
    }

    public FilterException( string message )
        : base( message )
    {
    }

    public FilterException( string message, Exception innerException )
        : base( message, innerException )
    {
    }
}
