using System.Text.Json;
using Hyperbee.Json.Query;

namespace Hyperbee.Json.Descriptors;

public interface INodeActions<TNode>
{
    bool TryParse( ref Utf8JsonReader reader, out TNode value );

    public bool TryGetFromPointer( in TNode node, JsonSegment segment, out TNode value );

    public bool DeepEquals( TNode left, TNode right );

    public IEnumerable<(TNode Value, string Key)> GetChildrenWithName( in TNode value, ChildEnumerationOptions options );
    public IEnumerable<TNode> GetChildren( TNode value, ChildEnumerationOptions options );
}
