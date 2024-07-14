using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser;

public static class CompareExpression<TNode>
{
    private static readonly MethodInfo AreEqualMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( AreEqual ) );
    private static readonly MethodInfo AreNotEqualMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( AreNotEqual ) );
    private static readonly MethodInfo IsLessThanMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( IsLessThan ) );
    private static readonly MethodInfo IsLessThanOrEqualMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( IsLessThanOrEqual ) );
    private static readonly MethodInfo IsGreaterThanMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( IsGreaterThan ) );
    private static readonly MethodInfo IsGreaterThanOrEqualMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( IsGreaterThanOrEqual ) );

    private static readonly MethodInfo AndAlsoMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( AndAlso ) );
    private static readonly MethodInfo OrElseMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( OrElse ) );
    private static readonly MethodInfo NotMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( NotBoolean ) );

    public static Expression Equal( Expression left, Expression right ) => Expression.Call( AreEqualMethod, left, right );
    public static Expression NotEqual( Expression left, Expression right ) => Expression.Call( AreNotEqualMethod, left, right );
    public static Expression LessThan( Expression left, Expression right ) => Expression.Call( IsLessThanMethod, left, right );
    public static Expression LessThanOrEqual( Expression left, Expression right ) => Expression.Call( IsLessThanOrEqualMethod, left, right );
    public static Expression GreaterThan( Expression left, Expression right ) => Expression.Call( IsGreaterThanMethod, left, right );
    public static Expression GreaterThanOrEqual( Expression left, Expression right ) => Expression.Call( IsGreaterThanOrEqualMethod, left, right );

    // Binary operators
    public static Expression And( Expression left, Expression right ) => Expression.Call( AndAlsoMethod, left, right );
    public static Expression Or( Expression left, Expression right ) => Expression.Call( OrElseMethod, left, right );
    public static Expression Not( Expression expression ) => Expression.Call( NotMethod, expression );

    public static bool AreEqual( IValueType left, IValueType right ) => left.Comparer.Compare( left, right, Operator.Equals ) == 0;
    public static bool AreNotEqual( IValueType left, IValueType right ) => left.Comparer.Compare( left, right, Operator.NotEquals ) != 0;
    public static bool IsLessThan( IValueType left, IValueType right ) => left.Comparer.Compare( left, right, Operator.LessThan ) < 0;
    public static bool IsLessThanOrEqual( IValueType left, IValueType right ) => left.Comparer.Compare( left, right, Operator.LessThanOrEqual ) <= 0;
    public static bool IsGreaterThan( IValueType left, IValueType right ) => left.Comparer.Compare( left, right, Operator.GreaterThan ) > 0;
    public static bool IsGreaterThanOrEqual( IValueType left, IValueType right ) => left.Comparer.Compare( left, right, Operator.GreaterThanOrEqual ) >= 0;

    public static bool AndAlso( IValueType left, IValueType right )
    {
        if ( left is ScalarValue<bool> leftBoolValue && right is ScalarValue<bool> rightBoolValue )
            return leftBoolValue.Value && rightBoolValue.Value;

        return left.Comparer.Exists( left ) &&
               right.Comparer.Exists( right );
    }

    public static bool OrElse( IValueType left, IValueType right )
    {
        if ( left is ScalarValue<bool> leftBoolValue && right is ScalarValue<bool> rightBoolValue )
            return leftBoolValue.Value || rightBoolValue.Value;

        return left.Comparer.Exists( left ) ||
               right.Comparer.Exists( right );
    }

    public static bool NotBoolean( IValueType value )
    {
        if ( value is ScalarValue<bool> { Value: false } )
            return true;

        return !value.Comparer.Exists( value );
    }
}
