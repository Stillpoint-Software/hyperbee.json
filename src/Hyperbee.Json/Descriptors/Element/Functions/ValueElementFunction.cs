using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class ValueElementFunction( string methodName, ParseExpressionContext context )
    : FilterExtensionFunction( methodName, 1, context )
{
    public const string Name = "value";

    public static readonly MethodInfo ValueMethod;

    static ValueElementFunction()
    {
        ValueMethod = typeof( ValueElementFunction ).GetMethod( nameof( Value ), [typeof( IEnumerable<JsonElement> )] );
    }

    public override Expression GetExtensionExpression( string methodName, Expression[] arguments, ParseExpressionContext context )
    {
        if ( arguments.Length != 1 )
        {
            return Expression.Throw( Expression.Constant( new ArgumentException( $"{Name} function has invalid parameter count." ) ) );
        }

        return Expression.Call( ValueMethod, arguments[0] );
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
