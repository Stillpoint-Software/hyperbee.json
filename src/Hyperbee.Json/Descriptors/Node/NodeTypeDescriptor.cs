using System.Text.Json.Nodes;
using Hyperbee.Json.Descriptors.Node.Functions;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node;

public class NodeTypeDescriptor : ITypeDescriptor<JsonNode>
{
    private NodeValueAccessor _accessor;
    private FilterEvaluator<JsonNode> _evaluator;
    private ValueTypeComparer<JsonNode> _comparer;

    public FunctionRegistry Functions { get; } = new();

    public IValueAccessor<JsonNode> Accessor =>
        _accessor ??= new NodeValueAccessor();

    public IFilterEvaluator<JsonNode> FilterEvaluator =>
        _evaluator ??= new FilterEvaluator<JsonNode>();

    public IValueTypeComparer Comparer =>
        _comparer ??= new ValueTypeComparer<JsonNode>( Accessor );

    public bool CanUsePointer => true;

    public NodeTypeDescriptor()
    {
        Functions.Register( CountNodeFunction.Name, () => new CountNodeFunction() );
        Functions.Register( LengthNodeFunction.Name, () => new LengthNodeFunction() );
        Functions.Register( MatchNodeFunction.Name, () => new MatchNodeFunction() );
        Functions.Register( SearchNodeFunction.Name, () => new SearchNodeFunction() );
        Functions.Register( ValueNodeFunction.Name, () => new ValueNodeFunction() );
    }
}
