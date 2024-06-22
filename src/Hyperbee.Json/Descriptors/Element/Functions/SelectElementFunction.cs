using System.Linq.Expressions;
using System.Text.Json;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class SelectElementFunction : FilterFunction
{
    private static readonly Expression SelectExpression = Expression.Constant( (Func<JsonElement, JsonElement, string, IEnumerable<JsonElement>>) Select );

    public override Expression GetExpression( ReadOnlySpan<char> filter, ReadOnlySpan<char> item, ref int start, ref int from, FilterExecutionContext executionContext )
    {
        var queryExp = Expression.Constant( item.ToString() );

        if ( item[0] == '$' ) // Current becomes root
            executionContext = executionContext with { Current = executionContext.Root };

        return Expression.Invoke( SelectExpression, executionContext.Current, executionContext.Root, queryExp );
    }

    public static IEnumerable<JsonElement> Select( JsonElement current, JsonElement root, string query )
    {
        return JsonPath<JsonElement>.Select( current, root, query );
    }
}
