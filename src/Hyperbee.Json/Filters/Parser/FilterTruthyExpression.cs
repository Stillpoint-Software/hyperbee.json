using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser;

public static class FilterTruthyExpression
{
    private static readonly MethodInfo IsTruthyMethodInfo = typeof( FilterTruthyExpression ).GetMethod( nameof( IsTruthy ) );

    public static Expression IsTruthyExpression( Expression expression ) =>
        expression.Type == typeof( bool )
            ? expression
            : Expression.Call( IsTruthyMethodInfo, expression );

    public static bool IsTruthy( object value )
    {
        var truthy = value switch
        {
            Nothing => false,
            Null => false,
            ScalarValue<bool> valueBool => valueBool.Value,
            ScalarValue<int> intValue => intValue.Value != 0,
            ScalarValue<float> floatValue => floatValue.Value != 0,
            ScalarValue<string> valueString => !string.IsNullOrEmpty( valueString.Value ) && !valueString.Value.Equals( "false", StringComparison.OrdinalIgnoreCase ),
            IEnumerable enumerable => enumerable.Cast<object>().Any(),  // NodeList<TNode>
            _ => true
        };

        return truthy;
    }
}
