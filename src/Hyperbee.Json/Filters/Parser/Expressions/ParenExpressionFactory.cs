using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class ParenExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo expressionInfo, ITypeDescriptor<TNode> descriptor )
    {
        if ( state.Operator == Operator.OpenParen && state.Item.IsEmpty )
        {
            var localState = state with
            {
                Terminal = FilterParser.ArgClose
            };

            expression = FilterParser<TNode>.Parse( ref localState, descriptor ); // will recurse.
            expressionInfo.Kind = ExpressionKind.Paren;
            return true;
        }

        expression = null;
        return false;
    }
}
