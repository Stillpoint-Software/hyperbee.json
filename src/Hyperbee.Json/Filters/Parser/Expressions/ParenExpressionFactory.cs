using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class ParenExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo expressionInfo, FilterParserContext<TNode> parserContext )
    {
        if ( state.Operator == Operator.OpenParen && state.Item.IsEmpty )
        {
            var localState = state with
            {
                Terminal = FilterParser.ArgClose
            };

            expression = FilterParser<TNode>.Parse( ref localState, parserContext ); // will recurse.
            expressionInfo.Kind = ExpressionKind.Paren;
            return true;
        }

        expression = null;
        return false;
    }
}
