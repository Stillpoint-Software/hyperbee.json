using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser;

public abstract class FilterExtensionFunction : FilterFunction
{
    private readonly int _argumentCount;

    protected FilterExtensionFunction( int argumentCount )
    {
        _argumentCount = argumentCount;
    }

    protected abstract Expression GetExtensionExpression( Expression[] arguments );

    public override Expression GetExpression( ReadOnlySpan<char> filter, ReadOnlySpan<char> item, ref int start, ref int from, ParseExpressionContext context )
    {
        var arguments = new Expression[_argumentCount];

        for ( var i = 0; i < _argumentCount; i++ )
        {
            var argument = FilterExpressionParser.Parse( filter,
                ref start,
                ref from,
                i == _argumentCount - 1
                    ? FilterExpressionParser.EndArg
                    : FilterExpressionParser.ArgSeparator,
                context );

            arguments[i] = argument;
        }

        return GetExtensionExpression( arguments );
    }
}

