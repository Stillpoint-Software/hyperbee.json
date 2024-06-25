namespace Hyperbee.Json.Filters.Parser.Expressions;

internal interface IExprItem
{
    static abstract bool TryGetItem( ref ParserState state, ExprItemFactory exprItemCreator, out ExprItem exprItem, FilterContext context );
}
