using System.Linq.Expressions;
using System.Text.Json;
using Hyperbee.Json.Descriptors.Element.Functions;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element;

public class ElementTypeDescriptor : ITypeDescriptor<JsonElement>
{
    private FilterEvaluator<JsonElement> _evaluator;
    private ElementValueAccessor _accessor;

    public FunctionRegistry Functions { get; } = new();

    public IValueAccessor<JsonElement> Accessor => 
        _accessor ??= new ElementValueAccessor();

    public IFilterEvaluator<JsonElement> FilterEvaluator => 
        _evaluator ??= new FilterEvaluator<JsonElement>( this );

    public ElementTypeDescriptor()
    {
        Functions.Register( CountElementFunction.Name, context => new CountElementFunction( context ) );
        Functions.Register( LengthElementFunction.Name, context => new LengthElementFunction( context ) );
        Functions.Register( MatchElementFunction.Name, context => new MatchElementFunction( context ) );
        Functions.Register( SearchElementFunction.Name, context => new SearchElementFunction( context ) );
        Functions.Register( ValueElementFunction.Name, context => new ValueElementFunction( context ) );
    }

    public FilterFunction GetSelectFunction( ParseExpressionContext context )
    {
        return new SelectElementFunction( context );
    }

    public Expression GetValueExpression( Expression expression )
    {
        if ( expression is null )
            return null;

        return expression.Type == typeof( IEnumerable<JsonElement> )
            ? Expression.Invoke( ValueElementFunction.ValueExpression, expression )
            : expression;
    }
}
