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
            ? Expression.Invoke( ValueNodeFunction.ValueExpression, expression )
            : expression;
    }

    public NodeTypeDescriptor()
    {
        Functions = new Dictionary<string, FunctionCreator>(
        [
            new KeyValuePair<string, FunctionCreator>( CountNodeFunction.Name, context => new CountNodeFunction( context ) ),
            new KeyValuePair<string, FunctionCreator>( LengthNodeFunction.Name, context => new LengthNodeFunction( context ) ),
            new KeyValuePair<string, FunctionCreator>( MatchNodeFunction.Name, context => new MatchNodeFunction( context ) ),
            new KeyValuePair<string, FunctionCreator>( SearchNodeFunction.Name, context => new SearchNodeFunction( context ) ),
            new KeyValuePair<string, FunctionCreator>( ValueNodeFunction.Name, context => new ValueNodeFunction( context ) ),
        ] );
    }
}
