using Hyperbee.Json.Path.Filters.Parser;

namespace Hyperbee.Json.Descriptors;

public delegate ExtensionFunction FunctionActivator();

public sealed class FunctionRegistry
{
    // Use StringComparer.Ordinal explicitly: it is required for the .NET 9+
    // ReadOnlySpan<char> alternate lookup, which lets the parser dispatch on
    // function names without allocating a string from state.Item.
    private Dictionary<string, FunctionActivator> Functions { get; } = new( StringComparer.Ordinal );

    public void Register<TFunction>( string name, Func<TFunction> factory )
        where TFunction : ExtensionFunction
    {
        Functions[name] = () => factory();
    }

    internal bool TryGetActivator( string name, out FunctionActivator functionActivator )
    {
        return Functions.TryGetValue( name, out functionActivator );
    }

    internal bool TryGetActivator( ReadOnlySpan<char> name, out FunctionActivator functionActivator )
    {
#if NET9_0_OR_GREATER
        var lookup = Functions.GetAlternateLookup<ReadOnlySpan<char>>();
        return lookup.TryGetValue( name, out functionActivator );
#else
        return Functions.TryGetValue( name.ToString(), out functionActivator );
#endif
    }
}
