namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class SelectExprItem : IExprItem
{
    public static bool TryGetItem( ref ParserState state, ExprItemFactory exprItemCreator, out ExprItem exprItem, FilterContext context )
    {
        if ( state.Item[0] == '$' || state.Item[0] == '@' )
        {
            var expression = context.Descriptor
                .GetSelectFunction()
                .GetExpression( ref state, context ); // may cause `Select` recursion.

            exprItem = exprItemCreator( ref state, expression );
            return true;
        }

        exprItem = null;
        return false;
    }
}
