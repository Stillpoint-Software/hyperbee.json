using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Descriptors.Element;
using Hyperbee.Json.Descriptors.Node;

namespace Hyperbee.Json;

public class JsonTypeRegistry
{
    private static readonly Dictionary<Type, IJsonTypeDescriptor> Descriptors = [];

    static JsonTypeRegistry()
    {
        Register( new ElementTypeDescriptor() );
        Register( new NodeTypeDescriptor() );
    }

    public static void Register<TElement>( ITypeDescriptor<TElement> descriptor )
    {
        Descriptors[typeof( TElement )] = descriptor;
    }

    public static ITypeDescriptor<TElement> GetDescriptor<TElement>()
    {
        if ( Descriptors.TryGetValue( typeof( TElement ), out var descriptor ) )
        {
            return descriptor as ITypeDescriptor<TElement>;
        }

        throw new InvalidOperationException( $"No JSON descriptors registered for type {typeof( TElement )}." );
    }
}
