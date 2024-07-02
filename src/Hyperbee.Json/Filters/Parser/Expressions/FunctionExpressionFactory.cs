using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class FunctionExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, FilterContext<TNode> context )
    {
        if ( context.Descriptor.Functions.TryGetCreator( state.Item.ToString(), out var functionCreator ) )
        {
            expression = functionCreator()
                .GetExpression( ref state, context ); // will recurse for each function argument.

            return true;
        }

        expression = null;
        return false;
    }
}
