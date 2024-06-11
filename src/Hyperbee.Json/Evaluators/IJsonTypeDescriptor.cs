using Hyperbee.Json.Evaluators.Parser;

namespace Hyperbee.Json.Evaluators;

public interface IJsonTypeDescriptor
{
    public Dictionary<string, FunctionCreator> Functions { get; }

    public IJsonValueAccessor<TElement> GetAccessor<TElement>();
    public IJsonPathFilterEvaluator<TElement> GetFilterEvaluator<TElement>();
    public FilterFunction GetFilterFunction( ParseExpressionContext context );
}
