using System.Linq.Expressions;
using System.Reflection;

namespace Hyperbee.Json.Filters.Parser;

public static class FilterTruthyExpression
{
    private static readonly MethodInfo IsTruthyMethod;

    static FilterTruthyExpression()
    {
        IsTruthyMethod = typeof( FilterTruthyExpression ).GetMethod( nameof( IsTruthy ), [typeof( object )] );
    }

    public static Expression IsTruthyExpression( Expression expression ) =>
        expression.Type == typeof( bool )
            ? expression
            : Expression.Call( IsTruthyMethod, expression );

    public static bool IsTruthy( object obj ) => !IsFalsy( obj );

    public static bool IsFalsy( object obj )
    {
        return obj switch
        {
            null => true,
            bool boolValue => !boolValue,
            string str => string.IsNullOrEmpty( str ) || str == "false",
            Array array => array.Length == 0,
            IConvertible convertible => !Convert.ToBoolean( convertible ),
            _ => false
        };
    }
}
