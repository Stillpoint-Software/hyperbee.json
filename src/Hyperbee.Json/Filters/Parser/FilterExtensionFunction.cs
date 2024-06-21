using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser;

public delegate FilterExtensionFunction FunctionCreator( ParseExpressionContext context );

public abstract class FilterExtensionFunction( int argumentCount, ParseExpressionContext context ) : FilterFunction
{
    public abstract Expression GetExtensionExpression( Expression[] arguments, ParseExpressionContext context );

    protected override Expression GetExpressionImpl( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        var arguments = new Expression[argumentCount];

        for ( var i = 0; i < argumentCount; i++ )
        {
            var argument = FilterExpressionParser.Parse( data,
                ref start,
                ref from,
                i == argumentCount - 1
                    ? FilterExpressionParser.EndArg
                    : FilterExpressionParser.ArgSeparator,
                context );

            arguments[i] = argument;
        }

        return GetExtensionExpression( arguments, context );
    }
}
