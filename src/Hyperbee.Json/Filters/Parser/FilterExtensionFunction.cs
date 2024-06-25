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

    public override Expression GetExpression( ref ParserState state, FilterContext context )
    {
        var arguments = new Expression[_argumentCount];

        for ( var i = 0; i < _argumentCount; i++ )
        {
            var localState = state with
            {
                Item = [],
                Terminal = i == _argumentCount - 1
                    ? FilterParser.EndArg
                    : FilterParser.ArgSeparator
            };

            var argument = FilterParser.Parse( ref localState, context );

            arguments[i] = argument;
        }

        return GetExtensionExpression( arguments );
    }
}

