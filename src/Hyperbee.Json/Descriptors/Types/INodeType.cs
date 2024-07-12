namespace Hyperbee.Json.Descriptors.Types;

public interface INodeType
{
    public NodeTypeKind Kind { get; }

    public INodeTypeComparer Comparer { get; set; }
}
