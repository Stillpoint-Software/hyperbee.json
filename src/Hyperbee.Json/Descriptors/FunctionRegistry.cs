using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors;

public sealed class FunctionRegistry
{
    private Dictionary<string, FunctionCreator> Functions { get; } = [];

    public void Register<TFunction>( string name, Func<ParseExpressionContext, TFunction> factory )
        where TFunction : FilterExtensionFunction
    {
        Functions[name] = context => factory( context );
    }

    internal bool TryGet( string name, out FunctionCreator functionCreator )
    {
        return Functions.TryGetValue( name, out functionCreator );
    }
}
