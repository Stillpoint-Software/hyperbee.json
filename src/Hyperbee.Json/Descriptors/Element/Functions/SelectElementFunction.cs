using System.Linq.Expressions;
using System.Text.Json;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class SelectElementFunction : FilterFunction
{
    private static readonly Expression SelectExpression = Expression.Constant( (Func<JsonElement, JsonElement, string, IEnumerable<JsonElement>>) Select );

    public override Expression GetExpression( ReadOnlySpan<char> filter, ReadOnlySpan<char> item, ref int start, ref int from, ParseExpressionContext context )
    {
        var queryExp = Expression.Constant( item.ToString() );

        if ( item[0] == '$' ) // Current becomes root
            context = context with { Current = context.Root };

        return Expression.Invoke( SelectExpression, context.Current, context.Root, queryExp );
    }

    public static IEnumerable<JsonElement> Select( JsonElement current, JsonElement root, string query )
    {
        return JsonPath<JsonElement>.Select( current, root, query );
    }
}
