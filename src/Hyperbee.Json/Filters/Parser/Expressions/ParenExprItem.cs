namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class ParenExprItem : IExprItem
{
    public static bool TryGetItem( ref ParserState state, ExprItemFactory exprItemCreator, out ExprItem exprItem, FilterContext context )
    {
        if ( state.Operator == Operator.OpenParen && state.Item.IsEmpty )
        {
            var localState = state with { Terminal = FilterParser.EndArg };
            var expression = FilterParser.Parse( ref localState, context ); // will recurse.
            exprItem = exprItemCreator( ref state, expression );
            return true;
        }

        exprItem = null;
        return false;
    }
}
