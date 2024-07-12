namespace Hyperbee.Json.Descriptors.Types;

public struct Nothing : INodeType
{
    public readonly NodeTypeKind Kind => NodeTypeKind.Nothing;
    public INodeTypeComparer Comparer { get; set; }
}
