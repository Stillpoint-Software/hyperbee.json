using System.Linq.Expressions;
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

    public FilterFunction GetSelectFunction( ParseExpressionContext context ) =>
        new SelectNodeFunction( context );

    public Expression GetValueExpression( Expression expression )
    {
        if ( expression is null ) return null;

        return expression.Type == typeof( IEnumerable<JsonNode> )
            ? Expression.Call( ValueNodeFunction.ValueMethod, expression )
            : expression;
    }

    public NodeTypeDescriptor()
    {
        Functions = new Dictionary<string, FunctionCreator>(
        [
            new KeyValuePair<string, FunctionCreator>( CountNodeFunction.Name, ( name, context ) => new CountNodeFunction( name, context ) ),
            new KeyValuePair<string, FunctionCreator>( LengthNodeFunction.Name, ( name, context ) => new LengthNodeFunction( name, context ) ),
            new KeyValuePair<string, FunctionCreator>( MatchNodeFunction.Name, ( name, context ) => new MatchNodeFunction( name, context ) ),
            new KeyValuePair<string, FunctionCreator>( SearchNodeFunction.Name, ( name, context ) => new SearchNodeFunction( name, context ) ),
            new KeyValuePair<string, FunctionCreator>( ValueNodeFunction.Name, ( name, context ) => new ValueNodeFunction( name, context ) ),
        ] );
    }
}
