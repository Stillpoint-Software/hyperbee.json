using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal static class CompareExpression<TNode>
{
    private static readonly IValueTypeComparer Comparer = JsonTypeDescriptorRegistry.GetDescriptor<TNode>().Comparer;

    // Expressions

    public static Expression Equal( Expression left, Expression right ) => Expression.Call( AreEqualMethod, left, right );
    public static Expression NotEqual( Expression left, Expression right ) => Expression.Call( AreNotEqualMethod, left, right );
    public static Expression LessThan( Expression left, Expression right ) => Expression.Call( IsLessThanMethod, left, right );
    public static Expression LessThanOrEqual( Expression left, Expression right ) => Expression.Call( IsLessThanOrEqualMethod, left, right );
    public static Expression GreaterThan( Expression left, Expression right ) => Expression.Call( IsGreaterThanMethod, left, right );
    public static Expression GreaterThanOrEqual( Expression left, Expression right ) => Expression.Call( IsGreaterThanOrEqualMethod, left, right );
    public static Expression And( Expression left, Expression right ) => Expression.Call( AndAlsoMethod, left, right );
    public static Expression Or( Expression left, Expression right ) => Expression.Call( OrElseMethod, left, right );
    public static Expression Not( Expression expression ) => Expression.Call( NotMethod, expression );

    // MethodInfo

    private const BindingFlags BindingAttr = BindingFlags.Static | BindingFlags.NonPublic;

    private static readonly MethodInfo AreEqualMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( AreEqual ), BindingAttr );
    private static readonly MethodInfo AreNotEqualMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( AreNotEqual ), BindingAttr );
    private static readonly MethodInfo IsLessThanMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( IsLessThan ), BindingAttr );
    private static readonly MethodInfo IsLessThanOrEqualMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( IsLessThanOrEqual ), BindingAttr );
    private static readonly MethodInfo IsGreaterThanMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( IsGreaterThan ), BindingAttr );
    private static readonly MethodInfo IsGreaterThanOrEqualMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( IsGreaterThanOrEqual ), BindingAttr );
    private static readonly MethodInfo AndAlsoMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( AndAlso ), BindingAttr );
    private static readonly MethodInfo OrElseMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( OrElse ), BindingAttr );
    private static readonly MethodInfo NotMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( NotBoolean ), BindingAttr );

    // Methods

    private static ScalarValue<bool> AreEqual( IValueType left, IValueType right )
    {
        return Comparer.Compare( left, right, Operator.Equals ) == 0;
    }

    private static ScalarValue<bool> AreNotEqual( IValueType left, IValueType right )
    {
        return Comparer.Compare( left, right, Operator.NotEquals ) != 0;
    }

    private static ScalarValue<bool> IsLessThan( IValueType left, IValueType right )
    {
        return Comparer.Compare( left, right, Operator.LessThan ) < 0;
    }

    private static ScalarValue<bool> IsLessThanOrEqual( IValueType left, IValueType right )
    {
        return Comparer.Compare( left, right, Operator.LessThanOrEqual ) <= 0;
    }

    private static ScalarValue<bool> IsGreaterThan( IValueType left, IValueType right )
    {
        return Comparer.Compare( left, right, Operator.GreaterThan ) > 0;
    }

    private static ScalarValue<bool> IsGreaterThanOrEqual( IValueType left, IValueType right )
    {
        return Comparer.Compare( left, right, Operator.GreaterThanOrEqual ) >= 0;
    }

    private static ScalarValue<bool> AndAlso( IValueType left, IValueType right )
    {
        if ( left is ScalarValue<bool> leftBoolValue && right is ScalarValue<bool> rightBoolValue )
            return leftBoolValue.Value && rightBoolValue.Value;

        return Comparer.Exists( left ) && Comparer.Exists( right );
    }

    private static ScalarValue<bool> OrElse( IValueType left, IValueType right )
    {
        if ( left is ScalarValue<bool> leftBoolValue && right is ScalarValue<bool> rightBoolValue )
            return leftBoolValue.Value || rightBoolValue.Value;

        return Comparer.Exists( left ) || Comparer.Exists( right );
    }

    private static ScalarValue<bool> NotBoolean( IValueType value )
    {
        if ( value is ScalarValue<bool> { Value: false } )
            return true;

        return !Comparer.Exists( value );
    }
}
