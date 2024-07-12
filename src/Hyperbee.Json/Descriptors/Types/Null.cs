namespace Hyperbee.Json.Descriptors.Types;

public struct Null : INodeType
{
    public NodeTypeKind Kind => NodeTypeKind.Null;
    public INodeTypeComparer Comparer { get; set; }
}
