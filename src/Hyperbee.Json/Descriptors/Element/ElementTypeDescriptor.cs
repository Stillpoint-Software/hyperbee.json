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
        if ( expression is null ) return null;

        return expression.Type == typeof( IEnumerable<JsonElement> )
            ? Expression.Call( ValueElementFunction.ValueMethod, expression )
            : expression;
    }


    public ElementTypeDescriptor()
    {
        Functions = new Dictionary<string, FunctionCreator>(
        [
            new KeyValuePair<string, FunctionCreator>( CountElementFunction.Name, ( name, context ) => new CountElementFunction( name, context ) ),
            new KeyValuePair<string, FunctionCreator>( LengthElementFunction.Name, ( name, context ) => new LengthElementFunction( name, context ) ),
            new KeyValuePair<string, FunctionCreator>( MatchElementFunction.Name, ( name, context ) => new MatchElementFunction( name, context ) ),
            new KeyValuePair<string, FunctionCreator>( SearchElementFunction.Name, ( name, context ) => new SearchElementFunction( name, context ) ),
            new KeyValuePair<string, FunctionCreator>( ValueElementFunction.Name, ( name, context ) => new ValueElementFunction( name, context ) ),
        ] );
    }
}
