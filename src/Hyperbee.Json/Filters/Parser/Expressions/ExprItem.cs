using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal delegate ExprItem ExprItemFactory( ref ParserState state, Expression expression );

internal class ExprItem( Expression expression, Operator tokenType )
{
    public Expression Expression { get; set; } = expression;
    public Operator Operator { get; set; } = tokenType;
}
