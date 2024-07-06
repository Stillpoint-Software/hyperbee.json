using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class ParenExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, FilterContext<TNode> context )
    {
        if ( state.Operator == Operator.OpenParen && state.Item.IsEmpty )
        {
            var localState = state with 
            { 
                Terminal = FilterParser.EndArg
            };

            expression = FilterParser<TNode>.Parse( ref localState, context ); // will recurse.
            return true;
        }

        expression = null;
        return false;
    }
}
