namespace Hyperbee.Json.Evaluators.Parser.Element;

public class JsonElementTypeDescriptor : IJsonTypeDescriptor
{
    public Dictionary<string, FunctionCreator> Functions { get; init; }

    public IJsonValueAccessor<TElement> GetAccessor<TElement>() =>
        new JsonElementValueAccessor() as IJsonValueAccessor<TElement>;

    public IJsonPathFilterEvaluator<TElement> GetFilterEvaluator<TElement>() =>
        new JsonPathFilterEvaluator<TElement>( this );

    public FilterFunction GetFilterFunction( ParseExpressionContext context ) =>
        new FilterElementFunction( context );

    public JsonElementTypeDescriptor()
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
