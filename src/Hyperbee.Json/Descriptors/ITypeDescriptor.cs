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

    public IParserAccessor<TNode> ParserAccessor { get; }

    bool CanUsePointer { get; }

    public bool TryGetFromPointer( in TNode element, JsonPathSegment segment, out TNode childValue );

    public bool DeepEquals( TNode left, TNode right );
}
