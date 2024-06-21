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

    public FunctionRegistry Functions { get; } = new();

    public IValueAccessor<JsonNode> Accessor =>
        _accessor ??= new NodeValueAccessor();

    public IFilterEvaluator<JsonNode> FilterEvaluator =>
        _evaluator ??= new FilterEvaluator<JsonNode>( this );

    public NodeTypeDescriptor()
    {
        Functions.Register( CountNodeFunction.Name, context => new CountNodeFunction( context ) );
        Functions.Register( LengthNodeFunction.Name, context => new LengthNodeFunction( context ) );
        Functions.Register( MatchNodeFunction.Name, context => new MatchNodeFunction( context ) );
        Functions.Register( SearchNodeFunction.Name, context => new SearchNodeFunction( context ) );
        Functions.Register( ValueNodeFunction.Name, context => new ValueNodeFunction( context ) );
    }

    public FilterFunction GetSelectFunction( ParseExpressionContext context )
    {
        return new SelectNodeFunction( context );
    }

    public Expression GetValueExpression( Expression expression )
    {
        if ( expression is null )
            return null;

        return expression.Type == typeof( IEnumerable<JsonNode> )
            ? Expression.Invoke( ValueNodeFunction.ValueExpression, expression )
            : expression;
    }
}
