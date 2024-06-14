using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class LengthElementFunction( string methodName, IList<string> arguments, ParseExpressionContext context ) : FilterExtensionFunction( methodName, arguments, context )
{
    public const string Name = "length";

    private static readonly MethodInfo LengthMethod;

    static LengthElementFunction()
    {
        LengthMethod = typeof( LengthElementFunction ).GetMethod( nameof( Length ), [typeof( JsonElement )] );
    }

    public override Expression GetExtensionExpression( string methodName, IList<string> arguments, ParseExpressionContext context )
    {
        if ( arguments.Count != 1 )
        {
            return Expression.Throw( Expression.Constant( new ArgumentException( $"{Name} function has invalid parameter count." ) ) );
        }

        var queryExp = Expression.Constant( arguments[0] );

        return Expression.Call(
            LengthMethod,
            Expression.Call( FilterElementHelper.SelectFirstMethod,
                context.Current,
                context.Root,
                queryExp ) );
    }

    public static float Length( JsonElement element )
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString()?.Length ?? 0,
            JsonValueKind.Array => element.GetArrayLength(),
            JsonValueKind.Object => element.EnumerateObject().Count(),
            _ => 0
        };
    }
}
