using System.Linq.Expressions;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class SelectNodeFunction( ParseExpressionContext context ) : FilterFunction
{
    private static readonly Expression SelectExpression = Expression.Constant( (Func<JsonNode, JsonNode, string, IEnumerable<JsonNode>>) Select );

    protected override Expression GetExpressionImpl( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        var queryExp = Expression.Constant( item.ToString() );

        return Expression.Invoke( SelectExpression, context.Current, context.Root, queryExp );
    }

    public static IEnumerable<JsonNode> Select( JsonNode current, JsonNode root, string query )
    {
        return JsonPath<JsonNode>.Select( current, root, query );
    }
}
