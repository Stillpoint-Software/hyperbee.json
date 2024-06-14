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

    public FilterFunction GetFilterFunction( ParseExpressionContext context ) =>
        new FilterElementFunction( context );

    public ElementTypeDescriptor()
    {
        Functions = new Dictionary<string, FunctionCreator>(
        [
            new KeyValuePair<string, FunctionCreator>( CountElementFunction.Name, ( name, arguments, context ) => new CountElementFunction( name, arguments, context ) ),
            new KeyValuePair<string, FunctionCreator>( LengthElementFunction.Name, ( name, arguments, context ) => new LengthElementFunction( name, arguments, context ) ),
            new KeyValuePair<string, FunctionCreator>( MatchElementFunction.Name, ( name, arguments, context ) => new MatchElementFunction( name, arguments, context ) ),
            new KeyValuePair<string, FunctionCreator>( SearchElementFunction.Name, ( name, arguments, context ) => new SearchElementFunction( name, arguments, context ) ),
            new KeyValuePair<string, FunctionCreator>( ValueElementFunction.Name, ( name, arguments, context ) => new ValueElementFunction( name, arguments, context ) ),
        ] );
    }
}
