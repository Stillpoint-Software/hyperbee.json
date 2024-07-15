using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors;

public delegate FilterExtensionFunction FunctionActivator();

public interface ITypeDescriptor
{
    public FunctionRegistry Functions { get; }
}

public interface ITypeDescriptor<TNode> : ITypeDescriptor
{
    public IValueAccessor<TNode> Accessor { get; }
    public IFilterEvaluator<TNode> FilterEvaluator { get; }

    public IValueTypeComparer Comparer { get; }
    bool CanUsePointer { get; }

    public void Deconstruct( out IValueAccessor<TNode> valueAccessor, out IFilterEvaluator<TNode> filterEvaluator )
    {
        valueAccessor = Accessor;
        filterEvaluator = FilterEvaluator;
    }
}
