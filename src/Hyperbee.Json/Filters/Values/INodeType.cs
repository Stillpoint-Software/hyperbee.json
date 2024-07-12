namespace Hyperbee.Json.Filters.Values;

public interface INodeType
{
    public NodeTypeKind Kind { get; }

    public INodeTypeComparer Comparer { get; set; }
}
