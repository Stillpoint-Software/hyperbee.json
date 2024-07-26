using System.Text.Json.Nodes;
using Hyperbee.Json.Descriptors.Node.Functions;
using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Descriptors.Node;

public class NodeTypeDescriptor : ITypeDescriptor<JsonNode>
{
    public IValueAccessor<JsonNode> ValueAccessor => new NodeValueAccessor();
    public IParserAccessor<JsonNode> ParserAccessor => new NodeParserAccessor();
    public FunctionRegistry Functions { get; } = new();

    public bool CanUsePointer => true;

    public NodeTypeDescriptor()
    {
        Functions.Register( CountNodeFunction.Name, () => new CountNodeFunction() );
        Functions.Register( LengthNodeFunction.Name, () => new LengthNodeFunction() );
        Functions.Register( MatchNodeFunction.Name, () => new MatchNodeFunction() );
        Functions.Register( SearchNodeFunction.Name, () => new SearchNodeFunction() );
        Functions.Register( ValueNodeFunction.Name, () => new ValueNodeFunction() );
    }

    public bool TryGetFromPointer( in JsonNode element, JsonPathSegment segment, out JsonNode childValue ) =>
        element.TryGetFromJsonPathPointer( segment, out childValue );

    public bool DeepEquals( JsonNode left, JsonNode right ) =>
        JsonNode.DeepEquals( left, right );
}
