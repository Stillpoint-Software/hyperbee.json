using System.Linq.Expressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class FilterElementFunction( ParseExpressionContext context ) : FilterFunction
{
    protected override Expression GetExpressionImpl( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        var queryExp = Expression.Constant( item.ToString() );

        // Create a call expression for the extension method
        return Expression.Call( FilterElementHelper.SelectFirstElementValueMethod, context.Current, context.Root, queryExp );
    }
}
