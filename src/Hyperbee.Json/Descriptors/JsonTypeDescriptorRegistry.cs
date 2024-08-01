using Hyperbee.Json.Descriptors.Element;
using Hyperbee.Json.Descriptors.Node;

namespace Hyperbee.Json.Descriptors;

public class JsonTypeDescriptorRegistry
{
    private static readonly Dictionary<Type, ITypeDescriptor> Descriptors = [];

    static JsonTypeDescriptorRegistry()
    {
        Register( new ElementTypeDescriptor() );
        Register( new NodeTypeDescriptor() );
    }

    public static void Register<TNode>( ITypeDescriptor<TNode> descriptor )
    {
        Descriptors[typeof( TNode )] = descriptor;
    }

    public static ITypeDescriptor<TNode> GetDescriptor<TNode>()
    {
        if ( Descriptors.TryGetValue( typeof( TNode ), out var descriptor ) )
        {
            return descriptor as ITypeDescriptor<TNode>;
        }

        throw new InvalidOperationException( $"No JSON descriptors registered for type {typeof( TNode )}." );
    }
}
