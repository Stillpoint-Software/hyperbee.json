using Hyperbee.Json.Path.Filters.Parser;

namespace Hyperbee.Json.Descriptors;

public delegate ExtensionFunction FunctionActivator();

public sealed class FunctionRegistry
{
    private Dictionary<string, FunctionActivator> Functions { get; } = [];

    public void Register<TFunction>( string name, Func<TFunction> factory )
        where TFunction : ExtensionFunction
    {
        Functions[name] = () => factory();
    }

    internal bool TryGetActivator( string name, out FunctionActivator functionActivator )
    {
        return Functions.TryGetValue( name, out functionActivator );
    }
}
