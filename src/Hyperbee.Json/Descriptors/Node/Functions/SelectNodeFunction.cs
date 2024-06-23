using System.Linq.Expressions;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class SelectNodeFunction : FilterFunction
{
    private static readonly Expression SelectExpression = Expression.Constant( (Func<JsonNode, JsonNode, string, IEnumerable<JsonNode>>) Select );

    public override Expression GetExpression( ReadOnlySpan<char> filter, ReadOnlySpan<char> item, ref int pos, FilterExecutionContext executionContext )
    {
        var queryExp = Expression.Constant( item.ToString() );

        if ( item[0] == '$' ) // Current becomes root
            executionContext = executionContext with { Current = executionContext.Root };

        return Expression.Invoke( SelectExpression, executionContext.Current, executionContext.Root, queryExp );
    }

    public static IEnumerable<JsonNode> Select( JsonNode current, JsonNode root, string query )
    {
        return JsonPath<JsonNode>.Select( current, root, query );
    }
}
