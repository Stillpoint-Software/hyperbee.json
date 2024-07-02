using System.Text.Json.Nodes;
using Hyperbee.Json.Descriptors.Node.Functions;
using Hyperbee.Json.Filters;

namespace Hyperbee.Json.Descriptors.Node;

public class NodeTypeDescriptor : ITypeDescriptor<JsonNode>
{
    private FilterEvaluator<JsonNode> _evaluator;
    private NodeValueAccessor _accessor;

    public FunctionRegistry Functions { get; } = new();

    public IValueAccessor<JsonNode> Accessor =>
        _accessor ??= new NodeValueAccessor();

    public IFilterEvaluator<JsonNode> FilterEvaluator =>
        _evaluator ??= new FilterEvaluator<JsonNode>( this );

    public NodeTypeDescriptor()
    {
        Functions.Register( CountNodeFunction.Name, () => new CountNodeFunction() );
        Functions.Register( LengthNodeFunction.Name, () => new LengthNodeFunction() );
        Functions.Register( MatchNodeFunction.Name, () => new MatchNodeFunction() );
        Functions.Register( SearchNodeFunction.Name, () => new SearchNodeFunction() );
        Functions.Register( ValueNodeFunction.Name, () => new ValueNodeFunction() );
    }
}
