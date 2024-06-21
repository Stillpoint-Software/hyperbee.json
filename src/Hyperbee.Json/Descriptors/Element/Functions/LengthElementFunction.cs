using System.Linq.Expressions;
using System.Text.Json;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class LengthElementFunction( ParseExpressionContext context ) : FilterExtensionFunction( argumentCount: 1, context )
{
    public const string Name = "length";
    private static readonly Expression LengthExpression = Expression.Constant( (Func<IEnumerable<JsonElement>, float>) Length );

    public override Expression GetExtensionExpression( Expression[] arguments, ParseExpressionContext context )
    {
        return Expression.Invoke( LengthExpression, arguments[0] );
    }

    public static float Length( IEnumerable<JsonElement> elements )
    {
        var element = elements.FirstOrDefault();
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString()?.Length ?? 0,
            JsonValueKind.Array => element.GetArrayLength(),
            JsonValueKind.Object => element.EnumerateObject().Count(),
            _ => 0
        };
    }
}
