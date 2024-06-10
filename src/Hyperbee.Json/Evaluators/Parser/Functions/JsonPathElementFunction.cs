using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class JsonPathElementFunction<TType>( ParseExpressionContext<TType> context ) : ParserFunction<TType>
{
    protected override Expression Evaluate( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        var queryExp = Expression.Constant( item.ToString() );
        
        // Create a call expression for the extension method
        return Expression.Call( JsonPathHelper<TType>.GetFirstElementValueMethod, context.Current, context.Root, queryExp );
    }
}
