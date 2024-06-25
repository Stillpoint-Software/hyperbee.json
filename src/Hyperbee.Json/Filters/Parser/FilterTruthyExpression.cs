using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Hyperbee.Json.Filters.Parser
{
    public static class FilterTruthyExpression
    {
        private static readonly MethodInfo IsTruthyMethodInfo = typeof( FilterTruthyExpression ).GetMethod( nameof( IsTruthy ) );

        public static System.Linq.Expressions.Expression IsTruthyExpression( System.Linq.Expressions.Expression expression ) =>
            expression.Type == typeof( bool )
                ? expression
                : System.Linq.Expressions.Expression.Call( IsTruthyMethodInfo, expression );

        public static bool IsTruthy( object value )
        {
            return value switch
            {
                null => false,
                bool boolValue => boolValue,
                string str => !string.IsNullOrEmpty( str ) && !str.Equals( "false", StringComparison.OrdinalIgnoreCase ),
                Array array => array.Length > 0,
                IEnumerable enumerable => enumerable.Cast<object>().Any(),
                IConvertible convertible => Convert.ToBoolean( convertible ),
                _ => true
            };
        }
    }
}
