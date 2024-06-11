namespace Hyperbee.Json.Evaluators.Parser.Node;

public class JsonNodeTypeDescriptor : IJsonTypeDescriptor
{
    public Dictionary<string, FunctionCreator> Functions { get; }

    public IJsonValueAccessor<TElement> GetAccessor<TElement>() =>
        new JsonNodeValueAccessor() as IJsonValueAccessor<TElement>;

    public IJsonPathFilterEvaluator<TElement> GetFilterEvaluator<TElement>() =>
        new JsonPathFilterEvaluator<TElement>( this );

    public FilterFunction GetFilterFunction( ParseExpressionContext context ) =>
        new FilterNodeFunction( context );

    public JsonNodeTypeDescriptor()
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
