using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser;

public static class FilterTruthyExpression
{
    private static readonly MethodInfo IsTruthyMethodInfo = typeof( FilterTruthyExpression ).GetMethod( nameof( IsTruthy ) );
    private static readonly MethodInfo ConvertBoolToValueTypeMethodInfo = typeof( FilterTruthyExpression ).GetMethod( nameof( ConvertBoolToValueType ) );

    public static Expression IsTruthyExpression( Expression expression ) =>
        expression.Type == typeof( bool )
            ? expression
            : Expression.Call( IsTruthyMethodInfo, expression );

    public static Expression ConvertBoolToValueTypeExpression( Expression expression ) =>
        Expression.Call( ConvertBoolToValueTypeMethodInfo, expression );

    public static bool IsTruthy( object value )
    {
        // TODO: remove unused checks
        var truthy = value switch
        {
            null => false,
            bool boolValue => boolValue,
            string str => !string.IsNullOrEmpty( str ) && !str.Equals( "false", StringComparison.OrdinalIgnoreCase ),
            Array array => array.Length > 0,
            IEnumerable enumerable => enumerable.Cast<object>().Any(),
            IConvertible convertible => Convert.ToBoolean( convertible ),
            Nothing => false,
            Null => false,
            ValueType<bool> valueBool => valueBool.Value,
            ValueType<float> floatValue => floatValue.Value != 0,
            ValueType<string> valueString => !string.IsNullOrEmpty( valueString.Value ) && !valueString.Value.Equals( "false", StringComparison.OrdinalIgnoreCase ),
            _ => true
        };

        return truthy;
    }

    public static INodeType ConvertBoolToValueType( bool value )
    {
        return value ? Constants.True : Constants.False;
    }

}
