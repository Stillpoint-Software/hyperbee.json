using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser;

public abstract class FilterExtensionFunction : FilterFunction
{
    private readonly int _argumentCount;

    protected FilterExtensionFunction( int argumentCount )
    {
        _argumentCount = argumentCount;
    }


    public abstract Expression GetExtensionExpression( Expression[] arguments );

    protected override Expression GetExpressionImpl( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from, ParseExpressionContext context )
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
                context );

            arguments[i] = argument;
        }

        return GetExtensionExpression( arguments );
    }
}

