namespace Hyperbee.Json.Descriptors.Types;

public struct Null : INodeType
{
    public readonly NodeTypeKind Kind => NodeTypeKind.Null;
    public INodeTypeComparer Comparer { get; set; }
}
