using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class SelectElementFunction( ParseExpressionContext context ) : FilterFunction
{
    public static readonly MethodInfo SelectMethod;

    static SelectElementFunction()
    {
        SelectMethod = typeof( SelectElementFunction ).GetMethod( nameof( Select ), [typeof( JsonElement ), typeof( JsonElement ), typeof( string )] );
    }

    protected override Expression GetExpressionImpl( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        var queryExp = Expression.Constant( item.ToString() );

        return Expression.Call( SelectMethod, context.Current, context.Root, queryExp );
    }

    public static IEnumerable<JsonElement> Select( JsonElement current, JsonElement root, string query )
    {
        return JsonPath<JsonElement>
            .Select( current, root, query );
    }
}
