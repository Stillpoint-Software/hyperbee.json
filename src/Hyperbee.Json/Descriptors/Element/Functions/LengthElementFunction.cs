using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class LengthElementFunction( string methodName, ParseExpressionContext context ) 
    : FilterExtensionFunction( methodName, 1, context )
{
    public const string Name = "length";

    private static readonly MethodInfo LengthMethod;

    static LengthElementFunction()
    {
        LengthMethod = typeof( LengthElementFunction ).GetMethod( nameof( Length ), [typeof( IEnumerable<JsonElement> )] );
    }

    public override Expression GetExtensionExpression( string methodName, Expression[] arguments, ParseExpressionContext context )
    {
        if ( arguments.Length != 1 )
        {
            return Expression.Throw( Expression.Constant( new ArgumentException( $"{Name} function has invalid parameter count." ) ) );
        }

        return Expression.Call( LengthMethod, arguments[0] );
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
