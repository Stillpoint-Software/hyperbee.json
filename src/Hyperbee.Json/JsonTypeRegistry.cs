using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Evaluators;
using Hyperbee.Json.Evaluators.Parser.Element;
using Hyperbee.Json.Evaluators.Parser.Node;

namespace Hyperbee.Json;

public class JsonTypeRegistry
{
    private static readonly Dictionary<Type, IJsonTypeDescriptor> Descriptors = [];

    static JsonTypeRegistry()
    {
        Register<JsonElement>( new JsonElementTypeDescriptor() );
        Register<JsonNode>( new JsonNodeTypeDescriptor() );
    }

    public static void Register<TElement>( IJsonTypeDescriptor descriptor )
    {
        Descriptors[typeof( TElement )] = descriptor;
    }

    public static IJsonTypeDescriptor GetDescriptor<TElement>()
    {
        if ( Descriptors.TryGetValue( typeof( TElement ), out var descriptor ) )
        {
            return descriptor;
        }

        throw new InvalidOperationException( $"No JSON descriptors registered for type {typeof( TElement )}." );
    }
}
