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

    public INodeActions<TNode> NodeActions { get; }

    public void Deconstruct( out IValueAccessor<TNode> valueAccessor, out INodeActions<TNode> nodeActions )
    {
        valueAccessor = ValueAccessor;
        nodeActions = NodeActions;
    }
}
