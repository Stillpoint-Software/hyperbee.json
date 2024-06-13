using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors;


public interface IJsonTypeDescriptor
{
    public Dictionary<string, FunctionCreator> Functions { get; }

    public FilterFunction GetFilterFunction( ParseExpressionContext context );
}

public interface ITypeDescriptor<TElement> : IJsonTypeDescriptor
{
    public IValueAccessor<TElement> Accessor { get; }
    public IFilterEvaluator<TElement> FilterEvaluator { get; }

    public void Deconstruct( out IValueAccessor<TElement> valueAccessor, out IFilterEvaluator<TElement> filterEvaluator )
    {
        valueAccessor = Accessor;
        filterEvaluator = FilterEvaluator;
    }
}
