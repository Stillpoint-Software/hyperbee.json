using System.Collections;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Filters.Values;

public struct NodeList<TNode>( IEnumerable<TNode> value, bool isNormalized ) : IValueType, IEnumerable<TNode>
{
    public readonly bool IsNormalized => isNormalized;
    public readonly ValueKind ValueKind => ValueKind.NodeList;

    public IValueTypeComparer Comparer { get; set; }

    public IEnumerable<TNode> Value { get; } = value;

    public readonly IEnumerator<TNode> GetEnumerator() => Value.GetEnumerator();

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
