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
    public Dictionary<string, FunctionCreator> Functions { get; init; }

    public IValueAccessor<JsonElement> Accessor
    {
        get => _accessor ??= new ElementValueAccessor();
    }

    public IFilterEvaluator<JsonElement> FilterEvaluator
    {
        get => _evaluator ??= new FilterEvaluator<JsonElement>( this );
    }

    public FilterFunction GetSelectFunction( ParseExpressionContext context ) =>
        new SelectElementFunction( context );

    public Expression GetValueExpression( Expression expression )
    {
        if ( expression is null ) 
            return null;

        return expression.Type == typeof( IEnumerable<JsonElement> )
            ? Expression.Invoke( ValueElementFunction.ValueExpression, expression )
            : expression;
    }


    public ElementTypeDescriptor()
    {
        Functions = new Dictionary<string, FunctionCreator>(
        [
            new KeyValuePair<string, FunctionCreator>( CountElementFunction.Name, context => new CountElementFunction( context ) ),
            new KeyValuePair<string, FunctionCreator>( LengthElementFunction.Name, context => new LengthElementFunction( context ) ),
            new KeyValuePair<string, FunctionCreator>( MatchElementFunction.Name, context => new MatchElementFunction( context ) ),
            new KeyValuePair<string, FunctionCreator>( SearchElementFunction.Name, context => new SearchElementFunction( context ) ),
            new KeyValuePair<string, FunctionCreator>( ValueElementFunction.Name, context => new ValueElementFunction( context ) ),
        ] );
    }
}
