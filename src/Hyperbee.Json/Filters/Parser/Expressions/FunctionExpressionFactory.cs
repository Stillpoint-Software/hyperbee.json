using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class FunctionExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo expressionInfo, FilterParserContext<TNode> parserContext )
    {
        if ( parserContext.Descriptor.Functions.TryGetCreator( state.Item.ToString(), out var functionCreator ) )
        {
            if ( state.TrailingWhitespace )
                throw new NotSupportedException( "Whitespace is not allowed after a function name." );

            expression = functionCreator()
                .GetExpression( ref state, parserContext ); // will recurse for each function argument.

            expressionInfo.Kind = ExpressionKind.Function;
            return true;
        }

        expression = null;
        return false;
    }
}
