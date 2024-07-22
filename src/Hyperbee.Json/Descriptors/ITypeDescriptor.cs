using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors;

public delegate ExtensionFunction FunctionActivator();

public interface ITypeDescriptor
{
    public FunctionRegistry Functions { get; }
}

public interface ITypeDescriptor<TNode> : ITypeDescriptor
{
    public IValueAccessor<TNode> Accessor { get; }
    public IFilterRuntime<TNode> FilterRuntime { get; }

    public IValueTypeComparer Comparer { get; }
    bool CanUsePointer { get; }

    public void Deconstruct( out IValueAccessor<TNode> valueAccessor, out IFilterRuntime<TNode> filterRuntime )
    {
        valueAccessor = Accessor;
        filterRuntime = FilterRuntime;
    }
}
