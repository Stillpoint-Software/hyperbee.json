namespace Hyperbee.Json.Path.Filters;

[Serializable]
public class FilterCompilerException : Exception
{
    public FilterCompilerException()
        : base( "JsonPath filter evaluator exception." )
    {
    }

    public FilterCompilerException( string message )
        : base( message )
    {
    }

    public FilterCompilerException( string message, Exception innerException )
        : base( message, innerException )
    {
    }
}
