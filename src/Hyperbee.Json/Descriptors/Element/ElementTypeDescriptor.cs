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
        Functions.Register( CountElementFunction.Name, () => new CountElementFunction() );
        Functions.Register( LengthElementFunction.Name, () => new LengthElementFunction() );
        Functions.Register( MatchElementFunction.Name, () => new MatchElementFunction() );
        Functions.Register( SearchElementFunction.Name, () => new SearchElementFunction() );
        Functions.Register( ValueElementFunction.Name, () => new ValueElementFunction() );
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
