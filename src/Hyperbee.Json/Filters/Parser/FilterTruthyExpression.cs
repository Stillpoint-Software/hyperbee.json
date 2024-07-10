using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Descriptors;
using ValueType = Hyperbee.Json.Descriptors.ValueType;

namespace Hyperbee.Json.Filters.Parser;

public static class FilterTruthyExpression
{
    private static readonly MethodInfo IsTruthyMethodInfo = typeof( FilterTruthyExpression ).GetMethod( nameof( IsTruthy ) );
    private static readonly MethodInfo ConvertTruthyMethodInfo = typeof( FilterTruthyExpression ).GetMethod( nameof( ConvertTruthy ) );

    public static Expression IsTruthyExpression( Expression expression ) =>
        expression.Type == typeof( bool )
            ? expression
            : Expression.Call( IsTruthyMethodInfo, expression );

    public static Expression ConvertTruthyExpression( Expression expression ) =>
        Expression.Call( ConvertTruthyMethodInfo, expression );

    public static bool IsTruthy( object value )
    {
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

    public static INodeType ConvertTruthy( bool value )
    {
        return value ? ValueType.True : ValueType.False;
    }

}
