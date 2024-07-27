using System.Text.Json;

namespace Hyperbee.Json.Descriptors;

public interface INodeActions<TNode>
{
    bool TryParse( ref Utf8JsonReader reader, out TNode value );

    public bool TryGetFromPointer( in TNode node, JsonPathSegment segment, out TNode childValue );

    public bool DeepEquals( TNode left, TNode right );

    public IEnumerable<(TNode Value, string Key)> GetChildren( in TNode value, bool complexTypesOnly = false );
}
