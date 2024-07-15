using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser.Expressions;

public static class FilterTruthyExpression
{
    private static readonly MethodInfo IsTruthyMethod = typeof( FilterTruthyExpression ).GetMethod( nameof( IsTruthy ), BindingFlags.NonPublic | BindingFlags.Static );

    public static Expression IsTruthyExpression( Expression expression ) =>
        expression.Type == typeof( bool )
            ? expression
            : Expression.Call( IsTruthyMethod, expression );

    private static bool IsTruthy( object value )
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
