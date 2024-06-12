using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser;

public abstract class FilterExtensionFunction : FilterFunction
{
    private readonly string _methodName;
    private readonly IList<string> _arguments;
    private readonly ParseExpressionContext _context;

    protected FilterExtensionFunction( string methodName,
        IList<string> arguments,
        ParseExpressionContext context )
    {
        _methodName = methodName;
        _arguments = arguments;
        _context = context;
    }

    public abstract Expression GetExtensionExpression( string methodName, IList<string> arguments, ParseExpressionContext context );

    protected override Expression GetExpressionImpl( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        // Convert to extension function shape
        return GetExtensionExpression( _methodName, _arguments, _context );
    }
}
