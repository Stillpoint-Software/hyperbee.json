using System.Collections;

namespace Hyperbee.Json.Filters.Values;

public readonly struct NodeList<TNode>( IEnumerable<TNode> value, bool isNormalized ) : IValueType, IEnumerable<TNode>
{
    public bool IsNormalized => isNormalized;
    public ValueKind ValueKind => ValueKind.NodeList;

    public IEnumerable<TNode> Value { get; } = value;

    public IEnumerator<TNode> GetEnumerator() => Value.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
