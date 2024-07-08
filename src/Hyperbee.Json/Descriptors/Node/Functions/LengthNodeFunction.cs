using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class LengthNodeFunction() : FilterExtensionFunction( argumentCount: 1 )
{
    public const string Name = "length";
    private static readonly Expression LengthExpression = Expression.Constant( (Func<IEnumerable<JsonNode>, float?>) Length );
    private static readonly Expression LengthObjectExpression = Expression.Constant( (Func<object, float?>) Length );

    protected override Expression GetExtensionExpression( Expression[] arguments, bool[] argumentInfo )
    {
        if ( argumentInfo[0] )
            throw new NotSupportedException( $"Function {Name} does not support non-singular arguments." );

        if ( arguments[0].Type == typeof( IEnumerable<JsonNode> ) )
            return Expression.Invoke( LengthExpression, arguments[0] );

        if ( arguments[0].Type == typeof( object ) )
            return Expression.Invoke( LengthObjectExpression, arguments[0] );

        if ( arguments[0].Type.IsAssignableTo( typeof( IConvertible ) ) )
            return Expression.Invoke( LengthObjectExpression, Expression.Convert( arguments[0], typeof( object ) ) );

        throw new NotSupportedException( $"Function {Name} does not support arguments with type {arguments[0].Type.Name}." );
    }

    public static float? Length( IEnumerable<JsonNode> nodes )
    {
        var jsonNodes = nodes as JsonNode[] ?? nodes.ToArray();

        return Length( jsonNodes.FirstOrDefault() );
    }

    public static float? Length( object value )
    {
        return value switch
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
    }


}
