using System.ComponentModel;
using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser;

public abstract class FilterFunction
{
    public abstract Expression GetExpression( ref ParserState state, FilterContext context );
}
