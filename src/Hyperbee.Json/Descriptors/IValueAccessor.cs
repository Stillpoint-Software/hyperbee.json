using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json.Descriptors;

public interface IValueAccessor<TNode>
{
    IEnumerable<(TNode, string, SelectorKind)> EnumerateChildren( TNode value, bool includeValues = true );
    bool TryGetElementAt( in TNode value, int index, out TNode element );
    NodeKind GetNodeKind( in TNode value );
    int GetArrayLength( in TNode value );
    bool TryGetChild( in TNode value, string childSelector, SelectorKind selectorKind, out TNode childValue );
    bool TryParseNode( ref Utf8JsonReader reader, out TNode value );
    bool DeepEquals( TNode left, TNode right );
    bool TryGetValueFromNode( TNode item, out IConvertible value );
    bool TryGetFromPointer( in TNode value, JsonPathSegment segment, out TNode childValue );
}
