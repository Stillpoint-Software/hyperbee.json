using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class SearchNodeFunction() : FilterExtensionFunction( argumentCount: 2 )
{
    public const string Name = "search";
    private static readonly Expression SearchExpression = Expression.Constant( (Func<INodeType, INodeType, bool>) Search );

    protected override Expression GetExtensionExpression( Expression[] arguments, bool[] argumentInfo )
    {
        return Expression.Invoke( SearchExpression,
                Expression.Convert( arguments[0], typeof( INodeType ) ),
                Expression.Convert( arguments[1], typeof( INodeType ) ) );
    }

    public static bool Search( INodeType input, INodeType regex )
    {
        if ( input.Kind != NodeTypeKind.NodeList )
            throw new NotSupportedException( $"Function {Name} does not support kind {input.Kind}" );

        if ( regex.Kind != NodeTypeKind.Value )
            throw new NotSupportedException( $"Function {Name} does not support kind {regex.Kind}" );


        return Search( (NodesType<JsonNode>) input, ((ValueType<string>) regex).Value );
    }

    public static bool Search( NodesType<JsonNode> nodes, string regex )
    {
        var value = nodes.FirstOrDefault();

        if( value?.GetValueKind() != JsonValueKind.String )
        {
            return false;
        }

        var regexPattern = new Regex( $"{regex.Trim( '\"', '\'' )}" );
        return regexPattern.IsMatch( value.GetValue<string>() );
    }
}
