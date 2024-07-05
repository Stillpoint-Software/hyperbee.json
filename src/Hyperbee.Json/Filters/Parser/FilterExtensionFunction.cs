using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser;

public abstract class FilterExtensionFunction
{
    private readonly int _argumentCount;

    protected FilterExtensionFunction( int argumentCount )
    {
        _argumentCount = argumentCount;
    }

    protected abstract Expression GetExtensionExpression( Expression[] arguments );

    internal Expression GetExpression<TNode>( ref ParserState state, FilterContext<TNode> context )
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

            if ( localState.IsTerminal )
                throw new NotSupportedException( $"Invalid arguments for filter: \"{state.Buffer}\"." );

            var argument = FilterParser<TNode>.Parse( ref localState, context );

            arguments[i] = argument;
        }

        return GetExtensionExpression( arguments );
    }
}

