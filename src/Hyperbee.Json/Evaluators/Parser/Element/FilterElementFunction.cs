using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser.Element;

public class FilterElementFunction( ParseExpressionContext context ) : FilterFunction
{
    protected override Expression Evaluate( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        var queryExp = Expression.Constant( item.ToString() );

        // Create a call expression for the extension method
        return Expression.Call( FilterElementHelper.SelectFirstElementValueMethod, context.Current, context.Root, queryExp );
    }
}
