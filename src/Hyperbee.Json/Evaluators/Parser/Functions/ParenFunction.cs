using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class ParenFunction<TType> : ParserFunction<TType>
{
    private readonly ParseExpressionContext<TType> _context;

    public ParenFunction( ParseExpressionContext<TType> context )
    {
        _context = context;
    }

    protected override Expression Evaluate( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        var childContext = _context with { BasePath = Expression.Constant( String.Empty ) };
        return JsonPathExpression.Parse( data, ref start, ref from, JsonPathExpression.EndArg, childContext );
    }
}
