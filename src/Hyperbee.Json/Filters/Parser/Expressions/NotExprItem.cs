namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class NotExprItem : IExprItem
{
    public static bool TryGetItem( ref ParserState state, ExprItemFactory exprItemCreator, out ExprItem exprItem, FilterContext context )
    {
        if ( state.Operator == Operator.Not )
        {
            exprItem = new( null, Operator.Not );
            return true;
        }

        exprItem = null;
        return false;
    }
}
