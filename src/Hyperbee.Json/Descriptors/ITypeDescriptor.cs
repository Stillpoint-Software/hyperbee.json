using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors;

public delegate ExtensionFunction FunctionActivator();

public interface ITypeDescriptor
{
    public FunctionRegistry Functions { get; }
}

public interface ITypeDescriptor<TNode> : ITypeDescriptor
{
    public IValueAccessor<TNode> Accessor { get; }

    bool CanUsePointer { get; }
}
