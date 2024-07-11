using System.Linq.Expressions;
using System.Text.Json;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class LengthElementFunction() : FilterExtensionFunction( argumentCount: 1 )
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
            NodesType<JsonElement> nodes => Length( nodes.FirstOrDefault() ),
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
            JsonElement node => node.ValueKind switch
            {
                JsonValueKind.String => new ValueType<float>( node.GetString()?.Length ?? 0 ),
                JsonValueKind.Array => new ValueType<float>( node.EnumerateArray().Count() ),
                JsonValueKind.Object => new ValueType<float>( node.EnumerateObject().Count() ),
                _ => ValueType.Null
            },
            _ => ValueType.Null
        };
    }
}
