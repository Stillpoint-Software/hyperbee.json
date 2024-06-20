using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser;

public abstract class FilterExtensionFunction : FilterFunction
{
    private readonly string _methodName;
    private readonly int _argumentCount;
    private readonly ParseExpressionContext _context;

    protected FilterExtensionFunction( string methodName, int argumentCount,
        ParseExpressionContext context )
    {
        _methodName = methodName;
        _argumentCount = argumentCount;
        _context = context;
    }

    public abstract Expression GetExtensionExpression( string methodName, Expression[] arguments, ParseExpressionContext context );

    protected override Expression GetExpressionImpl( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        var arguments = new Expression[_argumentCount];

        for ( var i = 0; i < _argumentCount; i++ )
        {
            var argument = FilterExpressionParser.Parse( data,
                ref start,
                ref from,
                i == _argumentCount - 1
                    ? FilterExpressionParser.EndArg
                    : FilterExpressionParser.ArgSeparator,
                _context );

            arguments[i] = argument;
        }

        return GetExtensionExpression( _methodName, arguments, _context );
    }
}
