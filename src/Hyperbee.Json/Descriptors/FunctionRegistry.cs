using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors;

public sealed class FunctionRegistry
{
    private Dictionary<string, FunctionCreator> Functions { get; } = [];

    public void Register<TFunction>( string name, Func<TFunction> factory )
        where TFunction : FilterExtensionFunction
    {
        Functions[name] = () => factory(); 
    }

    internal bool TryGetCreator( string name, out FunctionCreator functionCreator )
    {
        return Functions.TryGetValue( name, out functionCreator );
    }
}
