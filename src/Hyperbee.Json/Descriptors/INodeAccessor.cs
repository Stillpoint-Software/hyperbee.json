using System.Text.Json;

namespace Hyperbee.Json.Descriptors;

public interface INodeAccessor<TNode>
{
    bool TryParse( ref Utf8JsonReader reader, out TNode value );

    bool CanUsePointer { get; }

    public bool TryGetFromPointer( in TNode element, JsonPathSegment segment, out TNode childValue );

    public bool DeepEquals( TNode left, TNode right );
}
