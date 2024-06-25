using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class SelectExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression( ref ParserState state, out Expression expression, FilterContext context )
    {
        if ( state.Item[0] == '$' || state.Item[0] == '@' )
        {
            expression = context
                .SelectFactory
                .GetExpression( ref state, context ); // may cause `Select` recursion.
            return true;
        }

        expression = null;
        return false;
    }
}
