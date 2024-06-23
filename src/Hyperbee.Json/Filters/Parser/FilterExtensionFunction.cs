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

    public override Expression GetExpression( ReadOnlySpan<char> filter, ReadOnlySpan<char> item, ref int pos, FilterExecutionContext executionContext )
    {
        var arguments = new Expression[_argumentCount];

        for ( var i = 0; i < _argumentCount; i++ )
        {
            var argument = FilterParser.Parse( filter,
                ref pos,
                i == _argumentCount - 1
                    ? FilterParser.EndArg
                    : FilterParser.ArgSeparator,
                executionContext );

            arguments[i] = argument;
        }

        return GetExtensionExpression( arguments );
    }
}

