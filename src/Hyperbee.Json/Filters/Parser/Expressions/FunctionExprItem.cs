namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class FunctionExprItem : IExprItem
{
    public static bool TryGetItem( ref ParserState state, ExprItemFactory exprItemCreator, out ExprItem exprItem, FilterContext context )
    {
        if ( context.Descriptor.Functions.TryGetCreator( state.Item.ToString(), out var functionCreator ) )
        {
            var expression = functionCreator()
                .GetExpression( ref state, context ); // will recurse for each function argument.

            exprItem = exprItemCreator( ref state, expression ); ;
            return true;
        }

        exprItem = null;
        return false;
    }
}
