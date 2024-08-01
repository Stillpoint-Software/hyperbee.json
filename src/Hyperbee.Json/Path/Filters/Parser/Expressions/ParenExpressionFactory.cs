using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Path.Filters.Parser.Expressions;

internal class ParenExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, out CompareConstraint compareConstraint, ITypeDescriptor<TNode> _ = null )
    {
        compareConstraint = CompareConstraint.None;

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
        return true;
    }
}
