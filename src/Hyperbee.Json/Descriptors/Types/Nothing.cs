namespace Hyperbee.Json.Descriptors.Types;

public struct Nothing : INodeType
{
    public NodeTypeKind Kind => NodeTypeKind.Nothing;
    public INodeTypeComparer Comparer { get; set; }
}
