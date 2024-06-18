using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class SelectNodeFunction( ParseExpressionContext context ) : FilterFunction
{
    public static readonly MethodInfo SelectMethod;

    static SelectNodeFunction()
    {
        SelectMethod = typeof( SelectNodeFunction ).GetMethod( nameof( Select ), [typeof( JsonNode ), typeof( JsonNode ), typeof( string )] );
    }

    protected override Expression GetExpressionImpl( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        var queryExp = Expression.Constant( item.ToString() );

        return Expression.Call( SelectMethod, context.Current, context.Root, queryExp );
    }

    public static IEnumerable<JsonNode> Select( JsonNode current, JsonNode root, string query )
    {
        return JsonPath<JsonNode>
            .Select( current, root, query );
    }
}
