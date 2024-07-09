using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class LengthNodeFunction() : FilterExtensionFunction( argumentCount: 1 )
{
    public const string Name = "length";
    private static readonly Expression LengthExpression = Expression.Constant( (Func<INodeType, INodeType>) Length );

    protected override Expression GetExtensionExpression( Expression[] arguments, bool[] argumentInfo )
    {
        if ( argumentInfo[0] )
            throw new NotSupportedException( $"Function {Name} does not support non-singular arguments." );

        return Expression.Invoke( LengthExpression,
            Expression.Convert( arguments[0], typeof( INodeType ) ) );
    }

    public static INodeType Length( INodeType input )
    {
        switch ( input.Kind )
        {
            case NodeTypeKind.NodeList:
                {
                    var list = (NodesType<JsonNode>) input;
                    return Length( list.FirstOrDefault() );
                }
            case NodeTypeKind.Value:
                {
                    var valueType = (ValueType<string>) input;
                    return new ValueType<float>( valueType.Value.Length );
                }
            case NodeTypeKind.Nothing:
                return input;
            case NodeTypeKind.Node:
            default:
                return new Nothing();
        }
    }

    public static ValueType<float> Length( object value )
    {
        float? result = value switch
        {
            string str => str.Length,
            Array array => array.Length,
            System.Collections.ICollection collection => collection.Count,
            System.Collections.IEnumerable enumerable => enumerable.Cast<object>().Count(),
            JsonNode node => node.GetValueKind() switch
            {
                JsonValueKind.String => node.GetValue<string>()?.Length ?? 0,
                JsonValueKind.Array => node.AsArray().Count,
                JsonValueKind.Object => node.AsObject().Count,
                _ => null
            },
            _ => null
        };

        return new ValueType<float>( result ?? 0F, result == null );
    }

}
