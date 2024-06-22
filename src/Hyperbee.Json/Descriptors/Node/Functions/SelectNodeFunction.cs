using System.Linq.Expressions;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class SelectNodeFunction() : FilterFunction
{
    private static readonly Expression SelectExpression = Expression.Constant( (Func<JsonNode, JsonNode, string, IEnumerable<JsonNode>>) Select );

    public override Expression GetExpression( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from, ParseExpressionContext context )
    {
        var queryExp = Expression.Constant( item.ToString() );

        if ( item[0] == '$' ) // Current becomes root
            context = context with { Current = context.Root };

        return Expression.Invoke( SelectExpression, context.Current, context.Root, queryExp );
    }

    public static IEnumerable<JsonNode> Select( JsonNode current, JsonNode root, string query )
    {
        return JsonPath<JsonNode>.Select( current, root, query );
    }
}
