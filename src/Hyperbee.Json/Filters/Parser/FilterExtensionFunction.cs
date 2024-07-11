using System.Linq.Expressions;
using Hyperbee.Json.Internal;

namespace Hyperbee.Json.Filters.Parser;

public abstract class FilterExtensionFunction
{
    private readonly int _argumentCount;

    protected FilterExtensionFunction( int argumentCount )
    {
        _argumentCount = argumentCount;
    }

    protected abstract Expression GetExtensionExpression( Expression[] arguments, bool[] argumentInfo );

    internal Expression GetExpression<TNode>( ref ParserState state, FilterParserContext<TNode> parserContext )
    {
        var arguments = new Expression[_argumentCount];

        var argumentInfo = new bool[_argumentCount];

        for ( var i = 0; i < _argumentCount; i++ )
        {
            var localState = state with
            {
                Item = [],
                Terminal = i == _argumentCount - 1
                    ? FilterParser.EndArg
                    : FilterParser.ArgSeparator,
                IsArgument = true
            };

            if ( localState.EndOfBuffer || localState.IsTerminal )
                throw new NotSupportedException( $"Invalid arguments for filter: \"{state.Buffer}\"." );

            var argument = FilterParser<TNode>.Parse( ref localState, parserContext );

            argumentInfo[i] = QueryHelper.IsNonSingular( localState.Item ); //BF - nsq
            arguments[i] = argument;
        }

        return GetExtensionExpression( arguments, argumentInfo );
    }



}

