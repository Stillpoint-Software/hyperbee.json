using Hyperbee.Json.Descriptors.Node;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors;

public delegate ExtensionFunction FunctionActivator();

public interface ITypeDescriptor
{
    public FunctionRegistry Functions { get; }
}

public interface ITypeDescriptor<TNode> : ITypeDescriptor
{
    public IValueAccessor<TNode> ValueAccessor { get; }

    public INodeAccessor<TNode> NodeAccessor { get; }

    public void Deconstruct( out IValueAccessor<TNode> valueAccessor, out INodeAccessor<TNode> nodeAccessor )
    {
        valueAccessor = ValueAccessor;
        nodeAccessor = NodeAccessor;
    }
}
