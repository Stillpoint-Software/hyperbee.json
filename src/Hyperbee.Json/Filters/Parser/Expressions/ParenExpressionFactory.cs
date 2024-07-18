using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class ParenExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo exprInfo, ITypeDescriptor<TNode> _ = null )
    {
        if ( state.Operator != Operator.OpenParen || !state.Item.IsEmpty )
        {
            expression = null;
            return false;
        }

        var localState = state with
        {
            TerminalCharacter = FilterParser.ArgClose
        };

        expression = FilterParser<TNode>.Parse( ref localState ); // will recurse.
        exprInfo.Kind = ExpressionKind.Paren;
        return true;
    }
}
