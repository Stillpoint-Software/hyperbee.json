using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class JsonPathElementFunction<TType> : ParserFunction<TType>
{
    private readonly ParseExpressionContext<TType> _context;

    public JsonPathElementFunction( ParseExpressionContext<TType> context )
    {
        _context = context;
    }

    protected override Expression Evaluate( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        var queryExp = Expression.Constant( item.ToString() );
        var evaluatorExp = Expression.Constant( _context.Evaluator );

        // Create a call expression for the extension method
        return Expression.Call( JsonPathHelper<TType>.GetFirstElementValueMethod, _context.Current, _context.Root, queryExp, evaluatorExp );
    }
}
