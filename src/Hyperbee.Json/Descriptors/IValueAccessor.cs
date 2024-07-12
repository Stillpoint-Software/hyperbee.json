﻿namespace Hyperbee.Json.Descriptors;

public interface IValueAccessor<TNode>
{
    IEnumerable<(TNode, string, SelectorKind)> EnumerateChildren( TNode value, bool includeValues = true );
    bool TryGetElementAt( in TNode value, int index, out TNode element );
    NodeKind GetNodeKind( in TNode value );
    int GetArrayLength( in TNode value );
    bool TryGetChildValue( in TNode value, string childSelector, SelectorKind selectorKind, out TNode childValue );
    bool TryParseNode( ReadOnlySpan<char> item, out TNode value );
    bool DeepEquals( TNode left, TNode right );
    bool TryGetValueFromNode( TNode item, out object value );
    bool TryGetFromPointer( in TNode value, JsonPathSegment segment, out TNode childValue );
}
