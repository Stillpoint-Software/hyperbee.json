using System.Linq.Expressions;
using System.Text.Json;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class SelectElementFunction( ParseExpressionContext context ) : FilterFunction
{
    private static readonly Expression SelectExpression = Expression.Constant( (Func<JsonElement, JsonElement, string, IEnumerable<JsonElement>>) Select );

    protected override Expression GetExpressionImpl( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        var queryExp = Expression.Constant( item.ToString() );

        return Expression.Invoke( SelectExpression, context.Current, context.Root, queryExp );
    }

    public static IEnumerable<JsonElement> Select( JsonElement current, JsonElement root, string query )
    {
        return JsonPath<JsonElement>.Select( current, root, query );
    }
}
