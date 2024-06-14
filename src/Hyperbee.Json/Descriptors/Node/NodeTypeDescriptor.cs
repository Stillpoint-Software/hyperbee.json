using System.Text.Json.Nodes;
using Hyperbee.Json.Descriptors.Node.Functions;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node;

public class NodeTypeDescriptor : ITypeDescriptor<JsonNode>
{
    private FilterEvaluator<JsonNode> _evaluator;
    private NodeValueAccessor _accessor;
    public Dictionary<string, FunctionCreator> Functions { get; init; }

    public IValueAccessor<JsonNode> Accessor
    {
        get => _accessor ??= new NodeValueAccessor();
    }

    public IFilterEvaluator<JsonNode> FilterEvaluator
    {
        get => _evaluator ??= new FilterEvaluator<JsonNode>( this );
    }

    public FilterFunction GetFilterFunction( ParseExpressionContext context ) =>
        new FilterNodeFunction( context );

    public NodeTypeDescriptor()
    {
        Functions = new Dictionary<string, FunctionCreator>(
        [
            new KeyValuePair<string, FunctionCreator>( CountNodeFunction.Name, ( name, arguments, context ) => new CountNodeFunction( name, arguments, context ) ),
            new KeyValuePair<string, FunctionCreator>( LengthNodeFunction.Name, ( name, arguments, context ) => new LengthNodeFunction( name, arguments, context ) ),
            new KeyValuePair<string, FunctionCreator>( MatchNodeFunction.Name, ( name, arguments, context ) => new MatchNodeFunction( name, arguments, context ) ),
            new KeyValuePair<string, FunctionCreator>( SearchNodeFunction.Name, ( name, arguments, context ) => new SearchNodeFunction( name, arguments, context ) ),
            new KeyValuePair<string, FunctionCreator>( ValueNodeFunction.Name, ( name, arguments, context ) => new ValueNodeFunction( name, arguments, context ) ),
        ] );
    }
}
