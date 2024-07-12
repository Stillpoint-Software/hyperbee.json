namespace Hyperbee.Json.Filters.Values;

public struct Null : INodeType
{
    public readonly NodeTypeKind Kind => NodeTypeKind.Null;
    public INodeTypeComparer Comparer { get; set; }
}
