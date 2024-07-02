using System.Linq.Expressions;
using System.Text.Json;

using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class CountElementFunction() : FilterExtensionFunction( argumentCount: 1 )
{
    public const string Name = "count";
    private static readonly Expression CountExpression = Expression.Constant( (Func<IEnumerable<JsonElement>, float>) Count );

    protected override Expression GetExtensionExpression( Expression[] arguments )
    {
        return Expression.Invoke( CountExpression, arguments[0] );
    }

    public static float Count( IEnumerable<JsonElement> elements )
    {
        return elements.Count();
    }
}
