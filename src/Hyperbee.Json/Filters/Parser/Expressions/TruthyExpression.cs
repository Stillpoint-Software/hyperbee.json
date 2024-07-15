using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser.Expressions;

public static class TruthyExpression
{
    private static readonly MethodInfo IsTruthyMethod = typeof( TruthyExpression ).GetMethod( nameof( IsTruthy ), BindingFlags.NonPublic | BindingFlags.Static );

    public static Expression IsTruthyExpression( Expression expression ) =>
        expression.Type == typeof( bool )
            ? expression
            : Expression.Call( IsTruthyMethod, expression );

    private static bool IsTruthy( IValueType value )
    {
        var truthy = value switch
        {
            Nothing => false,
            Null => false,
            ScalarValue<bool> valueBool => valueBool.Value,
            ScalarValue<int> intValue => intValue.Value != 0,
            ScalarValue<float> floatValue => floatValue.Value != 0,
            ScalarValue<string> valueString => !string.IsNullOrEmpty( valueString.Value ) && !valueString.Value.Equals( "false", StringComparison.OrdinalIgnoreCase ),
            IEnumerable enumerable => Any( enumerable ), // NodeList<TNode>
            _ => true
        };

        return truthy;

        static bool Any( IEnumerable enumerable ) // Avoid cast to object: enumerable.Cast<object>().Any()
        {
            var enumerator = enumerable.GetEnumerator();
            using var disposable = enumerator as IDisposable;
            return enumerator.MoveNext();
        }
    }
}
