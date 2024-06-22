using System.Linq.Expressions;
using System.Text.Json;

using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class ValueElementFunction() : FilterExtensionFunction( argumentCount: 1 )
{
    public static readonly Expression ValueExpression = Expression.Constant( (Func<IEnumerable<JsonElement>, object>) Value );

    public const string Name = "value";

    public override Expression GetExtensionExpression( Expression[] arguments )
    {
        return Expression.Invoke( ValueExpression, arguments[0] );
    }

    public static object Value( IEnumerable<JsonElement> elements )
    {
        var element = elements.FirstOrDefault();

        return element.ValueKind switch
        {
            JsonValueKind.Number => element.GetSingle(),
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Object => IsNotEmpty( element ),
            JsonValueKind.Array => IsNotEmpty( element ),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => false,
            JsonValueKind.Undefined => false,
            _ => false
        };
    }

    private static bool IsNotEmpty( JsonElement element )
    {
        return element.ValueKind switch
        {
            JsonValueKind.Array => element.EnumerateArray().Any(),
            JsonValueKind.Object => element.EnumerateObject().Any(),
            _ => false
        };
    }
}
