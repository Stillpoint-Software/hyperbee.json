using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Descriptors.Types;
using Hyperbee.Json.Filters.Parser;
using ValueType = Hyperbee.Json.Descriptors.Types.ValueType;

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
        return input switch
        {
            NodesType<JsonNode> nodes => Length( nodes.FirstOrDefault() ),
            ValueType<string> valueString => new ValueType<float>( valueString.Value.Length ),
            Null or Nothing => input,
            _ => ValueType.Nothing
        };
    }

    public static INodeType Length( object value )
    {
        return value switch
        {
            string str => new ValueType<float>( str.Length ),
            Array array => new ValueType<float>( array.Length ),
            System.Collections.ICollection collection => new ValueType<float>( collection.Count ),
            System.Collections.IEnumerable enumerable => new ValueType<float>( enumerable.Cast<object>().Count() ),
            JsonNode node => node.GetValueKind() switch
            {
                JsonValueKind.String => new ValueType<float>( node.GetValue<string>()?.Length ?? 0 ),
                JsonValueKind.Array => new ValueType<float>( node.AsArray().Count ),
                JsonValueKind.Object => new ValueType<float>( node.AsObject().Count ),
                _ => ValueType.Nothing
            },
            _ => ValueType.Nothing
        };
    }

}
