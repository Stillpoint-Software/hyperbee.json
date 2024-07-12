using System.Collections;

namespace Hyperbee.Json.Filters.Values;

public struct NodesType<TNode>( IEnumerable<TNode> value, bool nonSingular ) : INodeType, IEnumerable<TNode>
{
    public readonly bool NonSingular => nonSingular;
    public readonly NodeTypeKind Kind => NodeTypeKind.NodeList;

    public INodeTypeComparer Comparer { get; set; }

    public IEnumerable<TNode> Value { get; } = value;

    public readonly IEnumerator<TNode> GetEnumerator() => Value.GetEnumerator();

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
